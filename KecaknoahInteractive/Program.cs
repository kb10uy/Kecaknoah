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
            lexer.DefaultSourceName = "himanoa";
            var input = "";
            do
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("himanoa > ");
                input = Console.ReadLine();
                var ret = lexer.AnalyzeFromSource(input);
                if (ret.Success)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("字句解析結果--------------------");
                    Console.WriteLine(string.Join(", ", ret.Tokens.Select(p => $"{{{p.TokenString}}}")));
                    var ast = parser.ParseAsExpression(ret);
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

                        Console.ForegroundColor = ConsoleColor.Blue;
                        var sw = new Stopwatch();
                        sw.Start();
                        for (int i = 0; i < 1024; i++)
                        {
                            ret = lexer.AnalyzeFromSource(input);
                            ast = parser.ParseAsExpression(ret);
                        }
                        sw.Stop();
                        Console.WriteLine($"解析 {sw.ElapsedMilliseconds / 1024.0}ms/回");

                        sw.Reset();
                        var ctx2 = module.CreateContext();
                        sw.Start();
                        for (int i = 0; i < 1048576; i++) ctx2.ExecuteExpression(il);
                        sw.Stop();
                        ctx2.Dispose();
                        Console.WriteLine($"実行 {sw.ElapsedMilliseconds / 1048576.0}ms/回");
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
