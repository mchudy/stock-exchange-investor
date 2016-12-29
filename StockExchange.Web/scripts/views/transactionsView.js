(function () {
    'use strict';
    
    $("input[type='radio']").iCheck({
        radioClass: 'iradio_flat'
    });
    $('#SelectedCompanyId').select2();

    var ajaxUrl = $('#grid').data('ajax-url');
    var columns = $('#grid th').DataTableColumns();
    var columnDefs = $('#grid th').DataTableColumnDefs();

    var dataTable = $('#grid').DataTable(
    {
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

    $('#add-transaction-form').on('submit', function (event) {
        event.preventDefault();

        var $this = $(this);
        $.ajax({
            url: $this.attr('action'),
            type: $this.attr('method'),
            data: $this.serialize()
        }).done(function () {
            toastr.success('Transaction has been added');
            dataTable.draw();
        });
    });
})();