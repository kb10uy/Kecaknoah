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
            module.RegisterStandardLibraries();
            var ctx = module.CreateContext();
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
                        try
                        {
                            var sf = ctx.ExecuteExpressionIL(il);
                            Console.WriteLine(sf.ToString());
                        }
                        catch (Exception e)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("実行時エラー");
                            Console.WriteLine($"{e.GetType().Name}: {e.Message}");
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
}
