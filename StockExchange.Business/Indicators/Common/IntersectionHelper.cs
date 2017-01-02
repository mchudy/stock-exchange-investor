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
                // we consider 2 lines - indicator (y=ax+b) and signal (y=cx+d) line
                // with equations 
                // y = (curr.* - prev.*)x + prev.* (y = Ax+B and y=Cx+D) - * may be Value or SecondLineValue
                // intersection exists when their difference
                // has value 0 somewhere, thus (c-a)x=b-d. Let e=c-a, f=b-d
                // intersection exists if e=f=0 (lines overlap -> NOSIGNAL) or (x=f/e and 0<x<1)
                // if MACD intersects signal line upside -> BUY otherwise SELL
                var currentValue = doubleLineValues[i];
                var a = currentValue.Value - previousValue.Value;
                var c = currentValue.SecondLineValue - previousValue.SecondLineValue;
                var b = previousValue.Value;
                var d = previousValue.SecondLineValue;
                decimal e = a - c;
                decimal f = b - d; //diff: y=ex+f
                if ((e == 0 && f == 0) || (e*f < 0 && e*(e+f)>0))   // intersection
                {
                    intersections.Add(new IntersectionInfo()
                    {
                        Date = currentValue.Date,
                        Start1 = previousValue.Value,
                        End1 = currentValue.Value,
                        Start2 = previousValue.SecondLineValue,
                        End2 = currentValue.SecondLineValue,
                        IntersectionType = (f == 0) ? IntersectionType.Same : (f < 0 ? IntersectionType.FirstAbove : IntersectionType.SecondAbove)
                    });
                }
                previousValue = currentValue;
            }
            return intersections;
        }
    }
}
