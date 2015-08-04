using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah.Type
{
    /// <summary>
    /// Kecaknoahで利用される型のアクセスを提供します。
    /// </summary>
    public class KecaknoahObject
    {

        /// <summary>
        /// 特定の名前を持つメンバーに対してアクセスを試み、値を取得します。
        /// </summary>
        /// <param name="name">メンバー名</param>
        /// <returns>アクセスできる場合は対象のオブジェクト</returns>
        public virtual KecaknoahObject GetMember(string name) => null;

        /// <summary>
        /// 特定の名前を持つメンバーに対してアクセスを試み、値を設定します。
        /// </summary>
        /// <param name="name">メンバー名</param>
        /// <param name="obj">代入する<see cref="KecaknoahObject"/></param>
        public virtual void SetMember(string name, KecaknoahObject obj)
        {
            throw new InvalidOperationException($"この{nameof(KecaknoahObject)}に対して代入は出来ません。");
        }

        /// <summary>
        /// このオブジェクトに対してメソッドとしての呼び出しをします。
        /// </summary>
        /// <param name="args">引数</param>
        /// <returns>返り値</returns>
        public virtual KecaknoahObject Call(KecaknoahObject[] args)
        {
            throw new InvalidOperationException($"この{nameof(KecaknoahObject)}に対してメソッド呼び出しは出来ません。");
        }

        /// <summary>
        /// このオブジェクトに対してインデクサーアクセスを試みます。 
        /// </summary>
        /// <param name="indices">インデックス引数</param>
        /// <returns></returns>
        public virtual KecaknoahObject GetIndexer(KecaknoahObject[] indices)
        {
            throw new InvalidOperationException($"この{nameof(KecaknoahObject)}に対してインデクサー呼び出しは出来ません。");
        }

        /// <summary>
        /// このオブジェクトに対してインデクサーアクセスを試みます。 
        /// </summary>
        /// <param name="args">インデックス引数</param>
        /// <param name="obj">代入する<see cref="KecaknoahObject"/></param>
        public virtual void SetIndexer(KecaknoahObject[] args,KecaknoahObject obj)
        {
            throw new InvalidOperationException($"この{nameof(KecaknoahObject)}に対してインデクサー呼び出しは出来ません。");
        }

        /// <summary>
        /// このオブジェクトに対して二項式としての演算をします。
        /// </summary>
        /// <param name="op">演算子</param>
        /// <param name="target">2項目の<see cref="KecaknoahObject"/></param>
        /// <returns></returns>
        public virtual KecaknoahObject ExpressionOperation(KecaknoahOperatorType op,KecaknoahObject target)
        {
            throw new InvalidOperationException($"この{nameof(KecaknoahObject)}に対して式操作は出来ません。");
        }

        /// <summary>
        /// このインスタンスを等価な.NETオブジェクトに変換します。
        /// </summary>
        /// <typeparam name="T">変換対象の型</typeparam>
        /// <returns>変換結果</returns>
        public virtual object AsRawObject<T>() => default(T);
    }
}
