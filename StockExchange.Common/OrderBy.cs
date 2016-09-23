namespace StockExchange.Common
{
    public sealed class OrderBy
    {
        public OrderBy(string column, bool desc)
        {
            Column = column;
            Desc = desc;
        }

        public string Column { get; private set; }

        public bool Desc { get; private set; }
    }
}
