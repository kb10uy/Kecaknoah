using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah
{
    internal static class Extensions
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


        public static bool SkipLogicalLineBreak(this Queue<KecaknoahToken> tokens)
        {
            if (!logicallines.Any(p => p == tokens.Peek().Type)) return false;
            do
            {
                tokens.Dequeue();
            } while (logicallines.Any(p => p == tokens.Peek().Type));
            return true;
        }
    }
}
