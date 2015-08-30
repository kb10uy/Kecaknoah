using System;
using System.Collections.Generic;
using System.Linq;

namespace Kecaknoah.Type
{
    /// <summary>
    /// 配列を提供します。
    /// </summary>
    public sealed class KecaknoahArray : KecaknoahObject
    {
        internal List<KecaknoahReference> array;

        private KecaknoahReference length;
        private KecaknoahReference find, each, filter, map, reduce, copy;//, first, last;

        internal KecaknoahArray(int[] dim)
        {
            Type = TypeCode.Object;
            ExtraType = "Array";
            array = new List<KecaknoahReference>();
            length = KecaknoahReference.Right(dim[0]);
            InitializeMembers();
            for (int i = 0; i < dim[0]; i++) array.Add(KecaknoahReference.Left(KecaknoahNil.Instance));
            if (dim.Length == 1) return;
            for (int i = 0; i < array.Count; i++) array[i] = KecaknoahReference.Left(new KecaknoahArray(dim.Skip(1).ToArray()));
        }

        internal KecaknoahArray(List<KecaknoahReference> arr)
        {
            Type = TypeCode.Object;
            array = arr;
            length = KecaknoahReference.Right(arr.Count);
            InitializeMembers();
        }

        /// <summary>
        /// 配列の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="arr">生成する対象の<see cref="KecaknoahObject"/>のリスト</param>
        public KecaknoahArray(IEnumerable<KecaknoahObject> arr)
        {
            Type = TypeCode.Object;
            array = new List<KecaknoahReference>();
            foreach (var i in arr) array.Add(KecaknoahReference.Left(i));
            length = KecaknoahReference.Right(arr.Count());
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            each = KecaknoahReference.Right(this, InstanceEach);
            find = KecaknoahReference.Right(this, InstanceFind);
            filter = KecaknoahReference.Right(this, InstanceFilter);
            map = KecaknoahReference.Right(this, InstanceMap);
            reduce = KecaknoahReference.Right(this, InstanceReduce);
            copy = KecaknoahReference.Right(this, InstanceCopy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected internal override KecaknoahReference GetMemberReference(string name)
        {
            switch (name)
            {
                case nameof(length):
                    return length;
                case nameof(each):
                    return each;
                case nameof(find):
                    return find;
                case nameof(filter):
                    return filter;
                case nameof(map):
                    return map;
                case nameof(reduce):
                    return reduce;
                case nameof(copy):
                    return copy;
                /*
            case nameof(first):
                return first;
            case nameof(last):
                return last;
                */
                default:
                    return base.GetMemberReference(name);
            }

        }

        /// <summary>
        /// 配列要素の参照を取得します。
        /// </summary>
        /// <param name="indices"></param>
        /// <returns></returns>
        protected internal override KecaknoahReference GetIndexerReference(KecaknoahObject[] indices)
        {
            if (indices.Length != 1) throw new ArgumentException("配列のインデックスの数は必ず1です。");
            return array[(int)indices[0].ToInt64()];
        }

        private KecaknoahFunctionResult InstanceCopy(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = new List<KecaknoahObject>();
            foreach (var i in array) result.Add(i.RawObject.AsByValValue());
            return new KecaknoahArray(result).NoResume();
        }

        private KecaknoahFunctionResult InstanceEach(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            foreach (var i in array) args[0].Call(ctx, new[] { i.RawObject });
            return KecaknoahNil.Instance.NoResume();
        }

        private KecaknoahFunctionResult InstanceFind(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = array.FindIndex(p => p.RawObject.ExpressionOperation(KecaknoahILCodeType.Equal, args[0]).ToBoolean());
            return result.AsKecaknoahInteger().NoResume();
        }

        private KecaknoahFunctionResult InstanceFilter(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = array.Where(p => args[0].Call(ctx, new[] { p.RawObject }).ReturningObject.ToBoolean());
            return new KecaknoahArray(result.ToList()).NoResume();
        }

        private KecaknoahFunctionResult InstanceMap(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = array.Select(p => args[0].Call(ctx, new[] { p.RawObject }).ReturningObject);
            return new KecaknoahArray(result.ToList()).NoResume();
        }

        private KecaknoahFunctionResult InstanceReduce(KecaknoahContext ctx, KecaknoahObject self, KecaknoahObject[] args)
        {
            var result = array.Aggregate((p, q) => KecaknoahReference.Right(args[0].Call(ctx, new[] { p.RawObject, q.RawObject }).ReturningObject));
            return result.RawObject.NoResume();
        }

#pragma warning disable 1591
        public override string ToString() => $"Array: {array.Count} elements";
        public override bool Equals(object obj)
        {
            var ar = obj as KecaknoahArray;
            if (ar == null) return false;
            return ar.array == array;
        }
        public override int GetHashCode() => array.GetHashCode();
#pragma warning restore 1591
    }
}
