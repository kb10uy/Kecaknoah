using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah.Type
{
    /// <summary>
    /// Kecaknoahでの.NET連携メソッドを定義します。
    /// </summary>
    public sealed class KecaknoahInteropFunction : KecaknoahObject
    {
        /// <summary>
        /// 呼び出し対象のデリゲートを取得・設定します。
        /// </summary>
        public KecaknoahDelegate Function { get; set; }

        /// <summary>
        /// 呼び出します。
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public override KecaknoahObject Call(KecaknoahObject[] args) => Function(args);
    }

    /// <summary>
    /// Kecaknoahでの.NET連携メソッドの形式を定義します。
    /// </summary>
    /// <param name="args">実引数</param>
    /// <returns>返り値</returns>
    public delegate KecaknoahObject KecaknoahDelegate(KecaknoahObject[] args);
}
