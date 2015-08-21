using Kecaknoah.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah
{
    /// <summary>
    /// 1つのソースコードを元にしたクラスとメソッドの集合体を定義します。
    /// </summary>
    public sealed class KecaknoahSource
    {
        internal List<KecaknoahScriptClassInfo> classes = new List<KecaknoahScriptClassInfo>();
        /// <summary>
        /// クラスを取得します。
        /// </summary>
        public IReadOnlyList<KecaknoahScriptClassInfo> Classes { get; }

        internal List<KecaknoahMethodInfo> methods = new List<KecaknoahMethodInfo>();
        /// <summary>
        /// トップレベルに定義されたメソッドを取得します。
        /// </summary>
        public IReadOnlyList<KecaknoahScriptMethodInfo> TopLevelMethods { get; }
    }
}
