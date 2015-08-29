using Kecaknoah.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah.Standard
{
#pragma warning disable 1591
    /// <summary>
    /// Kecaknoahでデータ集合を定義するハッシュを定義します。
    /// </summary>
    public sealed class KecaknoahHash : KecaknoahObject
    {
        public static readonly string ClassName = "Hash";

        #region 改変不要
        internal static KecaknoahInteropClassInfo Information { get; } = new KecaknoahInteropClassInfo(ClassName);
        #endregion

        Dictionary<string, KecaknoahReference> members = new Dictionary<string, KecaknoahReference>();

        #region overrideメンバー

        static KecaknoahHash()
        {
            Information.AddClassMethod(new KecaknoahInteropMethodInfo("new", ClassNew));
        }

        public KecaknoahHash()
        {
            ExtraType = ClassName;
        }

        protected internal override KecaknoahReference GetMemberReference(string name)
        {
            if (!members.ContainsKey(name))
            {
                members[name] = new KecaknoahReference { IsLeftValue = true, RawObject = KecaknoahNil.Instance };
            }
            return members[name];
        }

        protected internal override KecaknoahReference GetIndexerReference(KecaknoahObject[] indices)
        {
            var name = indices[0].ToString();
            return GetMemberReference(name);
        }

        public override KecaknoahObject AsByValValue() => base.AsByValValue();
        
        
        public override int GetHashCode() => members.GetHashCode();
        #endregion


        #region クラスメソッド
        

        private static KecaknoahFunctionResult ClassNew(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args) => new KecaknoahInteropClassBase().NoResume();
        #endregion
    }
#pragma warning restore 1591
}
