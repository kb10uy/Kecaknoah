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
    /// Kecaknoahで正規表現を扱います。
    /// </summary>
    public sealed class KecaknoahRegex : KecaknoahObject
    {
        public static readonly string ClassName = "Regex";
        internal static KecaknoahInteropClassInfo Information { get; } = new KecaknoahInteropClassInfo(ClassName);
        private Regex regex;

        #region overrideメンバー
        static KecaknoahRegex()
        {
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("new", ClassNew));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("match", ClassMatch));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("replace", ClassReplace));
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("split", ClassSplit));
        }


        public KecaknoahRegex(string pattern)
        {
            regex = new Regex(pattern);
            ExtraType = ClassName;
            RegisterInstanceMembers();
        }

        protected internal override KecaknoahReference GetMemberReference(string name)
        {
            switch (name)
            {
                case "match": return i_match;
                case "split": return i_split;

                default: return base.GetMemberReference(name);
            }
        }

        #endregion

        #region インスタンスメンバー
        private KecaknoahReference i_match, i_split;

        private void RegisterInstanceMembers()
        {
            i_match = KecaknoahReference.Right(this, InstanceMatch);
            i_split = KecaknoahReference.Right(this, InstanceSplit);

        }

        private KecaknoahFunctionResult InstanceMatch(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            Match result;
            switch (args.Length)
            {
                case 1:
                    result = regex.Match(args[0].ToString());
                    break;
                case 2:
                    result = regex.Match(args[0].ToString(), args[1].ToInt32());
                    break;
                case 3:
                    result = regex.Match(args[0].ToString(), args[1].ToInt32(), args[2].ToInt32());
                    break;
                default:
                    result = null;
                    break;
            }
            return new KecaknoahMatch(result).NoResume();
        }

        private KecaknoahFunctionResult InstanceSplit(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = regex.Split(args[0].ToString());
            return result.ToKecaknoahArray().NoResume();
        }

        #endregion
        #region クラスメソッド

        private static KecaknoahFunctionResult ClassNew(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args) => new KecaknoahRegex(args[0].ToString()).NoResume();

        private static KecaknoahFunctionResult ClassMatch(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            //TODO: matchメソッドの処理を記述してください
            var result = Regex.Match(args[0].ToString(), args[1].ToString());
            return new KecaknoahMatch(result).NoResume();
        }

        private static KecaknoahFunctionResult ClassReplace(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var sa = args.ExpectString(3, false);
            var result = Regex.Replace(sa[0], sa[1], sa[2]);
            return result.AsKecaknoahString().NoResume();
        }

        private static KecaknoahFunctionResult ClassSplit(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var sa = args.ExpectString(3, false);
            var result = Regex.Split(sa[0], sa[1]);
            return result.ToKecaknoahArray().NoResume();
        }

        #endregion
    }
#pragma warning restore 1591
}
