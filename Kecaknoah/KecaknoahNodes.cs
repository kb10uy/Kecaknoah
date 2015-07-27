using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah
{
    /// <summary>
    /// クラスのノードです。
    /// </summary>
    public class KecaknoahClassAstNode : KecaknoahAstNode
    {
        /// <summary>
        /// クラス名を取得します。
        /// </summary>
        public string Name { get; protected internal set; } = "";

        private List<KecaknoahFunctionAstNode> funcs = new List<KecaknoahFunctionAstNode>();
        /// <summary>
        /// メソッドのノードのリストを取得します。
        /// </summary>
        public IReadOnlyList<KecaknoahFunctionAstNode> Functions { get; }

        private List<KecaknoahLocalAstNode> locals = new List<KecaknoahLocalAstNode>();
        /// <summary>
        /// local宣言のノードのリストを取得します。
        /// </summary>
        public IReadOnlyList<KecaknoahLocalAstNode> Locals { get; }

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        public KecaknoahClassAstNode()
        {
            Functions = funcs;
            Locals = locals;
            Type = KecaknoahAstNodeType.Class;
        }

        /// <summary>
        /// メソッドのノードを追加します。
        /// </summary>
        /// <param name="node">メソッドノード</param>
        protected internal void AddFunctionNode(KecaknoahFunctionAstNode node)
        {
            funcs.Add(node);
            AddNode(node);
        }

        /// <summary>
        /// local宣言のノードを追加します。
        /// </summary>
        /// <param name="node">localノード</param>
        protected internal void AddLocalNode(IEnumerable<KecaknoahLocalAstNode> node)
        {
            locals.AddRange(node);
            AddNode(node);
        }
    }

    /// <summary>
    /// メソッドのノードです。
    /// </summary>
    public class KecaknoahFunctionAstNode : KecaknoahAstNode
    {
        /// <summary>
        /// メソッド名を取得します。
        /// </summary>
        public string Name { get; protected internal set; } = "";

        /// <summary>
        /// 仮引数リストを取得します。
        /// </summary>
        public IList<string> Parameters { get; } = new List<string>();

        /// <summary>
        /// このメソッドが可変長引数であるかどうかを取得します。
        /// </summary>
        public bool AllowsVariableArguments { get; protected internal set; }

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        protected internal KecaknoahFunctionAstNode()
        {
            Type = KecaknoahAstNodeType.Function;
        }
    }

    /// <summary>
    /// local宣言のノードです。
    /// </summary>
    public class KecaknoahLocalAstNode : KecaknoahAstNode
    {
        /// <summary>
        /// 変数名を取得します。
        /// </summary>
        public string Name { get; protected internal set; } = "";

        /// <summary>
        /// 初期値定義がある場合はその式のノードが格納されます。
        /// </summary>
        public KecaknoahExpressionAstNode InitialExpression { get; protected internal set; }

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        protected internal KecaknoahLocalAstNode()
        {
            Type = KecaknoahAstNodeType.LocalStatement;
        }
    }

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
        /// 知るか
        /// </summary>
        public KecaknoahFactorExpressionAstNode() : base() { }
    }

    /// <summary>
    /// 単項式ノード
    /// </summary>
    public class KecaknoahPrimaryExpressionAstNode : KecaknoahExpressionAstNode
    {
        /// <summary>
        /// 対象の一次式を取得します。
        /// </summary>
        public KecaknoahPrimaryExpressionAstNode Target { get; protected internal set; }

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
            ExpressionType = KecaknoahOperatorType.MemberAccess;
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
        public KecaknoahUnaryExpressionAstNode FirstNode { get; protected internal set; }

        /// <summary>
        /// 1項目の式ノードを取得します。
        /// </summary>
        public KecaknoahUnaryExpressionAstNode SecondNode { get; protected internal set; }
    }

    /// <summary>
    /// 三項演算子ノード
    /// </summary>
    public class KecaknoahConditionalExpressionAstNode : KecaknoahAstNode
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
