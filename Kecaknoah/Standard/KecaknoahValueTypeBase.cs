using Kecaknoah.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah.Standard
{
    /// <summary>
    /// Kecaknoahと.NET連携の値型の基底を提供します。
    /// 実際に作成する際はこのクラスを継承してください。
    /// というかこの他にもオーバーロードできるメソッドはあるので適当にどうにかしてください。
    /// </summary>
    public class KecaknoahValueTypeBase : KecaknoahObject
    {
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
        /// 返り値。<see cref="KecaknoahInteropClassBase.InstanceMethod(KecaknoahContext, KecaknoahObject, KecaknoahObject[])"/>も
        /// 参考にしてください。
        /// </returns>
        protected internal override KecaknoahFunctionResult Call(KecaknoahContext context, KecaknoahObject[] args)
        {
            return base.Call(context, args);
        }

        /// <summary>
        /// インデクサーの参照を得ます。
        /// <see cref="GetMemberReference(string)"/>と<see cref="Call(KecaknoahContext, KecaknoahObject[])"/>の
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
        /// Kecaknoah内部ではdynamicを利用した動的ディスパッチと演算子オーバーロードで実現しています。
        /// </summary>
        /// <param name="op">演算子</param>
        /// <param name="target">対象のインスタンス</param>
        /// <returns></returns>
        protected internal override KecaknoahObject ExpressionOperation(KecaknoahILCodeType op, KecaknoahObject target)
        {
            return base.ExpressionOperation(op, target);
        }
    }
}
