(function() {
    'use strict';

    var chart;
    var loadingText = 'Loading...';

    var $companySelect = $('.company-select');
    var $isCandleStickCheckbox = $('#is-candlestick-chart');

    var chosenCompanies = $companySelect.val();

    $companySelect.select2({
        placeholder: 'Choose companies',
        width: '100%'
    });
    $companySelect.trigger('change');

    $('indicator-select').select2();

    initChart();
    loadChart();

    $isCandleStickCheckbox.on('change', function () {
        loadChart();
    });

    $companySelect.on('change', function () {
        chosenCompanies = $(this).val();
        loadChart();
    });

    function initChart() {
        Highcharts.setOptions({
            global: {
                useUTC: true
            }
        });

        chart = new Highcharts.stockChart('chart-container', {
            rangeSelector: {
                inputDateFormat: '%Y-%m-%d',
                inputEditDateFormat: '%Y-%m-%d'
            },
            title: {
                text: 'Stock chart'
            },
            legend: {
                enabled: true
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
            $.getJSON(addCompaniesToUrl(config.getLineChartDataUrl, chosenCompanies), function (data) {
                refreshChartData(data);
            });
        } else {
            $.getJSON(addCompaniesToUrl(config.getCandlestickDataUrl, chosenCompanies), function (data) {
                for (var i = 0; i < data.length; i++) {
                    data[i] = $.extend(data[i], {
                        type: 'candlestick'
                    });
                }
                refreshChartData(data);
            });
        }
    }

    function initDatepickers() {
        $('input.highcharts-range-selector', $('#chart-container'))
            .datepicker({
                format: 'yyyy-MM-dd',
                todayBtn: 'linked',
                todayHighlight: true,
                orientation: 'auto right'
            });
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
        initDatepickers();
    };

    function clearChart() {
        while (chart.series.length > 0) {
            chart.series[0].remove(true);
        }
    }

})();