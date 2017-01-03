(function ($) {
    'use strict';

    drawPieChart('stocks-by-value-chart', config.stocksByValueData.title,
        mapData(config.stocksByValueData.data));

    createDataTable($('#signal-grid'));
    createDataTable($('#advancers-grid'));
    createDataTable($('#decliners-grid'));
    createDataTable($('#most-grid'));
    initCurrentStocksTable();

    function mapData(data) {
        return data.map(function (item) {
            return {
                name: item.name,
                y: item.value
            };
        });
    }

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

    function initCurrentStocksTable() {
        var columnDefsCurrent = [{
            targets: $('#current-grid th[data-column=Profit]').index(),
            render: function (data) {
                return getPriceWithIconHtml(data);
            }
        }];
        return createDataTable($('#current-grid'), columnDefsCurrent);
    }

    //TODO: extract to common StockExchange namespace
    function getPriceWithIconHtml(value, down) {
        if (down || value < 0) {
            return '<i class="fa fa-arrow-down icon-stock-down"></i>' +
                ' <span class="text-danger">' + value + '</span>';
        } else {
            return '<i class="fa fa-arrow-up icon-stock-up"></i>' +
                ' <span class="text-success">' + value + '</span>';
        }
    }

    function createDataTable($selector, columnDefs) {
        var ajaxUrl = $selector.data('ajax-url');
        var columns = $('th', $selector).DataTableColumns();

        return $selector.DataTable({
            columns: columns,
            columnDefs: columnDefs,
            responsive: true,
            ajax: {
                url: ajaxUrl,
                contentType: 'application/json',
                type: 'POST',
                data: function (d) {
                    return JSON.stringify(d);
                }
            }
        });
    }
    //END TODO

})(jQuery);