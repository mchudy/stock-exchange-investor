/**
 * General util functions
 */
(function (ns, $) {
    'use strict';

    /**
     * Returns HTML with an arrow and number in appropriate color
     * (depending whether there was an increase or decrease)
     * 
     * @param {number} value - The price value
     * @param {boolean} down - Indicates that the price has decreased
     * @returns {string} - HTML
     */
    ns.getPriceWithIconHtml = function(value, down) {
        if (down || value < 0) {
            return '<i class="fa fa-arrow-down icon-stock-down"></i>' +
                ' <span class="text-danger">' + value + '</span>';
        } else {
            return '<i class="fa fa-arrow-up icon-stock-up"></i>' +
                ' <span class="text-success">' + value + '</span>';
        }
    }

    if ($.fn.DataTable) {
        /**
         * Creates a new DataTable
         * 
         * @param {jQuery} $selector - Table
         * @param {Object} columnDefs - Column definitions
         * @returns {Object} - DataTable object
         */
        ns.createDataTable = function($selector, columnDefs) {
            var ajaxUrl = $selector.data('ajax-url');
            var columns = $('th', $selector).DataTableColumns();

            return $selector.DataTable({
                deferRender: true,
                columns: columns,
                columnDefs: columnDefs,
                responsive: true,
                ajax: {
                    url: ajaxUrl,
                    contentType: 'application/json',
                    type: 'POST',
                    data: function(d) {
                        return JSON.stringify(d);
                    }
                }
            });
        }
    }
})(window.StockExchange = window.StockExchange || {}, jQuery)