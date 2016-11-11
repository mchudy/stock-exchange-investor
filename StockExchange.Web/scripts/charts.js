(function() {
    'use strict';

    var getLineChartDataUrl = '/Charts/GetLineChartData';
    var getCandlestickDataUrl = '/Charts/GetCandlestickChartData';
    var chart;

    Highcharts.setOptions({
        global: {
            useUTC: false
        }
    });

    $.getJSON(getLineChartDataUrl + '?companyIds=1&companyIds=2', function (data) {
        chart = Highcharts.stockChart('chart-container', {
            rangeSelector: {
                selected: 1
            },

            title: {
                text: 'Line chart'
            },

            series: data
        });
    });

    $('#isCandlestickChart').on('change', function () {
        chart.showLoading('Loading...');

       if (!$(this).is(':checked')) {
           $.getJSON(getLineChartDataUrl + '?companyIds=1&companyIds=2', function (data) {
               refreshChartData(data);
           });
       } else {
           $.getJSON(getCandlestickDataUrl + '?companyIds=1&companyIds=2', function (data) {
               for (var i = 0; i < data.length; i++) {
                   data[i] = $.extend(data[i], {
                       type: 'candlestick',
                       dataGrouping: {
                           units: [
                               [
                                   'day', [1]
                               ]
                           ]
                       }
                   });
               }
               refreshChartData(data);
           });
       }
    });

    function refreshChartData(data) {
        while (chart.series.length > 0) {
            chart.series[0].remove(true);
        }
        for (var i = 0; i < data.length; i++) {
            chart.addSeries(data[i]);
        }
        chart.hideLoading();
    };

})();