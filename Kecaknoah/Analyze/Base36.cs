/*
 * from tallesl/csharpbase36 (https://github.com/tallesl/csharpbase36)
 *
 * LICENSE----------------------------------------------------------------
 * This is free and unencumbered software released into the public domain.
 *
 * Anyone is free to copy, modify, publish, use, compile, sell, or
 * distribute this software, either in source code form or as a compiled
 * binary, for any purpose, commercial or non-commercial, and by any
 * means.
 *
 * In jurisdictions that recognize copyright laws, the author or authors
 * of this software dedicate any and all copyright interest in the
 * software to the public domain. We make this dedication for the benefit
 * of the public at large and to the detriment of our heirs and
 * successors. We intend this dedication to be an overt act of
 * relinquishment in perpetuity of all present and future rights to this
 * software under copyright law.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
 * OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 * For more information, please refer to <http://unlicense.org>
 */
using System;
using System.Linq;
using System.Numerics;

namespace Base36Encoder
{
    internal static class Base36
    {
        private const string Digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static long Decode(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Empty value.");
            value = value.ToUpper();
            bool negative = false;
            if (value[0] == '-')
            {
                negative = true;
                value = value.Substring(1, value.Length - 1);
            }
            if (value.Any(c => !Digits.Contains(c)))
                throw new ArgumentException("Invalid value: \"" + value + "\".");
            var decoded = 0L;
            for (var i = 0; i < value.Length; ++i)
                decoded += Digits.IndexOf(value[i]) * (long)BigInteger.Pow(Digits.Length, value.Length - i - 1);
            return negative ? decoded * -1 : decoded;
        }

        public static string Encode(long value)
        {
            if (value == long.MinValue)
            {
                //hard coded value due to error when getting absolute value below: "Negating the minimum value of a twos complement number is invalid.".
                return "-1Y2P0IJ32E8E8";
            }
            bool negative = value < 0;
            value = Math.Abs(value);
            string encoded = string.Empty;
            do
                encoded = Digits[(int)(value % Digits.Length)] + encoded;
            while ((value /= Digits.Length) != 0);
            return negative ? "-" + encoded : encoded;
        }
    }
}