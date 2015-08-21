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
        private Dictionary<string, KecaknoahReference> objects = new Dictionary<string, KecaknoahReference>();
        /// <summary>
        /// このコンテキストで定義されている変数を取得します。
        /// </summary>
        public IReadOnlyDictionary<string, KecaknoahReference> Objects { get; }

        internal KecaknoahContext(KecaknoahModule module)
        {
            Module = module;
            Objects = objects;
        }

        /// <summary>
        /// 指定した<see cref="KecaknoahMethodInfo"/>を呼び出します。
        /// </summary>
        /// <param name="method">メソッド</param>
        /// <param name="args">引数</param>
        /// <returns>返り値</returns>
        public KecaknoahObject ExecuteMethod(KecaknoahMethodInfo method, params KecaknoahObject[] args)
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
            return objstack.Pop().RawObject;
        }

        private void ExecuteIL(KecaknoahIL il, params KecaknoahObject[] ilargs)
        {
            var codes = il.Codes;
            int pc = 0;
            KecaknoahObject v1, v2;
            KecaknoahReference rfr;
            Stack<KecaknoahObject> args;
            while (pc < codes.Count)
            {
                var c = codes[pc];
                switch (c.Type)
                {
                    //基本--------------------------------------------------------------------
                    case KecaknoahILCodeType.Nop:
                        break;
                    case KecaknoahILCodeType.Label:
                        break;
                    case KecaknoahILCodeType.PushInteger:
                        objstack.Push(KecaknoahReference.CreateRightReference(c.IntegerValue));
                        break;
                    case KecaknoahILCodeType.PushString:
                        objstack.Push(KecaknoahReference.CreateRightReference(c.StringValue));
                        break;
                    case KecaknoahILCodeType.PushDouble:
                        objstack.Push(KecaknoahReference.CreateRightReference(c.FloatValue));
                        break;
                    case KecaknoahILCodeType.PushBoolean:
                        objstack.Push(KecaknoahReference.CreateRightReference(c.BooleanValue));
                        break;
                    case KecaknoahILCodeType.PushNil:
                        objstack.Push(KecaknoahNil.Reference);
                        break;
                    case KecaknoahILCodeType.Pop:
                        objstack.Pop();
                        break;

                    //二項演算子--------------------------------------------------------------
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
                        v2 = objstack.Pop().RawObject;
                        v1 = objstack.Pop().RawObject;
                        objstack.Push(KecaknoahReference.CreateRightReference(v1.ExpressionOperation(c.Type, v2)));
                        break;
                    case KecaknoahILCodeType.Not:
                    case KecaknoahILCodeType.Negative:
                        v1 = objstack.Pop().RawObject;
                        objstack.Push(KecaknoahReference.CreateRightReference(v1.ExpressionOperation(c.Type, null)));
                        break;

                    //代入など--------------------------------------------------------------
                    case KecaknoahILCodeType.Assign:
                        v1 = objstack.Pop().RawObject;
                        rfr = objstack.Pop();
                        rfr.RawObject = v1;
                        objstack.Push(KecaknoahReference.CreateRightReference(v1));
                        break;
                    case KecaknoahILCodeType.PlusAssign:
                        v1 = objstack.Pop().RawObject;
                        rfr = objstack.Pop();
                        v2 = rfr.RawObject.ExpressionOperation(KecaknoahILCodeType.Plus, v1);
                        rfr.RawObject = v2;
                        objstack.Push(KecaknoahReference.CreateRightReference(v2));
                        break;
                    case KecaknoahILCodeType.MinusAssign:
                        v1 = objstack.Pop().RawObject;
                        rfr = objstack.Pop();
                        v2 = rfr.RawObject.ExpressionOperation(KecaknoahILCodeType.Minus, v1);
                        rfr.RawObject = v2;
                        objstack.Push(KecaknoahReference.CreateRightReference(v2));
                        break;
                    case KecaknoahILCodeType.MultiplyAssign:
                        v1 = objstack.Pop().RawObject;
                        rfr = objstack.Pop();
                        v2 = rfr.RawObject.ExpressionOperation(KecaknoahILCodeType.Multiply, v1);
                        rfr.RawObject = v2;
                        objstack.Push(KecaknoahReference.CreateRightReference(v2));
                        break;
                    case KecaknoahILCodeType.DivideAssign:
                        v1 = objstack.Pop().RawObject;
                        rfr = objstack.Pop();
                        v2 = rfr.RawObject.ExpressionOperation(KecaknoahILCodeType.Divide, v1);
                        rfr.RawObject = v2;
                        objstack.Push(KecaknoahReference.CreateRightReference(v2));
                        break;
                    case KecaknoahILCodeType.AndAssign:
                        v1 = objstack.Pop().RawObject;
                        rfr = objstack.Pop();
                        v2 = rfr.RawObject.ExpressionOperation(KecaknoahILCodeType.And, v1);
                        rfr.RawObject = v2;
                        objstack.Push(KecaknoahReference.CreateRightReference(v2));
                        break;
                    case KecaknoahILCodeType.OrAssign:
                        v1 = objstack.Pop().RawObject;
                        rfr = objstack.Pop();
                        v2 = rfr.RawObject.ExpressionOperation(KecaknoahILCodeType.Or, v1);
                        rfr.RawObject = v2;
                        objstack.Push(KecaknoahReference.CreateRightReference(v2));
                        break;
                    case KecaknoahILCodeType.XorAssign:
                        v1 = objstack.Pop().RawObject;
                        rfr = objstack.Pop();
                        v2 = rfr.RawObject.ExpressionOperation(KecaknoahILCodeType.Xor, v1);
                        rfr.RawObject = v2;
                        objstack.Push(KecaknoahReference.CreateRightReference(v2));
                        break;
                    case KecaknoahILCodeType.ModularAssign:
                        v1 = objstack.Pop().RawObject;
                        rfr = objstack.Pop();
                        v2 = rfr.RawObject.ExpressionOperation(KecaknoahILCodeType.Modular, v1);
                        rfr.RawObject = v2;
                        objstack.Push(KecaknoahReference.CreateRightReference(v2));
                        break;
                    case KecaknoahILCodeType.LeftBitShiftAssign:
                        v1 = objstack.Pop().RawObject;
                        rfr = objstack.Pop();
                        v2 = rfr.RawObject.ExpressionOperation(KecaknoahILCodeType.LeftBitShift, v1);
                        rfr.RawObject = v2;
                        objstack.Push(KecaknoahReference.CreateRightReference(v2));
                        break;
                    case KecaknoahILCodeType.RightBitShiftAssign:
                        v1 = objstack.Pop().RawObject;
                        rfr = objstack.Pop();
                        v2 = rfr.RawObject.ExpressionOperation(KecaknoahILCodeType.RightBitShift, v1);
                        rfr.RawObject = v2;
                        objstack.Push(KecaknoahReference.CreateRightReference(v2));
                        break;
                    case KecaknoahILCodeType.NilAssign:
                        v1 = objstack.Pop().RawObject;
                        rfr = objstack.Pop();
                        v2 = rfr.RawObject.ExpressionOperation(KecaknoahILCodeType.NilAssign, v1);
                        rfr.RawObject = v2;
                        objstack.Push(KecaknoahReference.CreateRightReference(v2));
                        break;
                    
                    //特殊----------------------------------------------------------------------------
                    case KecaknoahILCodeType.Jump:
                        pc = (int)c.IntegerValue;
                        continue;
                    case KecaknoahILCodeType.Return:
                        return;
                    case KecaknoahILCodeType.Call:
                        args = new Stack<KecaknoahObject>();
                        for (int i = 0; i < c.IntegerValue; i++) args.Push(objstack.Pop().RawObject);
                        v1 = objstack.Pop().RawObject;
                        objstack.Push(KecaknoahReference.CreateRightReference(v1.Call(args.ToArray())));
                        break;
                    case KecaknoahILCodeType.IndexerCall:
                        throw new NotImplementedException("いらなくね");
                    case KecaknoahILCodeType.PushArgument:
                        objstack.Push(KecaknoahReference.CreateRightReference(ilargs[c.IntegerValue]));
                        break;
                    case KecaknoahILCodeType.LoadObject:
                        //名前解決順
                        //TODO: オーバーロード解決
                        //Context変数
                        //Contextメソッド
                        //Module変数
                        //Moduleメソッド
                        KecaknoahReference target;
                        string refname = c.StringValue;
                        //なんつうコードだ
                        if (!Objects.TryGetValue(c.StringValue, out target))
                        {
                            if ((target = Module[c.StringValue]) == KecaknoahNil.Reference)
                            {
                                target = new KecaknoahReference { IsLeftValue = true };
                                objects[c.StringValue] = target;
                                objstack.Push(target);
                            }
                            else
                            {
                                objstack.Push(target);
                            }
                        }
                        else
                        {
                            objstack.Push(target);
                        }
                        break;
                    case KecaknoahILCodeType.LoadMember:
                        var or = objstack.Pop();
                        objstack.Push(or.GetMemberReference(c.StringValue));
                        break;
                    case KecaknoahILCodeType.AsValue:
                        objstack.Push(KecaknoahReference.CreateRightReference(objstack.Pop().RawObject.Clone() as KecaknoahObject));
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
}
