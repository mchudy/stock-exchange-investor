using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockExchange.Business.Models.Indicators;

namespace StockExchange.Business.Indicators.Common
{
    public enum IntersectionType
    {
        FirstAbove,
        SecondAbove,
        Same
    }

    public class IntersectionInfo
    {
        public decimal Start1 { get; set; }
        public decimal End1 { get; set; }
        public decimal Start2 { get; set; }
        public decimal End2 { get; set; }
        public DateTime Date { get; set; }
        public IntersectionType IntersectionType { get; set; }
    }

    public static class IntersectionHelper
    {
        public static IList<IntersectionInfo> FindIntersections(IList<IndicatorValue> line1, IList<IndicatorValue> line2)
        {
            var doubleLine = line1.Select((t, i) => new DoubleLineIndicatorValue()
            {
                Date = t.Date, Value = t.Value, SecondLineValue = line2[i].Value
            }).ToList();
            return FindIntersections(doubleLine);
        } 

        public static IList<IntersectionInfo> FindIntersections(IList<DoubleLineIndicatorValue> doubleLineValues)
        {
            var intersections = new List<IntersectionInfo>();
            var previousValue = doubleLineValues[0];
            for (int i = 1; i < doubleLineValues.Count; i++)
            {
                // we consider 2 lines - indicator and signal line
                // with equations 
                // y = (curr.* - prev.*)x + prev.* (y = Ax+B and y=Cx+D) - * may be Value or SecondLineValue
                // intersection exists when their difference
                // has value 0 somewhere, thus (C-A)x=B-D -> a=C-A, b=B-D
                // intersection exists if a=b=0 (lines overlap -> NOSIGNAL) or (x=B/A and 0<x<1)
                // if MACD intersects signal line upside -> BUY otherwise SELL
                var currentValue = doubleLineValues[i];
                decimal a = currentValue.SecondLineValue - previousValue.SecondLineValue - currentValue.Value +
                            previousValue.Value;
                decimal b = previousValue.Value - currentValue.Value;
                if ((a == 0 && b == 0) || (a * b > 0 && b < a))   // intersection
                {
                    decimal diff = previousValue.Value - previousValue.SecondLineValue;     // B - D
                    intersections.Add(new IntersectionInfo()
                    {
                        Date = currentValue.Date,
                        Start1 = previousValue.Value,
                        End1 = currentValue.Value,
                        Start2 = previousValue.SecondLineValue,
                        End2 = currentValue.SecondLineValue,
                        IntersectionType = (diff == 0) ? IntersectionType.Same : (diff < 0 ? IntersectionType.FirstAbove : IntersectionType.SecondAbove)
                    });
                }
                previousValue = currentValue;
            }
            return intersections;
        }
    }
}
