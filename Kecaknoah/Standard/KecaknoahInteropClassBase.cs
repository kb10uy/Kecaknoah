using Kecaknoah.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah.Standard
{
    /// <summary>
    /// Kecaknoahと.NET連携の型の基底を提供します。
    /// 実際に作成する際はこのクラスをコピーするといいかもしれません。
    /// というかこの他にもオーバーロードできるメソッドはあるので適当にどうにかしてください。
    /// </summary>
    public sealed class KecaknoahInteropClassBase : KecaknoahObject
    {
        /// <summary>
        /// Kecaknoah上でのクラス名を取得します。
        /// </summary>
        public static readonly string ClassName = "InteropBase";

        #region 改変不要
        /// <summary>
        /// このクラスのクラスメソッドが定義される<see cref="KecaknoahInteropClassInfo"/>を取得します。
        /// こいつを適当なタイミングで<see cref="KecaknoahModule.RegisterClass(KecaknoahInteropClassInfo)"/>に
        /// 渡してください。
        /// </summary>
        internal static KecaknoahInteropClassInfo Information { get; } = new KecaknoahInteropClassInfo(ClassName);
        #endregion

        #region overrideメンバー
        /// <summary>
        /// 主にInformationを初期化します。
        /// コンストラクタを含む全てのクラスメソッドはここから追加してください。
        /// 逆に登録しなければコンストラクタを隠蔽できるということでもありますが。
        /// </summary>
        static KecaknoahInteropClassBase()
        {
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("new", ClassNew));
        }

        /// <summary>
        /// このクラスのインスタンスを初期化します。
        /// 要するにこのインスタンスがスクリプト中で参照されるので、
        /// インスタンスメソッドやプロパティの設定を忘れずにしてください。
        /// あと<see cref="KecaknoahObject.ExtraType"/>に型名をセットしておくと便利です。
        /// </summary>
        public KecaknoahInteropClassBase()
        {
            ExtraType = ClassName;
            RegisterInstanceFunction();
        }

        /// <summary>
        /// 指定された名前のメンバーへの参照を取得します。
        /// 「参照」というのは右辺値・左辺値どちらにもなりうる<see cref="KecaknoahReference"/>を差し、
        /// インスタンスごとに1つずつ(呼ばれる毎にnewしない)である必要があります。
        /// ここで返されるべき参照は
        /// ・インスタンスメソッド
        /// ・プロパティ
        /// などです。どちらもフィールドに<see cref="KecaknoahReference"/>のインスタンスを確保して
        /// switch分岐でそれらを返すことが推奨されます。
        /// </summary>
        /// <param name="name">メンバー名</param>
        /// <returns></returns>
        protected internal override KecaknoahReference GetMemberReference(string name)
        {
            return base.GetMemberReference(name);
        }

        /// <summary>
        /// この<see cref="KecaknoahObject"/>を「呼び出し」ます。
        /// このインスタンスそのものがメソッドのオブジェクトであるかのように振る舞います。
        /// あまり乱用するべきではありません。
        /// </summary>
        /// <param name="context">呼び出される時の<see cref="KecaknoahContext"/></param>
        /// <param name="args">引数</param>
        /// <returns>
        /// 返り値。基本的にはresumeできないと思うので適当な<see cref="KecaknoahObject"/>に<see cref="TypeExtensions.NoResume(KecaknoahObject)"/>してください。
        /// 参考にしてください。
        /// </returns>
        protected internal override KecaknoahFunctionResult Call(KecaknoahContext context, KecaknoahObject[] args)
        {
            return base.Call(context, args);
        }

        /// <summary>
        /// インデクサーの参照を得ます。
        /// <see cref="GetMemberReference(string)"/>と<see cref="KecaknoahObject.Call(KecaknoahContext, KecaknoahObject[])"/>の
        /// 中間のような存在です。
        /// </summary>
        /// <param name="indices">インデックス</param>
        /// <returns>返す参照</returns>
        protected internal override KecaknoahReference GetIndexerReference(KecaknoahObject[] indices)
        {
            return base.GetIndexerReference(indices);
        }

        /// <summary>
        /// 二項演算をします。
        /// 実際には単項演算子でも適用されるため、-(<see cref="KecaknoahILCodeType.Negative"/>)と
        /// !(<see cref="KecaknoahILCodeType.Not"/>)にも対応出来ます。
        /// <see cref="KecaknoahILCodeType"/>内にはその他の演算も含まれていますが、
        /// 複合演算子はILレベルで処理されるので対応する意味はありません。
        /// ちなみに<see cref="KecaknoahObject"/>との演算方法はどのように実装しても構いません。
        /// Kecaknoah内部では<see cref="KecaknoahObject.Type"/>の比較と内部のValueプロパティによる比較となっています。
        /// thisで比較すると99%falseになってしまうので注意してください。
        /// </summary>
        /// <param name="op">演算子</param>
        /// <param name="target">対象のインスタンス</param>
        /// <returns></returns>
        protected internal override KecaknoahObject ExpressionOperation(KecaknoahILCodeType op, KecaknoahObject target)
        {
            return base.ExpressionOperation(op, target);
        }

        /// <summary>
        /// 値渡しの際に渡すオブジェクトを生成します。
        /// 値型の場合は必ずこれをオーバーライドしてください。それ以外の場合は原則的に挙動が参照型になります。
        /// </summary>
        /// <returns>クローン</returns>
        public override KecaknoahObject AsByValValue() => base.AsByValValue();

        /// <summary>
        /// 値的に等価であるか比較します。
        /// Dictionaryなどで利用されるのでできるだけオーバーライドしましょう。
        /// ==演算子の方はその機能をExpressionOperationで担うのでオーバーライドする必要はありません。
        /// </summary>
        /// <param name="obj">
        /// 比較対象。
        /// デフォルト実装のようにasキャストしてnull比較と内部値比較を同時に行うのが早いかと思われます。
        /// 重ね重ね言いますが==はオーバーライドする必要はありません。むしろしないでください。
        /// </param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var t = obj as KecaknoahInteropClassBase;
            return t != null && t.Value == Value;
        }

        /// <summary>
        /// この<see cref="KecaknoahObject"/>のインスタンスを表す値を返します。
        /// 同じくDictionaryなどで利用されるので同じ値を表す<see cref="KecaknoahObject"/>が
        /// 同じ値を表すように振る舞いましょう。
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => Value.GetHashCode();
        #endregion


        #region インスタンスメソッド
        //nameof使おうな
        KecaknoahReference instance_method;

        private void RegisterInstanceFunction()
        {

        }
        #endregion

        #region クラスメソッド
        /* 
        当たり前ですがクラスメソッド呼び出しではselfはnullになります。
        selfに代入するのではなく生成したのをNoResumeで返却します。
        */

        private static KecaknoahFunctionResult ClassNew(KecaknoahContext ctx,KecaknoahObject self,KecaknoahObject[] args)
        {
            return new KecaknoahInteropClassBase().NoResume();
        }
        #endregion
    }
}
