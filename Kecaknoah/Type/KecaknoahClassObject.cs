using System.Collections.Generic;

namespace Kecaknoah.Type
{
    /// <summary>
    /// クラスメソッドを提供するオブジェクトを定義します。
    /// </summary>
    public sealed class KecaknoahScriptClassObject : KecaknoahObject
    {
        /// <summary>
        /// 元になるクラスを取得します。
        /// </summary>
        public KecaknoahScriptClassInfo Class { get; }

        /// <summary>
        /// コンストラクターの参照を取得します。
        /// </summary>
        public KecaknoahReference Constructor { get; }

        private Dictionary<string, KecaknoahReference> methods = new Dictionary<string, KecaknoahReference>();
        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="info"></param>
        public KecaknoahScriptClassObject(KecaknoahScriptClassInfo info)
        {
            Class = info;
            ExtraType = "ScriptClass";
            Constructor = KecaknoahReference.Right(this, CreateInstance);
            foreach (var i in Class.classMethods)
            {
                methods[i.Name] = (KecaknoahReference.Right(new KecaknoahScriptFunction(KecaknoahNil.Instance, i)));
            }
        }

        /// <summary>
        /// メンバーの参照を取得します。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected internal override KecaknoahReference GetMemberReference(string name)
        {
            switch (name)
            {
                case "new":
                    return Constructor;
                default:
                    if (methods.ContainsKey(name)) return methods[name];
                    return KecaknoahNil.Reference;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        protected internal override KecaknoahFunctionResult Call(KecaknoahContext context, KecaknoahObject[] args) => new KecaknoahInstance(Class, context, args).NoResume();

        private KecaknoahFunctionResult CreateInstance(KecaknoahContext context, KecaknoahObject self, KecaknoahObject[] args) => new KecaknoahInstance(Class, context, args).NoResume();
    }

    /// <summary>
    /// クラスメソッドを提供するオブジェクトを定義します。
    /// </summary>
    public sealed class KecaknoahInteropClassObject : KecaknoahObject
    {
        /// <summary>
        /// 元になるクラスを取得します。
        /// </summary>
        public KecaknoahInteropClassInfo Class { get; }

        private Dictionary<string, KecaknoahReference> methods = new Dictionary<string, KecaknoahReference>();
        private Dictionary<string, KecaknoahReference> consts = new Dictionary<string, KecaknoahReference>();

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="info"></param>
        public KecaknoahInteropClassObject(KecaknoahInteropClassInfo info)
        {
            Class = info;
            foreach (var i in Class.classMethods) methods[i.Name] = KecaknoahReference.Right(KecaknoahNil.Instance, i.Body);
            foreach (var i in Class.ConstInfos) consts[i.Name] = KecaknoahReference.Right(i.Value);
        }

        /// <summary>
        /// メンバーの参照を取得します。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected internal override KecaknoahReference GetMemberReference(string name)
        {
            switch (name)
            {
                /*
                case "new":
                    return Constructor;
                */
                default:
                    if (methods.ContainsKey(name)) return methods[name];
                    if (consts.ContainsKey(name)) return methods[name];
                    return KecaknoahNil.Reference;
            }
        }

        //private KecaknoahFunctionResult CreateInstance(KecaknoahContext context, KecaknoahObject self, KecaknoahObject[] args) => new KecaknoahInstance(Class, context, args).NoResume();
    }
}
