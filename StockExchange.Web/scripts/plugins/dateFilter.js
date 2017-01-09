/**
 * jQuery plugin for date filtering in DataTables
 */
(function ($) {
    'use strict';

    /**
     * The DateFilter object
     * @returns {} 
     */
    $.DateFilter = function () {
        var date = new Date();
        date = new Date(date.getFullYear(), date.getMonth(), 1);
        var dateType = 'month';

        return {
            /**
             * Sets the date type
             * @param {string} value - New date type
             */
            setDateType: function (value) {
                dateType = value;
            },

            /**
             * Gets the date type
             * @returns {string} - The date type
             */
            getDateType: function () {
                return dateType;
            },

            /**
             * Sets the date
             * @param {string} value - New date
             */
            setDate: function (value) {
                date = value;
            },

            /**
             * Sets the date
             * @returns  {string} - The date
             */
            getDate: function () {
                return date;
            },

            /**
             * Invoked when the date value changes
             */
            onChanged: function () { },

            /**
             * Gets the chosen period
             * @returns {Object} - The chosen date period
             */
            getPeriod: function () {
                if (dateType === 'month') {
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

    /**
     * The DateFilter jQuery plugin
     * @param {Object} d - A DateFilter object
     * @param {Object} o - Initial configuration
     * @returns {Object} - A DateFilter object
     */
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
                format: 'yyyy',
                viewMode: 'years',
                minViewMode: 'years',
                immediateUpdates: true,
                autoclose: true,
                startDate: options.startDate
            },
            months: {
                format: 'mm/yyyy',
                viewMode: 'months',
                minViewMode: 'months',
                immediateUpdates: true,
                autoclose: true,
                startDate: options.startDate
            }
        };

        $('<select id="filterType" class="form-control"><option value="month">Monthly</option><option value="year">Yearly</option></select><input type="text" class="form-control" id="filterValue"/>').appendTo(that);

        var $filterValue = $('#filterValue', that);
        $filterValue.datepicker(filterTypeOptions.months);
        $filterValue.datepicker('setDate', dateFilter.getDate())
            .on('changeDate', function() {
                if (!freezeEvents) {
                    dateFilter.setDate($(this).datepicker('getDate'));
                    if (typeof dateFilter.onChanged === 'function') {
                        dateFilter.onChanged();
                    }
                }
            });

        $('#filterType', that).on('change', function () {
            dateFilter.setDateType($(this).val());

            freezeEvents = true;
            if (dateFilter.getDateType() === 'month') {
                $('#filterValue', that).datepicker('remove');
                $('#filterValue', that).datepicker(filterTypeOptions.months);
                $('#filterValue', that).datepicker('setDate', dateFilter.getDate());
            } else {
                $('#filterValue', that).datepicker('remove');
                $('#filterValue', that).datepicker(filterTypeOptions.years);
                $('#filterValue', that).datepicker('setDate', dateFilter.getDate());
            }
            freezeEvents = false;
            if (typeof dateFilter.onChanged === 'function') {
                dateFilter.onChanged();
            }
        });

        return dateFilter;
    }
}(jQuery));