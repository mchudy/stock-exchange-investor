(function (StockExchange, $) {
    'use strict';

    // ReSharper disable once UnusedLocals
    var budgetBox = new StockExchange.BudgetInfoBox($('.budget-infobox'));

    drawPieChart('stocks-by-value-chart', config.stocksByValueData.title,
        mapData(config.stocksByValueData.data));

    StockExchange.createDataTable($('#signal-grid'));
    StockExchange.createDataTable($('#advancers-grid'));
    StockExchange.createDataTable($('#decliners-grid'));
    StockExchange.createDataTable($('#most-grid'));
    initCurrentStocksTable();

    /**
     * Maps the data loaded from the server to Highcharts format
     * @param {Array} data 
     * @returns {Array} - Transformed data
     */
    function mapData(data) {
        return data.map(function (item) {
            return {
                name: item.name,
                y: item.value
            };
        });
    }

    /**
     * Draws the owned stocks pie chart
     * @param {jQuery} element - Container for the chart
     * @param {string} title - Chart title
     * @param {Array} data - Chart data
     */
    function drawPieChart(element, title, data) {
        Highcharts.chart(element, {
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false,
                type: 'pie',
                height: 300
            },
            title: {
                text: title
            },
            credits: {
                enabled: false
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.y:.2f}</b> ({point.percentage:.2f}%)'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: false
                    },
                    showInLegend: true
                }
            },
            series: [{
                name: 'Company',
                colorByPoint: true,
                data: data
            }]
        });
    }

    /**
     * Initializes the currently owned stocks table
     * @returns {Object} - A created DataTables object
     */
    function initCurrentStocksTable() {
        var columnDefsCurrent = [{
            targets: $('#current-grid th[data-column=Profit]').index(),
            render: function (data) {
                return StockExchange.getPriceWithIconHtml(data);
            }
        }];
        return StockExchange.createDataTable($('#current-grid'), columnDefsCurrent);
    }

})(StockExchange, jQuery);