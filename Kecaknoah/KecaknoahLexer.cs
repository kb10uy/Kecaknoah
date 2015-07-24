using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah
{
    /// <summary>
    /// Kecaknoahの字句解析器を定義します。
    /// </summary>
    public class KecaknoahLexer
    {
        #region static fields
        private static IOrderedEnumerable<Tuple<string, KecaknoahTokenType>> keywords = new List<Tuple<string, KecaknoahTokenType>>
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

        private static IOrderedEnumerable<Tuple<string, KecaknoahTokenType>> operators = new List<Tuple<string, KecaknoahTokenType>>
        {
            new Tuple<string, KecaknoahTokenType>("+", KecaknoahTokenType.ClassKeyword),
            new Tuple<string, KecaknoahTokenType>("-", KecaknoahTokenType.ClassKeyword),
            new Tuple<string, KecaknoahTokenType>("*", KecaknoahTokenType.ClassKeyword),
            new Tuple<string, KecaknoahTokenType>("/", KecaknoahTokenType.ClassKeyword),
            new Tuple<string, KecaknoahTokenType>("&", KecaknoahTokenType.ClassKeyword),
            new Tuple<string, KecaknoahTokenType>("|", KecaknoahTokenType.ClassKeyword),
            new Tuple<string, KecaknoahTokenType>("!", KecaknoahTokenType.ClassKeyword),
            new Tuple<string, KecaknoahTokenType>("^", KecaknoahTokenType.ClassKeyword),
            new Tuple<string, KecaknoahTokenType>("%", KecaknoahTokenType.ClassKeyword),
            new Tuple<string, KecaknoahTokenType>("=", KecaknoahTokenType.ClassKeyword),
            new Tuple<string, KecaknoahTokenType>("<<", KecaknoahTokenType.ClassKeyword),
            new Tuple<string, KecaknoahTokenType>(">>", KecaknoahTokenType.ClassKeyword),
            new Tuple<string, KecaknoahTokenType>("==", KecaknoahTokenType.ClassKeyword),
            new Tuple<string, KecaknoahTokenType>("!=", KecaknoahTokenType.ClassKeyword),
            new Tuple<string, KecaknoahTokenType>(">", KecaknoahTokenType.ClassKeyword),
            new Tuple<string, KecaknoahTokenType>("<", KecaknoahTokenType.ClassKeyword),
            new Tuple<string, KecaknoahTokenType>(">=", KecaknoahTokenType.ClassKeyword),
            new Tuple<string, KecaknoahTokenType>("<=", KecaknoahTokenType.ClassKeyword),
            new Tuple<string, KecaknoahTokenType>("~=", KecaknoahTokenType.ClassKeyword),
            new Tuple<string, KecaknoahTokenType>("&&", KecaknoahTokenType.ClassKeyword),
            new Tuple<string, KecaknoahTokenType>("||", KecaknoahTokenType.ClassKeyword),
            new Tuple<string, KecaknoahTokenType>("+=", KecaknoahTokenType.ClassKeyword),
            new Tuple<string, KecaknoahTokenType>("-=", KecaknoahTokenType.ClassKeyword),
            new Tuple<string, KecaknoahTokenType>("*=", KecaknoahTokenType.ClassKeyword),
            new Tuple<string, KecaknoahTokenType>("/=", KecaknoahTokenType.ClassKeyword),
            new Tuple<string, KecaknoahTokenType>("&=", KecaknoahTokenType.ClassKeyword),
            new Tuple<string, KecaknoahTokenType>("|=", KecaknoahTokenType.ClassKeyword),
            new Tuple<string, KecaknoahTokenType>("^=", KecaknoahTokenType.ClassKeyword),
            new Tuple<string, KecaknoahTokenType>("%=", KecaknoahTokenType.ClassKeyword),
            new Tuple<string, KecaknoahTokenType>("<<=", KecaknoahTokenType.ClassKeyword),
            new Tuple<string, KecaknoahTokenType>(">>=", KecaknoahTokenType.ClassKeyword),
            new Tuple<string, KecaknoahTokenType>("?:", KecaknoahTokenType.ClassKeyword),
            new Tuple<string, KecaknoahTokenType>("||=", KecaknoahTokenType.ClassKeyword),
            new Tuple<string, KecaknoahTokenType>(",", KecaknoahTokenType.ClassKeyword),
            new Tuple<string, KecaknoahTokenType>(".", KecaknoahTokenType.ClassKeyword),
        }.OrderByDescending(p => p.Item1.Length).ThenBy(p => p.Item1);
        #endregion

        #region properties
        /// <summary>
        /// エラーや警告などの情報を出力するTextWriterを取得・設定します。
        /// </summary>
        public TextWriter OutputWriter { get; set; }
        #endregion

        public KecaknoahLexer()
        {
            OutputWriter = Console.Out;
        }

    }
}
