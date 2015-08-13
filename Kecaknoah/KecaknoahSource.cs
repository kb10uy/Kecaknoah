using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kecaknoah
{
    /// <summary>
    /// 1つのソースコードを元にしたクラスとメソッドの集合体を定義します。
    /// </summary>
    public sealed class KecaknoahSource
    {
        private List<KecaknoahClass> classes = new List<KecaknoahClass>();
        /// <summary>
        /// クラスを取得します。
        /// </summary>
        public IReadOnlyList<KecaknoahClass> Classes { get; }

        private List<KecaknoahMethod> methods = new List<KecaknoahMethod>();
        /// <summary>
        /// トップレベルに定義されたメソッドを取得します。
        /// </summary>
        public IReadOnlyList<KecaknoahMethod> GlobalMethods { get; }
    }

    /// <summary>
    /// Kecaknoahで定義されるクラスを定義します。
    /// </summary>
    public sealed class KecaknoahClass
    {
        /// <summary>
        /// このクラスの名前を取得します。
        /// </summary>
        public string Name { get; }

        private List<KecaknoahClass> inners = new List<KecaknoahClass>();
        /// <summary>
        /// このクラスのインナークラスを取得します。
        /// </summary>
        public IReadOnlyList<KecaknoahClass> InnerClasses { get; }

        private List<KecaknoahMethod> methods = new List<KecaknoahMethod>();
        /// <summary>
        /// このクラスのインナークラスを取得します。
        /// </summary>
        public IReadOnlyList<KecaknoahMethod> Methods { get; }

        private List<string> locals = new List<string>();
        /// <summary>
        /// このクラスのフィールドを取得します。
        /// </summary>
        public IReadOnlyList<string> Locals { get; }

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="name">クラス名</param>
        public KecaknoahClass(string name)
        {
            Name = name;
            InnerClasses = inners;
            Methods = methods;
        }

        /// <summary>
        /// インナークラスを追加します。
        /// </summary>
        /// <param name="klass">追加するクラス</param>
        internal void AddInnerClass(KecaknoahClass klass)
        {
            if (inners.Any(p => p.Name == klass.Name)) throw new ArgumentException("同じ名前のインナークラスがすでに存在します。");
            inners.Add(klass);
        }

        /// <summary>
        /// メソッドを追加します。
        /// </summary>
        /// <param name="method">追加するメソッド</param>
        internal void AddMethod(KecaknoahMethod method)
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

    /// <summary>
    /// Kecaknoahで定義されるメソッドを定義します。
    /// </summary>
    public sealed class KecaknoahMethod
    {
        /// <summary>
        /// このメソッドの名前を取得します。
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 引数の数を取得します。
        /// </summary>
        public int ArgumentLength { get; }

        /// <summary>
        /// 可変長引数メソッドかどうかを取得します。
        /// </summary>
        public bool VariableArgument { get; }

        /// <summary>
        /// このメソッドの<see cref="KecaknoahIL"/>を取得します。
        /// </summary>
        public KecaknoahIL Codes { get; set; }

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="name">メソッド名</param>
        /// <param name="length">引数の数</param>
        /// <param name="vargs">可変長引数の場合はtrue</param>
        public KecaknoahMethod(string name,int length,bool vargs)
        {
            Name = name;
            ArgumentLength = length;
            VariableArgument = vargs;
        }

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="name">メソッド名</param>
        public KecaknoahMethod(string name)
        {
            Name = name;
            ArgumentLength = 0;
            VariableArgument = false;
        }
    }
}
