(function() {
    'use strict';

    var getLineChartDataUrl = '/Charts/GetLineChartData';
    var getCandlestickDataUrl = '/Charts/GetCandlestickChartData';
    var loadingText = 'Loading...';
    var chosenCompanies = [21];

    var $isCandleStickCheckbox = $('#is-candlestick-chart');

    var chart;

    $('.company-select').select2();
    $('.company-select').val(chosenCompanies).trigger('change');

    initChart();
    loadChart();

    $isCandleStickCheckbox.on('change', function () {
        loadChart();
    });

    $('.company-select').on('change', function () {
        chosenCompanies = $(this).val();
        loadChart();
    });

    function initChart() {
        Highcharts.setOptions({
            global: {
                useUTC: true
            }
        });

        chart = Highcharts.stockChart('chart-container', {
            rangeSelector: {
                selected: 1
            },
            title: {
                text: 'Stock chart'
            }
        });
        chart.showLoading(loadingText);
    }

    function loadChart() {
        if (!chosenCompanies || !chosenCompanies.length) {
            clearChart();
            return;
        }

        chart.showLoading(loadingText);

        if (!$isCandleStickCheckbox.is(':checked')) {
            $.getJSON(addCompaniesToUrl(getLineChartDataUrl, chosenCompanies), function (data) {
                refreshChartData(data);
            });
        } else {
            $.getJSON(addCompaniesToUrl(getCandlestickDataUrl, chosenCompanies), function (data) {
                for (var i = 0; i < data.length; i++) {
                    data[i] = $.extend(data[i], {
                        type: 'candlestick'
                    });
                }
                refreshChartData(data);
            });
        }
    }

    function addCompaniesToUrl(baseUrl, companyIds) {
        var newUrl = baseUrl + '?';
        for (var i = 0; i < companyIds.length; i++) {
            newUrl += 'companyIds=' + companyIds[i] + '&';
        }
        return newUrl.slice(0, -1);
    }

    function refreshChartData(data) {
        clearChart();
        for (var i = 0; i < data.length; i++) {
            chart.addSeries(data[i]);
        }
        chart.hideLoading();
    };

    function clearChart() {
        while (chart.series.length > 0) {
            chart.series[0].remove(true);
        }
    }

})();