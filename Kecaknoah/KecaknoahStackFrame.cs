using System;
using System.Collections.Generic;
using Kecaknoah.Type;

namespace Kecaknoah
{
    /// <summary>
    /// ローカル変数を含むスタックフレームを提供します。
    /// </summary>
    public sealed class KecaknoahStackFrame
    {
        private Dictionary<string, KecaknoahReference> locals = new Dictionary<string, KecaknoahReference>();
        /// <summary>
        /// ローカル変数の参照を取得します。
        /// </summary>
        /// <remarks>再開可能な状態で操作すると不具合が発生する可能性があります。</remarks>
        public IDictionary<string, KecaknoahReference> Locals => locals;

        private Dictionary<string, KecaknoahCoroutineFrame> cors = new Dictionary<string, KecaknoahCoroutineFrame>();
        /// <summary>
        /// 起動中のコルーチンの参照を取得します。
        /// </summary>
        /// <remarks>再開可能な状態で操作すると不具合が発生する可能性があります。</remarks>
        public IDictionary<string, KecaknoahCoroutineFrame> Coroutines => cors;

        /// <summary>
        /// 実行中の<see cref="KecaknoahILCode"/>のリストを取得します。
        /// </summary>
        public IReadOnlyList<KecaknoahILCode> Codes { get; internal set; }

        /// <summary>
        /// コードにおける参照のスタックを取得します。
        /// </summary>
        /// <remarks>再開可能な状態で操作すると不具合が発生する可能性があります。</remarks>
        public Stack<KecaknoahReference> ReferenceStack { get; internal set; } = new Stack<KecaknoahReference>();

        /// <summary>
        /// 引数を取得します。
        /// </summary>
        public IList<KecaknoahObject> Arguments { get; internal set; } = new List<KecaknoahObject>();

        /// <summary>
        /// 可変長引数を取得します。
        /// </summary>
        public IList<KecaknoahObject> VariableArguments { get; internal set; } = new List<KecaknoahObject>();

        /// <summary>
        /// 現在このスタックフレームが属している<see cref="KecaknoahContext"/>を取得します。
        /// </summary>
        public KecaknoahContext RunningContext { get; internal set; }

        /// <summary>
        /// 現在の<see cref="KecaknoahIL"/>の位置を取得します。
        /// </summary>
        public int ProgramCounter { get; internal set; }

        /// <summary>
        /// 現在の状態でreturn/yieldされた<see cref="KecaknoahObject"/>を取得します。
        /// </summary>
        public KecaknoahObject ReturningObject { get; internal set; } = KecaknoahNil.Instance;

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="ctx">実行している<see cref="KecaknoahContext"/></param>
        /// <param name="il">実行する<see cref="KecaknoahIL"/></param>
        public KecaknoahStackFrame(KecaknoahContext ctx, KecaknoahIL il)
        {
            Codes = il.Codes;
            ProgramCounter = 0;
            RunningContext = ctx;
        }

        private KecaknoahReference GetReference(string name)
        {
            KecaknoahReference result;
            if (Locals.ContainsKey(name)) return Locals[name];
            if ((result = RunningContext.Module.GetReference(name)) != KecaknoahNil.Reference) return result;
            result = new KecaknoahReference { IsLeftValue = true };
            locals[name] = result;
            return result;
        }

        /// <summary>
        /// 現在のコードを最初から実行します。
        /// </summary>
        /// <returns>継続可能な場合はtrueが、それ以外の場合はfalseが帰ります。</returns>
        public bool Execute()
        {
            Reset();
            return Resume() == KecaknoahFunctionResultType.CanResume;
        }

        /// <summary>
        /// 現在の状態で、現在のコードの実行を再開します。
        /// </summary>
        /// <returns></returns>
        public KecaknoahFunctionResultType Resume()
        {
            KecaknoahObject v1, v2;
            KecaknoahReference rfr;
            Stack<KecaknoahObject> args;
            while (ProgramCounter < Codes.Count)
            {
                var c = Codes[ProgramCounter];
                switch (c.Type)
                {
                    //基本--------------------------------------------------------------------
                    case KecaknoahILCodeType.Nop:
                        break;
                    case KecaknoahILCodeType.Label:
                        break;
                    case KecaknoahILCodeType.PushInteger:
                        ReferenceStack.Push(KecaknoahReference.Right(c.IntegerValue));
                        break;
                    case KecaknoahILCodeType.PushString:
                        ReferenceStack.Push(KecaknoahReference.Right(c.StringValue));
                        break;
                    case KecaknoahILCodeType.PushDouble:
                        ReferenceStack.Push(KecaknoahReference.Right(c.FloatValue));
                        break;
                    case KecaknoahILCodeType.PushBoolean:
                        ReferenceStack.Push(KecaknoahReference.Right(c.BooleanValue));
                        break;
                    case KecaknoahILCodeType.PushNil:
                        ReferenceStack.Push(KecaknoahNil.Reference);
                        break;
                    case KecaknoahILCodeType.Pop:
                        ReferenceStack.Pop();
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
                        v2 = ReferenceStack.Pop().RawObject;
                        v1 = ReferenceStack.Pop().RawObject;
                        ReferenceStack.Push(KecaknoahReference.Right(v1.ExpressionOperation(c.Type, v2)));
                        break;
                    case KecaknoahILCodeType.Not:
                    case KecaknoahILCodeType.Negative:
                        v1 = ReferenceStack.Pop().RawObject;
                        ReferenceStack.Push(KecaknoahReference.Right(v1.ExpressionOperation(c.Type, null)));
                        break;

                    //代入など--------------------------------------------------------------
                    case KecaknoahILCodeType.Assign:
                        v1 = ReferenceStack.Pop().RawObject;
                        rfr = ReferenceStack.Pop();
                        rfr.RawObject = v1.AsByValValue();
                        ReferenceStack.Push(KecaknoahReference.Right(v1));
                        break;
                    case KecaknoahILCodeType.PlusAssign:
                        v1 = ReferenceStack.Pop().RawObject;
                        rfr = ReferenceStack.Pop();
                        v2 = rfr.RawObject.ExpressionOperation(KecaknoahILCodeType.Plus, v1);
                        rfr.RawObject = v2;
                        ReferenceStack.Push(KecaknoahReference.Right(v2));
                        break;
                    case KecaknoahILCodeType.MinusAssign:
                        v1 = ReferenceStack.Pop().RawObject;
                        rfr = ReferenceStack.Pop();
                        v2 = rfr.RawObject.ExpressionOperation(KecaknoahILCodeType.Minus, v1);
                        rfr.RawObject = v2;
                        ReferenceStack.Push(KecaknoahReference.Right(v2));
                        break;
                    case KecaknoahILCodeType.MultiplyAssign:
                        v1 = ReferenceStack.Pop().RawObject;
                        rfr = ReferenceStack.Pop();
                        v2 = rfr.RawObject.ExpressionOperation(KecaknoahILCodeType.Multiply, v1);
                        rfr.RawObject = v2;
                        ReferenceStack.Push(KecaknoahReference.Right(v2));
                        break;
                    case KecaknoahILCodeType.DivideAssign:
                        v1 = ReferenceStack.Pop().RawObject;
                        rfr = ReferenceStack.Pop();
                        v2 = rfr.RawObject.ExpressionOperation(KecaknoahILCodeType.Divide, v1);
                        rfr.RawObject = v2;
                        ReferenceStack.Push(KecaknoahReference.Right(v2));
                        break;
                    case KecaknoahILCodeType.AndAssign:
                        v1 = ReferenceStack.Pop().RawObject;
                        rfr = ReferenceStack.Pop();
                        v2 = rfr.RawObject.ExpressionOperation(KecaknoahILCodeType.And, v1);
                        rfr.RawObject = v2;
                        ReferenceStack.Push(KecaknoahReference.Right(v2));
                        break;
                    case KecaknoahILCodeType.OrAssign:
                        v1 = ReferenceStack.Pop().RawObject;
                        rfr = ReferenceStack.Pop();
                        v2 = rfr.RawObject.ExpressionOperation(KecaknoahILCodeType.Or, v1);
                        rfr.RawObject = v2;
                        ReferenceStack.Push(KecaknoahReference.Right(v2));
                        break;
                    case KecaknoahILCodeType.XorAssign:
                        v1 = ReferenceStack.Pop().RawObject;
                        rfr = ReferenceStack.Pop();
                        v2 = rfr.RawObject.ExpressionOperation(KecaknoahILCodeType.Xor, v1);
                        rfr.RawObject = v2;
                        ReferenceStack.Push(KecaknoahReference.Right(v2));
                        break;
                    case KecaknoahILCodeType.ModularAssign:
                        v1 = ReferenceStack.Pop().RawObject;
                        rfr = ReferenceStack.Pop();
                        v2 = rfr.RawObject.ExpressionOperation(KecaknoahILCodeType.Modular, v1);
                        rfr.RawObject = v2;
                        ReferenceStack.Push(KecaknoahReference.Right(v2));
                        break;
                    case KecaknoahILCodeType.LeftBitShiftAssign:
                        v1 = ReferenceStack.Pop().RawObject;
                        rfr = ReferenceStack.Pop();
                        v2 = rfr.RawObject.ExpressionOperation(KecaknoahILCodeType.LeftBitShift, v1);
                        rfr.RawObject = v2;
                        ReferenceStack.Push(KecaknoahReference.Right(v2));
                        break;
                    case KecaknoahILCodeType.RightBitShiftAssign:
                        v1 = ReferenceStack.Pop().RawObject;
                        rfr = ReferenceStack.Pop();
                        v2 = rfr.RawObject.ExpressionOperation(KecaknoahILCodeType.RightBitShift, v1);
                        rfr.RawObject = v2;
                        ReferenceStack.Push(KecaknoahReference.Right(v2));
                        break;
                    case KecaknoahILCodeType.NilAssign:
                        v1 = ReferenceStack.Pop().RawObject;
                        rfr = ReferenceStack.Pop();
                        v2 = rfr.RawObject.ExpressionOperation(KecaknoahILCodeType.NilAssign, v1);
                        rfr.RawObject = v2;
                        ReferenceStack.Push(KecaknoahReference.Right(v2));
                        break;

                    //特殊----------------------------------------------------------------------------
                    case KecaknoahILCodeType.StartCoroutine:
                        args = new Stack<KecaknoahObject>();
                        for (int i = 0; i < c.IntegerValue; i++) args.Push(ReferenceStack.Pop().RawObject.AsByValValue());
                        var ct = ReferenceStack.Peek().RawObject as KecaknoahScriptFunction;
                        var ict = ReferenceStack.Peek().RawObject as KecaknoahInteropFunction;
                        ReferenceStack.Pop();
                        if (ct == null && ict == null) throw new InvalidOperationException("スクリプト上のメソッド以外はコルーチン化出来ません");
                        if (!ct.Equals(null))
                        {
                            cors[c.StringValue] = new KecaknoahScriptCoroutineFrame(RunningContext, ct, args.ToArray());
                        }
                        else
                        {
                            cors[c.StringValue] = new KecaknoahInteropCoroutineFrame(RunningContext, ict, args.ToArray());
                        }
                        break;
                    case KecaknoahILCodeType.ResumeCoroutine:
                        if (!cors.ContainsKey(c.StringValue)) throw new KeyNotFoundException($"{c.StringValue}という名前のコルーチンは生成されていません。");
                        var cobj = cors[c.StringValue];
                        if (cobj == null)
                        {
                            if (c.BooleanValue) ReferenceStack.Pop();
                            ReferenceStack.Push(KecaknoahNil.Reference);
                            break;
                        }
                        var cr = cobj.Resume();
                        if (c.BooleanValue)
                        {
                            //2引数
                            var vas = ReferenceStack.Pop();
                            vas.RawObject = cr.ReturningObject;
                            ReferenceStack.Push(KecaknoahReference.Right((cr.ResultState == KecaknoahFunctionResultType.CanResume).AsKecaknoahBoolean()));
                        }
                        else
                        {
                            //1引数
                            ReferenceStack.Push(KecaknoahReference.Right(cr.ReturningObject));
                        }
                        if (cr.ResultState != KecaknoahFunctionResultType.CanResume)
                        {
                            cors[c.StringValue] = null;
                        }
                        break;
                    case KecaknoahILCodeType.MakeArray:
                        var ars = new Stack<KecaknoahObject>();
                        for (int i = 0; i < c.IntegerValue; i++) ars.Push(ReferenceStack.Pop().RawObject);
                        var arr = new KecaknoahArray(new[] { (int)c.IntegerValue });
                        for (int i = 0; i < c.IntegerValue; i++) arr.array[i] = new KecaknoahReference { IsLeftValue = true, RawObject = ars.Pop() };
                        ReferenceStack.Push(KecaknoahReference.Right(arr));
                        break;
                    case KecaknoahILCodeType.Jump:
                        ProgramCounter = (int)c.IntegerValue;
                        continue;
                    case KecaknoahILCodeType.TrueJump:
                        v1 = ReferenceStack.Pop().RawObject;
                        if (v1.ToBoolean())
                        {
                            ProgramCounter = (int)c.IntegerValue;
                            continue;
                        }
                        break;
                    case KecaknoahILCodeType.FalseJump:
                        v1 = ReferenceStack.Pop().RawObject;
                        if (!v1.ToBoolean())
                        {
                            ProgramCounter = (int)c.IntegerValue;
                            continue;
                        }
                        break;
                    case KecaknoahILCodeType.Return:
                        ReturningObject = ReferenceStack.Pop().RawObject;
                        return KecaknoahFunctionResultType.NoResume;
                    case KecaknoahILCodeType.Yield:
                        ReturningObject = ReferenceStack.Pop().RawObject;
                        ProgramCounter++;
                        return KecaknoahFunctionResultType.CanResume;
                    case KecaknoahILCodeType.Call:
                        args = new Stack<KecaknoahObject>();
                        for (int i = 0; i < c.IntegerValue; i++) args.Push(ReferenceStack.Pop().RawObject);
                        v1 = ReferenceStack.Pop().RawObject;
                        if (v1 == KecaknoahNil.Instance) throw new InvalidOperationException("nilに対してメソッド呼び出し出来ません。名前を間違っていませんか？");
                        ReferenceStack.Push(KecaknoahReference.Right(v1.Call(RunningContext, args.ToArray()).ReturningObject));
                        break;
                    case KecaknoahILCodeType.IndexerCall:
                        args = new Stack<KecaknoahObject>();
                        for (int i = 0; i < c.IntegerValue; i++) args.Push(ReferenceStack.Pop().RawObject);
                        v1 = ReferenceStack.Pop().RawObject;
                        if (v1 == KecaknoahNil.Instance) throw new InvalidOperationException("nilに対してインデクサ呼び出し出来ません。名前を間違っていませんか？");
                        ReferenceStack.Push(v1.GetIndexerReference(args.ToArray()));
                        break;
                    case KecaknoahILCodeType.PushArgument:
                        ReferenceStack.Push(KecaknoahReference.Right(Arguments[(int)c.IntegerValue]));
                        break;
                    case KecaknoahILCodeType.LoadObject:
                        string refname = c.StringValue;
                        ReferenceStack.Push(GetReference(refname));
                        break;
                    case KecaknoahILCodeType.LoadMember:
                        var or = ReferenceStack.Pop();
                        if (or.RawObject == KecaknoahNil.Instance) throw new InvalidOperationException("nilに対してメンバーアクセス出来ません。名前を間違っていませんか？");
                        ReferenceStack.Push(or.GetMemberReference(c.StringValue));
                        break;
                    case KecaknoahILCodeType.LoadVarg:
                        args = new Stack<KecaknoahObject>();
                        for (int i = 0; i < c.IntegerValue; i++) args.Push(ReferenceStack.Pop().RawObject);
                        ReferenceStack.Push(KecaknoahReference.Right(VariableArguments[(int)args.Pop().ToInt64()]));
                        break;
                    case KecaknoahILCodeType.AsValue:
                        ReferenceStack.Push(KecaknoahReference.Right(ReferenceStack.Pop().RawObject.AsByValValue()));
                        break;
                }
                ProgramCounter++;
            }
            if (ReferenceStack.Count == 0) ReferenceStack.Push(KecaknoahNil.Reference);
            ReturningObject = ReferenceStack.Pop().RawObject;
            return KecaknoahFunctionResultType.NoResume;
        }

        /// <summary>
        /// 実行位置をリセットします。
        /// </summary>
        public void Reset()
        {
            ProgramCounter = 0;
        }
    }
}
