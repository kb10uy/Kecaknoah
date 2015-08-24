using System;

namespace Kecaknoah.Type
{
    /// <summary>
    /// Kecaknoahでの倍精度浮動小数点数を定義します。
    /// </summary>
    public sealed class KecaknoahFloat : KecaknoahObject
    {

        /// <summary>
        /// 実際の値を取得・設定します。
        /// </summary>
        public new double Value { get; set; }

        /// <summary>
        /// 最大値のインスタンスを取得します。
        /// </summary>
        public static KecaknoahReference MaxValue = KecaknoahReference.CreateRightReference(double.MaxValue);

        /// <summary>
        /// 最小値のインスタンスを取得します。
        /// </summary>
        public static KecaknoahReference MinValue = KecaknoahReference.CreateRightReference(double.MinValue);
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
                    return (-Value).AsKecaknoahFloat();
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
                    return (-Value).AsKecaknoahFloat();
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
        /// 現在の以下略。
        /// </summary>
        /// <returns>知るか</returns>
        public override string ToString() => Value.ToString();

        /// <summary>
        /// 可能ならば<see cref="long"/>型に変換します。
        /// </summary>
        /// <returns></returns>
        public override long ToInt64() => (long)Value;

        /// <summary>
        /// 可能ならば<see cref="double"/>型に変換します。
        /// </summary>
        /// <returns></returns>
        public override double ToDouble() => Value;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public KecaknoahFloat()
        {
            Type = TypeCode.Double;
        }

#pragma warning disable 1591
        public override object Clone() => Value.AsKecaknoahFloat();
#pragma warning restore 1591
    }
}
