using Kecaknoah.Type;
using System.Collections.Generic;

namespace Kecaknoah
{
    /// <summary>
    /// 1つのソースコードを元にしたクラスとメソッドの集合体を定義します。
    /// </summary>
    public sealed class KecaknoahSource
    {
        internal List<string> uses = new List<string>();
        /// <summary>
        /// useによるインポート対象を取得します。
        /// </summary>
        public IReadOnlyList<string> Uses => uses;

        internal List<KecaknoahScriptClassInfo> classes = new List<KecaknoahScriptClassInfo>();
        /// <summary>
        /// クラスを取得します。
        /// </summary>
        public IReadOnlyList<KecaknoahScriptClassInfo> Classes => classes;

        internal List<KecaknoahScriptMethodInfo> methods = new List<KecaknoahScriptMethodInfo>();
        /// <summary>
        /// トップレベルに定義されたメソッドを取得します。
        /// </summary>
        public IReadOnlyList<KecaknoahScriptMethodInfo> TopLevelMethods => methods;

    }
}
