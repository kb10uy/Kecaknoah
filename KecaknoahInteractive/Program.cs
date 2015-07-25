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
            lexer.DefaultSourceName = "himanoa";
            var input = "";
            do
            {
                Console.Write("himanoa >");
                input = Console.ReadLine();
                var ret = lexer.AnalyzeFromSource(input);
                if (ret.Success)
                {
                    Console.WriteLine(string.Join(", ", ret.Tokens.Select(p => $"{{{p.TokenString}}}")));
                }
                else
                {
                    Console.WriteLine($"{ret.SourceName}({ret.Error.Column}, {ret.Error.Line})\n{ret.Error.Message}");
                }
            } while (input != "exit");
        }
    }
}
