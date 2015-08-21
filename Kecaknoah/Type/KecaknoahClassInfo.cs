using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah.Type
{
    /// <summary>
    /// Kecakanoahで利用されるクラスの情報を提供します。
    /// </summary>
    public abstract class KecaknoahClassInfo
    {
        /// <summary>
        /// クラス名を取得します。
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// インナークラスを取得します。
        /// </summary>
        public abstract IReadOnlyList<KecaknoahClassInfo> InnerClasses { get; }

        /// <summary>
        /// メソッドを取得します。
        /// </summary>
        public abstract IReadOnlyList<KecaknoahMethodInfo> InstanceMethods { get; }

        /// <summary>
        /// 予め定義されるフィールドを定義します。
        /// </summary>
        public abstract IReadOnlyList<string> Locals { get; }

        /// <summary>
        /// 継承元クラスを取得します。
        /// </summary>
        public abstract KecaknoahClassInfo BaseClass { get; }
    }
}
