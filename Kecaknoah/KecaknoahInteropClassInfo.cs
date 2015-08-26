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
        internal List<KecaknoahInteropClassInfo> inners = new List<KecaknoahInteropClassInfo>();

        internal List<KecaknoahInteropMethodInfo> methods = new List<KecaknoahInteropMethodInfo>();

        internal List<KecaknoahInteropMethodInfo> classMethods = new List<KecaknoahInteropMethodInfo>();

        private List<string> locals = new List<string>();

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="name">クラス名</param>
        public KecaknoahInteropClassInfo(string name)
        {
            Name = name;
            InnerClasses = inners;
            InstanceMethods = methods;
            ClassMethods = ClassMethods;
            BaseClass = "";
        }

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
    }
}
