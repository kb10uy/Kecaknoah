using System;
using System.Collections.Generic;
using System.Linq;

namespace Kecaknoah
{
    /// <summary>
    /// Kecaknoahで定義されるクラスを定義します。
    /// </summary>
    public sealed class KecaknoahScriptClassInfo : KecaknoahClassInfo
    {
        internal List<KecaknoahScriptClassInfo> inners = new List<KecaknoahScriptClassInfo>();
        internal List<KecaknoahScriptMethodInfo> methods = new List<KecaknoahScriptMethodInfo>();
        internal List<KecaknoahScriptMethodInfo> classMethods = new List<KecaknoahScriptMethodInfo>();
        private List<string> localnames = new List<string>();
        internal IList<KecaknoahScriptLocalInfo> LocalInfos { get; } = new List<KecaknoahScriptLocalInfo>();
        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="name">クラス名</param>
        public KecaknoahScriptClassInfo(string name)
        {
            Name = name;
            Locals = localnames;
            InnerClasses = inners;
            InstanceMethods = methods;
            ClassMethods = classMethods;
        }

        /// <summary>
        /// インナークラスを追加します。
        /// </summary>
        /// <param name="klass">追加するクラス</param>
        internal void AddInnerClass(KecaknoahScriptClassInfo klass)
        {
            if (inners.Any(p => p.Name == klass.Name)) throw new ArgumentException("同じ名前のインナークラスがすでに存在します。");
            inners.Add(klass);
        }

        /// <summary>
        /// メソッドを追加します。
        /// </summary>
        /// <param name="method">追加するメソッド</param>
        internal void AddInstanceMethod(KecaknoahScriptMethodInfo method)
        {
            methods.Add(method);
        }

        /// <summary>
        /// メソッドを追加します。
        /// </summary>
        /// <param name="method">追加するメソッド</param>
        internal void AddClassMethod(KecaknoahScriptMethodInfo method)
        {
            classMethods.Add(method);
        }

        /// <summary>
        /// フィールドを追加します。
        /// </summary>
        /// <param name="local">追加するメソッド</param>
        /// <param name="exp">初期化式を定義する<see cref="KecaknoahIL"/></param>
        internal void AddLocal(string local, KecaknoahIL exp)
        {
            LocalInfos.Add(new KecaknoahScriptLocalInfo { Name = local, InitializeIL = exp });
            localnames.Add(local);
        }

        /// <summary>
        /// スクリプトのlocal宣言の情報を定義します。
        /// </summary>
        internal sealed class KecaknoahScriptLocalInfo
        {
            /// <summary>
            /// 名前を取得します。
            /// </summary>
            public string Name { get; internal set; }

            /// <summary>
            /// 初期化式の<see cref="KecaknoahIL"/>を取得します。
            /// </summary>
            public KecaknoahIL InitializeIL { get; internal set; }
        }
    }
}
