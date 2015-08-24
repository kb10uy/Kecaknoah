using System.Collections.Generic;
using System.Linq;

namespace Kecaknoah.Analyze
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

        /// <summary>
        /// 現在のオブジェクトを表す文字列を返します。
        /// </summary>
        /// <returns>文字列</returns>
        public override IReadOnlyList<string> ToDebugStringList()
        {
            var result = new List<string>();
            result.Add($"Class: {Name}");
            result.Add("| [Locals]");
            foreach (var i in Locals) result.AddRange(i.ToDebugStringList().Select(p => "| " + p));
            result.Add("| [Methods]");
            foreach (var i in Functions) result.AddRange(i.ToDebugStringList().Select(p => "| " + p));
            return result;
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
        /// このメソッドがクラスメソッドであるかどうかを取得します。
        /// </summary>
        public bool StaticMethod { get; protected internal set; }

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        protected internal KecaknoahFunctionAstNode()
        {
            Type = KecaknoahAstNodeType.Function;
        }

        /// <summary>
        /// 現在のオブジェクトを表す文字列を返します。
        /// </summary>
        /// <returns>文字列</returns>
        public override IReadOnlyList<string> ToDebugStringList()
        {
            var result = new List<string>();
            result.Add($"Method: {Name}");
            if (Parameters.Count > 0)
            {
                result.Add("| [Parameters]");
                foreach (var i in Parameters) result.Add($"| {i.ToString()}");
            }
            result.Add("| [Block]");
            foreach (var i in Children) result.AddRange(i.ToDebugStringList().Select(p => "| " + p));
            return result;
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

        /// <summary>
        /// 現在のオブジェクトを表す文字列を返します。
        /// </summary>
        /// <returns>文字列</returns>
        public override IReadOnlyList<string> ToDebugStringList()
        {
            var result = new List<string>();
            result.Add($"Local Assign: \"{Name}\"");
            if (InitialExpression != null)
            {
                result.Add("| [Initial Expression]");
                result.AddRange(InitialExpression.ToDebugStringList().Select(p => "| " + p));
            }
            return result;
        }
    }

    /// <summary>
    /// return/yield文のノードです。
    /// </summary>
    public class KecaknoahReturnAstNode : KecaknoahAstNode
    {
        /// <summary>
        /// 返り値がある場合はその式のノードが格納されます。
        /// </summary>
        public KecaknoahExpressionAstNode Value { get; protected internal set; }

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        protected internal KecaknoahReturnAstNode()
        {
            Type = KecaknoahAstNodeType.ReturnStatement;
        }

        /// <summary>
        /// 現在のオブジェクトを表す文字列を返します。
        /// </summary>
        /// <returns>文字列</returns>
        public override IReadOnlyList<string> ToDebugStringList()
        {
            var result = new List<string>();
            result.Add("Return");
            if (Value != null)
            {
                result.Add("| [Expression]");
                result.AddRange(Value.ToDebugStringList().Select(p => "| " + p));
            }
            return result;
        }
    }

    /// <summary>
    /// local宣言のノードです。
    /// </summary>
    public class KecaknoahCoroutineDeclareAstNode : KecaknoahAstNode
    {
        /// <summary>
        /// 変数名を取得します。
        /// </summary>
        public string Name { get; protected internal set; } = "";

        /// <summary>
        /// コルーチンのノードを取得します。
        /// </summary>
        public KecaknoahExpressionAstNode InitialExpression { get; protected internal set; }

        /// <summary>
        /// 引数がある場合はその式のノードが格納されます。
        /// </summary>
        public IList<KecaknoahExpressionAstNode> ParameterExpressions { get; } = new List<KecaknoahExpressionAstNode>();

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        protected internal KecaknoahCoroutineDeclareAstNode()
        {
            Type = KecaknoahAstNodeType.CoroutineStatement;
        }

        /// <summary>
        /// 現在のオブジェクトを表す文字列を返します。
        /// </summary>
        /// <returns>文字列</returns>
        public override IReadOnlyList<string> ToDebugStringList()
        {
            var result = new List<string>();
            result.Add($"Coroutine Declare: \"{Name}\"");
            if (InitialExpression != null)
            {
                result.Add("| [Expression]");
                result.AddRange(InitialExpression.ToDebugStringList().Select(p => "| " + p));
            }
            return result;
        }
    }

    /// <summary>
    /// if文のノードです。
    /// </summary>
    public class KecaknoahIfAstNode : KecaknoahAstNode
    {
        /// <summary>
        /// ifブロックのノードを取得します。
        /// </summary>
        public KecaknoahIfBlockAstNode IfBlock { get; protected internal set; }

        /// <summary>
        /// elifブロックのノードのリストを取得します。
        /// </summary>
        public IList<KecaknoahIfBlockAstNode> ElifBlocks { get; protected internal set; } = new List<KecaknoahIfBlockAstNode>();

        /// <summary>
        /// elseブロックのノードを取得します。
        /// </summary>
        public KecaknoahIfBlockAstNode ElseBlock { get; protected internal set; }

        /// <summary>
        /// 現在のオブジェクトを表す文字列を返します。
        /// </summary>
        /// <returns>文字列</returns>
        public override IReadOnlyList<string> ToDebugStringList()
        {
            var result = new List<string>();
            result.Add("If Statement");
            result.Add("| [If Block]");
            result.AddRange(IfBlock.ToDebugStringList().Select(p => "| " + p));
            foreach (var i in ElifBlocks)
            {
                result.Add("| [Elif Block]");
                result.AddRange(i.ToDebugStringList().Select(p => "| " + p));
            }
            if (ElseBlock != null)
            {
                result.Add("| [Else Block]");
                result.AddRange(IfBlock.ToDebugStringList().Select(p => "| " + p));
            }
            return result;
        }
    }

    /// <summary>
    /// if文内の各ブロックのノード(if/elif/else)です。
    /// 実行内容は<see cref="KecaknoahAstNode.Children"/>に格納されます。
    /// </summary>
    public class KecaknoahIfBlockAstNode : KecaknoahAstNode
    {
        /// <summary>
        /// 条件式のノードを取得します。
        /// <see cref="KecaknoahIfAstNode.ElseBlock"/>に格納される場合は利用されません。
        /// </summary>
        public KecaknoahExpressionAstNode Condition { get; protected internal set; }

        /// <summary>
        /// 現在のオブジェクトを表す文字列を返します。
        /// </summary>
        /// <returns>文字列</returns>
        public override IReadOnlyList<string> ToDebugStringList()
        {
            var result = new List<string>();
            result.Add("If Block");
            result.Add("| [Condition]");
            result.AddRange(Condition.ToDebugStringList().Select(p => "| " + p));
            result.Add("| [Block]");
            foreach (var i in Children)
            {
                result.AddRange(i.ToDebugStringList().Select(p => "| " + p));
            }
            return result;
        }
    }

    /// <summary>
    /// 繰り返し文だよ
    /// </summary>
    public class KecaknoahLoopAstNode : KecaknoahAstNode
    {
        /// <summary>
        /// 条件式を取得します。
        /// </summary>
        public KecaknoahExpressionAstNode Condition { get; protected internal set; }

        /// <summary>
        /// 
        /// </summary>
        public KecaknoahLoopAstNode()
        {
            Type = KecaknoahAstNodeType.WhileStatement;
        }
    }

    /// <summary>
    /// for文です
    /// </summary>
    public class KecaknoahForAstNode : KecaknoahLoopAstNode
    {
        /// <summary>
        /// 初期化式を取得します。
        /// </summary>
        public IList<KecaknoahExpressionAstNode> InitializeExpressions { get; } = new List<KecaknoahExpressionAstNode>();

        /// <summary>
        /// カウンタ操作の式を取得します。
        /// </summary>
        public IList<KecaknoahExpressionAstNode> CounterExpressions { get; } = new List<KecaknoahExpressionAstNode>();

        /// <summary>
        /// 
        /// </summary>
        public KecaknoahForAstNode()
        {
            Type = KecaknoahAstNodeType.ForStatement;
        }
    }

    /// <summary>
    /// continue文
    /// </summary>
    public class KecaknoahContinueAstNode : KecaknoahAstNode
    {
        /// <summary>
        /// ジャンプ先ラベル名を取得します。
        /// </summary>
        public string Label { get; protected internal set; }

        /// <summary>
        /// 
        /// </summary>
        public KecaknoahContinueAstNode()
        {
            Type = KecaknoahAstNodeType.ContinueStatement;
        }
    }
}