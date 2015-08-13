using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah.Analyze
{
    /// <summary>
    /// KecaknoahLexerの解析結果を定義します。
    /// </summary>
    public sealed class KecaknoahLexResult
    {
        /// <summary>
        /// 解析元のソースコードの名前を取得します。
        /// ファイルから解析した場合はデフォルトでファイル名になります。
        /// </summary>
        public string SourceName { get; }

        /// <summary>
        /// 字句解析が成功した場合はtrueになります。
        /// </summary>
        public bool Success { get; internal set; } = false;

        /// <summary>
        /// 解析が失敗した場合のエラー情報を取得します。
        /// </summary>
        public KecaknoahError Error { get; internal set; } = null;

        private List<KecaknoahToken> tokens = new List<KecaknoahToken>();
        /// <summary>
        /// 解析したトークンを取得します。
        /// </summary>
        /// <remarks>失敗した場合でも解析できたポイントまでのトークンが格納されます。</remarks>
        public IReadOnlyList<KecaknoahToken> Tokens { get; }

        /// <summary>
        /// ソース名を指定して初期化します。
        /// </summary>
        /// <param name="name"></param>
        public KecaknoahLexResult(string name)
        {
            SourceName = name;
            Tokens = tokens;
        }

        /// <summary>
        /// トークンを追加します。
        /// </summary>
        /// <param name="token">トークン</param>
        internal void AddToken(KecaknoahToken token) => tokens.Add(token);
    }

    /// <summary>
    /// 字句解析のエラーを定義します。
    /// </summary>
    public sealed class KecaknoahError
    {
        /// <summary>
        /// 列位置を取得します。
        /// </summary>
        public int Column { get; internal set; }

        /// <summary>
        /// 行位置を取得します。
        /// </summary>
        public int Line { get; internal set; }

        /// <summary>
        /// エラーメッセージを取得します。
        /// </summary>
        public string Message { get; internal set; }
    }
}
