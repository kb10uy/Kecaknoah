using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Kecaknoah.Type;

namespace Kecaknoah.Standard
{
#pragma warning disable 1591
    /// <summary>
    /// KecaknoahでTimeSpanを表現します。
    /// </summary>
    public sealed class KecaknoahTimeSpan : KecaknoahObject
    {
        public static readonly string ClassName = "TimeSpan";
        internal static KecaknoahInteropClassInfo Information { get; } = new KecaknoahInteropClassInfo(ClassName);
        internal TimeSpan timespan;

        #region overrideメンバー
        static KecaknoahTimeSpan()
        {
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("from_days", ClassFromDays));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("from_hours", ClassFromHours));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("from_milliseconds", ClassFromMilliseconds));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("from_minutes", ClassFromMinutes));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("from_seconds", ClassFromSeconds));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("from_ticks", ClassFromTicks));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("parse", ClassParse));
        }

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="span">基にする<see cref="TimeSpan"/></param>
        public KecaknoahTimeSpan(TimeSpan span)
        {
            ExtraType = ClassName;
            timespan = span;
            f_days = KecaknoahReference.Right(span.Days);
            f_hours = KecaknoahReference.Right(span.Hours);
            f_milliseconds = KecaknoahReference.Right(span.Milliseconds);
            f_minutes = KecaknoahReference.Right(span.Minutes);
            f_seconds = KecaknoahReference.Right(span.Seconds);
            f_ticks = KecaknoahReference.Right(span.Ticks);
            f_total_days = KecaknoahReference.Right(span.TotalDays);
            f_total_hours = KecaknoahReference.Right(span.TotalHours);
            f_total_milliseconds = KecaknoahReference.Right(span.TotalMilliseconds);
            f_total_minutes = KecaknoahReference.Right(span.TotalMinutes);
            f_total_seconds = KecaknoahReference.Right(span.TotalSeconds);
            RegisterInstanceMembers();
        }

        protected internal override KecaknoahReference GetMemberReference(string name)
        {
            switch (name)
            {
                case "add": return i_add;
                case "sub": return i_sub;

                case "days": return f_days;
                case "hours": return f_hours;
                case "milliseconds": return f_milliseconds;
                case "minutes": return f_minutes;
                case "seconds": return f_seconds;
                case "ticks": return f_ticks;
                case "total_days": return f_total_days;
                case "total_hours": return f_total_hours;
                case "total_milliseconds": return f_total_milliseconds;
                case "total_minutes": return f_total_minutes;
                case "total_seconds": return f_total_seconds;
                default: return base.GetMemberReference(name);
            }
        }

        protected internal override KecaknoahObject ExpressionOperation(KecaknoahILCodeType op, KecaknoahObject target)
        {
            if (target.ExtraType == "TimeSpan")
            {
                var t = ((KecaknoahTimeSpan)target).timespan;
                switch (op)
                {
                    case KecaknoahILCodeType.Plus:
                        return new KecaknoahTimeSpan(timespan + t);
                    case KecaknoahILCodeType.Minus:
                        return new KecaknoahTimeSpan(timespan - t);
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

        public override bool Equals(object obj)
        {
            if (((KecaknoahObject)obj).ExtraType != "TimeSpan") return false;
            return ((KecaknoahTimeSpan)obj).timespan == timespan;
        }

        public override int GetHashCode() => timespan.GetHashCode();

        public override KecaknoahObject AsByValValue() => new KecaknoahTimeSpan(timespan);

        public override string ToString() => timespan.ToString();

        #endregion

        #region インスタンスメンバー
        private KecaknoahReference f_days, f_hours, f_milliseconds, f_minutes, f_seconds, f_ticks, f_total_days, f_total_hours, f_total_milliseconds, f_total_minutes, f_total_seconds;
        private KecaknoahReference i_add, i_sub;

        private void RegisterInstanceMembers()
        {
            i_add = KecaknoahReference.Right(this, InstanceAdd);
            i_sub = KecaknoahReference.Right(this, InstanceSub);
        }

        private KecaknoahFunctionResult InstanceAdd(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = new KecaknoahTimeSpan(timespan.Add(((KecaknoahTimeSpan)args[0]).timespan));
            return result.NoResume();
        }

        private KecaknoahFunctionResult InstanceSub(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = new KecaknoahTimeSpan(timespan.Subtract(((KecaknoahTimeSpan)args[0]).timespan));
            return result.NoResume();
        }

        #endregion

        #region クラスメソッド

        private static KecaknoahFunctionResult ClassFromDays(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = new KecaknoahTimeSpan(TimeSpan.FromDays(args[0].ToInt32()));
            return result.NoResume();
        }

        private static KecaknoahFunctionResult ClassFromHours(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = new KecaknoahTimeSpan(TimeSpan.FromHours(args[0].ToInt32()));
            return result.NoResume();
        }

        private static KecaknoahFunctionResult ClassFromMilliseconds(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = new KecaknoahTimeSpan(TimeSpan.FromMilliseconds(args[0].ToInt32()));
            return result.NoResume();
        }

        private static KecaknoahFunctionResult ClassFromMinutes(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = new KecaknoahTimeSpan(TimeSpan.FromMinutes(args[0].ToInt32()));
            return result.NoResume();
        }

        private static KecaknoahFunctionResult ClassFromSeconds(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = new KecaknoahTimeSpan(TimeSpan.FromSeconds(args[0].ToInt32()));
            return result.NoResume();
        }

        private static KecaknoahFunctionResult ClassFromTicks(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = new KecaknoahTimeSpan(TimeSpan.FromTicks(args[0].ToInt32()));
            return result.NoResume();
        }

        private static KecaknoahFunctionResult ClassParse(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = new KecaknoahTimeSpan(TimeSpan.Parse(args[0].ToString()));
            return result.NoResume();
        }

        #endregion
    }
#pragma warning restore 1591
}
