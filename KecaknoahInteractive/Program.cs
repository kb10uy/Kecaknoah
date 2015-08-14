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
            var module = environment.CreateModule("H1manoa");
            module.RegisterFunction(prm =>
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
                        using (var ctx = module.CreateContext())
                        {
                            var val = ctx.ExecuteExpression(il);
                            Console.WriteLine(val);
                        }
                        
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
        }
    }

    static class KecaknoahMethods
    {
        public static KecaknoahObject Sin(KecaknoahObject[] args) => Math.Sin((double)args[0].AsRawObject<double>()).AsKecaknoahDouble();
        public static KecaknoahObject Cos(KecaknoahObject[] args) => Math.Cos((double)args[0].AsRawObject<double>()).AsKecaknoahDouble();
        public static KecaknoahObject Tan(KecaknoahObject[] args) => Math.Tan((double)args[0].AsRawObject<double>()).AsKecaknoahDouble();
        public static KecaknoahObject Max(KecaknoahObject[] args) => Math.Max((double)args[0].AsRawObject<double>(), (double)args[1].AsRawObject<double>()).AsKecaknoahDouble();
        public static KecaknoahObject Min(KecaknoahObject[] args) => Math.Min((double)args[0].AsRawObject<double>(), (double)args[1].AsRawObject<double>()).AsKecaknoahDouble();
    }
}
