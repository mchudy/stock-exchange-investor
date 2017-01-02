(function ($) {
    'use strict';
    
    $("input[type='radio']").iCheck({
        radioClass: 'iradio_flat'
    });
    $('#SelectedCompanyId').select2();
    $('.company-select').select2({
        height: '100%'
    });

    $('#AddTransactionViewModel_Date').datepicker({
        endDate: new Date()
    });

    $('.budget-infobox').trigger('refresh');

    var dataTable = initTransactionsTable();
    var dataTableCurrent = initCurrentStocksTable();

    $('#add-transaction-form').on('submit', function (event) {
        event.preventDefault();

        var $this = $(this);
        if (!($this.valid())) {
            return;
        }

        $('#add-transaction-button').prop('disabled', true);

        $.ajax({
            url: $this.attr('action'),
            type: $this.attr('method'),
            data: $this.serialize()
        }).done(function () {
            toastr.success('Transaction has been added');
            $('.budget-infobox').trigger('box.refresh');
            dataTable.draw();
            dataTableCurrent.draw();
        }).always(function() {
            $('#add-transaction-button').prop('disabled', false);
        });
    });

    function initTransactionsTable() {
        var columnDefs =[{
            targets: $('#grid th[data-column=Total]').index(),
            render: function (data, type, full) {
                return getPriceWithIconHtml(data, full.Action === 'Sell');
            }
        }];
        return createDataTable($('#grid'), columnDefs);
    }

    function initCurrentStocksTable() {
        var columnDefsCurrent = [{
            targets: $('#current-grid th[data-column=Profit]').index(),
            render: function (data) {
                return getPriceWithIconHtml(data);
            }
        }];
        return createDataTable($('#current-grid'), columnDefsCurrent);
    }

    function createDataTable($selector, columnDefs) {
        var ajaxUrl = $selector.data('ajax-url');
        var columns = $('th', $selector).DataTableColumns();

        return $selector.DataTable({
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
    }

    function getPriceWithIconHtml(value, down) {
        if (down || value < 0) {
            return '<i class="fa fa-arrow-down icon-stock-down"></i>' +
                ' <span class="text-danger">' + value + '</span>';
        } else {
            return '<i class="fa fa-arrow-up icon-stock-up"></i>' +
                ' <span class="text-success">' + value + '</span>';
        }
    }

})(jQuery);