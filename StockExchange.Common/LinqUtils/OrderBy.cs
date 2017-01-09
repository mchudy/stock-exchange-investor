namespace StockExchange.Common.LinqUtils
{
    /// <summary>
    /// An object representing an ordering
    /// </summary>
    public sealed class OrderBy
    {
        /// <summary>
        /// Creates a new instance of <see cref="OrderBy"/>
        /// </summary>
        /// <param name="column"></param>
        /// <param name="desc"></param>
        public OrderBy(string column, bool desc)
        {
            Column = column;
            Desc = desc;
        }

        /// <summary>
        /// Column by which to order
        /// </summary>
        public string Column { get; private set; }

        /// <summary>
        /// Whether the ordering should be in descending order
        /// </summary>
        public bool Desc { get; private set; }
    }
}
