using Kecaknoah.Analyze;
using Kecaknoah.Standard;
using Kecaknoah.Type;
using System;
using System.Collections.Generic;
using System.IO;

namespace Kecaknoah
{
    /// <summary>
    /// Kecaknoahのモジュール(名前空間)を定義します。
    /// </summary>
    public sealed class KecaknoahModule
    {
        /// <summary>
        /// このインスタンスが定義されている<see cref="KecaknoahEnvironment"/>を取得します。
        /// </summary>
        public KecaknoahEnvironment Environment { get; internal set; }

        /// <summary>
        /// このインスタンスの名前を取得します。
        /// </summary>
        public string Name { get; }

        internal Dictionary<string, KecaknoahReference> globalObjects = new Dictionary<string, KecaknoahReference>();
        /// <summary>
        /// このモジュール全体で定義されるオブジェクトを取得します。
        /// </summary>
        public IReadOnlyDictionary<string, KecaknoahReference> GlobalObjects => globalObjects;

        internal List<KecaknoahClassInfo> classes = new List<KecaknoahClassInfo>();
        internal List<KecaknoahReference> classReferences = new List<KecaknoahReference>();
        /// <summary>
        /// このモジュールで定義されているクラスを取得します。
        /// </summary>
        public IReadOnlyList<KecaknoahClassInfo> Classes => classes;

        internal List<KecaknoahMethodInfo> topMethods = new List<KecaknoahMethodInfo>();
        internal List<KecaknoahReference> methodReferences = new List<KecaknoahReference>();

        /// <summary>
        /// このモジュールで定義されているトップレベルメソッドを取得します。
        /// </summary>
        public IReadOnlyList<KecaknoahMethodInfo> TopLevelMethods => topMethods;

        /// <summary>
        /// 拡張ライブラリ検索の際のディレクトリを取得します。
        /// </summary>
        public string BaseDirectory { get; } =
            Path.Combine(Path.GetDirectoryName(typeof(KecaknoahModule).Assembly.Location), "kclib");

        /// <summary>
        /// 指定した名前を持つトップレベルの<see cref="KecaknoahObject"/>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public KecaknoahObject this[string name] => GetReference(name).RawObject;

        /// <summary>
        /// <see cref="KecaknoahModule"/>の新しいインスタンスを生成します。
        /// </summary>
        internal KecaknoahModule(string name)
        {
            Name = name;
            RegisterFunction(Eval, "eval");
        }

        /// <summary>
        /// 新しい<see cref="KecaknoahContext"/>を生成します。
        /// </summary>
        /// <returns>生成</returns>
        public KecaknoahContext CreateContext() => new KecaknoahContext(this);

        /// <summary>
        /// 定義されているオブジェクト・メソッド・クラスの中から検索し、参照を取得・設定します。
        /// </summary>
        /// <param name="name">キー</param>
        /// <returns>なければ<see cref="KecaknoahNil.Reference"/></returns>
        public KecaknoahReference GetReference(string name)
        {
            int idx = 0;
            if (GlobalObjects.ContainsKey(name)) return GlobalObjects[name];
            if ((idx = topMethods.FindLastIndex(p => p.Name == name)) >= 0) return methodReferences[idx];
            if ((idx = classes.FindLastIndex(p => p.Name == name)) >= 0) return classReferences[idx];
            return KecaknoahNil.Reference;
        }

        private KecaknoahFunctionResult Eval(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args) => new KecaknoahContext(this).ExecuteExpressionIL(new KecaknoahPrecompiler().PrecompileExpression(new KecaknoahParser().ParseAsExpression(new KecaknoahLexer().AnalyzeFromSource(args[0].ToString())))).NoResume();

        #region Registerers
        /// <summary>
        /// プリコンパイルしたソースコードを登録します。
        /// </summary>
        /// <param name="src">登録する<see cref="KecaknoahSource"/></param>
        public void RegisterSource(KecaknoahSource src)
        {
            ProcessUseDirective(src.Uses);
            foreach (var c in src.Classes)
            {
                classes.Add(c);
                classReferences.Add(KecaknoahReference.Right(new KecaknoahScriptClassObject(c)));
            }
            foreach (var m in src.TopLevelMethods)
            {
                topMethods.Add(m);
                methodReferences.Add(KecaknoahReference.Right(new KecaknoahScriptFunction(KecaknoahNil.Instance, m)));
            }
        }

        /// <summary>
        /// .NET上のKecaknoah連携クラスを登録します。
        /// </summary>
        /// <param name="klass"></param>
        public void RegisterClass(KecaknoahInteropClassInfo klass)
        {
            classes.Add(klass);
            classReferences.Add(KecaknoahReference.Right(new KecaknoahInteropClassObject(klass)));
        }

        /// <summary>
        /// .NETメソッドをトップレベルに登録します。
        /// </summary>
        /// <param name="method">登録する<see cref="KecaknoahInteropMethodInfo"/>形式のメソッド</param>
        public void RegisterMethod(KecaknoahInteropMethodInfo method)
        {
            topMethods.Add(method);
            methodReferences.Add(KecaknoahReference.Right(KecaknoahNil.Instance, method.Body));
        }


        /// <summary>
        /// .NETメソッドをトップレベルに登録します。
        /// </summary>
        /// <param name="func">登録する<see cref="KecaknoahInteropDelegate"/>形式のメソッド</param>
        /// <param name="name">メソッド名</param>
        public void RegisterFunction(KecaknoahInteropDelegate func, string name)
        {
            var fo = new KecaknoahInteropMethodInfo(name, func);
            topMethods.Add(fo);
            methodReferences.Add(KecaknoahReference.Right(KecaknoahNil.Instance, func));
        }

        /// <summary>
        /// .NETメソッドをトップレベルに登録します。
        /// </summary>
        /// <param name="func">登録する<see cref="Func{T1, T2, TResult}"/>形式のメソッド</param>
        /// <param name="name">メソッド名</param>
        public void RegisterFunction(Func<KecaknoahObject, KecaknoahObject[], KecaknoahObject> func, string name)
        {
            KecaknoahInteropDelegate wp =
                (ctx, self, args) => func(self, args).NoResume();
            var fo = new KecaknoahInteropMethodInfo(name, wp);
            topMethods.Add(fo);
            methodReferences.Add(KecaknoahReference.Right(KecaknoahNil.Instance, wp));
        }

        /// <summary>
        /// .NETメソッドをトップレベルに登録します。
        /// </summary>
        /// <param name="func">登録する<see cref="Func{T1, T2, TResult}"/>形式のメソッド</param>
        /// <param name="name">メソッド名</param>
        public void RegisterInt32Function(Func<KecaknoahObject, KecaknoahObject[], int> func, string name)
        {
            KecaknoahInteropDelegate wp =
                (ctx, self, args) => func(self, args).AsKecaknoahInteger().NoResume();
            var fo = new KecaknoahInteropMethodInfo(name, wp);
            topMethods.Add(fo);
            methodReferences.Add(KecaknoahReference.Right(KecaknoahNil.Instance, wp));
        }

        /// <summary>
        /// .NETメソッドをトップレベルに登録します。
        /// </summary>
        /// <param name="func">登録する<see cref="Func{T1, T2, TResult}"/>形式のメソッド</param>
        /// <param name="name">メソッド名</param>
        public void RegisterInt64Function(Func<KecaknoahObject, KecaknoahObject[], long> func, string name)
        {
            KecaknoahInteropDelegate wp =
                (ctx, self, args) => func(self, args).AsKecaknoahInteger().NoResume();
            var fo = new KecaknoahInteropMethodInfo(name, wp);
            topMethods.Add(fo);
            methodReferences.Add(KecaknoahReference.Right(KecaknoahNil.Instance, wp));
        }

        /// <summary>
        /// .NETメソッドをトップレベルに登録します。
        /// </summary>
        /// <param name="func">登録する<see cref="Func{T1, T2, TResult}"/>形式のメソッド</param>
        /// <param name="name">メソッド名</param>
        public void RegisterSingleFunction(Func<KecaknoahObject, KecaknoahObject[], float> func, string name)
        {
            KecaknoahInteropDelegate wp =
                (ctx, self, args) => ((double)func(self, args)).AsKecaknoahFloat().NoResume();
            var fo = new KecaknoahInteropMethodInfo(name, wp);
            topMethods.Add(fo);
            methodReferences.Add(KecaknoahReference.Right(KecaknoahNil.Instance, wp));
        }

        /// <summary>
        /// .NETメソッドをトップレベルに登録します。
        /// </summary>
        /// <param name="func">登録する<see cref="Func{T1, T2, TResult}"/>形式のメソッド</param>
        /// <param name="name">メソッド名</param>
        public void RegisterDoubleFunction(Func<KecaknoahObject, KecaknoahObject[], double> func, string name)
        {
            KecaknoahInteropDelegate wp =
                (ctx, self, args) => func(self, args).AsKecaknoahFloat().NoResume();
            var fo = new KecaknoahInteropMethodInfo(name, wp);
            topMethods.Add(fo);
            methodReferences.Add(KecaknoahReference.Right(KecaknoahNil.Instance, wp));
        }

        /// <summary>
        /// .NETメソッドをトップレベルに登録します。
        /// </summary>
        /// <param name="func">登録する<see cref="Func{T1, T2, TResult}"/>形式のメソッド</param>
        /// <param name="name">メソッド名</param>
        public void RegisterBooleanFunction(Func<KecaknoahObject, KecaknoahObject[], bool> func, string name)
        {
            KecaknoahInteropDelegate wp =
                (ctx, self, args) => func(self, args).AsKecaknoahBoolean().NoResume();
            var fo = new KecaknoahInteropMethodInfo(name, wp);
            topMethods.Add(fo);
            methodReferences.Add(KecaknoahReference.Right(KecaknoahNil.Instance, wp));
        }

        /// <summary>
        /// .NETメソッドをトップレベルに登録します。
        /// </summary>
        /// <param name="func">登録する<see cref="Func{T1, T2, TResult}"/>形式のメソッド</param>
        /// <param name="name">メソッド名</param>
        public void RegisterStringFunction(Func<KecaknoahObject, KecaknoahObject[], string> func, string name)
        {
            KecaknoahInteropDelegate wp =
                (ctx, self, args) => func(self, args).AsKecaknoahString().NoResume();
            var fo = new KecaknoahInteropMethodInfo(name, wp);
            topMethods.Add(fo);
            methodReferences.Add(KecaknoahReference.Right(KecaknoahNil.Instance, wp));
        }
        #endregion

        #region Stdlib Register
        /// <summary>
        /// 標準ライブラリを登録します。
        /// </summary>
        public void RegisterStandardLibraries()
        {
            RegisterClass(KecaknoahString.Information);
            RegisterClass(KecaknoahConvert.Information);
            RegisterClass(KecaknoahList.Information);
            RegisterClass(KecaknoahDictionary.Information);
            RegisterClass(KecaknoahDirectory.Information);
            RegisterClass(KecaknoahFile.Information);
            RegisterClass(KecaknoahMath.Information);
            RegisterClass(KecaknoahDynamicLibrary.Information);
            RegisterClass(KecaknoahHash.Information);
            RegisterClass(KecaknoahRegex.Information);
            RegisterClass(KecaknoahExtensionLibrary.Information);
        }
        #endregion

        private void ProcessUseDirective(IEnumerable<string> list)
        {
            var cur = Directory.GetCurrentDirectory();
            var lex = new KecaknoahLexer();
            var par = new KecaknoahParser();
            var prc = new KecaknoahPrecompiler();
            foreach (var text in list)
            {
                var arg = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                switch (arg[0])
                {
                    case "import":
                        var it = Path.Combine(cur, arg[1]);
                        Directory.SetCurrentDirectory(Path.GetDirectoryName(it));
                        var s = prc.PrecompileAll(par.Parse(lex.AnalyzeFromFile(it)));
                        RegisterSource(s);
                        Directory.SetCurrentDirectory(cur);
                        break;
                    case "exlib":
                        var et = Path.Combine(BaseDirectory, arg[1]);
                        Directory.SetCurrentDirectory(et);
                        var s2 = prc.PrecompileAll(par.Parse(lex.AnalyzeFromFile(Path.Combine(et, $"{arg[1]}.kt"))));
                        RegisterSource(s2);
                        Directory.SetCurrentDirectory(cur);
                        break;
                }
            }
        }
    }
}
