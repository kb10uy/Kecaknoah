﻿using System;
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
            var sw = new Stopwatch();
            var il = src.TopLevelMethods[0].Codes;
            ctx.Execute(il);
            Console.ReadLine();
        }
    }
}