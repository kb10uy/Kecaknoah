using Kecaknoah.Type;
using System;
using System.Diagnostics;

namespace Kecaknoah
{
    /// <summary>
    /// Kecaknoahを実行するためのスタックフレームなどのセットを提供します。
    /// </summary>
    public sealed class KecaknoahContext
    {
        /// <summary>
        /// 属する<see cref="KecaknoahModule"/>を取得します。
        /// </summary>
        public KecaknoahModule Module { get; }

        internal KecaknoahContext(KecaknoahModule module)
        {
            Module = module;
        }

        internal KecaknoahContext(KecaknoahContext parent)
        {
            Module = parent.Module;
        }


        /// <summary>
        /// 指定した<see cref="KecaknoahObject"/>をメソッドとして呼び出します。
        /// </summary>
        /// <param name="obj">呼び出す<see cref="KecaknoahObject"/></param>
        /// <param name="args">引数</param>
        /// <returns>結果</returns>
        public KecaknoahObject Execute(KecaknoahObject obj, params KecaknoahObject[] args)
        {
            var r = obj.Call(this, args).ReturningObject;
            return r;
        }
        /// <summary>
        /// 指定した<see cref="KecaknoahIL"/>を式として実行します。
        /// </summary>
        /// <param name="il"></param>
        /// <returns>結果</returns>
        public KecaknoahObject Execute(KecaknoahIL il)
        {
            var sf = new KecaknoahStackFrame(this, il);
            sf.Execute();
            return sf.ReturningObject;
        }

        /// <summary>
        /// 指定した<see cref="KecaknoahIL"/>を式として実行し、<see cref="KecaknoahStackFrame"/>を返します。
        /// </summary>
        /// <param name="il"></param>
        /// <returns>結果</returns>
        public KecaknoahStackFrame ExecuteWithStackFrame(KecaknoahIL il)
        {
            var sf = new KecaknoahStackFrame(this, il);
            sf.Execute();
            return sf;
        }
    }
}
