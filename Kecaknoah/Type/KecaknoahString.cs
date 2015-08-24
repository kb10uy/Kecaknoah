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
            if (target.Type == TypeCode.String)
            {
                var t = (KecaknoahString)target;
                switch (op)
                {
                    case KecaknoahILCodeType.Plus:
                        return (Value + t.Value).AsKecaknoahString();
                    case KecaknoahILCodeType.Equal:
                        return (Value == t.Value).AsKecaknoahBoolean();
                    case KecaknoahILCodeType.NotEqual:
                        return (Value != t.Value).AsKecaknoahBoolean();
                    case KecaknoahILCodeType.Greater:
                        return (Value.CompareTo(t.Value) > 0).AsKecaknoahBoolean();
                    case KecaknoahILCodeType.Lesser:
                        return (Value.CompareTo(t.Value) < 0).AsKecaknoahBoolean();
                    case KecaknoahILCodeType.GreaterEqual:
                        return (Value.CompareTo(t.Value) >= 0).AsKecaknoahBoolean();
                    case KecaknoahILCodeType.LesserEqual:
                        return (Value.CompareTo(t.Value) <= 0).AsKecaknoahBoolean();
                    default:
                        return KecaknoahNil.Instance;
                }
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
        public override object Clone() => Value.AsKecaknoahString();
#pragma warning restore 1591
    }
}
