using Base36Encoder;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;

namespace Kecaknoah.Analyze
{
    /// <summary>
    /// Kecaknoahの構文解析器を定義します。
    /// </summary>
    public sealed class KecaknoahParser
    {
        private static Dictionary<KecaknoahTokenType, int> OperatorPriorities = new Dictionary<KecaknoahTokenType, int>
        {
            //C#に準拠
            [KecaknoahTokenType.Plus] = 11,
            [KecaknoahTokenType.Minus] = 11,
            [KecaknoahTokenType.Multiply] = 12,
            [KecaknoahTokenType.Divide] = 12,
            [KecaknoahTokenType.And] = 7,
            [KecaknoahTokenType.Or] = 5,
            //[KecaknoahTokenType.Not] = 1,
            [KecaknoahTokenType.Xor] = 6,
            [KecaknoahTokenType.Modular] = 12,
            [KecaknoahTokenType.Assign] = 1,
            [KecaknoahTokenType.LeftBitShift] = 10,
            [KecaknoahTokenType.RightBitShift] = 10,
            [KecaknoahTokenType.Equal] = 8,
            [KecaknoahTokenType.NotEqual] = 8,
            [KecaknoahTokenType.Greater] = 9,
            [KecaknoahTokenType.Lesser] = 9,
            [KecaknoahTokenType.GreaterEqual] = 9,
            [KecaknoahTokenType.LesserEqual] = 9,
            [KecaknoahTokenType.SpecialEqual] = 8,
            [KecaknoahTokenType.AndAlso] = 4,
            [KecaknoahTokenType.OrElse] = 3,
            [KecaknoahTokenType.PlusAssign] = 1,
            [KecaknoahTokenType.MinusAssign] = 1,
            [KecaknoahTokenType.MultiplyAssign] = 1,
            [KecaknoahTokenType.DivideAssign] = 1,
            [KecaknoahTokenType.AndAssign] = 1,
            [KecaknoahTokenType.OrAssign] = 1,
            [KecaknoahTokenType.XorAssign] = 1,
            [KecaknoahTokenType.ModularAssign] = 1,
            [KecaknoahTokenType.LeftBitShiftAssign] = 1,
            [KecaknoahTokenType.RightBitShiftAssign] = 1,
            //[KecaknoahTokenType.Increment] = 1,
            //[KecaknoahTokenType.Decrement] = 1,
            [KecaknoahTokenType.Question] = 2,
            [KecaknoahTokenType.Colon] = 2,
            [KecaknoahTokenType.NilAssign] = 1,
        };

        private static Dictionary<KecaknoahTokenType, KecaknoahOperatorType> OperatorsTokenTable = new Dictionary<KecaknoahTokenType, KecaknoahOperatorType>
        {
            //C#に準拠
            [KecaknoahTokenType.Plus] = KecaknoahOperatorType.Plus,
            [KecaknoahTokenType.Minus] = KecaknoahOperatorType.Minus,
            [KecaknoahTokenType.Multiply] = KecaknoahOperatorType.Multiply,
            [KecaknoahTokenType.Divide] = KecaknoahOperatorType.Divide,
            [KecaknoahTokenType.And] = KecaknoahOperatorType.And,
            [KecaknoahTokenType.Or] = KecaknoahOperatorType.Or,
            //[KecaknoahTokenType.Not] = KecaknoahOperatorType.Not,
            [KecaknoahTokenType.Xor] = KecaknoahOperatorType.Xor,
            [KecaknoahTokenType.Modular] = KecaknoahOperatorType.Modular,
            [KecaknoahTokenType.Assign] = KecaknoahOperatorType.Assign,
            [KecaknoahTokenType.LeftBitShift] = KecaknoahOperatorType.LeftBitShift,
            [KecaknoahTokenType.RightBitShift] = KecaknoahOperatorType.RightBitShift,
            [KecaknoahTokenType.Equal] = KecaknoahOperatorType.Equal,
            [KecaknoahTokenType.NotEqual] = KecaknoahOperatorType.NotEqual,
            [KecaknoahTokenType.Greater] = KecaknoahOperatorType.Greater,
            [KecaknoahTokenType.Lesser] = KecaknoahOperatorType.Lesser,
            [KecaknoahTokenType.GreaterEqual] = KecaknoahOperatorType.GreaterEqual,
            [KecaknoahTokenType.LesserEqual] = KecaknoahOperatorType.LesserEqual,
            [KecaknoahTokenType.SpecialEqual] = KecaknoahOperatorType.SpecialEqual,
            [KecaknoahTokenType.AndAlso] = KecaknoahOperatorType.AndAlso,
            [KecaknoahTokenType.OrElse] = KecaknoahOperatorType.OrElse,
            [KecaknoahTokenType.PlusAssign] = KecaknoahOperatorType.PlusAssign,
            [KecaknoahTokenType.MinusAssign] = KecaknoahOperatorType.MinusAssign,
            [KecaknoahTokenType.MultiplyAssign] = KecaknoahOperatorType.MultiplyAssign,
            [KecaknoahTokenType.DivideAssign] = KecaknoahOperatorType.DivideAssign,
            [KecaknoahTokenType.AndAssign] = KecaknoahOperatorType.AndAssign,
            [KecaknoahTokenType.OrAssign] = KecaknoahOperatorType.OrAssign,
            [KecaknoahTokenType.XorAssign] = KecaknoahOperatorType.XorAssign,
            [KecaknoahTokenType.ModularAssign] = KecaknoahOperatorType.ModularAssign,
            [KecaknoahTokenType.LeftBitShiftAssign] = KecaknoahOperatorType.LeftBitShiftAssign,
            [KecaknoahTokenType.RightBitShiftAssign] = KecaknoahOperatorType.RightBitShiftAssign,
            //[KecaknoahTokenType.Increment] = KecaknoahOperatorType.Increment,
            //[KecaknoahTokenType.Decrement] = KecaknoahOperatorType.Decrement,
            [KecaknoahTokenType.Question] = KecaknoahOperatorType.ConditionalQuestion,
            [KecaknoahTokenType.Colon] = KecaknoahOperatorType.ConditionalElse,
            [KecaknoahTokenType.NilAssign] = KecaknoahOperatorType.NilAssign,
        };

        private static int OperatorMaxPriority = OperatorPriorities.Max(p => p.Value);

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        public KecaknoahParser()
        {

        }

        /// <summary>
        /// 指定された<see cref="KecaknoahLexResult"/>を元にASTを構築します。
        /// </summary>
        /// <param name="lex">字句解析の結果</param>
        /// <returns>構築されたAST</returns>
        public KecaknoahAst Parse(KecaknoahLexResult lex)
        {
            var result = new KecaknoahAst(lex.SourceName);
            try
            {
                var top = ParseFirstLevel(new Queue<KecaknoahToken>(lex.Tokens));
                result.RootNode = top;
                result.Success = true;
            }
            catch (KecaknoahParseException e)
            {
                result.RootNode = null;
                result.Success = false;
                result.Error = e.Error;
            }
            return result;
        }

        /// <summary>
        /// 指定された<see cref="KecaknoahLexResult"/>を式として解析します。
        /// </summary>
        /// <param name="lex">字句解析の結果</param>
        /// <returns>構築されたAST</returns>
        public KecaknoahAst ParseAsExpression(KecaknoahLexResult lex)
        {
            var result = new KecaknoahAst(lex.SourceName);
            try
            {
                var q = new Queue<KecaknoahToken>(lex.Tokens);
                var top = ParseExpression(q);
                if (q.Count != 0) throw new KecaknoahParseException(q.Dequeue().CreateErrorAt("解析されないトークンが残っています。"));
                result.RootNode = top;
                result.Success = true;
            }
            catch (KecaknoahParseException e)
            {
                result.RootNode = null;
                result.Success = false;
                result.Error = e.Error;
            }
            return result;
        }

        private KecaknoahAstNode ParseFirstLevel(Queue<KecaknoahToken> tokens)
        {
            var result = new KecaknoahAstNode();
            tokens.SkipLogicalLineBreak();
            try
            {
                while (tokens.Count != 0)
                {
                    var t = tokens.Dequeue();
                    switch (t.Type)
                    {
                        case KecaknoahTokenType.ClassKeyword:
                            result.AddNode(ParseClass(tokens));
                            break;
                        case KecaknoahTokenType.FuncKeyword:
                            result.AddNode(ParseFunction(tokens, true));
                            break;
                        default:
                            throw new KecaknoahParseException(t.CreateErrorAt("トップレベルにはクラスとメソッド以外は定義できません。"));
                    }
                    tokens.SkipLogicalLineBreak();
                }
            }
            catch (KecaknoahParseException)
            {
                throw;
            }
            return result;
        }

        private KecaknoahClassAstNode ParseClass(Queue<KecaknoahToken> tokens)
        {
            var result = new KecaknoahClassAstNode();
            var nt = tokens.Dequeue();
            if (nt.Type != KecaknoahTokenType.Identifer) throw new KecaknoahParseException(nt.CreateErrorAt("クラス名にはキーワードではない識別子を指定してください。"));
            result.Name = nt.TokenString;
            if (!tokens.SkipLogicalLineBreak()) throw new KecaknoahParseException(nt.CreateErrorAt("class宣言の後ろに改行が必要です。"));

            while (true)
            {
                tokens.SkipLogicalLineBreak();
                var t = tokens.Dequeue();
                if (t.Type == KecaknoahTokenType.EndclassKeyword) break;
                switch (t.Type)
                {
                    case KecaknoahTokenType.FuncKeyword:
                        result.AddFunctionNode(ParseFunction(tokens, false));
                        break;
                    case KecaknoahTokenType.LocalKeyword:
                        result.AddLocalNode(ParseLocal(tokens));
                        break;
                    default:
                        throw new KecaknoahParseException(nt.CreateErrorAt("クラス内にはメソッドかlocal宣言のみ記述出来ます。"));
                }
            }
            return result;
        }

        private KecaknoahFunctionAstNode ParseFunction(Queue<KecaknoahToken> tokens, bool top)
        {
            var result = new KecaknoahFunctionAstNode();
            if (tokens.CheckSkipToken(KecaknoahTokenType.StaticKeyword))
            {
                if (top)
                {
                    throw new KecaknoahParseException(tokens.Dequeue().CreateErrorAt("トップレベルのメソッドにstaticは指定できません。"));
                }
                else
                {
                    result.StaticMethod = true;
                }
            }
            var nt = tokens.Dequeue();
            if (nt.Type != KecaknoahTokenType.Identifer) throw new KecaknoahParseException(nt.CreateErrorAt("メソッド名にはキーワードではない識別子を指定してください。"));
            result.Name = nt.TokenString;
            ParseFunctionArgumentsList(tokens, result);
            if (!tokens.SkipLogicalLineBreak()) throw new KecaknoahParseException(nt.CreateErrorAt("func宣言の後ろに改行が必要です。"));
            foreach (var n in ParseBlock(tokens)) result.AddNode(n);
            if (!tokens.CheckSkipToken(KecaknoahTokenType.EndFuncKeyword)) throw new KecaknoahParseException(nt.CreateErrorAt("endfuncがありません。"));
            return result;
        }

        private static void ParseFunctionArgumentsList(Queue<KecaknoahToken> tokens, KecaknoahFunctionAstNode result)
        {
            var nt = tokens.Peek();
            switch (nt.Type)
            {
                case KecaknoahTokenType.NewLine:
                case KecaknoahTokenType.Semicolon:
                    break;
                case KecaknoahTokenType.ParenStart:
                    tokens.Dequeue();
                    while (true)
                    {
                        nt = tokens.Dequeue();
                        switch (nt.Type)
                        {
                            case KecaknoahTokenType.Identifer:
                                result.Parameters.Add(nt.TokenString);
                                tokens.CheckSkipToken(KecaknoahTokenType.Comma);
                                break;
                            case KecaknoahTokenType.VariableArguments:
                                result.AllowsVariableArguments = true;
                                if (!tokens.CheckToken(KecaknoahTokenType.ParenEnd)) throw new KecaknoahParseException(nt.CreateErrorAt("可変長引数は最後に配置してください"));
                                break;
                            case KecaknoahTokenType.ParenEnd:
                                goto EndArgsList;
                            default:
                                throw new KecaknoahParseException(nt.CreateErrorAt("仮引数リストに識別子・可変長引数以外を指定しないでください。"));
                        }

                    }
                EndArgsList:;
                    break;
            }
        }

        private IList<KecaknoahAstNode> ParseBlock(Queue<KecaknoahToken> tokens)
        {
            var result = new List<KecaknoahAstNode>();
            while (true)
            {
                tokens.SkipLogicalLineBreak();
                //式かもしれないのでPeek
                var nt = tokens.Peek();
                switch (nt.Type)
                {
                    case KecaknoahTokenType.IfKeyword:
                        tokens.Dequeue();
                        result.Add(ParseIf(tokens, false));
                        break;
                    case KecaknoahTokenType.CaseKeyword:
                        tokens.Dequeue();
                        result.Add(ParseCase(tokens));
                        break;
                    case KecaknoahTokenType.ForKeyword:
                        tokens.Dequeue();
                        result.Add(ParseFor(tokens, false));
                        break;
                    case KecaknoahTokenType.WhileKeyword:
                        tokens.Dequeue();
                        result.Add(ParseWhile(tokens, false));
                        break;
                    case KecaknoahTokenType.ForeachKeyword:
                        tokens.Dequeue();
                        result.Add(ParseForeach(tokens, false));
                        break;
                    case KecaknoahTokenType.DoKeyword:
                        break;
                    case KecaknoahTokenType.ElifKeyword:
                    case KecaknoahTokenType.ElseKeyword:
                    case KecaknoahTokenType.EndifKeyword:
                    case KecaknoahTokenType.WhenKeyword:
                    case KecaknoahTokenType.DefaultKeyword:
                    case KecaknoahTokenType.EndCaseKeyword:
                    case KecaknoahTokenType.EndFuncKeyword:
                    case KecaknoahTokenType.NextKeyword:
                        //呼ばれ元で終了判定するから飛ばさないでね
                        goto EndBlock;
                    default:
                        result.AddRange(ParseSingleLineStatement(tokens));
                        break;
                }
            }
        EndBlock: return result;
        }

        private IList<KecaknoahAstNode> ParseSingleLineStatement(Queue<KecaknoahToken> tokens)
        {
            var result = new List<KecaknoahAstNode>();
            var nt = tokens.Peek();
            var l = "";
            switch (nt.Type)
            {
                //loop系
                case KecaknoahTokenType.BreakKeyword:
                    tokens.Dequeue();
                    var bl = tokens.Peek();
                    if (bl.Type == KecaknoahTokenType.Identifer)
                    {
                        tokens.Dequeue();
                        l = bl.TokenString;
                    }
                    result.Add(new KecaknoahContinueAstNode { Type = KecaknoahAstNodeType.BreakStatement, Label = l });
                    break;
                case KecaknoahTokenType.ContinueKeyword:
                    tokens.Dequeue();
                    var cl = tokens.Peek();
                    if (cl.Type == KecaknoahTokenType.Identifer)
                    {
                        tokens.Dequeue();
                        l = cl.TokenString;
                    }
                    result.Add(new KecaknoahContinueAstNode { Label = l });
                    break;
                //return系
                case KecaknoahTokenType.ReturnKeyword:
                case KecaknoahTokenType.YieldKeyword:
                    result.Add(ParseReturn(tokens));
                    break;
                //declare系
                case KecaknoahTokenType.CoroutineKeyword:
                    tokens.Dequeue();
                    result.AddRange(ParseCoroutineDeclare(tokens));
                    break;
                case KecaknoahTokenType.LocalKeyword:
                    tokens.Dequeue();
                    result.AddRange(ParseLocal(tokens));
                    break;
                //単行制御構造系
                //ブロックからは呼ばれないはず
                case KecaknoahTokenType.IfKeyword:
                    tokens.Dequeue();
                    result.Add(ParseIf(tokens, true));
                    break;
                case KecaknoahTokenType.ForKeyword:
                    tokens.Dequeue();
                    result.Add(ParseFor(tokens, true));
                    break;
                case KecaknoahTokenType.WhileKeyword:
                    tokens.Dequeue();
                    result.Add(ParseWhile(tokens, true));
                    break;
                case KecaknoahTokenType.ForeachKeyword:
                    tokens.Dequeue();
                    result.Add(ParseForeach(tokens, true));
                    break;

                case KecaknoahTokenType.Semicolon:
                case KecaknoahTokenType.NewLine:
                    throw new KecaknoahParseException(nt.CreateErrorAt("ステートメントが空です。"));
                default:
                    var exp = ParseExpression(tokens);
                    if (!CheckStatementExpression(exp)) throw new KecaknoahParseException("ステートメントにできない式です。");
                    result.Add(exp);
                    break;
            }
            return result;
        }

        private bool CheckStatementExpression(KecaknoahExpressionAstNode node)
        {
            if (node is KecaknoahArgumentCallExpressionAstNode &&
                (node as KecaknoahArgumentCallExpressionAstNode).ExpressionType == KecaknoahOperatorType.FunctionCall)
                return true;
            var bn = node as KecaknoahBinaryExpressionAstNode;
            if (bn == null) return false;
            switch (bn.ExpressionType)
            {
                case KecaknoahOperatorType.Assign:
                case KecaknoahOperatorType.PlusAssign:
                case KecaknoahOperatorType.MinusAssign:
                case KecaknoahOperatorType.MultiplyAssign:
                case KecaknoahOperatorType.DivideAssign:
                case KecaknoahOperatorType.AndAssign:
                case KecaknoahOperatorType.OrAssign:
                case KecaknoahOperatorType.XorAssign:
                case KecaknoahOperatorType.LeftBitShiftAssign:
                case KecaknoahOperatorType.RightBitShiftAssign:
                case KecaknoahOperatorType.NilAssign:
                    return true;
            }
            return false;
        }

        private IList<KecaknoahLocalAstNode> ParseLocal(Queue<KecaknoahToken> tokens)
        {
            var result = new List<KecaknoahLocalAstNode>();
            while (true)
            {
                var nt = tokens.Dequeue();
                if (nt.Type != KecaknoahTokenType.Identifer) throw new KecaknoahParseException(nt.CreateErrorAt("識別子を指定してください。"));
                var lan = new KecaknoahLocalAstNode();
                lan.Name = nt.TokenString;
                result.Add(lan);
                if (tokens.SkipLogicalLineBreak()) return result;
                nt = tokens.Dequeue();
                switch (nt.Type)
                {
                    case KecaknoahTokenType.Assign:
                        lan.InitialExpression = ParseExpression(tokens);
                        if (tokens.SkipLogicalLineBreak()) return result;
                        if (!tokens.CheckSkipToken(KecaknoahTokenType.Comma)) throw new KecaknoahParseException(nt.CreateErrorAt("無効なlocal宣言です。"));
                        tokens.SkipLogicalLineBreak();    //TODO: 暗黙改行
                        break;
                    case KecaknoahTokenType.Comma:
                        tokens.SkipLogicalLineBreak();    //TODO: 暗黙改行
                        continue;
                    default:
                        throw new KecaknoahParseException(nt.CreateErrorAt("無効なlocal宣言です。"));
                }
            }
        }

        private IList<KecaknoahCoroutineDeclareAstNode> ParseCoroutineDeclare(Queue<KecaknoahToken> tokens)
        {
            var result = new List<KecaknoahCoroutineDeclareAstNode>();
            while (true)
            {
                var nt = tokens.Dequeue();
                if (nt.Type != KecaknoahTokenType.Identifer) throw new KecaknoahParseException(nt.CreateErrorAt("識別子を指定してください。"));
                var lan = new KecaknoahCoroutineDeclareAstNode();
                lan.Name = nt.TokenString;
                nt = tokens.Dequeue();
                if (nt.Type != KecaknoahTokenType.Assign) throw new KecaknoahParseException(nt.CreateErrorAt("coroutine宣言は必ず代入してください。"));
                lan.InitialExpression = ParseExpression(tokens);

                if (tokens.CheckSkipToken(KecaknoahTokenType.Colon))
                {
                    //引数
                    if (!tokens.CheckSkipToken(KecaknoahTokenType.ParenStart)) throw new KecaknoahParseException(nt.CreateErrorAt("coroutine宣言の引数リストが不正です。"));
                    tokens.SkipLogicalLineBreak();    //TODO: 暗黙改行
                    while (true)
                    {
                        lan.ParameterExpressions.Add(ParseExpression(tokens));
                        if (tokens.CheckSkipToken(KecaknoahTokenType.ParenEnd)) break;
                        if (!tokens.CheckSkipToken(KecaknoahTokenType.Comma)) throw new KecaknoahParseException(nt.CreateErrorAt("coroutine宣言の引数リストが閉じていません。"));
                        tokens.SkipLogicalLineBreak();    //TODO: 暗黙改行
                    }
                }
                result.Add(lan);
                if (!tokens.CheckSkipToken(KecaknoahTokenType.Comma)) break;
            }
            return result;
        }

        private KecaknoahReturnAstNode ParseReturn(Queue<KecaknoahToken> tokens)
        {
            var result = new KecaknoahReturnAstNode();
            var nt = tokens.Dequeue();
            result.Type = nt.Type == KecaknoahTokenType.ReturnKeyword ? KecaknoahAstNodeType.ReturnStatement : KecaknoahAstNodeType.YieldStatement;
            if (!tokens.SkipLogicalLineBreak()) result.Value = ParseExpression(tokens);
            return result;
        }

        private KecaknoahIfAstNode ParseIf(Queue<KecaknoahToken> tokens, bool single)
        {
            var result = new KecaknoahIfAstNode();
            if (!tokens.CheckSkipToken(KecaknoahTokenType.ParenStart)) throw new KecaknoahParseException(tokens.Peek().CreateErrorAt("if文の条件式はカッコでくくってください。"));
            tokens.SkipLogicalLineBreak();    //TODO: 暗黙改行
            var cnd = ParseExpression(tokens);
            tokens.SkipLogicalLineBreak();    //TODO: 暗黙改行
            var ifb = new KecaknoahIfBlockAstNode();
            ifb.Condition = cnd;
            if (!tokens.CheckSkipToken(KecaknoahTokenType.ParenEnd)) throw new KecaknoahParseException(tokens.Peek().CreateErrorAt("if文の条件式が閉じていません。"));
            if (!tokens.CheckSkipToken(KecaknoahTokenType.ThenKeyword)) throw new KecaknoahParseException(tokens.Peek().CreateErrorAt("thenキーワードをおいてください。"));
            if (!single && tokens.SkipLogicalLineBreak())
            {
                //ブロックif
                var b = ParseBlock(tokens);
                foreach (var i in b) ifb.AddNode(i);
                result.IfBlock = ifb;
                while (true)
                {
                    var nt = tokens.Dequeue();
                    if (nt.Type == KecaknoahTokenType.EndifKeyword)
                    {
                        break;
                    }
                    else if (nt.Type == KecaknoahTokenType.ElifKeyword)
                    {
                        if (!tokens.CheckSkipToken(KecaknoahTokenType.ParenStart)) throw new KecaknoahParseException(tokens.Peek().CreateErrorAt("elif文の条件式はカッコでくくってください。"));
                        tokens.SkipLogicalLineBreak();    //TODO: 暗黙改行
                        cnd = ParseExpression(tokens);
                        var elb = new KecaknoahIfBlockAstNode();
                        tokens.SkipLogicalLineBreak();    //TODO: 暗黙改行
                        elb.Condition = cnd;
                        if (!tokens.CheckSkipToken(KecaknoahTokenType.ParenEnd)) throw new KecaknoahParseException(tokens.Peek().CreateErrorAt("elif文の条件式が閉じていません。"));
                        if (!tokens.CheckSkipToken(KecaknoahTokenType.ThenKeyword)) throw new KecaknoahParseException(tokens.Peek().CreateErrorAt("thenキーワードをおいてください。"));
                        tokens.SkipLogicalLineBreak();
                        b = ParseBlock(tokens);
                        foreach (var i in b) elb.AddNode(i);
                        result.ElifBlocks.Add(elb);
                    }
                    else if (nt.Type == KecaknoahTokenType.ElseKeyword)
                    {
                        tokens.SkipLogicalLineBreak();
                        var esb = new KecaknoahIfBlockAstNode();
                        b = ParseBlock(tokens);
                        foreach (var i in b) esb.AddNode(i);
                        result.ElseBlock = esb;
                    }
                    else
                    {
                        throw new KecaknoahParseException(nt.CreateErrorAt("不正なif文です。"));
                    }
                }
            }
            else
            {
                //単行if
                ifb.AddNode(ParseSingleLineStatement(tokens));
                result.IfBlock = ifb;
                if (tokens.CheckSkipToken(KecaknoahTokenType.ElseKeyword))
                {
                    var esb = new KecaknoahIfBlockAstNode();
                    esb.AddNode(ParseSingleLineStatement(tokens));
                    result.ElseBlock = esb;
                }
            }
            return result;
        }

        private KecaknoahIfAstNode ParseCase(Queue<KecaknoahToken> tokens)
        {
            var result = new KecaknoahIfAstNode();
            var ifb = new KecaknoahIfBlockAstNode();
            ifb.Condition = new KecaknoahFactorExpressionAstNode { FactorType = KecaknoahFactorType.BooleanValue, BooleanValue = false };
            result.IfBlock = ifb;
            if (!tokens.CheckSkipToken(KecaknoahTokenType.ParenStart)) throw new KecaknoahParseException(tokens.Peek().CreateErrorAt("case文の判定式はカッコでくくってください。"));
            tokens.SkipLogicalLineBreak();    //TODO: 暗黙改行
            var target = ParseExpression(tokens);
            tokens.SkipLogicalLineBreak();    //TODO: 暗黙改行
            if (!tokens.CheckSkipToken(KecaknoahTokenType.ParenEnd)) throw new KecaknoahParseException(tokens.Peek().CreateErrorAt("case文の判定式はカッコでくくってください。"));
            if (!tokens.SkipLogicalLineBreak()) throw new KecaknoahParseException(tokens.Peek().CreateErrorAt("case文の判定式の後は改行してください。"));
            var wls = new List<KecaknoahExpressionAstNode>();
            var df = false;
            while (true)
            {
                var nt = tokens.Peek();
                if (nt.Type == KecaknoahTokenType.EndCaseKeyword)
                {
                    tokens.Dequeue();
                    break;
                }
                else if (nt.Type == KecaknoahTokenType.WhenKeyword)
                {
                    tokens.Dequeue();
                    var t2 = ParseExpression(tokens);
                    if (!tokens.CheckSkipToken(KecaknoahTokenType.Colon)) throw new KecaknoahParseException(nt.CreateErrorAt("whenの式の後ろはコロンを付けてください。"));
                    wls.Add(t2);
                    tokens.SkipLogicalLineBreak();
                    continue;
                }
                else if (nt.Type == KecaknoahTokenType.DefaultKeyword)
                {
                    tokens.Dequeue();
                    if (!tokens.CheckSkipToken(KecaknoahTokenType.Colon)) throw new KecaknoahParseException(nt.CreateErrorAt("defaultの後ろはコロンを付けてください。"));
                    df = true;
                    continue;
                }
                else
                {
                    var bl = ParseBlock(tokens);
                    if (wls.Count == 0)
                    {
                        if (!df) throw new KecaknoahParseException(nt.CreateErrorAt("case文内でブロックが浮いています。"));
                        var en = new KecaknoahIfBlockAstNode();
                        foreach (var j in bl) en.AddNode(j);
                        result.ElseBlock = en;
                        df = false;
                    }
                    else
                    {
                        var tn = new KecaknoahBinaryExpressionAstNode();
                        tn.ExpressionType = KecaknoahOperatorType.OrElse;
                        tn.FirstNode = new KecaknoahBinaryExpressionAstNode { ExpressionType = KecaknoahOperatorType.Equal, FirstNode = target, SecondNode = wls[0] };
                        var eln = new KecaknoahIfBlockAstNode();
                        foreach (var i in wls.Skip(1))
                        {
                            var nc = new KecaknoahBinaryExpressionAstNode { ExpressionType = KecaknoahOperatorType.Equal, FirstNode = target, SecondNode = i };
                            tn.SecondNode = nc;
                            var ntn = new KecaknoahBinaryExpressionAstNode();
                            ntn.FirstNode = tn;
                            ntn.ExpressionType = KecaknoahOperatorType.OrElse;
                            tn = ntn;

                        }
                        eln.Condition = tn.FirstNode;
                        foreach (var j in bl) eln.AddNode(j);
                        result.ElifBlocks.Add(eln);
                        if (df)
                        {
                            result.ElseBlock = eln;
                            df = false;
                        }
                        wls.Clear();
                    }
                }
            }
            return result;
        }


        private KecaknoahForAstNode ParseFor(Queue<KecaknoahToken> tokens, bool single)
        {
            var result = new KecaknoahForAstNode();
            var t = tokens.Peek();
            if (t.Type == KecaknoahTokenType.Identifer)
            {
                tokens.Dequeue();
                result.Name = t.TokenString;
            }
            else
            {
                result.Name = Guid.NewGuid().ToString().Substring(0, 8);
            }
            if (!tokens.CheckSkipToken(KecaknoahTokenType.ParenStart)) throw new KecaknoahParseException(tokens.Dequeue().CreateErrorAt("forの各式は全体をカッコでくくってください。"));
            tokens.SkipLogicalLineBreak();    //TODO: 暗黙改行
            while (true)
            {
                var exp = ParseExpression(tokens);
                result.InitializeExpressions.Add(exp);
                if (tokens.CheckSkipToken(KecaknoahTokenType.Semicolon)) break;
                if (!tokens.CheckSkipToken(KecaknoahTokenType.Comma)) throw new KecaknoahParseException(tokens.Dequeue().CreateErrorAt("初期化式はコンマで区切ってください。"));
                tokens.SkipLogicalLineBreak();    //TODO: 暗黙改行
            }
            tokens.SkipLogicalLineBreak();    //TODO: 暗黙改行
            result.Condition = ParseExpression(tokens);
            if (!tokens.CheckSkipToken(KecaknoahTokenType.Semicolon)) throw new KecaknoahParseException(tokens.Dequeue().CreateErrorAt("セミコロンで区切ってください。"));
            tokens.SkipLogicalLineBreak();    //TODO: 暗黙改行
            while (true)
            {
                var exp = ParseExpression(tokens);
                result.CounterExpressions.Add(exp);
                tokens.SkipLogicalLineBreak();    //TODO: 暗黙改行
                if (tokens.CheckToken(KecaknoahTokenType.ParenEnd)) break;
                if (!tokens.CheckSkipToken(KecaknoahTokenType.Comma)) throw new KecaknoahParseException(tokens.Dequeue().CreateErrorAt("初期化式はコンマで区切ってください。"));
                tokens.SkipLogicalLineBreak();    //TODO: 暗黙改行
            }
            if (!tokens.CheckSkipToken(KecaknoahTokenType.ParenEnd)) throw new KecaknoahParseException(tokens.Dequeue().CreateErrorAt("forの各式は全体をカッコでくくってください。"));
            if (single || !tokens.SkipLogicalLineBreak())
            {
                if (tokens.SkipLogicalLineBreak()) throw new KecaknoahParseException(tokens.Dequeue().CreateErrorAt("単行for文のみ記述できます。"));
                foreach (var i in ParseSingleLineStatement(tokens)) result.AddNode(i);
            }
            else
            {
                foreach (var i in ParseBlock(tokens)) result.AddNode(i);
                if (!tokens.CheckSkipToken(KecaknoahTokenType.NextKeyword)) throw new KecaknoahParseException(tokens.Dequeue().CreateErrorAt("nextで終わっていません。"));
            }
            return result;
        }

        private KecaknoahLoopAstNode ParseWhile(Queue<KecaknoahToken> tokens, bool single)
        {
            var result = new KecaknoahLoopAstNode();
            var t = tokens.Peek();
            if (t.Type == KecaknoahTokenType.Identifer)
            {
                tokens.Dequeue();
                result.Name = t.TokenString;
            }
            else
            {
                result.Name = Guid.NewGuid().ToString().Substring(0, 8);
            }
            if (!tokens.CheckSkipToken(KecaknoahTokenType.ParenStart)) throw new KecaknoahParseException(tokens.Dequeue().CreateErrorAt("whileの条件式はカッコでくくってください。"));
            tokens.SkipLogicalLineBreak();    //TODO: 暗黙改行
            result.Condition = ParseExpression(tokens);
            tokens.SkipLogicalLineBreak();    //TODO: 暗黙改行
            if (!tokens.CheckSkipToken(KecaknoahTokenType.ParenEnd)) throw new KecaknoahParseException(tokens.Dequeue().CreateErrorAt("whileの条件式はカッコでくくってください。"));
            if (single || !tokens.SkipLogicalLineBreak())
            {
                if (tokens.SkipLogicalLineBreak()) throw new KecaknoahParseException(tokens.Dequeue().CreateErrorAt("単行while文のみ記述できます。"));
                foreach (var i in ParseSingleLineStatement(tokens)) result.AddNode(i);
            }
            else
            {
                foreach (var i in ParseBlock(tokens)) result.AddNode(i);
                if (!tokens.CheckSkipToken(KecaknoahTokenType.NextKeyword)) throw new KecaknoahParseException(tokens.Dequeue().CreateErrorAt("nextで終わっていません。"));
            }
            return result;
        }

        private KecaknoahLoopAstNode ParseForeach(Queue<KecaknoahToken> tokens, bool single)
        {
            var result = new KecaknoahForeachAstNode();
            var t = tokens.Peek();
            if (t.Type == KecaknoahTokenType.Identifer)
            {
                tokens.Dequeue();
                result.Name = t.TokenString;
            }
            else
            {
                result.Name = Guid.NewGuid().ToString().Substring(0, 8);
            }
            if (!tokens.CheckSkipToken(KecaknoahTokenType.ParenStart)) throw new KecaknoahParseException(tokens.Dequeue().CreateErrorAt("whileの条件式はカッコでくくってください。"));
            tokens.SkipLogicalLineBreak();    //TODO: 暗黙改行
            var nt = tokens.Dequeue();
            if (nt.Type != KecaknoahTokenType.Identifer) throw new KecaknoahParseException(nt.CreateErrorAt("foreachのループ変数には識別子を指定してください。"));
            result.ElementVariableName = nt.TokenString;
            tokens.SkipLogicalLineBreak();    //TODO: 暗黙改行
            nt = tokens.Dequeue();
            tokens.SkipLogicalLineBreak();    //TODO: 暗黙改行
            if (nt.Type == KecaknoahTokenType.InKeyword)
            {
                result.Source = ParseExpression(tokens);
            }
            else if (nt.Type == KecaknoahTokenType.OfKeyword)
            {
                result.IsCoroutineSource = true;
                result.Source = ParseExpression(tokens);
                if (tokens.CheckSkipToken(KecaknoahTokenType.Colon))
                {
                    if (!tokens.CheckSkipToken(KecaknoahTokenType.ParenStart)) throw new KecaknoahParseException(nt.CreateErrorAt("コルーチンの引数リストが不正です。"));
                    tokens.SkipLogicalLineBreak();    //TODO: 暗黙改行
                    while (true)
                    {
                        result.CoroutineArguments.Add(ParseExpression(tokens));
                        tokens.SkipLogicalLineBreak();    //TODO: 暗黙改行
                        if (tokens.CheckSkipToken(KecaknoahTokenType.ParenEnd)) break;
                        if (!tokens.CheckSkipToken(KecaknoahTokenType.Comma)) throw new KecaknoahParseException(nt.CreateErrorAt("コルーチンの引数リストが閉じていません。"));
                        tokens.SkipLogicalLineBreak();    //TODO: 暗黙改行
                    }
                }
            }
            else
            {
                throw new KecaknoahParseException(nt.CreateErrorAt("foreachにはinかofを指定してください。"));
            }
            tokens.SkipLogicalLineBreak();    //TODO: 暗黙改行
            if (!tokens.CheckSkipToken(KecaknoahTokenType.ParenEnd)) throw new KecaknoahParseException(tokens.Dequeue().CreateErrorAt("foreachの条件式はカッコでくくってください。"));
            if (single || !tokens.SkipLogicalLineBreak())
            {
                if (tokens.SkipLogicalLineBreak()) throw new KecaknoahParseException(tokens.Dequeue().CreateErrorAt("単行foreach文のみ記述できます。"));
                foreach (var i in ParseSingleLineStatement(tokens)) result.AddNode(i);
            }
            else
            {
                foreach (var i in ParseBlock(tokens)) result.AddNode(i);
                if (!tokens.CheckSkipToken(KecaknoahTokenType.NextKeyword)) throw new KecaknoahParseException(tokens.Dequeue().CreateErrorAt("nextで終わっていません。"));
            }
            return result;
        }

        private KecaknoahExpressionAstNode ParseExpression(Queue<KecaknoahToken> tokens)
            => ParseBinaryExpression(tokens, 1);

        private KecaknoahExpressionAstNode ParseBinaryExpression(Queue<KecaknoahToken> tokens, int priority)
        {
            if (priority > OperatorMaxPriority) return ParseUnaryExpression(tokens);
            var left = ParseBinaryExpression(tokens, priority + 1);
            var result = new KecaknoahBinaryExpressionAstNode();
            result.FirstNode = left;
            while (true)
            {
                if (tokens.Count == 0) break;
                if (tokens.CheckToken(
                    KecaknoahTokenType.ParenEnd, KecaknoahTokenType.Comma, KecaknoahTokenType.BracketEnd,
                    KecaknoahTokenType.ThenKeyword, KecaknoahTokenType.ElseKeyword, KecaknoahTokenType.Semicolon,
                    KecaknoahTokenType.NewLine, KecaknoahTokenType.Colon))
                {
                    //tokens.Dequeue();
                    break;
                }
                var nt = tokens.Peek();
                if (!OperatorPriorities.ContainsKey(nt.Type)) throw new KecaknoahParseException(nt.CreateErrorAt($"演算子ではなく{nt.Type}が検出されました。"));
                if (OperatorPriorities[nt.Type] != priority) break;
                tokens.Dequeue();
                tokens.SkipLogicalLineBreak();    //TODO: 暗黙改行
                var right = ParseBinaryExpression(tokens, priority + 1);

                result.SecondNode = right;
                result.ExpressionType = OperatorsTokenTable[nt.Type];
                var newres = new KecaknoahBinaryExpressionAstNode();
                newres.FirstNode = result;
                result = newres;
            }
            if (priority == 1)
            {
                var pn = result.FirstNode as KecaknoahBinaryExpressionAstNode;
                if (pn == null) return result.FirstNode;
                while (pn.FirstNode is KecaknoahBinaryExpressionAstNode)
                {
                    switch (pn.ExpressionType)
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
                            break;
                        default:
                            return pn;
                    }
                    var kb = pn.FirstNode as KecaknoahBinaryExpressionAstNode;
                    var nn = new KecaknoahBinaryExpressionAstNode();
                    nn.ExpressionType = pn.ExpressionType;
                    nn.SecondNode = pn.SecondNode;
                    nn.FirstNode = kb.SecondNode;
                    pn.FirstNode = kb.FirstNode;
                    pn.SecondNode = nn;
                }
                return pn;
            }
            return result.FirstNode;
        }

        private KecaknoahUnaryExpressionAstNode ParseUnaryExpression(Queue<KecaknoahToken> tokens)
        {
            var ut = tokens.Peek();
            switch (ut.Type)
            {
                case KecaknoahTokenType.Minus:
                    tokens.Dequeue();
                    var mue = new KecaknoahUnaryExpressionAstNode();
                    mue.Target = ParsePrimaryExpression(tokens);
                    mue.ExpressionType = KecaknoahOperatorType.Minus;
                    return mue;
                case KecaknoahTokenType.Not:
                    tokens.Dequeue();
                    var nue = new KecaknoahUnaryExpressionAstNode();
                    nue.Target = ParsePrimaryExpression(tokens);
                    nue.ExpressionType = KecaknoahOperatorType.Not;
                    return nue;
                case KecaknoahTokenType.Increment:
                    tokens.Dequeue();
                    var iue = new KecaknoahUnaryExpressionAstNode();
                    iue.Target = ParsePrimaryExpression(tokens);
                    iue.ExpressionType = KecaknoahOperatorType.Increment;
                    return iue;
                case KecaknoahTokenType.Decrement:
                    tokens.Dequeue();
                    var due = new KecaknoahUnaryExpressionAstNode();
                    due.Target = ParsePrimaryExpression(tokens);
                    due.ExpressionType = KecaknoahOperatorType.Decrement;
                    return due;
                case KecaknoahTokenType.Plus:
                    tokens.Dequeue();
                    goto default;
                default:
                    var pe = ParsePrimaryExpression(tokens);
                    return pe;
            }
        }

        /// <summary>
        /// 一次式の処理
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        private KecaknoahPrimaryExpressionAstNode ParsePrimaryExpression(Queue<KecaknoahToken> tokens)
        {
            var factor = ParseFactorExpression(tokens);
            //tokens.SkipLogicalLineBreak();
            var re = ParsePrimaryRecursiveExpression(tokens, factor);
            if (re != factor) return re;
            if (!tokens.CheckToken(KecaknoahTokenType.Increment, KecaknoahTokenType.Decrement)) return factor;
            re = new KecaknoahPrimaryExpressionAstNode();
            re.Target = factor;
            if (tokens.CheckSkipToken(KecaknoahTokenType.Increment)) re.ExpressionType = KecaknoahOperatorType.Increment;
            if (tokens.CheckSkipToken(KecaknoahTokenType.Decrement)) re.ExpressionType = KecaknoahOperatorType.Decrement;
            return re;
        }

        /// <summary>
        /// 再帰的に連続させられる一次式の処理
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        private KecaknoahPrimaryExpressionAstNode ParsePrimaryRecursiveExpression(Queue<KecaknoahToken> tokens, KecaknoahPrimaryExpressionAstNode parent)
        {
            var result = parent;
            if (!tokens.CheckToken(KecaknoahTokenType.Period, KecaknoahTokenType.ParenStart, KecaknoahTokenType.BracketStart)) return result;
            while (true)
            {
                if (tokens.CheckSkipToken(KecaknoahTokenType.Period))
                {
                    result = ParsePrimaryMemberAccessExpression(tokens, result);
                }
                else if (tokens.CheckSkipToken(KecaknoahTokenType.ParenStart))
                {
                    result = ParsePrimaryFunctionCallExpression(tokens, result);
                }
                else if (tokens.CheckSkipToken(KecaknoahTokenType.BracketStart))
                {
                    result = ParsePrimaryIndexerAccessExpression(tokens, result);
                }
                else
                {
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// メンバーアクセス処理
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        private KecaknoahMemberAccessExpressionAstNode ParsePrimaryMemberAccessExpression(Queue<KecaknoahToken> tokens, KecaknoahPrimaryExpressionAstNode parent)
        {
            var mnt = tokens.Dequeue();
            if (mnt.Type != KecaknoahTokenType.Identifer) throw new KecaknoahParseException(mnt.CreateErrorAt("有効なメンバー名を指定してください。"));
            var r = new KecaknoahMemberAccessExpressionAstNode();
            r.Target = parent;
            r.ExpressionType = KecaknoahOperatorType.MemberAccess;
            r.MemberName = mnt.TokenString;
            return r;
        }

        /// <summary>
        /// メソッド呼び出しの引数リスト処理
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        private KecaknoahArgumentCallExpressionAstNode ParsePrimaryFunctionCallExpression(Queue<KecaknoahToken> tokens, KecaknoahPrimaryExpressionAstNode parent)
        {
            var r = new KecaknoahArgumentCallExpressionAstNode();
            r.Target = parent;
            r.ExpressionType = KecaknoahOperatorType.FunctionCall;
            tokens.SkipLogicalLineBreak();    //TODO: 暗黙改行
            if (tokens.CheckSkipToken(KecaknoahTokenType.ParenEnd)) return r;
            while (true)
            {
                r.Arguments.Add(ParseExpression(tokens));
                if (tokens.CheckSkipToken(KecaknoahTokenType.Comma)) continue;
                tokens.SkipLogicalLineBreak();    //TODO: 暗黙改行
                if (tokens.CheckSkipToken(KecaknoahTokenType.ParenEnd)) break;
                throw new KecaknoahParseException(tokens.Peek().CreateErrorAt("メソッド呼び出しの引数リストが無効です。"));
            }
            return r;
        }

        /// <summary>
        /// インデクサの引数リスト処理
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        private KecaknoahArgumentCallExpressionAstNode ParsePrimaryIndexerAccessExpression(Queue<KecaknoahToken> tokens, KecaknoahPrimaryExpressionAstNode parent)
        {
            var r = new KecaknoahArgumentCallExpressionAstNode();
            r.Target = parent;
            r.ExpressionType = KecaknoahOperatorType.IndexerAccess;
            tokens.SkipLogicalLineBreak();    //TODO: 暗黙改行
            if (tokens.CheckSkipToken(KecaknoahTokenType.BracketEnd)) return r;
            while (true)
            {
                r.Arguments.Add(ParseExpression(tokens));
                if (tokens.CheckSkipToken(KecaknoahTokenType.Comma)) continue;
                tokens.SkipLogicalLineBreak();    //TODO: 暗黙改行
                if (tokens.CheckSkipToken(KecaknoahTokenType.BracketEnd)) break;
                throw new KecaknoahParseException(tokens.Peek().CreateErrorAt("メソッド呼び出しの引数リストが無効です。"));
            }
            return r;
        }

        private KecaknoahFactorExpressionAstNode ParseFactorExpression(Queue<KecaknoahToken> tokens)
        {
            var t = tokens.Dequeue();
            string lv = "";
            switch (t.Type)
            {
                case KecaknoahTokenType.CoresumeKeyword:
                    if (!tokens.CheckSkipToken(KecaknoahTokenType.ParenStart)) throw new KecaknoahParseException(t.CreateErrorAt("coresumeが不正です。"));
                    t = tokens.Dequeue();
                    if (t.Type != KecaknoahTokenType.Identifer) throw new KecaknoahParseException(t.CreateErrorAt("coresumeが不正です。"));
                    var result = new KecaknoahFactorExpressionAstNode { FactorType = KecaknoahFactorType.CoroutineResume, StringValue = t.TokenString };
                    if (tokens.CheckSkipToken(KecaknoahTokenType.Comma))
                    {
                        //代入とステート返却
                        result.ExpressionNode = ParseExpression(tokens);
                        result.BooleanValue = true;
                    }
                    if (!tokens.CheckSkipToken(KecaknoahTokenType.ParenEnd)) throw new KecaknoahParseException(t.CreateErrorAt("coresumeが不正です。"));
                    return result;
                case KecaknoahTokenType.And:
                    var lambda = new KecaknoahFactorExpressionAstNode();
                    lambda.FactorType = KecaknoahFactorType.Lambda;
                    if (!tokens.CheckSkipToken(KecaknoahTokenType.ParenStart)) throw new KecaknoahParseException(t.CreateErrorAt("ラムダ式の&には引数リストを続けてください。"));
                    if (!tokens.CheckSkipToken(KecaknoahTokenType.ParenEnd))
                        while (true)
                        {
                            lambda.ElementNodes.Add(new KecaknoahFactorExpressionAstNode { FactorType = KecaknoahFactorType.Identifer, StringValue = tokens.Dequeue().TokenString });
                            if (tokens.CheckSkipToken(KecaknoahTokenType.ParenEnd)) break;
                            if (!tokens.CheckSkipToken(KecaknoahTokenType.Comma)) throw new KecaknoahParseException(tokens.Peek().CreateErrorAt("ラムダ引数がカッコで閉じていなません。"));
                        }
                    if (!tokens.CheckSkipToken(KecaknoahTokenType.Lambda)) throw new KecaknoahParseException(t.CreateErrorAt("ラムダ式の引数リストに=>で式を続けてください。"));
                    lambda.ExpressionNode = ParseExpression(tokens);
                    return lambda;
                case KecaknoahTokenType.ParenStart:
                    tokens.SkipLogicalLineBreak();    //TODO: 暗黙改行
                    var exp = ParseExpression(tokens);
                    tokens.SkipLogicalLineBreak();    //TODO: 暗黙改行
                    if (!tokens.CheckSkipToken(KecaknoahTokenType.ParenEnd)) throw new KecaknoahParseException(tokens.Peek().CreateErrorAt("カッコは閉じてください。"));
                    return new KecaknoahFactorExpressionAstNode { FactorType = KecaknoahFactorType.ParenExpression, ExpressionNode = exp };
                case KecaknoahTokenType.BracketStart:
                    var are = new KecaknoahFactorExpressionAstNode { FactorType = KecaknoahFactorType.Array };
                    tokens.SkipLogicalLineBreak();    //TODO: 暗黙改行
                    while (true)
                    {
                        are.ElementNodes.Add(ParseExpression(tokens));
                        tokens.SkipLogicalLineBreak();    //TODO: 暗黙改行
                        if (tokens.CheckSkipToken(KecaknoahTokenType.BracketEnd)) break;
                        if (!tokens.CheckSkipToken(KecaknoahTokenType.Comma)) throw new KecaknoahParseException(tokens.Peek().CreateErrorAt("配列がカッコで閉じていなません。"));
                        tokens.SkipLogicalLineBreak();    //TODO: 暗黙改行
                    }
                    return are;
                case KecaknoahTokenType.TrueKeyword:
                case KecaknoahTokenType.FalseKeyword:
                    return new KecaknoahFactorExpressionAstNode { FactorType = KecaknoahFactorType.BooleanValue, BooleanValue = Convert.ToBoolean(t.TokenString) };
                case KecaknoahTokenType.NilKeyword:
                    return new KecaknoahFactorExpressionAstNode { FactorType = KecaknoahFactorType.Nil };
                case KecaknoahTokenType.VargsKeyword:
                    return new KecaknoahFactorExpressionAstNode { FactorType = KecaknoahFactorType.VariableArguments };
                case KecaknoahTokenType.Identifer:
                    return new KecaknoahFactorExpressionAstNode { FactorType = KecaknoahFactorType.Identifer, StringValue = t.TokenString };
                case KecaknoahTokenType.StringLiteral:
                    return new KecaknoahFactorExpressionAstNode { FactorType = KecaknoahFactorType.StringValue, StringValue = t.TokenString };
                case KecaknoahTokenType.BinaryNumberLiteral:
                    lv = t.TokenString.Substring(2);
                    if (lv.Length > 64) lv = lv.Substring(lv.Length - 64);
                    return new KecaknoahFactorExpressionAstNode { FactorType = KecaknoahFactorType.IntegerValue, IntegerValue = unchecked(Convert.ToInt64(lv, 2)) };
                case KecaknoahTokenType.OctadecimalNumberLiteral:
                    lv = t.TokenString.Substring(2);
                    if (lv.Length > 64) lv = lv.Substring(lv.Length - 21);
                    return new KecaknoahFactorExpressionAstNode { FactorType = KecaknoahFactorType.IntegerValue, IntegerValue = unchecked(Convert.ToInt64(lv, 8)) };
                case KecaknoahTokenType.HexadecimalNumberLiteral:
                    lv = t.TokenString.Substring(2);
                    if (lv.Length > 64) lv = lv.Substring(lv.Length - 16);
                    return new KecaknoahFactorExpressionAstNode { FactorType = KecaknoahFactorType.IntegerValue, IntegerValue = unchecked(Convert.ToInt64(lv, 16)) };
                case KecaknoahTokenType.HexatridecimalNumberLiteral:
                    return new KecaknoahFactorExpressionAstNode { FactorType = KecaknoahFactorType.IntegerValue, IntegerValue = unchecked(Base36.Decode(t.TokenString.Substring(2))) };
                case KecaknoahTokenType.DecimalNumberLiteral:
                    if (t.TokenString.IndexOf('.') >= 0)
                    {
                        lv = t.TokenString.Substring(0, t.TokenString.Length - 1);
                        var v = 0.0;
                        var r = double.TryParse(lv, out v);
                        return new KecaknoahFactorExpressionAstNode { FactorType = KecaknoahFactorType.DoubleValue, DoubleValue = r ? v : double.MaxValue };
                    }
                    else
                    {
                        var v = 0L;
                        var r = long.TryParse(t.TokenString, out v);
                        return new KecaknoahFactorExpressionAstNode { FactorType = KecaknoahFactorType.IntegerValue, IntegerValue = r ? v : long.MaxValue };
                    }
                default:
                    throw new KecaknoahParseException(t.CreateErrorAt($"Factorが検出されるべきですが{t.Type}が検出されました。"));

            }
        }
    }

    /// <summary>
    /// KecaknoahのAST構築時の例外を定義します。
    /// </summary>
    [Serializable]
    internal class KecaknoahParseException : Exception
    {
        /// <summary>
        /// 発生時の<see cref="KecaknoahError"/>を取得します。
        /// </summary>
        public KecaknoahError Error { get; internal set; }

        /// <summary>
        /// 規定のコンストラクター
        /// </summary>
        public KecaknoahParseException()
        {
            Error = new KecaknoahError { Column = 0, Line = 0, Message = "" };
        }

        /// <summary>
        /// 指定した<see cref="KecaknoahError"/>を格納して初期化します。
        /// </summary>
        /// <param name="error">エラー情報</param>
        public KecaknoahParseException(KecaknoahError error) : base(error.Message)
        {
            Error = error;
        }

        /// <summary>
        /// 指定したメッセージ以下略
        /// </summary>
        /// <param name="message">メッセージ</param>
        public KecaknoahParseException(string message) : base(message)
        {
            Error = new KecaknoahError { Column = 0, Line = 0, Message = message };
        }

        /// <summary>
        /// 規定
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="inner">内側</param>
        public KecaknoahParseException(string message, Exception inner) : base(message, inner)
        {
            Error = new KecaknoahError { Column = 0, Line = 0, Message = message };
        }
    }
}
