using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah.Type
{
    /// <summary>
    /// 配列を提供します。
    /// </summary>
    public class KecaknoahArray : KecaknoahObject
    {
        private KecaknoahReference[] array;

        internal KecaknoahArray(int[] dim)
        {
            array = new KecaknoahReference[dim[0]];
            for (int i = 0; i < array.Length; i++) array[i] = new KecaknoahReference { IsLeftValue = true, RawObject = KecaknoahNil.Instance };
            if (dim.Length == 1) return;
            for (int i = 0; i < array.Length; i++) array[i] = new KecaknoahReference { IsLeftValue = true, RawObject = new KecaknoahArray(dim.Skip(1).ToArray()) };
        }

        /// <summary>
        /// 配列要素の参照を取得します。
        /// </summary>
        /// <param name="indices"></param>
        /// <returns></returns>
        protected internal override KecaknoahReference GetIndexerReference(KecaknoahObject[] indices)
        {
            if (indices.Length != 1) throw new ArgumentException("配列のインデックスの数は必ず1です。");
            return array[indices[0].ToInt64()];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"Array: {array.Length} elements";
    }
}
