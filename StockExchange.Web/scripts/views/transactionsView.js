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
        ajax: {
            url: ajaxUrl,
            contentType: 'application/json',
            type: 'POST',
            data: function (d) {
                d.filter = {
                    Aa: ''
                };
                return JSON.stringify(d);
            }
        }
    });
})();