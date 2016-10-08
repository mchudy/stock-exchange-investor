(function ($) {
    $.DateFilter = function () {
        var date = new Date();
        date = new Date(date.getFullYear(), date.getMonth(), 1);
        var dateType = "month";

        return {
            setDateType: function (value) {
                dateType = value;
            },
            getDateType: function () {
                return dateType;
            },
            setDate: function (value) {
                date = value;
            },
            getDate: function () {
                return date;
            },
            onChanged: function () { },
            getPeriod: function () {
                if (dateType === "month") {
                    return {
                        startDate: new Date(date.getFullYear(), date.getMonth(), 1),
                        endDate: new Date(date.getFullYear(), date.getMonth() + 1, 0) // last day of month
                    };
                } else {
                    return {
                        startDate: new Date(date.getFullYear() - 1, 10, 1), // 1 Nov fiscal year start
                        endDate: new Date(date.getFullYear(), 9, 31) // 31 Oct fiscal year end
                    }
                }
            }
        }
    }
    $.fn.DateFilter = function (d, o) {
        var dateFilter = d;
        var defaults = {
            startDate: null
        };
        var options = $.extend({}, defaults, o || {});

        var that = this;

        var freezeEvents = false;

        var yearStartDate = options.startDate;
        if (yearStartDate.getMonth() >= 10) {
            yearStartDate = new Date(yearStartDate.getFullYear() + 1, 0, 1);
        }

        var filterTypeOptions = {
            years: {
                format: "yyyy",
                viewMode: "years",
                minViewMode: "years",
                immediateUpdates: true,
                autoclose: true,
                startDate: yearStartDate
            },
            months: {
                format: "mm/yyyy",
                viewMode: "months",
                minViewMode: "months",
                immediateUpdates: true,
                autoclose: true,
                startDate: options.startDate
            }
        };



        $("<select id=\"filterType\"><option value=\"month\">Monthly</option><option value=\"year\">FY</option></select><input type=\"text\" id=\"filterValue\"/>").appendTo(that);
        $("#filterValue", that).datepicker(filterTypeOptions.months);
        $("#filterValue", that).datepicker("setDate", dateFilter.getDate());

        $("#filterValue", that).on("changeDate", function () {
            if (!freezeEvents) {
                dateFilter.setDate($(this).datepicker("getDate"));
                if (typeof dateFilter.onChanged === "function") {
                    dateFilter.onChanged();
                }
            }
        });
        $("#filterType", that).on("change", function () {
            dateFilter.setDateType($(this).val());

            freezeEvents = true;
            if (dateFilter.getDateType() === "month") {
                $("#filterValue", that).datepicker("remove");
                $("#filterValue", that).datepicker(filterTypeOptions.months);
                $("#filterValue", that).datepicker("setDate", dateFilter.getDate());
            } else {
                $("#filterValue", that).datepicker("remove");
                $("#filterValue", that).datepicker(filterTypeOptions.years);
                $("#filterValue", that).datepicker("setDate", dateFilter.getDate());
            }
            freezeEvents = false;
            if (typeof dateFilter.onChanged === "function") {
                dateFilter.onChanged();
            }
        });

        return dateFilter;
    }
}(jQuery));