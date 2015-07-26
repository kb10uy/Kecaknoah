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
        protected internal void AddLocalNode(KecaknoahLocalAstNode node)
        {
            locals.Add(node);
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
    }

    /// <summary>
    /// 式のノードです。
    /// </summary>
    public class KecaknoahExpressionAstNode : KecaknoahAstNode
    {

    }

    /// <summary>
    /// 単項式ノード
    /// </summary>
    public class KecaknoahUnaryExpressionAstNode : KecaknoahAstNode
    {

    }

    /// <summary>
    /// 二項式ノード
    /// </summary>
    public class KecaknoahBinaryExpressionAstNode : KecaknoahAstNode
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
        /// 演算子のタイプを取得します。
        /// </summary>
        public KecaknoahOperatorType ExpressionType { get; protected internal set; }
    }

    /// <summary>
    /// 三項演算子ノード
    /// </summary>
    public class KecaknoahConditionalExpressionAstNode : KecaknoahAstNode
    {

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
    }
}
