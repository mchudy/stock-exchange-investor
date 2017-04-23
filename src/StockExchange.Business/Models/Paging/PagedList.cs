using System.Collections.Generic;

namespace StockExchange.Business.Models.Paging
{
    /// <summary>
    /// Represents a paged list of objects
    /// </summary>
    /// <typeparam name="T">Type of objects in the list</typeparam>
    public sealed class PagedList<T>
    {
        /// <summary>
        /// List of objects
        /// </summary>
        public List<T> List { get; set; }

        /// <summary>
        /// Total number of items in the whole list
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Number of items included in the paged list
        /// </summary>
        public int Take { get; set; }

        /// <summary>
        /// Number of items skipped from the whole list
        /// </summary>
        public int Skip { get; set; }

        /// <summary>
        /// Number of items in the paged list
        /// </summary>
        public int Count => List.Count;

        /// <summary>
        /// Returns the item at the given index from the paged list
        /// </summary>
        /// <param name="index">The index of the element</param>
        /// <returns>The item at the given index</returns>
        public T this[int index] => List[index];
    }
}