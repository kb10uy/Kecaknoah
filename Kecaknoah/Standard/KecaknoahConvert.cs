using Kecaknoah.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah.Standard
{
    /// <summary>
    /// 値変換を担います。
    /// </summary>
    public sealed class KecaknoahConvert : KecaknoahObject
    {
        /// <summary>
        /// Kecaknoah上でのクラス名を取得します。
        /// </summary>
        public static readonly string ClassName = "Convert";

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
        static KecaknoahConvert()
        {
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("to_int", ToInteger));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("to_float", ToFloat));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("to_bool", ToBoolean));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("to_str", ToString));
        }

        /// <summary>
        /// このクラスのインスタンスを初期化します。
        /// 要するにこのインスタンスがスクリプト中で参照されるので、
        /// インスタンスメソッドやプロパティの設定を忘れずにしてください。
        /// あと<see cref="KecaknoahObject.ExtraType"/>に型名をセットしておくと便利です。
        /// </summary>
        public KecaknoahConvert()
        {
            ExtraType = ClassName;
        }
        #endregion

        #region クラスメソッド
        /* 
        当たり前ですがクラスメソッド呼び出しではselfはnullになります。
        selfに代入するのではなく生成したのをNoResumeで返却します。
        */

        private static KecaknoahFunctionResult ToBoolean(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            switch (args[0].Type)
            {
                case TypeCode.Boolean:
                    return args[0].NoResume();
                case TypeCode.Int64:
                    return Convert.ToBoolean(args[0].ToInt64()).AsKecaknoahBoolean().NoResume();
                case TypeCode.Double:
                    return Convert.ToBoolean(args[0].ToDouble()).AsKecaknoahBoolean().NoResume();
                case TypeCode.String:
                    return Convert.ToBoolean(args[0].ToString()).AsKecaknoahBoolean().NoResume();
                default:
                    return false.AsKecaknoahBoolean().NoResume();
            }
        }

        private static KecaknoahFunctionResult ToInteger(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            switch (args[0].Type)
            {
                case TypeCode.Boolean:
                    return Convert.ToInt64(args[0].ToBoolean()).AsKecaknoahInteger().NoResume();
                case TypeCode.Int64:
                    return args[0].NoResume();
                case TypeCode.Double:
                    return Convert.ToInt64(args[0].ToDouble()).AsKecaknoahInteger().NoResume();
                case TypeCode.String:
                    return Convert.ToInt64(args[0].ToString()).AsKecaknoahInteger().NoResume();
                default:
                    return 0.AsKecaknoahInteger().NoResume();
            }
        }

        private static KecaknoahFunctionResult ToFloat(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            switch (args[0].Type)
            {
                case TypeCode.Boolean:
                    return Convert.ToDouble(args[0].ToBoolean()).AsKecaknoahFloat().NoResume();
                case TypeCode.Int64:
                    return Convert.ToDouble(args[0].ToInt64()).AsKecaknoahFloat().NoResume();
                case TypeCode.Double:
                    return args[0].NoResume();
                case TypeCode.String:
                    return Convert.ToDouble(args[0].ToString()).AsKecaknoahFloat().NoResume();
                default:
                    return 0.0.AsKecaknoahFloat().NoResume();
            }
        }

        private static KecaknoahFunctionResult ToString(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args) => args[0].ToString().AsKecaknoahString().NoResume();
        #endregion
    }
}

