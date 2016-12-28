﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockExchange.Business.Indicators;
using StockExchange.Business.Models.Indicators;
using StockExchange.DataAccess.Models;
using StockExchange.UnitTest.TestHelpers;

namespace StockExchange.UnitTest.Indicators.ADX
{
    /// <summary>
    /// File with example data:
    /// http://stockcharts.com/school/data/media/chart_school/technical_indicators_and_overlays/average_directional_index_adx/cs-adx.xls
    /// </summary>
    public static class AdxData
    {
        public const int AdxPrecision = 4;
        public static IList<Price> GetData()
        {
            return DataHelper.ConvertToPrices(new decimal[,]
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
                {32.5172m, 31.7642m, 31.9425m},
                {32.4379m, 31.7840m, 32.3883m},
                {33.2207m, 32.0912m, 32.4875m},
                {32.8343m, 32.1903m, 32.8046m},
                {33.6169m, 32.7648m, 33.3792m},
                {33.7459m, 33.0423m, 33.4188m},
                {33.5971m, 33.0522m, 33.1711m},
                {34.0825m, 33.3297m, 33.6268m},
                {34.5780m, 33.7260m, 33.9638m},
                {34.2214m, 33.6962m, 34.0529m},
                {34.7663m, 34.2015m, 34.7266m},
                {34.7364m, 34.3105m, 34.6969m},
                {35.0140m, 34.1420m, 34.7067m},
                {34.9447m, 33.5674m, 33.8944m},
                {34.4194m, 33.5674m, 33.9142m},
                {34.3995m, 33.3692m, 34.0331m},
                {34.1619m, 33.2108m, 33.6169m},
                {33.3396m, 32.6560m, 32.7154m},
                {33.3892m, 32.7747m, 33.0819m},
                {33.5079m, 32.9235m, 33.0621m},
                {33.9638m, 33.0820m, 33.9240m},
                {34.4194m, 33.6368m, 34.0825m},
                {34.7167m, 33.8647m, 33.9638m},
                {33.9440m, 33.0027m, 33.3396m},
                {33.6567m, 33.0127m, 33.2306m},
                {34.5086m, 32.8738m, 34.4691m},
                {34.8653m, 34.1124m, 34.2312m},
                {34.7464m, 33.8944m, 34.6275m},
                {35.1725m, 34.4393m, 35.0536m},
                {36.1633m, 35.2816m, 36.0543m},
                {36.4504m, 35.7768m, 36.1038m},
                {36.0344m, 35.5985m, 35.9948m},
                {36.4504m, 36.0048m, 36.4011m},
                {36.7380m, 36.0839m, 36.4407m},
                {36.6091m, 35.7868m, 36.3318m},
                {36.8270m, 36.3318m, 36.6091m},
                {36.8369m, 35.9552m, 36.4803m},
                {36.8865m, 36.4110m, 36.4803m},
                {36.3802m, 35.8659m, 36.3119m},
                {35.9948m, 35.2516m, 35.5688m},
                {35.8561m, 35.1923m, 35.2220m},
                {35.8759m, 35.1230m, 35.5578m},
                {35.7273m, 35.2418m, 35.4896m},
                {36.0688m, 35.6225m, 35.8703m},
                {35.6025m, 34.7394m, 34.7990m},
                {34.9775m, 34.4915m, 34.7196m},
                {35.5827m, 34.9974m, 35.3049m},
                {36.0688m, 34.9974m, 35.9993m},
                {36.2076m, 35.7612m, 36.0786m},
                {36.4555m, 35.8307m, 36.1580m},
                {36.4359m, 35.8207m, 36.0885m},
                {36.5448m, 36.0985m, 36.1084m},
                {35.8107m, 35.2156m, 35.3149m},
                {35.2552m, 34.7593m, 35.1263m},
                {35.2058m, 34.2335m, 34.2534m},
                {34.5908m, 34.0252m, 34.4320m},
                {34.7296m, 34.3725m, 34.4915m},
                {34.8586m, 34.2833m, 34.6403m},
                {35.3149m, 34.2038m, 35.3049m},
                {35.5034m, 35.1165m, 35.4338m},
                {36.6342m, 35.8505m, 36.6243m},
                {37.1401m, 36.4259m, 37.0608m},
                {37.2691m, 36.8722m, 37.2592m},
                {37.6956m, 37.3087m, 37.6162m},
                {37.8742m, 37.3385m, 37.8742m},
                {38.3801m, 37.8245m, 38.1917m},
                {39.1736m, 38.0825m, 39.0347m},
                {39.0546m, 38.4693m, 38.7372m},
                {39.0944m, 38.5586m, 39.0347m},
                {39.2729m, 38.6181m, 39.1538m},
                {39.1141m, 38.6876m, 39.0249m},
                {39.8581m, 39.1935m, 39.2530m},
                {39.5330m, 39.0944m, 39.1339m},
                {39.7392m, 39.3225m, 39.7193m},
                {39.8680m, 39.4513m, 39.7193m},
                {39.8185m, 39.1439m, 39.4117m},
                {39.6102m, 38.9257m, 39.0644m},
                {39.7491m, 39.2729m, 39.5605m},
                {39.5309m, 39.0249m, 39.2827m},
                {39.1836m, 38.7372m, 38.9454m},
                {39.8978m, 38.8860m, 39.5506m},
                {39.8383m, 39.3225m, 39.7689m},
                {39.6399m, 38.9653m, 39.3126m},
                {38.6975m, 38.1520m, 38.1717m},
                {38.7968m, 38.3007m, 38.7174m},
                {39.0844m, 38.2312m, 38.9852m},
                {39.5209m, 38.8860m, 39.4415m},
                {40.0368m, 39.4217m, 39.9671m},
                {40.2450m, 39.7788m, 39.9276m},
                {40.4335m, 39.9276m, 40.0468m},
                {40.2757m, 39.7589m, 39.9771m},
                {40.1458m, 39.3522m, 40.0763m},
                {40.7509m, 39.8383m, 40.1161m},
                {39.8780m, 39.4713m, 39.7093m},
                {40.2649m, 38.8464m, 38.9653m},
                {39.1538m, 38.7272m, 38.9356m},
                {39.2532m, 38.7075m, 39.1935m},
                {40.0566m, 39.2035m, 40.0368m},
                {40.4534m, 40.1359m, 40.4137m},
                {40.9593m, 40.2649m, 40.7609m},
                {41.1973m, 40.6717m, 41.1478m},
                {41.2867m, 40.8898m, 41.1874m},
                {41.3759m, 40.8799m, 41.3461m},
                {41.6140m, 41.2371m, 41.4750m},
                {42.1397m, 41.5049m, 42.0802m},
                {42.3183m, 41.8719m, 42.0703m},
                {42.3193m, 41.9122m, 42.1406m},
                {42.3988m, 41.8625m, 42.2696m},
                {42.5279m, 42.1404m, 42.3492m},
                {42.8655m, 42.0781m, 42.1505m},
                {42.3888m, 41.4752m, 41.7732m},
                {41.7931m, 41.2867m, 41.4059m},
                {42.3591m, 41.5944m, 42.1107m},
                {42.3194m, 41.7335m, 41.9221m},
                {42.3019m, 41.3065m, 41.9519m},
                {41.8328m, 40.7107m, 40.7107m},
                {40.9590m, 40.4327m, 40.5916m},
                {41.1079m, 40.5419m, 40.9192m},
                {41.8427m, 40.9298m, 41.6440m},
                {41.7832m, 41.4752m, 41.7633m},
                {42.3193m, 41.8427m, 41.9420m},
                {42.2498m, 41.7633m, 42.1803m},
                {42.5676m, 41.9817m, 42.2696m},
                {42.4485m, 42.0711m, 42.2795m},
                {42.9876m, 42.5476m, 42.8555m},
                {42.8158m, 42.5676m, 42.7561m},
                {42.6867m, 42.1803m, 42.4781m},
                {42.9647m, 42.2895m, 42.9052m},
                {43.1533m, 42.6370m, 42.9151m},
                {43.5109m, 42.7561m, 42.8356m},
                {43.1832m, 42.4881m, 43.0044m},
                {43.4215m, 42.7165m, 42.8257m},
                {43.4513m, 42.4881m, 42.6867m},
                {42.8059m, 41.8825m, 42.0413m},
                {42.0214m, 41.0086m, 41.0980m},
                {41.8925m, 41.2470m, 41.7931m},
                {41.8526m, 40.6313m, 40.6711m},
                {41.2767m, 40.3532m, 40.8399m},
                {41.0285m, 40.5519m, 40.9689m},
                {41.5747m, 40.9888m, 41.0384m},
                {42.1009m, 41.4852m, 42.0512m},
                {42.3492m, 41.7832m, 42.2995m},
                {43.2129m, 42.5775m, 43.2030m},
                {43.4627m, 43.0938m, 43.3122m},
                {43.8286m, 43.3024m, 43.5903m},
                {43.8484m, 43.2428m, 43.3421m},
                {43.8286m, 43.3221m, 43.6995m},
                {44.3350m, 43.8088m, 44.1464m},
                {44.2854m, 43.9378m, 44.2854m},
                {44.1761m, 43.7392m, 44.0401m},
                {43.7690m, 43.0442m, 43.3520m},
                {43.2428m, 42.9747m, 43.1336m},
                {44.0868m, 43.5506m, 43.8286m},
                {43.8384m, 43.4215m, 43.6796m},
                {43.9080m, 43.6995m, 43.8683m},
                {43.5208m, 42.5973m, 43.2030m},
                {43.3122m, 42.8059m, 43.2528m},
                {43.9876m, 43.5704m, 43.6995m},
                {44.1761m, 43.6697m, 43.7591m},
                {44.1861m, 43.5704m, 43.5803m},
                {44.4144m, 43.3520m, 43.8088m},
                {43.9776m, 43.5109m, 43.6002m},
                {43.6896m, 43.0540m, 43.3341m},
                {43.8088m, 43.0143m, 43.7690m},
                {44.2257m, 43.9279m, 43.9876m},
                {44.2754m, 43.6400m, 43.8188m},
                {44.2854m, 43.8782m, 44.2357m},
                {44.3549m, 43.8584m, 43.9876m},
                {44.3846m, 43.9776m, 44.0469m},
                {43.8981m, 43.4513m, 43.5109m},
                {44.2446m, 43.7671m, 44.2246m},
                {44.8911m, 44.4335m, 44.7220m},
                {45.0502m, 44.7916m, 44.9905m},
                {45.3287m, 44.9707m, 45.3188m},
                {45.7366m, 45.3586m, 45.7366m},
                {46.0548m, 45.6968m, 45.9753m},
                {46.0052m, 45.7466m, 45.7863m},
                {46.0151m, 45.7167m, 45.9256m},
                {46.0301m, 45.5078m, 45.5078m},
                {46.2389m, 46.0251m, 46.1743m},
                {46.2538m, 45.9157m, 46.1743m},
                {46.3036m, 45.8262m, 45.8957m},
                {46.0251m, 45.6769m, 45.9256m},
                {46.3036m, 45.6868m, 46.3036m},
                {46.3931m, 45.8759m, 46.1146m},
                {45.8957m, 45.2890m, 45.5376m},
                {46.2439m, 45.3685m, 46.1046m},
                {46.2737m, 45.9753m, 46.1444m},
                {46.3036m, 45.4083m, 45.6073m},
                {46.3931m, 45.7068m, 46.3434m},
                {46.3572m, 45.1896m, 45.6769m},
                {46.1046m, 45.0602m, 45.2492m},
                {45.2392m, 43.8069m, 43.9263m},
                {44.3639m, 43.8865m, 44.0755m},
                {44.6524m, 43.8169m, 44.1152m},
                {44.6126m, 43.7771m, 44.4635m},
                {44.1949m, 43.0908m, 43.3194m},
                {43.7869m, 42.4044m, 42.5635m},
                {43.0515m, 42.6530m, 43.0310m},
                {43.5483m, 42.8022m, 43.4189m},
                {43.7372m, 43.1901m, 43.6576m},
                {43.4289m, 42.3944m, 42.3944m},
                {42.7923m, 41.8969m, 42.7524m},
                {42.9514m, 42.4143m, 42.4442m},
                {43.2797m, 42.5337m, 42.8818m},
                {43.0808m, 42.5237m, 42.7923m},
                {43.5582m, 42.5337m, 43.4388m},
                {43.6478m, 42.9315m, 43.5283m},
                {44.1152m, 43.6179m, 44.0854m},
                {44.3341m, 44.0257m, 44.3341m},
                {44.6921m, 44.2146m, 44.6126m},
                {44.8115m, 44.3838m, 44.5927m},
                {44.7916m, 44.3241m, 44.5032m},
                {44.5032m, 43.7174m, 43.9263m},
                {44.5529m, 44.0854m, 44.3739m},
                {44.4635m, 43.5980m, 44.3639m},
                {44.6225m, 44.1649m, 44.5230m},
                {45.2492m, 44.7121m, 45.1696m},
                {45.6073m, 45.2094m, 45.3089m},
                {45.5775m, 45.1896m, 45.3586m},
                {45.5675m, 45.1596m, 45.5078m},
                {46.2937m, 45.7466m, 46.1942m},
                {46.3931m, 46.1942m, 46.2837m},
                {46.8009m, 46.1543m, 46.5423m},
                {47.0298m, 46.5523m, 46.9203m},
                {47.0993m, 46.7313m, 47.0993m},
                {47.2784m, 46.8507m, 47.1093m},
                {47.1292m, 46.6617m, 46.9899m},
                {47.3579m, 46.9800m, 47.2884m},
                {47.6464m, 47.2784m, 47.4177m},
                {47.6265m, 47.3480m, 47.5768m},
                {47.7476m, 47.1203m, 47.2896m},
                {47.9069m, 47.0505m, 47.7177m},
                {48.0961m, 47.5683m, 48.0488m},
                {47.9667m, 47.7078m, 47.8173m},
                {48.3949m, 47.6978m, 47.7476m},
                {48.1260m, 47.5385m, 47.7974m},
                {48.1956m, 47.8969m, 48.0264m},
                {48.3451m, 47.8571m, 48.1858m},
                {48.2454m, 47.8671m, 47.9567m},
                {48.4944m, 47.5883m, 47.9567m},
                {48.5143m, 47.9367m, 48.4048m},
                {48.6936m, 48.1858m, 48.5442m},
                {48.6637m, 48.1658m, 48.4247m},
                {48.6139m, 48.0363m, 48.5343m},
                {48.8429m, 48.4297m, 48.8230m},
                {48.9923m, 48.7334m, 48.8629m},
                {49.1416m, 48.6538m, 49.1118m},
                {49.7093m, 49.2910m, 49.6993m},
                {49.9781m, 49.6594m, 49.9184m},
                {49.9084m, 48.9923m, 49.3209m},
                {49.4503m, 48.6936m, 49.2910m},
                {49.6694m, 49.2213m, 49.5400m},
                {49.9781m, 49.5151m, 49.8188m},
                {50.1474m, 49.0520m, 50.0976m},
                {50.3565m, 49.8785m, 50.3067m},
                {50.4362m, 50.0876m, 50.1972m},
                {50.2171m, 49.0023m, 49.1318m},
                {49.4205m, 48.7732m, 49.1616m},
                {50.1176m, 49.4005m, 50.0180m},
                {50.0778m, 48.9923m, 49.0296m},
                {49.9582m, 49.2213m, 49.7192m},
                {49.1118m, 47.8770m, 48.2256m},
                {48.2952m, 47.4389m, 47.9767m},
                {48.1161m, 41.3746m, 46.3734m},
                {46.6025m, 44.0964m, 45.2183m},
                {47.5982m, 47.1103m, 47.5683m},
                {48.1858m, 47.0007m, 47.5185m},
                {48.4545m, 47.6978m, 48.4147m},
                {48.5840m, 47.5286m, 47.6480m},
                {47.3194m, 46.1842m, 46.7319m},
                {47.0705m, 45.8756m, 46.8812m},
                {47.3791m, 46.0249m, 46.2340m},
                {46.4332m, 45.3577m, 45.8656m},
                {45.0889m, 44.0632m, 44.1628m},
                {45.4474m, 43.3065m, 44.6507m},
                {45.1337m, 44.4516m, 44.4714m},
                {44.5710m, 43.0476m, 44.5113m},
                {45.3776m, 43.9338m, 44.0134m},
                {45.6963m, 44.8996m, 45.6763m},
                {45.7958m, 45.0092m, 45.4075m},
                {46.0547m, 44.9394m, 44.9892m},
                {46.0847m, 44.9394m, 46.0547m},
                {46.5725m, 45.8854m, 46.4929m},
                {46.0945m, 44.7478m, 44.9021m},
                {45.2681m, 44.0334m, 44.0831m},
                {44.2923m, 43.4060m, 44.0034m},
                {44.7205m, 43.5056m, 43.6350m},
                {44.9296m, 44.0532m, 44.8798m},
                {45.3478m, 44.4216m, 45.3079m},
                {46.0448m, 45.2183m, 45.2980m},
                {46.5725m, 45.2613m, 46.5128m},
                {46.9410m, 46.3037m, 46.7020m},
                {46.9809m, 46.4431m, 46.8514m},
                {47.2298m, 46.7509m, 46.8906m},
                {47.5690m, 46.2221m, 46.4915m},
                {47.0303m, 46.0426m, 46.1324m},
                {46.3618m, 45.5836m, 45.9428m},
                {45.8829m, 45.0948m, 45.2444m},
                {45.5537m, 44.8454m, 45.1646m},
                {45.4440m, 44.7057m, 45.0050m},
                {44.3565m, 42.9697m, 43.2690m},
                {43.5683m, 42.5407m, 42.6106m},
                {42.8600m, 41.6728m, 42.4909m},
                {42.7218m, 41.9920m, 42.3711m},
                {43.2690m, 42.1516m, 42.5008m},
                {43.9075m, 42.6006m, 43.8577m},
                {44.2767m, 43.5783m, 44.0971m},
                {44.5261m, 43.9774m, 44.5161m},
                {44.9252m, 44.3565m, 44.6458m},
                {45.3941m, 44.6957m, 45.2245m},
                {45.7034m, 45.1347m, 45.4539m},
                {45.6335m, 44.8853m, 45.4938m},
                {45.5219m, 44.1969m, 44.2368m},
                {44.7057m, 43.9973m, 44.6159m},
                {45.1546m, 43.7579m, 45.1546m},
                {45.6535m, 44.4563m, 44.5361m},
                {45.8730m, 45.1347m, 45.6635m},
                {45.9927m, 45.2744m, 45.9528m},
                {46.3518m, 45.8031m, 46.3319m},
                {46.6112m, 46.1024m, 46.3119m},
                {46.4716m, 45.7732m, 45.9428m},
                {46.3020m, 45.1447m, 45.6036m},
                {45.9827m, 44.9651m, 45.7034m},
                {46.6811m, 46.1025m, 46.5614m},
                {46.5913m, 46.1423m, 46.3618m},
                {46.8806m, 46.3918m, 46.8287m},
                {46.8108m, 46.4117m, 46.7210m},
                {46.7409m, 45.9428m, 46.6511m},
                {47.0801m, 46.6811m, 46.9704m},
                {46.8407m, 46.1723m, 46.5639m},
                {45.8131m, 45.1048m, 45.2943m},
                {45.1347m, 44.3465m, 44.9352m},
                {44.9551m, 44.6059m, 44.6159m},
                {45.0050m, 44.1969m, 44.6957m},
                {45.6734m, 44.9252m, 45.2664m},
                {45.7133m, 45.0050m, 45.4440m},
                {45.3542m, 44.4563m, 44.7556m},
                {44.9252m, 44.4363m, 44.8154m},
                {45.2345m, 44.3565m, 44.3765m},
                {44.0173m, 43.3638m, 43.5484m},
                {44.1570m, 43.1693m, 43.9674m},
                {44.2168m, 43.3987m, 43.4386m},
                {44.0572m, 42.8700m, 43.9674m},
                {44.1470m, 43.4985m, 43.5085m},
                {43.7479m, 43.0795m, 43.3588m},
                {44.8055m, 43.9674m, 44.6558m},
                {45.1746m, 44.6259m, 45.1546m},
                {45.9129m, 45.4440m, 45.9029m},
                {45.9228m, 45.5188m, 45.5936m},
                {46.3419m, 45.7133m, 46.1423m},
                {46.5913m, 46.2122m, 46.3219m},
                {46.5614m, 46.1423m, 46.4915m},
                {47.2597m, 46.8307m, 47.1400m},
                {47.5890m, 46.9704m, 47.3395m},
                {47.6887m, 47.0801m, 47.6388m},
                {47.8683m, 47.4293m, 47.8284m},
                {48.1400m, 47.7500m, 47.9950m},
                {48.9300m, 48.1101m, 48.8299m},
                {49.1700m, 48.6100m, 48.8200m},
                {49.0210m, 48.4200m, 48.6900m},
                {49.1600m, 48.3200m, 48.6700m},
                {49.6900m, 49.1500m, 49.6600m},
                {49.7500m, 49.3500m, 49.3900m},
                {49.5400m, 48.5900m, 49.3700m},
                {49.5300m, 49.1100m, 49.2900m},
                {49.8400m, 48.7500m, 49.0700m},
                {49.5300m, 48.7810m, 49.0100m},
                {49.0500m, 48.2000m, 48.4800m},
                {49.7600m, 49.0000m, 49.6600m},
                {49.7100m, 48.9100m, 49.2300m},
                {49.5400m, 49.0000m, 49.4100m},
                {49.8700m, 49.0800m, 49.7500m},
                {50.0200m, 49.6200m, 49.7700m},
                {50.2100m, 49.2600m, 50.1100m},
                {50.7500m, 50.2800m, 50.5200m},
                {50.6400m, 50.1700m, 50.4200m},
                {51.5000m, 50.6300m, 51.4900m},
                {51.7200m, 51.3000m, 51.3000m},
                {51.3000m, 50.4200m, 50.8200m},
                {51.5700m, 50.8700m, 51.1900m},
                {51.7101m, 50.7900m, 51.2900m},
                {51.6900m, 51.2100m, 51.6400m},
                {52.2300m, 51.8500m, 51.8900m},
                {52.1500m, 51.4200m, 52.0300m},
                {52.2300m, 51.6600m, 52.1900m},
                {52.4500m, 51.8400m, 52.3000m},
                {52.4900m, 52.1698m, 52.1800m},
                {52.7500m, 51.9800m, 52.2200m},
                {52.9300m, 52.5750m, 52.7800m},
                {53.0400m, 52.3600m, 53.0200m},
                {53.8614m, 53.5000m, 53.6700m},
                {53.8100m, 53.5100m, 53.6700m},
                {53.8300m, 53.4499m, 53.7375m},
                {54.0401m, 53.2100m, 53.4500m},
                {53.7700m, 53.1000m, 53.7150m},
                {53.4800m, 52.6600m, 53.3850m},
                {53.3672m, 52.1100m, 52.5100m},
                {52.8800m, 52.2900m, 52.3150m},
                {52.2462m, 50.8500m, 51.4500m},
                {51.8700m, 51.3500m, 51.6000m},
                {52.7900m, 52.1300m, 52.4300m},
                {52.5900m, 52.1400m, 52.4700m},
                {52.9100m, 52.1700m, 52.9100m},
                {52.4500m, 51.7700m, 52.0700m},
                {53.2500m, 52.5600m, 53.1200m},
                {53.1300m, 52.6700m, 52.7700m},
                {52.9000m, 52.1000m, 52.7300m},
                {52.7355m, 51.8800m, 52.0850m},
                {53.4600m, 52.8400m, 53.1900m},
                {53.8100m, 53.2100m, 53.7300m},
                {53.9400m, 53.5000m, 53.8700m},
                {53.9500m, 53.6800m, 53.8450m},
                {54.5200m, 53.8200m, 53.8800m},
                {54.1500m, 53.6899m, 54.0800m},
                {54.4400m, 53.9500m, 54.1350m},
                {54.5500m, 54.0900m, 54.4950m},
                {54.7400m, 54.2700m, 54.3000m},
                {54.6200m, 54.2300m, 54.3950m},
                {54.7000m, 54.0300m, 54.1600m},
                {54.6600m, 54.0600m, 54.5800m},
                {54.6800m, 54.4100m, 54.5200m},
                {54.7600m, 54.1600m, 54.5600m},
                {54.8900m, 54.6200m, 54.8900m},
                {54.9600m, 54.7900m, 54.8850m},
                {54.8700m, 54.6100m, 54.7420m},
                {54.8600m, 54.2100m, 54.7700m},
                {54.9200m, 54.5500m, 54.6700m},
                {54.9000m, 54.7300m, 54.7900m},
                {54.8000m, 54.5500m, 54.6600m},
                {54.6200m, 54.2100m, 54.4600m},
                {55.6900m, 54.9500m, 55.3100m},
                {55.5500m, 54.9200m, 55.2650m},
                {55.7600m, 55.0700m, 55.7400m},
                {55.9600m, 55.6800m, 55.9200m},
                {56.0500m, 55.3200m, 55.8700m},
                {56.1800m, 55.5800m, 56.0800m},
                {56.3600m, 55.9500m, 56.1600m},
                {56.5600m, 56.2000m, 56.5550m},
                {56.7300m, 56.4100m, 56.5750m},
                {57.0200m, 56.4600m, 57.0000m},
                {57.2300m, 56.4900m, 57.1600m},
                {57.2600m, 56.3200m, 56.5100m},
                {56.3500m, 55.6800m, 56.1100m},
                {56.4900m, 55.6500m, 55.6800m},
                {56.4600m, 55.6800m, 56.4500m},
                {56.5500m, 56.0500m, 56.5325m},
                {56.9800m, 56.4500m, 56.8300m},
                {57.3500m, 56.9200m, 57.1800m},
                {57.2200m, 55.4700m, 55.7300m},
                {56.1600m, 55.3900m, 56.0000m},
                {57.1800m, 56.3600m, 57.0500m},
                {57.1700m, 56.8400m, 56.9550m},
                {57.1400m, 56.4000m, 57.0575m},
                {57.4200m, 56.9000m, 57.3750m},
                {57.9700m, 57.4000m, 57.6500m},
                {58.0700m, 57.5600m, 58.0250m},
                {58.1200m, 57.7500m, 57.9300m}
            });
        }

        public static IList<IndicatorValue> GetValues()
        {
            return DataHelper.ConvertToIndicatorValues(new decimal[]
            {
                33.5833m,
                32.1535m,
                29.9292m,
                28.3576m,
                26.8983m,
                25.7765m,
                23.9459m,
                22.7833m,
                22.0715m,
                21.5276m,
                20.7995m,
                19.5906m,
                18.7176m,
                18.7515m,
                18.8378m,
                18.5544m,
                17.7269m,
                17.9545m,
                18.2291m,
                17.3521m,
                16.5377m,
                16.5912m,
                16.6408m,
                17.4112m,
                18.2370m,
                19.0038m,
                20.1474m,
                21.6061m,
                22.9053m,
                24.5325m,
                26.0435m,
                27.6585m,
                28.0052m,
                28.3271m,
                28.2320m,
                27.8298m,
                26.4457m,
                25.2323m,
                24.2837m,
                24.0461m,
                24.3853m,
                25.0316m,
                24.0207m,
                23.0821m,
                23.2436m,
                23.7572m,
                23.8339m,
                24.3504m,
                25.6765m,
                27.1156m,
                28.1088m,
                29.3531m,
                30.7179m,
                31.3860m,
                32.1893m,
                32.1847m,
                32.2305m,
                31.2385m,
                29.3295m,
                27.4672m,
                25.6282m,
                23.9205m,
                22.8851m,
                21.9778m,
                21.4649m,
                20.0127m,
                19.1765m,
                18.5827m,
                18.3556m,
                18.1266m,
                18.0668m,
                17.0541m,
                16.7406m,
                17.0845m,
                17.6370m,
                17.8915m,
                17.8837m,
                17.0601m,
                15.9811m,
                16.2361m,
                17.0031m,
                17.8442m,
                19.0365m,
                20.3081m,
                21.9262m,
                24.0019m,
                25.9292m,
                27.7474m,
                29.5666m,
                31.2560m,
                33.3400m,
                35.0272m,
                36.7360m,
                38.4113m,
                39.1433m,
                39.2692m,
                39.5317m,
                39.1400m,
                38.0855m,
                37.9797m,
                37.8815m,
                36.9536m,
                34.5421m,
                32.4573m,
                30.9617m,
                30.1809m,
                30.0790m,
                30.2148m,
                30.5462m,
                30.4769m,
                29.5587m,
                29.4638m,
                28.6651m,
                26.8711m,
                25.0207m,
                23.4575m,
                23.1143m,
                23.2517m,
                23.9004m,
                24.7298m,
                25.5859m,
                26.4697m,
                27.5273m,
                28.9823m,
                30.4809m,
                31.8734m,
                33.2379m,
                34.6236m,
                36.2049m,
                36.0672m,
                35.4975m,
                35.6344m,
                35.7616m,
                34.8797m,
                32.8904m,
                30.5650m,
                28.6537m,
                27.9517m,
                27.2999m,
                27.3713m,
                27.2702m,
                27.5624m,
                27.8338m,
                28.7016m,
                29.5075m,
                29.3084m,
                29.4808m,
                29.8724m,
                30.6467m,
                30.6964m,
                31.0366m,
                30.7886m,
                29.2586m,
                27.9170m,
                26.6713m,
                26.3403m,
                26.3620m,
                26.3821m,
                25.3276m,
                23.5624m,
                22.2977m,
                22.2331m,
                22.4476m,
                23.0290m,
                23.4533m,
                23.8472m,
                24.7656m,
                25.6183m,
                25.9493m,
                24.8517m,
                23.7061m,
                23.7696m,
                23.5818m,
                23.4966m,
                22.0782m,
                20.7610m,
                19.9901m,
                19.5184m,
                18.9097m,
                18.6598m,
                18.4278m,
                17.3611m,
                16.5650m,
                16.4688m,
                15.8311m,
                15.2553m,
                14.8410m,
                14.5103m,
                13.8251m,
                13.1510m,
                13.5581m,
                14.1595m,
                15.0954m,
                16.4660m,
                18.0907m,
                19.5995m,
                20.9198m,
                21.5787m,
                22.4814m,
                23.0143m,
                23.2559m,
                23.0584m,
                23.3624m,
                23.7944m,
                22.6313m,
                22.1959m,
                21.8447m,
                20.3437m,
                19.0155m,
                18.5304m,
                18.3036m,
                19.7329m,
                21.0601m,
                21.6254m,
                22.2007m,
                23.5171m,
                25.3485m,
                27.0491m,
                27.4819m,
                27.4827m,
                28.3155m,
                29.5131m,
                30.2881m,
                30.3320m,
                30.3836m,
                29.4778m,
                28.4671m,
                26.6900m,
                24.8887m,
                23.7735m,
                22.9172m,
                22.0048m,
                20.7972m,
                19.5853m,
                19.2361m,
                18.5991m,
                17.6325m,
                17.2710m,
                16.8975m,
                16.4901m,
                17.1589m,
                17.9059m,
                19.0906m,
                20.4447m,
                21.7794m,
                23.2195m,
                24.0512m,
                25.1073m,
                26.4225m,
                27.6437m,
                28.1064m,
                28.7573m,
                29.6160m,
                30.4135m,
                31.7021m,
                32.3753m,
                33.0959m,
                33.9700m,
                34.7817m,
                34.5245m,
                34.3222m,
                34.4624m,
                34.5146m,
                34.0521m,
                34.0931m,
                34.4135m,
                34.9789m,
                36.3279m,
                37.8856m,
                37.0919m,
                35.5645m,
                34.5345m,
                34.0802m,
                32.5058m,
                31.4063m,
                30.5223m,
                29.1363m,
                28.1950m,
                26.2952m,
                24.9353m,
                23.6726m,
                24.1544m,
                25.0023m,
                28.2834m,
                31.3302m,
                33.0099m,
                33.9453m,
                34.5316m,
                35.1505m,
                36.2670m,
                37.4161m,
                38.1210m,
                39.0463m,
                40.3619m,
                41.8144m,
                43.1631m,
                44.8099m,
                45.3895m,
                45.5682m,
                45.6186m,
                45.3545m,
                45.0718m,
                44.1948m,
                44.0167m,
                44.1939m,
                44.6343m,
                44.5021m,
                44.1161m,
                43.2344m,
                41.6028m,
                39.5220m,
                37.2111m,
                35.0234m,
                32.7223m,
                31.1061m,
                29.7766m,
                28.9698m,
                28.6444m,
                28.5498m,
                28.5797m,
                29.7883m,
                31.1376m,
                32.8039m,
                34.3512m,
                34.9476m,
                34.6110m,
                33.8186m,
                32.7637m,
                31.2851m,
                29.3589m,
                27.3021m,
                25.5922m,
                24.7224m,
                24.1092m,
                22.9526m,
                21.3498m,
                20.1150m,
                19.1091m,
                18.5925m,
                18.4034m,
                17.7363m,
                16.6690m,
                15.9027m,
                15.2460m,
                14.6361m,
                14.4313m,
                14.2412m,
                13.3258m,
                12.9493m,
                12.2053m,
                12.7763m,
                14.0006m,
                15.1375m,
                16.5475m,
                16.7896m,
                16.9538m,
                17.6541m,
                18.3238m,
                18.4180m,
                19.4661m,
                20.6023m,
                21.5494m,
                22.8829m,
                23.9480m,
                25.2920m,
                24.7209m,
                23.6642m,
                22.2009m,
                20.8541m,
                20.1050m,
                19.6943m,
                19.2047m,
                19.5269m,
                20.1508m,
                20.8277m,
                21.6356m,
                22.6518m,
                24.2675m,
                25.9468m,
                27.1095m,
                28.3078m,
                29.8438m,
                31.3157m,
                31.0944m,
                30.8890m,
                30.0106m,
                29.1949m,
                27.3938m,
                26.6783m,
                25.8572m,
                25.0948m,
                24.8329m,
                24.7849m,
                24.0088m,
                24.0111m,
                23.7904m,
                24.5726m,
                25.5129m,
                24.7611m,
                24.3772m,
                24.1826m,
                24.0019m,
                24.4587m,
                24.0693m,
                23.8072m,
                23.8394m,
                23.9202m,
                24.3261m,
                24.9244m,
                24.9720m,
                25.9763m,
                26.9088m,
                27.6248m,
                27.6936m,
                27.4892m,
                26.3027m,
                24.6827m,
                23.1784m,
                23.7384m,
                24.2583m,
                23.1130m,
                22.0495m,
                20.5550m,
                19.7402m,
                18.8363m,
                17.9970m,
                17.0425m,
                16.4526m,
                15.6888m,
                15.4173m,
                15.3248m,
                15.2517m,
                15.8815m,
                16.2249m,
                16.8916m,
                17.6408m,
                18.5623m,
                19.3268m,
                19.5751m,
                19.8057m,
                20.0515m,
                19.6458m,
                19.5062m,
                19.5058m,
                19.0156m,
                17.7259m,
                16.5292m,
                15.4180m,
                14.6974m,
                14.7902m,
                14.8806m,
                14.8960m,
                15.2664m,
                15.9345m,
                15.6720m,
                15.6611m,
                15.9676m,
                16.5899m,
                17.4438m,
                18.6764m,
                20.1145m,
                20.9341m,
                20.0581m,
                19.5018m,
                18.9853m,
                18.6845m,
                19.1848m,
                20.2125m,
                19.3213m,
                18.6127m,
                18.1633m,
                17.7461m,
                16.6336m,
                16.0082m,
                16.1499m,
                16.4049m,
                16.7059m
            }, 2*AdxIndicator.DefaultTerm - 1);
        }
    }
}