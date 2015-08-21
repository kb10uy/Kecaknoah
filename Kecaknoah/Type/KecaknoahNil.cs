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
        /// <summary>
        /// <see cref="Instance"/>への参照を取得します。
        /// </summary>
        public static KecaknoahReference Reference { get; } = KecaknoahReference.CreateRightReference(Instance);

        private KecaknoahNil()
        {

        }

        /// <summary>
        /// 特定の名前を持つメンバーに対してアクセスを試み、値を取得しようとしてもnilです。
        /// </summary>
        /// <param name="name">メンバー名</param>
        /// <returns>アクセスできる場合は対象のオブジェクト</returns>
        public override KecaknoahReference GetMemberReference(string name) => Reference;

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
        public override KecaknoahReference GetIndexerReference(KecaknoahObject[] indices) => Reference;

        /// <summary>
        /// このオブジェクトに対して二項式としての演算をしようとしてもnilです。
        /// </summary>
        /// <param name="op">演算子</param>
        /// <param name="target">2項目の<see cref="KecaknoahObject"/></param>
        /// <returns></returns>
        public override KecaknoahObject ExpressionOperation(KecaknoahILCodeType op, KecaknoahObject target) => Instance;

        /// <summary>
        /// 現在の以下略。
        /// </summary>
        /// <returns>知るか</returns>
        public override string ToString() => "nil";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone() => Instance;
    }
}
