using Kecaknoah.Analyze;
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
        /// 利用される<see cref="KecaknoahLexer"/>を取得します。
        /// </summary>
        public KecaknoahLexer Lexer { get; } = new KecaknoahLexer();

        /// <summary>
        /// 利用される<see cref="KecaknoahLexer"/>を取得します。
        /// </summary>
        public KecaknoahParser Parser { get; } = new KecaknoahParser();

        /// <summary>
        /// 利用される<see cref="KecaknoahPrecompiler"/>を取得します。
        /// </summary>
        public KecaknoahPrecompiler Precompiler { get; } = new KecaknoahPrecompiler();

        /// <summary>
        /// 現在の<see cref="KecaknoahModule"/>を取得します。
        /// </summary>
        public KecaknoahModule CurrentModule { get; private set; }
        

        /// <summary>
        /// モジュールを作成し、<see cref="CurrentModule"/>に設定します。
        /// </summary>
        /// <param name="name">名前</param>
        /// <returns>作成されたモジュール</returns>
        public KecaknoahModule CreateModule(string name)
        {
            var result = new KecaknoahModule(name);
            result.Environment = this;
            modules[name] = result;
            CurrentModule = result;

            result.RegisterFunction(CreateArray, "array");
            result.RegisterFunction(ReadLine, "readln");
            result.RegisterFunction(WriteLine, "println");
            result.RegisterFunction(Write, "print");
            result.RegisterFunction(Format, "format");
            result.RegisterFunction(Exit, "exit");
            result.RegisterFunction(Throw, "throw");

            return result;
        }

        #region 組み込み関数
        private static KecaknoahFunctionResult CreateArray(KecaknoahContext context, KecaknoahObject self, KecaknoahObject[] args)
        {
            if (args.Length == 0) throw new ArgumentException("次元数が不正です");
            if (args.Length >= 5) throw new ArgumentException("次元数が多すぎます");
            var dq = args.Select(p => (int)p.ToInt64()).ToArray();
            var result = new KecaknoahArray(dq);
            return result.NoResume();
        }

        private static KecaknoahFunctionResult WriteLine(KecaknoahContext context, KecaknoahObject self, KecaknoahObject[] args)
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

        private static KecaknoahFunctionResult ReadLine(KecaknoahContext context, KecaknoahObject self, KecaknoahObject[] args) => Console.ReadLine().AsKecaknoahString().NoResume();

        private static KecaknoahFunctionResult Format(KecaknoahContext context, KecaknoahObject self, KecaknoahObject[] args)
        {
            var b = args[0].ToString();
            var ar = args.Skip(1).Select(p => p.ToString()).ToArray();
            return string.Format(b, ar).AsKecaknoahString().NoResume();
        }

        private static KecaknoahFunctionResult Write(KecaknoahContext context, KecaknoahObject self, KecaknoahObject[] args)
        {
            Console.Write(args[0]);
            return KecaknoahNil.Instance.NoResume();
        }

        private static KecaknoahFunctionResult Exit(KecaknoahContext context, KecaknoahObject self, KecaknoahObject[] args)
        {
            Environment.Exit(args.Length > 0 ? (int)args[0].ToInt64() : 0);
            return KecaknoahNil.Instance.NoResume();
        }

        private static KecaknoahFunctionResult Throw(KecaknoahContext context, KecaknoahObject self, KecaknoahObject[] args)
        {
            var d = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            switch (args[0].ExtraType)
            {
                case "String":
                    Console.WriteLine($"Kecaknoahで例外がスローされました : {args[0].ToString()}");
                    break;
                default:
                    var ex = args[0] as KecaknoahInstance;
                    if (ex == null) throw new ArgumentException("Kecaknoah上で生成したインスタンス以外渡せません");
                    var mes = ex["message"].ToString();
                    Console.WriteLine($"{ex.Class.Name}: {mes}");
                    break;

            }
            Console.Write("Enterで終了します...");
            Console.ReadLine();
            Environment.Exit(-1);
            return KecaknoahNil.Instance.NoResume();
        }
        #endregion
    }

    /// <summary>
    /// Kecaknoahの実行中の例外を定義します。
    /// </summary>
    public class KecaknoahException : Exception
    {
        /// <summary>
        /// 付与されたオブジェクトを取得します。
        /// </summary>
        public KecaknoahObject Object { get; internal set; }
    }
}
