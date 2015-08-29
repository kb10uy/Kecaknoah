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
        /// コンストラクタがあった場合、呼び出します。
        /// </summary>
        /// <param name="klass">クラス</param>
        /// <param name="ctx">コンテキスト</param>
        /// <param name="ctorArgs">コンストラクタ引数</param>
        public KecaknoahInstance(KecaknoahScriptClassInfo klass, KecaknoahContext ctx, KecaknoahObject[] ctorArgs)
        {
            Class = klass;
            ExtraType = klass.Name;
            LocalFieldReferences = localReferences;
            InstanceMethodReferences = methodReferences;
            foreach (var i in klass.LocalInfos)
            {
                localReferences[i.Name] = new KecaknoahReference() { IsLeftValue = true };
                if (i.InitializeIL != null)
                {
                    localReferences[i.Name].RawObject = ctx.ExecuteExpressionIL(i.InitializeIL);
                }
            }
            foreach (var i in klass.methods)
                methodReferences[i.Name] = new KecaknoahReference()
                {
                    IsLeftValue = true,
                    RawObject = new KecaknoahScriptFunction(this, i)
                };
            var ctor = klass.classMethods.FirstOrDefault(p => p.Name == "new");
            if (ctor != null)
            {
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
        public KecaknoahInstance(KecaknoahInteropClassInfo klass, KecaknoahContext ctx, KecaknoahObject[] ctorArgs)
        {
            Class = klass;
            ExtraType = klass.Name;
            LocalFieldReferences = localReferences;
            InstanceMethodReferences = methodReferences;
            foreach (var i in klass.LocalInfos)
            {
                localReferences[i.Name] = KecaknoahReference.Left(i.Value.AsByValValue());
            }
            foreach (var i in klass.methods)
                methodReferences[i.Name] = new KecaknoahReference()
                {
                    IsLeftValue = true,
                    RawObject = new KecaknoahInteropFunction(this, i.Body)
                };
            var ctor = klass.classMethods.FirstOrDefault(p => p.Name == "new");
            if (ctor != null) ctor.Body(ctx, this, ctorArgs);
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
