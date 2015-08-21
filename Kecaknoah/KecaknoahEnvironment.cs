using Kecaknoah.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah
{
    /// <summary>
    /// Kecaknoahの実行環境を定義します。
    /// </summary>
    public class KecaknoahEnvironment
    {
        private Dictionary<string, KecaknoahModule> modules = new Dictionary<string, KecaknoahModule>();
        /// <summary>
        /// このインスタンスで定義されている<see cref="KecaknoahModule"/>を取得します。
        /// </summary>
        public KecaknoahModule this[string name] => modules[name];

        /// <summary>
        /// 名前を指定して新しい<see cref="KecaknoahModule"/>のインスタンスを生成します。
        /// </summary>
        /// <param name="name">名前</param>
        /// <returns>このインスタンスで定義される<see cref="KecaknoahModule"/></returns>
        public KecaknoahModule CreateModule(string name)
        {
            var result = new KecaknoahModule(name);
            result.Environment = this;
            result.RegisterFunction(WriteLine, "print");
            modules[name] = result;
            return result;
        }

        private KecaknoahObject WriteLine(KecaknoahObject self, KecaknoahObject[] args)
        {
            Console.WriteLine(args[0]);
            return KecaknoahNil.Instance;
        }
    }
}
