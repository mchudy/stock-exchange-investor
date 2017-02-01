(function($) {
    'use strict';

    var chart;
    var loadingIndicator = '<div class="spinner"></div>';
    var $companySelect = $('.company-select');
    var $companyGroupSelect = $('.company-group-select');
    var $indicatorSelect = $('.indicator-select');
    var $isCandleStickCheckbox = $('#is-candlestick-chart');
    var $refreshBtn = $('.refresh-chart');
    var chosenCompanies = $companySelect.val();

    bindUIElements();
    initChart();
    loadChart();

    /**
     * Initializes and binds events to the UI elements used for manipulating 
     * the chart
     */
    function bindUIElements() {
        $companySelect.select2({
            placeholder: 'Choose companies',
            width: '100%'
        });

        $companySelect.trigger('change');
        $companySelect.on('change', function () {
            chosenCompanies = $(this).val();
            loadChart();
        });

        $companyGroupSelect.on('change', function () {
            var $selected = $(this).find(':selected');
            var companies = $selected.data('companies');

            $('option', $companySelect).each(function () {
                var value = parseInt($(this).val());
                if (companies && $.inArray(value, companies) < 0) {
                    $(this).prop('disabled', true).prop('selected', false);
                } else {
                    $(this).prop('disabled', false);
                }
            });
            $companySelect.select2({
                placeholder: 'Choose companies',
                width: '100%'
            });
        });

        $indicatorSelect.on('change', function () {
            var type = $(this).val();

            $('.indicator-properties').addClass('hidden');
            $refreshBtn.addClass('hidden');

            if (type) {
                var props = $('.indicator-properties[data-type="' + type + '"]');
                props.removeClass('hidden');
                if (props.length > 0) {
                    $refreshBtn.removeClass('hidden');
                }
            }

            loadIndicatorValues(type);
        });

        $isCandleStickCheckbox.on('change', function () {
            loadChart();
        });

        $refreshBtn.on('click', function () {
            loadChart();
        });
    }

    /**
     * Initialized the stock chart
     * @returns {Object} - Highcharts chart object
     */
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
            credits: {
                enabled: false
            },
            yAxis: [{
                id: 'price-axis'
            }],
            tooltip: {
                pointFormat: '<span style="color:{point.color}">\u25CF</span> {series.name}: <b>{point.y:.2f}</b><br/>'
            },
            rangeSelector: {
                inputDateFormat: '%Y-%m-%d',
                inputEditDateFormat: '%Y-%m-%d',
                inputDateParser: function (value) {
                    var date = new Date(value);
                    // hack for dealing with timezone issue
                    date.setTime(date.getTime() + 1 * 1000 * 60 * 60 * 4);
                    return date.getTime();
                }
            },
        

        exporting: {
                buttons: {
                    contextButton: {
                        menuItems: [
                        {
                            textKey: 'downloadPNG',
                            onclick: function() {
                                this.exportChart();
                            }
                        }, {
                            textKey: 'downloadJPEG',
                            onclick: function() {
                                this.exportChart({
                                    type: 'image/jpeg'
                                });
                            }
                        }]
                    }
                }
            }
        });
        chart.showLoading(loadingIndicator);
    }

    /**
     * Loads the chart data
     */
    function loadChart() {
        if (!chosenCompanies || !chosenCompanies.length) {
            clearChart();
            return;
        }

        chart.showLoading(loadingIndicator);

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

    /**
     * Loads values for the given indicator
     * @param {string} type - Indicator type
     */
    function loadIndicatorValues(type) {
        if (!type) {
            removeIndicatorAxis();
            chart.redraw();
            chart.hideLoading();
            return;
        }
        chart.showLoading(loadingIndicator);

        var properties = getIndicatorProperties();
        var params = $.param({
                type: type,
                companyIds: chosenCompanies
            }, true); // ASP.NET requires traditional param for lists
        params += '&' + convertPropertiesToUrl(properties);

        $.get(config.getIndicatorValuesUrl, params)
            .done(function (data) {
                drawIndicatorValues(type, data);
            });
    }

    /**
     * Extracts current indicator properties values from the HTML
     * @returns {Object} - Current indicator properties
     */
    function getIndicatorProperties() {
        var properties = [];
        $('.indicator-property:visible').each(function () {
            properties.push({
                name: $(this).data('name'),
                value: $(this).find('.property-value').val()
            });
        });
        return properties;
    }

    /**
     * Converts properties to an URL format recognized by ASP.NET MVC
     * @param {Object} properties - Indicator properties
     * @returns {string} - URL formatted properties
     */
    function convertPropertiesToUrl(properties) {
        var url = '';
        for (var i = 0; i < properties.length; i++) {
            var property = properties[i];
            url += 'properties[' + i + '].name=' + property.name +
                '&properties[' + i + '].value=' + property.value + '&';
        }
        return url;
    }

    /**
     * Draws the lower chart with the indicator values
     * @param {string} type - Indicator type
     * @param {Array<Object>} data - Indicator values data returned from the server
     */
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
        chart.hideLoading();
    }

    /**
     * Adds indicator series to the chart
     * @param {string} name - Name of the series
     * @param {Array} data - Chart data
     */
    function addIndicatorSeries(name, data) {
        chart.addSeries({
            id: 'indicator-series',
            name: name,
            data: data,
            yAxis: 2
        }, false);
    }

    /**
     * Constructs a title for an indicator series
     * @param {string} companyName - Name of the company
     * @param {string} type - Indicator type
     * @returns {string} - Title for the chart series
     */
    function getIndicatorLineTitle(companyName, type) {
        return companyName + ' - ' + type.toUpperCase();
    }

    /**
     * Checks whether the indicator has two lines
     * @param {Array} data - Indicator chart data
     * @returns {boolean} 
     */
    function isDoubleLineIndicator(data) {
        return data[0] && data[0].length === 3;
    }

    /**
     * Removes the indicator axis from the chart
     */
    function removeIndicatorAxis() {
        var axis = chart.get('indicator-axis');
        if (axis) {
            axis.remove();
        }
        chart.get('price-axis').update({ height: '100%' }, false);
    }

    /**
     * Adds a new axis for the indicator series
     */
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

    /**
     * Refreshes the whole chart
     * @param {Array} data - Data for the chart (stocks value, not indicators)
     */
    function refreshChartData(data) {
        clearChart();
        for (var i = 0; i < data.length; i++) {
            chart.addSeries(data[i], false);
        }
        loadIndicatorValues($indicatorSelect.val());
        initDatepickers();
    };

    /**
     * Clears the chart
     */
    function clearChart() {
        while (chart.series.length > 0) {
            chart.series[0].remove(false);
        }
    }

    /**
     * Initializes the datepickers for the chart
     */
    function initDatepickers() {
        $('input.highcharts-range-selector', $('#chart-container'))
            .datepicker({
                format: 'yyyy-mm-dd',
                todayBtn: 'linked',
                todayHighlight: true,
                orientation: 'auto right'
            })
            .on('focus', function () {
                var currentDate = new Date($(this).val());
                $(this).datepicker('setDate', currentDate);
            });
    }

})(jQuery);