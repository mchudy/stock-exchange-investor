using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace StockExchange.Indicators.Lines
{
    public class PointLine
    {
        public IList<decimal> Values { get; }

        public PointLine(IEnumerable<decimal> values)
        {
            if(values == null)
                throw new ArgumentNullException(nameof(values));
            Values = values.ToList();
        }

        public static PointLine operator +(PointLine line1, PointLine line2)
        {
            if(line1 == null || line2 == null || line1.Values.Count != line2.Values.Count)
                throw new ArgumentException();
            var list = new List<decimal>();
            for(int i=0; i<line1.Values.Count; i++)
                list.Add(line1.Values[i]+line2.Values[i]);
            return new PointLine(list);
        }

        public static PointLine operator -(PointLine line1, PointLine line2)
        {
            if (line1 == null || line2 == null || line1.Values.Count != line2.Values.Count)
                throw new ArgumentException();
            var list = new List<decimal>();
            for (int i = 0; i < line1.Values.Count; i++)
                list.Add(line1.Values[i] - line2.Values[i]);
            return new PointLine(list);
        }
    }
}
