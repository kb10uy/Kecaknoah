using Kecaknoah.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah.Standard
{
    /// <summary>
    /// Kecaknoahでのリストを定義します。
    /// </summary>
    public sealed class KecaknoahList : KecaknoahObject
    {
        /// <summary>
        /// クラス名を取得します。
        /// </summary>
        public static readonly string ClassName = "List";
        #region 改変不要
        /// <summary>
        /// このクラスのクラスメソッドが定義される<see cref="KecaknoahInteropClassInfo"/>を取得します。
        /// こいつを適当なタイミングで<see cref="KecaknoahModule.RegisterClass(KecaknoahInteropClassInfo)"/>に
        /// 渡してください。
        /// </summary>
        internal static KecaknoahInteropClassInfo Information { get; } = new KecaknoahInteropClassInfo(ClassName);
        #endregion

        #region overrideメンバー
        /// <summary>
        /// 主にInformationを初期化します。
        /// コンストラクタを含む全てのクラスメソッドはここから追加してください。
        /// 逆に登録しなければコンストラクタを隠蔽できるということでもありますが。
        /// </summary>
        static KecaknoahList()
        {
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("new", ClassNew));
        }

        private List<KecaknoahReference> list = new List<KecaknoahReference>();
        /// <summary>
        /// このクラスのインスタンスを初期化します。
        /// 要するにこのインスタンスがスクリプト中で参照されるので、
        /// インスタンスメソッドやプロパティの設定を忘れずにしてください。
        /// あと<see cref="KecaknoahObject.ExtraType"/>に型名をセットしておくと便利です。
        /// </summary>
        public KecaknoahList()
        {
            ExtraType = ClassName;
            RegisterInstanceFunction();
        }

        /// <summary>
        /// 要素を指定して新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="refs">追加する要素</param>
        public KecaknoahList(IEnumerable<KecaknoahObject> refs) : base()
        {
            foreach (var i in refs) list.Add(KecaknoahReference.Left(i));
        }

        private KecaknoahList(IEnumerable<KecaknoahReference> refs) : base()
        {
            list.AddRange(refs);
        }

        /// <summary>
        /// 指定された名前のメンバーへの参照を取得します。
        /// 「参照」というのは右辺値・左辺値どちらにもなりうる<see cref="KecaknoahReference"/>を差し、
        /// インスタンスごとに1つずつ(呼ばれる毎にnewしない)である必要があります。
        /// ここで返されるべき参照は
        /// ・インスタンスメソッド
        /// ・プロパティ
        /// などです。どちらもフィールドに<see cref="KecaknoahReference"/>のインスタンスを確保して
        /// switch分岐でそれらを返すことが推奨されます。
        /// </summary>
        /// <param name="name">メンバー名</param>
        /// <returns></returns>
        protected internal override KecaknoahReference GetMemberReference(string name)
        {
            switch (name)
            {
                case nameof(add): return add;
                case nameof(add_range): return add_range;
                case nameof(insert): return insert;
                case nameof(clear): return clear;
                case nameof(each): return each;
                case nameof(remove_at): return remove_at;
                case nameof(remove_by): return remove_by;
                case nameof(map): return map;
                case nameof(reduce): return reduce;
                case nameof(filter): return filter;

                case "length": return KecaknoahReference.Right(list.Count);
            }
            return base.GetMemberReference(name);
        }

        /// <summary>
        /// インデクサーの参照を得ます。
        /// <see cref="KecaknoahObject.GetMemberReference(string)"/>と<see cref="KecaknoahObject.Call(KecaknoahContext, KecaknoahObject[])"/>の
        /// 中間のような存在です。
        /// </summary>
        /// <param name="indices">インデックス</param>
        /// <returns>返す参照</returns>
        protected internal override KecaknoahReference GetIndexerReference(KecaknoahObject[] indices) => list[(int)indices[0].ToInt64()];
        #endregion


        #region インスタンスメソッド
        //Dictionary解決でもいいかも
        KecaknoahReference
            add, add_range, insert, each, remove_at, remove_by,
            filter, map, reduce, clear, any, all;

        private void RegisterInstanceFunction()
        {
            add = KecaknoahReference.Right(this, InstanceAdd);
            add_range = KecaknoahReference.Right(this, InstanceAddRange);
            clear = KecaknoahReference.Right(this, InstanceClear);
            insert = KecaknoahReference.Right(this, InstanceInsert);
            each = KecaknoahReference.Right(this, InstanceEach);
            remove_at = KecaknoahReference.Right(this, InstanceRemoveAt);
            remove_by = KecaknoahReference.Right(this, InstanceRemoveBy);
            filter = KecaknoahReference.Right(this, InstanceFilter);
            map = KecaknoahReference.Right(this, InstanceMap);
            reduce = KecaknoahReference.Right(this, InstanceReduce);
            any = KecaknoahReference.Right(this, list.Select(p => p.RawObject).GenerateAnyFunction());
            all = KecaknoahReference.Right(this, list.Select(p => p.RawObject).GenerateAllFunction());
        }

        private KecaknoahFunctionResult InstanceAdd(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            foreach (var i in args)
                list.Add(KecaknoahReference.Left(i));
            return KecaknoahNil.Instance.NoResume();
        }

        private KecaknoahFunctionResult InstanceAddRange(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var al = args[0].AsArray();
            list.AddRange(al.Select(p => KecaknoahReference.Left(p)));
            return KecaknoahNil.Instance.NoResume();
        }

        private KecaknoahFunctionResult InstanceClear(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            list.Clear();
            return KecaknoahNil.Instance.NoResume();
        }

        private KecaknoahFunctionResult InstanceInsert(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            list.Insert((int)args[0].ToInt64(), new KecaknoahReference { IsLeftValue = true, RawObject = args[1] });
            return KecaknoahNil.Instance.NoResume();
        }

        private KecaknoahFunctionResult InstanceRemoveAt(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            list.RemoveAt((int)args[0].ToInt64());
            return KecaknoahNil.Instance.NoResume();
        }

        private KecaknoahFunctionResult InstanceRemoveBy(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            list.RemoveAll(p => args[0].CallAsPredicate(ctx, p.RawObject));
            return KecaknoahNil.Instance.NoResume();
        }

        private KecaknoahFunctionResult InstanceEach(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            foreach (var i in list) args[0].Call(ctx, new[] { i.RawObject });
            return KecaknoahNil.Instance.NoResume();
        }

        private KecaknoahFunctionResult InstanceFilter(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var r = list.Where(p => args[0].CallAsPredicate(ctx, p.RawObject));
            return new KecaknoahList(r).NoResume();
        }

        private KecaknoahFunctionResult InstanceMap(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var r = list.Select(p => args[0].Call(ctx, new[] { p.RawObject }).ReturningObject);
            return new KecaknoahList(r).NoResume();
        }

        private KecaknoahFunctionResult InstanceReduce(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var r = list.Select(p => p.RawObject).Aggregate((p, q) => args[0].Call(ctx, new[] { p, q }).ReturningObject);
            return r.NoResume();
        }

#pragma warning disable 1591
        public override bool Equals(object obj)
        {
            var t = obj as KecaknoahList;
            return t != null && list.Equals(t.list);
        }

        public override int GetHashCode() => list.GetHashCode();
#pragma warning restore 1591

        #endregion

        #region クラスメソッド
        /* 
        当たり前ですがクラスメソッド呼び出しではselfはnullになります。
        selfに代入するのではなく生成したのをNoResumeで返却します。
        */

        private static KecaknoahFunctionResult ClassNew(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args) => new KecaknoahList().NoResume();
        #endregion
    }
}
