$(function () {
    'use strict';

    Highcharts.chart('owned-stocks-pie-chart', {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie',
            height: 300
        },
        title: {
            text: ''
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
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
            data: [{
                name: 'ASSECO',
                y: 60
            }, {
                name: 'ALIOR',
                y: 40
            }, {
                name: 'MBANK',
                y: 20
            }]
        }]
    });
})();