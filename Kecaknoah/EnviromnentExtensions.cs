using Kecaknoah.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah
{
    internal static class EnvironmentExtensions
    {
        /// <summary>
        /// 右辺値化します。
        /// </summary>
        /// <param name="obj">対象</param>
        /// <returns></returns>
        public static KecaknoahReference AsRightValue(this KecaknoahObject obj) => new KecaknoahReference { Object = obj, IsLeftValue = false };
        /// <summary>
        /// 左辺値化します。
        /// </summary>
        /// <param name="obj">対象</param>
        /// <returns></returns>
        public static KecaknoahReference AsLeftValue(this KecaknoahObject obj) => new KecaknoahReference { Object = obj, IsLeftValue = true };
    }
}
