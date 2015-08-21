using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kecaknoah;
using Kecaknoah.Analyze;
using Kecaknoah.Type;
using System.Diagnostics;

namespace KecaknoahInteractive
{
    class Program
    {
        static void Main(string[] args)
        {
            var lexer = new KecaknoahLexer();
            var parser = new KecaknoahParser();
            var precompiler = new KecaknoahPrecompiler();
            var environment = new KecaknoahEnvironment();
            var sw = new Stopwatch();
            var module = environment.CreateModule("Himanoa");
            var ctx = module.CreateContext();
            module.RegisterFunction((self, prm) =>
            {
                Console.WriteLine($"printメソッド: {prm[0].ToString()}");
                return KecaknoahNil.Instance;
            }, "print");
            module.RegisterFunction(KecaknoahMethods.Sin, "sin");
            module.RegisterFunction(KecaknoahMethods.Cos, "cos");
            module.RegisterFunction(KecaknoahMethods.Tan, "tan");
            module.RegisterFunction(KecaknoahMethods.Max, "max");
            module.RegisterFunction(KecaknoahMethods.Min, "min");
            lexer.DefaultSourceName = "himanoa";
            var input = "";
            //var il = "";
            do
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("himanoa > ");
                input = Console.ReadLine();
                /*
                il = "";
                input = "";
                do
                {
                    il += input + Environment.NewLine;
                    input = Console.ReadLine();
                } while (input != "---");
                */
                //var ret = lexer.AnalyzeFromSource(il);
                var ret = lexer.AnalyzeFromSource(input);
                if (ret.Success)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("字句解析結果--------------------");
                    Console.WriteLine(string.Join(", ", ret.Tokens.Select(p => $"{{{p.TokenString}}}")));
                    var ast = parser.ParseAsExpression(ret);
                    //var ast = parser.Parse(ret);
                    if (ast.Success)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("抽象構文木----------------------");
                        foreach (var i in ast.RootNode.ToDebugStringList()) Console.WriteLine(i);

                        var il = precompiler.PrecompileExpression(ast);

                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("KecaknoahIL---------------------");
                        foreach (var i in il.Codes) Console.WriteLine(i.ToString());

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("実行結果------------------------");
                        var val = ctx.ExecuteExpression(il);
                        Console.WriteLine(val);
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("環境情報------------------------");
                        foreach (var i in ctx.Objects)
                        {
                            Console.WriteLine($"{i.Key} : {i.Value.RawObject.ToString()}");
                        }


                        /*
                        sw.Reset();
                        sw.Start();
                        using (var ctx = module.CreateContext())
                        {
                            for (int i = 0; i < 1048576; i++)
                            {
                                var val = ctx.ExecuteExpression(il);
                            }
                        }
                        sw.Stop();
                        Console.WriteLine($"{sw.ElapsedMilliseconds / 1048576.0 }ms/call");
                        */
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"{ast.SourceName}({ast.Error.Column}, {ast.Error.Line})\n{ast.Error.Message}");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("字句解析エラー");
                    Console.WriteLine($"{ret.SourceName}({ret.Error.Column}, {ret.Error.Line})\n{ret.Error.Message}");
                }
                Console.WriteLine();
            } while (input != "exit");
            ctx.Dispose();
        }
    }

    static class KecaknoahMethods
    {
        public static KecaknoahObject Sin(KecaknoahObject self, KecaknoahObject[] args) => Math.Sin(args[0].ToDouble()).AsKecaknoahFloat();
        public static KecaknoahObject Cos(KecaknoahObject self, KecaknoahObject[] args) => Math.Cos(args[0].ToDouble()).AsKecaknoahFloat();
        public static KecaknoahObject Tan(KecaknoahObject self, KecaknoahObject[] args) => Math.Tan(args[0].ToDouble()).AsKecaknoahFloat();
        public static KecaknoahObject Max(KecaknoahObject self, KecaknoahObject[] args) => Math.Max(args[0].ToInt64(), args[1].ToInt64()).AsKecaknoahInteger();
        public static KecaknoahObject Min(KecaknoahObject self, KecaknoahObject[] args) => Math.Min(args[0].ToInt64(), args[1].ToInt64()).AsKecaknoahInteger();
    }
}
