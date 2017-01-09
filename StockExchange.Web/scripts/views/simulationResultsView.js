/**
 * View for the simulation results page
 */
(function ($) {
    'use strict';

    var $transactionLogTable = $('#simulation-transactions-grid');
    
    var columns = $('th', $transactionLogTable).DataTableColumns();

    $transactionLogTable.DataTable({
        data: config.transactionLogData,
        deferRender: true,
        columns: columns,
        responsive: true
    });

})(jQuery);