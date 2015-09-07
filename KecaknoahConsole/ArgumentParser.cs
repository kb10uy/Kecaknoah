using System;
using System.Collections.Generic;
using System.Linq;

namespace ArgumentParser
{
    /// <summary>
    /// コマンドライン引数をgetopt風にパースします。
    /// </summary>
    public sealed class ArgumentParser
    {
        #region Property
        /// <summary>
        /// 現在設定されているオプションを取得します。
        /// </summary>
        public IList<ArgumentOption> Options { get; } = new List<ArgumentOption>();

        /// <summary>
        /// --以降をオプションとしてパースしないかどうかを取得・設定します。
        /// </summary>
        public bool EnableNoParseElement { get; set; } = true;

        /// <summary>
        /// 不明なオプションを受け取った場合に呼ばれるコールバックを取得・設定します。
        /// <para>
        /// 不明なオプション、その次の文字列を受け取り、次の文字列を消費する場合はtrue、
        /// そうでない場合はfalseを返します。
        /// </para>
        /// </summary>
        public Func<string, string, bool> UndefinedOptionCallback { get; set; }
        #endregion

        #region Registerer
        /// <summary>
        /// 引数を持たないオプションを追加します。
        /// </summary>
        /// <param name="shortName">短い形式のオプション名。先頭の-は含みません。</param>
        /// <param name="callback">オプションが指定された際に呼ばれるデリゲート。</param>
        public void AddOption(string shortName, Action<ArgumentOption, object> callback)
        {
            Options.Add(new ArgumentOption { ShortName = shortName, Callback = callback });
        }

        /// <summary>
        /// 引数を持たないオプションを追加します。
        /// </summary>
        /// <param name="shortName">短い形式のオプション名。先頭の-は含みません。</param>
        /// <param name="longName">長い形式のオプション名。先頭の--は含みません。</param>
        /// <param name="callback">オプションが指定された際に呼ばれるデリゲート。</param>
        public void AddOption(string shortName, string longName, Action<ArgumentOption, object> callback)
        {
            Options.Add(new ArgumentOption { ShortName = shortName, LongName = longName, Callback = callback });
        }

        /// <summary>
        /// 文字列の引数を持つオプションを追加します。
        /// </summary>
        /// <param name="shortName">短い形式のオプション名。先頭の-は含みません。</param>
        /// <param name="callback">オプションが指定された際に呼ばれるデリゲート。</param>
        public void AddStringOption(string shortName, Action<ArgumentOption, object> callback)
        {
            Options.Add(new ArgumentOption { ShortName = shortName, Callback = callback, ParameterType = TypeCode.String });
        }

        /// <summary>
        /// 文字列の引数を持つオプションを追加します。
        /// </summary>
        /// <param name="shortName">短い形式のオプション名。先頭の-は含みません。</param>
        /// <param name="longName">長い形式のオプション名。先頭の--は含みません。</param>
        /// <param name="callback">オプションが指定された際に呼ばれるデリゲート。</param>
        public void AddStringOption(string shortName, string longName, Action<ArgumentOption, object> callback)
        {
            Options.Add(new ArgumentOption { ShortName = shortName, LongName = longName, Callback = callback, ParameterType = TypeCode.String });
        }

        /// <summary>
        /// 整数の引数を持つオプションを追加します。
        /// </summary>
        /// <param name="shortName">短い形式のオプション名。先頭の-は含みません。</param>
        /// <param name="callback">オプションが指定された際に呼ばれるデリゲート。</param>
        public void AddInt32Option(string shortName, Action<ArgumentOption, object> callback)
        {
            Options.Add(new ArgumentOption { ShortName = shortName, Callback = callback, ParameterType = TypeCode.Int32 });
        }

        /// <summary>
        /// 整数の引数を持つオプションを追加します。
        /// </summary>
        /// <param name="shortName">短い形式のオプション名。先頭の-は含みません。</param>
        /// <param name="longName">長い形式のオプション名。先頭の--は含みません。</param>
        /// <param name="callback">オプションが指定された際に呼ばれるデリゲート。</param>
        public void AddInt32Option(string shortName, string longName, Action<ArgumentOption, object> callback)
        {
            Options.Add(new ArgumentOption { ShortName = shortName, LongName = longName, Callback = callback, ParameterType = TypeCode.Int32 });
        }

        /// <summary>
        /// 引数を持つオプションを追加します。
        /// </summary>
        /// <param name="shortName">短い形式のオプション名。先頭の-は含みません。</param>
        /// <param name="code">受け取る型の<see cref="TypeCode"/>。</param>
        /// <param name="callback">オプションが指定された際に呼ばれるデリゲート。</param>
        public void AddParameterOption(string shortName, TypeCode code, Action<ArgumentOption, object> callback)
        {
            Options.Add(new ArgumentOption { ShortName = shortName, Callback = callback, ParameterType = code });
        }

        /// <summary>
        /// 引数を持つオプションを追加します。
        /// </summary>
        /// <param name="shortName">短い形式のオプション名。先頭の-は含みません。</param>
        /// <param name="longName">長い形式のオプション名。先頭の--は含みません。</param>
        /// <param name="code">受け取る型の<see cref="TypeCode"/>。</param>
        /// <param name="callback">オプションが指定された際に呼ばれるデリゲート。</param>
        public void AddParameterOption(string shortName, string longName, TypeCode code, Action<ArgumentOption, object> callback)
        {
            Options.Add(new ArgumentOption { ShortName = shortName, LongName = longName, Callback = callback, ParameterType = code });
        }
        #endregion

        #region --help
        private void ShowHelp(ArgumentOption option, object param)
        {
            Console.WriteLine("Options:");
            foreach (var i in Options)
            {
                Console.Write("    ");
                if (i.ShortName != "") Console.Write($"-{i.ShortName} ");
                if (i.LongName != "") Console.Write($"-{i.LongName} ");
                Console.WriteLine();
            }
            Environment.Exit(0);
        }
        #endregion

        #region Parsing
        /// <summary>
        /// コマンドライン引数をパースします。
        /// </summary>
        /// <param name="args">指定された引数</param>
        /// <returns>
        /// パースされずに残った引数。
        /// <see cref="EnableNoParseElement"/>がtrueの場合の後続の引数を含みます。
        /// </returns>
        public IList<string> Parse(IList<string> args)
        {
            var rest = new List<string>();
            var aq = new Queue<string>(args);
            while (aq.Count > 0)
            {
                var now = aq.Dequeue();
                if (now == "--")
                {
                    if (!EnableNoParseElement) continue;
                    rest.AddRange(aq);
                    return rest;
                }
                else if (now.StartsWith("--"))
                {
                    var opt = Options.FirstOrDefault(p => p.LongName == now.Substring(2));
                    if (opt == null && UndefinedOptionCallback != null)
                    {
                        var next = aq.Count > 0 ? aq.Peek() : "";
                        if (UndefinedOptionCallback(now, next)) aq.Dequeue();
                    }
                    else
                    {
                        if (opt.Callback != null)
                            opt.Callback(opt, (opt.ParameterType != TypeCode.Empty) ? ConvertString(aq.Dequeue(), opt.ParameterType) : null);
                    }
                }
                else if (now.StartsWith("-"))
                {
                    var opt = Options.FirstOrDefault(p => p.ShortName == now.Substring(1));
                    if (opt == null && UndefinedOptionCallback != null)
                    {
                        var next = aq.Count > 0 ? aq.Peek() : "";
                        if (UndefinedOptionCallback(now, next)) aq.Dequeue();
                    }
                    else
                    {
                        if (opt.Callback != null)
                            opt.Callback(opt, (opt.ParameterType != TypeCode.Empty) ? ConvertString(aq.Dequeue(), opt.ParameterType) : null);
                    }
                }
                else
                {
                    rest.Add(now);
                }
            }
            return rest;
        }

        private static object ConvertString(string str, TypeCode code)
        {
            switch (code)
            {
                case TypeCode.Boolean: return Convert.ToBoolean(str);
                case TypeCode.Byte: return Convert.ToByte(str);
                case TypeCode.Char: return Convert.ToChar(str);
                case TypeCode.DateTime: return Convert.ToDateTime(str);
                case TypeCode.Decimal: return Convert.ToDecimal(str);
                case TypeCode.Double: return Convert.ToDouble(str);
                case TypeCode.Int16: return Convert.ToInt16(str);
                case TypeCode.Int32: return Convert.ToInt32(str);
                case TypeCode.Int64: return Convert.ToInt64(str);
                case TypeCode.SByte: return Convert.ToSByte(str);
                case TypeCode.Single: return Convert.ToSingle(str);
                case TypeCode.String: return str;
                case TypeCode.UInt16: return Convert.ToUInt16(str);
                case TypeCode.UInt32: return Convert.ToUInt32(str);
                case TypeCode.UInt64: return Convert.ToUInt64(str);
                default: return null;
            }
        }
        #endregion

        public ArgumentParser()
        {
            AddOption("h", "help", ShowHelp);
        }
    }

    /// <summary>
    /// 一つのオプションを定義します。
    /// </summary>
    public sealed class ArgumentOption
    {
        /// <summary>
        /// 短い形式の名前を取得します。
        /// </summary>
        public string ShortName { get; set; } = "";

        /// <summary>
        /// 長い形式の名前を取得します。
        /// </summary>
        public string LongName { get; set; } = "";

        /// <summary>
        /// 受け取る引数の<see cref="TypeCode"/>を取得します。
        /// </summary>
        public TypeCode ParameterType { get; set; } = TypeCode.Empty;

        /// <summary>
        /// オプションが検出された時に呼ばれるデリゲートを取得します。
        /// </summary>
        public Action<ArgumentOption, object> Callback { get; set; }
    }
}
