using System;

namespace Kecaknoah.Type
{
    /// <summary>
    /// Kecaknoahで定義されるメソッドを定義します。
    /// </summary>
    public sealed class KecaknoahScriptMethodInfo : KecaknoahMethodInfo
    {
        /// <summary>
        /// このメソッドの名前を取得します。
        /// </summary>
        public override string Name { get; }

        /// <summary>
        /// 引数の数を取得します。
        /// </summary>
        public override int ArgumentLength { get; }

        /// <summary>
        /// 可変長引数メソッドかどうかを取得します。
        /// </summary>
        public override bool VariableArgument { get; }

        /// <summary>
        /// このメソッドの<see cref="KecaknoahIL"/>を取得します。
        /// </summary>
        public KecaknoahIL Codes { get; set; }

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="name">メソッド名</param>
        /// <param name="length">引数の数</param>
        /// <param name="vargs">可変長引数の場合はtrue</param>
        public KecaknoahScriptMethodInfo(string name, int length, bool vargs)
        {
            Name = name;
            ArgumentLength = length;
            VariableArgument = vargs;
        }

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="name">メソッド名</param>
        public KecaknoahScriptMethodInfo(string name)
        {
            Name = name;
            ArgumentLength = 0;
            VariableArgument = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override KecaknoahObject CreateFunctionObject()
        {
            throw new NotImplementedException();
        }
    }
}
