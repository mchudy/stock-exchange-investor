using System;
using StockExchange.Business.Models.Indicators;
using StockExchange.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;
using StockExchange.Business.Indicators.Common;

namespace StockExchange.Business.Indicators
{
    public class PpIndicator : IIndicator
    {
        public IndicatorType Type => IndicatorType.PivotPoint;

        public IList<IndicatorValue> Calculate(IList<Price> prices)
        {
            return prices.Select(price => new IndicatorValue
            {
                Date = price.Date, Value = (price.HighPrice + price.LowPrice + price.ClosePrice)/3
            }).ToList();
        }

        public IList<Signal> GenerateSignals(IList<Price> prices)
        {
            List<Signal> signals = new List<Signal>();
            var values = CalculateSupportsAndResistances(prices);
            PivotPointSupportResistance lastpp = null;
            const SignalAction lastAction = SignalAction.NoSignal;
            foreach (var pp in values)
            {
                if (lastpp == null)
                {
                    lastpp = pp;
                    continue;
                }
                // ReSharper disable once RedundantLogicalConditionalExpressionOperand
                if(pp.ClosePrice > lastpp.Resistance2 && lastAction != SignalAction.Buy)
                    signals.Add(new Signal(SignalAction.Buy) {Date = pp.Date});
                // ReSharper disable once RedundantLogicalConditionalExpressionOperand
                if(pp.ClosePrice < lastpp.Support2 && lastAction != SignalAction.Sell)
                    signals.Add(new Signal(SignalAction.Sell) {Date = pp.Date});
                lastpp = pp;
            }
            return signals;
        }

        private static IEnumerable<PivotPointSupportResistance> CalculateSupportsAndResistances(IEnumerable<Price> prices)
        {
            return prices.Select(p => new PivotPointSupportResistance(p)).ToList();
        } 
    }

    internal class PivotPointSupportResistance
    {
        private readonly Price _price;
        public DateTime Date => _price.Date;
        public decimal PivotPoint => (_price.LowPrice + _price.HighPrice + _price.ClosePrice)/3;
        public decimal Resistance1 => 2*PivotPoint - _price.LowPrice;
        public decimal Resistance2 => PivotPoint + _price.HighPrice - _price.LowPrice;
        public decimal Support1 => 2*PivotPoint - _price.HighPrice;
        public decimal Support2 => PivotPoint - _price.HighPrice + _price.LowPrice;
        public decimal OpenPrice => _price.OpenPrice;
        public decimal ClosePrice => _price.ClosePrice;
        public decimal HightPrice => _price.HighPrice;
        public decimal LowPrice => _price.LowPrice;

        public decimal TomorrowHigh => GetX()/2 - _price.LowPrice;
        public decimal TomorrowLow => GetX()/2 - _price.HighPrice;

        private decimal GetX()
        {
            if (ClosePrice > OpenPrice)
                return ClosePrice + LowPrice + 2 * HightPrice;
            if (ClosePrice < OpenPrice)
                return HightPrice + ClosePrice + 2 * LowPrice;
            return HightPrice + LowPrice + 2 * ClosePrice;
        }

        public PivotPointSupportResistance(Price price)
        {
            _price = price;
        }
    }
}
