using System.Collections.Generic;
using System.Linq;

namespace Kecaknoah.Type
{
    /// <summary>
    /// Kecaknoahで定義されたメソッドのオブジェクトを定義します。
    /// </summary>
    public class KecaknoahScriptFunction : KecaknoahObject
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
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="inst">インスタンス</param>
        /// <param name="method">メソッド</param>
        public KecaknoahScriptFunction(KecaknoahObject inst, KecaknoahScriptMethodInfo method)
        {
            Instance = inst;
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
            var sf = new KecaknoahStackFrame(context, BaseMethod.Codes);
            sf.Locals["self"] = KecaknoahReference.CreateRightReference(Instance);
            sf.Arguments = args;
            var r = sf.Resume();
            return new KecaknoahFunctionResult(sf.ReturningObject, r);
        }
    }
}
