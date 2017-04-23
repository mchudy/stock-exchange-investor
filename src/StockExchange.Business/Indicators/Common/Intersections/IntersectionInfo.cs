using System;

namespace StockExchange.Business.Indicators.Common.Intersections
{
    internal class IntersectionInfo
    {
        internal decimal Start1 { get; set; }
        internal decimal End1 { get; set; }
        internal decimal Start2 { get; set; }
        internal decimal End2 { get; set; }
        internal DateTime Date { get; set; }
        internal IntersectionType IntersectionType { get; set; }
    }
}