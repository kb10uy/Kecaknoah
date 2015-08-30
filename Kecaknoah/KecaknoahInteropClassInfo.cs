using Kecaknoah.Type;
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
        internal IList<KecaknoahInteropClassLocalInfo> LocalInfos { get; } = new List<KecaknoahInteropClassLocalInfo>();
        internal IList<KecaknoahInteropClassLocalInfo> ConstInfos { get; } = new List<KecaknoahInteropClassLocalInfo>();
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
        public void AddInnerClass(KecaknoahInteropClassInfo klass)
        {
            if (inners.Any(p => p.Name == klass.Name)) throw new ArgumentException("同じ名前のインナークラスがすでに存在します。");
            inners.Add(klass);
        }

        /// <summary>
        /// メソッドを追加します。
        /// </summary>
        /// <param name="method">追加するメソッド</param>
        public void AddInstanceMethod(KecaknoahInteropMethodInfo method)
        {
            methods.Add(method);
        }

        /// <summary>
        /// クラスメソッドを追加します。
        /// </summary>
        /// <param name="method">追加するメソッド</param>
        public void AddClassMethod(KecaknoahInteropMethodInfo method)
        {
            classMethods.Add(method);
        }

        /// <summary>
        /// フィールドを追加します。
        /// </summary>
        /// <param name="local">追加するメソッド</param>
        public void AddLocal(string local)
        {
            locals.Add(local);
            LocalInfos.Add(new KecaknoahInteropClassLocalInfo { Name = local, Value = KecaknoahNil.Instance });
        }

        /// <summary>
        /// フィールドを追加します。
        /// </summary>
        /// <param name="local">追加するメソッド</param>
        /// <param name="obj">設定する初期値</param>
        public void AddLocal(string local, KecaknoahObject obj)
        {
            locals.Add(local);
            LocalInfos.Add(new KecaknoahInteropClassLocalInfo { Name = local, Value = obj });
        }

        /// <summary>
        /// クラス定数を追加します。
        /// </summary>
        /// <param name="local">名前</param>
        /// <param name="obj">値</param>
        public void AddConstant(string local, KecaknoahObject obj)
        {
            ConstInfos.Add(new KecaknoahInteropClassLocalInfo { Name = local, Value = obj });
        }

        internal sealed class KecaknoahInteropClassLocalInfo
        {
            public string Name { get; set; }
            public KecaknoahObject Value { get; set; }
        }

        #region ヘルパー
        /// <summary>
        /// 指定した列挙体から同等の<see cref="KecaknoahInteropClassInfo"/>を作成します。
        /// </summary>
        /// <param name="enumType">作成する列挙体の<see cref="System.Type"/>オブジェクト</param>
        /// <returns></returns>
        public static KecaknoahInteropClassInfo CreateFromEnum(System.Type enumType)
        {
            var type = enumType;
            var result = new KecaknoahInteropClassInfo(type.Name);
            var names = Enum.GetNames(type);
            foreach (var i in names)
            {
                var val = (int)Enum.Parse(type, i);
                result.AddConstant(i, val.AsKecaknoahInteger());
            }
            return result;
        }
        #endregion
    }
}
