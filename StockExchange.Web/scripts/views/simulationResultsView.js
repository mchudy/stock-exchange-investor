/**
 * View for the simulation results page
 */
jQuery.extend(jQuery.fn.dataTableExt.oSort, {
    "date-eu-pre": function (date) {
        date = date.replace(" ", "");

        if (!date) {
            return 0;
        }

        var year;
        var eu_date = date.split(/[\.\-\/]/);

        /*year (optional)*/
        if (eu_date[2]) {
            year = eu_date[2];
        }
        else {
            year = 0;
        }

        /*month*/
        var month = eu_date[1];
        if (month.length == 1) {
            month = 0 + month;
        }

        /*day*/
        var day = eu_date[0];
        if (day.length == 1) {
            day = 0 + day;
        }

        return (year + month + day) * 1;
    },

    "date-eu-asc": function (a, b) {
        return ((a < b) ? -1 : ((a > b) ? 1 : 0));
    },

    "date-eu-desc": function (a, b) {
        return ((a < b) ? 1 : ((a > b) ? -1 : 0));
    }
});
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
    
    function mapData(data) {
        return data.map(function (item) {
            return {
                name: item.name,
                y: item.value
            };
        });
    }

    var chartData = mapData(config.budgetData.data);
    initChart();
    loadChart();
    var chart;
    function initChart() {
        Highcharts.setOptions({
            global: {
                useUTC: true
            }
        });
        chart = new Highcharts.stockChart('chart-container', {
            title: {
                text: 'Budget History'
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
                            onclick: function () {
                                this.exportChart();
                            }
                        }, {
                            textKey: 'downloadJPEG',
                            onclick: function () {
                                this.exportChart({
                                    type: 'image/jpeg'
                                });
                            }
                        }]
                    }
                }
            }
        });
    }

    function loadChart() {
       
      
        for (var i = 0; i < config.budgetData.data.length; i++) {
            chart.addSeries(config.budgetData.data[i], false);
        }
        
    }

})(jQuery);