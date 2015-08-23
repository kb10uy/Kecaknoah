using System.Collections.Generic;
using System.Linq;

namespace Kecaknoah.Analyze
{
    /// <summary>
    /// 式のノードです。
    /// </summary>
    public class KecaknoahExpressionAstNode : KecaknoahAstNode
    {
        /// <summary>
        /// 演算子のタイプを取得します。
        /// </summary>
        public KecaknoahOperatorType ExpressionType { get; protected internal set; }

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        protected internal KecaknoahExpressionAstNode()
        {
            Type = KecaknoahAstNodeType.Expression;
        }
    }

    /// <summary>
    /// 単項式ノード
    /// </summary>
    public class KecaknoahFactorExpressionAstNode : KecaknoahPrimaryExpressionAstNode
    {
        /// <summary>
        /// 因子の種類を取得します。
        /// </summary>
        public KecaknoahFactorType FactorType { get; protected internal set; }

        /// <summary>
        /// boolリテラルの内容を取得します。
        /// </summary>
        public bool BooleanValue { get; protected internal set; }

        /// <summary>
        /// 識別子名・文字列リテラルの内容を取得します。
        /// </summary>
        public string StringValue { get; protected internal set; }

        /// <summary>
        /// 整数リテラルの内容を取得します。
        /// </summary>
        public long IntegerValue { get; protected internal set; }

        /// <summary>
        /// 単精度リテラルの内容を取得します。
        /// </summary>
        public float SingleValue { get; protected internal set; }

        /// <summary>
        /// 倍精度リテラルの内容を取得します。
        /// </summary>
        public double DoubleValue { get; protected internal set; }

        /// <summary>
        /// カッコ式のノードを取得します。
        /// </summary>
        public KecaknoahExpressionAstNode ExpressionNode { get; protected internal set; }

        /// <summary>
        /// 知るか
        /// </summary>
        public KecaknoahFactorExpressionAstNode() : base() { }

        /// <summary>
        /// 現在のオブジェクトを表す文字列を返します。
        /// </summary>
        /// <returns>文字列</returns>
        public override IReadOnlyList<string> ToDebugStringList()
        {
            var result = new List<string>();
            switch (FactorType)
            {
                case KecaknoahFactorType.Nil:
                    result.Add("Nil: Nil");
                    break;
                case KecaknoahFactorType.BooleanValue:
                    result.Add($"Boolean: {BooleanValue}");
                    break;
                case KecaknoahFactorType.IntegerValue:
                    result.Add($"Integer: {IntegerValue}");
                    break;
                case KecaknoahFactorType.SingleValue:
                    result.Add($"Single: {SingleValue}");
                    break;
                case KecaknoahFactorType.DoubleValue:
                    result.Add($"Double: {DoubleValue}");
                    break;
                case KecaknoahFactorType.StringValue:
                    result.Add($"String: {StringValue}");
                    break;
                case KecaknoahFactorType.Identifer:
                    result.Add($"Identifer: {StringValue}");
                    break;
                case KecaknoahFactorType.ParenExpression:
                    result.AddRange(ExpressionNode.ToDebugStringList());
                    break;
                case KecaknoahFactorType.VariableArguments:
                    result.Add($"VariableArguments");
                    break;
            }
            return result;
        }
    }

    /// <summary>
    /// 単項式ノード
    /// </summary>
    public class KecaknoahPrimaryExpressionAstNode : KecaknoahUnaryExpressionAstNode
    {
        /// <summary>
        /// 初期化
        /// </summary>
        protected internal KecaknoahPrimaryExpressionAstNode() : base()
        {

        }
    }

    /// <summary>
    /// 単項式(インクリメント、デクリメントを含む)ノード
    /// </summary>
    public class KecaknoahUnaryExpressionAstNode : KecaknoahExpressionAstNode
    {
        /// <summary>
        /// 対象の一次式を取得します。
        /// </summary>
        public KecaknoahPrimaryExpressionAstNode Target { get; protected internal set; }

        /// <summary>
        /// 現在のオブジェクトを表す文字列を返します。
        /// </summary>
        /// <returns>文字列</returns>
        public override IReadOnlyList<string> ToDebugStringList()
        {
            var result = new List<string>();
            result.Add($"Unary: {ExpressionType}");
            result.AddRange(Target.ToDebugStringList().Select(p => "| " + p));
            return result;
        }
    }

    /// <summary>
    /// アクセサノード
    /// </summary>
    public class KecaknoahMemberAccessExpressionAstNode : KecaknoahPrimaryExpressionAstNode
    {
        /// <summary>
        /// 初期化します。
        /// </summary>
        protected internal KecaknoahMemberAccessExpressionAstNode() : base()
        {
            ExpressionType = KecaknoahOperatorType.MemberAccess;
        }

        /// <summary>
        /// 対象のメンバー名を取得します。
        /// </summary>
        public string MemberName { get; protected internal set; } = "";

        /// <summary>
        /// 現在のオブジェクトを表す文字列を返します。
        /// </summary>
        /// <returns>文字列</returns>
        public override IReadOnlyList<string> ToDebugStringList()
        {
            var result = new List<string>();
            if (ExpressionType == KecaknoahOperatorType.IndexerAccess)
            {
                MemberName = "{Indexer}";
            }
            result.Add($"Member Access: {MemberName}");
            result.Add("| [Target]");
            result.AddRange(Target.ToDebugStringList().Select(p => "| " + p));
            return result;
        }
    }

    /// <summary>
    /// メソッド呼び出しノード
    /// </summary>
    public class KecaknoahArgumentCallExpressionAstNode : KecaknoahPrimaryExpressionAstNode
    {

        /// <summary>
        /// 呼び出し時の引数リストを取得します。
        /// </summary>
        public IList<KecaknoahExpressionAstNode> Arguments { get; protected internal set; } = new List<KecaknoahExpressionAstNode>();

        /// <summary>
        /// 初期化します。
        /// </summary>
        protected internal KecaknoahArgumentCallExpressionAstNode() : base()
        {
            ExpressionType = KecaknoahOperatorType.FunctionCall;
        }

        /// <summary>
        /// 現在のオブジェクトを表す文字列を返します。
        /// </summary>
        /// <returns>文字列</returns>
        public override IReadOnlyList<string> ToDebugStringList()
        {
            var result = new List<string>();
            result.Add($"Function Call:");
            result.Add("| [Target]");
            result.AddRange(Target.ToDebugStringList().Select(p => "| " + p));
            if (Arguments.Count > 0)
            {
                result.Add("| [Arguments]");
                foreach (var i in Arguments) result.AddRange(i.ToDebugStringList().Select(p => "| " + p));
            }
            return result;
        }
    }

    /// <summary>
    /// 二項式ノード
    /// </summary>
    public class KecaknoahBinaryExpressionAstNode : KecaknoahExpressionAstNode
    {
        /// <summary>
        /// 1項目の式ノードを取得します。
        /// </summary>
        public KecaknoahExpressionAstNode FirstNode { get; protected internal set; }

        /// <summary>
        /// 1項目の式ノードを取得します。
        /// </summary>
        public KecaknoahExpressionAstNode SecondNode { get; protected internal set; }

        /// <summary>
        /// 現在のオブジェクトを表す文字列を返します。
        /// </summary>
        /// <returns>文字列</returns>
        public override IReadOnlyList<string> ToDebugStringList()
        {
            var result = new List<string>();
            result.Add($"{ExpressionType}:");
            result.AddRange(FirstNode.ToDebugStringList().Select(p => "| " + p));
            result.AddRange(SecondNode.ToDebugStringList().Select(p => "| " + p));
            return result;
        }
    }

    /// <summary>
    /// 三項演算子ノード
    /// </summary>
    public class KecaknoahConditionalExpressionAstNode : KecaknoahExpressionAstNode
    {
        /// <summary>
        /// 条件式
        /// </summary>
        public KecaknoahExpressionAstNode ConditionNode { get; protected internal set; }

        /// <summary>
        /// trueの場合の式
        /// </summary>
        public KecaknoahExpressionAstNode TrueValueNode { get; protected internal set; }

        /// <summary>
        /// falseの場合の式
        /// </summary>
        public KecaknoahExpressionAstNode FalseValueNode { get; protected internal set; }

        /// <summary>
        /// 現在のオブジェクトを表す文字列を返します。
        /// </summary>
        /// <returns>文字列</returns>
        public override IReadOnlyList<string> ToDebugStringList()
        {
            var result = new List<string>();
            result.Add("Conditional:");
            result.Add("| [Condition]");
            result.AddRange(ConditionNode.ToDebugStringList().Select(p => "| " + p));
            result.Add("| [True Expression]");
            result.AddRange(TrueValueNode.ToDebugStringList().Select(p => "| " + p));
            result.Add("| [False Expression]");
            result.AddRange(FalseValueNode.ToDebugStringList().Select(p => "| " + p));
            return result;
        }
    }

    /// <summary>
    /// 因子の種類を定義します。
    /// </summary>
    public enum KecaknoahFactorType
    {
        /// <summary>
        /// 未定義
        /// </summary>
        Undefined,
        /// <summary>
        /// カッコ式
        /// </summary>
        ParenExpression,
        /// <summary>
        /// nil
        /// </summary>
        Nil,
        /// <summary>
        /// boolリテラル
        /// </summary>
        BooleanValue,
        /// <summary>
        /// 単精度リテラル
        /// </summary>
        SingleValue,
        /// <summary>
        /// 倍精度リテラル
        /// </summary>
        DoubleValue,
        /// <summary>
        /// 整数リテラル
        /// </summary>
        IntegerValue,
        /// <summary>
        /// 文字列リテラル
        /// </summary>
        StringValue,
        /// <summary>
        /// 識別子
        /// </summary>
        Identifer,
        /// <summary>
        /// 可変長引数
        /// </summary>
        VariableArguments,
        /// <summary>
        /// coresume文
        /// </summary>
        CoroutineResume,
    }

    /// <summary>
    /// 演算子の種類を定義します。
    /// </summary>
    public enum KecaknoahOperatorType
    {
        /// <summary>
        /// 未定義
        /// </summary>
        Undefined,
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
        ConditionalQuestion,
        /// <summary>
        /// :
        /// </summary>
        ConditionalElse,
        /// <summary>
        /// ||=
        /// </summary>
        NilAssign,

        /// <summary>
        /// .
        /// </summary>
        MemberAccess,
        /// <summary>
        /// ()
        /// </summary>
        FunctionCall,
        /// <summary>
        /// []
        /// </summary>
        IndexerAccess,
    }
}
