using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah
{
    /// <summary>
    /// KecaknoahのAST(抽象構文木)を提供します。
    /// </summary>
    public sealed class KecaknoahAst
    {
        /// <summary>
        /// <see cref="KecaknoahLexResult.SourceName"/>から継承されたソース名を取得します。
        /// </summary>
        public string SourceName { get; private set; }

        /// <summary>
        /// 構文解析が成功した場合はtrueになります。
        /// </summary>
        public bool Success { get; internal set; } = false;

        /// <summary>
        /// 解析が失敗した場合のエラー情報を取得します。
        /// </summary>
        public KecaknoahError Error { get; internal set; } = null;

        /// <summary>
        /// ASTのルートを取得します。
        /// </summary>
        public KecaknoahAstNode RootNode { get; internal set; }

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        /// <param name="name">ソース名</param>
        public KecaknoahAst(string name)
        {
            SourceName = name;
        }


    }

    /// <summary>
    /// ASTのノードを定義します。
    /// </summary>
    public class KecaknoahAstNode
    {
        /// <summary>
        /// このノードの種類を取得します。
        /// </summary>
        public KecaknoahAstNodeType Type { get; protected internal set; } = KecaknoahAstNodeType.Undefined;

        private List<KecaknoahAstNode> children = new List<KecaknoahAstNode>();
        /// <summary>
        /// 子ノードを取得します。
        /// </summary>
        public IReadOnlyList<KecaknoahAstNode> Children { get; protected internal set; }

        /// <summary>
        /// このノードが文脈上完全な胴体の場合trueになります。
        /// </summary>
        public bool IsComplete { get; internal set; }

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        public KecaknoahAstNode()
        {
            Children = children;
        }
        
        /// <summary>
        /// ノードを追加します。
        /// </summary>
        /// <param name="node">追加するノード</param>
        protected internal void AddNode(KecaknoahAstNode node) => children.Add(node);

    }

    /// <summary>
    /// ASTのノードの種類を定義します。
    /// </summary>
    public enum KecaknoahAstNodeType
    {
        /// <summary>
        /// 未定義
        /// </summary>
        Undefined,
        /// <summary>
        /// クラス
        /// </summary>
        Class,
        /// <summary>
        /// メソッド
        /// </summary>
        Function,
        /// <summary>
        /// ブロック
        /// </summary>
        StatementBlock,
        /// <summary>
        /// メソッド呼び出し
        /// </summary>
        CallStatement,
        /// <summary>
        /// ifブロック
        /// </summary>
        IfStatement,
        /// <summary>
        /// caseブロック
        /// </summary>
        CaseStatement,
        /// <summary>
        /// forブロック
        /// </summary>
        ForStatement,
        /// <summary>
        /// doブロック
        /// </summary>
        DoStatement,
        /// <summary>
        /// whileブロック
        /// </summary>
        WhileStatement,
        /// <summary>
        /// local文
        /// </summary>
        LocalStatement,
        /// <summary>
        /// 式
        /// </summary>
        Expression,

    }

}
