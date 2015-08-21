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
    public class KecaknoahObject : ICloneable
    {
        /// <summary>
        /// 実際の値を取得・設定します。
        /// </summary>
        public dynamic Value { get; set; }

        /// <summary>
        /// このオブジェクトの型を取得・設定します。
        /// </summary>
        public TypeCode Type { get; set; }

        /// <summary>
        /// 特定の名前を持つメンバーに対してアクセスを試み、値を取得します。
        /// </summary>
        /// <param name="name">メンバー名</param>
        /// <returns>アクセスできる場合は対象のオブジェクト</returns>
        public virtual KecaknoahReference GetMemberReference(string name) => null;

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
        public virtual KecaknoahReference GetIndexerReference(KecaknoahObject[] indices)
        {
            throw new InvalidOperationException($"この{nameof(KecaknoahObject)}に対してインデクサー呼び出しは出来ません。");
        }

        /// <summary>
        /// このオブジェクトに対して二項式としての演算をします。
        /// </summary>
        /// <param name="op">演算子</param>
        /// <param name="target">2項目の<see cref="KecaknoahObject"/></param>
        /// <returns></returns>
        public virtual KecaknoahObject ExpressionOperation(KecaknoahILCodeType op, KecaknoahObject target)
        {
            throw new InvalidOperationException($"この{nameof(KecaknoahObject)}に対して式操作は出来ません。");
        }

        /// <summary>
        /// 現在の以下略。
        /// </summary>
        /// <returns>知るか</returns>
        public override string ToString() => "KecaknoahObject";

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public KecaknoahObject()
        {
            Type = TypeCode.Empty;
        }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// <param name="st"></param>
        /// </summary>
        public KecaknoahObject(bool st)
        {
            Type = TypeCode.Empty;
        }

        /// <summary>
        /// 可能ならば<see cref="long"/>型に変換します。
        /// </summary>
        /// <returns></returns>
        public virtual long ToInt64()
        {
            throw new InvalidCastException();
        }

        /// <summary>
        /// 可能ならば<see cref="double"/>型に変換します。
        /// </summary>
        /// <returns></returns>
        public virtual double ToDouble()
        {
            throw new InvalidCastException();
        }

        /// <summary>
        /// 可能ならば<see cref="bool"/>型に変換します。
        /// </summary>
        /// <returns></returns>
        public virtual bool ToBoolean()
        {
            throw new InvalidCastException();
        }

#pragma warning disable 1591
        public override int GetHashCode() => Value.GetHashCode();
        public override bool Equals(object obj) => ReferenceEquals(this, obj);

        public virtual object Clone()
        {
            throw new NotImplementedException();
        }

        public static KecaknoahObject operator +(KecaknoahObject v1, KecaknoahObject v2) => KecaknoahNil.Instance;
        public static KecaknoahObject operator -(KecaknoahObject v1, KecaknoahObject v2) => KecaknoahNil.Instance;
        public static KecaknoahObject operator *(KecaknoahObject v1, KecaknoahObject v2) => KecaknoahNil.Instance;
        public static KecaknoahObject operator /(KecaknoahObject v1, KecaknoahObject v2) => KecaknoahNil.Instance;
        public static KecaknoahObject operator %(KecaknoahObject v1, KecaknoahObject v2) => KecaknoahNil.Instance;
        public static KecaknoahObject operator &(KecaknoahObject v1, KecaknoahObject v2) => KecaknoahNil.Instance;
        public static KecaknoahObject operator ^(KecaknoahObject v1, KecaknoahObject v2) => KecaknoahNil.Instance;
        public static KecaknoahObject operator |(KecaknoahObject v1, KecaknoahObject v2) => KecaknoahNil.Instance;
        public static KecaknoahObject operator <<(KecaknoahObject v1, int v2) => KecaknoahNil.Instance;
        public static KecaknoahObject operator >>(KecaknoahObject v1, int v2) => KecaknoahNil.Instance;
        public static KecaknoahObject operator ==(KecaknoahObject v1, KecaknoahObject v2) => KecaknoahNil.Instance;
        public static KecaknoahObject operator !=(KecaknoahObject v1, KecaknoahObject v2) => KecaknoahNil.Instance;
        public static KecaknoahObject operator <(KecaknoahObject v1, KecaknoahObject v2) => KecaknoahNil.Instance;
        public static KecaknoahObject operator >(KecaknoahObject v1, KecaknoahObject v2) => KecaknoahNil.Instance;
        public static KecaknoahObject operator <=(KecaknoahObject v1, KecaknoahObject v2) => KecaknoahNil.Instance;
        public static KecaknoahObject operator >=(KecaknoahObject v1, KecaknoahObject v2) => KecaknoahNil.Instance;
        public static bool operator true(KecaknoahObject v1) => false;
        public static bool operator false(KecaknoahObject v1) => false;


        public static explicit operator long (KecaknoahObject obj) => ((KecaknoahInteger)obj).Value;
        public static explicit operator double (KecaknoahObject obj) => ((KecaknoahFloat)obj).Value;
        public static explicit operator string (KecaknoahObject obj) => ((KecaknoahString)obj).Value;
        public static explicit operator bool (KecaknoahObject obj) => ((KecaknoahBoolean)obj).Value;
#pragma warning restore 1591

    }
}
