(function ($) {

    var ajaxUrl = $("#grid").data("ajax-url");
    var companyName = "";

    var ajaxFilterUrl = $("#grid").data("filter-ajax-url");
    var ajaxFilterParamName = $("#grid").data("filter-ajax-paramname");

    var dateFilter = $.DateFilter();

    function getFilterValuesUrl(filterParamValue) {
        var search = $("#grid-container [type=search]").val();
        var url = $.AppendUrlParam(ajaxFilterUrl, ajaxFilterParamName, filterParamValue);
        var params = dateFilter.getPeriod();
        url = $.AppendUrlParam(url, "Filter.StartDate", params.startDate.toISOString());
        url = $.AppendUrlParam(url, "Filter.EndDate", params.endDate.toISOString());
        url = $.AppendUrlParam(url, "Filter.CompanyName", companyName);
        url = $.AppendUrlParam(url, "Search.Value", search);
        return url;
    }

    var columns = $("#grid th").DataTableColumns();
    var columnDefs = $("#grid th").DataTableColumnDefs();
    var columnFilters = $("#grid-container").DataTableColumnFilters({
        getValuesUrlCallback: getFilterValuesUrl
    });

    var dataTable = $("#grid").DataTable(
    {
        "columns": columns,
        "columnDefs": columnDefs,
        "fixedColumns": {
            "drawCallback": function () { columnFilters.init() },
            "leftColumns": 1
        },
        "ajax": {
            "url": ajaxUrl,
            "contentType": "application/json",
            "type": "POST",
            "data": function (d) {
                var params = dateFilter.getPeriod();
                d.filter = {
                    startDate: params.startDate,
                    endDate: params.endDate,
                    companyName: companyName
                };
                return JSON.stringify(d);
            }
        }
    });
    columnFilters.assign(dataTable);

    /* Filters */
    $("#grid_filter").append($("#filters").html());
    var dateFilterControl = $("#grid_filter #dateFilter").DateFilter(dateFilter, {
        startDate: new Date(2014, 10, 1)
    });

    dateFilterControl.onChanged = function () {
        columnFilters.clear();
        dataTable.draw();
    }

    $("#reset").on("click", function () {
        columnFilters.clear();
    });

    $("#grid-container [type=search]").on("change", function () {
        columnFilters.clear();
    });

    $("#grid-container #companyName").on("change", function () {
        companyName = $(this).val();
        dataTable.draw();
    });

    /* Edit */
    function modalEditInit() {
        $("#modalEdit .modal-content .datepicker").datepicker({
            format: "mm/yyyy",
            forceParse: false,
            viewMode: "months",
            minViewMode: "months",
            immediateUpdates: true,
            autoclose: true
        });
        $("#modalEdit .modal-content .datepicker").each(function () {
            $(this).datepicker("setDate", new Date($(this).val()));
        });
    }

    $("#modalEdit").on("shown.bs.modal", function () {
        modalEditInit();
    });

    $(document).on("click", "#grid-container .edit", function () {
        var url = $(this).data("href");
        globalFunc.bodyOverlay.show();
        $.get(url, function (result) {
            $("#modalEdit .modal-content").html(result);
            $.validator.unobtrusive.parse("#modalEdit .modal-content");
            $("#modalEdit").modal("show");

        }).always(function () {
            globalFunc.bodyOverlay.hide();
        });
        return false;
    });

    /* Export */
    $("#export").click(function () {
        var search = $("#grid-container [type=search]").val();
        var params = dateFilter.getPeriod();
        window.location = $("#export").data("url") + "?Filter.StartDate=" + params.startDate.toISOString() + "&Filter.EndDate=" + params.endDate.toISOString() + "&Filter.companyName=" + companyName + "&Search.Value=" + search;
    });

})(jQuery);
