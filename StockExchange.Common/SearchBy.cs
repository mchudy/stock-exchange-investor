using System.Collections.Generic;
using System.Linq;

namespace StockExchange.Common
{
    public sealed class SearchBy
    {
        public string Field { get; private set; }

        public List<string> Values { get; private set; }

        public SearchBy(string field, IEnumerable<string> values)
        {
            Field = field;
            Values = values?.ToList() ?? new List<string>();
        }

        public SearchBy(string field, string value)
        {
            Field = field;
            Values = new List<string> { value };
        }
    }
}
