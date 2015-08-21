using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah.Type
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
        /// 呼び出します。
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public override KecaknoahObject Call(KecaknoahObject[] args) => Function(KecaknoahNil.Instance, args);
    }

    /// <summary>
    /// Kecaknoahでの.NET連携インスタンスメソッドを定義します。
    /// </summary>
    public class KecaknoahInteropInstanceFunction : KecaknoahInteropFunction
    {
        /// <summary>
        /// 呼び出し対象のデリゲートを取得・設定します。
        /// </summary>
        public new KecaknoahInteropDelegate Function { get; }

        /// <summary>
        /// インスタンスを取得・設定します。
        /// </summary>
        public KecaknoahObject Instance { get; }

        /// <summary>
        /// 呼び出します。
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public override KecaknoahObject Call(KecaknoahObject[] args) => Function(Instance, args);

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="instance">属するインスタンス</param>
        /// <param name="func">メソッド</param>
        public KecaknoahInteropInstanceFunction(KecaknoahObject instance,KecaknoahInteropDelegate func)
        {
            Function = func;
            Instance = instance;
        }
    }

    /// <summary>
    /// Kecaknoahでの.NET連携インスタンスメソッドの形式を定義します。
    /// </summary>
    /// <param name="self">インスタンス</param>
    /// <param name="args">引数</param>
    /// <returns>返り値</returns>
    public delegate KecaknoahObject KecaknoahInteropDelegate(KecaknoahObject self, KecaknoahObject[] args);
}
