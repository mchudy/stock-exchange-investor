(function () {
    'use strict';
    
    $("input[type='radio']").iCheck({
        radioClass: 'iradio_flat'
    });
    $('#SelectedCompanyId').select2();

    $('#AddTransactionViewModel_Date').datepicker({
        format: 'mm/dd/yyyy',
        autoclose: true,
        enddate: new Date(),
        defaultDate: new Date()
    });

    $.post('/Transactions/GetBudget/', function (result) {
        var options = $('#companieslist');
        $.each(result.Companies, function () {
            options.append('<option value="' + this.Id + '">' + this.Code + '</option>');
        });
        $('#totalbudget').text(result.TotalBudget.toFixed(2));
        $('#freebudget').text(result.FreeBudget.toFixed(2));
        $('#allstocks').text(result.AllStocksValue.toFixed(2));
    });

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

    var ajaxUrlCurrent = $('#current-grid').data('ajax-url');
    var columnsCurrent = $('#current-grid th').DataTableColumns();
    var columnDefsCurrent = $('#currentgrid th').DataTableColumnDefs();

    var dataTableCurrent = $('#current-grid').DataTable(
    {
        columns: columnsCurrent,
        columnDefs: columnDefsCurrent,
        responsive: true,
        ajax: {
            url: ajaxUrlCurrent,
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
            dataTableCurrent.draw();
            $.post('/Transactions/GetBudget/', function (result) {
                var options = $('#companieslist');
                $.each(result.Companies, function () {
                    options.append('<option value="' + this.Id + '">' + this.Code + '</option>');
                });
                $('#totalbudget').text(result.TotalBudget.toFixed(2));
                $('#freebudget').text(result.FreeBudget.toFixed(2));
                $('#allstocks').text(result.AllStocksValue.toFixed(2));
            });
        });
    });

    $('#edit-budget-form').on('submit', function (event) {
        event.preventDefault();

        var $this = $(this);
        $.ajax({
            url: $this.attr('action'),
            type: $this.attr('method'),
            data: $this.serialize()
        }).done(function () {
            toastr.success('Budget has been edited');
            $.post('/Transactions/GetBudget/', function (result) {
                var options = $('#companieslist');
                $.each(result.Companies, function () {
                    options.append('<option value="' + this.Id + '">' + this.Code + '</option>');
                });
                $('#totalbudget').text(result.TotalBudget.toFixed(2));
                $('#freebudget').text(result.FreeBudget.toFixed(2));
                $('#allstocks').text(result.AllStocksValue.toFixed(2));
            });
        });
    });

})();