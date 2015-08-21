using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

                }
                else
                {
                    throw new InvalidOperationException("トップレベルにはクラスとメソッド以外おけません");
                }
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// 1つのブロックからなる<see cref="KecaknoahAst"/>をプリコンパイルします。
        /// </summary>
        /// <param name="ast">対象の<see cref="KecaknoahAst"/></param>
        /// <returns>プリコンパイル結果</returns>
        public KecaknoahIL PrecompileBlock(KecaknoahAst ast)
        {
            throw new NotImplementedException();
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

        private KecaknoahScriptClassInfo PrecompileClass(KecaknoahClassAstNode ast)
        {
            //TODO: local初期値式対応
            throw new NotImplementedException();
        }

        private KecaknoahScriptMethodInfo PrecompileClass(KecaknoahFunctionAstNode ast)
        {
            throw new NotImplementedException();
        }

        private IList<KecaknoahILCode> PrecompileExpression(KecaknoahAstNode node)
        {
            var result = new List<KecaknoahILCode>();
            if (node.Type != KecaknoahAstNodeType.Expression) throw new ArgumentException("ASTが式でない件について");
            var en = node as KecaknoahExpressionAstNode;
            if (en is KecaknoahBinaryExpressionAstNode)
            {
                var exp = en as KecaknoahBinaryExpressionAstNode;
                result.AddRange(PrecompileExpression(exp.FirstNode));
                result.AddRange(PrecompileExpression(exp.SecondNode));
                switch (exp.ExpressionType)
                {
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
                        //将来ここは"hoge = hoge <op> val"形式のコードを生成するようになるかも
                        result.Add(new KecaknoahILCode { Type = (KecaknoahILCodeType)Enum.Parse(typeof(KecaknoahILCodeType), exp.ExpressionType.ToString(), true) });
                        break;
                    default:
                        result.Add(new KecaknoahILCode { Type = (KecaknoahILCodeType)Enum.Parse(typeof(KecaknoahILCodeType), exp.ExpressionType.ToString(), true) });
                        break;
                }
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
                }
            }
            else if (en is KecaknoahArgumentCallExpressionAstNode)
            {
                var exp = en as KecaknoahArgumentCallExpressionAstNode;
                result.AddRange(PrecompileExpression(exp.Target));
                foreach (var arg in exp.Arguments) result.AddRange(PrecompileExpression(arg));
                if (exp.ExpressionType == KecaknoahOperatorType.IndexerAccess)
                {
                    result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.IndexerCall, IntegerValue = exp.Arguments.Count });
                }
                else
                {
                    result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.Call, IntegerValue = exp.Arguments.Count });
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
                result.AddRange(PrecompileExpression(exp.Target));
                result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.AsValue });
                result.AddRange(PrecompileExpression(exp.Target));
                result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.PushInteger, IntegerValue = 1 });
                switch(exp.ExpressionType)
                {
                    case KecaknoahOperatorType.Increment:
                        result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.PlusAssign });
                        break;
                    case KecaknoahOperatorType.Decrement:
                        result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.MinusAssign });
                        break;
                    default:
                        throw new NotImplementedException("多分実装してない1次式なんだと思う");
                }
                result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.Pop });
                
            }
            else if (en is KecaknoahUnaryExpressionAstNode)
            {
                var exp = en as KecaknoahUnaryExpressionAstNode;
                result.AddRange(PrecompileExpression(exp.Target));
                switch (exp.ExpressionType)
                {
                    case KecaknoahOperatorType.Minus:
                        result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.Negative });
                        break;
                    case KecaknoahOperatorType.Not:
                        result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.Not });
                        break;
                    case KecaknoahOperatorType.Increment:
                        result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.PushInteger, IntegerValue = 1 });
                        result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.PlusAssign });
                        break;
                    case KecaknoahOperatorType.Decrement:
                        result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.PushInteger, IntegerValue = 1 });
                        result.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.MinusAssign });
                        break;
                }
            }
            else
            {
                throw new InvalidOperationException("ごめん何言ってるかさっぱりわかんない");
            }
            return result;
        }
    }
}
