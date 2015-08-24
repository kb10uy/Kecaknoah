using System;

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
        protected internal override KecaknoahObject ExpressionOperation(KecaknoahILCodeType op, KecaknoahObject target)
        {
            if (target.Type == TypeCode.Boolean)
            {
                var t = (KecaknoahBoolean)target;
                switch (op)
                {
                    case KecaknoahILCodeType.Not:
                        return (!Value).AsKecaknoahBoolean();
                    case KecaknoahILCodeType.AndAlso:
                        return (Value && t.Value).AsKecaknoahBoolean();
                    case KecaknoahILCodeType.OrElse:
                        return (Value || t.Value).AsKecaknoahBoolean();
                    case KecaknoahILCodeType.And:
                        return (Value & t.Value).AsKecaknoahBoolean();
                    case KecaknoahILCodeType.Or:
                        return (Value | t.Value).AsKecaknoahBoolean();
                    case KecaknoahILCodeType.Xor:
                        return (Value ^ t.Value).AsKecaknoahBoolean();
                    case KecaknoahILCodeType.Equal:
                        return (Value == t.Value).AsKecaknoahBoolean();
                    case KecaknoahILCodeType.NotEqual:
                        return (Value != t.Value).AsKecaknoahBoolean();
                    default:
                        return KecaknoahNil.Instance;
                }
            }
            else
            {
                switch (op)
                {
                    case KecaknoahILCodeType.Equal:
                        return False;
                    case KecaknoahILCodeType.NotEqual:
                        return True;
                    default:
                        return KecaknoahNil.Instance;
                }
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

        /// <summary>
        /// true。
        /// </summary>
        public static KecaknoahBoolean True { get; } = true.AsKecaknoahBoolean();

        /// <summary>
        /// false。
        /// </summary>
        public static KecaknoahBoolean False { get; } = false.AsKecaknoahBoolean();

#pragma warning disable 1591
        public override object Clone() => Value.AsKecaknoahBoolean();
#pragma warning restore 1591

    }
}
