using Base36Encoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah
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
            [KecaknoahTokenType.ConditionalQuestion] = 2,
            [KecaknoahTokenType.ConditionalElse] = 2,
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
            [KecaknoahTokenType.ConditionalQuestion] = KecaknoahOperatorType.ConditionalQuestion,
            [KecaknoahTokenType.ConditionalElse] = KecaknoahOperatorType.ConditionalElse,
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
                result.Success = top.IsComplete;
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
                var top = ParseExpression(new Queue<KecaknoahToken>(lex.Tokens));
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
            try
            {
                while (tokens.Count != 0)
                {
                    tokens.SkipLogicalLineBreak();
                    var t = tokens.Dequeue();
                    switch (t.Type)
                    {
                        case KecaknoahTokenType.ClassKeyword:
                            result.AddNode(ParseClass(tokens));
                            break;
                        case KecaknoahTokenType.FuncKeyword:
                            result.AddNode(ParseFunction(tokens));
                            break;
                        default:
                            throw new KecaknoahParseException(t.CreateErrorAt("トップレベルにはクラスとメソッド以外は定義できません。"));
                    }
                }
            }
            catch (KecaknoahParseException e)
            {
                throw;
            }
            catch
            {
                result.IsComplete = false;
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
                        result.AddFunctionNode(ParseFunction(tokens));
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

        private KecaknoahFunctionAstNode ParseFunction(Queue<KecaknoahToken> tokens)
        {
            var result = new KecaknoahFunctionAstNode();
            var nt = tokens.Dequeue();
            if (nt.Type != KecaknoahTokenType.Identifer) throw new KecaknoahParseException(nt.CreateErrorAt("メソッド名にはキーワードではない識別子を指定してください。"));
            result.Name = nt.TokenString;
            ParseFunctionArgumentsList(tokens, result);
            if (!tokens.SkipLogicalLineBreak()) throw new KecaknoahParseException(nt.CreateErrorAt("func宣言の後ろに改行が必要です。"));
            return result;
        }

        private static void ParseFunctionArgumentsList(Queue<KecaknoahToken> tokens, KecaknoahFunctionAstNode result)
        {
            var nt = tokens.Dequeue();
            switch (nt.Type)
            {
                case KecaknoahTokenType.NewLine:
                case KecaknoahTokenType.Semicolon:
                    break;
                case KecaknoahTokenType.ParenStart:
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

        private IList<KecaknoahLocalAstNode> ParseLocal(Queue<KecaknoahToken> tokens)
        {
            var result = new List<KecaknoahLocalAstNode>();
            if (tokens.SkipLogicalLineBreak()) return result;
            while (true)
            {
                var nt = tokens.Dequeue();
                if (nt.Type != KecaknoahTokenType.Identifer) throw new KecaknoahParseException(nt.CreateErrorAt("識別子を指定してください。"));
                var lan = new KecaknoahLocalAstNode();
                lan.Name = nt.TokenString;
                if (tokens.SkipLogicalLineBreak()) return result;
                nt = tokens.Dequeue();
                switch (nt.Type)
                {
                    case KecaknoahTokenType.Assign:
                        lan.InitialExpression = ParseExpression(tokens);
                        if (tokens.SkipLogicalLineBreak()) return result;
                        if (!tokens.CheckToken(KecaknoahTokenType.Comma)) throw new KecaknoahParseException(nt.CreateErrorAt("無効なlocal宣言です。"));
                        break;
                    case KecaknoahTokenType.Comma:
                        continue;
                    default:
                        throw new KecaknoahParseException(nt.CreateErrorAt("無効なlocal宣言です。"));
                }
            }
        }

        private KecaknoahExpressionAstNode ParseExpression(Queue<KecaknoahToken> tokens)
            => ParseBinaryExpression(tokens, 1);

        private KecaknoahExpressionAstNode ParseBinaryExpression(Queue<KecaknoahToken> tokens, int priority)
        {
            if (priority > OperatorMaxPriority) return ParsePrimaryExpression(tokens);
            var left = ParseBinaryExpression(tokens, priority + 1);
            var result = new KecaknoahBinaryExpressionAstNode();
            result.FirstNode = left;
            while (true)
            {
                if (tokens.Count == 0) break;
                if (tokens.CheckToken(KecaknoahTokenType.ParenEnd, KecaknoahTokenType.Comma,KecaknoahTokenType.BracketEnd))
                {
                    //tokens.Dequeue();
                    break;
                }
                var nt = tokens.Peek();
                if (OperatorPriorities[nt.Type] != priority) break;
                tokens.Dequeue();
                var right = ParseBinaryExpression(tokens, priority + 1);
                result.SecondNode = right;
                result.ExpressionType = OperatorsTokenTable[nt.Type];
                var newres = new KecaknoahBinaryExpressionAstNode();
                newres.FirstNode = result;
                result = newres;
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
            if (re != null) return re;
            if (tokens.CheckToken(KecaknoahTokenType.Increment, KecaknoahTokenType.Decrement)) return factor;
            re = new KecaknoahPrimaryExpressionAstNode();
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
            if (!tokens.CheckToken(KecaknoahTokenType.Period, KecaknoahTokenType.ParenStart)) return result;
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
            if (tokens.CheckSkipToken(KecaknoahTokenType.ParenEnd)) return r;
            while (true)
            {
                r.Arguments.Add(ParseExpression(tokens));
                if (tokens.CheckSkipToken(KecaknoahTokenType.Comma)) continue;
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
            if (tokens.CheckSkipToken(KecaknoahTokenType.BracketEnd)) return r;
            while (true)
            {
                r.Arguments.Add(ParseExpression(tokens));
                if (tokens.CheckSkipToken(KecaknoahTokenType.Comma)) continue;
                if (tokens.CheckSkipToken(KecaknoahTokenType.BracketEnd)) break;
                throw new KecaknoahParseException(tokens.Peek().CreateErrorAt("メソッド呼び出しの引数リストが無効です。"));
            }
            return r;
        }

        private KecaknoahFactorExpressionAstNode ParseFactorExpression(Queue<KecaknoahToken> tokens)
        {
            var t = tokens.Dequeue();
            switch (t.Type)
            {
                case KecaknoahTokenType.ParenStart:
                    var exp = ParseExpression(tokens);
                    if (!tokens.CheckSkipToken(KecaknoahTokenType.ParenEnd)) throw new KecaknoahParseException(tokens.Peek().CreateErrorAt("カッコは閉じてください。"));
                    return new KecaknoahFactorExpressionAstNode { FactorType = KecaknoahFactorType.ParenExpression, ExpressionNode = exp };
                case KecaknoahTokenType.TrueKeyword:
                case KecaknoahTokenType.FalseKeyword:
                    return new KecaknoahFactorExpressionAstNode { FactorType = KecaknoahFactorType.BooleanValue, BooleanValue = Convert.ToBoolean(t.TokenString) };
                case KecaknoahTokenType.NilKeyword:
                    return new KecaknoahFactorExpressionAstNode { FactorType = KecaknoahFactorType.Nil };
                case KecaknoahTokenType.Identifer:
                    return new KecaknoahFactorExpressionAstNode { FactorType = KecaknoahFactorType.Identifer, StringValue = t.TokenString };
                case KecaknoahTokenType.StringLiteral:
                    return new KecaknoahFactorExpressionAstNode { FactorType = KecaknoahFactorType.StringValue, StringValue = t.TokenString };
                case KecaknoahTokenType.BinaryNumberLiteral:
                    return new KecaknoahFactorExpressionAstNode { FactorType = KecaknoahFactorType.IntegerValue, IntegerValue = unchecked(Convert.ToInt64(t.TokenString.Substring(2), 2)) };
                case KecaknoahTokenType.OctadecimalNumberLiteral:
                    return new KecaknoahFactorExpressionAstNode { FactorType = KecaknoahFactorType.IntegerValue, IntegerValue = unchecked(Convert.ToInt64(t.TokenString.Substring(2), 8)) };
                case KecaknoahTokenType.HexadecimalNumberLiteral:
                    return new KecaknoahFactorExpressionAstNode { FactorType = KecaknoahFactorType.IntegerValue, IntegerValue = unchecked(Convert.ToInt64(t.TokenString.Substring(2), 16)) };
                case KecaknoahTokenType.HexatridecimalNumberLiteral:
                    return new KecaknoahFactorExpressionAstNode { FactorType = KecaknoahFactorType.IntegerValue, IntegerValue = Base36.Decode(t.TokenString.Substring(2)) };
                case KecaknoahTokenType.DecimalNumberLiteral:
                    if (t.TokenString.EndsWith("f"))
                    {
                        return new KecaknoahFactorExpressionAstNode { FactorType = KecaknoahFactorType.SingleValue, SingleValue = unchecked(Convert.ToSingle(t.TokenString.Substring(0, t.TokenString.Length - 1))) };
                    }
                    else if (t.TokenString.IndexOf('.') >= 0)
                    {
                        return new KecaknoahFactorExpressionAstNode { FactorType = KecaknoahFactorType.DoubleValue, DoubleValue = unchecked(Convert.ToDouble(t.TokenString)) };
                    }
                    else
                    {
                        return new KecaknoahFactorExpressionAstNode { FactorType = KecaknoahFactorType.IntegerValue, IntegerValue = unchecked(Convert.ToInt64(t.TokenString)) };
                    }
                default:
                    throw new KecaknoahParseException(t.CreateErrorAt("意味不明なfactorが検出されました。"));

            }
        }
    }

    /// <summary>
    /// KecaknoahのAST構築時の例外を定義します。
    /// </summary>
    [Serializable]
    public class KecaknoahParseException : Exception
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
