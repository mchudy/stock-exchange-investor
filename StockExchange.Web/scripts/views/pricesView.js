(function ($) {
    'use strict';

    var ajaxUrl = $('#grid').data('ajax-url');
    var companyName = '';
    var ajaxFilterUrl = $('#grid').data('filter-ajax-url');
    var ajaxFilterParamName = $('#grid').data('filter-ajax-paramname');
    var dateFilter = $.DateFilter();
    var columns = $('#grid th').DataTableColumns();
    var columnDefs = $('#grid th').DataTableColumnDefs();
    var columnFilters = $('#grid-container').DataTableColumnFilters({
        getValuesUrlCallback: getFilterValuesUrl
    });
    var dataTable;

    columnFilters.init();
    initDataTables();
    initFilters();

    /**
     * Initializes the prices DataTable
     * @returns {} 
     */
    function initDataTables() {
        dataTable = $('#grid').DataTable(
        {
            columns: columns,
            columnDefs: columnDefs,
            deferRender: true,
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
    }

    /**
     * Initializes the DataTables filters
     */
    function initFilters() {
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
    }

    /**
     * Returns URL with applied filter values
     * @param {string} filterParamValue - Value for the filter
     * @returns {string} - URL
     */
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

    /**
     * Appends a parameter to a URL
     * @param {string} base - Base URL
     * @param {string} key - Parameter name
     * @param {string} value - Parameter value
     * @returns {string} - The new URL
     */
    $.AppendUrlParam = function (base, key, value) {
        var sep = (base.indexOf('?') > -1) ? '&' : '?';
        return base + sep + encodeURIComponent(key) + '=' + encodeURIComponent(value);
    }

})(jQuery);
