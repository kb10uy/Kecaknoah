using System;

namespace Kecaknoah.Type
{
    /// <summary>
    /// Kecaknoahの型システムに関するヘルパーメソッドを提供します。
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// 再開可能です。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static KecaknoahFunctionResult CanResume(this KecaknoahObject obj) => new KecaknoahFunctionResult(obj, true);

        /// <summary>
        /// 再開不可能です。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static KecaknoahFunctionResult NoResume(this KecaknoahObject obj) => new KecaknoahFunctionResult(obj, false);

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
        /// <see cref="KecaknoahInteropDelegate"/>への参照を生成します。
        /// </summary>
        /// <param name="dele"></param>
        /// <param name="self">属するインスタンス</param>
        /// <returns></returns>
        public static KecaknoahReference CreateReference(this KecaknoahInteropDelegate dele, KecaknoahObject self) => KecaknoahReference.CreateRightReference(new KecaknoahInteropFunction(self, dele));

        /// <summary>
        /// <see cref="KecaknoahInteropFunction"/>を生成します。
        /// </summary>
        /// <param name="val">対象の<see cref="KecaknoahInteropDelegate"/></param>
        /// <returns>結果</returns>
        public static KecaknoahInteropFunction AsKecaknoahInteropFunction(this KecaknoahInteropDelegate val) => new KecaknoahInteropFunction(null, val);

        /// <summary>
        /// <see cref="Action"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns></returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate(this Action func) =>
            (ctx, self, args) =>
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
            (ctx, self, args) =>
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
            (ctx, self, args) =>
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
            (ctx, self, args) =>
            {
                func((T1)args[0].Value, (T2)args[1].Value, (T3)args[2].Value);
                return null;
            };


        /// <summary>
        /// 整数を返す<see cref="Func{TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate(this Func<int> func) => (ctx, self, args) => func().AsKecaknoahInteger().NoResume();

        /// <summary>
        /// 整数を返す<see cref="Func{T, TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate<T1>(this Func<T1, int> func) => (ctx, self, args) => func((T1)args[0].Value).AsKecaknoahInteger().NoResume();

        /// <summary>
        /// 文字列整数を返す<see cref="Func{T1, T2, TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate<T1, T2>(this Func<T1, T2, int> func) => (ctx, self, args) => func((T1)args[0].Value, (T2)args[1].Value).AsKecaknoahInteger().NoResume();

        /// <summary>
        /// 整数を返す<see cref="Func{T1, T2, T3, TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate<T1, T2, T3>(this Func<T1, T2, T3, int> func) => (ctx, self, args) => func((T1)args[0].Value, (T2)args[1].Value, (T3)args[2].Value).AsKecaknoahInteger().NoResume();

        /// <summary>
        /// 整数を返す<see cref="Func{TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate(this Func<long> func) => (ctx, self, args) => func().AsKecaknoahInteger().NoResume();

        /// <summary>
        /// 整数を返す<see cref="Func{T, TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate<T1>(this Func<T1, long> func) => (ctx, self, args) => func((T1)args[0].Value).AsKecaknoahInteger().NoResume();

        /// <summary>
        /// 文字列整数を返す<see cref="Func{T1, T2, TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate<T1, T2>(this Func<T1, T2, long> func) => (ctx, self, args) => func((T1)args[0].Value, (T2)args[1].Value).AsKecaknoahInteger().NoResume();

        /// <summary>
        /// 整数を返す<see cref="Func{T1, T2, T3, TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate<T1, T2, T3>(this Func<T1, T2, T3, long> func) => (ctx, self, args) => func((T1)args[0].Value, (T2)args[1].Value, (T3)args[2].Value).AsKecaknoahInteger().NoResume();

        /// <summary>
        /// 文字列を返す<see cref="Func{TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate(this Func<string> func) => (ctx, self, args) => func().AsKecaknoahString().NoResume();

        /// <summary>
        /// 文字列を返す<see cref="Func{T, TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate<T1>(this Func<T1, string> func) => (ctx, self, args) => func((T1)args[0].Value).AsKecaknoahString().NoResume();

        /// <summary>
        /// 文字列を返す<see cref="Func{T1, T2, TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate<T1, T2>(this Func<T1, T2, string> func) => (ctx, self, args) => func((T1)args[0].Value, (T2)args[1].Value).AsKecaknoahString().NoResume();

        /// <summary>
        /// 文字列を返す<see cref="Func{T1, T2, T3, TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate<T1, T2, T3>(this Func<T1, T2, T3, string> func) => (ctx, self, args) => func((T1)args[0].Value, (T2)args[1].Value, (T3)args[2].Value).AsKecaknoahString().NoResume();

        /// <summary>
        /// 文字列を返す<see cref="Func{TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate(this Func<double> func) => (ctx, self, args) => func().AsKecaknoahFloat().NoResume();

        /// <summary>
        /// 文字列を返す<see cref="Func{T, TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate<T1>(this Func<T1, double> func) => (ctx, self, args) => func((T1)args[0].Value).AsKecaknoahFloat().NoResume();

        /// <summary>
        /// 文字列を返す<see cref="Func{T1, T2, TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate<T1, T2>(this Func<T1, T2, double> func) => (ctx, self, args) => func((T1)args[0].Value, (T2)args[1].Value).AsKecaknoahFloat().NoResume();

        /// <summary>
        /// 文字列を返す<see cref="Func{T1, T2, T3, TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate<T1, T2, T3>(this Func<T1, T2, T3, double> func) => (ctx, self, args) => func((T1)args[0].Value, (T2)args[1].Value, (T3)args[2].Value).AsKecaknoahFloat().NoResume();

        /// <summary>
        /// 文字列を返す<see cref="Func{TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate(this Func<bool> func) => (ctx, self, args) => func().AsKecaknoahBoolean().NoResume();

        /// <summary>
        /// 文字列を返す<see cref="Func{T, TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate<T1>(this Func<T1, bool> func) => (ctx, self, args) => func((T1)args[0].Value).AsKecaknoahBoolean().NoResume();

        /// <summary>
        /// 文字列を返す<see cref="Func{T1, T2, TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate<T1, T2>(this Func<T1, T2, bool> func) => (ctx, self, args) => func((T1)args[0].Value, (T2)args[1].Value).AsKecaknoahBoolean().NoResume();

        /// <summary>
        /// 文字列を返す<see cref="Func{T1, T2, T3, TResult}"/>を<see cref="KecaknoahInteropDelegate"/>に適合するようにラップします。
        /// </summary>
        /// <param name="func">対象のメソッド</param>
        /// <returns>結果</returns>
        public static KecaknoahInteropDelegate AsKecaknoahInteropDelegate<T1, T2, T3>(this Func<T1, T2, T3, bool> func) => (ctx, self, args) => func((T1)args[0].Value, (T2)args[1].Value, (T3)args[2].Value).AsKecaknoahBoolean().NoResume();
    }
}
