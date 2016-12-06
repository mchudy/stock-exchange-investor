using StockExchange.Business.Models;
using StockExchange.DataAccess.Models;
using System;
using System.Collections.Generic;

namespace StockExchange.Business.Indicators
{
    public class AtrIndicator : IIndicator
    {
        public const int DefaultAtrTerm = 14;

        public int Term { get; set; } = DefaultAtrTerm;

        public IList<IndicatorValue> Calculate(IList<Price> prices)
        {
            var rsValues = GetRsValues(prices);
            return MovingAverageHelper.SmoothedMovingAverage(rsValues, Term);
        }

        private static List<IndicatorValue> GetRsValues(IList<Price> prices)
        {
            var rsValues = new List<IndicatorValue>
            {
                new IndicatorValue
                {
                    Date = prices[0].Date,
                    Value = prices[0].HighPrice - prices[0].LowPrice
                }
            };
            for (var i = 1; i < prices.Count; i++)
            {
                var rs = Math.Max(
                    prices[i].HighPrice - prices[i].LowPrice, Math.Max(
                        Math.Abs(prices[i].HighPrice - prices[i - 1].ClosePrice),
                        Math.Abs(prices[i].LowPrice - prices[i - 1].ClosePrice)));

                rsValues.Add(new IndicatorValue
                {
                    Date = prices[i].Date,
                    Value = rs
                });
            }
            return rsValues;
        }
    }
}
