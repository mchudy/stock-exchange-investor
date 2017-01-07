(function (ns, $) {
    'use strict';
    
    /**
    * A budget info box which enables editing budget and includes refreshing
    */
    ns.BudgetInfoBox = function ($box) {
        var self = this;
        var getBudgetUrl = '/Wallet/GetBudget';
        var currencyCode = 'zł';

        initEditModal();

        /**
         * Refreshes the budget in the box
         */
        this.refresh = function() {
            $('.info-box-inner-content', $box).addClass('hidden');
            $('.spinner', $box).removeClass('hidden');

            $.getJSON(getBudgetUrl).done(function (result) {
                $('#total-budget', $box).text(formatCurrency(result.totalBudget));
                $('#free-budget', $box).text(formatCurrency(result.freeBudget));
                $('#all-stocks', $box).text(formatCurrency(result.allStocksValue));

                $('.info-box-inner-content', $box).removeClass('hidden');
                $('.spinner', $box).addClass('hidden');
            });
        }

        /**
         * Initializes modal for editing the budget
         */
        function initEditModal() {
            $('#modal-container').on('loaded.bs.modal', function () {
                $('#edit-budget-form', $(this)).on('submit', function (event) {
                    event.preventDefault();

                    if (!$(this).valid()) {
                        return;
                    }

                    var $this = $(this);
                    $.ajax({
                        url: $this.attr('action'),
                        type: $this.attr('method'),
                        data: $this.serialize()
                    }).done(function () {
                        self.refresh();
                        toastr.success('Budget has been edited');
                        $('#modal-container').modal('hide');
                    });
                });
            });
        }

        /**
         * Formats a number to a currency format
         * @param {number} value - Number
         * @returns {string} - Formatted number
         */
        function formatCurrency(value) {
            return value.toFixed(2) + ' ' + currencyCode;
        }
    };

})(window.StockExchange = window.StockExchange || {}, jQuery);