namespace Kecaknoah.Type
{
    /// <summary>
    /// Kecaknoahでの.NET連携メソッドを定義します。
    /// </summary>
    public class KecaknoahInteropFunction : KecaknoahObject
    {
        /// <summary>
        /// 呼び出し対象のデリゲートを取得・設定します。
        /// </summary>
        public KecaknoahInteropDelegate Function { get; set; }

        /// <summary>
        /// インスタンスを取得・設定します。
        /// </summary>
        public KecaknoahObject Instance { get; }

        /// <summary>
        /// 呼び出します。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        protected internal override KecaknoahFunctionResult Call(KecaknoahContext context, KecaknoahObject[] args) => Function(context, Instance, args);

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="inst">インスタンス</param>
        /// <param name="method">メソッド</param>
        public KecaknoahInteropFunction(KecaknoahObject inst, KecaknoahInteropDelegate method)
        {
            Instance = inst;
            Function = method;
        }
    }

    /// <summary>
    /// Kecaknoahでの.NET連携インスタンスメソッドの形式を定義します。
    /// </summary>
    /// <param name="context">実行される<see cref="KecaknoahContext"/></param>
    /// <param name="self">インスタンス。インスタンスメソッドでない場合はnullです。</param>
    /// <param name="args">引数。コルーチンで継続中の場合はnullです。</param>
    /// <returns>返り値</returns>
    public delegate KecaknoahFunctionResult KecaknoahInteropDelegate(KecaknoahContext context, KecaknoahObject self, KecaknoahObject[] args);
}
