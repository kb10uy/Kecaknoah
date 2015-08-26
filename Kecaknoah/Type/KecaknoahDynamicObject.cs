using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah.Type
{
    /// <summary>
    /// Kecaknoah上でメンバーや動作を直接変更できるオブジェクトを定義します。
    /// </summary>
    public sealed class KecaknoahDynamicObject : KecaknoahObject
    {
        private Dictionary<string, KecaknoahReference> funcs = new Dictionary<string, KecaknoahReference>();
        /// <summary>
        /// 現在.NETから定義されているメソッドを取得します。
        /// </summary>
        public IReadOnlyDictionary<string, KecaknoahReference> FunctionReferences => funcs;

        private Dictionary<string, KecaknoahReference> props = new Dictionary<string, KecaknoahReference>();
        /// <summary>
        /// 現在.NET/Kecaknoah上から定義されているプロパティの参照を取得します。
        /// </summary>
        public IReadOnlyDictionary<string, KecaknoahReference> PropertyReferences => props;

        /// <summary>
        /// <see cref="Call(KecaknoahContext, KecaknoahObject[])"/>が呼ばれた場合に呼び出される
        /// デリゲートを取得・設定します。
        /// </summary>
        public Func<KecaknoahObject, KecaknoahObject[], KecaknoahObject> CallingFunction { get; set; }

        /// <summary>
        /// <see cref="GetIndexerReference(KecaknoahObject[])"/>が呼ばれた場合に呼び出される
        /// デリゲートを取得・設定します。
        /// </summary>
        public Func<KecaknoahObject, KecaknoahObject[], KecaknoahReference> IndexerFunction { get; set; }

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        public KecaknoahDynamicObject()
        {
            Type = TypeCode.Object;
            ExtraType = "DynamicObject";
        }

        #region Registerers
        /// <summary>
        /// メソッドを登録します。
        /// </summary>
        /// <param name="name">メソッド名</param>
        /// <param name="func">登録するメソッドのデリゲート</param>
        public void AddFunction(string name, KecaknoahInteropDelegate func)
        {
            funcs[name] = KecaknoahReference.CreateRightReference(new KecaknoahInteropFunction(this, func));
        }

        /// <summary>
        /// メソッドを登録します。
        /// </summary>
        /// <param name="name">メソッド名</param>
        /// <param name="func">
        /// 登録するメソッドのデリゲート。
        /// インスタンスのみが渡されます。
        /// </param>
        public void AddFunction(string name, Action<KecaknoahObject> func)
        {
            KecaknoahInteropDelegate wp =
                (c, s, a) =>
                {
                    func(s);
                    return KecaknoahNil.Instance.NoResume();
                };
            funcs[name] = KecaknoahReference.CreateRightReference(new KecaknoahInteropFunction(this, wp));
        }

        /// <summary>
        /// メソッドを登録します。
        /// </summary>
        /// <param name="name">メソッド名</param>
        /// <param name="func">
        /// 登録するメソッドのデリゲート。
        /// インスタンスと引数配列が渡されます。
        /// </param>
        public void AddFunction(string name, Action<KecaknoahObject, KecaknoahObject[]> func)
        {
            KecaknoahInteropDelegate wp =
                (c, s, a) =>
                {
                    func(s, a);
                    return KecaknoahNil.Instance.NoResume();
                };
            funcs[name] = KecaknoahReference.CreateRightReference(new KecaknoahInteropFunction(this, wp));
        }

        /// <summary>
        /// メソッドを登録します。
        /// </summary>
        /// <param name="name">メソッド名</param>
        /// <param name="func">
        /// 登録するメソッドのデリゲート。
        /// インスタンスのみが渡されます。
        /// </param>
        public void AddFunction(string name, Func<KecaknoahObject, KecaknoahObject> func)
        {
            KecaknoahInteropDelegate wp = (c, s, a) => func(s).NoResume();
            funcs[name] = KecaknoahReference.CreateRightReference(new KecaknoahInteropFunction(this, wp));
        }

        /// <summary>
        /// メソッドを登録します。
        /// </summary>
        /// <param name="name">メソッド名</param>
        /// <param name="func">
        /// 登録するメソッドのデリゲート。
        /// インスタンスと引数が渡されます。
        /// </param>
        public void AddFunction(string name, Func<KecaknoahObject, KecaknoahObject[], KecaknoahObject> func)
        {
            KecaknoahInteropDelegate wp = (c, s, a) => func(s, a).NoResume();
            funcs[name] = KecaknoahReference.CreateRightReference(new KecaknoahInteropFunction(this, wp));
        }

        /// <summary>
        /// メソッドを登録します。
        /// </summary>
        /// <param name="name">メソッド名</param>
        /// <param name="func">
        /// 登録するメソッドのデリゲート。
        /// インスタンスと引数が渡されます。
        /// </param>
        public void AddInt32Function(string name, Func<KecaknoahObject, KecaknoahObject[], int> func)
        {
            KecaknoahInteropDelegate wp = (c, s, a) => func(s, a).AsKecaknoahInteger().NoResume();
            funcs[name] = KecaknoahReference.CreateRightReference(new KecaknoahInteropFunction(this, wp));
        }

        /// <summary>
        /// メソッドを登録します。
        /// </summary>
        /// <param name="name">メソッド名</param>
        /// <param name="func">
        /// 登録するメソッドのデリゲート。
        /// インスタンスと引数が渡されます。
        /// </param>
        public void AddInt64Function(string name, Func<KecaknoahObject, KecaknoahObject[], long> func)
        {
            KecaknoahInteropDelegate wp = (c, s, a) => func(s, a).AsKecaknoahInteger().NoResume();
            funcs[name] = KecaknoahReference.CreateRightReference(new KecaknoahInteropFunction(this, wp));
        }

        /// <summary>
        /// メソッドを登録します。
        /// </summary>
        /// <param name="name">メソッド名</param>
        /// <param name="func">
        /// 登録するメソッドのデリゲート。
        /// インスタンスと引数が渡されます。
        /// </param>
        public void AddSingleFunction(string name, Func<KecaknoahObject, KecaknoahObject[], float> func)
        {
            KecaknoahInteropDelegate wp = (c, s, a) => ((double)func(s, a)).AsKecaknoahFloat().NoResume();
            funcs[name] = KecaknoahReference.CreateRightReference(new KecaknoahInteropFunction(this, wp));
        }

        /// <summary>
        /// メソッドを登録します。
        /// </summary>
        /// <param name="name">メソッド名</param>
        /// <param name="func">
        /// 登録するメソッドのデリゲート。
        /// インスタンスと引数が渡されます。
        /// </param>
        public void AddDoubleFunction(string name, Func<KecaknoahObject, KecaknoahObject[], double> func)
        {
            KecaknoahInteropDelegate wp = (c, s, a) => func(s, a).AsKecaknoahFloat().NoResume();
            funcs[name] = KecaknoahReference.CreateRightReference(new KecaknoahInteropFunction(this, wp));
        }

        /// <summary>
        /// メソッドを登録します。
        /// </summary>
        /// <param name="name">メソッド名</param>
        /// <param name="func">
        /// 登録するメソッドのデリゲート。
        /// インスタンスと引数が渡されます。
        /// </param>
        public void AddBooleanFunction(string name, Func<KecaknoahObject, KecaknoahObject[], bool> func)
        {
            KecaknoahInteropDelegate wp = (c, s, a) => func(s, a).AsKecaknoahBoolean().NoResume();
            funcs[name] = KecaknoahReference.CreateRightReference(new KecaknoahInteropFunction(this, wp));
        }

        /// <summary>
        /// メソッドを登録します。
        /// </summary>
        /// <param name="name">メソッド名</param>
        /// <param name="func">
        /// 登録するメソッドのデリゲート。
        /// インスタンスと引数が渡されます。
        /// </param>
        public void AddStringFunction(string name, Func<KecaknoahObject, KecaknoahObject[], string> func)
        {
            KecaknoahInteropDelegate wp = (c, s, a) => func(s, a).AsKecaknoahString().NoResume();
            funcs[name] = KecaknoahReference.CreateRightReference(new KecaknoahInteropFunction(this, wp));
        }
        #endregion

        #region Overrides
#pragma warning disable 1591
        protected internal override KecaknoahReference GetMemberReference(string name)
        {
            KecaknoahReference result;
            if (PropertyReferences.TryGetValue(name, out result)) return result;
            if (FunctionReferences.TryGetValue(name, out result)) return result;
            result = new KecaknoahReference { IsLeftValue = true };
            props[name] = result;
            return result;
        }

        protected internal override KecaknoahFunctionResult Call(KecaknoahContext context, KecaknoahObject[] args)
        {
            if (CallingFunction == null) return KecaknoahNil.Instance.NoResume();
            return CallingFunction(this, args).NoResume();
        }

        protected internal override KecaknoahReference GetIndexerReference(KecaknoahObject[] indices)
        {
            if (IndexerFunction == null) return KecaknoahNil.Reference;
            return IndexerFunction(this, indices);
        }

        protected internal override KecaknoahObject ExpressionOperation(KecaknoahILCodeType op, KecaknoahObject target)
        {
            if (target.ExtraType == "DynamicObject")
            {
                var t = target as KecaknoahDynamicObject;
                switch (op)
                {
                    case KecaknoahILCodeType.Equal: return (t == target).AsKecaknoahBoolean();
                    case KecaknoahILCodeType.NotEqual: return (t != target).AsKecaknoahBoolean();
                    default: return base.ExpressionOperation(op, target);
                }
            }
            else
            {
                switch (op)
                {
                    case KecaknoahILCodeType.Equal: return KecaknoahBoolean.False;
                    case KecaknoahILCodeType.NotEqual: return KecaknoahBoolean.True;
                    default: return base.ExpressionOperation(op, target);
                }
            }
        }

        public override bool Equals(object obj)
        {
            var t = obj as KecaknoahDynamicObject;
            if (t == null) return false;
            return funcs.Equals(t.funcs) && props.Equals(t.props);
        }
        public override int GetHashCode() => unchecked(funcs.GetHashCode() * props.GetHashCode());

#pragma warning restore 1591
        #endregion
    }
}
