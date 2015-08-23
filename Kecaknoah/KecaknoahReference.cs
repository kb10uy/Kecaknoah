using Kecaknoah.Type;
using System;

namespace Kecaknoah
{
    /// <summary>
    /// Kecaknoahでの値に関する全ての参照を定義します。
    /// </summary>
    public class KecaknoahReference
    {
        private KecaknoahObject obj = KecaknoahNil.Instance;
        /// <summary>
        /// 内部的に保持する<see cref="KecaknoahObject"/>を取得します。
        /// </summary>
        public KecaknoahObject RawObject
        {
            get { return obj; }
            set
            {
                if (!IsLeftValue) throw new InvalidOperationException("右辺値です");
                obj = value;
            }
        }

        /// <summary>
        /// この参照が左辺値であるかどうかを取得します。
        /// </summary>
        public bool IsLeftValue { get; protected internal set; }

        /// <summary>
        /// 指定した名前の参照を取得します。
        /// </summary>
        /// <param name="name">名前</param>
        /// <returns>参照</returns>
        public KecaknoahReference GetMemberReference(string name) => RawObject.GetMemberReference(name);

        /// <summary>
        /// 右辺値を生成します。
        /// </summary>
        /// <param name="sobj">対象</param>
        /// <returns>右辺値参照</returns>
        public static KecaknoahReference CreateRightReference(KecaknoahObject sobj) => new KecaknoahReference
        {
            IsLeftValue = false,
            obj = sobj
        };

        /// <summary>
        /// 右辺値を生成します。
        /// </summary>
        /// <param name="sobj">対象</param>
        /// <returns>右辺値参照</returns>
        public static KecaknoahReference CreateRightReference(long sobj) => new KecaknoahReference
        {
            IsLeftValue = false,
            obj = sobj.AsKecaknoahInteger()
        };

        /// <summary>
        /// 右辺値を生成します。
        /// </summary>
        /// <param name="sobj">対象</param>
        /// <returns>右辺値参照</returns>
        public static KecaknoahReference CreateRightReference(double sobj) => new KecaknoahReference
        {
            IsLeftValue = false,
            obj = sobj.AsKecaknoahFloat()
        };

        /// <summary>
        /// 右辺値を生成します。
        /// </summary>
        /// <param name="sobj">対象</param>
        /// <returns>右辺値参照</returns>
        public static KecaknoahReference CreateRightReference(bool sobj) => new KecaknoahReference
        {
            IsLeftValue = false,
            obj = sobj.AsKecaknoahBoolean()
        };

        /// <summary>
        /// 右辺値を生成します。
        /// </summary>
        /// <param name="sobj">対象</param>
        /// <returns>右辺値参照</returns>
        public static KecaknoahReference CreateRightReference(string sobj) => new KecaknoahReference
        {
            IsLeftValue = false,
            obj = sobj.AsKecaknoahString()
        };

        /// <summary>
        /// 右辺値を生成します。
        /// </summary>
        /// <param name="self">属するインスタンス</param>
        /// <param name="sobj">対象</param>
        /// <returns>右辺値参照</returns>
        public static KecaknoahReference CreateRightReference(KecaknoahObject self, KecaknoahInteropDelegate sobj) => new KecaknoahReference
        {
            IsLeftValue = false,
            obj = new KecaknoahInteropFunction(self, sobj)
        };
    }
}
