using System.Collections.Generic;
using System.Linq;

namespace StockExchange.Common.LinqUtils
{
    /// <summary>
    /// An object representing a collection filtering
    /// </summary>
    public sealed class SearchBy
    {
        /// <summary>
        /// Creates a new <see cref="SearchBy"/> instance
        /// </summary>
        /// <param name="field">Field by which to filter data</param>
        /// <param name="values">Values which the field might take</param>
        public SearchBy(string field, IEnumerable<string> values)
        {
            Field = field;
            Values = values?.ToList() ?? new List<string>();
        }

        /// <summary>
        /// Creates a new <see cref="SearchBy"/> instance
        /// </summary>
        /// <param name="field">Field by which to filter data</param>
        /// <param name="value">Value which the field might take</param>
        public SearchBy(string field, string value)
        {
            Field = field;
            Values = new List<string> { value };
        }

        /// <summary>
        /// Field by which to filter data
        /// </summary>
        public string Field { get; private set; }

        /// <summary>
        /// Values which the field might take
        /// </summary>
        public List<string> Values { get; private set; }
    }
}
