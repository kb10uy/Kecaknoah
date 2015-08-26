using System;
using System.Collections.Generic;
using System.Linq;

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
        /// ラムダ式などで<see cref="Predicate{T}"/>相当のオブジェクトが渡されたものとしてCallします。
        /// </summary>
        /// <param name="obj">対象</param>
        /// <param name="ctx">現在の<see cref="KecaknoahContext"/></param>
        /// <param name="tr">渡すオブジェクト</param>
        /// <returns></returns>
        public static bool CallAsPredicate(this KecaknoahObject obj, KecaknoahContext ctx, KecaknoahObject tr) => obj.Call(ctx, new[] { tr }).ReturningObject.ToBoolean();

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
        /// この<see cref="KecaknoahObject"/>のリストが指定した個数の<see cref="Int32"/>に変換可能であるとみなし、
        /// そのリストを返します。
        /// </summary>
        /// <param name="arr">対象</param>
        /// <param name="length">長さ</param>
        /// <param name="allowMore">
        /// 指定した長さを超えるリストであるときに全て変換する場合はtrueを指定します。
        /// 超過分を切り捨てる場合はfalseを指定します。
        /// </param>
        /// <returns>変換結果</returns>
        public static IList<int> ExpectInt32(this IList<KecaknoahObject> arr, int length, bool allowMore)
        {

            if (arr.Count < length) throw new ArgumentException("要素の数が足りません");
            return (allowMore ? arr : arr.Take(length)).Select(p => p.ToInt32()).ToList();
        }

        /// <summary>
        /// この<see cref="KecaknoahObject"/>のリストが指定した個数の<see cref="Int64"/>に変換可能であるとみなし、
        /// そのリストを返します。
        /// </summary>
        /// <param name="arr">対象</param>
        /// <param name="length">長さ</param>
        /// <param name="allowMore">
        /// 指定した長さを超えるリストであるときに全て変換する場合はtrueを指定します。
        /// 超過分を切り捨てる場合はfalseを指定します。
        /// </param>
        /// <returns>変換結果</returns>
        public static IList<long> ExpectInt64(this IList<KecaknoahObject> arr, int length, bool allowMore)
        {

            if (arr.Count < length) throw new ArgumentException("要素の数が足りません");
            return (allowMore ? arr : arr.Take(length)).Select(p => p.ToInt64()).ToList();
        }

        /// <summary>
        /// この<see cref="KecaknoahObject"/>のリストが指定した個数の<see cref="Single"/>に変換可能であるとみなし、
        /// そのリストを返します。
        /// </summary>
        /// <param name="arr">対象</param>
        /// <param name="length">長さ</param>
        /// <param name="allowMore">
        /// 指定した長さを超えるリストであるときに全て変換する場合はtrueを指定します。
        /// 超過分を切り捨てる場合はfalseを指定します。
        /// </param>
        /// <returns>変換結果</returns>
        public static IList<float> ExpectSingle(this IList<KecaknoahObject> arr, int length, bool allowMore)
        {

            if (arr.Count < length) throw new ArgumentException("要素の数が足りません");
            return (allowMore ? arr : arr.Take(length)).Select(p => (float)p.ToDouble()).ToList();
        }

        /// <summary>
        /// この<see cref="KecaknoahObject"/>のリストが指定した個数の<see cref="Double"/>に変換可能であるとみなし、
        /// そのリストを返します。
        /// </summary>
        /// <param name="arr">対象</param>
        /// <param name="length">長さ</param>
        /// <param name="allowMore">
        /// 指定した長さを超えるリストであるときに全て変換する場合はtrueを指定します。
        /// 超過分を切り捨てる場合はfalseを指定します。
        /// </param>
        /// <returns>変換結果</returns>
        public static IList<double> ExpectDouble(this IList<KecaknoahObject> arr, int length, bool allowMore)
        {

            if (arr.Count < length) throw new ArgumentException("要素の数が足りません");
            return (allowMore ? arr : arr.Take(length)).Select(p => p.ToDouble()).ToList();
        }

        /// <summary>
        /// この<see cref="KecaknoahObject"/>のリストが指定した個数の<see cref="String"/>に変換可能であるとみなし、
        /// そのリストを返します。
        /// </summary>
        /// <param name="arr">対象</param>
        /// <param name="length">長さ</param>
        /// <param name="allowMore">
        /// 指定した長さを超えるリストであるときに全て変換する場合はtrueを指定します。
        /// 超過分を切り捨てる場合はfalseを指定します。
        /// </param>
        /// <returns>変換結果</returns>
        public static IList<string> ExpectString(this IList<KecaknoahObject> arr, int length, bool allowMore)
        {

            if (arr.Count < length) throw new ArgumentException("要素の数が足りません");
            return (allowMore ? arr : arr.Take(length)).Select(p => p.ToString()).ToList();
        }

        /// <summary>
        /// この<see cref="KecaknoahObject"/>のリストが指定した<see cref="TypeCode"/>の順に従うとみなし、
        /// そのリストを返します。
        /// </summary>
        /// <param name="arr">対象</param>
        /// <param name="length">長さ</param>
        /// <param name="codes">
        /// 変換先の<see cref="TypeCode"/>のリスト。
        /// <para>
        ///     利用できるのは
        ///     <see cref="TypeCode.Int32"/>、
        ///     <see cref="TypeCode.Int64"/>、
        ///     <see cref="TypeCode.Single"/>、
        ///     <see cref="TypeCode.Double"/>、
        ///     <see cref="TypeCode.Boolean"/>、
        ///     <see cref="TypeCode.String"/>、
        ///     <see cref="TypeCode.Object"/>です。
        ///     <see cref="TypeCode.Object"/>を指定した場合、該当する<see cref="KecaknoahObject"/>は
        ///     変換されずそのまま格納されます。
        /// </para>
        /// <para>
        ///     また、
        ///     <see cref="TypeCode.Int32"/>、
        ///     <see cref="TypeCode.Single"/>を指定した場合
        ///     精度が失われる可能性があります。
        /// </para>
        /// </param>
        /// <param name="allowMore">
        /// 指定した長さを超えるリストであるときに全て変換する場合はtrueを指定します。
        /// 超過分を切り捨てる場合はfalseを指定します。
        /// 超過分は<see cref="TypeCode.Object"/>と同じ挙動になります。
        /// </param>
        /// <returns>変換結果</returns>
        public static IList<object> ExpectTypes(this IList<KecaknoahObject> arr, int length, IList<TypeCode> codes, bool allowMore)
        {
            if (arr.Count < length) throw new ArgumentException("要素の数が足りません");
            var result = new List<object>();
            var q = new Queue<KecaknoahObject>(arr);
            foreach (var i in codes)
            {
                var obj = q.Dequeue();
                switch(i)
                {
                    case TypeCode.Int32:
                        result.Add(obj.ToInt32());
                        break;
                    case TypeCode.Int64:
                        result.Add(obj.ToInt64());
                        break;
                    case TypeCode.Single:
                        result.Add((float)obj.ToDouble());
                        break;
                    case TypeCode.Double:
                        result.Add(obj.ToDouble());
                        break;
                    case TypeCode.Boolean:
                        result.Add(obj.ToBoolean());
                        break;
                    case TypeCode.String:
                        result.Add(obj.ToString());
                        break;
                    case TypeCode.Object:
                        result.Add(obj);
                        break;
                    default:
                        throw new ArgumentException("無効なTypeCode値です");
                }
            }
            if (allowMore) result.AddRange(q);
            return result;
        }

        /// <summary>
        /// この<see cref="KecaknoahObject"/>のリストが指定した<see cref="TypeCode"/>の順に従うとみなし、
        /// そのリストを返します。超過分は切り捨てられます。
        /// </summary>
        /// <param name="arr">対象</param>
        /// <param name="length">長さ</param>
        /// <param name="codes">
        /// 変換先の<see cref="TypeCode"/>のリスト。
        /// <para>
        ///     利用できるのは
        ///     <see cref="TypeCode.Int32"/>、
        ///     <see cref="TypeCode.Int64"/>、
        ///     <see cref="TypeCode.Single"/>、
        ///     <see cref="TypeCode.Double"/>、
        ///     <see cref="TypeCode.Boolean"/>、
        ///     <see cref="TypeCode.String"/>、
        ///     <see cref="TypeCode.Object"/>です。
        ///     <see cref="TypeCode.Object"/>を指定した場合、該当する<see cref="KecaknoahObject"/>は
        ///     変換されずそのまま格納されます。
        /// </para>
        /// <para>
        ///     また、
        ///     <see cref="TypeCode.Int32"/>、
        ///     <see cref="TypeCode.Single"/>を指定した場合
        ///     精度が失われる可能性があります。
        /// </para>
        /// </param>
        /// <returns>変換結果</returns>
        public static IList<object> ExpectTypes(this IList<KecaknoahObject> arr, int length, params TypeCode[] codes) => ExpectTypes(arr, length, codes, false);
    }
}
