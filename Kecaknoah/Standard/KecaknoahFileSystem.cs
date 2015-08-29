using Kecaknoah.Type;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah.Standard
{
    /// <summary>
    /// Kecaknoahでディレクトリ操作をします。
    /// </summary>
    public sealed class KecaknoahDirectory : KecaknoahObject
    {
        /// <summary>
        /// Kecaknoah上でのクラス名を取得します。
        /// </summary>
        public static readonly string ClassName = "Directory";

        #region 改変不要
        /// <summary>
        /// このクラスのクラスメソッドが定義される<see cref="KecaknoahInteropClassInfo"/>を取得します。
        /// こいつを適当なタイミングで<see cref="KecaknoahModule.RegisterClass(KecaknoahInteropClassInfo)"/>に
        /// 渡してください。
        /// </summary>
        internal static KecaknoahInteropClassInfo Information { get; } = new KecaknoahInteropClassInfo(ClassName);
        #endregion

        /// <summary>
        /// 主にInformationを初期化します。
        /// コンストラクタを含む全てのクラスメソッドはここから追加してください。
        /// 逆に登録しなければコンストラクタを隠蔽できるということでもありますが。
        /// </summary>
        static KecaknoahDirectory()
        {
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("create", ClassCreateDirectory));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("delete", ClassDeleteDirectory));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("change", ClassChangeDirectory));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("exists", ClassExists));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("current", ClassCurrentDirectory));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("get_dirs", ClassGetDirectories));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("get_files", ClassGetFiles));
        }

        /// <summary>
        /// このクラスのインスタンスを初期化します。
        /// 要するにこのインスタンスがスクリプト中で参照されるので、
        /// インスタンスメソッドやプロパティの設定を忘れずにしてください。
        /// あと<see cref="KecaknoahObject.ExtraType"/>に型名をセットしておくと便利です。
        /// </summary>
        public KecaknoahDirectory()
        {
            ExtraType = ClassName;
        }

        #region クラスメソッド
        /* 
        当たり前ですがクラスメソッド呼び出しではselfはnullになります。
        selfに代入するのではなく生成したのをNoResumeで返却します。
        */

        private static KecaknoahFunctionResult ClassCreateDirectory(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var sarg = args.ExpectString(1, false);
            Directory.CreateDirectory(sarg[0]);
            return KecaknoahNil.Instance.NoResume();
        }

        private static KecaknoahFunctionResult ClassExists(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var sarg = args.ExpectString(1, false);
            return Directory.Exists(sarg[0]).AsKecaknoahBoolean().NoResume();
        }

        private static KecaknoahFunctionResult ClassChangeDirectory(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var sarg = args.ExpectString(1, false);
            Directory.SetCurrentDirectory(sarg[0]);
            return KecaknoahNil.Instance.NoResume();
        }

        private static KecaknoahFunctionResult ClassDeleteDirectory(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var sarg = args.ExpectString(1, false);
            Directory.Delete(sarg[0]);
            return KecaknoahNil.Instance.NoResume();
        }

        private static KecaknoahFunctionResult ClassCurrentDirectory(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args) => Directory.GetCurrentDirectory().AsKecaknoahString().NoResume();


        private static KecaknoahFunctionResult ClassGetFiles(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var sarg = args.ExpectString(1, true);
            if (sarg.Count >= 2)
            {
                var list = Directory.GetFiles(sarg[0], sarg[1]);
                return new KecaknoahArray(list.Select(p => p.AsKecaknoahString())).NoResume();
            }
            else
            {
                var list = Directory.GetFiles(sarg[0]);
                return new KecaknoahArray(list.Select(p => p.AsKecaknoahString())).NoResume();
            }
        }

        private static KecaknoahFunctionResult ClassGetDirectories(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var sarg = args.ExpectString(1, true);
            if (sarg.Count >= 2)
            {
                var list = Directory.GetDirectories(sarg[0], sarg[1]);
                return new KecaknoahArray(list.Select(p => p.AsKecaknoahString())).NoResume();
            }
            else
            {
                var list = Directory.GetFiles(sarg[0]);
                return new KecaknoahArray(list.Select(p => p.AsKecaknoahString())).NoResume();
            }
        }
        #endregion
    }

    /// <summary>
    /// Kecaknoahでファイル操作をします。
    /// </summary>
    public sealed class KecaknoahFile : KecaknoahObject
    {
        /// <summary>
        /// Kecaknoah上でのクラス名を取得します。
        /// </summary>
        public static readonly string ClassName = "File";

        #region 改変不要
        /// <summary>
        /// このクラスのクラスメソッドが定義される<see cref="KecaknoahInteropClassInfo"/>を取得します。
        /// こいつを適当なタイミングで<see cref="KecaknoahModule.RegisterClass(KecaknoahInteropClassInfo)"/>に
        /// 渡してください。
        /// </summary>
        internal static KecaknoahInteropClassInfo Information { get; } = new KecaknoahInteropClassInfo(ClassName);
        #endregion

        /// <summary>
        /// 主にInformationを初期化します。
        /// コンストラクタを含む全てのクラスメソッドはここから追加してください。
        /// 逆に登録しなければコンストラクタを隠蔽できるということでもありますが。
        /// </summary>
        static KecaknoahFile()
        {
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("delete", ClassDeleteFile));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("exists", ClassExists));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("write_text", ClassWriteText));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("read_text", ClassReadText));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("write_lines", ClassWriteLines));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("read_lines", ClassReadLines));
        }

        /// <summary>
        /// このクラスのインスタンスを初期化します。
        /// 要するにこのインスタンスがスクリプト中で参照されるので、
        /// インスタンスメソッドやプロパティの設定を忘れずにしてください。
        /// あと<see cref="KecaknoahObject.ExtraType"/>に型名をセットしておくと便利です。
        /// </summary>
        public KecaknoahFile()
        {
            ExtraType = ClassName;
        }

        #region クラスメソッド
        /* 
        当たり前ですがクラスメソッド呼び出しではselfはnullになります。
        selfに代入するのではなく生成したのをNoResumeで返却します。
        */

        private static KecaknoahFunctionResult ClassExists(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var sarg = args.ExpectString(1, false);
            return File.Exists(sarg[0]).AsKecaknoahBoolean().NoResume();
        }

        private static KecaknoahFunctionResult ClassDeleteFile(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var sarg = args.ExpectString(1, false);
            File.Delete(sarg[0]);
            return KecaknoahNil.Instance.NoResume();
        }

        private static KecaknoahFunctionResult ClassWriteText(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var sarg = args.ExpectString(2, true);
            if (sarg.Count >= 3)
            {
                File.WriteAllText(sarg[0], sarg[1], Encoding.GetEncoding(sarg[2]));
            }
            else
            {
                File.WriteAllText(sarg[0], sarg[1]);
            }
            return KecaknoahNil.Instance.NoResume();
        }

        private static KecaknoahFunctionResult ClassReadText(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var sarg = args.ExpectString(1, true);
            var result = "";
            if (sarg.Count >= 2)
            {
                result = File.ReadAllText(sarg[0], Encoding.GetEncoding(sarg[1]));
            }
            else
            {
                result = File.ReadAllText(sarg[0]);
            }
            return KecaknoahNil.Instance.NoResume();
        }

        private static KecaknoahFunctionResult ClassWriteLines(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var path = args[0].ToString();
            var list = args[1].ToStringArray();
            if (args.Length >= 3)
            {
                var enc = Encoding.GetEncoding(args[2].ToString());
                File.WriteAllLines(path, list, enc);
            }
            else
            {
                File.WriteAllLines(path, list);
            }
            return KecaknoahNil.Instance.NoResume();
        }

        private static KecaknoahFunctionResult ClassReadLines(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var path = args[0].ToString();
            IList<string> list;
            if (args.Length >= 2)
            {
                var enc = Encoding.GetEncoding(args[1].ToString());
                list = File.ReadAllLines(path, enc);
            }
            else
            {
                list = File.ReadAllLines(path);
            }

            return new KecaknoahArray(list.Select(p => p.AsKecaknoahString())).NoResume();
        }
        #endregion
    }
}
