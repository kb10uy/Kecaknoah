using System;

namespace Kecaknoah.Type
{
    /// <summary>
    /// Kecaknoahのnil(null)を定義します。
    /// </summary>
    public sealed class KecaknoahNil : KecaknoahObject
    {

        private static KecaknoahNil instance = new KecaknoahNil();
        /// <summary>
        /// 唯一のインスタンスを取得します。
        /// </summary>
        public static KecaknoahNil Instance { get; } = instance;
        /// <summary>
        /// <see cref="Instance"/>への参照を取得します。
        /// </summary>
        public static KecaknoahReference Reference { get; } = KecaknoahReference.Right(Instance);

        private KecaknoahNil()
        {
            Type = TypeCode.Empty;
            ExtraType = "Nil";
        }

        /// <summary>
        /// このオブジェクトに対して二項式としての演算をしようとしても大体nilです。
        /// </summary>
        /// <param name="op">演算子</param>
        /// <param name="target">2項目の<see cref="KecaknoahObject"/></param>
        /// <returns></returns>
        protected internal override KecaknoahObject ExpressionOperation(KecaknoahILCodeType op, KecaknoahObject target)
        {
            switch (op)
            {
                case KecaknoahILCodeType.Equal:
                    return (ExtraType == target.ExtraType).AsKecaknoahBoolean();
                case KecaknoahILCodeType.NotEqual:
                    return (ExtraType != target.ExtraType).AsKecaknoahBoolean();
                default:
                    return Instance;
            }
        }

        /// <summary>
        /// 現在の以下略。
        /// </summary>
        /// <returns>知るか</returns>
        public override string ToString() => "nil";

#pragma warning disable 1591
        public override object Clone() => Instance;
        public override bool Equals(object obj) => obj is KecaknoahNil;
        public override int GetHashCode() => "nil".GetHashCode();
#pragma warning restore 1591
    }
}
