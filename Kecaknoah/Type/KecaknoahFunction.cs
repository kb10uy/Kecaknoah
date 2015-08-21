using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public KecaknoahMethodInfo BaseMethod { get; }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="method"></param>
        public KecaknoahScriptFunction(KecaknoahMethodInfo method)
        {
            BaseMethod = method;
        }

        /// <summary>
        /// 呼び出します。
        /// </summary>
        /// <param name="args">引数</param>
        /// <returns>返り値</returns>
        public override KecaknoahObject Call(KecaknoahObject[] args)
        {
            //TODO: スタティックメソッド呼び出し実装
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// インスタンスメソッドを提供します。
    /// </summary>
    public class KecaknoahScriptInstanceFunction : KecaknoahScriptFunction
    {
        /// <summary>
        /// インスタンスを取得します。
        /// </summary>
        public KecaknoahObject Instance { get; }

        /// <summary>
        /// 初期化します。
        /// </summary>
        /// <param name="inst"></param>
        /// <param name="method"></param>
        public KecaknoahScriptInstanceFunction(KecaknoahObject inst, KecaknoahScriptMethodInfo method) : base(method)
        {
            Instance = inst;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public override KecaknoahObject Call(KecaknoahObject[] args)
        {
            //TODO: インスタンスメソッド呼び出し実装
            throw new NotImplementedException();
        }
    }
}
