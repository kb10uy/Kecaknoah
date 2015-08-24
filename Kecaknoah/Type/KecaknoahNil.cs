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
        public static KecaknoahReference Reference { get; } = KecaknoahReference.CreateRightReference(Instance);

        private KecaknoahNil()
        {

        }

        /// <summary>
        /// 特定の名前を持つメンバーに対してアクセスを試み、値を取得しようとしてもnilです。
        /// </summary>
        /// <param name="name">メンバー名</param>
        /// <returns>アクセスできる場合は対象のオブジェクト</returns>
        protected internal override KecaknoahReference GetMemberReference(string name) => Reference;

        /// <summary>
        /// このオブジェクトに対してメソッドとしての呼び出しをしようとしてもnilです。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="args">引数</param>
        /// <returns>返り値</returns>
        protected internal override KecaknoahFunctionResult Call(KecaknoahContext context, KecaknoahObject[] args) => Instance.NoResume();

        /// <summary>
        /// このオブジェクトに対してインデクサーアクセスを試みようとしてもnilです。 
        /// </summary>
        /// <param name="indices">インデックス引数</param>
        /// <returns></returns>
        protected internal override KecaknoahReference GetIndexerReference(KecaknoahObject[] indices) => Reference;

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
                    return (dynamic)this == (dynamic)target;
                case KecaknoahILCodeType.NotEqual:
                    return (dynamic)this != (dynamic)target;
                default:
                    return KecaknoahNil.Instance;
            }
        }

        /// <summary>
        /// 現在の以下略。
        /// </summary>
        /// <returns>知るか</returns>
        public override string ToString() => "nil";

#pragma warning disable 1591
        public override object Clone() => Instance;
#pragma warning restore 1591
    }
}
