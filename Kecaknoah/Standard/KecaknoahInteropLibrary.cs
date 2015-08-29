using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Kecaknoah.Type;
using System.IO;

namespace Kecaknoah.Standard
{
#pragma warning disable 1591
    /// <summary>
    /// リフレクションでどうにかします。
    /// </summary>
    public sealed class KecaknoahDynamicLibrary : KecaknoahObject
    {
        public static readonly string ClassName = "DynamicLibrary";
        internal static KecaknoahInteropClassInfo Information { get; } = new KecaknoahInteropClassInfo(ClassName);
        private Assembly assembly;

        static KecaknoahDynamicLibrary()
        {
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("load_file", ClassLoadFile));
        }

        private KecaknoahDynamicLibrary(Assembly asm)
        {
            ExtraType = ClassName;
            assembly = asm;
        }

        protected internal override KecaknoahReference GetIndexerReference(KecaknoahObject[] indices)
        {
            var tn = indices[0].ToString();
            var type = assembly.GetType(tn);
            return KecaknoahReference.Right(new KecaknoahDynamicType(type));
        }

        private static KecaknoahFunctionResult ClassLoadFile(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var fn = args[0].ToString();
            return new KecaknoahDynamicLibrary(Assembly.LoadFile(Path.GetFullPath(fn))).NoResume();
        }
    }

    /// <summary>
    /// リフレクションでどうにかしたやつのあれです。
    /// </summary>
    public sealed class KecaknoahDynamicType : KecaknoahObject
    {
        public static readonly string ClassName = "DynamicLibraryType";
        internal static KecaknoahInteropClassInfo Information { get; } = new KecaknoahInteropClassInfo(ClassName);
        private System.Type type;
        private KecaknoahReference name, create_instance;

        internal KecaknoahDynamicType(System.Type t)
        {
            ExtraType = ClassName;
            type = t;
            name = KecaknoahReference.Right(type.Name);
            create_instance = KecaknoahReference.Right(new KecaknoahInteropFunction(this, InstanceCreareInstance));
        }

        protected internal override KecaknoahReference GetMemberReference(string name)
        {
            if (name == "create") return create_instance;
            return base.GetMemberReference(name);
        }

        private KecaknoahFunctionResult InstanceCreareInstance(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            List<object> ia = new List<object>();
            foreach (var i in args)
            {
                switch (i.Type)
                {
                    case TypeCode.Boolean:
                        ia.Add(i.ToBoolean());
                        break;
                    case TypeCode.Double:
                        ia.Add(i.ToDouble());
                        break;
                    case TypeCode.Int64:
                        ia.Add(i.ToInt32());
                        break;
                    case TypeCode.String:
                        ia.Add(i.ToString());
                        break;
                    case TypeCode.Empty:
                        ia.Add(null);
                        break;
                    default:
                        var t = i as KecaknoahDynamicLibraryObject;
                        ia.Add(t != null ? t.rawobj : null);
                        break;
                }
            }
            var obj = Activator.CreateInstance(type, ia.ToArray());
            return new KecaknoahDynamicLibraryObject(obj).NoResume();
        }
    }

    /// <summary>
    /// リフレクションでどうにかしたやつのあれです。
    /// </summary>
    public sealed class KecaknoahDynamicLibraryObject : KecaknoahObject
    {
        public static readonly string ClassName = "DynamicLibraryObject";
        internal static KecaknoahInteropClassInfo Information { get; } = new KecaknoahInteropClassInfo(ClassName);
        private System.Type type;
        internal object rawobj;
        private KecaknoahReference name;
        private Dictionary<string, KecaknoahReference> methodCache = new Dictionary<string, KecaknoahReference>();
        private Dictionary<string, KecaknoahReference> fieldCache = new Dictionary<string, KecaknoahReference>();

        internal KecaknoahDynamicLibraryObject(object self)
        {
            ExtraType = ClassName;
            type = self?.GetType();
            name = KecaknoahReference.Right(type?.Name ?? "");
            rawobj = self;
        }

        protected internal override KecaknoahReference GetMemberReference(string name)
        {
            if (!methodCache.ContainsKey(name))
            {
                var mi = type.GetMethod(name);
                methodCache[name] = KecaknoahReference.Right(new KecaknoahDynamicLibraryFunction(rawobj, mi));
            }
            return methodCache[name];
        }

        protected internal override KecaknoahReference GetIndexerReference(KecaknoahObject[] indices)
        {
            var name = indices[0].ToString();
            if (!fieldCache.ContainsKey(name))
            {
                var mi = type.GetField(name);
                fieldCache[name] = KecaknoahReference.Right(new KecaknoahDynamicLibraryField(rawobj, mi));
            }
            return fieldCache[name];
        }
    }

    /// <summary>
    /// リフレクションでどうにかしたやつのあれのこれです。
    /// </summary>
    public sealed class KecaknoahDynamicLibraryFunction : KecaknoahObject
    {
        public static readonly string ClassName = "DynamicLibraryObjectFunction";
        private MethodInfo info;
        private object instance;
        private Dictionary<string, KecaknoahReference> methodCache = new Dictionary<string, KecaknoahReference>();

        internal KecaknoahDynamicLibraryFunction(object self, MethodInfo mi)
        {
            ExtraType = ClassName;
            instance = self;
            info = mi;
        }

        protected internal override KecaknoahFunctionResult Call(KecaknoahContext context, KecaknoahObject[] args)
        {
            List<object> ia = new List<object>();
            foreach (var i in args)
            {
                switch (i.Type)
                {
                    case TypeCode.Boolean:
                        ia.Add(i.ToBoolean());
                        break;
                    case TypeCode.Double:
                        ia.Add(i.ToDouble());
                        break;
                    case TypeCode.Int64:
                        ia.Add(i.ToInt32());
                        break;
                    case TypeCode.String:
                        ia.Add(i.ToString());
                        break;
                    case TypeCode.Empty:
                        ia.Add(null);
                        break;
                    default:
                        var t = i as KecaknoahDynamicLibraryObject;
                        ia.Add(t != null ? t.rawobj : null);
                        break;
                }
            }
            var result = info.Invoke(instance, ia.ToArray());
            return new KecaknoahDynamicLibraryObject(result).NoResume();
        }
    }

    /// <summary>
    /// リフレクションの友利奈緒を保登心愛して越谷夏海したやつです。
    /// </summary>
    public sealed class KecaknoahDynamicLibraryField : KecaknoahObject
    {
        public static readonly string ClassName = "DynamicLibraryObjectField";
        private FieldInfo info;
        internal object instance;
        private KecaknoahReference getter, setter;

        internal KecaknoahDynamicLibraryField(object self, FieldInfo fi)
        {
            ExtraType = ClassName;
            instance = self;
            info = fi;
            getter = KecaknoahReference.Right(new KecaknoahInteropFunction(this, InstanceGet));
            setter = KecaknoahReference.Right(new KecaknoahInteropFunction(this, InstanceSet));
        }

        protected internal override KecaknoahReference GetMemberReference(string name)
        {
            switch (name)
            {
                case "get": return getter;
                case "set": return setter;
            }
            return base.GetMemberReference(name);
        }

        private KecaknoahFunctionResult InstanceGet(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args) => new KecaknoahDynamicLibraryObject(info.GetValue(instance)).NoResume();

        private KecaknoahFunctionResult InstanceSet(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            object sv = null;
            switch (args[0].Type)
            {
                case TypeCode.Boolean:
                    sv = args[0].ToBoolean();
                    break;
                case TypeCode.Double:
                    sv = args[0].ToBoolean();
                    break;
                case TypeCode.Int64:
                    sv = args[0].ToInt64();
                    break;
                case TypeCode.String:
                    sv = args[0].ToString();
                    break;
                case TypeCode.Empty:
                    sv = null;
                    break;
                default:
                    break;
            }
            info.SetValue(instance, sv);
            return KecaknoahNil.Instance.NoResume();
        }
    }
#pragma warning restore 1591
}
