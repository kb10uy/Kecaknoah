using System;
using System.Linq;

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

        internal static KecaknoahInteropClassInfo Information { get; } = new KecaknoahInteropClassInfo("String");

        static KecaknoahString()
        {
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("join", ClassJoin));
        }

        /// <summary>
        /// 長さへの参照を取得します。
        /// </summary>
        public KecaknoahReference Length { get; } = new KecaknoahReference { IsLeftValue = true, RawObject = 0.AsKecaknoahInteger() };


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

                case nameof(replace): return replace;
                case nameof(substring): return substring;
                case nameof(split): return split;
                case nameof(to_upper): return to_upper;
                case nameof(to_lower): return to_lower;
                case nameof(starts): return starts;
                case nameof(ends): return ends;
                case nameof(pad_left): return pad_left;
                case nameof(pad_right): return pad_right;
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
        /// 文字を参照します。
        /// </summary>
        /// <param name="indices">引数。Integer以外禁止。</param>
        /// <returns></returns>
        protected internal override KecaknoahReference GetIndexerReference(KecaknoahObject[] indices)
        {
            var i = indices[0].ToInt32();
            return KecaknoahReference.Right(raw[i].ToString().AsKecaknoahString());
        }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public KecaknoahString()
        {
            Type = TypeCode.String;
            ExtraType = "String";

            split = KecaknoahReference.Right(this, InstanceSplit);
            replace = KecaknoahReference.Right(this, InstanceReplace);
            substring = KecaknoahReference.Right(this, InstanceSubstring);
            to_upper = KecaknoahReference.Right(this, InstanceToUpper);
            to_lower = KecaknoahReference.Right(this, InstanceToLower);
            starts = KecaknoahReference.Right(this, InstanceStartsWith);
            ends = KecaknoahReference.Right(this, InstanceEndsWith);
            pad_left = KecaknoahReference.Right(this, InstancePadLeft);
            pad_right = KecaknoahReference.Right(this, InstancePadRight);
        }

        private KecaknoahReference to_upper, to_lower, starts, ends, pad_left, pad_right, replace, substring, split;

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

        private KecaknoahFunctionResult InstanceReplace(KecaknoahContext context, KecaknoahObject self, KecaknoahObject[] args) => raw.Replace(args[0].ToString(), args[1].ToString()).AsKecaknoahString().NoResume();

        private KecaknoahFunctionResult InstanceSplit(KecaknoahContext context, KecaknoahObject self, KecaknoahObject[] args)
        {
            var l = args[0];
            var op = StringSplitOptions.None;
            if (args.Length >= 2)
            {
                var flag = args[1].ToBoolean();
                if (flag) op = StringSplitOptions.RemoveEmptyEntries;
            }
            if (l.ExtraType == "Array")
            {
                var list = l.ToStringArray();
                var result = raw.Split(list.ToArray(), op);
                return new KecaknoahArray(result.Select(p => p.AsKecaknoahString())).NoResume();
            }
            else
            {
                var str = l.ToString();
                var result = raw.Split(new[] { str }, op);
                return new KecaknoahArray(result.Select(p => p.AsKecaknoahString())).NoResume();
            }
        }

        private KecaknoahFunctionResult InstanceStartsWith(KecaknoahContext context, KecaknoahObject self, KecaknoahObject[] args) => raw.StartsWith(args[0].ToString()).AsKecaknoahBoolean().NoResume();

        private KecaknoahFunctionResult InstanceEndsWith(KecaknoahContext context, KecaknoahObject self, KecaknoahObject[] args) => raw.EndsWith(args[0].ToString()).AsKecaknoahBoolean().NoResume();

        private KecaknoahFunctionResult InstancePadLeft(KecaknoahContext context, KecaknoahObject self, KecaknoahObject[] args) => raw.PadLeft(args[0].ToInt32()).AsKecaknoahString().NoResume();

        private KecaknoahFunctionResult InstancePadRight(KecaknoahContext context, KecaknoahObject self, KecaknoahObject[] args) => raw.PadRight(args[0].ToInt32()).AsKecaknoahString().NoResume();

        private KecaknoahFunctionResult InstanceToUpper(KecaknoahContext context, KecaknoahObject self, KecaknoahObject[] args) => raw.ToUpper().AsKecaknoahString().NoResume();

        private KecaknoahFunctionResult InstanceToLower(KecaknoahContext context, KecaknoahObject self, KecaknoahObject[] args) => raw.ToLower().AsKecaknoahString().NoResume();

        private static KecaknoahFunctionResult ClassJoin(KecaknoahContext context, KecaknoahObject self, KecaknoahObject[] args)
        {
            var ls = args[1].ToStringArray();
            var s = args[0].ToString();
            var result = string.Join(s, ls);
            return result.AsKecaknoahString().NoResume();
        }

#pragma warning disable 1591
        public override object Clone() => Value.AsKecaknoahString();
        public override string ToString() => Value;
        public override KecaknoahObject AsByValValue() => Value.AsKecaknoahString();
        public override bool Equals(object obj)
        {
            var t = obj as KecaknoahString;
            return t != null && t.Value == Value;
        }
        public override int GetHashCode() => Value.GetHashCode();
#pragma warning restore 1591
    }
}
