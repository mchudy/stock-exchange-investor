/**
 * View for the simulation results page
 */
(function ($) {
    'use strict';

    var $transactionLogTable = $('#simulation-transactions-grid');
    var columns = $('th', $transactionLogTable).DataTableColumns();

    $transactionLogTable.DataTable({
        data: config.transactionLogData,
        deferRender: true,
        columns: columns,
        columnDefs: [
      { type: 'date-eu', targets: 0 }
        ],
        responsive: true
    });

    var $transactionLogTable2 = $('#simulation-transactions2-grid');
    var columns2 = $('th', $transactionLogTable2).DataTableColumns();

    $transactionLogTable2.DataTable({
        data: config.transactionLogData2,
        deferRender: true,
        columns: columns2,
       
        responsive: true
    });
    
    Highcharts.stockChart('chart-container', {
        legend: {
            enabled: false
        },
        credits: {
            enabled: false
        },
        xAxis: {
            type: 'datetime',
            dateTimeLabelFormats: {
                day: '%d %b %Y'
            }
        },
        yAxis: {
            title: {
                text: 'Budget (PLN)'
            }
        },
        tooltip: {
            pointFormat: '<span style="color:{point.color}">\u25CF</span>  <b>{point.y:.2f} PLN</b><br/>'
        },
        series: [{
            data: config.budgetData.data
        }]
    });

})(jQuery);