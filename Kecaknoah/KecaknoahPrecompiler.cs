using System;
using System.Collections.Generic;
using System.Linq;
using Kecaknoah.Analyze;
using Kecaknoah.Type;

namespace Kecaknoah
{
    /// <summary>
    /// KecaknoahAstからKecaknoahILにプリコンパイルする機能を提供します。
    /// </summary>
    public sealed class KecaknoahPrecompiler
    {
        /// <summary>
        /// コンパイル時に式の定数畳み込みを行うがどうかを取得・設定します。
        /// </summary>
        public bool AllowConstantFolding { get; set; }

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        public KecaknoahPrecompiler()
        {

        }

        /// <summary>
        /// 1つのソースコード全体からなる<see cref="KecaknoahAst"/>をプリコンパイルします。
        /// </summary>
        /// <param name="ast">対象の<see cref="KecaknoahAst"/></param>
        /// <returns>プリコンパイル結果</returns>
        public KecaknoahSource PrecompileAll(KecaknoahAst ast)
        {
            var result = new KecaknoahSource();
            foreach (var i in ast.RootNode.Children)
            {
                if (i is KecaknoahClassAstNode)
                {
                    result.classes.Add(PrecompileClass(i as KecaknoahClassAstNode));
                }
                else if (i is KecaknoahFunctionAstNode)
                {
                    result.methods.Add(PrecompileFunction(i as KecaknoahFunctionAstNode));
                }
                else
                {
                    throw new InvalidOperationException("トップレベルにはクラスとメソッド以外おけません");
                }
            }
            return result;
        }

        private KecaknoahScriptClassInfo PrecompileClass(KecaknoahClassAstNode ast)
        {
            //TODO: local初期値式対応
            var result = new KecaknoahScriptClassInfo(ast.Name);
            foreach (var i in ast.Functions)
            {
                if (i.StaticMethod)
                {
                    result.AddClassMethod(PrecompileFunction(i));
                }
                else
                {
                    result.AddInstanceMethod(PrecompileFunction(i));
                }
            }
            foreach (var i in ast.Locals)
            {
                result.AddLocal(i.Name);
            }
            return result;
        }

        private KecaknoahScriptMethodInfo PrecompileFunction(KecaknoahFunctionAstNode ast)
        {
            var al = ast.Parameters;
            var result = new KecaknoahScriptMethodInfo(ast.Name, ast.Parameters.Count, ast.AllowsVariableArguments);
            result.Codes = new KecaknoahIL();
            var b = PrecompileBlock(ast.Children, "").ToList();
            foreach (var i in b.Where(p => p.Type == KecaknoahILCodeType.Jump || p.Type == KecaknoahILCodeType.FalseJump || p.Type == KecaknoahILCodeType.TrueJump))
            {
                i.IntegerValue = b.FindIndex(p => p.Type == KecaknoahILCodeType.Label && p.StringValue == i.StringValue);
            }
            foreach (var i in b.Where(p => p.Type == KecaknoahILCodeType.Label)) i.Type = KecaknoahILCodeType.Nop;
            result.Codes.PushCodes(b);
            foreach (var i in result.Codes.Codes)
            {
                if (i.Type == KecaknoahILCodeType.LoadObject && al.Contains(i.StringValue))
                {
                    i.Type = KecaknoahILCodeType.PushArgument;
                    i.IntegerValue = al.IndexOf(i.StringValue);
                }
            }
            return result;
        }

        /// <summary>
        /// 式からなる<see cref="KecaknoahAst"/>をプリコンパイルします。
        /// </summary>
        /// <param name="ast">対象の<see cref="KecaknoahAst"/></param>
        /// <returns>プリコンパイル結果</returns>
        public KecaknoahIL PrecompileExpression(KecaknoahAst ast)
        {
            var result = new KecaknoahIL();
            result.PushCodes(PrecompileExpression(ast.RootNode));
            return result;
        }

        internal IReadOnlyList<KecaknoahILCode> PrecompileBlock(IReadOnlyList<KecaknoahAstNode> ast, string loopId)
        {
            var result = new KecaknoahIL();
            List<string> locals = new List<string>();
            foreach (var i in ast)
            {
                if (i is KecaknoahExpressionAstNode)
                {
                    if (i is KecaknoahArgumentCallExpressionAstNode)
                    {
                        var exp = i as KecaknoahArgumentCallExpressionAstNode;
                        if (exp.ExpressionType != KecaknoahOperatorType.FunctionCall) throw new InvalidOperationException("ステートメントにできる式は代入・呼び出し・インクリメント・デクリメントのみです");
                        result.PushCodes(PrecompileFunctionCall(exp));
                        result.PushCode(KecaknoahILCodeType.Pop);
                    }
                    else if (i is KecaknoahBinaryExpressionAstNode)
                    {
                        var exp = i as KecaknoahBinaryExpressionAstNode;
                        switch (exp.ExpressionType)
                        {
                            case KecaknoahOperatorType.Assign:
                            case KecaknoahOperatorType.PlusAssign:
                            case KecaknoahOperatorType.MinusAssign:
                            case KecaknoahOperatorType.MultiplyAssign:
                            case KecaknoahOperatorType.DivideAssign:
                            case KecaknoahOperatorType.AndAssign:
                            case KecaknoahOperatorType.OrAssign:
                            case KecaknoahOperatorType.XorAssign:
                            case KecaknoahOperatorType.ModularAssign:
                            case KecaknoahOperatorType.LeftBitShiftAssign:
                            case KecaknoahOperatorType.RightBitShiftAssign:
                            case KecaknoahOperatorType.NilAssign:
                                result.PushCodes(PrecompileBinaryExpression(exp));
                                result.PushCode(KecaknoahILCodeType.Pop);
                                break;
                            default:
                                throw new InvalidOperationException("ステートメントにできる式は代入・呼び出し・インクリメント・デクリメントのみです");
                        }

                    }
                    else if (i is KecaknoahPrimaryExpressionAstNode)
                    {
                        var exp = i as KecaknoahPrimaryExpressionAstNode;
                        switch (exp.ExpressionType)
                        {
                            case KecaknoahOperatorType.Increment:
                                result.PushCodes(PrecompileSuffixIncrement(exp));
                                result.PushCode(KecaknoahILCodeType.Pop);
                                break;
                            case KecaknoahOperatorType.Decrement:
                                result.PushCodes(PrecompileSuffixDecrement(exp));
                                result.PushCode(KecaknoahILCodeType.Pop);
                                break;
                            default:
                                throw new InvalidOperationException("ステートメントにできる式は代入・呼び出し・インクリメント・デクリメントのみです");
                        }
                    }
                    else if (i is KecaknoahUnaryExpressionAstNode)
                    {
                        var exp = i as KecaknoahUnaryExpressionAstNode;
                        switch (exp.ExpressionType)
                        {
                            case KecaknoahOperatorType.Increment:
                                result.PushCodes(PrecompilePrefixIncrement(exp));
                                result.PushCode(KecaknoahILCodeType.Pop);
                                break;
                            case KecaknoahOperatorType.Decrement:
                                result.PushCodes(PrecompilePrefixDecrement(exp));
                                result.PushCode(KecaknoahILCodeType.Pop);
                                break;
                            default:
                                throw new InvalidOperationException("ステートメントにできる式は代入・呼び出し・インクリメント・デクリメントのみです");
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("ステートメントにできる式は代入・呼び出し・インクリメント・デクリメントのみです");
                    }
                }
                else if (i is KecaknoahLocalAstNode)
                {
                    var lc = i as KecaknoahLocalAstNode;
                    locals.Add(lc.Name);
                    if (lc.InitialExpression != null)
                    {
                        result.PushCode(KecaknoahILCodeType.LoadObject, lc.Name);
                        result.PushCodes(PrecompileExpression(lc.InitialExpression));
                        result.PushCode(KecaknoahILCodeType.Assign);
                    }
                }
                else if (i is KecaknoahReturnAstNode)
                {
                    var rc = i as KecaknoahReturnAstNode;
                    if (rc.Value != null)
                    {
                        result.PushCodes(PrecompileExpression(rc.Value));
                    }
                    else
                    {
                        result.PushCode(KecaknoahILCodeType.PushNil);
                    }
                    result.PushCode(rc.Type == KecaknoahAstNodeType.ReturnStatement ? KecaknoahILCodeType.Return : KecaknoahILCodeType.Yield);
                }
                else if (i is KecaknoahCoroutineDeclareAstNode)
                {
                    var cd = i as KecaknoahCoroutineDeclareAstNode;
                    result.PushCodes(PrecompileExpression(cd.InitialExpression));
                    result.PushCode(KecaknoahILCodeType.StartCoroutine, cd.Name);
                }
                else if (i is KecaknoahContinueAstNode)
                {
                    var ca = i as KecaknoahContinueAstNode;
                    result.PushCode(KecaknoahILCodeType.Jump, $"{loopId}-" + (i.Type == KecaknoahAstNodeType.ContinueStatement ? "Continue" : "End"));
                }
                else if (i is KecaknoahIfAstNode)
                {
                    result.PushCodes(PrecompileIf(i as KecaknoahIfAstNode, loopId));
                }
                else if (i is KecaknoahForAstNode)
                {
                    result.PushCodes(PrecompileFor(i as KecaknoahForAstNode));
                }
                else if (i is KecaknoahLoopAstNode)
                {
                    result.PushCodes(PrecompileWhile(i as KecaknoahLoopAstNode));
                }
            }
            return result.Codes;
        }

        private IList<KecaknoahILCode> PrecompileIf(KecaknoahIfAstNode ifn, string loopId)
        {
            //まあまずかぶらないでしょう
            var id = Guid.NewGuid().ToString().Substring(0, 8);
            var result = new List<KecaknoahILCode>();
            result.AddRange(PrecompileExpression(ifn.IfBlock.Condition));
            result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.FalseJump, StringValue = $"{id}-IfEnd" });
            result.AddRange(PrecompileBlock(ifn.IfBlock.Children, loopId));
            result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.Jump, StringValue = $"{id}-End" });
            result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.Label, StringValue = $"{id}-IfEnd" });
            var c = 0;
            foreach (var i in ifn.ElifBlocks)
            {
                result.AddRange(PrecompileExpression(i.Condition));
                result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.FalseJump, StringValue = $"{id}-Elif{c}End" });
                result.AddRange(PrecompileBlock(i.Children, loopId));
                result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.Jump, StringValue = $"{id}-End" });
                result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.Label, StringValue = $"{id}-Elif{c}End" });
                c++;
            }
            if (ifn.ElseBlock != null)
            {
                result.AddRange(PrecompileBlock(ifn.ElseBlock.Children, loopId));
            }
            result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.Label, StringValue = $"{id}-End" });
            return result;
        }

        private IList<KecaknoahILCode> PrecompileFor(KecaknoahForAstNode fn)
        {
            var id = Guid.NewGuid().ToString().Substring(0, 8);
            var result = new List<KecaknoahILCode>();
            foreach (var i in fn.InitializeExpressions)
            {
                result.AddRange(PrecompileExpression(i));
                result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.Pop });
            }
            result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.Label, StringValue = $"{id}-Next" });
            result.AddRange(PrecompileExpression(fn.Condition));
            result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.FalseJump, StringValue = $"{id}-End" });
            result.AddRange(PrecompileBlock(fn.Children, id));

            result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.Label, StringValue = $"{id}-Continue" });
            foreach (var i in fn.CounterExpressions)
            {
                result.AddRange(PrecompileExpression(i));
                result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.Pop });
            }
            result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.Jump, StringValue = $"{id}-Next" });
            result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.Label, StringValue = $"{id}-End" });
            return result;
        }

        private IList<KecaknoahILCode> PrecompileWhile(KecaknoahLoopAstNode fn)
        {
            var id = Guid.NewGuid().ToString().Substring(0, 8);
            var result = new List<KecaknoahILCode>();
            result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.Label, StringValue = $"{id}" });
            result.AddRange(PrecompileExpression(fn.Condition));
            result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.FalseJump, StringValue = $"{id}-End" });
            result.AddRange(PrecompileBlock(fn.Children, id));
            result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.Jump, StringValue = $"{id}" });
            result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.Label, StringValue = $"{id}-End" });
            return result;
        }

        private IList<KecaknoahILCode> PrecompileExpression(KecaknoahAstNode node)
        {
            var result = new List<KecaknoahILCode>();
            if (node.Type != KecaknoahAstNodeType.Expression) throw new ArgumentException("ASTが式でない件について");
            var en = node as KecaknoahExpressionAstNode;
            if (en is KecaknoahBinaryExpressionAstNode)
            {
                var exp = en as KecaknoahBinaryExpressionAstNode;
                result.AddRange(PrecompileBinaryExpression(exp));
            }
            else if (en is KecaknoahFactorExpressionAstNode)
            {
                var exp = en as KecaknoahFactorExpressionAstNode;
                switch (exp.FactorType)
                {
                    case KecaknoahFactorType.IntegerValue:
                        result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.PushInteger, IntegerValue = exp.IntegerValue });
                        break;
                    case KecaknoahFactorType.SingleValue:
                        result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.PushDouble, FloatValue = exp.SingleValue });
                        break;
                    case KecaknoahFactorType.DoubleValue:
                        result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.PushDouble, FloatValue = exp.DoubleValue });
                        break;
                    case KecaknoahFactorType.StringValue:
                        result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.PushString, StringValue = exp.StringValue });
                        break;
                    case KecaknoahFactorType.BooleanValue:
                        result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.PushBoolean, BooleanValue = exp.BooleanValue });
                        break;
                    case KecaknoahFactorType.Nil:
                        result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.PushNil });
                        break;
                    case KecaknoahFactorType.Identifer:
                        result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.LoadObject, StringValue = exp.StringValue });
                        break;
                    case KecaknoahFactorType.ParenExpression:
                        result.AddRange(PrecompileExpression(exp.ExpressionNode));
                        break;
                    case KecaknoahFactorType.CoroutineResume:
                        result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.ResumeCoroutine, StringValue = exp.StringValue });
                        break;
                }
            }
            else if (en is KecaknoahArgumentCallExpressionAstNode)
            {
                var exp = en as KecaknoahArgumentCallExpressionAstNode;
                if (exp.Target is KecaknoahFactorExpressionAstNode && (exp.Target as KecaknoahFactorExpressionAstNode).FactorType == KecaknoahFactorType.VariableArguments)
                {
                    //vargs
                    foreach (var arg in exp.Arguments) result.AddRange(PrecompileExpression(arg));
                    result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.LoadVarg, IntegerValue = exp.Arguments.Count });
                }
                else
                {
                    if (exp.ExpressionType == KecaknoahOperatorType.IndexerAccess)
                    {
                        result.AddRange(PrecompileIndexerCall(exp));
                    }
                    else
                    {
                        result.AddRange(PrecompileFunctionCall(exp));
                    }
                }
            }
            else if (en is KecaknoahMemberAccessExpressionAstNode)
            {
                var exp = en as KecaknoahMemberAccessExpressionAstNode;
                result.AddRange(PrecompileExpression(exp.Target));
                result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.LoadMember, StringValue = exp.MemberName });
            }
            else if (en is KecaknoahPrimaryExpressionAstNode)
            {
                var exp = en as KecaknoahPrimaryExpressionAstNode;
                //後置

                switch (exp.ExpressionType)
                {
                    case KecaknoahOperatorType.Increment:
                        result.AddRange(PrecompileSuffixIncrement(exp));
                        break;
                    case KecaknoahOperatorType.Decrement:
                        result.AddRange(PrecompileSuffixDecrement(exp));
                        break;
                    default:
                        throw new NotImplementedException("多分実装してない1次式なんだと思う");
                }
                result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.Pop });

            }
            else if (en is KecaknoahUnaryExpressionAstNode)
            {
                var exp = en as KecaknoahUnaryExpressionAstNode;
                switch (exp.ExpressionType)
                {
                    case KecaknoahOperatorType.Minus:
                        result.AddRange(PrecompileExpression(exp.Target));
                        result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.Negative });
                        break;
                    case KecaknoahOperatorType.Not:
                        result.AddRange(PrecompileExpression(exp.Target));
                        result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.Not });
                        break;
                    case KecaknoahOperatorType.Increment:
                        result.AddRange(PrecompilePrefixIncrement(exp));
                        break;
                    case KecaknoahOperatorType.Decrement:
                        result.AddRange(PrecompilePrefixDecrement(exp));

                        break;
                }
            }
            else
            {
                throw new InvalidOperationException("ごめん何言ってるかさっぱりわかんない");
            }
            return result;
        }

        internal IList<KecaknoahILCode> PrecompileBinaryExpression(KecaknoahBinaryExpressionAstNode node)
        {
            var result = new List<KecaknoahILCode>();
            result.AddRange(PrecompileExpression(node.FirstNode));
            result.AddRange(PrecompileExpression(node.SecondNode));
            result.Add(new KecaknoahILCode { Type = (KecaknoahILCodeType)Enum.Parse(typeof(KecaknoahILCodeType), node.ExpressionType.ToString(), true) });
            return result;
        }

        internal IList<KecaknoahILCode> PrecompileFunctionCall(KecaknoahArgumentCallExpressionAstNode node)
        {
            var result = new List<KecaknoahILCode>();
            result.AddRange(PrecompileExpression(node.Target));
            foreach (var arg in node.Arguments) result.AddRange(PrecompileExpression(arg));
            result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.Call, IntegerValue = node.Arguments.Count });
            return result;
        }

        internal IList<KecaknoahILCode> PrecompileIndexerCall(KecaknoahArgumentCallExpressionAstNode node)
        {
            var result = new List<KecaknoahILCode>();
            result.AddRange(PrecompileExpression(node.Target));
            foreach (var arg in node.Arguments) result.AddRange(PrecompileExpression(arg));
            result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.IndexerCall, IntegerValue = node.Arguments.Count });
            return result;
        }

        internal IList<KecaknoahILCode> PrecompileSuffixIncrement(KecaknoahPrimaryExpressionAstNode node)
        {
            var result = new List<KecaknoahILCode>();
            result.AddRange(PrecompileExpression(node.Target));
            result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.AsValue });
            result.AddRange(PrecompileExpression(node.Target));
            result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.PushInteger, IntegerValue = 1 });
            result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.PlusAssign });
            return result;
        }

        internal IList<KecaknoahILCode> PrecompileSuffixDecrement(KecaknoahPrimaryExpressionAstNode node)
        {
            var result = new List<KecaknoahILCode>();
            result.AddRange(PrecompileExpression(node.Target));
            result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.AsValue });
            result.AddRange(PrecompileExpression(node.Target));
            result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.PushInteger, IntegerValue = 1 });
            result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.MinusAssign });
            return result;
        }

        internal IList<KecaknoahILCode> PrecompilePrefixIncrement(KecaknoahUnaryExpressionAstNode node)
        {
            var result = new List<KecaknoahILCode>();
            result.AddRange(PrecompileExpression(node.Target));
            result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.PushInteger, IntegerValue = 1 });
            result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.PlusAssign });
            return result;
        }

        internal IList<KecaknoahILCode> PrecompilePrefixDecrement(KecaknoahUnaryExpressionAstNode node)
        {
            var result = new List<KecaknoahILCode>();
            result.AddRange(PrecompileExpression(node.Target));
            result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.PushInteger, IntegerValue = 1 });
            result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.MinusAssign });
            return result;
        }
    }
}
