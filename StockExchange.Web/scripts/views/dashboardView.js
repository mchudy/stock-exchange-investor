(function ($) {
    'use strict';

    drawPieChart('stocks-by-value-chart', config.stocksByValueData.title,
        mapData(config.stocksByValueData.data));

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

    var ajaxUrlA = $('#advancers-grid').data('ajax-url');
    var columnsA = $('#advancers-grid th').DataTableColumns();
    var columnDefsA = $('#advancers-grid th').DataTableColumnDefs();
    // ReSharper disable once UnusedLocals
    var dataTableA = $('#advancers-grid').DataTable(
    {
        columns: columnsA,
        columnDefs: columnDefsA,
        ajax: {
            url: ajaxUrlA,
            contentType: 'application/json',
            type: 'POST',
            data: function (d) {
                d.filter = {
                };
                return JSON.stringify(d);
            }
        }
    });

    var ajaxUrlD = $('#decliners-grid').data('ajax-url');
    var columnsD = $('#decliners-grid th').DataTableColumns();
    var columnDefsD = $('#decliners-grid th').DataTableColumnDefs();
    // ReSharper disable once UnusedLocals
    var dataTableD = $('#decliners-grid').DataTable(
    {
        columns: columnsD,
        columnDefs: columnDefsD,
        ajax: {
            url: ajaxUrlD,
            contentType: 'application/json',
            type: 'POST',
            data: function (d) {
                d.filter = {
                };
                return JSON.stringify(d);
            }
        }
    });

    var ajaxUrlM = $('#most-grid').data('ajax-url');
    var columnsM = $('#most-grid th').DataTableColumns();
    var columnDefsM = $('#most-grid th').DataTableColumnDefs();
    // ReSharper disable once UnusedLocals
    var dataTableM = $('#most-grid').DataTable(
    {
        columns: columnsM,
        columnDefs: columnDefsM,
        ajax: {
            url: ajaxUrlM,
            contentType: 'application/json',
            type: 'POST',
            data: function (d) {
                d.filter = {
                };
                return JSON.stringify(d);
            }
        }
    });

    var ajaxUrl = $('#signal-grid').data('ajax-url');
    var columns = $('#signal-grid th').DataTableColumns();
    var columnDefs = $('#signal-grid th').DataTableColumnDefs();
    // ReSharper disable once UnusedLocals
    var dataTableSignals = $('#signal-grid').DataTable(
    {
        columns: columns,
        columnDefs: columnDefs,
        ajax: {
            url: ajaxUrl,
            contentType: 'application/json',
            type: 'POST',
            data: function (d) {
                d.filter = {
                };
                return JSON.stringify(d);
            }
        }
    });

    // ReSharper disable once UnusedLocals
    var dataTable = initCurrentStocksTable();

    function initCurrentStocksTable() {
        var columnDefsCurrent = [{
            targets: $('#current-grid th[data-column=Profit]').index(),
            render: function (data) {
                return getPriceWithIconHtml(data);
            }
        }];
        return createDataTable($('#current-grid'), columnDefsCurrent);
    }

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

})(jQuery);