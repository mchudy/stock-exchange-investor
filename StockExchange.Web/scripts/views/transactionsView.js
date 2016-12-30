(function () {
    'use strict';
    
    $("input[type='radio']").iCheck({
        radioClass: 'iradio_flat'
    });
    $('#SelectedCompanyId').select2();

    $('#AddTransactionViewModel_Date').datepicker({
        format: 'mm/dd/yyyy',
        endDate: '+0d',
        defaultDate: new Date()
    });
    refreshBudget();

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
            refreshBudget();
            toastr.success('Transaction has been added');
            dataTable.draw();
            dataTableCurrent.draw();
        });
    });

    //TODO: extract to a component
    $('#modal-container').on('loaded.bs.modal', function (e) {
        $('#edit-budget-form').on('submit', function (event) {
            event.preventDefault();

            var $this = $(this);
            $.ajax({
                url: $this.attr('action'),
                type: $this.attr('method'),
                data: $this.serialize()
            }).done(function () {
                toastr.success('Budget has been edited');
                $('#modal-container').modal('hide');
                refreshBudget();
            });
        });
    });

    function refreshBudget() {
        $.getJSON('/Transactions/GetBudget/').done(function (result) {
            var options = $('#companieslist');
            $.each(result.companies, function () {
                options.append('<option value="' + this.id + '">' + this.code + '</option>');
            });
            $('#total-budget').text(result.totalBudget.toFixed(2));
            $('#free-budget').text(result.freeBudget.toFixed(2));
            $('#all-stocks').text(result.allStocksValue.toFixed(2));
        });
    }

})();