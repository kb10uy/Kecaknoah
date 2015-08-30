using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kecaknoah.Type;

namespace Kecaknoah.Standard
{
#pragma warning disable 1591
    /// <summary>
    /// OSとのやりとりを提供します。
    /// </summary>
    public sealed class KecaknoahSystem
    {
        public static readonly string ClassName = "System";
        internal static KecaknoahInteropClassInfo Information { get; } = new KecaknoahInteropClassInfo(ClassName);

        static KecaknoahSystem()
        {

        }

        public KecaknoahSystem()
        {
        }

        private static KecaknoahFunctionResult ClassFoo(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args) => KecaknoahNil.Instance.NoResume();
    }
#pragma warning restore 1591
}
