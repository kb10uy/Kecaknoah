using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            LocalFieldReferences = localReferences;
            InstanceMethodReferences = methodReferences;
            foreach (var i in klass.Locals) localReferences[i] = new KecaknoahReference() { IsLeftValue = true };
            foreach (var i in klass.methods)
                methodReferences[i.Name] = new KecaknoahReference()
                {
                    IsLeftValue = true,
                    RawObject = new KecaknoahScriptInstanceFunction(this, i)
                };
        }

        /// <summary>
        /// 特定の<see cref="KecaknoahInteropClassInfo"/>を元にして、インスタンスを生成します。
        /// </summary>
        /// <param name="klass">クラス</param>
        public KecaknoahInstance(KecaknoahInteropClassInfo klass)
        {
            Class = klass;
            LocalFieldReferences = localReferences;
            InstanceMethodReferences = methodReferences;
            foreach (var i in klass.Locals) localReferences[i] = new KecaknoahReference() { IsLeftValue = true };
            foreach (var i in klass.methods)
                methodReferences[i.Name] = new KecaknoahReference()
                {
                    IsLeftValue = true,
                    RawObject = new KecaknoahInteropInstanceFunction(this, i.Body)
                };
        }

        /// <summary>
        /// メンバーの参照を取得します。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override KecaknoahReference GetMemberReference(string name) =>
            LocalFieldReferences.FirstOrDefault(p => p.Key == name).Value
                ?? InstanceMethodReferences.FirstOrDefault(p => p.Key == name).Value
                ?? base.GetMemberReference(name);
    }
}
