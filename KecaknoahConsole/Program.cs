using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kecaknoah;
using Kecaknoah.Analyze;
using Kecaknoah.Type;
using System.Diagnostics;

namespace KecaknoahConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: kecaknoah <input file>");
                return;
            }

            var txt = File.ReadAllText(args[0]);
            var lexer = new KecaknoahLexer();
            var parser = new KecaknoahParser();
            var lr = lexer.AnalyzeFromSource(txt);
            if (!lr.Success)
            {
                Console.WriteLine($"字句解析エラー ({lr.Error.Column}, {lr.Error.Line}): {lr.Error.Message}");
                Console.ReadLine();
                return;
            }
            var ast = parser.Parse(lr);
            if (!ast.Success)
            {
                Console.WriteLine($"構文解析エラー ({ast.Error.Column}, {ast.Error.Line}): {ast.Error.Message}");
                Console.ReadLine();
            }
            var prc = new KecaknoahPrecompiler();
            var src = prc.PrecompileAll(ast);

            var environment = new KecaknoahEnvironment();
            var module = environment.CreateModule("Main");
            module.RegisterSource(src);
            var ctx = module.CreateContext();
            var il = module["main"];
            if (il != KecaknoahNil.Instance) ctx.Execute(il);
            /*
            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 10; i++)
            {
                ctx.Execute(il);
            }
            sw.Stop();
            Console.WriteLine($"{sw.ElapsedMilliseconds}");
            */
            var asm = AssembleSource(src);
            File.WriteAllLines(args[0] + ".asm", asm);
            Console.ReadLine();
        }

        static IList<string> AssembleSource(KecaknoahSource info)
        {
            var result = new List<string>();
            foreach (var i in info.Classes)
            {
                result.AddRange(AssembleClass(i as KecaknoahScriptClassInfo));
                result.Add("");
            }
            foreach (var i in info.TopLevelMethods)
            {
                result.AddRange(AssembleFunction(i as KecaknoahScriptMethodInfo));
                result.Add("");
            }
            return result;
        }

        static IList<string> AssembleClass(KecaknoahScriptClassInfo info)
        {
            var result = new List<string>();
            result.Add($".class {info.Name}");
            foreach (var i in info.Locals)
            {
                result.Add("  .local" + i);
            }
            result.Add("  ");
            foreach (var i in info.InstanceMethods)
            {
                result.AddRange(AssembleFunction(i as KecaknoahScriptMethodInfo).Select(p => "  " + p));
                result.Add("  ");
            }
            foreach (var i in info.ClassMethods)
            {
                result.AddRange(AssembleFunction(i as KecaknoahScriptMethodInfo).Select(p => "  " + p));
                result.Add("  ");
            }
            foreach (var i in info.InnerClasses)
            {
                result.AddRange(AssembleClass(i as KecaknoahScriptClassInfo).Select(p => "  " + p));
                result.Add("  ");
            }
            return result;
        }

        static IList<string> AssembleFunction(KecaknoahScriptMethodInfo info)
        {
            var result = new List<string>();
            result.Add($".function {info.Name}");
            foreach (var i in info.Codes.Codes)
            {
                result.Add("  " + i.ToString());
            }
            return result;
        }
    }
}
