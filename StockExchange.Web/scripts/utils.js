(function (ns, $) {
    'use strict';

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
        ns.createDataTable = function($selector, columnDefs) {
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
                    data: function(d) {
                        return JSON.stringify(d);
                    }
                }
            });
        }
    }
})(window.StockExchange = window.StockExchange || {}, jQuery)