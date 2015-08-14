using Kecaknoah.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah
{
    /// <summary>
    /// Kecaknoahを実行するためのスタックフレームなどのセットを提供します。
    /// </summary>
    public sealed class KecaknoahContext : IDisposable
    {
        /// <summary>
        /// 属する<see cref="KecaknoahModule"/>を取得します。
        /// </summary>
        public KecaknoahModule Module { get; }

        private Stack<KecaknoahReference> objstack = new Stack<KecaknoahReference>();
        private Dictionary<string, KecaknoahObject> objects = new Dictionary<string, KecaknoahObject>();
        /// <summary>
        /// このコンテキストで定義されている変数を取得します。
        /// </summary>
        public IReadOnlyDictionary<string, KecaknoahObject> Objects { get; }

        internal KecaknoahContext(KecaknoahModule module)
        {
            Module = module;
            Objects = objects;
        }

        /// <summary>
        /// 指定した<see cref="KecaknoahMethod"/>を呼び出します。
        /// </summary>
        /// <param name="method">メソッド</param>
        /// <param name="args">引数</param>
        /// <returns>返り値</returns>
        public KecaknoahObject ExecuteMethod(KecaknoahMethod method, params KecaknoahObject[] args)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 指定した<see cref="KecaknoahIL"/>を式として実行し、結果を返します。
        /// </summary>
        /// <param name="il">式のコード</param>
        /// <returns>結果</returns>
        public KecaknoahObject ExecuteExpression(KecaknoahIL il)
        {
            objstack.Clear();
            ExecuteIL(il);
            if (objstack.Count != 1) throw new InvalidOperationException("最終スタックの数が不正です");
            return objstack.Pop().AsRightValue();
        }

        private void ExecuteIL(KecaknoahIL il, params KecaknoahObject[] ilargs)
        {
            var codes = il.Codes;
            int pc = 0;
            KecaknoahObject v1, v2;
            Stack<KecaknoahObject> args;
            while (pc < codes.Count)
            {
                var c = codes[pc];
                switch (c.Type)
                {
                    case KecaknoahILCodeType.Nop:
                        break;
                    case KecaknoahILCodeType.Label:
                        break;
                    case KecaknoahILCodeType.PushInteger:
                        objstack.Push(c.IntegerValue.AsKecaknoahInteger().AsRightValue());
                        break;
                    case KecaknoahILCodeType.PushString:
                        objstack.Push(c.StringValue.AsKecaknoahString().AsRightValue());
                        break;
                    case KecaknoahILCodeType.PushSingle:
                        objstack.Push(((float)c.FloatValue).AsKecaknoahSingle().AsRightValue());
                        break;
                    case KecaknoahILCodeType.PushDouble:
                        objstack.Push(c.FloatValue.AsKecaknoahDouble().AsRightValue());
                        break;
                    case KecaknoahILCodeType.PushBoolean:
                        objstack.Push(c.BooleanValue.AsKecaknoahBoolean().AsRightValue());
                        break;
                    case KecaknoahILCodeType.PushNil:
                        objstack.Push(KecaknoahNil.Instance.AsRightValue());
                        break;
                    case KecaknoahILCodeType.Pop:
                        objstack.Pop();
                        break;
                    case KecaknoahILCodeType.Plus:
                    case KecaknoahILCodeType.Minus:
                    case KecaknoahILCodeType.Multiply:
                    case KecaknoahILCodeType.Divide:
                    case KecaknoahILCodeType.Modular:
                    case KecaknoahILCodeType.And:
                    case KecaknoahILCodeType.Or:
                    case KecaknoahILCodeType.Xor:
                    case KecaknoahILCodeType.AndAlso:
                    case KecaknoahILCodeType.OrElse:
                    case KecaknoahILCodeType.LeftBitShift:
                    case KecaknoahILCodeType.RightBitShift:
                    case KecaknoahILCodeType.Equal:
                    case KecaknoahILCodeType.NotEqual:
                    case KecaknoahILCodeType.Greater:
                    case KecaknoahILCodeType.Lesser:
                    case KecaknoahILCodeType.GreaterEqual:
                    case KecaknoahILCodeType.LesserEqual:
                        v2 = objstack.Pop().AsRightValue();
                        v1 = objstack.Pop().AsRightValue();
                        objstack.Push(v1.ExpressionOperation(c.Type, v2).AsRightValue());
                        break;
                    case KecaknoahILCodeType.Not:
                    case KecaknoahILCodeType.Negative:
                        v1 = objstack.Pop().AsRightValue();
                        objstack.Push(v1.ExpressionOperation(c.Type, null).AsRightValue());
                        break;
                    case KecaknoahILCodeType.Assign:
                        var val = objstack.Pop().AsRightValue();
                        var aref = objstack.Pop();
                        if (!aref.IsLeftValue)
                        {
                            //ムチャ言わないで
                            objstack.Push(KecaknoahNil.Instance.AsRightValue());
                            break;
                        }
                        else if (aref.LeftAccessName.StartsWith("@@"))
                        {
                            objects[aref.LeftAccessName.Substring(2)] = val;
                        }
                        else if (aref.LeftAccessName.StartsWith("$$"))
                        {
                            Module.globalObjects[aref.LeftAccessName.Substring(2)] = val;
                        }
                        else
                        {
                            aref.Object.SetMember(aref.LeftAccessName, val);
                        }
                        break;
                    case KecaknoahILCodeType.PlusAssign:
                    case KecaknoahILCodeType.MinusAssign:
                    case KecaknoahILCodeType.MultiplyAssign:
                    case KecaknoahILCodeType.DivideAssign:
                    case KecaknoahILCodeType.AndAssign:
                    case KecaknoahILCodeType.OrAssign:
                    case KecaknoahILCodeType.XorAssign:
                    case KecaknoahILCodeType.ModularAssign:
                    case KecaknoahILCodeType.LeftBitShiftAssign:
                    case KecaknoahILCodeType.RightBitShiftAssign:
                    case KecaknoahILCodeType.NilAssign:
                        throw new NotImplementedException("Assign未対応");
                    case KecaknoahILCodeType.Jump:
                        pc = (int)c.IntegerValue;
                        continue;
                    case KecaknoahILCodeType.Return:
                        return;
                    case KecaknoahILCodeType.Call:
                        args = new Stack<KecaknoahObject>();
                        for (int i = 0; i < c.IntegerValue; i++) args.Push(objstack.Pop().AsRightValue());
                        v1 = objstack.Pop().AsRightValue();
                        objstack.Push(v1.Call(args.ToArray()).AsRightValue());
                        break;
                    case KecaknoahILCodeType.IndexerCall:
                        throw new NotImplementedException("未対応");
                    //args = new Stack<KecaknoahObject>();
                    //for (int i = 0; i < c.IntegerValue + 1; i++) args.Push(objstack.Pop().Object);
                    //v1 = objstack.Pop().Object;
                    //args.Push(v1.GetIndexer(args.ToArray()));
                    //break;
                    case KecaknoahILCodeType.PushArgument:
                        objstack.Push(ilargs[(int)c.IntegerValue].AsRightValue());
                        break;
                    case KecaknoahILCodeType.LoadObject:
                        //名前解決順
                        //TODO: オーバーロード解決
                        //Context変数
                        //Contextメソッド
                        //Module変数
                        //Moduleメソッド
                        KecaknoahObject target;
                        string refname = c.StringValue;
                        //なんつうコードだ
                        if (!Objects.TryGetValue(c.StringValue, out target))
                        {
                            if (!Module.GlobalObjects.TryGetValue(c.StringValue, out target))
                            {
                                target = KecaknoahNil.Instance;
                            }
                            else
                            {
                                refname = $"$${c.StringValue}";
                            }
                        }
                        else
                        {
                            refname = $"@@{c.StringValue}";
                        }
                        objstack.Push(new KecaknoahReference { Object = target, IsLeftValue = true, LeftAccessName = refname });
                        break;
                    case KecaknoahILCodeType.LoadMember:
                        var or = objstack.Pop();
                        if (or.IsLeftValue)
                        {
                            if (or.LeftAccessName.StartsWith("@@") || or.LeftAccessName.StartsWith("$$"))
                            {
                                var newref = new KecaknoahReference
                                {
                                    IsLeftValue = true,
                                    Object = or.Object.GetMember(c.StringValue),
                                    LeftAccessName = c.StringValue
                                };
                                objstack.Push(newref);
                            }
                            else
                            {
                                var newref = new KecaknoahReference
                                {
                                    IsLeftValue = true,
                                    Object = or.Object.GetMember(or.LeftAccessName),
                                    LeftAccessName = c.StringValue
                                };
                                objstack.Push(newref);
                            }
                        }
                        else
                        {
                            var newref = new KecaknoahReference
                            {
                                IsLeftValue = false,
                                Object = or.Object.GetMember(c.StringValue),
                            };
                            objstack.Push(newref);
                        }
                        break;
                }
                pc++;
            }
        }

        /// <summary>
        /// オブジェクトを開放します。
        /// </summary>
        public void Dispose()
        {
            objstack.Clear();
            objects.Clear();
        }
    }

    /// <summary>
    /// <see cref="KecaknoahObject"/>と参照を保持します。
    /// </summary>
    public sealed class KecaknoahReference
    {
        /// <summary>
        /// 対象の<see cref="KecaknoahObject"/>を取得します。
        /// </summary>
        public KecaknoahObject Object { get; internal set; }

        /// <summary>
        /// 左辺値になりうる場合はtrue。
        /// </summary>
        public bool IsLeftValue { get; internal set; }

        /// <summary>
        /// 左辺値の場合のアクセス名を取得します。
        /// </summary>
        public string LeftAccessName { get; internal set; }

        /// <summary>
        /// 右辺値化します。
        /// </summary>
        public KecaknoahObject AsRightValue() => (IsLeftValue == false || LeftAccessName.StartsWith("@@") || LeftAccessName.StartsWith("$$")) ? Object : Object.GetMember(LeftAccessName);
    }


}
