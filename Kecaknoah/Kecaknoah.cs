using System;
using System.Text;
using Kecaknoah.Type;
using Kecaknoah.Analyze;

namespace Kecaknoah
{
    /// <summary>
    /// Kecaknoah Langugageを解析し、実行する環境を定義します。
    /// </summary>
    public sealed class Kecaknoah
    {
        /// <summary>
        /// 利用される<see cref="KecaknoahEnvironment"/>を取得します。
        /// </summary>
        public KecaknoahEnvironment Environment { get; } = new KecaknoahEnvironment();

        /// <summary>
        /// 利用される<see cref="KecaknoahLexer"/>を取得します。
        /// </summary>
        public KecaknoahLexer Lexer { get; } = new KecaknoahLexer();

        /// <summary>
        /// 利用される<see cref="KecaknoahLexer"/>を取得します。
        /// </summary>
        public KecaknoahParser Parser { get; } = new KecaknoahParser();

        /// <summary>
        /// 利用される<see cref="KecaknoahPrecompiler"/>を取得します。
        /// </summary>
        public KecaknoahPrecompiler Precompiler { get; } = new KecaknoahPrecompiler();

        /// <summary>
        /// 現在の<see cref="KecaknoahModule"/>を取得します。
        /// </summary>
        public KecaknoahModule CurrentModule { get; private set; }

        /// <summary>
        /// モジュールを作成し、<see cref="CurrentModule"/>に設定します。
        /// </summary>
        /// <param name="name">名前</param>
        /// <returns>作成されたモジュール</returns>
        public KecaknoahModule CreateModule(string name)
        {
            var m = Environment.CreateModule(name);
            CurrentModule = m;
            return m;
        }

        /// <summary>
        /// <see cref="CurrentModule"/>でファイルを読み込み、実行します。
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        public KecaknoahObject DoFile(string fileName) => DoFile(fileName, Encoding.Default);

        /// <summary>
        /// <see cref="CurrentModule"/>でファイルを指定したエンコードで読み込み、実行します。
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <param name="enc">読み込む際に利用する<see cref="Encoding"/></param>
        public KecaknoahObject DoFile(string fileName, Encoding enc)
        {
            var le = Lexer.AnalyzeFromFile(fileName, enc);
            if (!le.Success) throw new KecaknoahException(le.Error);
            var ast = Parser.Parse(le);
            if (!ast.Success) throw new KecaknoahException(le.Error);
            var src = Precompiler.PrecompileAll(ast);
            CurrentModule.RegisterSource(src);
            if (CurrentModule["main"] != KecaknoahNil.Instance)
            {
                return new KecaknoahContext(CurrentModule).CallInstant(CurrentModule["main"]);
            }
            else
            {
                return KecaknoahNil.Instance;
            }
        }

        /// <summary>
        /// <see cref="CurrentModule"/>で指定したソースコードを直接解析し、実行します。
        /// </summary>
        /// <param name="source">ソースコード</param>
        public KecaknoahObject DoString(string source)
        {
            var le = Lexer.AnalyzeFromSource(source);
            if (!le.Success) throw new KecaknoahException(le.Error);
            var ast = Parser.Parse(le);
            if (!ast.Success) throw new KecaknoahException(le.Error);
            var src = Precompiler.PrecompileAll(ast);
            CurrentModule.RegisterSource(src);
            if (CurrentModule["main"] != KecaknoahNil.Instance)
            {
                return new KecaknoahContext(CurrentModule).CallInstant(CurrentModule["main"]);
            }
            else
            {
                return KecaknoahNil.Instance;
            }
        }

        /// <summary>
        /// <see cref="CurrentModule"/>で指定したソースコードを式として解析し、実行します。
        /// </summary>
        /// <param name="source">ソースコード</param>
        public KecaknoahObject DoExpressionString(string source)
        {
            var le = Lexer.AnalyzeFromSource(source);
            if (!le.Success) throw new KecaknoahException(le.Error);
            var ast = Parser.ParseAsExpression(le);
            if (!ast.Success) throw new KecaknoahException(le.Error);
            var src = Precompiler.PrecompileExpression(ast);
            return new KecaknoahContext(CurrentModule).ExecuteExpressionIL(src);
        }
    }

    /// <summary>
    /// Kecaknoah
    /// </summary>
    public sealed class KecaknoahException : Exception
    {
        /// <summary>
        /// 発生したエラーを取得します。
        /// </summary>
        public KecaknoahError Error { get; }

        internal KecaknoahException(KecaknoahError err) : base($"解析中にエラーが発生しました: {err.Message}")
        {
            Error = err;
        }

        internal KecaknoahException(string err) : base($"実行中にエラーが発生しました: {err}")
        {

        }
    }
}
