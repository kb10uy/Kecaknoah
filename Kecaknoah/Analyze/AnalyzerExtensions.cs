using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah.Analyze
{
    internal static class AnalyzerExtensions
    {
        private static List<KecaknoahTokenType> logicallines = new List<KecaknoahTokenType> {
            KecaknoahTokenType.NewLine,
            KecaknoahTokenType.Semicolon,
        };

        public static KecaknoahToken CreateToken(this Tuple<string, KecaknoahTokenType> tt, int col, int line)
            => new KecaknoahToken { Position = new Tuple<int, int>(col, line), TokenString = tt.Item1, Type = tt.Item2 };

        public static KecaknoahToken CreateTokenAsIdentifer(this string ls, int col, int line)
            => new KecaknoahToken { Position = new Tuple<int, int>(col, line), TokenString = ls, Type = KecaknoahTokenType.Identifer };

        public static KecaknoahToken CreateTokenAsDecimalNumber(this string ls, int col, int line)
            => new KecaknoahToken { Position = new Tuple<int, int>(col, line), TokenString = ls, Type = KecaknoahTokenType.DecimalNumberLiteral };

        public static KecaknoahToken CreateTokenAsBinaryNumber(this string ls, int col, int line)
            => new KecaknoahToken { Position = new Tuple<int, int>(col, line), TokenString = ls, Type = KecaknoahTokenType.BinaryNumberLiteral };

        public static KecaknoahToken CreateTokenAsOctadecimalNumber(this string ls, int col, int line)
            => new KecaknoahToken { Position = new Tuple<int, int>(col, line), TokenString = ls, Type = KecaknoahTokenType.OctadecimalNumberLiteral };

        public static KecaknoahToken CreateTokenAsHexadecimalNumber(this string ls, int col, int line)
            => new KecaknoahToken { Position = new Tuple<int, int>(col, line), TokenString = ls, Type = KecaknoahTokenType.HexadecimalNumberLiteral };

        public static KecaknoahToken CreateTokenAsHexatridecimalNumber(this string ls, int col, int line)
            => new KecaknoahToken { Position = new Tuple<int, int>(col, line), TokenString = ls, Type = KecaknoahTokenType.HexatridecimalNumberLiteral };

        public static KecaknoahError CreateErrorAt(this KecaknoahToken token, string message)
            => new KecaknoahError { Column = token.Position.Item1, Line = token.Position.Item2, Message = message };


        /// <summary>
        /// 飛ばせるなら飛ばせるだけ飛ばす
        /// </summary>
        /// <param name="tokens">リスト</param>
        /// <returns>1つ以上飛ばせたらtrue</returns>
        public static bool SkipLogicalLineBreak(this Queue<KecaknoahToken> tokens)
        {
            if (tokens.Count == 0) return false;
            if (!logicallines.Any(p => p == tokens.Peek().Type)) return false;
            do
            {
                tokens.Dequeue();
            } while (tokens.Count > 0 && logicallines.Any(p => p == tokens.Peek().Type));
            return true;
        }

        /// <summary>
        /// チェックして飛ばせたら飛ばす
        /// </summary>
        /// <param name="tokens">きゅう</param>
        /// <param name="tt">チェック対象</param>
        /// <returns>結果</returns>
        public static bool CheckSkipToken(this Queue<KecaknoahToken> tokens, params KecaknoahTokenType[] tt)
        {
            if (tokens.Count == 0) return false;
            if (!tt.Any(p => p == tokens.Peek().Type)) return false;
            tokens.Dequeue();
            return true;
        }

        /// <summary>
        /// チェックする
        /// </summary>
        /// <param name="tokens">きゅう</param>
        /// <param name="tt">チェック対象</param>
        /// <returns>結果</returns>
        public static bool CheckToken(this Queue<KecaknoahToken> tokens, params KecaknoahTokenType[] tt) => tokens.Count != 0 ? tt.Any(p => p == tokens.Peek().Type) : false;
    }
}
