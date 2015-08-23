using Kecaknoah.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah.Standard
{
    /// <summary>
    /// Kecaknoahと.NET連携クラスの基底を提供します。
    /// </summary>
    public sealed class KecaknoahInteropClassBase
    {
        /// <summary>
        /// このクラスの構造を定義する<see cref="KecaknoahInteropClassInfo"/>を取得します。
        /// </summary>
        public static KecaknoahInteropClassInfo Information { get; } = new KecaknoahInteropClassInfo();

        /// <summary>
        /// Informationにメソッドなどを登録します。
        /// 適当なポイントで<see cref="KecaknoahModule.RegisterClass(KecaknoahInteropClassInfo)"/>に渡して登録します。
        /// </summary>
        static KecaknoahInteropClassBase()
        {
            /*
            ・インスタンスメソッド
              Information.AddInstanceMethodで登録します。
              ここで登録されたメソッドは必ずインスタンスが付与されて呼び出されます。
            */
            Information.AddInstanceMethod(new KecaknoahInteropMethodInfo("instance_method", InstanceMethod));

            Information.AddInstanceMethod(new KecaknoahInteropMethodInfo("new", Constructor));
        }

        /// <summary>
        /// インスタンスメソッドを定義します。
        /// </summary>
        /// <param name="context">
        /// メソッドが呼ばれた時の<see cref="KecaknoahContext"/>。
        /// <see cref="KecaknoahStackFrame"/>などの初期化に利用します。
        /// </param>
        /// <param name="self">
        /// インスタンス。
        /// 内部で<see cref="KecaknoahInteropClassInfo"/>から生成された<see cref="KecaknoahInstance"/>が渡されます。
        /// </param>
        /// <param name="args">
        /// 実行時引数。
        /// 数はわからないので注意してください。
        /// </param>
        /// <returns>
        /// 返り値。
        /// 純粋に<see cref="KecaknoahObject"/>を返すわけではないので注意してください。
        /// 特別な事情がなければ<see cref="TypeExtensions"/>クラスの拡張メソッド、
        /// AsKecaknoah系メソッドや<see cref="TypeExtensions.NoResume(KecaknoahObject)"/>を利用しましょう。
        /// voidを表現したい場合は<see cref="KecaknoahNil.Instance"/>を利用してください。
        /// </returns>
        internal static KecaknoahFunctionResult InstanceMethod(KecaknoahContext context, KecaknoahObject self, KecaknoahObject[] args)
        {
            if (args.Length < 2) return KecaknoahNil.Instance.NoResume();
            var v1 = args[0].ToInt64();
            var v2 = args[1].ToInt64();
            return (v1 + v2).AsKecaknoahInteger().NoResume();
        }

        /// <summary>
        /// クラスメソッドを定義します。
        /// コンストラクタも含まれ、その場合のみインスタンスが渡されます。
        /// </summary>
        /// <param name="context">同上</param>
        /// <param name="self">同上</param>
        /// <param name="args">同上</param>
        /// <returns>同上</returns>
        internal static KecaknoahFunctionResult Constructor(KecaknoahContext context, KecaknoahObject self, KecaknoahObject[] args)
        {
            if (args.Length < 2) return KecaknoahNil.Instance.NoResume();
            var v1 = args[0].ToInt64();
            var v2 = args[1].ToInt64();
            return (v1 + v2).AsKecaknoahInteger().NoResume();
        }

    }
}
