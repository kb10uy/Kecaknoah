using System.Collections.Generic;
using System.Linq;

namespace Kecaknoah.Type
{
    /// <summary>
    /// Kecaknoahで定義されたメソッドのオブジェクトを定義します。
    /// </summary>
    public sealed class KecaknoahScriptFunction : KecaknoahObject
    {
        /// <summary>
        /// 基となるメソッドを取得します。
        /// </summary>
        public KecaknoahScriptMethodInfo BaseMethod { get; }

        /// <summary>
        /// インスタンスを取得します。
        /// </summary>
        public KecaknoahObject Instance { get; }

        /// <summary>
        /// このメソッドの現在のフレームを取得します。
        /// </summary>
        public KecaknoahStackFrame CurrentFrame { get; private set; }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="inst">インスタンス</param>
        /// <param name="method">メソッド</param>
        public KecaknoahScriptFunction(KecaknoahObject inst, KecaknoahScriptMethodInfo method)
        {
            ExtraType = "ScriptFunction";
            Instance = inst ?? KecaknoahNil.Instance;
            BaseMethod = method;
        }

        /// <summary>
        /// 呼び出します。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="args">引数</param>
        /// <returns>返り値</returns>
        protected internal override KecaknoahFunctionResult Call(KecaknoahContext context, KecaknoahObject[] args)
        {
            if (args != null)
            {
                CurrentFrame = new KecaknoahStackFrame(context, BaseMethod.Codes);
                CurrentFrame.Locals["self"] = KecaknoahReference.Right(Instance);
                CurrentFrame.Arguments = args;
            }
            var r = CurrentFrame.Resume();
            return new KecaknoahFunctionResult(CurrentFrame.ReturningObject, r);
        }
    }
}
