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
        public string Name { get; protected set; }

        /// <summary>
        /// インナークラスを取得します。
        /// </summary>
        public IReadOnlyList<KecaknoahClassInfo> InnerClasses { get; protected set; }

        /// <summary>
        /// インスタンスメソッドを取得します。
        /// </summary>
        public IReadOnlyList<KecaknoahMethodInfo> InstanceMethods { get; protected set; }

        /// <summary>
        /// クラスメソッドを取得します。
        /// </summary>
        public IReadOnlyList<KecaknoahMethodInfo> ClassMethods { get; protected set; }

        /// <summary>
        /// 予め定義されるフィールドを定義します。
        /// </summary>
        public IReadOnlyList<string> Locals { get; protected set; }

        /// <summary>
        /// 継承元クラスの名前を取得します。
        /// </summary>
        public string BaseClass { get; protected set; }
    }
}
