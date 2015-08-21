using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah.Type
{
    /// <summary>
    /// Kecaknoahの型システムに関するヘルパーメソッドを提供します。
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// <see cref="KecaknoahInteger"/>を生成します。
        /// </summary>
        /// <param name="num">対象の64bit整数</param>
        /// <returns>結果</returns>
        public static KecaknoahInteger AsKecaknoahInteger(this long num) => new KecaknoahInteger { Value = num };

        /// <summary>
        /// <see cref="KecaknoahInteger"/>を生成します。
        /// </summary>
        /// <param name="num">対象の32bit整数</param>
        /// <returns>結果</returns>
        public static KecaknoahInteger AsKecaknoahInteger(this int num) => new KecaknoahInteger { Value = num };

        /// <summary>
        /// <see cref="KecaknoahFloat"/>を生成します。
        /// </summary>
        /// <param name="num">対象の倍精度浮動小数点数</param>
        /// <returns>結果</returns>
        public static KecaknoahFloat AsKecaknoahFloat(this double num) => new KecaknoahFloat { Value = num };

        /// <summary>
        /// <see cref="KecaknoahString"/>を生成します。
        /// </summary>
        /// <param name="val">対象の文字列</param>
        /// <returns>結果</returns>
        public static KecaknoahString AsKecaknoahString(this string val) => new KecaknoahString { Value = val };

        /// <summary>
        /// <see cref="KecaknoahBoolean"/>を生成します。
        /// </summary>
        /// <param name="val">対象の値</param>
        /// <returns>結果</returns>
        public static KecaknoahBoolean AsKecaknoahBoolean(this bool val) => new KecaknoahBoolean { Value = val };

        /// <summary>
        /// nil化します。
        /// </summary>
        /// <param name="obj">値</param>
        /// <returns>nil</returns>
        public static KecaknoahNil AsNil(this KecaknoahObject obj) => KecaknoahNil.Instance;

        /// <summary>
        /// <see cref="KecaknoahInteropFunction"/>を生成します。
        /// </summary>
        /// <param name="val">対象の<see cref="KecaknoahInteropDelegate"/></param>
        /// <returns>結果</returns>
        public static KecaknoahInteropFunction AsKecaknoahInteropFunction(this KecaknoahInteropDelegate val) => new KecaknoahInteropFunction { Function = val };

        /// <summary>
        /// <see cref="Action"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns></returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate(this Action func) =>
            (self, args) =>
            {
                func();
                return null;
            };

        /// <summary>
        /// <see cref="Action"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns></returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate<T1>(this Action<T1> func) =>
            (self, args) =>
            {
                func((T1)args[0].Value);
                return null;
            };

        /// <summary>
        /// <see cref="Action"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns></returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate<T1, T2>(this Action<T1, T2> func) =>
            (self, args) =>
            {
                func((T1)args[0].Value, (T2)args[1].Value);
                return null;
            };

        /// <summary>
        /// <see cref="Action"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns></returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate<T1, T2, T3>(this Action<T1, T2, T3> func) =>
            (self, args) =>
            {
                func((T1)args[0].Value, (T2)args[1].Value, (T3)args[2].Value);
                return null;
            };


        /// <summary>
        /// 整数を返す<see cref="Func{TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate(this Func<int> func) => (self, args) => func().AsKecaknoahInteger();

        /// <summary>
        /// 整数を返す<see cref="Func{T, TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate<T1>(this Func<T1, int> func) => (self, args) => func((T1)args[0].Value).AsKecaknoahInteger();

        /// <summary>
        /// 文字列整数を返す<see cref="Func{T1, T2, TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate<T1, T2>(this Func<T1, T2, int> func) => (self, args) => func((T1)args[0].Value, (T2)args[1].Value).AsKecaknoahInteger();

        /// <summary>
        /// 整数を返す<see cref="Func{T1, T2, T3, TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate<T1, T2, T3>(this Func<T1, T2, T3, int> func) => (self, args) => func((T1)args[0].Value, (T2)args[1].Value, (T3)args[2].Value).AsKecaknoahInteger();

        /// <summary>
        /// 整数を返す<see cref="Func{TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate(this Func<long> func) => (self, args) => func().AsKecaknoahInteger();

        /// <summary>
        /// 整数を返す<see cref="Func{T, TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate<T1>(this Func<T1, long> func) => (self, args) => func((T1)args[0].Value).AsKecaknoahInteger();

        /// <summary>
        /// 文字列整数を返す<see cref="Func{T1, T2, TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate<T1, T2>(this Func<T1, T2, long> func) => (self, args) => func((T1)args[0].Value, (T2)args[1].Value).AsKecaknoahInteger();

        /// <summary>
        /// 整数を返す<see cref="Func{T1, T2, T3, TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate<T1, T2, T3>(this Func<T1, T2, T3, long> func) => (self, args) => func((T1)args[0].Value, (T2)args[1].Value, (T3)args[2].Value).AsKecaknoahInteger();

        /// <summary>
        /// 文字列を返す<see cref="Func{TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate(this Func<string> func) => (self, args) => func().AsKecaknoahString();

        /// <summary>
        /// 文字列を返す<see cref="Func{T, TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate<T1>(this Func<T1, string> func) => (self, args) => func((T1)args[0].Value).AsKecaknoahString();

        /// <summary>
        /// 文字列を返す<see cref="Func{T1, T2, TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate<T1, T2>(this Func<T1, T2, string> func) => (self, args) => func((T1)args[0].Value, (T2)args[1].Value).AsKecaknoahString();

        /// <summary>
        /// 文字列を返す<see cref="Func{T1, T2, T3, TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate<T1, T2, T3>(this Func<T1, T2, T3, string> func) => (self, args) => func((T1)args[0].Value, (T2)args[1].Value, (T3)args[2].Value).AsKecaknoahString();

        /// <summary>
        /// 文字列を返す<see cref="Func{TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate(this Func<double> func) => (self, args) => func().AsKecaknoahFloat();

        /// <summary>
        /// 文字列を返す<see cref="Func{T, TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate<T1>(this Func<T1, double> func) => (self, args) => func((T1)args[0].Value).AsKecaknoahFloat();

        /// <summary>
        /// 文字列を返す<see cref="Func{T1, T2, TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate<T1, T2>(this Func<T1, T2, double> func) => (self, args) => func((T1)args[0].Value, (T2)args[1].Value).AsKecaknoahFloat();

        /// <summary>
        /// 文字列を返す<see cref="Func{T1, T2, T3, TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate<T1, T2, T3>(this Func<T1, T2, T3, double> func) => (self, args) => func((T1)args[0].Value, (T2)args[1].Value, (T3)args[2].Value).AsKecaknoahFloat();

        /// <summary>
        /// 文字列を返す<see cref="Func{TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate(this Func<bool> func) => (self, args) => func().AsKecaknoahBoolean();

        /// <summary>
        /// 文字列を返す<see cref="Func{T, TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate<T1>(this Func<T1, bool> func) => (self, args) => func((T1)args[0].Value).AsKecaknoahBoolean();

        /// <summary>
        /// 文字列を返す<see cref="Func{T1, T2, TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate<T1, T2>(this Func<T1, T2, bool> func) => (self, args) => func((T1)args[0].Value, (T2)args[1].Value).AsKecaknoahBoolean();

        /// <summary>
        /// 文字列を返す<see cref="Func{T1, T2, T3, TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate<T1, T2, T3>(this Func<T1, T2, T3, bool> func) => (self, args) => func((T1)args[0].Value, (T2)args[1].Value, (T3)args[2].Value).AsKecaknoahBoolean();
    }
}
