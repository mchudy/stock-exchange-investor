using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.Indicators.Lines
{
    public static class PointLineInterceptionHelper
    {
        public static IEnumerable<InterceptionInfo> FindAllInterceptions(PointLine line1, PointLine line2)
        {
            if (line1 == null || line2 == null || line1.Values.Count != line2.Values.Count)
                return null;
            int n = line1.Values.Count;
            IList<InterceptionInfo> interceptions = new List<InterceptionInfo>();
            for (int i = 0; i < n-1; i++)
            {
                if (AreIntercepting(line1.Values[i], line1.Values[i + 1], line2.Values[i], line2.Values[i + 1]))
                {
                    interceptions.Add(new InterceptionInfo(i, line1.Values[i], line1.Values[i+1], line2.Values[i], line2.Values[i]));
                }
            }
            return interceptions;
        }

        private static bool AreIntercepting(decimal start1, decimal end1, decimal start2, decimal end2)
        {
            decimal a = end2 - end1 + start1 - start2;
            decimal b = start1 - start2;
            if (a == 0)
                return b == 0;
            decimal x = b/a;
            return x >= 0 && x <= 1;
        }
    }
}
