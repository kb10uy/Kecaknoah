using System.Collections.Generic;

namespace Kecaknoah
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
        /// インスタンスメソッドを取得します。
        /// </summary>
        public abstract IReadOnlyList<KecaknoahMethodInfo> InstanceMethods { get; }

        /// <summary>
        /// クラスメソッドを取得します。
        /// </summary>
        public abstract IReadOnlyList<KecaknoahMethodInfo> ClassMethods { get; }

        /// <summary>
        /// 予め定義されるフィールドを定義します。
        /// </summary>
        public abstract IReadOnlyList<string> Locals { get; }

        /// <summary>
        /// 継承元クラスの名前を取得します。
        /// </summary>
        public abstract string BaseClass { get; }
    }
}
