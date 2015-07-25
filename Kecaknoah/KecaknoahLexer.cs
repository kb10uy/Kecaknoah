using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Kecaknoah
{
    /// <summary>
    /// Kecaknoahの字句解析器を定義します。
    /// </summary>
    public sealed class KecaknoahLexer
    {
        #region static fields
        private static IOrderedEnumerable<Tuple<string, KecaknoahTokenType>> Keywords = new List<Tuple<string, KecaknoahTokenType>>
        {
            new Tuple<string, KecaknoahTokenType>("class",KecaknoahTokenType.ClassKeyword),
            new Tuple<string, KecaknoahTokenType>("endclass",KecaknoahTokenType.EndclassKeyword),
            new Tuple<string, KecaknoahTokenType>("func",KecaknoahTokenType.FuncKeyword),
            new Tuple<string, KecaknoahTokenType>("endfunc",KecaknoahTokenType.EndFuncKeyword),
            new Tuple<string, KecaknoahTokenType>("if",KecaknoahTokenType.IfKeyword),
            new Tuple<string, KecaknoahTokenType>("elif",KecaknoahTokenType.ElifKeyword),
            new Tuple<string, KecaknoahTokenType>("else",KecaknoahTokenType.ElseKeyword),
            new Tuple<string, KecaknoahTokenType>("endif",KecaknoahTokenType.EndifKeyword),
            new Tuple<string, KecaknoahTokenType>("case",KecaknoahTokenType.CaseKeyword),
            new Tuple<string, KecaknoahTokenType>("when",KecaknoahTokenType.WhenKeyword),
            new Tuple<string, KecaknoahTokenType>("default",KecaknoahTokenType.DefaultKeyword),
            new Tuple<string, KecaknoahTokenType>("endcase",KecaknoahTokenType.EndcaseKeyword),
            new Tuple<string, KecaknoahTokenType>("for",KecaknoahTokenType.ForKeyword),
            new Tuple<string, KecaknoahTokenType>("continue",KecaknoahTokenType.ContinueKeyword),
            new Tuple<string, KecaknoahTokenType>("break",KecaknoahTokenType.BreakKeyword),
            new Tuple<string, KecaknoahTokenType>("next",KecaknoahTokenType.NextKeyword),
            new Tuple<string, KecaknoahTokenType>("while",KecaknoahTokenType.WhileKeyword),
            new Tuple<string, KecaknoahTokenType>("do",KecaknoahTokenType.DoKeyword),
            new Tuple<string, KecaknoahTokenType>("local",KecaknoahTokenType.LocalKeyword),
            new Tuple<string, KecaknoahTokenType>("true",KecaknoahTokenType.TrueKeyword),
            new Tuple<string, KecaknoahTokenType>("false",KecaknoahTokenType.FalseKeyword),
            new Tuple<string, KecaknoahTokenType>("nil",KecaknoahTokenType.NilKeyword),
        }.OrderByDescending(p => p.Item1.Length).ThenBy(p => p.Item1);

        private static IOrderedEnumerable<Tuple<string, KecaknoahTokenType>> Operators = new List<Tuple<string, KecaknoahTokenType>>
        {
            new Tuple<string, KecaknoahTokenType>("+", KecaknoahTokenType.Plus),
            new Tuple<string, KecaknoahTokenType>("-", KecaknoahTokenType.Minus),
            new Tuple<string, KecaknoahTokenType>("*", KecaknoahTokenType.Multiply),
            new Tuple<string, KecaknoahTokenType>("/", KecaknoahTokenType.Divide),
            new Tuple<string, KecaknoahTokenType>("&", KecaknoahTokenType.And),
            new Tuple<string, KecaknoahTokenType>("|", KecaknoahTokenType.Or),
            new Tuple<string, KecaknoahTokenType>("!", KecaknoahTokenType.Not),
            new Tuple<string, KecaknoahTokenType>("^", KecaknoahTokenType.Xor),
            new Tuple<string, KecaknoahTokenType>("%", KecaknoahTokenType.Modular),
            new Tuple<string, KecaknoahTokenType>("=", KecaknoahTokenType.Assign),
            new Tuple<string, KecaknoahTokenType>("<<", KecaknoahTokenType.LeftBitShift),
            new Tuple<string, KecaknoahTokenType>(">>", KecaknoahTokenType.RightBitShift),
            new Tuple<string, KecaknoahTokenType>("==", KecaknoahTokenType.Equal),
            new Tuple<string, KecaknoahTokenType>("!=", KecaknoahTokenType.NotEqual),
            new Tuple<string, KecaknoahTokenType>(">", KecaknoahTokenType.Greater),
            new Tuple<string, KecaknoahTokenType>("<", KecaknoahTokenType.Lesser),
            new Tuple<string, KecaknoahTokenType>(">=", KecaknoahTokenType.GreaterEqual),
            new Tuple<string, KecaknoahTokenType>("<=", KecaknoahTokenType.LesserEqual),
            new Tuple<string, KecaknoahTokenType>("~=", KecaknoahTokenType.SpecialEqual),
            new Tuple<string, KecaknoahTokenType>("&&", KecaknoahTokenType.AndAlso),
            new Tuple<string, KecaknoahTokenType>("||", KecaknoahTokenType.OrElse),
            new Tuple<string, KecaknoahTokenType>("+=", KecaknoahTokenType.PlusAssign),
            new Tuple<string, KecaknoahTokenType>("-=", KecaknoahTokenType.MinusAssign),
            new Tuple<string, KecaknoahTokenType>("*=", KecaknoahTokenType.MultiplyAssign),
            new Tuple<string, KecaknoahTokenType>("/=", KecaknoahTokenType.DivideAssign),
            new Tuple<string, KecaknoahTokenType>("&=", KecaknoahTokenType.AndAssign),
            new Tuple<string, KecaknoahTokenType>("|=", KecaknoahTokenType.OrAssign),
            new Tuple<string, KecaknoahTokenType>("^=", KecaknoahTokenType.XorAssign),
            new Tuple<string, KecaknoahTokenType>("%=", KecaknoahTokenType.ModularAssign),
            new Tuple<string, KecaknoahTokenType>("<<=", KecaknoahTokenType.LeftBitShiftAssign),
            new Tuple<string, KecaknoahTokenType>(">>=", KecaknoahTokenType.RightBitShiftAssign),
            new Tuple<string, KecaknoahTokenType>("?", KecaknoahTokenType.ConditionalQuestion),
            new Tuple<string, KecaknoahTokenType>(":", KecaknoahTokenType.ConditionalElse),
            new Tuple<string, KecaknoahTokenType>("||=", KecaknoahTokenType.NilAssign),
            new Tuple<string, KecaknoahTokenType>(",", KecaknoahTokenType.Comma),
            new Tuple<string, KecaknoahTokenType>(".", KecaknoahTokenType.Period),

            new Tuple<string, KecaknoahTokenType>(Environment.NewLine, KecaknoahTokenType.NewLine),
            new Tuple<string, KecaknoahTokenType>(";", KecaknoahTokenType.Semicolon),
            new Tuple<string, KecaknoahTokenType>("(", KecaknoahTokenType.ParenStart),
            new Tuple<string, KecaknoahTokenType>(")", KecaknoahTokenType.ParenEnd),
            new Tuple<string, KecaknoahTokenType>("{", KecaknoahTokenType.BraceStart),
            new Tuple<string, KecaknoahTokenType>("}", KecaknoahTokenType.BraceEnd),
            new Tuple<string, KecaknoahTokenType>("[", KecaknoahTokenType.BracketStart),
            new Tuple<string, KecaknoahTokenType>("]", KecaknoahTokenType.BracketEnd),
        }.OrderByDescending(p => p.Item1.Length).ThenBy(p => p.Item1);

        private static Regex DecimalNumberPattern = new Regex("[0-9_]+(\\.[0-9_]+)?[df]?");
        private static Regex BinaryNumberPattern = new Regex("0b[01]+");
        private static Regex OctadecimalNumberPattern = new Regex("0[oO][0-7]+");
        private static Regex HexadecimalNumberPattern = new Regex("0[xX][0-9a-fA-F]+");
        private static Regex HexatridecimalNumberPattern = new Regex("0[tT][0-9a-zA-Z]+");
        private static Regex IdentiferPattern = new Regex("[_a-zA-Z][A-Za-z_0-9@]*");
        private static List<Tuple<string, string>> MultilineCommentQuotations = new List<Tuple<string, string>> {
            new Tuple<string, string>("/*", "*/"),
            new Tuple<string, string>("#{", "#}"),
        };
        private static List<string> LineCommentStart = new List<string> { "//", "#" };
        private static List<string> StringQuotation = new List<string> { "\"", "'" };
        private static Regex WhitespacePattern = new Regex("[ \t]+");
        private static List<Tuple<string, string>> StringLiteralEscapes = new List<Tuple<string, string>>
        {
            new Tuple<string, string>("\\r", "\r"),
            new Tuple<string, string>("\\n", Environment.NewLine),
            new Tuple<string, string>("\\t", "\t"),
            new Tuple<string, string>("\\\\", "\\"),
            new Tuple<string, string>("\\\"", "\""),
            new Tuple<string, string>("\\'", "'"),
        };
        #endregion

        #region properties
        /// <summary>
        /// エラーや警告などの情報を出力する<see cref="TextWriter"/>を取得・設定します。
        /// </summary>
        public TextWriter OutputWriter { get; set; } = Console.Out;

        /// <summary>
        /// <see cref="AnalyzeFromFile(string)"/>で出力される<see cref="KecaknoahLexResult.SourceName"/>にフルパスを設定します。
        /// </summary>
        public bool SetFullFileName { get; set; } = false;

        /// <summary>
        /// <see cref="AnalyzeFromSource(string)"/>で出力される<see cref="KecaknoahLexResult.SourceName"/>に設定される文字列を取得・設定します。
        /// </summary>
        public string DefaultSourceName { get; set; } = "No name";
        #endregion

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        public KecaknoahLexer()
        {
        }

        /// <summary>
        /// ファイルを指定して解析します。
        /// <see cref="Encoding.Default"/>のエンコードのものとされます。
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <returns>解析結果</returns>
        public KecaknoahLexResult AnalyzeFromFile(string fileName) => AnalyzeFromText(Path.GetFileName(fileName), File.ReadAllText(fileName, Encoding.Default));

        /// <summary>
        /// ファイルを指定して解析します。
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <param name="encode">仕様する<see cref="Encoding"/></param>
        /// <returns>解析結果</returns>
        public KecaknoahLexResult AnalyzeFromFile(string fileName, Encoding encode) => AnalyzeFromText(Path.GetFileName(fileName), File.ReadAllText(fileName, encode));

        /// <summary>
        /// ソースコードを直接指定して解析します。
        /// </summary>
        /// <param name="source">ソースコード</param>
        /// <returns>解析結果</returns>
        public KecaknoahLexResult AnalyzeFromSource(string source) => AnalyzeFromText(DefaultSourceName, source);

        /// <summary>
        /// ソースコードを直接指定して解析します。
        /// </summary>
        /// <param name="source">ソースコード</param>
        /// <returns>解析結果</returns>
        public KecaknoahLexResult AnalyzeFromSource(string source, string sourceName) => AnalyzeFromText(sourceName, source);

        /// <summary>
        /// ソース名とソースを指定して解析します。
        /// </summary>
        /// <param name="name">ソース名</param>
        /// <param name="source">解析対象のソースコード</param>
        /// <returns></returns>
        private KecaknoahLexResult AnalyzeFromText(string name, string source)
        {
            var result = new KecaknoahLexResult(name);
            var line = 0;
            var col = 0;
            var cq = "";
            Tuple<string, KecaknoahTokenType> kw;
            Match lm;
            KecaknoahLexError ei;
            while (source != "")
            {
                //空白論理行
                /*
                if (source.StartsWith(Environment.NewLine))
                {
                    source = source.Substring(Environment.NewLine.Length);
                    result.AddToken(new KecaknoahToken { Position = new Tuple<int, int>(line, col), TokenString = Environment.NewLine, Type = KecaknoahTokenType.NewLine });
                    line++;
                    col = 0;
                    continue;
                }
                if (source.StartsWith(";"))
                {
                    source = source.Substring(1);
                    result.AddToken(new KecaknoahToken { Position = new Tuple<int, int>(line, col), TokenString = ";", Type = KecaknoahTokenType.Semicolon });
                    col++;
                    continue;
                }
                */
                //コメント
                if ((cq = LineCommentStart.FirstOrDefault(p => source.StartsWith(p))) != null)
                {
                    source = source.Substring(cq.Length);
                    col += cq.Length;
                    var cl = source.IndexOf(Environment.NewLine);
                    if (cl >= 0)
                    {
                        source.Substring(cl + Environment.NewLine.Length);
                        line++;
                        col = 0;
                    }
                    else
                    {
                        //ラストコメント
                        source = "";
                    }
                    continue;
                }
                Tuple<string, string> mcq = null;
                if ((mcq = MultilineCommentQuotations.FirstOrDefault(p => source.StartsWith(p.Item1))) != null)
                {
                    source = source.Substring(mcq.Item1.Length);
                    col += mcq.Item1.Length;
                    var ce = source.IndexOf(mcq.Item2);
                    var ecs = source.IndexOf(mcq.Item1);
                    //不正な複数行コメント
                    if ((ecs >= 0 && ecs < ce) || ce < 0)
                    {
                        ei = new KecaknoahLexError
                        {
                            Column = col,
                            Line = line,
                            Message = "不正な複数行コメントです。コメントが終了していないか、入れ子になっています。"
                        };
                        result.Error = ei;
                        result.Success = false;
                        return result;
                    }
                    while (true)
                    {
                        ce = source.IndexOf(mcq.Item2);
                        var cl = source.IndexOf(Environment.NewLine);
                        if ((cl > 0 && ce < cl) || cl < 0)
                        {
                            source = source.Substring(ce + mcq.Item2.Length);
                            col += ce + mcq.Item2.Length;
                            break;
                        }
                        else
                        {
                            source = source.Substring(cl + Environment.NewLine.Length);
                            line++;
                            col = 0;
                        }
                    }
                    continue;
                }
                //空白
                if ((lm = WhitespacePattern.Match(source)).Success && lm.Index == 0)
                {
                    source = source.Substring(lm.Length);
                    col += lm.Length;
                    continue;
                }
                //演算子
                if ((kw = Operators.FirstOrDefault(p => source.StartsWith(p.Item1))) != null)
                {
                    source = source.Substring(kw.Item1.Length);
                    col += kw.Item1.Length;
                    result.AddToken(kw.CreateToken(col, line));
                    continue;
                }
                //識別子・キーワード
                if ((lm = IdentiferPattern.Match(source)).Success && lm.Index == 0)
                {
                    if ((kw = Keywords.FirstOrDefault(p => lm.Value == p.Item1)) != null)
                    {
                        source = source.Substring(kw.Item1.Length);
                        col += kw.Item1.Length;
                        result.AddToken(kw.CreateToken(col, line));
                    }
                    else
                    {
                        source = source.Substring(lm.Length);
                        col += lm.Length;
                        result.AddToken(lm.Value.CreateTokenAsIdentifer(col, line));
                    }
                    continue;
                }
                //リテラル
                if ((lm = BinaryNumberPattern.Match(source)).Success && lm.Index == 0)
                {
                    source = source.Substring(lm.Length);
                    col += lm.Length;
                    result.AddToken(lm.Value.CreateTokenAsBinaryNumber(col, line));
                    if (!(lm = IdentiferPattern.Match(source)).Success || lm.Index != 0) continue;
                }
                if ((lm = OctadecimalNumberPattern.Match(source)).Success && lm.Index == 0)
                {
                    source = source.Substring(lm.Length);
                    col += lm.Length;
                    result.AddToken(lm.Value.CreateTokenAsOctadecimalNumber(col, line));
                    if (!(lm = IdentiferPattern.Match(source)).Success || lm.Index != 0) continue;
                }
                if ((lm = HexadecimalNumberPattern.Match(source)).Success && lm.Index == 0)
                {
                    source = source.Substring(lm.Length);
                    col += lm.Length;
                    result.AddToken(lm.Value.CreateTokenAsHexadecimalNumber(col, line));
                    if (!(lm = IdentiferPattern.Match(source)).Success || lm.Index != 0) continue;
                }
                if ((lm = HexatridecimalNumberPattern.Match(source)).Success && lm.Index == 0)
                {
                    source = source.Substring(lm.Length);
                    col += lm.Length;
                    result.AddToken(lm.Value.CreateTokenAsHexatridecimalNumber(col, line));
                    if (!(lm = IdentiferPattern.Match(source)).Success || lm.Index != 0) continue;
                }
                if ((lm = DecimalNumberPattern.Match(source)).Success && lm.Index == 0)
                {
                    source = source.Substring(lm.Length);
                    col += lm.Length;
                    result.AddToken(lm.Value.CreateTokenAsDecimalNumber(col, line));
                    if (!(lm = IdentiferPattern.Match(source)).Success || lm.Index != 0) continue;
                }
                if ((cq = StringQuotation.FirstOrDefault(p => source.StartsWith(p))) != null)
                {
                    source = source.Substring(cq.Length);
                    col += cq.Length;
                    int qp = 0, eqp = 0, inp = 0;
                    var ls = "";
                    do
                    {
                        eqp = source.IndexOf("\\" + cq);
                        qp = source.IndexOf(cq);
                        inp = source.IndexOf(Environment.NewLine);
                        if (inp >= 0 && inp < qp)
                        {
                            ei = new KecaknoahLexError
                            {
                                Column = col,
                                Line = line,
                                Message = "文字列リテラル中に直接改行が含まれています。改行を表現したい場合、\\nを利用してください。"
                            };
                            result.Error = ei;
                            result.Success = false;
                            return result;
                        }
                        if (qp < 0)
                        {
                            ei = new KecaknoahLexError
                            {
                                Column = col,
                                Line = line,
                                Message = "文字列リテラルが閉じていません。"
                            };
                            result.Error = ei;
                            result.Success = false;
                            return result;
                        }
                        if (eqp >= 0 && qp - eqp == 1)
                        {
                            ls += source.Substring(0, qp + cq.Length);
                            source = source.Substring(qp + cq.Length);
                            col += qp + cq.Length - 1;
                            continue;
                        }
                        else
                        {
                            ls += source.Substring(0, qp);
                            source = source.Substring(qp + cq.Length);
                            foreach (var i in StringLiteralEscapes) ls = ls.Replace(i.Item1, i.Item2);
                            result.AddToken(new KecaknoahToken { Position = new Tuple<int, int>(col, line), TokenString = ls, Type = KecaknoahTokenType.StringLiteral });
                            col += qp + cq.Length;
                            break;
                        }
                    } while (true);
                    continue;
                }
                //不明
                ei = new KecaknoahLexError
                {
                    Column = col,
                    Line = line,
                    Message = "不正なトークンです。"
                };
                result.Error = ei;
                result.Success = false;
                return result;
            }
            result.Success = true;
            return result;
        }
    }

    internal static class Extensions
    {
        public static KecaknoahToken CreateToken(this Tuple<string, KecaknoahTokenType> tt, int col, int line)
            => new KecaknoahToken { Position = new Tuple<int, int>(col, line), TokenString = tt.Item1, Type = tt.Item2 };

        public static KecaknoahToken CreateTokenAsIdentifer(this string ls, int col, int line)
            => new KecaknoahToken { Position = new Tuple<int, int>(col, line), TokenString = ls, Type = KecaknoahTokenType.Identifer };

        public static KecaknoahToken CreateTokenAsDecimalNumber(this string ls, int col, int line)
            => new KecaknoahToken { Position = new Tuple<int, int>(col, line), TokenString = ls, Type = KecaknoahTokenType.DecimalNumberLiteral };

        public static KecaknoahToken CreateTokenAsBinaryNumber(this string ls, int col, int line)
            => new KecaknoahToken { Position = new Tuple<int, int>(col, line), TokenString = ls, Type = KecaknoahTokenType.BinaryNumberLiteral };

        public static KecaknoahToken CreateTokenAsOctadecimalNumber(this string ls, int col, int line)
            => new KecaknoahToken { Position = new Tuple<int, int>(col, line), TokenString = ls, Type = KecaknoahTokenType.OctadecimalNumberLiteral };

        public static KecaknoahToken CreateTokenAsHexadecimalNumber(this string ls, int col, int line)
            => new KecaknoahToken { Position = new Tuple<int, int>(col, line), TokenString = ls, Type = KecaknoahTokenType.HexadecimalNumberLiteral };

        public static KecaknoahToken CreateTokenAsHexatridecimalNumber(this string ls, int col, int line)
            => new KecaknoahToken { Position = new Tuple<int, int>(col, line), TokenString = ls, Type = KecaknoahTokenType.HexatridecimalNumberLiteral };
    }
}
