using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockExchange.Business.Indicators.Common;
using StockExchange.DataAccess.Models;
using StockExchange.UnitTest.TestHelpers;
using Xunit;

namespace StockExchange.UnitTest.Indicators
{
    public class IndicatorRequiredPricesCountTests
    {
        private static readonly IndicatorFactory _indicatorFactory = new IndicatorFactory();

        private static readonly IList<Price> ExampleData = DataHelper.ConvertToPrices(new decimal[,]
        {
            {30.1983m, 29.4072m, 29.8720m},
            {30.2776m, 29.3182m, 30.2381m},
            {30.4458m, 29.9611m, 30.0996m},
            {29.3478m, 28.7443m, 28.9028m},
            {29.3477m, 28.5566m, 28.9225m},
            {29.2886m, 28.4081m, 28.4775m},
            {28.8334m, 28.0818m, 28.5566m},
            {28.7346m, 27.4289m, 27.5576m},
            {28.6654m, 27.6565m, 28.4675m},
            {28.8532m, 27.8345m, 28.2796m},
            {28.6356m, 27.3992m, 27.4882m},
            {27.6761m, 27.0927m, 27.2310m},
            {27.2112m, 26.1826m, 26.3507m},
            {26.8651m, 26.1332m, 26.3309m},
            {27.4090m, 26.6277m, 27.0333m},
            {26.9441m, 26.1332m, 26.2221m},
            {26.5189m, 25.4307m, 26.0144m},
            {26.5189m, 25.3518m, 25.4605m},
            {27.0927m, 25.8760m, 27.0333m},
            {27.6860m, 26.9640m, 27.4487m},
            {28.4477m, 27.1421m, 28.3586m},
            {28.5267m, 28.0123m, 28.4278m},
            {28.6654m, 27.8840m, 27.9530m},
            {29.0116m, 27.9928m, 29.0116m},
            {29.8720m, 28.7643m, 29.3776m},
            {29.8028m, 29.1402m, 29.3576m},
            {29.7529m, 28.7127m, 28.9107m},
            {30.6546m, 28.9290m, 30.6149m},
            {30.5951m, 30.0304m, 30.0502m},
            {30.7635m, 29.3863m, 30.1890m},
            {31.1698m, 30.1365m, 31.1202m},
            {30.8923m, 30.4267m, 30.5356m},
            {30.0402m, 29.3467m, 29.7827m},
            {30.6645m, 29.9906m, 30.0402m},
            {30.5951m, 29.5152m, 30.4861m},
            {31.9724m, 30.9418m, 31.4670m},
            {32.1011m, 31.5364m, 32.0515m},
            {32.0317m, 31.3580m, 31.9724m},
            {31.6255m, 30.9220m, 31.1302m},
            {31.8534m, 31.1994m, 31.6551m},
            {32.7055m, 32.1308m, 32.6360m},
            {32.7648m, 32.2298m, 32.5866m},
            {32.5766m, 31.9724m, 32.1903m},
            {32.1308m, 31.5562m, 32.1011m},
            {33.1215m, 32.2101m, 32.9335m},
            {33.1909m, 32.6262m, 33.0027m},
            {32.5172m, 31.7642m, 31.9425m}
        });

        [Theory]
        [InlineData(IndicatorType.Macd)]
        [InlineData(IndicatorType.Rsi)]
        [InlineData(IndicatorType.Roc)]
        [InlineData(IndicatorType.Obv)]
        [InlineData(IndicatorType.Atr)]
        [InlineData(IndicatorType.Sma)]
        [InlineData(IndicatorType.Ema)]
        [InlineData(IndicatorType.Vroc)]
        [InlineData(IndicatorType.Vhf)]
        [InlineData(IndicatorType.PivotPoint)]
        [InlineData(IndicatorType.Vpt)]
        [InlineData(IndicatorType.Adx)]
        public void Is_Required_Prices_Count_Valid_For_Generating_Signals(IndicatorType type)
        {
            IIndicator indicator = _indicatorFactory.CreateIndicator(type);
            var examplePrices = ExampleData.Take(indicator.RequiredPricesForSignalCount).ToList();
            try
            {
                var signals = indicator.GenerateSignals(examplePrices);
            }
            catch(Exception ex)
            {
                Assert.True(false, $"Exception occured {ex}");
            }

        }
    }
}
