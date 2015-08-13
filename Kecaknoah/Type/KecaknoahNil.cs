using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah.Type
{
    /// <summary>
    /// Kecaknoahのnil(null)を定義します。
    /// </summary>
    public sealed class KecaknoahNil : KecaknoahObject
    {

        private static KecaknoahNil instance = new KecaknoahNil();
        /// <summary>
        /// 唯一のインスタンスを取得します。
        /// </summary>
        public static KecaknoahNil Instance { get; } = instance;

        private KecaknoahNil()
        {

        }

        /// <summary>
        /// 特定の名前を持つメンバーに対してアクセスを試み、値を取得しようとしてもnilです。
        /// </summary>
        /// <param name="name">メンバー名</param>
        /// <returns>アクセスできる場合は対象のオブジェクト</returns>
        public override KecaknoahObject GetMember(string name) => Instance;

        /// <summary>
        /// 特定の名前を持つメンバーに対してアクセスを試み、値を設定しようとしてもnilです。
        /// </summary>
        /// <param name="name">メンバー名</param>
        /// <param name="obj">代入する<see cref="KecaknoahObject"/></param>
        public override void SetMember(string name, KecaknoahObject obj)
        {

        }

        /// <summary>
        /// このオブジェクトに対してメソッドとしての呼び出しをしようとしてもnilです。
        /// </summary>
        /// <param name="args">引数</param>
        /// <returns>返り値</returns>
        public override KecaknoahObject Call(KecaknoahObject[] args) => Instance;

        /// <summary>
        /// このオブジェクトに対してインデクサーアクセスを試みようとしてもnilです。 
        /// </summary>
        /// <param name="indices">インデックス引数</param>
        /// <returns></returns>
        public override KecaknoahObject GetIndexer(KecaknoahObject[] indices) => Instance;

        /// <summary>
        /// このオブジェクトに対してインデクサーアクセスを試みようとしてもnilです。 
        /// </summary>
        /// <param name="args">インデックス引数</param>
        /// <param name="obj">代入する<see cref="KecaknoahObject"/></param>
        public override void SetIndexer(KecaknoahObject[] args, KecaknoahObject obj)
        {

        }

        /// <summary>
        /// このオブジェクトに対して二項式としての演算をしようとしてもnilです。
        /// </summary>
        /// <param name="op">演算子</param>
        /// <param name="target">2項目の<see cref="KecaknoahObject"/></param>
        /// <returns></returns>
        public override KecaknoahObject ExpressionOperation(KecaknoahILCodeType op, KecaknoahObject target) => Instance;

        /// <summary>
        /// このインスタンスを等価な.NETオブジェクトに変換しようとしてもnilです。
        /// </summary>
        /// <typeparam name="T">変換対象の型</typeparam>
        /// <returns>変換結果</returns>
        public override object AsRawObject<T>() => Instance;

        /// <summary>
        /// 現在の以下略。
        /// </summary>
        /// <returns>知るか</returns>
        public override string ToString() => "nil";
    }
}
