(function () {
    'use strict';

    drawPieChart('stocks-by-value-chart', config.stocksByValueData.title,
        mapData(config.stocksByValueData.data));

    function mapData(data) {
        return data.map(function(item) {
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
})();