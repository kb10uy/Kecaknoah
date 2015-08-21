using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="info"></param>
        public KecaknoahScriptClassObject(KecaknoahScriptClassInfo info)
        {
            Class = info;
            Constructor = KecaknoahReference.CreateRightReference(new KecaknoahInteropInstanceFunction(this, CreateInstance));
        }

        /// <summary>
        /// メンバーの参照を取得します。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override KecaknoahReference GetMemberReference(string name)
        {
            switch (name)
            {
                case "new":
                    return Constructor;
            }
            return KecaknoahNil.Reference;
        }

        private KecaknoahObject CreateInstance(KecaknoahObject self, KecaknoahObject[] args) => new KecaknoahInstance(Class);
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

        /// <summary>
        /// コンストラクターの参照を取得します。
        /// </summary>
        public KecaknoahReference Constructor { get; }

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="info"></param>
        public KecaknoahInteropClassObject(KecaknoahInteropClassInfo info)
        {
            Class = info;
            Constructor = KecaknoahReference.CreateRightReference(new KecaknoahInteropInstanceFunction(this, CreateInstance));
        }

        /// <summary>
        /// メンバーの参照を取得します。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override KecaknoahReference GetMemberReference(string name)
        {
            switch (name)
            {
                case "new":
                    return Constructor;
            }
            return KecaknoahNil.Reference;
        }

        private KecaknoahObject CreateInstance(KecaknoahObject self, KecaknoahObject[] args) => new KecaknoahInstance(Class);
    }
}
