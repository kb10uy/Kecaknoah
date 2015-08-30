using Kecaknoah.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Kecaknoah.External;
using System.IO;

namespace Kecaknoah.Standard
{
#pragma warning disable 1591
    /// <summary>
    /// Kecaknoah向けのアセンブリを読み込み、クラスをやりとりするための
    /// 受け渡しを担います。
    /// </summary>
    public sealed class KecaknoahExtensionLibrary : KecaknoahObject, IPartImportsSatisfiedNotification
    {

        public static readonly string ClassName = "ExtensionLibrary";

        #region 改変不要
        /// <summary>
        /// このクラスのクラスメソッドが定義される<see cref="KecaknoahInteropClassInfo"/>を取得します。
        /// こいつを適当なタイミングで<see cref="KecaknoahModule.RegisterClass(KecaknoahInteropClassInfo)"/>に
        /// 渡してください。
        /// </summary>
        internal static KecaknoahInteropClassInfo Information { get; } = new KecaknoahInteropClassInfo(ClassName);
        #endregion
        [ImportMany]
        private List<ExternalInfoFetcher> exclasses = new List<ExternalInfoFetcher>();
        private List<KecaknoahExternalClassInfo> infos = new List<KecaknoahExternalClassInfo>();
        private Dictionary<string, KecaknoahReference> classReferences = new Dictionary<string, KecaknoahReference>();


        static KecaknoahExtensionLibrary()
        {
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("load_file", ClassLoadFile));
        }

        public KecaknoahExtensionLibrary()
        {
            ExtraType = ClassName;
        }

        protected internal override KecaknoahReference GetIndexerReference(KecaknoahObject[] indices)
        {
            var name = indices[0].ToString();
            if (!classReferences.ContainsKey(name)) return KecaknoahNil.Reference;
            return classReferences[name];
        }

        private static KecaknoahFunctionResult ClassLoadFile(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var name = Path.GetFullPath(args[0].ToString());
            var catalog = new DirectoryCatalog(Path.GetDirectoryName(name), Path.GetFileName(name));
            var result = new KecaknoahExtensionLibrary();
            var container = new CompositionContainer(catalog);
            container.ComposeParts(result);
            return result.NoResume();
        }

        public void OnImportsSatisfied()
        {
            infos.AddRange(exclasses.Select(p => p()));
            foreach (var i in infos) classReferences[i.ClassName] = KecaknoahReference.Right(new KecaknoahExtensionClass(i));
        }
    }

    /// <summary>
    /// Kecaknoah外部ライブラリのクラスを定義します。
    /// </summary>
    public sealed class KecaknoahExtensionClass : KecaknoahObject
    {

        public static readonly string ClassName = "ExtensionClass";
        private KecaknoahExternalClassInfo exclass;
        private KecaknoahReference i_create = KecaknoahNil.Reference;
        private KecaknoahInteropClassObject cobj;
        private KecaknoahReference cobjRef;
        private KecaknoahInteropMethodInfo ctor;
        private bool isStatic;

        internal KecaknoahExtensionClass(KecaknoahExternalClassInfo kec)
        {
            ExtraType = ClassName;
            exclass = kec;
            cobj = new KecaknoahInteropClassObject(exclass.Information);
            cobjRef = KecaknoahReference.Right(cobj);

            isStatic = exclass.IsStaticClass;
            if (!isStatic)
            {
                i_create = KecaknoahReference.Right(this, InstanceCreate);
                ctor = cobj.Class.classMethods.FirstOrDefault(p => p.Name == "new");
                if (ctor == null) throw new CompositionException("staticクラスでないにもかかわらずコンストラクタがありません。");
            }
        }

        protected internal override KecaknoahReference GetMemberReference(string name)
        {
            switch (name)
            {
                case "create": return i_create;
                case "class": return cobjRef;
            }
            return base.GetMemberReference(name);
        }

        private KecaknoahFunctionResult InstanceCreate(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            if (isStatic) throw new InvalidOperationException("静的クラスでコンストラクタが呼ばれています。");
            var result = ctor.Body(ctx, self, args);
            return result;
        }
    }
#pragma warning restore 1591
}
