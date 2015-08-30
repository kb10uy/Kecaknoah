using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah.External
{
    /// <summary>
    /// プラグインクラスを定義する際の情報を定義します。
    /// </summary>
    public sealed class KecaknoahExternalClassInfo
    {
        /// <summary>
        /// このクラスが静的クラスで、インスタンスメソッドを持たない場合はtrueを返します。
        /// </summary>
        public bool IsStaticClass { get; set; }

        /// <summary>
        /// このクラスの名前を取得します。
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// このクラスを定義する<see cref="KecaknoahInteropClassInfo"/>を取得します。
        /// </summary>
        public KecaknoahInteropClassInfo Information { get; set; }
    }

    /// <summary>
    /// <see cref="KecaknoahExternalClassInfo"/>を取得するためのデリゲートを定義します。
    /// </summary>
    /// <returns></returns>
    public delegate KecaknoahExternalClassInfo ExternalInfoFetcher();
}
