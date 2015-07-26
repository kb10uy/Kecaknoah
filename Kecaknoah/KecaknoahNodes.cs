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
        public IReadOnlyList<KecaknoahFunctionAstNode> Functions { get; private set; }

        private List<KecaknoahLocalAstNode> locals = new List<KecaknoahLocalAstNode>();
        /// <summary>
        /// local宣言のノードのリストを取得します。
        /// </summary>
        public IReadOnlyList<KecaknoahLocalAstNode> Locals { get; private set; }

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
    }

    /// <summary>
    /// local宣言のノードです。
    /// </summary>
    public class KecaknoahLocalAstNode : KecaknoahAstNode
    {
        /// <summary>
        /// 変数名を取得します。
        /// </summary>
        public string Name { get; protected internal set; }

        /// <summary>
        /// 初期値定義がある場合はその式のノードが格納されます。
        /// </summary>
        public KecaknoahExpressionNode InitialExpression { get; protected internal set; }
    }

    /// <summary>
    /// 式のノードです。
    /// </summary>
    public class KecaknoahExpressionNode : KecaknoahAstNode
    {

    }
}
