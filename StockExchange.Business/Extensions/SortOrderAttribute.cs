using System;

namespace StockExchange.Business.Extensions
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SortOrderAttribute : Attribute
    {
        public string[] Orders { get; private set; }

        public SortOrderAttribute(params string[] orders)
        {
            Orders = orders;
        }
    }
}
