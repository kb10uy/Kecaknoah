using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah.Type
{
    /// <summary>
    /// Kecaknoahで定義されるクラスを定義します。
    /// </summary>
    public sealed class KecaknoahScriptClassInfo : KecaknoahClassInfo
    {
        /// <summary>
        /// このクラスの名前を取得します。
        /// </summary>
        public override string Name { get; }

        internal List<KecaknoahScriptClassInfo> inners = new List<KecaknoahScriptClassInfo>();
        /// <summary>
        /// このクラスのインナークラスを取得します。
        /// </summary>
        public override IReadOnlyList<KecaknoahClassInfo> InnerClasses { get; }

        internal List<KecaknoahScriptMethodInfo> methods = new List<KecaknoahScriptMethodInfo>();
        /// <summary>
        /// このクラスのインナークラスを取得します。
        /// </summary>
        public override IReadOnlyList<KecaknoahMethodInfo> InstanceMethods => methods;

        private List<string> locals = new List<string>();
        /// <summary>
        /// このクラスのフィールドを取得します。
        /// </summary>
        public override IReadOnlyList<string> Locals => locals;

        /// <summary>
        /// 継承元のクラスを取得・設定します。
        /// </summary>
        public override KecaknoahClassInfo BaseClass { get; }

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="name">クラス名</param>
        public KecaknoahScriptClassInfo(string name)
        {
            Name = name;
        }

        /// <summary>
        /// インナークラスを追加します。
        /// </summary>
        /// <param name="klass">追加するクラス</param>
        internal void AddInnerClass(KecaknoahScriptClassInfo klass)
        {
            if (inners.Any(p => p.Name == klass.Name)) throw new ArgumentException("同じ名前のインナークラスがすでに存在します。");
            inners.Add(klass);
        }

        /// <summary>
        /// メソッドを追加します。
        /// </summary>
        /// <param name="method">追加するメソッド</param>
        internal void AddMethod(KecaknoahScriptMethodInfo method)
        {
            methods.Add(method);
        }

        /// <summary>
        /// フィールドを追加します。
        /// </summary>
        /// <param name="local">追加するメソッド</param>
        internal void AddLocal(string local)
        {
            locals.Add(local);
        }
    }
}
