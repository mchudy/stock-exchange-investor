using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StockExchange.Common
{
    public static class PagedListExtension
    {
        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> enumerable, int skip, int take)
        {
            return new PagedList<T>(enumerable, skip, take);
        }

        public static PagedList<T> ToPagedList<T>(this IQueryable<T> queryable, int skip, int take)
        {
            return new PagedList<T>(queryable, skip, take);
        }
    }

    public sealed class PagedList<T> : IReadOnlyList<T>
    {
        private readonly List<T> _list;

        public int TotalCount { get; private set; }

        public int Take { get; set; }

        public int Skip { get; set; }

        private PagedList(int skip, int take)
        {
            Skip = skip;
            Take = take;
        }

        public PagedList(IEnumerable<T> enumerable, int skip, int take) : this(skip, take)
        {
            var enumerable1 = enumerable as T[] ?? enumerable.ToArray();
            TotalCount = enumerable1.Length;
            _list = enumerable1.Skip(skip).Take(take).ToList();
        }

        public PagedList(IQueryable<T> enumerable, int skip, int take) : this(skip, take)
        {
            TotalCount = enumerable.Count();
            _list = enumerable.Skip(skip).Take(take).ToList();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => _list.Count;

        public T this[int index] => _list[index];
    }
}
