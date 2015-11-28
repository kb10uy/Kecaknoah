using Kecaknoah.Type;
using System;

namespace Kecaknoah.Standard
{
#pragma warning disable 1591
    /// <summary>
    /// Kecaknoahに乱数を提供します。
    /// </summary>
    public sealed class KecaknoahRandom : KecaknoahObject
    {
        public static readonly string ClassName = "Random";
        internal static KecaknoahInteropClassInfo Information { get; } = new KecaknoahInteropClassInfo(ClassName);
        Random random;

        #region overrideメンバー
        static KecaknoahRandom()
        {
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("new", ClassNew));
        }


        public KecaknoahRandom()
        {
            ExtraType = ClassName;
            random = new Random();
            RegisterInstanceMembers();
        }

        protected internal override KecaknoahReference GetMemberReference(string name)
        {
            switch (name)
            {
                case "get_int": return i_get_int;
                case "get_float": return i_get_float;

                default: return base.GetMemberReference(name);
            }
        }
        #endregion

        #region インスタンスメンバー
        private KecaknoahReference i_get_int, i_get_float;

        private void RegisterInstanceMembers()
        {
            i_get_int = KecaknoahReference.Right(this, InstanceGetInteger);
            i_get_float = KecaknoahReference.Right(this, InstanceGetFloat);

        }

        private KecaknoahFunctionResult InstanceGetInteger(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            switch (args.Length)
            {
                case 0:
                    return random.Next().AsKecaknoahInteger().NoResume();
                case 1:
                    var a = args.ExpectInt32(1, false);
                    return random.Next(a[0]).AsKecaknoahInteger().NoResume();
                default:
                    var a2 = args.ExpectInt32(2, false);
                    return random.Next(a2[0], a2[1]).AsKecaknoahInteger().NoResume();
            }
        }

        private KecaknoahFunctionResult InstanceGetFloat(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args) => random.NextDouble().AsKecaknoahFloat().NoResume();
        #endregion

        #region クラスメソッド
        private static KecaknoahFunctionResult ClassNew(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args) => new KecaknoahRandom().NoResume();
        #endregion

    }

    /// <summary>
    /// KecaknoahにXorshiftを提供します。
    /// </summary>
    public sealed class KecaknoahXorshift : KecaknoahObject
    {
        public static readonly string ClassName = "Xorshift";
        internal static KecaknoahInteropClassInfo Information { get; } = new KecaknoahInteropClassInfo(ClassName);
        int x, y, z, w;

        #region overrideメンバー
        static KecaknoahXorshift()
        {
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("new", ClassNew));
        }


        public KecaknoahXorshift(int xs,int ys,int zs,int ws)
        {
            ExtraType = ClassName;
            RegisterInstanceMembers();
            x = xs;
            y = ys;
            z = zs;
            w = ws;
        }

        public KecaknoahXorshift(int xs) : this(xs, 362436069, 521288629, 88675123)
        {
        }

        public KecaknoahXorshift() : this(new Random().Next(), 362436069, 521288629, 88675123)
        {
        }

        protected internal override KecaknoahReference GetMemberReference(string name)
        {
            switch (name)
            {
                case "get_int": return i_get_int;
                case "get_float": return i_get_float;

                default: return base.GetMemberReference(name);
            }
        }
        #endregion

        #region インスタンスメンバー
        private int NextRawInteger()
        {
            int t = x ^ (x << 11);
            x = y; y = z; z = w;
            return w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));
        }

        private int NextInteger(int max)
        {
            int t = x ^ (x << 11);
            x = y; y = z; z = w;
            w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));
            return w % max;
        }

        private int NextInteger(int start, int count)
        {
            int t = x ^ (x << 11);
            x = y; y = z; z = w;
            w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));
            return w % count + start;
        }

        private double NextRawDouble() => (uint)NextRawInteger() / (double)uint.MaxValue;

        private KecaknoahReference i_get_int, i_get_float;

        private void RegisterInstanceMembers()
        {
            i_get_int = KecaknoahReference.Right(this, InstanceGetInteger);
            i_get_float = KecaknoahReference.Right(this, InstanceGetFloat);

        }

        private KecaknoahFunctionResult InstanceGetInteger(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            switch (args.Length)
            {
                case 0:
                    return NextRawInteger().AsKecaknoahInteger().NoResume();
                case 1:
                    var a = args.ExpectInt32(1, false);
                    return NextInteger(a[0]).AsKecaknoahInteger().NoResume();
                default:
                    var a2 = args.ExpectInt32(2, false);
                    return NextInteger(a2[0], a2[1]).AsKecaknoahInteger().NoResume();
            }
        }

        private KecaknoahFunctionResult InstanceGetFloat(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args) => NextRawDouble().AsKecaknoahFloat().NoResume();
        #endregion

        #region クラスメソッド
        private static KecaknoahFunctionResult ClassNew(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            switch(args.Length)
            {
                case 0:
                    return new KecaknoahXorshift().NoResume();
                case 1:
                    return new KecaknoahXorshift(args[0].ToInt32()).NoResume();
                case 4:
                    var seeds = args.ExpectInt32(4, false);
                    return new KecaknoahXorshift(seeds[0], seeds[1], seeds[2], seeds[3]).NoResume();
                default:
                    return new KecaknoahXorshift().NoResume();
            }
        }
        #endregion

    }
#pragma warning restore 1591
}
