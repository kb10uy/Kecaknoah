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
        /// <see cref="KecaknoahSingle"/>を生成します。
        /// </summary>
        /// <param name="num">対象の単精度浮動小数点数</param>
        /// <returns>結果</returns>
        public static KecaknoahSingle AsKecaknoahSingle(this float num) => new KecaknoahSingle { Value = num };

        /// <summary>
        /// <see cref="KecaknoahDouble"/>を生成します。
        /// </summary>
        /// <param name="num">対象の倍精度浮動小数点数</param>
        /// <returns>結果</returns>
        public static KecaknoahDouble AsKecaknoahDouble(this double num) => new KecaknoahDouble { Value = num };

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
        /// <see cref="KecaknoahInteropFunction"/>を生成します。
        /// </summary>
        /// <param name="val">対象の<see cref="KecaknoahDelegate"/></param>
        /// <returns>結果</returns>
        public static KecaknoahInteropFunction AsKecaknoahInteropFunction(this KecaknoahDelegate val) => new KecaknoahInteropFunction { Function = val };

        /// <summary>
        /// <see cref="Action"/>を<see cref="KecaknoahDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns></returns>
        public static KecaknoahDelegate AsKecaknoahDelegate(this Action func) =>
            (args) =>
            {
                func();
                return null;
            };

        /// <summary>
        /// <see cref="Action"/>を<see cref="KecaknoahDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns></returns>
        public static KecaknoahDelegate AsKecaknoahDelegate<T1>(this Action<T1> func) =>
            (args) =>
            {
                func((T1)args[0].AsRawObject<T1>());
                return null;
            };

        /// <summary>
        /// <see cref="Action"/>を<see cref="KecaknoahDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns></returns>
        public static KecaknoahDelegate AsKecaknoahDelegate<T1, T2>(this Action<T1, T2> func) =>
            (args) =>
            {
                func((T1)args[0].AsRawObject<T1>(), (T2)args[1].AsRawObject<T2>());
                return null;
            };

        /// <summary>
        /// <see cref="Action"/>を<see cref="KecaknoahDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns></returns>
        public static KecaknoahDelegate AsKecaknoahDelegate<T1, T2, T3>(this Action<T1, T2, T3> func) =>
            (args) =>
            {
                func((T1)args[0].AsRawObject<T1>(), (T2)args[1].AsRawObject<T2>(), (T3)args[2].AsRawObject<T3>());
                return null;
            };


        /// <summary>
        /// 整数を返す<see cref="Func{TResult}"/>を<see cref="KecaknoahDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahDelegate AsKecaknoahDelegate(this Func<int> func) => (args) => func().AsKecaknoahInteger();

        /// <summary>
        /// 整数を返す<see cref="Func{T, TResult}"/>を<see cref="KecaknoahDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahDelegate AsKecaknoahDelegate<T1>(this Func<T1, int> func) => (args) => func((T1)args[0].AsRawObject<T1>()).AsKecaknoahInteger();

        /// <summary>
        /// 文字列整数を返す<see cref="Func{T1, T2, TResult}"/>を<see cref="KecaknoahDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahDelegate AsKecaknoahDelegate<T1, T2>(this Func<T1, T2, int> func) => (args) => func((T1)args[0].AsRawObject<T1>(), (T2)args[1].AsRawObject<T2>()).AsKecaknoahInteger();

        /// <summary>
        /// 整数を返す<see cref="Func{T1, T2, T3, TResult}"/>を<see cref="KecaknoahDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahDelegate AsKecaknoahDelegate<T1, T2, T3>(this Func<T1, T2, T3, int> func) => (args) => func((T1)args[0].AsRawObject<T1>(), (T2)args[1].AsRawObject<T2>(), (T3)args[2].AsRawObject<T3>()).AsKecaknoahInteger();

        /// <summary>
        /// 整数を返す<see cref="Func{TResult}"/>を<see cref="KecaknoahDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahDelegate AsKecaknoahDelegate(this Func<long> func) => (args) => func().AsKecaknoahInteger();

        /// <summary>
        /// 整数を返す<see cref="Func{T, TResult}"/>を<see cref="KecaknoahDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahDelegate AsKecaknoahDelegate<T1>(this Func<T1, long> func) => (args) => func((T1)args[0].AsRawObject<T1>()).AsKecaknoahInteger();

        /// <summary>
        /// 文字列整数を返す<see cref="Func{T1, T2, TResult}"/>を<see cref="KecaknoahDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahDelegate AsKecaknoahDelegate<T1, T2>(this Func<T1, T2, long> func) => (args) => func((T1)args[0].AsRawObject<T1>(), (T2)args[1].AsRawObject<T2>()).AsKecaknoahInteger();

        /// <summary>
        /// 整数を返す<see cref="Func{T1, T2, T3, TResult}"/>を<see cref="KecaknoahDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahDelegate AsKecaknoahDelegate<T1, T2, T3>(this Func<T1, T2, T3, long> func) => (args) => func((T1)args[0].AsRawObject<T1>(), (T2)args[1].AsRawObject<T2>(), (T3)args[2].AsRawObject<T3>()).AsKecaknoahInteger();

        /// <summary>
        /// 文字列を返す<see cref="Func{TResult}"/>を<see cref="KecaknoahDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahDelegate AsKecaknoahDelegate(this Func<string> func) => (args) => func().AsKecaknoahString();

        /// <summary>
        /// 文字列を返す<see cref="Func{T, TResult}"/>を<see cref="KecaknoahDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahDelegate AsKecaknoahDelegate<T1>(this Func<T1, string> func) => (args) => func((T1)args[0].AsRawObject<T1>()).AsKecaknoahString();

        /// <summary>
        /// 文字列を返す<see cref="Func{T1, T2, TResult}"/>を<see cref="KecaknoahDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahDelegate AsKecaknoahDelegate<T1, T2>(this Func<T1, T2, string> func) => (args) => func((T1)args[0].AsRawObject<T1>(), (T2)args[1].AsRawObject<T2>()).AsKecaknoahString();

        /// <summary>
        /// 文字列を返す<see cref="Func{T1, T2, T3, TResult}"/>を<see cref="KecaknoahDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahDelegate AsKecaknoahDelegate<T1, T2, T3>(this Func<T1, T2, T3, string> func) => (args) => func((T1)args[0].AsRawObject<T1>(), (T2)args[1].AsRawObject<T2>(), (T3)args[2].AsRawObject<T3>()).AsKecaknoahString();

        /// <summary>
        /// 文字列を返す<see cref="Func{TResult}"/>を<see cref="KecaknoahDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahDelegate AsKecaknoahDelegate(this Func<double> func) => (args) => func().AsKecaknoahDouble();

        /// <summary>
        /// 文字列を返す<see cref="Func{T, TResult}"/>を<see cref="KecaknoahDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahDelegate AsKecaknoahDelegate<T1>(this Func<T1, double> func) => (args) => func((T1)args[0].AsRawObject<T1>()).AsKecaknoahDouble();

        /// <summary>
        /// 文字列を返す<see cref="Func{T1, T2, TResult}"/>を<see cref="KecaknoahDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahDelegate AsKecaknoahDelegate<T1, T2>(this Func<T1, T2, double> func) => (args) => func((T1)args[0].AsRawObject<T1>(), (T2)args[1].AsRawObject<T2>()).AsKecaknoahDouble();

        /// <summary>
        /// 文字列を返す<see cref="Func{T1, T2, T3, TResult}"/>を<see cref="KecaknoahDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahDelegate AsKecaknoahDelegate<T1, T2, T3>(this Func<T1, T2, T3, double> func) => (args) => func((T1)args[0].AsRawObject<T1>(), (T2)args[1].AsRawObject<T2>(), (T3)args[2].AsRawObject<T3>()).AsKecaknoahDouble();

        /// <summary>
        /// 文字列を返す<see cref="Func{TResult}"/>を<see cref="KecaknoahDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahDelegate AsKecaknoahDelegate(this Func<float> func) => (args) => func().AsKecaknoahSingle();

        /// <summary>
        /// 文字列を返す<see cref="Func{T, TResult}"/>を<see cref="KecaknoahDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahDelegate AsKecaknoahDelegate<T1>(this Func<T1, float> func) => (args) => func((T1)args[0].AsRawObject<T1>()).AsKecaknoahSingle();

        /// <summary>
        /// 文字列を返す<see cref="Func{T1, T2, TResult}"/>を<see cref="KecaknoahDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahDelegate AsKecaknoahDelegate<T1, T2>(this Func<T1, T2, float> func) => (args) => func((T1)args[0].AsRawObject<T1>(), (T2)args[1].AsRawObject<T2>()).AsKecaknoahSingle();

        /// <summary>
        /// 文字列を返す<see cref="Func{T1, T2, T3, TResult}"/>を<see cref="KecaknoahDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahDelegate AsKecaknoahDelegate<T1, T2, T3>(this Func<T1, T2, T3, float> func) => (args) => func((T1)args[0].AsRawObject<T1>(), (T2)args[1].AsRawObject<T2>(), (T3)args[2].AsRawObject<T3>()).AsKecaknoahSingle();

        /// <summary>
        /// 文字列を返す<see cref="Func{TResult}"/>を<see cref="KecaknoahDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahDelegate AsKecaknoahDelegate(this Func<bool> func) => (args) => func().AsKecaknoahBoolean();

        /// <summary>
        /// 文字列を返す<see cref="Func{T, TResult}"/>を<see cref="KecaknoahDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahDelegate AsKecaknoahDelegate<T1>(this Func<T1, bool> func) => (args) => func((T1)args[0].AsRawObject<T1>()).AsKecaknoahBoolean();

        /// <summary>
        /// 文字列を返す<see cref="Func{T1, T2, TResult}"/>を<see cref="KecaknoahDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahDelegate AsKecaknoahDelegate<T1, T2>(this Func<T1, T2, bool> func) => (args) => func((T1)args[0].AsRawObject<T1>(), (T2)args[1].AsRawObject<T2>()).AsKecaknoahBoolean();

        /// <summary>
        /// 文字列を返す<see cref="Func{T1, T2, T3, TResult}"/>を<see cref="KecaknoahDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahDelegate AsKecaknoahDelegate<T1, T2, T3>(this Func<T1, T2, T3, bool> func) => (args) => func((T1)args[0].AsRawObject<T1>(), (T2)args[1].AsRawObject<T2>(), (T3)args[2].AsRawObject<T3>()).AsKecaknoahBoolean();
    }
}
