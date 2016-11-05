using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using StockExchange.Business.Models;
using StockExchange.Common.LinqUtils;
using StockExchange.Web.Models;
using StockExchange.Web.Models.DataTables;

namespace StockExchange.Web.Helpers
{
    internal static class DataTableMessageConverter
    {
        internal static PagedFilterDefinition<T> ToPagedFilterDefinition<T>(DataTableMessage<T> dataTableMessage) where T : IFilter
        {
            return new PagedFilterDefinition<T>
            {
                Start = dataTableMessage.Start,
                Length = dataTableMessage.Length,
                OrderBys = dataTableMessage.Order.Select(o => new OrderBy(dataTableMessage.Columns[o.Column].Data, o.Desc)).ToList(),
                Searches = dataTableMessage.Columns.Where(c => !string.IsNullOrWhiteSpace(c.Search.Value)).Select(c => new { Field = c.Data, Value = JsonConvert.DeserializeObject<string[]>(c.Search.Value) }).Where(c => c != null)
                    .Select(c =>
                    {
                        var nullValue = new List<string> { null };
                        var values = (c.Value.Contains(string.Empty) ? c.Value.Concat(nullValue) : c.Value).Distinct();
                        return new SearchBy(c.Field, values);

                    }).ToList(),
                Search = dataTableMessage.Search.Value,
                Filter = dataTableMessage.Filter
            };
        }

        internal static FilterDefinition<T> ToFilterDefinition<T>(DataTableSimpleMessage<T> dataTableMessage) where T : IFilter
        {
            return new FilterDefinition<T>
            {
                Search = dataTableMessage.Search.Value,
                Filter = dataTableMessage.Filter
            };
        }
    }
}
