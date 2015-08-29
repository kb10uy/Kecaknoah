using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Kecaknoah.Type;

namespace Kecaknoah.Standard
{
#pragma warning disable 1591
    /// <summary>
    /// Kecaknoahに数学関数を提供します。
    /// </summary>
    public sealed class KecaknoahMath : KecaknoahObject
    {
        public static readonly string ClassName = "Math";
        internal static KecaknoahInteropClassInfo Information { get; } = new KecaknoahInteropClassInfo(ClassName);

        #region overrideメンバー
        static KecaknoahMath()
        {
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("sin", ClassSin));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("cos", ClassCos));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("tan", ClassTan));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("sinh", ClassSinh));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("cosh", ClassCosh));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("tanh", ClassTanh));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("asin", ClassAsin));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("acos", ClassAcos));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("atan", ClassAtan));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("atan2", ClassAtan2));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("max", ClassMax));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("min", ClassMin));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("limit", ClassLimit));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("abs", ClassAbs));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("ceil", ClassCeil));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("floor", ClassFloor));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("exp", ClassExp));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("log", ClassLog));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("log10", ClassLog10));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("pow", ClassPow));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("round", ClassRound));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("sign", ClassSign));
        }

        public KecaknoahMath()
        {
            ExtraType = ClassName;
            RegisterInstanceMembers();
        }

        protected internal override KecaknoahReference GetMemberReference(string name)
        {
            switch (name)
            {

                default: return base.GetMemberReference(name);
            }
        }

        #endregion

        #region インスタンスメンバー

        private void RegisterInstanceMembers()
        {

        }

        #endregion
        #region クラスメソッド

        private static KecaknoahFunctionResult ClassSin(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = Math.Sin(args[0].ToDouble());
            return result.AsKecaknoahFloat().NoResume();
        }

        private static KecaknoahFunctionResult ClassCos(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = Math.Cos(args[0].ToDouble());
            return result.AsKecaknoahFloat().NoResume();
        }

        private static KecaknoahFunctionResult ClassTan(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = Math.Tan(args[0].ToDouble());
            return result.AsKecaknoahFloat().NoResume();
        }

        private static KecaknoahFunctionResult ClassSinh(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = Math.Sinh(args[0].ToDouble());
            return result.AsKecaknoahFloat().NoResume();
        }

        private static KecaknoahFunctionResult ClassCosh(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = Math.Cosh(args[0].ToDouble());
            return result.AsKecaknoahFloat().NoResume();
        }

        private static KecaknoahFunctionResult ClassTanh(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = Math.Tanh(args[0].ToDouble());
            return result.AsKecaknoahFloat().NoResume();
        }

        private static KecaknoahFunctionResult ClassAsin(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = Math.Asin(args[0].ToDouble());
            return result.AsKecaknoahFloat().NoResume();
        }

        private static KecaknoahFunctionResult ClassAcos(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = Math.Acos(args[0].ToDouble());
            return result.AsKecaknoahFloat().NoResume();
        }

        private static KecaknoahFunctionResult ClassAtan(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = Math.Atan(args[0].ToDouble());
            return result.AsKecaknoahFloat().NoResume();
        }

        private static KecaknoahFunctionResult ClassAtan2(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var da = args.ExpectDouble(2, false);
            var result = Math.Atan2(da[0], da[1]);
            return result.AsKecaknoahFloat().NoResume();
        }

        private static KecaknoahFunctionResult ClassMax(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var da = args.ExpectDouble(2, false);
            var result = Math.Max(da[0], da[1]);
            return result.AsKecaknoahFloat().NoResume();
        }

        private static KecaknoahFunctionResult ClassMin(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var da = args.ExpectDouble(2, false);
            var result = Math.Min(da[0], da[1]);
            return result.AsKecaknoahFloat().NoResume();
        }

        private static KecaknoahFunctionResult ClassLimit(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var da = args.ExpectDouble(3, false);
            var result = da[0];
            if (result < da[1]) result = da[1];
            if (result > da[2]) result = da[2];
            return result.AsKecaknoahFloat().NoResume();
        }

        private static KecaknoahFunctionResult ClassAbs(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = Math.Abs(args[0].ToDouble());
            return result.AsKecaknoahFloat().NoResume();
        }

        private static KecaknoahFunctionResult ClassCeil(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = Math.Ceiling(args[0].ToDouble());
            return result.AsKecaknoahFloat().NoResume();
        }

        private static KecaknoahFunctionResult ClassFloor(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = Math.Floor(args[0].ToDouble());
            return result.AsKecaknoahFloat().NoResume();
        }

        private static KecaknoahFunctionResult ClassExp(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = Math.Exp(args[0].ToDouble());
            return result.AsKecaknoahFloat().NoResume();
        }

        private static KecaknoahFunctionResult ClassLog(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = Math.Log(args[0].ToDouble());
            return result.AsKecaknoahFloat().NoResume();
        }

        private static KecaknoahFunctionResult ClassLog10(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = Math.Log10(args[0].ToDouble());
            return result.AsKecaknoahFloat().NoResume();
        }

        private static KecaknoahFunctionResult ClassPow(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var da = args.ExpectDouble(2, false);
            var result = Math.Pow(da[0], da[1]);
            return result.AsKecaknoahFloat().NoResume();
        }

        private static KecaknoahFunctionResult ClassRound(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = Math.Round(args[0].ToDouble());
            return result.AsKecaknoahFloat().NoResume();
        }

        private static KecaknoahFunctionResult ClassSign(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = Math.Sign(args[0].ToDouble());
            return result.AsKecaknoahInteger().NoResume();
        }

        #endregion
    }
#pragma warning restore 1591
}
