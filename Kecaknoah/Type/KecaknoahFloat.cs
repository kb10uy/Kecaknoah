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
            switch (op)
            {
                case KecaknoahILCodeType.Negative:
                    return (-Value).AsKecaknoahFloat();
                case KecaknoahILCodeType.Plus:
                    return (dynamic)this + (dynamic)target;
                case KecaknoahILCodeType.Minus:
                    return (dynamic)this - (dynamic)target;
                case KecaknoahILCodeType.Multiply:
                    return (dynamic)this * (dynamic)target;
                case KecaknoahILCodeType.Divide:
                    return (dynamic)this / (dynamic)target;
                case KecaknoahILCodeType.Modular:
                    return (dynamic)this % (dynamic)target;
                case KecaknoahILCodeType.Equal:
                    return (dynamic)this == (dynamic)target;
                case KecaknoahILCodeType.NotEqual:
                    return (dynamic)this != (dynamic)target;
                case KecaknoahILCodeType.Greater:
                    return (dynamic)this > (dynamic)target;
                case KecaknoahILCodeType.Lesser:
                    return (dynamic)this < (dynamic)target;
                case KecaknoahILCodeType.GreaterEqual:
                    return (dynamic)this >= (dynamic)target;
                case KecaknoahILCodeType.LesserEqual:
                    return (dynamic)this <= (dynamic)target;
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
        public override int GetHashCode() => Value.GetHashCode();
        public override bool Equals(object obj) => ReferenceEquals(this, obj);
        public override object Clone() => Value.AsKecaknoahFloat();

        public static KecaknoahObject operator +(KecaknoahFloat v1, KecaknoahFloat v2) => (v1.Value + v2.Value).AsKecaknoahFloat();
        public static KecaknoahObject operator -(KecaknoahFloat v1, KecaknoahFloat v2) => (v1.Value - v2.Value).AsKecaknoahFloat();
        public static KecaknoahObject operator *(KecaknoahFloat v1, KecaknoahFloat v2) => (v1.Value * v2.Value).AsKecaknoahFloat();
        public static KecaknoahObject operator /(KecaknoahFloat v1, KecaknoahFloat v2) => (v1.Value / v2.Value).AsKecaknoahFloat();
        public static KecaknoahObject operator %(KecaknoahFloat v1, KecaknoahFloat v2) => (v1.Value % v2.Value).AsKecaknoahFloat();
        public static KecaknoahObject operator +(KecaknoahFloat v1, KecaknoahInteger v2) => (v1.Value + v2.Value).AsKecaknoahFloat();
        public static KecaknoahObject operator -(KecaknoahFloat v1, KecaknoahInteger v2) => (v1.Value - v2.Value).AsKecaknoahFloat();
        public static KecaknoahObject operator *(KecaknoahFloat v1, KecaknoahInteger v2) => (v1.Value * v2.Value).AsKecaknoahFloat();
        public static KecaknoahObject operator /(KecaknoahFloat v1, KecaknoahInteger v2) => (v1.Value / v2.Value).AsKecaknoahFloat();
        public static KecaknoahObject operator %(KecaknoahInteger v1, KecaknoahFloat v2) => (v1.Value % v2.Value).AsKecaknoahFloat();

        public static KecaknoahObject operator ==(KecaknoahFloat v1, KecaknoahFloat v2) => (v1.Value == v2.Value).AsKecaknoahBoolean();
        public static KecaknoahObject operator !=(KecaknoahFloat v1, KecaknoahFloat v2) => (v1.Value != v2.Value).AsKecaknoahBoolean();
        public static KecaknoahObject operator <(KecaknoahFloat v1, KecaknoahFloat v2) => (v1.Value < v2.Value).AsKecaknoahBoolean();
        public static KecaknoahObject operator >(KecaknoahFloat v1, KecaknoahFloat v2) => (v1.Value > v2.Value).AsKecaknoahBoolean();
        public static KecaknoahObject operator <=(KecaknoahFloat v1, KecaknoahFloat v2) => (v1.Value <= v2.Value).AsKecaknoahBoolean();
        public static KecaknoahObject operator >=(KecaknoahFloat v1, KecaknoahFloat v2) => (v1.Value >= v2.Value).AsKecaknoahBoolean();

        public static KecaknoahObject operator ==(KecaknoahFloat v1, KecaknoahInteger v2) => (v1.Value == v2.Value).AsKecaknoahBoolean();
        public static KecaknoahObject operator !=(KecaknoahFloat v1, KecaknoahInteger v2) => (v1.Value != v2.Value).AsKecaknoahBoolean();
        public static KecaknoahObject operator <(KecaknoahFloat v1, KecaknoahInteger v2) => (v1.Value < v2.Value).AsKecaknoahBoolean();
        public static KecaknoahObject operator >(KecaknoahFloat v1, KecaknoahInteger v2) => (v1.Value > v2.Value).AsKecaknoahBoolean();
        public static KecaknoahObject operator <=(KecaknoahFloat v1, KecaknoahInteger v2) => (v1.Value <= v2.Value).AsKecaknoahBoolean();
        public static KecaknoahObject operator >=(KecaknoahFloat v1, KecaknoahInteger v2) => (v1.Value >= v2.Value).AsKecaknoahBoolean();

        public static explicit operator double (KecaknoahFloat v1) => v1.Value;
        public static explicit operator KecaknoahInteger(KecaknoahFloat v1) => ((long)v1.Value).AsKecaknoahInteger();
        public static explicit operator KecaknoahFloat(KecaknoahInteger v1) => ((double)v1.Value).AsKecaknoahFloat();
#pragma warning restore 1591
    }
}
