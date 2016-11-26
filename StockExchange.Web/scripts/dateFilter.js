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
                        endDate: new Date(date.getFullYear(), date.getMonth() + 1, 0) 
                    };
                } else {
                    return {
                        startDate: new Date(date.getFullYear(), 0, 1), 
                        endDate: new Date(date.getFullYear(), 11, 31)
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

        var filterTypeOptions = {
            years: {
                format: "yyyy",
                viewMode: "years",
                minViewMode: "years",
                immediateUpdates: true,
                autoclose: true,
                startDate: options.startDate
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

        $("<select id=\"filterType\" class=\"form-control\"><option value=\"month\">Monthly</option><option value=\"year\">Yearly</option></select><input type=\"text\" class=\"form-control\" id=\"filterValue\"/>").appendTo(that);

        var $filterValue = $('#filterValue', that);
        $filterValue.datepicker(filterTypeOptions.months);
        $filterValue.datepicker('setDate', dateFilter.getDate())
            .on('changeDate', function () {
                if (!freezeEvents) {
                    dateFilter.setDate($(this).datepicker('getDate'));
                    if (typeof dateFilter.onChanged === 'function') {
                        dateFilter.onChanged();
                    }
                }
            })
            .on('show', function() {
                $('.datepicker').addClass('calendar');
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