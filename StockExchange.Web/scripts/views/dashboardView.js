(function () {
    'use strict';

    drawPieChart('stocks-by-value-chart', config.stocksByValueData.title,
        mapData(config.stocksByValueData.data));

    //TODO: extract to a component
    $('#modal-container').on('loaded.bs.modal', function () {
        $('#edit-budget-form').on('submit', function (event) {
            event.preventDefault();

            var $this = $(this);
            $.ajax({
                url: $this.attr('action'),
                type: $this.attr('method'),
                data: $this.serialize()
            }).done(function () {
                refreshBudget();
                toastr.success('Budget has been edited');
                $('#modal-container').modal('hide');
            });
        });
    });

    function refreshBudget() {
        $.getJSON('/Wallet/GetBudget/').done(function (result) {
            $('#total-budget').text(result.totalBudget.toFixed(2));
            $('#free-budget').text(result.freeBudget.toFixed(2));
            $('#all-stocks').text(result.allStocksValue.toFixed(2));
        });
    }

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
})();