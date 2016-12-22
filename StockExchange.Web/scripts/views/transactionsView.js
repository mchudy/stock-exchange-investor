(function() {
    'use strict';

    var $transactionsTable = $('#transactions-table-container');

    loadTable();

    $("input[type='radio']").iCheck({
        radioClass: 'iradio_flat'
    });

    $('#SelectedCompanyId').select2();

    $('#add-transaction-form').on('submit', function(event) {
        event.preventDefault();

        var $this = $(this);
        $.ajax({
            url: $this.attr('action'),
            type: $this.attr('method'),
            data: $this.serialize()
        }).done(function () {
            loadTable();
        }).fail(function(data) {
            console.error('Could not add transaction');
            console.log(data);
        });
    });

    function loadTable() {
        $.get(config.getTransactionsTableUrl)
            .done(function (data) {
                $transactionsTable.html(data);
            });
    }
})();