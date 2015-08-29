using System.IO;
using System.Text;
using System.Linq;

namespace Kecaknoah
{
    /// <summary>
    /// Kecaknoahのバイトコード関連の機能を提供します。
    /// </summary>
    public static class KecaknoahBytecode
    {
        private static readonly Encoding stringEncoding = new UTF8Encoding(false, true);
        private static readonly byte[] magicNumber = { (byte)'K', (byte)'C' };
        private const ushort BytecodeVersion = 1;

        private enum TopLevelBlockType : byte
        {
            Class = 1,
            TopLevelMethod = 2
        }

        private enum ClassBlockType : byte
        {
            InnerClass = 10,
            InstanceMethod = 11,
            ClassMethod = 12,
            Local = 13
        }

        private enum ClassElementType : byte
        {
            StartBlocks = 0,
            Name = 1
        }

        private enum MethodElementType : byte
        {
            StartCode = 0,
            Name = 1,
            ArgumentLength = 2,
            VariableArgument = 3
        }

        /// <summary>
        /// <see cref="KecaknoahSource"/>から指定の<see cref="Stream"/>にバイトコードを出力します。
        /// </summary>
        /// <param name="source">対象の<see cref="KecaknoahSource"/></param>
        /// <param name="output">出力先</param>
        public static void Save(KecaknoahSource source, Stream output)
        {
            using (var writer = new BinaryWriter(output, stringEncoding, true))
            {
                writer.Write(magicNumber);
                writer.Write(BytecodeVersion);

                writer.Write(source.Classes.Count + source.TopLevelMethods.Count);

                foreach (var x in source.Classes)
                {
                    writer.Write((byte)TopLevelBlockType.Class);
                    WriteClass(x, writer);
                }

                foreach (var x in source.TopLevelMethods)
                {
                    writer.Write((byte)TopLevelBlockType.TopLevelMethod);
                    WriteMethod(x, writer);
                }
            }
        }

        private static void WriteClass(KecaknoahScriptClassInfo klass, BinaryWriter writer)
        {
            writer.Write((byte)ClassElementType.Name);
            writer.Write(klass.Name);

            writer.Write((byte)ClassElementType.StartBlocks);
            writer.Write(klass.inners.Count + klass.methods.Count + klass.classMethods.Count + klass.Locals.Count);

            foreach (var x in klass.inners)
            {
                writer.Write((byte)ClassBlockType.InnerClass);
                WriteClass(x, writer);
            }

            foreach (var x in klass.methods)
            {
                writer.Write((byte)ClassBlockType.InstanceMethod);
                WriteMethod(x, writer);
            }

            foreach (var x in klass.classMethods)
            {
                writer.Write((byte)ClassBlockType.ClassMethod);
                WriteMethod(x, writer);
            }

            foreach (var x in klass.Locals)
            {
                writer.Write((byte)ClassBlockType.Local);
                writer.Write(x);
            }
        }

        private static void WriteMethod(KecaknoahScriptMethodInfo method, BinaryWriter writer)
        {
            writer.Write((byte)MethodElementType.Name);
            writer.Write(method.Name);

            writer.Write((byte)MethodElementType.ArgumentLength);
            writer.Write(method.ArgumentLength);

            if (method.VariableArgument)
                writer.Write((byte)MethodElementType.VariableArgument);

            writer.Write((byte)MethodElementType.StartCode);
            var codes = method.Codes.Codes;
            writer.Write(codes.Count);

            foreach (var x in codes)
            {
                switch (x.Type)
                {
                    case KecaknoahILCodeType.Nop:
                        writer.Write((byte)0);
                        break;
                    case KecaknoahILCodeType.Label:
                        goto case KecaknoahILCodeType.Nop;
                    case KecaknoahILCodeType.PushInteger:
                        writer.Write((byte)1);
                        writer.Write(x.IntegerValue);
                        break;
                    case KecaknoahILCodeType.PushString:
                        writer.Write((byte)2);
                        writer.Write(x.StringValue);
                        break;
                    case KecaknoahILCodeType.PushSingle:
                        writer.Write((byte)3);
                        writer.Write((float)x.FloatValue);
                        break;
                    case KecaknoahILCodeType.PushDouble:
                        writer.Write((byte)4);
                        writer.Write(x.FloatValue);
                        break;
                    case KecaknoahILCodeType.PushBoolean:
                        writer.Write(x.BooleanValue ? (byte)6 : (byte)5);
                        break;
                    case KecaknoahILCodeType.PushNil:
                        writer.Write((byte)7);
                        break;
                    case KecaknoahILCodeType.Pop:
                        writer.Write((byte)8);
                        break;
                    case KecaknoahILCodeType.Plus:
                        writer.Write((byte)9);
                        break;
                    case KecaknoahILCodeType.Minus:
                        writer.Write((byte)10);
                        break;
                    case KecaknoahILCodeType.Multiply:
                        writer.Write((byte)11);
                        break;
                    case KecaknoahILCodeType.Divide:
                        writer.Write((byte)12);
                        break;
                    case KecaknoahILCodeType.Modular:
                        writer.Write((byte)13);
                        break;
                    case KecaknoahILCodeType.And:
                        writer.Write((byte)14);
                        break;
                    case KecaknoahILCodeType.Or:
                        writer.Write((byte)15);
                        break;
                    case KecaknoahILCodeType.Xor:
                        writer.Write((byte)16);
                        break;
                    case KecaknoahILCodeType.Not:
                        writer.Write((byte)17);
                        break;
                    case KecaknoahILCodeType.Negative:
                        writer.Write((byte)18);
                        break;
                    case KecaknoahILCodeType.AndAlso:
                        writer.Write((byte)19);
                        break;
                    case KecaknoahILCodeType.OrElse:
                        writer.Write((byte)20);
                        break;
                    case KecaknoahILCodeType.LeftBitShift:
                        writer.Write((byte)21);
                        break;
                    case KecaknoahILCodeType.RightBitShift:
                        writer.Write((byte)22);
                        break;
                    case KecaknoahILCodeType.Equal:
                        writer.Write((byte)23);
                        break;
                    case KecaknoahILCodeType.NotEqual:
                        writer.Write((byte)24);
                        break;
                    case KecaknoahILCodeType.Greater:
                        writer.Write((byte)25);
                        break;
                    case KecaknoahILCodeType.Lesser:
                        writer.Write((byte)26);
                        break;
                    case KecaknoahILCodeType.GreaterEqual:
                        writer.Write((byte)27);
                        break;
                    case KecaknoahILCodeType.LesserEqual:
                        writer.Write((byte)28);
                        break;
                    case KecaknoahILCodeType.Assign:
                        writer.Write((byte)29);
                        break;
                    case KecaknoahILCodeType.Jump:
                        writer.Write((byte)30);
                        writer.Write((int)x.IntegerValue);
                        break;
                    case KecaknoahILCodeType.TrueJump:
                        writer.Write((byte)31);
                        writer.Write((int)x.IntegerValue);
                        break;
                    case KecaknoahILCodeType.FalseJump:
                        writer.Write((byte)32);
                        writer.Write((int)x.IntegerValue);
                        break;
                    case KecaknoahILCodeType.Return:
                        writer.Write((byte)33);
                        break;
                    case KecaknoahILCodeType.Yield:
                        writer.Write((byte)34);
                        break;
                    case KecaknoahILCodeType.Call:
                        writer.Write((byte)35);
                        writer.Write((int)x.IntegerValue);
                        break;
                    case KecaknoahILCodeType.IndexerCall:
                        writer.Write((byte)36);
                        writer.Write((int)x.IntegerValue);
                        break;
                    case KecaknoahILCodeType.PushArgument:
                        writer.Write((byte)37);
                        writer.Write((int)x.IntegerValue);
                        break;
                    case KecaknoahILCodeType.LoadObject:
                        writer.Write((byte)38);
                        writer.Write(x.StringValue);
                        break;
                    case KecaknoahILCodeType.LoadMember:
                        writer.Write((byte)39);
                        writer.Write(x.StringValue);
                        break;
                    case KecaknoahILCodeType.AsValue:
                        writer.Write((byte)40);
                        break;
                    case KecaknoahILCodeType.LoadVarg:
                        writer.Write((byte)41);
                        writer.Write((int)x.IntegerValue);
                        break;
                    case KecaknoahILCodeType.StartCoroutine:
                        writer.Write((byte)42);
                        writer.Write(x.StringValue);
                        writer.Write((int)x.IntegerValue);
                        break;
                    case KecaknoahILCodeType.ResumeCoroutine:
                        writer.Write(x.BooleanValue ? (byte)44 : (byte)43);
                        writer.Write(x.StringValue);
                        break;
                    case KecaknoahILCodeType.MakeArray:
                        writer.Write((byte)45);
                        writer.Write((int)x.IntegerValue);
                        break;
                    case KecaknoahILCodeType.PlusAssign:
                        writer.Write((byte)46);
                        break;
                    case KecaknoahILCodeType.MinusAssign:
                        writer.Write((byte)47);
                        break;
                    case KecaknoahILCodeType.MultiplyAssign:
                        writer.Write((byte)48);
                        break;
                    case KecaknoahILCodeType.DivideAssign:
                        writer.Write((byte)49);
                        break;
                    case KecaknoahILCodeType.AndAssign:
                        writer.Write((byte)50);
                        break;
                    case KecaknoahILCodeType.OrAssign:
                        writer.Write((byte)51);
                        break;
                    case KecaknoahILCodeType.XorAssign:
                        writer.Write((byte)52);
                        break;
                    case KecaknoahILCodeType.ModularAssign:
                        writer.Write((byte)53);
                        break;
                    case KecaknoahILCodeType.LeftBitShiftAssign:
                        writer.Write((byte)54);
                        break;
                    case KecaknoahILCodeType.RightBitShiftAssign:
                        writer.Write((byte)55);
                        break;
                    case KecaknoahILCodeType.NilAssign:
                        writer.Write((byte)56);
                        break;
                    default:
                        throw new InvalidDataException("In Kecaknoah, please");
                }
            }
        }

        /// <summary>
        /// バイトコードを読み込み、<see cref="KecaknoahSource"/>に変換します。
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static KecaknoahSource Load(Stream input)
        {
            using (var reader = new BinaryReader(input, stringEncoding, true))
            {
                if (!reader.ReadBytes(magicNumber.Length).SequenceEqual(magicNumber))
                    throw new InvalidDataException("合言葉は「KC」");

                var version = reader.ReadUInt16();
                if (version > BytecodeVersion)
                    throw new InvalidDataException($"Kecaknoah Bytecode v{version} には対応していません。");

                var result = new KecaknoahSource();
                var count = reader.ReadInt32();

                for (var i = 0; i < count; i++)
                {
                    switch ((TopLevelBlockType)reader.ReadByte())
                    {
                        case TopLevelBlockType.Class:
                            result.classes.Add(ReadClass(reader));
                            break;
                        case TopLevelBlockType.TopLevelMethod:
                            result.methods.Add(ReadMethod(reader));
                            break;
                        default:
                            throw new InvalidDataException("変なデータが出たー");
                    }
                }

                return result;
            }
        }

        private static KecaknoahScriptClassInfo ReadClass(BinaryReader reader)
        {
            string name = null;
            while (true)
            {
                switch ((ClassElementType)reader.ReadByte())
                {
                    case ClassElementType.Name:
                        name = reader.ReadString();
                        break;
                    case ClassElementType.StartBlocks:
                        var klass = new KecaknoahScriptClassInfo(name);
                        var count = reader.ReadInt32();
                        for (var i = 0; i < count; i++)
                        {
                            switch ((ClassBlockType)reader.ReadByte())
                            {
                                case ClassBlockType.InnerClass:
                                    klass.AddInnerClass(ReadClass(reader));
                                    break;
                                case ClassBlockType.InstanceMethod:
                                    klass.AddInstanceMethod(ReadMethod(reader));
                                    break;
                                case ClassBlockType.ClassMethod:
                                    klass.AddInstanceMethod(ReadMethod(reader));
                                    break;
                                case ClassBlockType.Local:
                                    klass.AddLocal(reader.ReadString(), null);
                                    break;
                                default:
                                    throw new InvalidDataException("やめて");
                            }
                        }
                        return klass;
                    default:
                        throw new InvalidDataException("無効なクラス");
                }
            }
        }

        private static KecaknoahScriptMethodInfo ReadMethod(BinaryReader reader)
        {
            string name = null;
            var length = 0;
            var vargs = false;

            while (true)
            {
                switch ((MethodElementType)reader.ReadByte())
                {
                    case MethodElementType.Name:
                        name = reader.ReadString();
                        break;
                    case MethodElementType.ArgumentLength:
                        length = reader.ReadInt32();
                        break;
                    case MethodElementType.VariableArgument:
                        vargs = true;
                        break;
                    case MethodElementType.StartCode:
                        var method = new KecaknoahScriptMethodInfo(name, length, vargs);
                        var il = new KecaknoahIL();
                        method.Codes = il;
                        var count = reader.ReadInt32();
                        for (var i = 0; i < count; i++)
                        {
                            switch (reader.ReadByte())
                            {
                                case 0:
                                    il.PushCode(KecaknoahILCodeType.Nop);
                                    break;
                                case 1:
                                    il.PushCode(KecaknoahILCodeType.PushInteger, reader.ReadInt64());
                                    break;
                                case 2:
                                    il.PushCode(KecaknoahILCodeType.PushString, reader.ReadString());
                                    break;
                                case 3:
                                    il.PushCode(KecaknoahILCodeType.PushSingle, reader.ReadSingle());
                                    break;
                                case 4:
                                    il.PushCode(KecaknoahILCodeType.PushDouble, reader.ReadDouble());
                                    break;
                                case 5:
                                    il.PushCode(KecaknoahILCodeType.PushBoolean, false);
                                    break;
                                case 6:
                                    il.PushCode(KecaknoahILCodeType.PushBoolean, true);
                                    break;
                                case 7:
                                    il.PushCode(KecaknoahILCodeType.PushNil);
                                    break;
                                case 8:
                                    il.PushCode(KecaknoahILCodeType.Pop);
                                    break;
                                case 9:
                                    il.PushCode(KecaknoahILCodeType.Plus);
                                    break;
                                case 10:
                                    il.PushCode(KecaknoahILCodeType.Minus);
                                    break;
                                case 11:
                                    il.PushCode(KecaknoahILCodeType.Multiply);
                                    break;
                                case 12:
                                    il.PushCode(KecaknoahILCodeType.Divide);
                                    break;
                                case 13:
                                    il.PushCode(KecaknoahILCodeType.Modular);
                                    break;
                                case 14:
                                    il.PushCode(KecaknoahILCodeType.And);
                                    break;
                                case 15:
                                    il.PushCode(KecaknoahILCodeType.Or);
                                    break;
                                case 16:
                                    il.PushCode(KecaknoahILCodeType.Xor);
                                    break;
                                case 17:
                                    il.PushCode(KecaknoahILCodeType.Not);
                                    break;
                                case 18:
                                    il.PushCode(KecaknoahILCodeType.Negative);
                                    break;
                                case 19:
                                    il.PushCode(KecaknoahILCodeType.AndAlso);
                                    break;
                                case 20:
                                    il.PushCode(KecaknoahILCodeType.OrElse);
                                    break;
                                case 21:
                                    il.PushCode(KecaknoahILCodeType.LeftBitShift);
                                    break;
                                case 22:
                                    il.PushCode(KecaknoahILCodeType.RightBitShift);
                                    break;
                                case 23:
                                    il.PushCode(KecaknoahILCodeType.Equal);
                                    break;
                                case 24:
                                    il.PushCode(KecaknoahILCodeType.NotEqual);
                                    break;
                                case 25:
                                    il.PushCode(KecaknoahILCodeType.Greater);
                                    break;
                                case 26:
                                    il.PushCode(KecaknoahILCodeType.Lesser);
                                    break;
                                case 27:
                                    il.PushCode(KecaknoahILCodeType.GreaterEqual);
                                    break;
                                case 28:
                                    il.PushCode(KecaknoahILCodeType.LesserEqual);
                                    break;
                                case 29:
                                    il.PushCode(KecaknoahILCodeType.Assign);
                                    break;
                                case 30:
                                    il.PushCode(KecaknoahILCodeType.Jump, reader.ReadInt32());
                                    break;
                                case 31:
                                    il.PushCode(KecaknoahILCodeType.TrueJump, reader.ReadInt32());
                                    break;
                                case 32:
                                    il.PushCode(KecaknoahILCodeType.FalseJump, reader.ReadInt32());
                                    break;
                                case 33:
                                    il.PushCode(KecaknoahILCodeType.Return);
                                    break;
                                case 34:
                                    il.PushCode(KecaknoahILCodeType.Yield);
                                    break;
                                case 35:
                                    il.PushCode(KecaknoahILCodeType.Call, reader.ReadInt32());
                                    break;
                                case 36:
                                    il.PushCode(KecaknoahILCodeType.IndexerCall, reader.ReadInt32());
                                    break;
                                case 37:
                                    il.PushCode(KecaknoahILCodeType.PushArgument, reader.ReadInt32());
                                    break;
                                case 38:
                                    il.PushCode(KecaknoahILCodeType.LoadObject, reader.ReadString());
                                    break;
                                case 39:
                                    il.PushCode(KecaknoahILCodeType.LoadMember, reader.ReadString());
                                    break;
                                case 40:
                                    il.PushCode(KecaknoahILCodeType.AsValue);
                                    break;
                                case 41:
                                    il.PushCode(KecaknoahILCodeType.LoadVarg, reader.ReadInt32());
                                    break;
                                case 42:
                                    var code = new KecaknoahILCode() { Type = KecaknoahILCodeType.StartCoroutine };
                                    code.StringValue = reader.ReadString();
                                    code.IntegerValue = reader.ReadInt32();
                                    il.PushCode(code);
                                    break;
                                case 43:
                                    il.PushCode(new KecaknoahILCode()
                                    {
                                        Type = KecaknoahILCodeType.ResumeCoroutine,
                                        StringValue = reader.ReadString(),
                                        BooleanValue = false
                                    });
                                    break;
                                case 44:
                                    il.PushCode(new KecaknoahILCode()
                                    {
                                        Type = KecaknoahILCodeType.ResumeCoroutine,
                                        StringValue = reader.ReadString(),
                                        BooleanValue = true
                                    });
                                    break;
                                case 45:
                                    il.PushCode(KecaknoahILCodeType.MakeArray, reader.ReadInt32());
                                    break;
                                case 46:
                                    il.PushCode(KecaknoahILCodeType.PlusAssign);
                                    break;
                                case 47:
                                    il.PushCode(KecaknoahILCodeType.MinusAssign);
                                    break;
                                case 48:
                                    il.PushCode(KecaknoahILCodeType.MultiplyAssign);
                                    break;
                                case 49:
                                    il.PushCode(KecaknoahILCodeType.DivideAssign);
                                    break;
                                case 50:
                                    il.PushCode(KecaknoahILCodeType.AndAssign);
                                    break;
                                case 51:
                                    il.PushCode(KecaknoahILCodeType.OrAssign);
                                    break;
                                case 52:
                                    il.PushCode(KecaknoahILCodeType.XorAssign);
                                    break;
                                case 53:
                                    il.PushCode(KecaknoahILCodeType.ModularAssign);
                                    break;
                                case 54:
                                    il.PushCode(KecaknoahILCodeType.LeftBitShiftAssign);
                                    break;
                                case 55:
                                    il.PushCode(KecaknoahILCodeType.RightBitShiftAssign);
                                    break;
                                case 56:
                                    il.PushCode(KecaknoahILCodeType.NilAssign);
                                    break;
                                default:
                                    throw new InvalidDataException("危険オペコードにはダマされない！！近づかない！！");
                            }
                        }

                        return method;
                    default:
                        throw new InvalidDataException("無効なメソッド");
                }
            }
        }
    }
}
