using Kecaknoah.Standard;
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
        #region KecaknoahFunctionResult拡張
        /// <summary>
        /// 再開可能です。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static KecaknoahFunctionResult CanResume(this KecaknoahObject obj) => new KecaknoahFunctionResult(obj, KecaknoahFunctionResultType.CanResume);

        /// <summary>
        /// 再開不可能です。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static KecaknoahFunctionResult NoResume(this KecaknoahObject obj) => new KecaknoahFunctionResult(obj, KecaknoahFunctionResultType.NoResume);

        /// <summary>
        /// 例外です。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static KecaknoahFunctionResult Exception(this KecaknoahObject obj) => new KecaknoahFunctionResult(obj, KecaknoahFunctionResultType.Exception);
        #endregion

        #region .NETオブジェクト拡張
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
        /// <see cref="KecaknoahInteropFunction"/>を生成します。
        /// </summary>
        /// <param name="val">対象の<see cref="KecaknoahInteropDelegate"/></param>
        /// <returns>結果</returns>
        public static KecaknoahInteropFunction AsKecaknoahInteropFunction(this KecaknoahInteropDelegate val) => new KecaknoahInteropFunction(null, val);

        /// <summary>
        /// nil化します。
        /// </summary>
        /// <param name="obj">値</param>
        /// <returns>nil</returns>
        public static KecaknoahNil AsNil(this KecaknoahObject obj) => KecaknoahNil.Instance;
        #endregion

        #region 引数配列関係
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
        public static IList<object> ExpectTypes(this IList<KecaknoahObject> arr, IList<TypeCode> codes, bool allowMore)
        {
            if (arr.Count < codes.Count) throw new ArgumentException("要素の数が足りません");
            var result = new List<object>();
            var q = new Queue<KecaknoahObject>(arr);
            foreach (var i in codes)
            {
                var obj = q.Dequeue();
                switch (i)
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
        public static IList<object> ExpectTypes(this IList<KecaknoahObject> arr, params TypeCode[] codes) => ExpectTypes(arr, codes, false);
        #endregion

        #region KecaknoahObject配列化関係
        /// <summary>
        /// この<see cref="KecaknoahObject"/>が配列・リストなどの列挙オブジェクトであるとみなし、
        /// lengthと[]を用いて<see cref="string"/>のリストに変換します。
        /// </summary>
        /// <param name="obj">対象</param>
        /// <returns>結果</returns>
        public static IList<string> ToStringArray(this KecaknoahObject obj)
        {
            var len = obj["length"].ToInt64();
            var result = new List<string>();
            for (int i = 0; i < len; i++)
            {
                result.Add(obj.GetIndexerReference(new[] { i.AsKecaknoahInteger() }).RawObject.ToString());
            }
            return result;
        }

        /// <summary>
        /// この<see cref="KecaknoahObject"/>が配列・リストなどの列挙オブジェクトであるとみなし、
        /// lengthと[]を用いて<see cref="int"/>のリストに変換します。
        /// </summary>
        /// <param name="obj">対象</param>
        /// <returns>結果</returns>
        public static IList<int> ToInt32Array(this KecaknoahObject obj)
        {
            var len = obj["length"].ToInt64();
            var result = new List<int>();
            for (int i = 0; i < len; i++)
            {
                result.Add(obj.GetIndexerReference(new[] { i.AsKecaknoahInteger() }).RawObject.ToInt32());
            }
            return result;
        }

        /// <summary>
        /// この<see cref="KecaknoahObject"/>が配列・リストなどの列挙オブジェクトであるとみなし、
        /// lengthと[]を用いて文<see cref="long"/>のリストに変換します。
        /// </summary>
        /// <param name="obj">対象</param>
        /// <returns>結果</returns>
        public static IList<long> ToInt64Array(this KecaknoahObject obj)
        {
            var len = obj["length"].ToInt64();
            var result = new List<long>();
            for (int i = 0; i < len; i++)
            {
                result.Add(obj.GetIndexerReference(new[] { i.AsKecaknoahInteger() }).RawObject.ToInt64());
            }
            return result;
        }

        /// <summary>
        /// この<see cref="KecaknoahObject"/>が配列・リストなどの列挙オブジェクトであるとみなし、
        /// lengthと[]を用いて文<see cref="double"/>のリストに変換します。
        /// </summary>
        /// <param name="obj">対象</param>
        /// <returns>結果</returns>
        public static IList<double> ToDoubleArray(this KecaknoahObject obj)
        {
            var len = obj["length"].ToInt64();
            var result = new List<double>();
            for (int i = 0; i < len; i++)
            {
                result.Add(obj.GetIndexerReference(new[] { i.AsKecaknoahInteger() }).RawObject.ToDouble());
            }
            return result;
        }

        /// <summary>
        /// この<see cref="KecaknoahObject"/>が配列・リストなどの列挙オブジェクトであるとみなし、
        /// lengthと[]を用いて文<see cref="bool"/>のリストに変換します。
        /// </summary>
        /// <param name="obj">対象</param>
        /// <returns>結果</returns>
        public static IList<bool> ToBooleanArray(this KecaknoahObject obj)
        {
            var len = obj["length"].ToInt64();
            var result = new List<bool>();
            for (int i = 0; i < len; i++)
            {
                result.Add(obj.GetIndexerReference(new[] { i.AsKecaknoahInteger() }).RawObject.ToBoolean());
            }
            return result;
        }

        /// <summary>
        /// この<see cref="KecaknoahObject"/>が配列・リストなどの列挙オブジェクトであるとみなし、
        /// lengthと[]を用いて文<see cref="KecaknoahObject"/>のリストに変換します。
        /// </summary>
        /// <param name="obj">対象</param>
        /// <returns>結果</returns>
        public static IList<KecaknoahObject> AsArray(this KecaknoahObject obj)
        {
            var len = obj["length"].ToInt64();
            var result = new List<KecaknoahObject>();
            for (int i = 0; i < len; i++)
            {
                result.Add(obj.GetIndexerReference(new[] { i.AsKecaknoahInteger() }).RawObject);
            }
            return result;
        }

        #endregion

        #region KecaknoahArray拡張
        /// <summary>
        /// このリストを<see cref="KecaknoahArray"/>に変換します。
        /// </summary>
        /// <param name="ol">対象</param>
        /// <returns></returns>
        public static KecaknoahArray ToKecaknoahArray(this IList<KecaknoahObject> ol) => new KecaknoahArray(ol);

        /// <summary>
        /// このリストを<see cref="KecaknoahArray"/>に変換します。
        /// </summary>
        /// <param name="ol">対象</param>
        /// <returns></returns>
        public static KecaknoahArray ToKecaknoahArray(this IList<int> ol) => new KecaknoahArray(ol.Select(p => p.AsKecaknoahInteger()));

        /// <summary>
        /// このリストを<see cref="KecaknoahArray"/>に変換します。
        /// </summary>
        /// <param name="ol">対象</param>
        /// <returns></returns>
        public static KecaknoahArray ToKecaknoahArray(this IList<long> ol) => new KecaknoahArray(ol.Select(p => p.AsKecaknoahInteger()));

        /// <summary>
        /// このリストを<see cref="KecaknoahArray"/>に変換します。
        /// </summary>
        /// <param name="ol">対象</param>
        /// <returns></returns>
        public static KecaknoahArray ToKecaknoahArray(this IList<double> ol) => new KecaknoahArray(ol.Select(p => p.AsKecaknoahFloat()));

        /// <summary>
        /// このリストを<see cref="KecaknoahArray"/>に変換します。
        /// </summary>
        /// <param name="ol">対象</param>
        /// <returns></returns>
        public static KecaknoahArray ToKecaknoahArray(this IList<bool> ol) => new KecaknoahArray(ol.Select(p => p.AsKecaknoahBoolean()));

        /// <summary>
        /// このリストを<see cref="KecaknoahArray"/>に変換します。
        /// </summary>
        /// <param name="ol">対象</param>
        /// <returns></returns>
        public static KecaknoahArray ToKecaknoahArray(this IList<string> ol) => new KecaknoahArray(ol.Select(p => p.AsKecaknoahString()));

        /// <summary>
        /// このリストを<see cref="KecaknoahList"/>に変換します。
        /// </summary>
        /// <param name="ol">対象</param>
        /// <returns></returns>
        public static KecaknoahList ToKecaknoahList(this IList<KecaknoahObject> ol) => new KecaknoahList(ol);

        /// <summary>
        /// このリストを<see cref="KecaknoahList"/>に変換します。
        /// </summary>
        /// <param name="ol">対象</param>
        /// <returns></returns>
        public static KecaknoahList ToKecaknoahList(this IList<int> ol) => new KecaknoahList(ol.Select(p => p.AsKecaknoahInteger()));

        /// <summary>
        /// このリストを<see cref="KecaknoahList"/>に変換します。
        /// </summary>
        /// <param name="ol">対象</param>
        /// <returns></returns>
        public static KecaknoahList ToKecaknoahList(this IList<long> ol) => new KecaknoahList(ol.Select(p => p.AsKecaknoahInteger()));

        /// <summary>
        /// このリストを<see cref="KecaknoahList"/>に変換します。
        /// </summary>
        /// <param name="ol">対象</param>
        /// <returns></returns>
        public static KecaknoahList ToKecaknoahList(this IList<double> ol) => new KecaknoahList(ol.Select(p => p.AsKecaknoahFloat()));

        /// <summary>
        /// このリストを<see cref="KecaknoahList"/>に変換します。
        /// </summary>
        /// <param name="ol">対象</param>
        /// <returns></returns>
        public static KecaknoahList ToKecaknoahList(this IList<bool> ol) => new KecaknoahList(ol.Select(p => p.AsKecaknoahBoolean()));

        /// <summary>
        /// このリストを<see cref="KecaknoahList"/>に変換します。
        /// </summary>
        /// <param name="ol">対象</param>
        /// <returns></returns>
        public static KecaknoahList ToKecaknoahList(this IList<string> ol) => new KecaknoahList(ol.Select(p => p.AsKecaknoahString()));
        #endregion

        #region IEnumerable<T>拡張系
        /// <summary>
        /// 列挙可能なリストに対してmapメソッド(Select相当)を生成します。
        /// </summary>
        /// <param name="list">対象のリスト</param>
        /// <returns>生成されたDelegate。</returns>
        public static KecaknoahInteropDelegate GenerateMapFunction(this IEnumerable<KecaknoahObject> list)
        {
            KecaknoahInteropDelegate retfunc = (ctx, self, args) =>
            {
                var lambda = args[0];
                var res = list.Select(p => lambda.Call(ctx, new[] { p }).ReturningObject);
                return new KecaknoahArray(res).NoResume();
            };
            return retfunc;
        }

        /// <summary>
        /// 列挙可能なリストに対してfilterメソッド(Where相当)を生成します。
        /// </summary>
        /// <param name="list">対象のリスト</param>
        /// <returns>生成されたDelegate。</returns>
        public static KecaknoahInteropDelegate GenerateFilterFunction(this IEnumerable<KecaknoahObject> list)
        {
            KecaknoahInteropDelegate retfunc = (ctx, self, args) =>
            {
                var lambda = args[0];
                var res = list.Where(p => lambda.Call(ctx, new[] { p }).ReturningObject.ToBoolean());
                return new KecaknoahArray(res).NoResume();
            };
            return retfunc;
        }

        /// <summary>
        /// 列挙可能なリストに対してeachメソッドを生成します。
        /// </summary>
        /// <param name="list">対象のリスト</param>
        /// <returns>生成されたDelegate。</returns>
        public static KecaknoahInteropDelegate GenerateEachFunction(this IEnumerable<KecaknoahObject> list)
        {
            KecaknoahInteropDelegate retfunc = (ctx, self, args) =>
            {
                var lambda = args[0];
                foreach (var i in list)
                    lambda.Call(ctx, new[] { i });
                return KecaknoahNil.Instance.NoResume();
            };
            return retfunc;
        }

        /// <summary>
        /// 列挙可能なリストに対してreduceメソッド(Aggregate相当)を生成します。
        /// </summary>
        /// <param name="list">対象のリスト</param>
        /// <returns>生成されたDelegate。</returns>
        public static KecaknoahInteropDelegate GenerateReduceFunction(this IEnumerable<KecaknoahObject> list)
        {
            KecaknoahInteropDelegate retfunc = (ctx, self, args) =>
            {
                var lambda = args[0];
                var res = list.Aggregate((p, q) => lambda.Call(ctx, new[] { p, q }).ReturningObject);
                return res.NoResume();
            };
            return retfunc;
        }

        /// <summary>
        /// 列挙可能なリストに対してskipメソッド(Skip相当)を生成します。
        /// </summary>
        /// <param name="list">対象のリスト</param>
        /// <returns>生成されたDelegate。</returns>
        public static KecaknoahInteropDelegate GenerateSkipFunction(this IEnumerable<KecaknoahObject> list)
        {
            KecaknoahInteropDelegate retfunc = (ctx, self, args) =>
            {
                var res = list.Skip(args[0].ToInt32());
                return new KecaknoahArray(res).NoResume();
            };
            return retfunc;
        }

        /// <summary>
        /// 列挙可能なリストに対してtakeメソッド(Skip相当)を生成します。
        /// </summary>
        /// <param name="list">対象のリスト</param>
        /// <returns>生成されたDelegate。</returns>
        public static KecaknoahInteropDelegate GenerateTakeFunction(this IEnumerable<KecaknoahObject> list)
        {
            KecaknoahInteropDelegate retfunc = (ctx, self, args) =>
            {
                var res = list.Take(args[0].ToInt32());
                return new KecaknoahArray(res).NoResume();
            };
            return retfunc;
        }

        /// <summary>
        /// 列挙可能なリストに対してreverseメソッド(Reverse相当)を生成します。
        /// </summary>
        /// <param name="list">対象のリスト</param>
        /// <returns>生成されたDelegate。</returns>
        public static KecaknoahInteropDelegate GenerateReverseFunction(this IEnumerable<KecaknoahObject> list)
        {
            KecaknoahInteropDelegate retfunc = (ctx, self, args) =>
            {
                var res = list.Reverse();
                return new KecaknoahArray(res).NoResume();
            };
            return retfunc;
        }

        /// <summary>
        /// 列挙可能なリストに対してfirstメソッド(First相当)を生成します。
        /// </summary>
        /// <param name="list">対象のリスト</param>
        /// <returns>生成されたDelegate。</returns>
        public static KecaknoahInteropDelegate GenerateFirstFunction(this IEnumerable<KecaknoahObject> list)
        {
            KecaknoahInteropDelegate retfunc = (ctx, self, args) =>
            {
                if (args.Length == 0) return list.First().NoResume();
                var lambda = args[0];
                var res = list.First(p => args[0].Call(ctx, new[] { p }).ReturningObject.ToBoolean());
                return res.NoResume();
            };
            return retfunc;
        }

        /// <summary>
        /// 列挙可能なリストに対してlastメソッド(Last相当)を生成します。
        /// </summary>
        /// <param name="list">対象のリスト</param>
        /// <returns>生成されたDelegate。</returns>
        public static KecaknoahInteropDelegate GenerateLastFunction(this IEnumerable<KecaknoahObject> list)
        {
            KecaknoahInteropDelegate retfunc = (ctx, self, args) =>
            {
                if (args.Length == 0) return list.Last().NoResume();
                var lambda = args[0];
                var res = list.Last(p => args[0].Call(ctx, new[] { p }).ReturningObject.ToBoolean());
                return res.NoResume();
            };
            return retfunc;
        }

        /// <summary>
        /// 列挙可能なリストに対してanyメソッド(any相当)を生成します。
        /// </summary>
        /// <param name="list">対象のリスト</param>
        /// <returns>生成されたDelegate。</returns>
        public static KecaknoahInteropDelegate GenerateAnyFunction(this IEnumerable<KecaknoahObject> list)
        {
            KecaknoahInteropDelegate retfunc = (ctx, self, args) =>
            {
                var lambda = args[0];
                var res = list.Any(p => args[0].Call(ctx, new[] { p }).ReturningObject.ToBoolean());
                return res.AsKecaknoahBoolean().NoResume();
            };
            return retfunc;
        }

        /// <summary>
        /// 列挙可能なリストに対してallメソッド(all相当)を生成します。
        /// </summary>
        /// <param name="list">対象のリスト</param>
        /// <returns>生成されたDelegate。</returns>
        public static KecaknoahInteropDelegate GenerateAllFunction(this IEnumerable<KecaknoahObject> list)
        {
            KecaknoahInteropDelegate retfunc = (ctx, self, args) =>
            {
                var lambda = args[0];
                var res = list.All(p => args[0].Call(ctx, new[] { p }).ReturningObject.ToBoolean());
                return res.AsKecaknoahBoolean().NoResume();
            };
            return retfunc;
        }
        #endregion
    }
}
