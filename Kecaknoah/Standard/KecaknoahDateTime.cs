using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Kecaknoah.Type;

namespace Kecaknoah.Standard
{
#pragma warning disable 1591
    /// <summary>
    /// Kecaknoahで日付を定義します。
    /// </summary>
    public sealed class KecaknoahDateTime : KecaknoahObject
    {
        public static readonly string ClassName = "DateTime";
        internal static KecaknoahInteropClassInfo Information { get; } = new KecaknoahInteropClassInfo(ClassName);
        private DateTime datetime;
        #region overrideメンバー
        static KecaknoahDateTime()
        {
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("now", ClassNow));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("days_in_month", ClassDaysInMonth));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("is_leap", ClassIsLeap));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("parse", ClassParse));
        }


        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="dt"></param>
        public KecaknoahDateTime(DateTime dt)
        {
            datetime = dt;
            ExtraType = ClassName;
            f_date = KecaknoahReference.Right(new KecaknoahObject());
            f_day = KecaknoahReference.Right(dt.Day);
            f_day_of_week = KecaknoahReference.Right((int)dt.DayOfWeek);
            f_day_of_year = KecaknoahReference.Right(dt.DayOfYear);
            f_hour = KecaknoahReference.Right(dt.Hour);
            f_kind = KecaknoahReference.Right(dt.Kind.ToString());
            f_millisecond = KecaknoahReference.Right(dt.Millisecond);
            f_minute = KecaknoahReference.Right(dt.Minute);
            f_month = KecaknoahReference.Right(dt.Month);
            f_second = KecaknoahReference.Right(dt.Second);
            f_ticks = KecaknoahReference.Right(dt.Ticks);
            f_time = KecaknoahReference.Right(new KecaknoahObject());
            f_today = KecaknoahReference.Right(new KecaknoahObject());
            f_year = KecaknoahReference.Right(dt.Year);
            RegisterInstanceMembers();
        }

        protected internal override KecaknoahReference GetMemberReference(string name)
        {
            switch (name)
            {
                case "add": return i_add;
                case "add_days": return i_add_days;
                case "add_hours": return i_add_hours;
                case "add_minutes": return i_add_minutes;
                case "add_months": return i_add_months;
                case "add_seconds": return i_add_seconds;
                case "add_ticks": return i_add_ticks;
                case "add_years": return i_add_years;
                case "sub": return i_sub;
                case "to_local": return i_to_local;

                case "date": return f_date;
                case "day": return f_day;
                case "day_of_week": return f_day_of_week;
                case "day_of_year": return f_day_of_year;
                case "hour": return f_hour;
                case "kind": return f_kind;
                case "millisecond": return f_millisecond;
                case "minute": return f_minute;
                case "month": return f_month;
                case "second": return f_second;
                case "ticks": return f_ticks;
                case "time": return f_time;
                case "today": return f_today;
                case "year": return f_year;
                default: return base.GetMemberReference(name);
            }
        }

        public override KecaknoahObject AsByValValue() => new KecaknoahDateTime(datetime);

        public override bool Equals(object obj)
        {
            if (((KecaknoahObject)obj).ExtraType != "DateTime") return false;
            return ((KecaknoahDateTime)obj).datetime == datetime;
        }

        public override int GetHashCode() => datetime.GetHashCode();

        public override string ToString() => datetime.ToString();

        protected internal override KecaknoahObject ExpressionOperation(KecaknoahILCodeType op, KecaknoahObject target)
        {
            if (target.ExtraType == "DateTime")
            {
                var t = ((KecaknoahDateTime)target).datetime;
                switch (op)
                {
                    case KecaknoahILCodeType.Minus:
                        return new KecaknoahTimeSpan(datetime - t);
                    default:
                        return KecaknoahNil.Instance;
                }
            }
            else if (target.ExtraType == "TimeSpan")
            {
                var t = ((KecaknoahTimeSpan)target).timespan;
                switch (op)
                {
                    case KecaknoahILCodeType.Plus:
                        return new KecaknoahDateTime(datetime + t);
                    case KecaknoahILCodeType.Minus:
                        return new KecaknoahDateTime(datetime - t);
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
        #endregion

        #region インスタンスメンバー
        private KecaknoahReference f_date, f_day, f_day_of_week, f_day_of_year, f_hour, f_kind, f_millisecond, f_minute, f_month, f_second, f_ticks, f_time, f_today, f_year;
        private KecaknoahReference i_add, i_add_days, i_add_hours, i_add_minutes, i_add_months, i_add_seconds, i_add_ticks, i_add_years, i_sub, i_to_local;

        private void RegisterInstanceMembers()
        {
            i_add = KecaknoahReference.Right(this, InstanceAdd);
            i_add_days = KecaknoahReference.Right(this, InstanceAddDays);
            i_add_hours = KecaknoahReference.Right(this, InstanceAddHours);
            i_add_minutes = KecaknoahReference.Right(this, InstanceAddMinutes);
            i_add_months = KecaknoahReference.Right(this, InstanceAddMonths);
            i_add_seconds = KecaknoahReference.Right(this, InstanceAddSeconds);
            i_add_ticks = KecaknoahReference.Right(this, InstanceAddTicks);
            i_add_years = KecaknoahReference.Right(this, InstanceAddYears);
            i_sub = KecaknoahReference.Right(this, InstanceSub);
            i_to_local = KecaknoahReference.Right(this, InstanceToLocal);
        }

        private KecaknoahFunctionResult InstanceAdd(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            if (args[0].ExtraType != "DateTime") return KecaknoahNil.Instance.NoResume();
            var result = new KecaknoahDateTime(datetime.Add(((KecaknoahTimeSpan)args[0]).timespan));
            return result.NoResume();
        }

        private KecaknoahFunctionResult InstanceAddDays(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = new KecaknoahDateTime(datetime.AddDays(args[0].ToInt32()));
            return result.NoResume();
        }

        private KecaknoahFunctionResult InstanceAddHours(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = new KecaknoahDateTime(datetime.AddHours(args[0].ToInt32()));
            return result.NoResume();
        }

        private KecaknoahFunctionResult InstanceAddMinutes(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = new KecaknoahDateTime(datetime.AddMinutes(args[0].ToInt32()));
            return result.NoResume();
        }

        private KecaknoahFunctionResult InstanceAddMonths(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = new KecaknoahDateTime(datetime.AddMonths(args[0].ToInt32()));
            return result.NoResume();
        }

        private KecaknoahFunctionResult InstanceAddSeconds(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = new KecaknoahDateTime(datetime.AddSeconds(args[0].ToInt32()));
            return result.NoResume();
        }

        private KecaknoahFunctionResult InstanceAddTicks(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = new KecaknoahDateTime(datetime.AddTicks(args[0].ToInt32()));
            return result.NoResume();
        }

        private KecaknoahFunctionResult InstanceAddYears(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = new KecaknoahDateTime(datetime.AddYears(args[0].ToInt32()));
            return result.NoResume();
        }

        private KecaknoahFunctionResult InstanceSub(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = new KecaknoahDateTime(datetime - ((KecaknoahTimeSpan)args[0]).timespan);
            return result.NoResume();
        }

        private KecaknoahFunctionResult InstanceToLocal(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = new KecaknoahDateTime(datetime.ToLocalTime());
            return result.NoResume();
        }

        #endregion

        #region クラスメソッド

        private static KecaknoahFunctionResult ClassNow(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = new KecaknoahDateTime(DateTime.Now);
            return result.NoResume();
        }

        private static KecaknoahFunctionResult ClassDaysInMonth(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = DateTime.DaysInMonth(args[0].ToInt32(), args[1].ToInt32());
            return result.AsKecaknoahInteger().NoResume();
        }

        private static KecaknoahFunctionResult ClassIsLeap(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = DateTime.IsLeapYear(args[0].ToInt32());
            return result.AsKecaknoahBoolean().NoResume();
        }

        private static KecaknoahFunctionResult ClassParse(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = new KecaknoahDateTime(DateTime.Parse(args[0].ToString()));
            return result.NoResume();
        }

        #endregion
    }
#pragma warning restore 1591
}
