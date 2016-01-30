using System;

namespace Kecaknoah.Analyze
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
        EndClassKeyword,
        /// <summary>
        /// static
        /// </summary>
        StaticKeyword,
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
        /// then
        /// </summary>
        ThenKeyword,
        /// <summary>
        /// else
        /// </summary>
        ElseKeyword,
        /// <summary>
        /// endif
        /// </summary>
        EndIfKeyword,
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
        EndCaseKeyword,
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
        /// foreach
        /// </summary>
        ForeachKeyword,
        /// <summary>
        /// in
        /// </summary>
        InKeyword,
        /// <summary>
        /// of
        /// </summary>
        OfKeyword,
        /// <summary>
        /// local
        /// </summary>
        LocalKeyword,
        /// <summary>
        /// self
        /// </summary>
        SelfKeyword,
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
        /// <summary>
        /// VARGS
        /// </summary>
        VargsKeyword,
        /// <summary>
        /// costart
        /// </summary>
        CoroutineKeyword,
        /// <summary>
        /// coresume
        /// </summary>
        CoresumeKeyword,

        /// <summary>
        /// use
        /// </summary>
        UseKeyword,

        /// <summary>
        /// (
        /// </summary>
        ParenStart,
        /// <summary>
        /// )
        /// </summary>
        ParenEnd,
        /// <summary>
        /// {
        /// </summary>
        BraceStart,
        /// <summary>
        /// }
        /// </summary>
        BraceEnd,
        /// <summary>
        /// [
        /// </summary>
        BracketStart,
        /// <summary>
        /// ]
        /// </summary>
        BracketEnd,

        /// <summary>
        /// +
        /// </summary>
        Plus,
        /// <summary>
        /// -
        /// </summary>
        Minus,
        /// <summary>
        /// *
        /// </summary>
        Multiply,
        /// <summary>
        /// /
        /// </summary>
        Divide,
        /// <summary>
        /// &amp;
        /// </summary>
        And,
        /// <summary>
        /// |
        /// </summary>
        Or,
        /// <summary>
        /// !
        /// </summary>
        Not,
        /// <summary>
        /// ^
        /// </summary>
        Xor,
        /// <summary>
        /// %
        /// </summary>
        Modular,
        /// <summary>
        /// =
        /// </summary>
        Assign,
        /// <summary>
        /// &lt;&lt;
        /// </summary>
        LeftBitShift,
        /// <summary>
        /// &gt;&gt;
        /// </summary>
        RightBitShift,
        /// <summary>
        /// ==
        /// </summary>
        Equal,
        /// <summary>
        /// !=
        /// </summary>
        NotEqual,
        /// <summary>
        /// &gt;
        /// </summary>
        Greater,
        /// <summary>
        /// &lt;
        /// </summary>
        Lesser,
        /// <summary>
        /// &gt;=
        /// </summary>
        GreaterEqual,
        /// <summary>
        /// &lt;=
        /// </summary>
        LesserEqual,
        /// <summary>
        /// ~=
        /// </summary>
        SpecialEqual,
        /// <summary>
        /// &amp;&amp;
        /// </summary>
        AndAlso,
        /// <summary>
        /// ||
        /// </summary>
        OrElse,
        /// <summary>
        /// +=
        /// </summary>
        PlusAssign,
        /// <summary>
        /// -=
        /// </summary>
        MinusAssign,
        /// <summary>
        /// *=
        /// </summary>
        MultiplyAssign,
        /// <summary>
        /// /=
        /// </summary>
        DivideAssign,
        /// <summary>
        /// &amp;=
        /// </summary>
        AndAssign,
        /// <summary>
        /// |=
        /// </summary>
        OrAssign,
        /// <summary>
        /// ^=
        /// </summary>
        XorAssign,
        /// <summary>
        /// %=
        /// </summary>
        ModularAssign,
        /// <summary>
        /// &lt;&lt;=
        /// </summary>
        LeftBitShiftAssign,
        /// <summary>
        /// &gt;&gt;=
        /// </summary>
        RightBitShiftAssign,
        /// <summary>
        /// ++
        /// </summary>
        Increment,
        /// <summary>
        /// --
        /// </summary>
        Decrement,
        /// <summary>
        /// ?
        /// </summary>
        Question,
        /// <summary>
        /// :
        /// </summary>
        Colon,
        /// <summary>
        /// ||=
        /// </summary>
        NilAssign,

        /// <summary>
        /// ,
        /// </summary>
        Comma,
        /// <summary>
        /// .
        /// </summary>
        Period,

        /// <summary>
        /// 改行
        /// </summary>
        NewLine,
        /// <summary>
        /// ;
        /// </summary>
        Semicolon,

        /// <summary>
        /// ...
        /// </summary>
        VariableArguments,

        /// <summary>
        /// return
        /// </summary>
        ReturnKeyword,
        /// <summary>
        /// yield
        /// </summary>
        YieldKeyword,
        /// <summary>
        /// lambda
        /// </summary>
        Lambda,
        /// <summary>
        /// λ
        /// </summary>
        LambdaStart,
    }
}
