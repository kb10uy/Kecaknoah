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
            [KecaknoahTokenType.Plus] = 1
        };

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
            catch (KecaknoahParseException)
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
            result.Type = KecaknoahAstNodeType.Class;
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
            result.Type = KecaknoahAstNodeType.Function;
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

        private KecaknoahLocalAstNode ParseLocal(Queue<KecaknoahToken> tokens)
        {
            var result = new KecaknoahLocalAstNode();
            result.Type = KecaknoahAstNodeType.LocalStatement;
            return result;
        }

        private KecaknoahExpressionAstNode ParseExpression(Queue<KecaknoahToken> tokens)
        {
            var result = new KecaknoahExpressionAstNode();
            result.Type = KecaknoahAstNodeType.Expression;
            return result;
        }
    }

    /// <summary>
    /// KecaknoahのAST構築時の例外を定義します。
    /// </summary>
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
