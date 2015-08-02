using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kecaknoah;

namespace KecaknoahInteractive
{
    class Program
    {
        static void Main(string[] args)
        {
            KecaknoahLexer lexer = new KecaknoahLexer();
            var parser = new KecaknoahParser();
            lexer.DefaultSourceName = "himanoa";
            var input = "";
            do
            {
                Console.Write("himanoa >");
                input = Console.ReadLine();
                var ret = lexer.AnalyzeFromSource(input);
                if (ret.Success)
                {
                    Console.WriteLine("字句解析結果--------------------");
                    Console.WriteLine(string.Join(", ", ret.Tokens.Select(p => $"{{{p.TokenString}}}")));
                    var ast = parser.ParseAsExpression(ret);
                    if (ast.Success)
                    {
                        Console.WriteLine("抽象構文木----------------------");
                        foreach (var i in ast.RootNode.ToDebugStringList()) Console.WriteLine(i);
                    }
                    else
                    {
                        Console.WriteLine($"{ast.SourceName}({ast.Error.Column}, {ast.Error.Line})\n{ast.Error.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("字句解析エラー");
                    Console.WriteLine($"{ret.SourceName}({ret.Error.Column}, {ret.Error.Line})\n{ret.Error.Message}");
                }
            } while (input != "exit");
        }
    }
}
