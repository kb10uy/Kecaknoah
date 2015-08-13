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

        private Stack<KecaknoahObject> objstack = new Stack<KecaknoahObject>();
        private Dictionary<string, KecaknoahObject> objects = new Dictionary<string, KecaknoahObject>();
        /// <summary>
        /// このコンテキストで定義されている変数を取得します。
        /// </summary>
        public IReadOnlyDictionary<string, KecaknoahObject> Objects { get; }

        internal KecaknoahContext(KecaknoahModule module)
        {
            Module = module;
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
            return objstack.Pop();
        }

        private void ExecuteIL(KecaknoahIL il)
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
                    case KecaknoahILCodeType.Label:
                        break;
                    case KecaknoahILCodeType.PushInteger:
                        objstack.Push(c.IntegerValue.AsKecaknoahInteger());
                        break;
                    case KecaknoahILCodeType.PushString:
                        objstack.Push(c.StringValue.AsKecaknoahString());
                        break;
                    case KecaknoahILCodeType.PushSingle:
                        objstack.Push(((float)c.FloatValue).AsKecaknoahSingle());
                        break;
                    case KecaknoahILCodeType.PushDouble:
                        objstack.Push(c.FloatValue.AsKecaknoahDouble());
                        break;
                    case KecaknoahILCodeType.PushBoolean:
                        objstack.Push(c.BooleanValue.AsKecaknoahBoolean());
                        break;
                    case KecaknoahILCodeType.PushNil:
                        objstack.Push(KecaknoahNil.Instance);
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
                        v2 = objstack.Pop();
                        v1 = objstack.Pop();
                        objstack.Push(v1.ExpressionOperation(c.Type, v2));
                        break;
                    case KecaknoahILCodeType.Not:
                    case KecaknoahILCodeType.Negative:
                        v1 = objstack.Pop();
                        objstack.Push(v1.ExpressionOperation(c.Type, null));
                        break;
                    case KecaknoahILCodeType.Assign:
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
                        for (int i = 0; i < c.IntegerValue + 1; i++) args.Push(objstack.Pop());
                        v1 = objstack.Pop();
                        args.Push(v1.Call(args.ToArray()));
                        break;
                    case KecaknoahILCodeType.IndexerCall:
                        args = new Stack<KecaknoahObject>();
                        for (int i = 0; i < c.IntegerValue + 1; i++) args.Push(objstack.Pop());
                        v1 = objstack.Pop();
                        args.Push(v1.GetIndexer(args.ToArray()));
                        break;
                    case KecaknoahILCodeType.PushArgument:
                        //仮
                    case KecaknoahILCodeType.LoadObject:
                        //仮
                        objstack.Push(KecaknoahNil.Instance);
                        break;
                    case KecaknoahILCodeType.LoadMember:
                        v1 = objstack.Pop();
                        var no = v1.GetMember(c.StringValue);
                        objstack.Push(no);
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
