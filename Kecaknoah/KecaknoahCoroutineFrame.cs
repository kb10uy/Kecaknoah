using Kecaknoah.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah
{
    /// <summary>
    /// コルーチン実行の際にデータを保持します。
    /// </summary>
    public class KecaknoahCoroutineFrame
    {
        /// <summary>
        /// 再開します。
        /// </summary>
        /// <returns></returns>
        public virtual KecaknoahFunctionResult Resume() => KecaknoahNil.Instance.NoResume();
    }

    internal sealed class KecaknoahInteropCoroutineFrame : KecaknoahCoroutineFrame
    {
        KecaknoahInteropFunction Function { get; }
        KecaknoahContext Context { get; }
        KecaknoahObject[] Args { get; }

        public KecaknoahInteropCoroutineFrame(KecaknoahContext ctx, KecaknoahInteropFunction func, KecaknoahObject[] args)
        {
            Function = func;
            Context = ctx;
        }

        public override KecaknoahFunctionResult Resume() => Function.Function(Context, Function.Instance, Args);
    }

    internal sealed class KecaknoahScriptCoroutineFrame : KecaknoahCoroutineFrame
    {
        KecaknoahStackFrame StackFrame { get; }
        KecaknoahObject[] Args { get; }

        public KecaknoahScriptCoroutineFrame(KecaknoahContext ctx, KecaknoahScriptFunction func, KecaknoahObject[] args)
        {
            StackFrame = new KecaknoahStackFrame(ctx, func.BaseMethod.Codes);
            StackFrame.Arguments = Args;
        }

        public override KecaknoahFunctionResult Resume()
        {
            var s = StackFrame.Resume();
            return new KecaknoahFunctionResult(StackFrame.ReturningObject, s);
        }
    }
}
