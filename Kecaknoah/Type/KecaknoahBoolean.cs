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
        /// 実際の値を取得します。
        /// </summary>
        public new bool Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public override KecaknoahObject ExpressionOperation(KecaknoahILCodeType op, KecaknoahObject target)
        {
            switch (op)
            {
                case KecaknoahILCodeType.Not:
                    return (!Value).AsKecaknoahBoolean();
                case KecaknoahILCodeType.AndAlso:
                    return (dynamic)this && (dynamic)target;
                case KecaknoahILCodeType.OrElse:
                    return (dynamic)this || (dynamic)target;
                case KecaknoahILCodeType.And:
                    return (dynamic)this & (dynamic)target;
                case KecaknoahILCodeType.Or:
                    return (dynamic)this | (dynamic)target;
                case KecaknoahILCodeType.Xor:
                    return (dynamic)this ^ (dynamic)target;
                case KecaknoahILCodeType.Equal:
                    return (dynamic)this == (dynamic)target;
                case KecaknoahILCodeType.NotEqual:
                    return (dynamic)this != (dynamic)target;
                default:
                    return KecaknoahNil.Instance;
            }
        }

        /// <summary>
        /// 現在の以下略。
        /// </summary>
        /// <returns>知るか</returns>
        public override string ToString() => Value.ToString();

        /// <summary>
        /// 可能ならば<see cref="bool"/>型に変換します。
        /// </summary>
        /// <returns></returns>
        public override bool ToBoolean() => Value;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// 
        /// </summary>
        public KecaknoahBoolean()
        {
            Type = TypeCode.Boolean;
        }


#pragma warning disable 1591
        public override int GetHashCode() => Value.GetHashCode();
        public override bool Equals(object obj) => ReferenceEquals(this, obj);
        public override object Clone() => Value.AsKecaknoahBoolean();

        public static KecaknoahObject operator &(KecaknoahBoolean v1, KecaknoahBoolean v2) => (v1.Value & v2.Value).AsKecaknoahBoolean();
        public static KecaknoahObject operator ^(KecaknoahBoolean v1, KecaknoahBoolean v2) => (v1.Value ^ v2.Value).AsKecaknoahBoolean();
        public static KecaknoahObject operator |(KecaknoahBoolean v1, KecaknoahBoolean v2) => (v1.Value | v2.Value).AsKecaknoahBoolean();
        public static KecaknoahObject operator ==(KecaknoahBoolean v1, KecaknoahBoolean v2) => (v1.Value == v2.Value).AsKecaknoahBoolean();
        public static KecaknoahObject operator !=(KecaknoahBoolean v1, KecaknoahBoolean v2) => (v1.Value != v2.Value).AsKecaknoahBoolean();
        public static bool operator true(KecaknoahBoolean v1) => v1.Value;
        public static bool operator false(KecaknoahBoolean v1) => !v1.Value;

        public static explicit operator bool (KecaknoahBoolean v1) => v1.Value;
#pragma warning restore 1591

    }
}
