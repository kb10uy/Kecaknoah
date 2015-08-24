using System;
using System.Collections.Generic;
using System.Linq;

namespace Kecaknoah
{
    /// <summary>
    /// Kecaknoahで利用されるILの一連を定義します。
    /// </summary>
    public sealed class KecaknoahIL
    {
        private List<KecaknoahILCode> codes = new List<KecaknoahILCode>();
        /// <summary>
        /// コードを取得します。
        /// </summary>
        public IReadOnlyList<KecaknoahILCode> Codes { get; }

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        public KecaknoahIL()
        {
            Codes = codes;
        }


        /// <summary>
        /// 現在の位置にラベルを追加します。
        /// </summary>
        /// <param name="labelName">追加するラベルの名前</param>
        /// <exception cref="ArgumentException">指定された名前のラベルがすでに存在する場合にスローされます。</exception>
        internal void PushLabel(string labelName)
        {
            if (codes.Any(p => p.Type == KecaknoahILCodeType.Label && p.StringValue == labelName))
                throw new ArgumentException("同じ名前のラベルがすでに存在します。");
            codes.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.Label, StringValue = labelName });
        }

        /// <summary>
        /// 値を伴わないコードを追加します。
        /// </summary>
        /// <param name="codeType">追加する<see cref="KecaknoahILCodeType"/></param>
        internal void PushCode(KecaknoahILCodeType codeType)
        {
            codes.Add(new KecaknoahILCode { Type = codeType });
        }

        /// <summary>
        /// コードを追加します。
        /// </summary>
        /// <param name="code">コード</param>
        internal void PushCode(KecaknoahILCode code)
        {
            codes.Add(code);
        }

        /// <summary>
        /// 整数値を伴うコードを追加します。
        /// </summary>
        /// <param name="codeType">追加する<see cref="KecaknoahILCodeType"/></param>
        /// <param name="value">値</param>
        internal void PushCode(KecaknoahILCodeType codeType, long value)
        {
            codes.Add(new KecaknoahILCode { Type = codeType, IntegerValue = value });
        }

        /// <summary>
        /// 浮動小数点数値を伴うコードを追加します。
        /// </summary>
        /// <param name="codeType">追加する<see cref="KecaknoahILCodeType"/></param>
        /// <param name="value">値</param>
        internal void PushCode(KecaknoahILCodeType codeType, double value)
        {
            codes.Add(new KecaknoahILCode { Type = codeType, FloatValue = value });
        }

        /// <summary>
        /// 文字列を伴うコードを追加します。
        /// </summary>
        /// <param name="codeType">追加する<see cref="KecaknoahILCodeType"/></param>
        /// <param name="value">値</param>
        internal void PushCode(KecaknoahILCodeType codeType, string value)
        {
            codes.Add(new KecaknoahILCode { Type = codeType, StringValue = value });
        }

        /// <summary>
        /// 真偽値を伴うコードを追加します。
        /// </summary>
        /// <param name="codeType">追加する<see cref="KecaknoahILCodeType"/></param>
        /// <param name="value">値</param>
        internal void PushCode(KecaknoahILCodeType codeType, bool value)
        {
            codes.Add(new KecaknoahILCode { Type = codeType, BooleanValue = value });
        }

        /// <summary>
        /// 複数のコードを追加します。
        /// </summary>
        /// <param name="list">コード</param>
        internal void PushCodes(IEnumerable<KecaknoahILCode> list)
        {
            codes.AddRange(list);
        }

        /// <summary>
        /// 複数のコードの先頭にFalseJumpを追加します。
        /// </summary>
        /// <param name="list">コード</param>
        internal void PushCodesWithFalseJump(IEnumerable<KecaknoahILCode> list)
        {
            var t = codes.Count;
            codes.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.FalseJump, IntegerValue = t + list.Count() + 1 });
            codes.AddRange(list);
        }

        /// <summary>
        /// 複数のコードの先頭にTrueJumpを追加します。
        /// </summary>
        /// <param name="list">コード</param>
        internal void PushCodesWithTrueJump(IEnumerable<KecaknoahILCode> list)
        {
            var t = codes.Count;
            codes.Add(new KecaknoahILCode { Type = KecaknoahILCodeType.FalseJump, IntegerValue = t + list.Count() + 1 });
            codes.AddRange(list);
        }
    }

    /// <summary>
    /// <see cref="KecaknoahIL"/>の1つのコードを定義します。
    /// </summary>
    public sealed class KecaknoahILCode
    {
        /// <summary>
        /// このコードの種類を取得します。
        /// </summary>
        public KecaknoahILCodeType Type { get; internal set; }

        /// <summary>
        /// このコードに格納される整数値またはジャンプ先アドレスを取得します。
        /// </summary>
        public long IntegerValue { get; internal set; }

        /// <summary>
        /// このコードに格納される浮動小数点数を取得します。
        /// </summary>
        public double FloatValue { get; internal set; }

        /// <summary>
        /// このコードに格納される文字列を取得します。
        /// </summary>
        public string StringValue { get; internal set; }

        /// <summary>
        /// このコードに格納される真偽値を取得します。
        /// </summary>
        public bool BooleanValue { get; internal set; }

        /// <summary>
        /// 現在のオブジェクトを表す文字列を返します。
        /// </summary>
        /// <returns>文字列</returns>
        public override string ToString()
        {
            switch (Type)
            {
                case KecaknoahILCodeType.Label:
                    return $"{StringValue}:";
                case KecaknoahILCodeType.PushInteger:
                    return $"Push.int {IntegerValue}";
                case KecaknoahILCodeType.PushSingle:
                    return $"Push.single {FloatValue}";
                case KecaknoahILCodeType.PushDouble:
                    return $"Push.double {FloatValue}";
                case KecaknoahILCodeType.PushString:
                    return $"Push.string \"{StringValue}\"";
                case KecaknoahILCodeType.PushBoolean:
                    return $"Push.bool {BooleanValue}";
                case KecaknoahILCodeType.PushNil:
                    return $"Push.nil";
                case KecaknoahILCodeType.Jump:
                    return $"Jump to {IntegerValue}";
                case KecaknoahILCodeType.FalseJump:
                    return $"FalseJump to {IntegerValue}";
                case KecaknoahILCodeType.TrueJump:
                    return $"TrueJump to {IntegerValue}";
                case KecaknoahILCodeType.Call:
                    return $"Call with {IntegerValue} arguments";
                case KecaknoahILCodeType.IndexerCall:
                    return $"Indexer with {IntegerValue} arguments";
                case KecaknoahILCodeType.LoadMember:
                    return $"LoadMember \"{StringValue}\"";
                case KecaknoahILCodeType.LoadObject:
                    return $"LoadObject \"{StringValue}\"";
                case KecaknoahILCodeType.PushArgument:
                    return $"Push Argument#{IntegerValue}";
                case KecaknoahILCodeType.LoadVarg:
                    return $"Load Vargs with {IntegerValue} arguments";
                case KecaknoahILCodeType.StartCoroutine:
                    return $"Start \"{StringValue}\" with {IntegerValue} arguments";
                case KecaknoahILCodeType.ResumeCoroutine:
                    return $"Resume \"{StringValue}\"";
                default:
                    return Type.ToString();
            }
        }
    }

    /// <summary>
    /// <see cref="KecaknoahILCode"/>の種類を定義します。
    /// </summary>
    public enum KecaknoahILCodeType
    {
        /// <summary>
        /// nop
        /// </summary>
        Nop,
        /// <summary>
        /// ジャンプ先などに指定するラベル
        /// </summary>
        Label,
        /// <summary>
        /// pin
        /// </summary>
        PushInteger,
        /// <summary>
        /// pst
        /// </summary>
        PushString,
        /// <summary>
        /// psi
        /// </summary>
        PushSingle,
        /// <summary>
        /// pdo
        /// </summary>
        PushDouble,
        /// <summary>
        /// pbo
        /// </summary>
        PushBoolean,
        /// <summary>
        /// pni
        /// </summary>
        PushNil,
        /// <summary>
        /// pop
        /// </summary>
        Pop,
        /// <summary>
        /// add
        /// </summary>
        Plus,
        /// <summary>
        /// sub
        /// </summary>
        Minus,
        /// <summary>
        /// mul
        /// </summary>
        Multiply,
        /// <summary>
        /// div
        /// </summary>
        Divide,
        /// <summary>
        /// mod
        /// </summary>
        Modular,
        /// <summary>
        /// and
        /// </summary>
        And,
        /// <summary>
        /// or
        /// </summary>
        Or,
        /// <summary>
        /// xor
        /// </summary>
        Xor,
        /// <summary>
        /// not
        /// </summary>
        Not,
        /// <summary>
        /// neg
        /// </summary>
        Negative,
        /// <summary>
        /// aal
        /// </summary>
        AndAlso,
        /// <summary>
        /// ore
        /// </summary>
        OrElse,
        /// <summary>
        /// lbs
        /// </summary>
        LeftBitShift,
        /// <summary>
        /// rbs
        /// </summary>
        RightBitShift,
        /// <summary>
        /// eql
        /// </summary>
        Equal,
        /// <summary>
        /// neq
        /// </summary>
        NotEqual,
        /// <summary>
        /// gre
        /// </summary>
        Greater,
        /// <summary>
        /// les
        /// </summary>
        Lesser,
        /// <summary>
        /// geq
        /// </summary>
        GreaterEqual,
        /// <summary>
        /// leq
        /// </summary>
        LesserEqual,
        /// <summary>
        /// mov
        /// </summary>
        Assign,
        /// <summary>
        /// jmp
        /// </summary>
        Jump,
        /// <summary>
        /// tjm
        /// </summary>
        TrueJump,
        /// <summary>
        /// fjm
        /// </summary>
        FalseJump,
        /// <summary>
        /// ret
        /// </summary>
        Return,
        /// <summary>
        /// yld
        /// </summary>
        Yield,
        /// <summary>
        /// cal
        /// </summary>
        Call,
        /// <summary>
        /// idx
        /// </summary>
        IndexerCall,
        /// <summary>
        /// par
        /// </summary>
        PushArgument,
        /// <summary>
        /// ldo
        /// </summary>
        LoadObject,
        /// <summary>
        /// ldm
        /// </summary>
        LoadMember,
        /// <summary>
        /// val
        /// </summary>
        AsValue,
        /// <summary>
        /// vrg
        /// </summary>
        LoadVarg,
        /// <summary>
        /// scr
        /// </summary>
        StartCoroutine,
        /// <summary>
        /// rcr
        /// </summary>
        ResumeCoroutine,
        /// <summary>
        /// arr
        /// </summary>
        MakeArray,

        /// <summary>
        /// ada
        /// </summary>
        PlusAssign,
        /// <summary>
        /// sba
        /// </summary>
        MinusAssign,
        /// <summary>
        /// mla
        /// </summary>
        MultiplyAssign,
        /// <summary>
        /// dva
        /// </summary>
        DivideAssign,
        /// <summary>
        /// ana
        /// </summary>
        AndAssign,
        /// <summary>
        /// ora
        /// </summary>
        OrAssign,
        /// <summary>
        /// xoa
        /// </summary>
        XorAssign,
        /// <summary>
        /// mda
        /// </summary>
        ModularAssign,
        /// <summary>
        /// lba
        /// </summary>
        LeftBitShiftAssign,
        /// <summary>
        /// rba
        /// </summary>
        RightBitShiftAssign,
        /// <summary>
        /// nla
        /// </summary>
        NilAssign,
    }
}
