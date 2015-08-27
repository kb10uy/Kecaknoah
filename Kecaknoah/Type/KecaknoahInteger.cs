using System;

namespace Kecaknoah.Type
{
    /// <summary>
    /// Kecaknoahでの整数を定義します。
    /// </summary>
    public sealed class KecaknoahInteger : KecaknoahObject
    {
        /// <summary>
        /// 実際の値を取得・設定します。
        /// </summary>
        public new long Value { get; set; }

        /// <summary>
        /// 最大値のインスタンスを取得します。
        /// </summary>
        public static KecaknoahReference MaxValue = KecaknoahReference.CreateRightReference(long.MaxValue);

        /// <summary>
        /// 最小値のインスタンスを取得します。
        /// </summary>
        public static KecaknoahReference MinValue = KecaknoahReference.CreateRightReference(long.MaxValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected internal override KecaknoahReference GetMemberReference(string name)
        {
            switch (name)
            {
                case "MAX_VALUE":
                    return MaxValue;
                case "MIN_VALUE":
                    return MinValue;
            }
            return base.GetMemberReference(name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        protected internal override KecaknoahObject ExpressionOperation(KecaknoahILCodeType op, KecaknoahObject target)
        {
            if (target.Type == TypeCode.Int64)
            {
                return ExpressionOperation(op, (KecaknoahInteger)target);
            }
            else if (target.Type == TypeCode.Double)
            {
                return ExpressionOperation(op, (KecaknoahFloat)target);
            }
            else
            {
                switch (op)
                {
                    case KecaknoahILCodeType.Equal:
                        return KecaknoahBoolean.False;
                    case KecaknoahILCodeType.NotEqual:
                        return KecaknoahBoolean.True;
                    default:
                        return KecaknoahNil.Instance;
                }
            }
        }

        private KecaknoahObject ExpressionOperation(KecaknoahILCodeType op, KecaknoahInteger target)
        {
            switch (op)
            {
                case KecaknoahILCodeType.Negative:
                    return (-Value).AsKecaknoahInteger();
                case KecaknoahILCodeType.Plus:
                    return (Value + target.Value).AsKecaknoahInteger();
                case KecaknoahILCodeType.Minus:
                    return (Value - target.Value).AsKecaknoahInteger();
                case KecaknoahILCodeType.Multiply:
                    return (Value * target.Value).AsKecaknoahInteger();
                case KecaknoahILCodeType.Divide:
                    return (Value / target.Value).AsKecaknoahInteger();
                case KecaknoahILCodeType.And:
                    return (Value & target.Value).AsKecaknoahInteger();
                case KecaknoahILCodeType.Or:
                    return (Value | target.Value).AsKecaknoahInteger();
                case KecaknoahILCodeType.Xor:
                    return (Value ^ target.Value).AsKecaknoahInteger();
                case KecaknoahILCodeType.Modular:
                    return (Value % target.Value).AsKecaknoahInteger();
                case KecaknoahILCodeType.LeftBitShift:
                    return (Value << (int)target.Value).AsKecaknoahInteger();
                case KecaknoahILCodeType.RightBitShift:
                    return (Value >> (int)target.Value).AsKecaknoahInteger();
                case KecaknoahILCodeType.Equal:
                    return (Value == target.Value).AsKecaknoahBoolean();
                case KecaknoahILCodeType.NotEqual:
                    return (Value != target.Value).AsKecaknoahBoolean();
                case KecaknoahILCodeType.Greater:
                    return (Value > target.Value).AsKecaknoahBoolean();
                case KecaknoahILCodeType.Lesser:
                    return (Value < target.Value).AsKecaknoahBoolean();
                case KecaknoahILCodeType.GreaterEqual:
                    return (Value >= target.Value).AsKecaknoahBoolean();
                case KecaknoahILCodeType.LesserEqual:
                    return (Value <= target.Value).AsKecaknoahBoolean();
                default:
                    return KecaknoahNil.Instance;
            }
        }

        private KecaknoahObject ExpressionOperation(KecaknoahILCodeType op, KecaknoahFloat target)
        {
            switch (op)
            {
                case KecaknoahILCodeType.Negative:
                    return (-Value).AsKecaknoahInteger();
                case KecaknoahILCodeType.Plus:
                    return (Value + target.Value).AsKecaknoahFloat();
                case KecaknoahILCodeType.Minus:
                    return (Value - target.Value).AsKecaknoahFloat();
                case KecaknoahILCodeType.Multiply:
                    return (Value * target.Value).AsKecaknoahFloat();
                case KecaknoahILCodeType.Divide:
                    return (Value / target.Value).AsKecaknoahFloat();
                case KecaknoahILCodeType.Modular:
                    return (Value % target.Value).AsKecaknoahFloat();
                case KecaknoahILCodeType.LeftBitShift:
                    return (Value << (int)target.Value).AsKecaknoahInteger();
                case KecaknoahILCodeType.RightBitShift:
                    return (Value >> (int)target.Value).AsKecaknoahInteger();
                case KecaknoahILCodeType.Equal:
                    return (Value == target.Value).AsKecaknoahBoolean();
                case KecaknoahILCodeType.NotEqual:
                    return (Value != target.Value).AsKecaknoahBoolean();
                case KecaknoahILCodeType.Greater:
                    return (Value > target.Value).AsKecaknoahBoolean();
                case KecaknoahILCodeType.Lesser:
                    return (Value < target.Value).AsKecaknoahBoolean();
                case KecaknoahILCodeType.GreaterEqual:
                    return (Value >= target.Value).AsKecaknoahBoolean();
                case KecaknoahILCodeType.LesserEqual:
                    return (Value <= target.Value).AsKecaknoahBoolean();
                default:
                    return KecaknoahNil.Instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>


        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public KecaknoahInteger()
        {
            Type = TypeCode.Int64;
            ExtraType = "Integer";
        }

#pragma warning disable 1591
        public override object Clone() => Value.AsKecaknoahInteger();
        public override KecaknoahObject AsByValValue() => Value.AsKecaknoahInteger();
        public override bool Equals(object obj)
        {
            var t = obj as KecaknoahInteger;
            return t != null && t.Value == Value;
        }
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString();
        public override long ToInt64() => Value;
        public override double ToDouble() => Value;
#pragma warning restore 1591
    }
}
