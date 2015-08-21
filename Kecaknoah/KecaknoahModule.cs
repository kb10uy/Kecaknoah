using Kecaknoah.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah
{
    /// <summary>
    /// Kecaknoahのモジュール(名前空間)を定義します。
    /// </summary>
    public sealed class KecaknoahModule
    {
        /// <summary>
        /// このインスタンスが定義されている<see cref="KecaknoahEnvironment"/>を取得します。
        /// </summary>
        public KecaknoahEnvironment Environment { get; internal set; }

        /// <summary>
        /// このインスタンスの名前を取得します。
        /// </summary>
        public string Name { get; }

        internal Dictionary<string, KecaknoahReference> globalObjects = new Dictionary<string, KecaknoahReference>();
        /// <summary>
        /// このモジュール全体で定義されるオブジェクトを取得します。
        /// </summary>
        public IReadOnlyDictionary<string, KecaknoahReference> GlobalObjects => globalObjects;

        internal List<KecaknoahClassInfo> classes = new List<KecaknoahClassInfo>();
        internal List<KecaknoahReference> classReferences = new List<KecaknoahReference>();
        /// <summary>
        /// このモジュールで定義されているクラスを取得します。
        /// </summary>
        public IReadOnlyList<KecaknoahClassInfo> Classes => classes;

        internal List<KecaknoahMethodInfo> topMethods = new List<KecaknoahMethodInfo>();
        internal List<KecaknoahReference> methodReferences = new List<KecaknoahReference>();
        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyList<KecaknoahMethodInfo> TopLevelMethods => topMethods;

        /// <summary>
        /// 定義されているオブジェクト・メソッド・クラスの中から検索し、参照を取得・設定します。
        /// </summary>
        /// <param name="name">キー</param>
        /// <returns>なければ<see cref="KecaknoahNil.Reference"/></returns>
        public KecaknoahReference this[string name]
        {
            get
            {
                KecaknoahReference result;
                int idx = 0;
                if (GlobalObjects.TryGetValue(name, out result)) return result;
                if ((idx = topMethods.FindIndex(p => p.Name == name)) > 0) return methodReferences[idx];
                if ((idx = classes.FindIndex(p => p.Name == name)) > 0) return classReferences[idx];
                return KecaknoahNil.Reference;
            }
        }

        /// <summary>
        /// <see cref="KecaknoahModule"/>の新しいインスタンスを生成します。
        /// </summary>
        internal KecaknoahModule(string name)
        {
            Name = name;
        }

        /// <summary>
        /// 新しい<see cref="KecaknoahContext"/>を生成します。
        /// </summary>
        /// <returns>生成</returns>
        public KecaknoahContext CreateContext() => new KecaknoahContext(this);

        /// <summary>
        /// .NET上のKecaknoah連携クラスを登録します。
        /// </summary>
        /// <param name="klass"></param>
        public void RegisterClass(KecaknoahInteropClassInfo klass)
        {
            classes.Add(klass);
            classReferences.Add(KecaknoahReference.CreateRightReference(new KecaknoahInteropClassObject(klass)));
        }

        /// <summary>
        /// .NETメソッドをトップレベルに登録します。
        /// </summary>
        /// <param name="method">登録する<see cref="KecaknoahInteropMethodInfo"/>形式のメソッド</param>
        public void RegisterMethod(KecaknoahInteropMethodInfo method)
        {
            topMethods.Add(method);
            methodReferences.Add(KecaknoahReference.CreateRightReference(new KecaknoahInteropInstanceFunction(null, method.Body)));
        }


        /// <summary>
        /// .NETメソッドをトップレベルに登録します。
        /// </summary>
        /// <param name="func">登録する<see cref="KecaknoahInteropDelegate"/>形式のメソッド</param>
        /// <param name="name">メソッド名</param>
        public void RegisterFunction(KecaknoahInteropDelegate func, string name)
        {
            var fo = new KecaknoahInteropMethodInfo(name, func);
            topMethods.Add(fo);
            methodReferences.Add(KecaknoahReference.CreateRightReference(new KecaknoahInteropInstanceFunction(null, func)));
        }

        /// <summary>
        /// プリコンパイルしたソースコードを登録します。
        /// </summary>
        /// <param name="src">登録する<see cref="KecaknoahSource"/></param>
        public void RegisterSource(KecaknoahSource src)
        {
            classes.AddRange(src.Classes);
            foreach (var c in src.Classes)
            {
                classes.Add(c);
                classReferences.Add(KecaknoahReference.CreateRightReference(new KecaknoahScriptClassObject(c)));
            }
            foreach (var m in src.TopLevelMethods)
            {
                topMethods.Add(m);
                methodReferences.Add(KecaknoahReference.CreateRightReference(new KecaknoahScriptFunction(m)));
            }
        }
    }
}
