/**
 * A view for the wallet main page
 */
(function (StockExchange, $) {
    'use strict';

    var deleteTransactionUrl = 'Wallet/DeleteTransaction';

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

    var budgetBox = new StockExchange.BudgetInfoBox($('.budget-infobox'));
    var transactionsTable = initTransactionsTable();
    var currentStocksTable = initCurrentStocksTable();

    bindDeleteTransactionModal();

    /*
     * Send AJAX request for adding a new transaction
     */
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
            budgetBox.refresh();
            transactionsTable.draw();
            currentStocksTable.draw();
        }).always(function() {
            $('#add-transaction-button').prop('disabled', false);
        });
    });

    /**
     * Initializes the transactions table
     * @returns {Object} - The created DataTables object
     */
    function initTransactionsTable() {
        var columnDefs =[{
            targets: $('#grid th[data-column=Total]').index(),
            render: function (data, type, full) {
                return StockExchange.getPriceWithIconHtml(data, full.Action === 'Buy');
            }
        }, {
            targets: $('#grid th.delete-column').index(),
            render: function (data, type, full) {
                var url = deleteTransactionUrl + '/' + full.Id;
                return '<i class="fa fa-remove delete-transaction" data-id=' + full.ID +
                    '" data-toggle="modal" data-target="#confirm-delete-modal" data-url="' + url + '"></i>';
            }
        }];
        return StockExchange.createDataTable($('#grid'), columnDefs);
    }

    /**
     * Initializes the current stocks table
     * @returns {Object} - The created DataTables object
     */
    function initCurrentStocksTable() {
        var columnDefsCurrent = [{
            targets: $('#current-grid th[data-column=Profit]').index(),
            render: function (data) {
                return StockExchange.getPriceWithIconHtml(data);
            }
        }];
        return StockExchange.createDataTable($('#current-grid'), columnDefsCurrent);
    }

    /**
     * Binds actions to be performed when clicking on the delete transaction button
     */
    function bindDeleteTransactionModal() {
        $('#confirm-delete-modal').on('show.bs.modal', function (e) {
            var target = $(e.relatedTarget);
            var url = target.data('url');
            $('.btn-confirm-delete', this).data('url', url);
        });

        $('.btn-confirm-delete').on('click', function () {
            var url = $(this).data('url');
            $.ajax(url, {
                type: 'POST'
            }).done(function() {
                transactionsTable.draw();
                currentStocksTable.draw();
                budgetBox.refresh();
            });
        });
    }

})(StockExchange, jQuery);