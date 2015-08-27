using System;

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
        public TypeCode Type { get; protected internal set; }

        /// <summary>
        /// 追加の型情報を取得します。
        /// </summary>
        public string ExtraType { get; protected internal set; } = "";

        /// <summary>
        /// 特定の名前を持つメンバーに対してアクセスを試み、参照を取得します。
        /// </summary>
        /// <param name="name">メンバー名</param>
        /// <returns>アクセスできる場合は対象のオブジェクト</returns>
        protected internal virtual KecaknoahReference GetMemberReference(string name)
        {
            switch (name)
            {
                case "to_str":
                    return InstanceToString(this);
                case "hash":
                    return InstanceHash(this);
                default:
                    return KecaknoahNil.Reference;
            }
        }

        /// <summary>
        /// <see cref="GetMemberReference(string)"/>の簡易版。
        /// <see cref="GetIndexerReference(KecaknoahObject[])"/>は参照しないので注意してください。
        /// </summary>
        /// <param name="name">メンバー名</param>
        /// <returns>アクセスできる場合は対象のオブジェクト</returns>
        public KecaknoahObject this[string name]
        {
            get { return GetMemberReference(name).RawObject; }
            set { GetMemberReference(name).RawObject = value; }
        }

        /// <summary>
        /// このオブジェクトに対してメソッドとしての呼び出しをします。
        /// </summary>
        /// <param name="context">実行される<see cref="KecaknoahContext"/></param>
        /// <param name="args">引数</param>
        /// <returns>返り値</returns>
        protected internal virtual KecaknoahFunctionResult Call(KecaknoahContext context, KecaknoahObject[] args)
        {
            throw new InvalidOperationException($"この{nameof(KecaknoahObject)}に対してメソッド呼び出しは出来ません。");
        }

        /// <summary>
        /// このオブジェクトに対してインデクサーアクセスを試みます。 
        /// </summary>
        /// <param name="indices">インデックス引数</param>
        /// <returns></returns>
        protected internal virtual KecaknoahReference GetIndexerReference(KecaknoahObject[] indices)
        {
            throw new InvalidOperationException($"この{nameof(KecaknoahObject)}に対してインデクサー呼び出しは出来ません。");
        }

        /// <summary>
        /// このオブジェクトに対して二項式としての演算をします。
        /// </summary>
        /// <param name="op">演算子</param>
        /// <param name="target">2項目の<see cref="KecaknoahObject"/></param>
        /// <returns></returns>
        protected internal virtual KecaknoahObject ExpressionOperation(KecaknoahILCodeType op, KecaknoahObject target)
        {
            throw new InvalidOperationException($"この{nameof(KecaknoahObject)}に対して式操作は出来ません。");
        }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public KecaknoahObject()
        {
            Type = TypeCode.Object;
            ExtraType = "Array";
        }

        private static KecaknoahReference InstanceToString(KecaknoahObject self) => KecaknoahReference.Right(self, (ctx, s, args) => s.ToString().AsKecaknoahString().NoResume());

        private static KecaknoahReference InstanceHash(KecaknoahObject self) => KecaknoahReference.Right(self, (ctx, s, args) => s.GetHashCode().AsKecaknoahInteger().NoResume());

        /// <summary>
        /// 現在の以下略。
        /// </summary>
        /// <returns>知るか</returns>
        public override string ToString() => "KecaknoahObject";

        /// <summary>
        /// 可能ならば<see cref="int"/>型に変換します。
        /// </summary>
        /// <returns></returns>
        public int ToInt32() => (int)ToInt64();

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

        /// <summary>
        /// 値渡しの時に渡されるオブジェクトを生成します。
        /// 値型の場合はクローンが、参照型の場合には自分自身が帰ります。
        /// </summary>
        /// <returns></returns>
        public virtual KecaknoahObject AsByValValue() => this;

#pragma warning disable 1591
        public virtual object Clone() => this;

        public override bool Equals(object obj) => ReferenceEquals(this, obj);
        public override int GetHashCode() => base.GetHashCode();
#pragma warning restore 1591

    }

    /// <summary>
    /// Kecaknoahのメソッドの実行結果を定義します。
    /// </summary>
    public sealed class KecaknoahFunctionResult
    {
        /// <summary>
        /// このメソッドを再開できるかどうかを取得します。
        /// </summary>
        public bool CanResume { get; }
        /// <summary>
        /// 今回のreturn/yieldに属する<see cref="KecaknoahObject"/>を取得します。
        /// </summary>
        public KecaknoahObject ReturningObject { get; }

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="obj">返却する<see cref="KecaknoahObject"/></param>
        /// <param name="res">再開可能な場合はtrue</param>
        public KecaknoahFunctionResult(KecaknoahObject obj, bool res)
        {
            CanResume = res;
            ReturningObject = obj;
        }

    }
}
