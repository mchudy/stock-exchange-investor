﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockExchange.Business.Models;
using StockExchange.Business.Models.Indicators;
using StockExchange.DataAccess.Models;

namespace StockExchange.Business.Indicators
{
    public class EmaIndicator : IIndicator
    {
        public const int DefaultTerm = 5;

        public IndicatorType Type => IndicatorType.Ema;

        public int Term { get; set; } = DefaultTerm;

        public IList<IndicatorValue> Calculate(IList<Price> prices)
        {
            return MovingAverageHelper.ExpotentialMovingAverage(prices, Term);
        }

        public IList<Signal> GenerateSignals(IList<IndicatorValue> values)
        {
            var signals = new List<Signal>();
            return signals;
        }
    }
}