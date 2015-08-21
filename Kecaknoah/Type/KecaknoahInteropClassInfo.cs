using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah.Type
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
        /// インナークラスを取得します。
        /// </summary>
        public override IReadOnlyList<KecaknoahMethodInfo> InstanceMethods => methods;

        private List<string> locals = new List<string>();
        /// <summary>
        /// フィールドの名前を取得します。
        /// </summary>
        public override IReadOnlyList<string> Locals => locals;

        /// <summary>
        /// 継承元クラスを取得します。
        /// </summary>
        public override KecaknoahClassInfo BaseClass { get; }

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
        internal void AddMethod(KecaknoahInteropMethodInfo method)
        {
            methods.Add(method);
        }

        /// <summary>
        /// フィールドを追加します。
        /// </summary>
        /// <param name="local">追加するメソッド</param>
        internal void AddLocal(string local)
        {
            locals.Add(local);
        }
    }
}
