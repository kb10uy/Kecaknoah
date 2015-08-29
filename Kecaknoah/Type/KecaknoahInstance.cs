using System.Collections.Generic;
using System.Linq;

namespace Kecaknoah.Type
{
    /// <summary>
    /// Kecaknoahで定義されたクラスのインスタンスを定義します。
    /// </summary>
    public class KecaknoahInstance : KecaknoahObject
    {
        private Dictionary<string, KecaknoahReference> localReferences = new Dictionary<string, KecaknoahReference>();
        /// <summary>
        /// このインスタンスのフィールドの参照を取得します。
        /// </summary>
        public IReadOnlyDictionary<string, KecaknoahReference> LocalFieldReferences { get; }

        private Dictionary<string, KecaknoahReference> methodReferences = new Dictionary<string, KecaknoahReference>();
        /// <summary>
        /// このインスタンスのフィールドの参照を取得します。
        /// </summary>
        public IReadOnlyDictionary<string, KecaknoahReference> InstanceMethodReferences { get; }

        /// <summary>
        /// このインスタンスのクラスを取得します。
        /// </summary>
        public KecaknoahClassInfo Class { get; protected internal set; }

        /// <summary>
        /// 特定の<see cref="KecaknoahScriptClassInfo"/>を元にして、インスタンスを生成します。
        /// </summary>
        /// <param name="klass">クラス</param>
        public KecaknoahInstance(KecaknoahScriptClassInfo klass)
        {
            Class = klass;
            ExtraType = klass.Name;
            LocalFieldReferences = localReferences;
            InstanceMethodReferences = methodReferences;
            foreach (var i in klass.Locals) localReferences[i] = new KecaknoahReference() { IsLeftValue = true };
            foreach (var i in klass.methods)
                methodReferences[i.Name] = new KecaknoahReference()
                {
                    IsLeftValue = true,
                    RawObject = new KecaknoahScriptFunction(this, i)
                };
        }

        /// <summary>
        /// 特定の<see cref="KecaknoahInteropClassInfo"/>を元にして、インスタンスを生成します。
        /// </summary>
        /// <param name="klass">クラス</param>
        public KecaknoahInstance(KecaknoahInteropClassInfo klass)
        {
            Class = klass;
            ExtraType = klass.Name;
            LocalFieldReferences = localReferences;
            InstanceMethodReferences = methodReferences;
            foreach (var i in klass.Locals) localReferences[i] = new KecaknoahReference() { IsLeftValue = true };
            foreach (var i in klass.methods)
                methodReferences[i.Name] = new KecaknoahReference()
                {
                    IsLeftValue = true,
                    RawObject = new KecaknoahInteropFunction(this, i.Body)
                };
        }

        /// <summary>
        /// 特定の<see cref="KecaknoahScriptClassInfo"/>を元にして、インスタンスを生成します。
        /// コンストラクタがあった場合、呼び出します。
        /// </summary>
        /// <param name="klass">クラス</param>
        /// <param name="ctx">コンテキスト</param>
        /// <param name="ctorArgs">コンストラクタ引数</param>
        public KecaknoahInstance(KecaknoahScriptClassInfo klass, KecaknoahContext ctx, KecaknoahObject[] ctorArgs) : this(klass)
        {
            if (klass.classMethods.Any(p => p.Name == "new"))
            {
                var ctor = klass.classMethods.First(p => p.Name == "new");
                new KecaknoahScriptFunction(this, ctor).Call(ctx, ctorArgs);
            }
        }

        /// <summary>
        /// 特定の<see cref="KecaknoahInteropClassInfo"/>を元にして、インスタンスを生成します。
        /// コンストラクタがあった場合、呼び出します。
        /// </summary>
        /// <param name="klass">クラス</param>
        /// <param name="ctx">コンテキスト</param>
        /// <param name="ctorArgs">コンストラクタ引数</param>
        public KecaknoahInstance(KecaknoahInteropClassInfo klass, KecaknoahContext ctx, KecaknoahObject[] ctorArgs) : this(klass)
        {
            if (klass.classMethods.Any(p => p.Name == "new"))
            {
                var ctor = klass.classMethods.First(p => p.Name == "new");
                ctor.Body(ctx, this, ctorArgs);
            }
        }

        private void TryCallConstructor(KecaknoahObject[] args)
        {

        }

        /// <summary>
        /// メンバーの参照を取得します。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected internal override KecaknoahReference GetMemberReference(string name)
        {
            if (LocalFieldReferences.ContainsKey(name)) return LocalFieldReferences[name];
            if (InstanceMethodReferences.ContainsKey(name)) return InstanceMethodReferences[name];
            KecaknoahReference result;
            if ((result = base.GetMemberReference(name)) == KecaknoahNil.Reference)
            {
                result = localReferences[name] = new KecaknoahReference { IsLeftValue = true };
            }
            return result;
        }

    }
}
