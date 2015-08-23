using System;

namespace Kecaknoah.Type
{
    /// <summary>
    /// Kecaknoahでの文字列を定義します。
    /// </summary>
    public sealed class KecaknoahString : KecaknoahObject
    {
        private string raw = "";
        /// <summary>
        /// 実際の値を取得します。
        /// </summary>
        public new string Value
        {
            get { return raw; }
            set
            {
                raw = value;
                Length.RawObject = raw.Length.AsKecaknoahInteger();
            }
        }

        /// <summary>
        /// 長さへの参照を取得します。
        /// </summary>
        public KecaknoahReference Length { get; } = new KecaknoahReference { IsLeftValue = true, RawObject = 0.AsKecaknoahInteger() };
        /// <summary>
        /// replaceメソッドの参照を取得します。
        /// </summary>
        public KecaknoahReference Replace { get; }
        /// <summary>
        /// Substringメソッドの参照を取得します。
        /// </summary>
        public KecaknoahReference Substring { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected internal override KecaknoahReference GetMemberReference(string name)
        {
            switch (name)
            {
                case "length":
                    return Length;
                case "replace":
                    return Replace;
                case "substring":
                    return Substring;
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
                case KecaknoahILCodeType.Plus:
                    return (dynamic)this + (dynamic)target;
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
        public override string ToString() => Value;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public KecaknoahString()
        {
            Type = TypeCode.String;
            Replace = KecaknoahReference.CreateRightReference(this, InstanceReplace);
            Substring = KecaknoahReference.CreateRightReference(this, InstanceSubstring);
        }

        /// <summary>
        /// 性的なインスタンスを生成します。
        /// </summary>
        /// <param name="st"></param>
        public KecaknoahString(bool st)
        {
            Type = TypeCode.String;
        }

        private KecaknoahFunctionResult InstanceSubstring(KecaknoahContext context, KecaknoahObject self, KecaknoahObject[] args)
        {
            var t = self.ToString();
            switch (args.Length)
            {
                case 0:
                    return "".AsKecaknoahString().NoResume();
                case 1:
                    return t.Substring((int)args[0].ToInt64()).AsKecaknoahString().NoResume();
                default:
                    return t.Substring((int)args[0].ToInt64(), (int)args[1].ToInt64()).AsKecaknoahString().NoResume();
            }
        }

        private KecaknoahFunctionResult InstanceReplace(KecaknoahContext context, KecaknoahObject self, KecaknoahObject[] args) => self.ToString().Replace(args[0].ToString(), args[1].ToString()).AsKecaknoahString().NoResume();

#pragma warning disable 1591
        public override int GetHashCode() => Value.GetHashCode();
        public override bool Equals(object obj) => ReferenceEquals(this, obj);
        public override object Clone() => Value.AsKecaknoahString();

        public static KecaknoahObject operator +(KecaknoahString v1, KecaknoahString v2) => (v1.Value + v2.Value).AsKecaknoahString();

        public static KecaknoahObject operator ==(KecaknoahString v1, KecaknoahString v2) => (v1.Value == v2.Value).AsKecaknoahBoolean();
        public static KecaknoahObject operator !=(KecaknoahString v1, KecaknoahString v2) => (v1.Value != v2.Value).AsKecaknoahBoolean();
        public static KecaknoahObject operator <(KecaknoahString v1, KecaknoahString v2) => (v1.Value.CompareTo(v2.Value) < 0).AsKecaknoahBoolean();
        public static KecaknoahObject operator >(KecaknoahString v1, KecaknoahString v2) => (v1.Value.CompareTo(v2.Value) > 0).AsKecaknoahBoolean();
        public static KecaknoahObject operator <=(KecaknoahString v1, KecaknoahString v2) => (v1.Value.CompareTo(v2.Value) <= 0).AsKecaknoahBoolean();
        public static KecaknoahObject operator >=(KecaknoahString v1, KecaknoahString v2) => (v1.Value.CompareTo(v2.Value) >= 0).AsKecaknoahBoolean();

        public static explicit operator string (KecaknoahString v1) => v1.Value;
#pragma warning restore 1591
    }
}
