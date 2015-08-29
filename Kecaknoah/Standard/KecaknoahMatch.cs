using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Kecaknoah.Type;
using System.Text.RegularExpressions;

namespace Kecaknoah.Standard
{
#pragma warning disable 1591
    /// <summary>
    /// <see cref="KecaknoahRegex"/>の結果を定義します。
    /// </summary>
    public sealed class KecaknoahMatch : KecaknoahObject
    {
        public static readonly string ClassName = "Match";
        internal static KecaknoahInteropClassInfo Information { get; } = new KecaknoahInteropClassInfo(ClassName);
        Match refm;

        #region overrideメンバー
        public KecaknoahMatch(Match match)
        {
            ExtraType = ClassName;
            refm = match;

            f_success = KecaknoahReference.Right(refm.Success);
            f_length = KecaknoahReference.Right(refm.Length);
            f_index = KecaknoahReference.Right(refm.Index);
            f_value = KecaknoahReference.Right(refm.Value);
            f_captures = KecaknoahReference.Right(new KecaknoahRegexCaptures(refm.Captures));
            f_groups = KecaknoahReference.Right(new KecaknoahRegexGroups(refm.Groups));
            RegisterInstanceMembers();
        }

        protected internal override KecaknoahReference GetMemberReference(string name)
        {
            switch (name)
            {

                case "success": return f_success;
                case "length": return f_length;
                case "index": return f_index;
                case "value": return f_value;
                case "captures": return f_captures;
                case "groups": return f_groups;
                default: return base.GetMemberReference(name);
            }
        }

        #endregion

        #region インスタンスメンバー
        private KecaknoahReference f_success, f_length, f_index, f_value, f_captures, f_groups;

        private void RegisterInstanceMembers()
        {

        }

        #endregion
    }

    /// <summary>
    /// <see cref="KecaknoahRegex"/>のキャプチャリストを定義します。
    /// </summary>
    public sealed class KecaknoahRegexCaptures : KecaknoahObject
    {
        public static readonly string ClassName = "RegexCaptures";
        CaptureCollection cc;
        List<KecaknoahReference> captures;

        public KecaknoahRegexCaptures(CaptureCollection col)
        {
            cc = col;
            captures = new List<KecaknoahReference>();
            for (int i = 0; i < cc.Count; i++) captures.Add(KecaknoahReference.Right(new KecaknoahRegexCapture(cc[i])));
        }

        protected internal override KecaknoahReference GetIndexerReference(KecaknoahObject[] indices)
        {
            var i = indices[0].ToInt32();
            return captures[i];
        }
    }

    /// <summary>
    /// <see cref="KecaknoahRegex"/>のキャプチャを定義します。
    /// </summary>
    public sealed class KecaknoahRegexCapture : KecaknoahObject
    {
        public static readonly string ClassName = "RegexCapture";
        Capture cap;
        KecaknoahReference length, index, value;

        internal KecaknoahRegexCapture(Capture c)
        {
            cap = c;
            length = KecaknoahReference.Right(cap.Length);
            index = KecaknoahReference.Right(cap.Index);
            value = KecaknoahReference.Right(cap.Value);
        }

        protected internal override KecaknoahReference GetMemberReference(string name)
        {
            switch (name)
            {
                case nameof(length): return length;
                case nameof(value): return value;
                case nameof(index): return index;
            }
            return base.GetMemberReference(name);
        }
    }

    /// <summary>
    /// <see cref="KecaknoahRegex"/>のグループリストを定義します。
    /// </summary>
    public sealed class KecaknoahRegexGroups : KecaknoahObject
    {
        public static readonly string ClassName = "RegexGroups";
        GroupCollection cc;
        List<KecaknoahReference> groups;

        public KecaknoahRegexGroups(GroupCollection col)
        {
            cc = col;
            groups = new List<KecaknoahReference>();
            for (int i = 0; i < cc.Count; i++) groups.Add(KecaknoahReference.Right(new KecaknoahRegexGroup(cc[i])));
        }

        protected internal override KecaknoahReference GetIndexerReference(KecaknoahObject[] indices)
        {
            var i = indices[0].ToInt32();
            return groups[i];
        }
    }

    /// <summary>
    /// <see cref="KecaknoahRegex"/>のグループを定義します。
    /// </summary>
    public sealed class KecaknoahRegexGroup : KecaknoahObject
    {
        public static readonly string ClassName = "RegexGroup";
        Group gr;
        KecaknoahReference length, index, value;

        internal KecaknoahRegexGroup(Group g)
        {
            gr = g;
            length = KecaknoahReference.Right(gr.Length);
            index = KecaknoahReference.Right(gr.Index);
            value = KecaknoahReference.Right(gr.Value);
        }

        protected internal override KecaknoahReference GetMemberReference(string name)
        {
            switch (name)
            {
                case nameof(length): return length;
                case nameof(value): return value;
                case nameof(index): return index;
            }
            return base.GetMemberReference(name);
        }
    }
#pragma warning restore 1591
}
