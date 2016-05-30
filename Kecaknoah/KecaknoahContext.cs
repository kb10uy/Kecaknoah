using Kecaknoah.Type;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections;
using System.Linq;

namespace Kecaknoah
{
    /// <summary>
    /// Kecaknoahを実行するためのスタックフレームなどのセットを提供します。
    /// </summary>
    public sealed class KecaknoahContext : IEnumerator<KecaknoahObject>
    {
        /// <summary>
        /// 属する<see cref="KecaknoahModule"/>を取得します。
        /// </summary>
        public KecaknoahModule Module { get; private set; }

        /// <summary>
        /// 現在呼び出し中のオブジェクトを取得します。
        /// </summary>
        public KecaknoahObject TargetObject { get; private set; } = KecaknoahNil.Instance;

        /// <summary>
        /// 可変長引数を含めた現在設定されている全ての引数を取得します。
        /// </summary>
        public IList<KecaknoahObject> Arguments { get; private set; } = new List<KecaknoahObject>();

        /// <summary>
        /// 現在の呼び出しが継続中であるかどうかを取得します。
        /// </summary>
        public bool IsResuming { get; private set; }

        /// <summary>
        /// 最後に返却されたオブジェクトを取得します。
        /// </summary>
        public KecaknoahObject Current { get; private set; }

        internal KecaknoahContext(KecaknoahModule module)
        {
            Module = module;
        }

        object IEnumerator.Current => Current;

        /// <summary>
        /// 現在の実行状態を破棄し、先頭からの実行に戻します。
        /// </summary>
        public void Reset()
        {
            IsResuming = false;
        }

        /// <summary>
        /// メソッドの呼び出しを新規に設定します。
        /// </summary>
        /// <param name="obj">呼び出すメソッドに相当する<see cref="KecaknoahObject"/></param>
        /// <param name="args">引数</param>
        public void Initialize(KecaknoahObject obj, IList<KecaknoahObject> args)
        {
            Reset();
            TargetObject = obj;
            Arguments = args;
        }

        /// <summary>
        /// メソッドの呼び出しを新規に設定します。
        /// </summary>
        /// <param name="obj">呼び出すメソッドに相当する<see cref="KecaknoahObject"/></param>
        /// <param name="args">引数</param>
        public void Initialize(KecaknoahObject obj, params KecaknoahObject[] args)
        {
            Reset();
            TargetObject = obj;
            Arguments = args;
        }

        /// <summary>
        /// 指定された<see cref="KecaknoahObject"/>を継続なしで実行し、
        /// 最初に返却された<see cref="KecaknoahObject"/>を返します。
        /// </summary>
        /// <param name="obj">呼び出すメソッドに相当する<see cref="KecaknoahObject"/></param>
        /// <param name="args">引数</param>
        /// <returns>
        /// 最初に返却された<see cref="KecaknoahObject"/>。
        /// 返り値がない場合は<see cref="KecaknoahNil.Instance"/>。
        /// </returns>
        public KecaknoahObject CallInstant(KecaknoahObject obj, IList<KecaknoahObject> args) => obj.Call(this, args.ToArray()).ReturningObject;

        /// <summary>
        /// 指定された<see cref="KecaknoahObject"/>を継続なしで実行し、
        /// 最初に返却された<see cref="KecaknoahObject"/>を返します。
        /// </summary>
        /// <param name="obj">呼び出すメソッドに相当する<see cref="KecaknoahObject"/></param>
        /// <param name="args">引数</param>
        /// <returns>
        /// 最初に返却された<see cref="KecaknoahObject"/>。
        /// 返り値がない場合は<see cref="KecaknoahNil.Instance"/>。
        /// </returns>
        public KecaknoahObject CallInstant(KecaknoahObject obj, params KecaknoahObject[] args) => obj.Call(this, args).ReturningObject;

        /// <summary>
        /// 指定されたILを式として実行し、結果を返します。
        /// </summary>
        /// <param name="il">実行する<see cref="KecaknoahIL"/></param>
        /// <returns>結果</returns>
        public KecaknoahObject ExecuteExpressionIL(KecaknoahIL il)
        {
            var s = new KecaknoahStackFrame(this, il);
            s.Execute();
            return s.ReturningObject;
        }

        /// <summary>
        /// 指定されている<see cref="KecaknoahObject"/>を呼び出し、次の値を取得します。
        /// </summary>
        /// <returns>継続可能な場合はtrue、それ以外の場合はfalse。</returns>
        public bool MoveNext()
        {
            var r = TargetObject.Call(this, IsResuming ? null : Arguments.ToArray());
            Current = r.ReturningObject;
            IsResuming = r.ResultState == KecaknoahFunctionResultType.CanResume;
            return IsResuming;
        }

        /// <summary>
        /// このコンテキストを破棄し、参照を開放します。
        /// </summary>
        public void Dispose()
        {
            Arguments.Clear();
            TargetObject = null;
            Module = null;
        }

    }
}
