using System;
using System.ComponentModel.Composition;
using Kecaknoah;
using Kecaknoah.External;
using Kecaknoah.Type;


namespace KecaknoahExtensionSample
{
    public class ExtensionClassSample : KecaknoahObject
    {
        #region クラス情報登録関係
        private static readonly string className = "Sample";
        private static readonly KecaknoahInteropClassInfo info = new KecaknoahInteropClassInfo(className);

        /// <summary>
        /// <see cref="KecaknoahExternalClassInfo"/>を取得します。
        /// このように、登録したいクラス自体にstaticメソッドとして<see cref="ExternalInfoFetcher"/>を
        /// 定義すると便利です。
        /// </summary>
        /// <returns></returns>
        [Export(typeof(ExternalInfoFetcher))]
        public static KecaknoahExternalClassInfo FetchExternalClassInfo() => new KecaknoahExternalClassInfo
        {
            ClassName = className,
            Information = info,
            IsStaticClass = false,
        };
        #endregion

        static ExtensionClassSample()
        {
            info.AddClassMethod(new KecaknoahInteropMethodInfo("new", ClassNew));
        }

        private static KecaknoahFunctionResult ClassNew(KecaknoahContext ctx, KecaknoahObject self,KecaknoahObject[] args)
        {
            Console.WriteLine("External Sample Class Constructor");
            return KecaknoahNil.Instance.NoResume();
        }
    }
}
