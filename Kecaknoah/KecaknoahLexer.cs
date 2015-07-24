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
