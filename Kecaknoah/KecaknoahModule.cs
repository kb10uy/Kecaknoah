using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah
{
    /// <summary>
    /// Kecaknoahのモジュール(名前空間)を定義します。
    /// </summary>
    public class KecaknoahModule
    {
        /// <summary>
        /// このインスタンスが定義されている<see cref="KecaknoahEnvironment"/>を取得します。
        /// </summary>
        public KecaknoahEnvironment Environment { get; internal set; }

        /// <summary>
        /// このインスタンスの名前を取得します。
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// <see cref="KecaknoahModule"/>の新しいインスタンスを生成します。
        /// </summary>
        protected internal KecaknoahModule(string name)
        {
            Name = name;
        }
    }
}
