(function() {
    'use strict';

    var chart;
    var loadingText = 'Loading...';
    var $companySelect = $('.company-select');
    var $indicatorSelect = $('.indicator-select');
    var $isCandleStickCheckbox = $('#is-candlestick-chart');
    var chosenCompanies = $companySelect.val();

    $companySelect.select2({
        placeholder: 'Choose companies',
        width: '100%'
    });
    $companySelect.trigger('change');

    initChart();
    loadChart();

    $isCandleStickCheckbox.on('change', function () {
        loadChart();
    });

    $companySelect.on('change', function () {
        chosenCompanies = $(this).val();
        loadChart();
    });

    $indicatorSelect.on('change', function () {
        loadIndicatorValues($(this).val());
    });

    function initChart() {
        Highcharts.setOptions({
            global: {
                useUTC: true
            }
        });

        chart = new Highcharts.stockChart('chart-container', {
            title: {
                text: 'Stock chart'
            },
            legend: {
                enabled: true
            },
            yAxis: [{
                id: 'price-axis'
            }],
            rangeSelector: {
                inputDateFormat: '%Y-%m-%d',
                inputEditDateFormat: '%Y-%m-%d',
                inputDateParser: function (value) {
                    var date = new Date(value);
                    // hack for dealing with timezone issue
                    date.setTime(date.getTime() + 1 * 1000 * 60 * 60 * 4);
                    return date.getTime();
                }
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
            $.get(config.getLineChartDataUrl, $.param({
                companyIds: chosenCompanies
            }, true), function (data) {
                refreshChartData(data);
            });
        } else {
            $.get(config.getCandlestickDataUrl, $.param({
                companyIds: chosenCompanies
            }, true), function (data) {
                for (var i = 0; i < data.length; i++) {
                    data[i] = $.extend(data[i], {
                        type: 'candlestick'
                    });
                }
                refreshChartData(data);
            });
        }
    }

    function loadIndicatorValues(type) {
        if (!type) {
            removeIndicatorAxis();
            chart.redraw();
            return;
        }
        $.get(config.getIndicatorValuesUrl,
            $.param({
                type: type,
            companyIds: chosenCompanies
        }, true), function(data) {
            drawIndicatorValues(type, data);
        });
    }

    function drawIndicatorValues(type, data) {
        // the false parameters in Highcharts functions prevent from redrawing the chart on every operation
        // (we redraw it only once at the end)
        var oldSeries;
        while ((oldSeries = chart.get('indicator-series'))) {
            oldSeries.remove(false);
        }

        if (!chart.get('indicator-axis')) {
            addIndicatorAxis();
        } else if(!type) {
            removeIndicatorAxis();
        }
        if (type) {
            for (var i = 0; i < data.length; i++) {
                var companyData = data[i];
                if (isDoubleLineIndicator(companyData.data)) {
                    var firstLine = companyData.data.map(function (elem) {
                        return [elem[0], elem[1]];
                    });
                    var secondLine = data[i].data.map(function(elem) {
                        return [elem[0], elem[2]];
                    });
                    addIndicatorSeries(getIndicatorLineTitle(companyData.name, type.toUpperCase()) + ' (Line 1)', firstLine);
                    addIndicatorSeries(getIndicatorLineTitle(companyData.name, type.toUpperCase()) + ' (Line 2)', secondLine);
                } else {
                    addIndicatorSeries(getIndicatorLineTitle(companyData.name, type.toUpperCase()), companyData.data);
                }
            }
        }
        chart.redraw();
    }

    function addIndicatorSeries(name, data) {
        chart.addSeries({
            id: 'indicator-series',
            name: name,
            data: data,
            yAxis: 2
        }, false);
    }

    function getIndicatorLineTitle(companyName, type) {
        return name + ' - ' + type.toUpperCase();
    }

    function isDoubleLineIndicator(data) {
        return data[0] && data[0].length === 3;
    }

    function removeIndicatorAxis() {
        var axis = chart.get('indicator-axis');
        if (axis) {
            axis.remove();
        }
        chart.get('price-axis').update({ height: '100%' }, false);
    }

    function addIndicatorAxis() {
        chart.addAxis({
            id: 'indicator-axis',
            labels: {
                align: 'left',
                x: -3
            },
            title: 'Indicator value',
            top: '65%',
            height: '35%',
            offset: 0,
            lineWidth: 2
        }, false, false);
        chart.get('price-axis').update({ height: '60%' }, false);
    }

    function refreshChartData(data) {
        clearChart();
        for (var i = 0; i < data.length; i++) {
            chart.addSeries(data[i], false);
        }
        loadIndicatorValues($indicatorSelect.val());
        chart.hideLoading();
        initDatepickers();
    };

    function clearChart() {
        while (chart.series.length > 0) {
            chart.series[0].remove(false);
        }
    }

    function initDatepickers() {
        $('input.highcharts-range-selector', $('#chart-container'))
            .datepicker({
                format: 'yyyy-mm-dd',
                todayBtn: 'linked',
                todayHighlight: true,
                orientation: 'auto right'
            })
            .on('focus', function (e) {
                var currentDate = new Date($(this).val());
                $(this).datepicker('setDate', currentDate);
            });
    }

})();