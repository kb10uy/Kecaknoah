using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah
{
    /// <summary>
    /// KecaknoahLexerで使用するトークンを定義します。
    /// </summary>
    public struct KecaknoahToken
    {
        /// <summary>
        /// このトークンの種類を定義します。
        /// </summary>
        public KecaknoahTokenType Type;
        /// <summary>
        /// このトークンの実際の文字列を定義します。
        /// </summary>
        public string TokenString;
        /// <summary>
        /// このトークンのソースコード上の位置を定義します。
        /// </summary>
        public Tuple<int, int> Position;
    }

    /// <summary>
    /// KecaknoahLexerで使用するトークンの種類を定義します。
    /// </summary>
    public enum KecaknoahTokenType
    {
        /// <summary>
        /// 未定義
        /// </summary>
        Undefined,
        /// <summary>
        /// [\w][\w0-9]+で定義される識別子
        /// </summary>
        Identifer,
        /// <summary>
        /// 10進数値リテラル
        /// </summary>
        DecimalNumberLiteral,
        /// <summary>
        /// 2進数値リテラル
        /// </summary>
        BinaryNumberLiteral,
        /// <summary>
        /// 8進数値リテラル
        /// </summary>
        OctadecimalNumberLiteral,
        /// <summary>
        /// 16進数値リテラル
        /// </summary>
        HexadecimalNumberLiteral,
        /// <summary>
        /// 36進数値リテラル
        /// </summary>
        HexatridecimalNumberLiteral,
        /// <summary>
        /// 文字列リテラル
        /// </summary>
        StringLiteral,

        /// <summary>
        /// class
        /// </summary>
        ClassKeyword,
        /// <summary>
        /// endclass
        /// </summary>
        EndclassKeyword,
        /// <summary>
        /// func
        /// </summary>
        FuncKeyword,
        /// <summary>
        /// endfunc
        /// </summary>
        EndFuncKeyword,
        /// <summary>
        /// if
        /// </summary>
        IfKeyword,
        /// <summary>
        /// elif
        /// </summary>
        ElifKeyword,
        /// <summary>
        /// else
        /// </summary>
        ElseKeyword,
        /// <summary>
        /// endif
        /// </summary>
        EndifKeyword,
        /// <summary>
        /// case
        /// </summary>
        CaseKeyword,
        /// <summary>
        /// when
        /// </summary>
        WhenKeyword,
        /// <summary>
        /// default
        /// </summary>
        DefaultKeyword,
        /// <summary>
        /// endcase
        /// </summary>
        EndcaseKeyword,
        /// <summary>
        /// for
        /// </summary>
        ForKeyword,
        /// <summary>
        /// continue
        /// </summary>
        ContinueKeyword,
        /// <summary>
        /// break
        /// </summary>
        BreakKeyword,
        /// <summary>
        /// next
        /// </summary>
        NextKeyword,
        /// <summary>
        /// while
        /// </summary>
        WhileKeyword,
        /// <summary>
        /// do
        /// </summary>
        DoKeyword,
        /// <summary>
        /// local
        /// </summary>
        LocalKeyword,
        /// <summary>
        /// true
        /// </summary>
        TrueKeyword,
        /// <summary>
        /// false
        /// </summary>
        FalseKeyword,
        /// <summary>
        /// nil
        /// </summary>
        NilKeyword,


        ParenStart,
        ParenEnd,
        BraceStart,
        BraceEnd,
        BracketStart,
        BracketEnd,

        Plus,
        Minus,
        Multiply,
        Divide,
        And,
        Or,
        Not,
        Xor,
        Modular,
        Assign,
        LeftBitShift,
        RightBitShift,
        Equal,
        NotEqual,
        Greater,
        Lesser,
        GreaterEqual,
        LesserEqual,
        SpecialEqual,
        AndAlso,
        OrElse,
        PlusAssign,
        MinusAssign,
        MultiplyAssign,
        DivideAssign,
        AndAssign,
        OrAssign,
        XorAssign,
        ModularAssign,
        LeftBitShiftAssign,
        RightBitShiftAssign,
        ConditionalQuestion,
        ConditionalElse,
        NilAssign,

        Comma,
        Period,

        NewLine,
        Semicolon,
    }
}
