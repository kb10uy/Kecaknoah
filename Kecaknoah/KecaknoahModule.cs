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
    public class KecaknoahModule
    {
        /// <summary>
        /// このインスタンスが定義されている<see cref="KecaknoahEnvironment"/>を取得します。
        /// </summary>
        public KecaknoahEnvironment Environment { get; internal set; }

        /// <summary>
        /// このインスタンスの名前を取得します。
        /// </summary>
        public string Name { get; }

        private Dictionary<string, KecaknoahObject> globalObjects = new Dictionary<string, KecaknoahObject>();
        /// <summary>
        /// このモジュール全体で定義されるオブジェクトを取得します。
        /// </summary>
        public IReadOnlyDictionary<string, KecaknoahObject> GlobalObjects { get; }

        /// <summary>
        /// <see cref="KecaknoahModule"/>の新しいインスタンスを生成します。
        /// </summary>
        protected internal KecaknoahModule(string name)
        {
            Name = name;
            GlobalObjects = globalObjects;
        }

        /// <summary>
        /// 新しい<see cref="KecaknoahContext"/>を生成します。
        /// </summary>
        /// <returns>生成</returns>
        public KecaknoahContext CreateContext() => new KecaknoahContext(this);

        /// <summary>
        /// .NETメソッドを登録します。
        /// </summary>
        /// <param name="func">登録する<see cref="KecaknoahDelegate"/>形式のメソッド</param>
        /// <param name="name">メソッド名</param>
        public void RegisterFunction(KecaknoahDelegate func, string name)
        {
            var fo = new KecaknoahInteropFunction() { Function = func };
            //var param = func.Method.GetParameters();
            globalObjects[name] = fo;
        }
    }
}
