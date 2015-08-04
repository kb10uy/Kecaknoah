using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah.Type
{
    /// <summary>
    /// Kecaknoahでの整数を定義します。
    /// </summary>
    public sealed class KecaknoahInteger : KecaknoahObject
    {
        /// <summary>
        /// 実際の値を取得・設定します。
        /// </summary>
        public long Value { get; set; } = 0;

        /// <summary>
        /// 最大値のインスタンスを取得します。
        /// </summary>
        public static KecaknoahInteger MaxValue = new KecaknoahInteger { Value = long.MaxValue };

        /// <summary>
        /// 最小値のインスタンスを取得します。
        /// </summary>
        public static KecaknoahInteger MinValue = new KecaknoahInteger { Value = long.MaxValue };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override KecaknoahObject GetMember(string name)
        {
            switch (name)
            {
                case "MAX_VALUE":
                    return long.MaxValue.AsKecaknoahInteger();
                case "MIN_VALUE":
                    return long.MinValue.AsKecaknoahInteger();
            }
            return base.GetMember(name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public override KecaknoahObject ExpressionOperation(KecaknoahOperatorType op, KecaknoahObject target)
        {
            var t = (long)target.AsRawObject<long>();
            switch(op)
            {
                case KecaknoahOperatorType.Plus:
                    return (Value + t).AsKecaknoahInteger();
                case KecaknoahOperatorType.Minus:
                    return (Value - t).AsKecaknoahInteger();
                case KecaknoahOperatorType.Multiply:
                    return (Value * t).AsKecaknoahInteger();
                case KecaknoahOperatorType.Divide:
                    return (Value / t).AsKecaknoahInteger();
                case KecaknoahOperatorType.And:
                    return (Value & t).AsKecaknoahInteger();
                case KecaknoahOperatorType.Or:
                    return (Value | t).AsKecaknoahInteger();
                case KecaknoahOperatorType.Xor:
                    return (Value ^ t).AsKecaknoahInteger();
                case KecaknoahOperatorType.Modular:
                    return (Value % t).AsKecaknoahInteger();
                case KecaknoahOperatorType.Assign:
                    return (Value = t).AsKecaknoahInteger();
                case KecaknoahOperatorType.Equal:
                    return (Value == t).AsKecaknoahBoolean();
                case KecaknoahOperatorType.NotEqual:
                    return (Value != t).AsKecaknoahBoolean();
                case KecaknoahOperatorType.Greater:
                    return (Value > t).AsKecaknoahBoolean();
                case KecaknoahOperatorType.Lesser:
                    return (Value < t).AsKecaknoahBoolean();
                case KecaknoahOperatorType.GreaterEqual:
                    return (Value >= t).AsKecaknoahBoolean();
                case KecaknoahOperatorType.LesserEqual:
                    return (Value <= t).AsKecaknoahBoolean();
                default:
                    throw new InvalidOperationException("この演算は対応していません。");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override object AsRawObject<T>()
        {
            var to = typeof(T);
            switch (to.Name)
            {
                case nameof(Byte):
                    return Convert.ToByte(Value);
                case nameof(SByte):
                    return Convert.ToSByte(Value);
                case nameof(Int16):
                    return Convert.ToInt16(Value);
                case nameof(UInt16):
                    return Convert.ToUInt16(Value);
                case nameof(Int32):
                    return Convert.ToInt32(Value);
                case nameof(UInt32):
                    return Convert.ToUInt32(Value);
                case nameof(Int64):
                    return Value;
                case nameof(UInt64):
                    return Convert.ToUInt64(Value);
                case nameof(Single):
                    return Convert.ToSingle(Value);
                case nameof(Double):
                    return Convert.ToDouble(Value);
                case nameof(Decimal):
                    return Convert.ToDecimal(Value);
                case nameof(String):
                    return Value.ToString();
                case nameof(Boolean):
                    return Convert.ToBoolean(Value);
            }

            return default(T);
        }
    }

    /// <summary>
    /// Kecaknoahでの単精度浮動小数点数を定義します。
    /// </summary>
    public sealed class KecaknoahSingle : KecaknoahObject
    {
        /// <summary>
        /// 実際の値を取得・設定します。
        /// </summary>
        public float Value { get; set; } = 0;

        /// <summary>
        /// 最大値のインスタンスを取得します。
        /// </summary>
        public static KecaknoahSingle MaxValue = new KecaknoahSingle { Value = float.MaxValue };

        /// <summary>
        /// 最小値のインスタンスを取得します。
        /// </summary>
        public static KecaknoahSingle MinValue = new KecaknoahSingle { Value = float.MaxValue };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override KecaknoahObject GetMember(string name)
        {
            switch (name)
            {
                case "MAX_VALUE":
                    return MaxValue;
                case "MIN_VALUE":
                    return MinValue;
            }
            return base.GetMember(name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public override KecaknoahObject ExpressionOperation(KecaknoahOperatorType op, KecaknoahObject target)
        {
            var t = (float)target.AsRawObject<float>();
            switch (op)
            {
                case KecaknoahOperatorType.Plus:
                    return (Value + t).AsKecaknoahSingle();
                case KecaknoahOperatorType.Minus:
                    return (Value - t).AsKecaknoahSingle();
                case KecaknoahOperatorType.Multiply:
                    return (Value * t).AsKecaknoahSingle();
                case KecaknoahOperatorType.Divide:
                    return (Value / t).AsKecaknoahSingle();
                case KecaknoahOperatorType.Modular:
                    return (Value % t).AsKecaknoahSingle();
                case KecaknoahOperatorType.Assign:
                    return (Value = t).AsKecaknoahSingle();
                case KecaknoahOperatorType.Equal:
                    return (Value == t).AsKecaknoahBoolean();
                case KecaknoahOperatorType.NotEqual:
                    return (Value != t).AsKecaknoahBoolean();
                case KecaknoahOperatorType.Greater:
                    return (Value > t).AsKecaknoahBoolean();
                case KecaknoahOperatorType.Lesser:
                    return (Value < t).AsKecaknoahBoolean();
                case KecaknoahOperatorType.GreaterEqual:
                    return (Value >= t).AsKecaknoahBoolean();
                case KecaknoahOperatorType.LesserEqual:
                    return (Value <= t).AsKecaknoahBoolean();
                default:
                    throw new InvalidOperationException("この演算は対応していません。");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override object AsRawObject<T>()
        {
            var to = typeof(T);
            switch (to.Name)
            {
                case nameof(Byte):
                    return Convert.ToByte(Value);
                case nameof(SByte):
                    return Convert.ToSByte(Value);
                case nameof(Int16):
                    return Convert.ToInt16(Value);
                case nameof(UInt16):
                    return Convert.ToUInt16(Value);
                case nameof(Int32):
                    return Convert.ToInt32(Value);
                case nameof(UInt32):
                    return Convert.ToUInt32(Value);
                case nameof(Int64):
                    return Convert.ToInt64(Value);
                case nameof(UInt64):
                    return Convert.ToUInt64(Value);
                case nameof(Single):
                    return Value;
                case nameof(Double):
                    return Convert.ToDouble(Value);
                case nameof(Decimal):
                    return Convert.ToDecimal(Value);
                case nameof(String):
                    return Value.ToString();
                case nameof(Boolean):
                    return Convert.ToBoolean(Value);
            }

            return default(T);
        }
    }

    /// <summary>
    /// Kecaknoahでの倍精度浮動小数点数を定義します。
    /// </summary>
    public sealed class KecaknoahDouble : KecaknoahObject
    {
        /// <summary>
        /// 実際の値を取得・設定します。
        /// </summary>
        public double Value { get; set; } = 0;

        /// <summary>
        /// 最大値のインスタンスを取得します。
        /// </summary>
        public static KecaknoahDouble MaxValue = new KecaknoahDouble { Value = double.MaxValue };

        /// <summary>
        /// 最小値のインスタンスを取得します。
        /// </summary>
        public static KecaknoahDouble MinValue = new KecaknoahDouble { Value = double.MaxValue };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override KecaknoahObject GetMember(string name)
        {
            switch (name)
            {
                case "MAX_VALUE":
                    return MaxValue;
                case "MIN_VALUE":
                    return MinValue;
            }
            return base.GetMember(name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public override KecaknoahObject ExpressionOperation(KecaknoahOperatorType op, KecaknoahObject target)
        {
            var t = (double)target.AsRawObject<double>();
            switch (op)
            {
                case KecaknoahOperatorType.Plus:
                    return (Value + t).AsKecaknoahDouble();
                case KecaknoahOperatorType.Minus:
                    return (Value - t).AsKecaknoahDouble();
                case KecaknoahOperatorType.Multiply:
                    return (Value * t).AsKecaknoahDouble();
                case KecaknoahOperatorType.Divide:
                    return (Value / t).AsKecaknoahDouble();
                case KecaknoahOperatorType.Modular:
                    return (Value % t).AsKecaknoahDouble();
                case KecaknoahOperatorType.Assign:
                    return (Value = t).AsKecaknoahDouble();
                case KecaknoahOperatorType.Equal:
                    return (Value == t).AsKecaknoahBoolean();
                case KecaknoahOperatorType.NotEqual:
                    return (Value != t).AsKecaknoahBoolean();
                case KecaknoahOperatorType.Greater:
                    return (Value > t).AsKecaknoahBoolean();
                case KecaknoahOperatorType.Lesser:
                    return (Value < t).AsKecaknoahBoolean();
                case KecaknoahOperatorType.GreaterEqual:
                    return (Value >= t).AsKecaknoahBoolean();
                case KecaknoahOperatorType.LesserEqual:
                    return (Value <= t).AsKecaknoahBoolean();
                default:
                    throw new InvalidOperationException("この演算は対応していません。");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override object AsRawObject<T>()
        {
            var to = typeof(T);
            switch (to.Name)
            {
                case nameof(Byte):
                    return Convert.ToByte(Value);
                case nameof(SByte):
                    return Convert.ToSByte(Value);
                case nameof(Int16):
                    return Convert.ToInt16(Value);
                case nameof(UInt16):
                    return Convert.ToUInt16(Value);
                case nameof(Int32):
                    return Convert.ToInt32(Value);
                case nameof(UInt32):
                    return Convert.ToUInt32(Value);
                case nameof(Int64):
                    return Convert.ToInt64(Value);
                case nameof(UInt64):
                    return Convert.ToUInt64(Value);
                case nameof(Single):
                    return Convert.ToSingle(Value);
                case nameof(Double):
                    return Value;
                case nameof(Decimal):
                    return Convert.ToDecimal(Value);
                case nameof(String):
                    return Value.ToString();
                case nameof(Boolean):
                    return Convert.ToBoolean(Value);
            }

            return default(T);
        }
    }

    /// <summary>
    /// Kecaknoahでの文字列を定義します。
    /// </summary>
    public sealed class KecaknoahString : KecaknoahObject
    {
        /// <summary>
        /// 実際の値を取得・設定します。
        /// </summary>
        public string Value { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override KecaknoahObject GetMember(string name)
        {
            switch (name)
            {
                case "length":
                    return Value.Length.AsKecaknoahInteger();
                case "replace":
                    return new Func<string, string, string>(Value.Replace).AsKecaknoahDelegate().AsKecaknoahInteropFunction();
                case "substring":
                    return new Func<int, string>(Value.Substring).AsKecaknoahDelegate().AsKecaknoahInteropFunction();
            }
            return base.GetMember(name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public override KecaknoahObject ExpressionOperation(KecaknoahOperatorType op, KecaknoahObject target)
        {
            var t = (string)target.AsRawObject<string>();
            switch (op)
            {
                case KecaknoahOperatorType.Plus:
                    return (Value + t).AsKecaknoahString();
                case KecaknoahOperatorType.Equal:
                    return (Value == t).AsKecaknoahBoolean();
                case KecaknoahOperatorType.NotEqual:
                    return (Value != t).AsKecaknoahBoolean();
                case KecaknoahOperatorType.Greater:
                    return (Value.CompareTo(t) > 0).AsKecaknoahBoolean();
                case KecaknoahOperatorType.Lesser:
                    return (Value.CompareTo(t) < 0).AsKecaknoahBoolean();
                case KecaknoahOperatorType.GreaterEqual:
                    return (Value.CompareTo(t) >= 0).AsKecaknoahBoolean();
                case KecaknoahOperatorType.LesserEqual:
                    return (Value.CompareTo(t) <= 0).AsKecaknoahBoolean();
                default:
                    throw new InvalidOperationException("この演算は対応していません。");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override object AsRawObject<T>()
        {
            var to = typeof(T);
            switch (to.Name)
            {
                case nameof(Byte):
                    return Convert.ToByte(Value);
                case nameof(SByte):
                    return Convert.ToSByte(Value);
                case nameof(Int16):
                    return Convert.ToInt16(Value);
                case nameof(UInt16):
                    return Convert.ToUInt16(Value);
                case nameof(Int32):
                    return Convert.ToInt32(Value);
                case nameof(UInt32):
                    return Convert.ToUInt32(Value);
                case nameof(Int64):
                    return Convert.ToInt64(Value);
                case nameof(UInt64):
                    return Convert.ToUInt64(Value);
                case nameof(Single):
                    return Convert.ToSingle(Value);
                case nameof(Double):
                    return Convert.ToDouble(Value);
                case nameof(Decimal):
                    return Convert.ToDecimal(Value);
                case nameof(String):
                    return Value;
                case nameof(Boolean):
                    return Convert.ToBoolean(Value);
            }

            return default(T);
        }
    }

    /// <summary>
    /// Kecaknoahでの真偽値を定義します。
    /// </summary>
    public sealed class KecaknoahBoolean : KecaknoahObject
    {
        /// <summary>
        /// 実際の値を取得・設定します。
        /// </summary>
        public bool Value { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public override KecaknoahObject ExpressionOperation(KecaknoahOperatorType op, KecaknoahObject target)
        {
            var t = (bool)target.AsRawObject<bool>();
            switch(op)
            {
                case KecaknoahOperatorType.AndAlso:
                    return (Value && t).AsKecaknoahBoolean();
                case KecaknoahOperatorType.OrElse:
                    return (Value || t).AsKecaknoahBoolean();
                case KecaknoahOperatorType.And:
                    return (Value & t).AsKecaknoahBoolean();
                case KecaknoahOperatorType.Or:
                    return (Value | t).AsKecaknoahBoolean();
                default:
                    throw new InvalidOperationException("この演算は対応していません。");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override object AsRawObject<T>()
        {
            var to = typeof(T);
            switch (to.Name)
            {
                case nameof(Byte):
                    return Convert.ToByte(Value);
                case nameof(SByte):
                    return Convert.ToSByte(Value);
                case nameof(Int16):
                    return Convert.ToInt16(Value);
                case nameof(UInt16):
                    return Convert.ToUInt16(Value);
                case nameof(Int32):
                    return Convert.ToInt32(Value);
                case nameof(UInt32):
                    return Convert.ToUInt32(Value);
                case nameof(Int64):
                    return Convert.ToInt64(Value);
                case nameof(UInt64):
                    return Convert.ToUInt64(Value);
                case nameof(Single):
                    return Convert.ToSingle(Value);
                case nameof(Double):
                    return Convert.ToDouble(Value);
                case nameof(Decimal):
                    return Convert.ToDecimal(Value);
                case nameof(String):
                    return Value.ToString();
                case nameof(Boolean):
                    return Value;
            }

            return default(T);
        }
    }

    /// <summary>
    /// Kecaknoahでの.NET連携メソッドを定義します。
    /// </summary>
    public sealed class KecaknoahInteropFunction : KecaknoahObject
    {
        /// <summary>
        /// 呼び出し対象のデリゲートを取得・設定します。
        /// </summary>
        public KecaknoahDelegate Function { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public override KecaknoahObject Call(KecaknoahObject[] args) => Function(args);
    }

    /// <summary>
    /// Kecaknoahでの.NET連携メソッドの形式を定義します。
    /// </summary>
    /// <param name="args">実引数</param>
    /// <returns>返り値</returns>
    public delegate KecaknoahObject KecaknoahDelegate(KecaknoahObject[] args);
}
