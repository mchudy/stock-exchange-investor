(function ($) {
    'use strict';

    $.AppendUrlParam = function (base, key, value) {
        var sep = (base.indexOf('?') > -1) ? '&' : '?';
        return base + sep + encodeURIComponent(key) + '=' + encodeURIComponent(value);
    }
    var ajaxUrl = $('#grid').data('ajax-url');
    var companyName = '';
    var ajaxFilterUrl = $('#grid').data('filter-ajax-url');
    var ajaxFilterParamName = $('#grid').data('filter-ajax-paramname');
    var dateFilter = $.DateFilter();
    function getFilterValuesUrl(filterParamValue) {
        var search = $('#grid-container [type=search]').val();
        var url = $.AppendUrlParam(ajaxFilterUrl, ajaxFilterParamName, filterParamValue);
        var params = dateFilter.getPeriod();
        url = $.AppendUrlParam(url, 'Filter.StartDate', params.startDate.toISOString());
        url = $.AppendUrlParam(url, 'Filter.EndDate', params.endDate.toISOString());
        url = $.AppendUrlParam(url, 'Filter.CompanyName', companyName);
        url = $.AppendUrlParam(url, 'Search.Value', search);
        return url;
    }
    var columns = $('#grid th').DataTableColumns();
    var columnDefs = $('#grid th').DataTableColumnDefs();
    var columnFilters = $('#grid-container').DataTableColumnFilters({
        getValuesUrlCallback: getFilterValuesUrl
    });
    columnFilters.init();

    var dataTable = $('#grid').DataTable(
    {
        columns: columns,
        columnDefs: columnDefs,
        ajax: {
            url: ajaxUrl,
            contentType: 'application/json',
            type: 'POST',
            data: function (d) {
                var params = dateFilter.getPeriod();
                d.filter = {
                    startDate: params.startDate,
                    endDate: params.endDate,
                    companyName: companyName
                };
                return JSON.stringify(d);
            }
        },
        dom: "<'row'<'col-sm-2'l><'col-sm-10'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>"
    });
    columnFilters.assign(dataTable);

    /* Filters */
    $('#grid_filter').append($('#filters').html());
    var dateFilterControl = $('#grid_filter #dateFilter').DateFilter(dateFilter, {
        startDate: new Date(2006, 0, 1)
    });
    dateFilterControl.onChanged = function () {
        columnFilters.clear();
        dataTable.draw();
    }
    $('#grid-container [type=search]').on('change', function () {
        columnFilters.clear();
    });
    $('#grid-container #companyName').on('change', function () {
        companyName = $(this).val();
        dataTable.draw();
    });
})(jQuery);
