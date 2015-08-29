using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Kecaknoah;
using Kecaknoah.Analyze;
using Kecaknoah.Type;

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

            KecaknoahSource src;
            if (Path.GetExtension(args[0]).Equals(".kcb", StringComparison.OrdinalIgnoreCase))
            {
                src = KecaknoahBytecode.Load(File.OpenRead(args[0]));
            }
            else
            {
                var txt = File.ReadAllText(args[0]);
                var lexer = new KecaknoahLexer();
                var parser = new KecaknoahParser();
                var lr = lexer.AnalyzeFromSource(txt);
                if (!lr.Success)
                {
                    Console.WriteLine($"字句解析エラー ({lr.Error.Column}, {lr.Error.Line}): {lr.Error.Message}");
                    Console.ReadLine();
                    Environment.Exit(1);
                }
                var ast = parser.Parse(lr);
                if (!ast.Success)
                {
                    Console.WriteLine($"構文解析エラー ({ast.Error.Column}, {ast.Error.Line}): {ast.Error.Message}");
                    Console.ReadLine();
                    Environment.Exit(1);
                }
                var prc = new KecaknoahPrecompiler();
                src = prc.PrecompileAll(ast);
#if DEBUG
                KecaknoahBytecode.Save(src, File.OpenWrite(args[0] + ".kcb"));
#endif
            }
#if DEBUG
            var asm = AssembleSource(src);
            File.WriteAllLines(args[0] + ".asm", asm);
            Console.WriteLine("Assembled source was generated.");
            Console.ReadLine();
#endif
            var environment = new KecaknoahEnvironment();
            var module = environment.CreateModule("Main");
            module.RegisterStandardLibraries();
            module.RegisterSource(src);
            var ctx = module.CreateContext();
            var il = module["main"];
            var kargs = new List<KecaknoahObject>();
            if (args.Length >= 2)
            {
                kargs.Add(new KecaknoahArray(args.Skip(1).Select(p => p.AsKecaknoahString())));
            }
            else
            {
                kargs.Add(new KecaknoahArray(new List<KecaknoahObject>()));
            }
            if (il != KecaknoahNil.Instance)
            {
                ctx.Initialize(il, kargs);
                while (ctx.MoveNext()) ;
            }
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

        public void MakeUnko(int val)
        {
            Console.WriteLine("うんこが{0}つ", val);
        }
    }
}
