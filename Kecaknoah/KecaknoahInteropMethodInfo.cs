using Kecaknoah.Type;
using System;

namespace Kecaknoah
{
    /// <summary>
    /// .NETで定義したメソッドの情報を提供します。
    /// </summary>
    public sealed class KecaknoahInteropMethodInfo : KecaknoahMethodInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public override int ArgumentLength { get; }

        /// <summary>
        /// 
        /// </summary>
        public override string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        public override bool VariableArgument => false;

        /// <summary>
        /// 実行される<see cref="KecaknoahInteropDelegate"/>を取得します。
        /// </summary>
        public KecaknoahInteropDelegate Body { get; }

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="name">メソッド名</param>
        /// <param name="length">引数の数</param>
        /// <param name="bd">メソッドのデリゲート</param>
        public KecaknoahInteropMethodInfo(string name, int length, KecaknoahInteropDelegate bd)
        {
            Name = name;
            ArgumentLength = length;
            Body = bd;
        }

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="name">メソッド名</param>
        /// <param name="bd">メソッドのデリゲート</param>
        public KecaknoahInteropMethodInfo(string name, KecaknoahInteropDelegate bd) : this(name, 0, bd)
        {
        }

        /// <summary>
        /// Callできる<see cref="KecaknoahObject"/>を生成します。
        /// </summary>
        /// <returns></returns>
        public override KecaknoahObject CreateFunctionObject()
        {
            throw new NotImplementedException();
        }
    }
}
