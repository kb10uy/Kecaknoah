using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah.Type
{
    /// <summary>
    /// Kecaknoahでの真偽値を定義します。
    /// </summary>
    public sealed class KecaknoahBoolean : KecaknoahObject
    {
        /// <summary>
        /// 実際の値を取得・設定します。
        /// </summary>
        public bool Value { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public override KecaknoahObject ExpressionOperation(KecaknoahILCodeType op, KecaknoahObject target)
        {
            var ot = target.AsRawObject<bool>();
            if (ot == null) return KecaknoahNil.Instance;
            var t = (bool)ot;
            switch (op)
            {
                case KecaknoahILCodeType.AndAlso:
                    return (Value && t).AsKecaknoahBoolean();
                case KecaknoahILCodeType.OrElse:
                    return (Value || t).AsKecaknoahBoolean();
                case KecaknoahILCodeType.And:
                    return (Value & t).AsKecaknoahBoolean();
                case KecaknoahILCodeType.Or:
                    return (Value | t).AsKecaknoahBoolean();
                case KecaknoahILCodeType.Xor:
                    return (Value ^ t).AsKecaknoahBoolean();
                case KecaknoahILCodeType.Equal:
                    return (Value == t).AsKecaknoahBoolean();
                case KecaknoahILCodeType.NotEqual:
                    return (Value != t).AsKecaknoahBoolean();
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
                    return Value.ToString();
                case nameof(Boolean):
                    return Value;
            }
            return null;
        }

        /// <summary>
        /// 現在の以下略。
        /// </summary>
        /// <returns>知るか</returns>
        public override string ToString() => Value.ToString();
    }
}
