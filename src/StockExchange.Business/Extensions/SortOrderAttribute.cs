using System;

namespace StockExchange.Business.Extensions
{
    /// <summary>
    /// Specifies the sort order for the property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SortOrderAttribute : Attribute
    {
        /// <summary>
        /// Creates a new instance of <see cref="SortOrderAttribute"/>
        /// </summary>
        /// <param name="orders">Names of properties to sort by</param>
        public SortOrderAttribute(params string[] orders)
        {
            Orders = orders;
        }

        /// <summary>
        /// Names of properties to sort by
        /// </summary>
        public string[] Orders { get; private set; }
    }
}
