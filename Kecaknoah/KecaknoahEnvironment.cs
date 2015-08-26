using Kecaknoah.Standard;
using Kecaknoah.Type;
using System;
using System.Collections.Generic;
using System.Linq;

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
            modules[name] = result;

            result.RegisterClass(KecaknoahList.Information);
            result.RegisterClass(KecaknoahDictionary.Information);
            result.RegisterClass(KecaknoahConvert.Information);

            result.RegisterFunction(CreateArray, "array");
            result.RegisterFunction(ReadLine, "readln");
            result.RegisterFunction(WriteLine, "println");
            result.RegisterFunction(Write, "print");
            result.RegisterFunction(Format, "format");
            result.RegisterFunction(Exit, "exit");

            return result;
        }

        private KecaknoahFunctionResult CreateArray(KecaknoahContext context, KecaknoahObject self, KecaknoahObject[] args)
        {
            if (args.Length == 0) throw new ArgumentException("次元数が不正です");
            if (args.Length >= 5) throw new ArgumentException("次元数が多すぎます");
            var dq = args.Select(p => (int)p.ToInt64()).ToArray();
            var result = new KecaknoahArray(dq);
            return result.NoResume();
        }

        private KecaknoahFunctionResult WriteLine(KecaknoahContext context, KecaknoahObject self, KecaknoahObject[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine(args[0].ToString());
            }

            return KecaknoahNil.Instance.NoResume();
        }

        private KecaknoahFunctionResult ReadLine(KecaknoahContext context, KecaknoahObject self, KecaknoahObject[] args) => Console.ReadLine().AsKecaknoahString().NoResume();

        private KecaknoahFunctionResult Format(KecaknoahContext context, KecaknoahObject self, KecaknoahObject[] args)
        {
            var b = args[0].ToString();
            var ar = args.Skip(1).Select(p => p.ToString()).ToArray();
            return string.Format(b, ar).AsKecaknoahString().NoResume();
        }

        private KecaknoahFunctionResult Write(KecaknoahContext context, KecaknoahObject self, KecaknoahObject[] args)
        {
            Console.Write(args[0]);
            return KecaknoahNil.Instance.NoResume();
        }

        private KecaknoahFunctionResult Exit(KecaknoahContext context, KecaknoahObject self, KecaknoahObject[] args)
        {
            Environment.Exit(args.Length > 0 ? (int)args[0].ToInt64() : 0);
            return KecaknoahNil.Instance.NoResume();
        }
    }
}
