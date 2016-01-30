using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kecaknoah;
using System.IO;
using Kecaknoah.Type;

namespace Makecak
{
    class Program
    {
        static void Main(string[] args)
        {
            var cdir = Directory.GetCurrentDirectory();
            if (!File.Exists("make.kc"))
            {
                Console.WriteLine("make.kc not found!");
                Environment.Exit(-1);
            }

            var env = new KecaknoahEnvironment();
            var module = env.CreateModule("Makecaknoah");
            module.DoFile("make.kc");

            var ctx = module.CreateContext();
            var init = module["init"];
            if (init == KecaknoahNil.Instance)
            {
                Console.WriteLine("init task not found!");
                Environment.Exit(-2);
            }
            ctx.CallInstant(init);
            
        }
    }
}
