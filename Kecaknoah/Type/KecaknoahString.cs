using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah.Type
{
    /// <summary>
    /// Kecaknoahでの文字列を定義します。
    /// </summary>
    public sealed class KecaknoahString : KecaknoahObject
    {
        /// <summary>
        /// 実際の値を取得・設定します。
        /// </summary>
        public string Value { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override KecaknoahObject GetMember(string name)
        {
            switch (name)
            {
                case "length":
                    return Value.Length.AsKecaknoahInteger();
                case "replace":
                    return new Func<string, string, string>(Value.Replace).AsKecaknoahDelegate().AsKecaknoahInteropFunction();
                case "substring":
                    return new Func<int, string>(Value.Substring).AsKecaknoahDelegate().AsKecaknoahInteropFunction();
            }
            return base.GetMember(name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public override KecaknoahObject ExpressionOperation(KecaknoahILCodeType op, KecaknoahObject target)
        {
            var t = (string)target.AsRawObject<string>();
            if (t == null) return KecaknoahNil.Instance;
            switch (op)
            {
                case KecaknoahILCodeType.Plus:
                    return (Value + t).AsKecaknoahString();
                case KecaknoahILCodeType.Equal:
                    return (Value == t).AsKecaknoahBoolean();
                case KecaknoahILCodeType.NotEqual:
                    return (Value != t).AsKecaknoahBoolean();
                case KecaknoahILCodeType.Greater:
                    return (Value.CompareTo(t) > 0).AsKecaknoahBoolean();
                case KecaknoahILCodeType.Lesser:
                    return (Value.CompareTo(t) < 0).AsKecaknoahBoolean();
                case KecaknoahILCodeType.GreaterEqual:
                    return (Value.CompareTo(t) >= 0).AsKecaknoahBoolean();
                case KecaknoahILCodeType.LesserEqual:
                    return (Value.CompareTo(t) <= 0).AsKecaknoahBoolean();
                default:
                    return KecaknoahNil.Instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override object AsRawObject<T>()
        {
            var to = typeof(T);
            switch (to.Name)
            {
                case nameof(String):
                    return Value;
            }

            return null;
        }

        /// <summary>
        /// 現在の以下略。
        /// </summary>
        /// <returns>知るか</returns>
        public override string ToString() => Value;
    }
}
