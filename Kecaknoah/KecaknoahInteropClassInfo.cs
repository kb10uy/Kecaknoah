using System;
using System.Collections.Generic;
using System.Linq;

namespace Kecaknoah
{
    /// <summary>
    /// .NET連携クラスの規定を提供します。
    /// </summary>
    public sealed class KecaknoahInteropClassInfo : KecaknoahClassInfo
    {
        /// <summary>
        /// クラス名を取得します。
        /// </summary>
        public override string Name { get; }

        internal List<KecaknoahInteropClassInfo> inners = new List<KecaknoahInteropClassInfo>();
        /// <summary>
        /// インナークラスを取得します。
        /// </summary>
        public override IReadOnlyList<KecaknoahClassInfo> InnerClasses => inners;

        internal List<KecaknoahInteropMethodInfo> methods = new List<KecaknoahInteropMethodInfo>();
        /// <summary>
        /// インスタンスメソッドを取得します。
        /// </summary>
        public override IReadOnlyList<KecaknoahMethodInfo> InstanceMethods => methods;

        internal List<KecaknoahInteropMethodInfo> classMethods = new List<KecaknoahInteropMethodInfo>();
        /// <summary>
        /// クラスメソッドを取得します。
        /// </summary>
        public override IReadOnlyList<KecaknoahMethodInfo> ClassMethods => classMethods;

        private List<string> locals = new List<string>();
        /// <summary>
        /// フィールドの名前を取得します。
        /// </summary>
        public override IReadOnlyList<string> Locals => locals;

        /// <summary>
        /// 継承元クラスを取得します。
        /// </summary>
        public override string BaseClass { get; }

        /// <summary>
        /// インナークラスを追加します。
        /// </summary>
        /// <param name="klass">追加するクラス</param>
        internal void AddInnerClass(KecaknoahInteropClassInfo klass)
        {
            if (inners.Any(p => p.Name == klass.Name)) throw new ArgumentException("同じ名前のインナークラスがすでに存在します。");
            inners.Add(klass);
        }

        /// <summary>
        /// メソッドを追加します。
        /// </summary>
        /// <param name="method">追加するメソッド</param>
        internal void AddInstanceMethod(KecaknoahInteropMethodInfo method)
        {
            methods.Add(method);
        }

        /// <summary>
        /// クラスメソッドを追加します。
        /// </summary>
        /// <param name="method">追加するメソッド</param>
        internal void AddClassMethod(KecaknoahInteropMethodInfo method)
        {
            classMethods.Add(method);
        }

        /// <summary>
        /// フィールドを追加します。
        /// </summary>
        /// <param name="local">追加するメソッド</param>
        internal void AddLocal(string local)
        {
            locals.Add(local);
        }

        /// <summary>
        /// クラス名を指定して新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="name">クラス名</param>
        public KecaknoahInteropClassInfo(string name)
        {
            Name = name;
        }
    }
}
