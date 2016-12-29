(function ($) {
    'use strict';

    $.fn.DataTableColumns = function () {
        return $(this).map(function () {
            return {
                "name": $(this).data('column'),
                "data": $(this).data('column')
            };
        });
    };

    $.fn.DataTableColumnDefs = function () {
        return $(this).map(function (col) {
            var template = $(this).data('template');
            var templateHtml = $(template).html();
            if (template) {
                return {
                    "targets": col,
                    "createdCell": function (td, cellData, rowData) {
                        $(td).html(templateHtml.replace(/\[(.*)\]/gi, function (match, p1) {
                            return rowData[p1] === null ? '' : rowData[p1];
                        }));
                    }
                }
            }
            return null;
        });
    };

    $.fn.DataTableColumnFilters = function(o) {

        var defaults = {
            NoValueText: 'No Value',
            getValuesUrlCallback: function() { throw new Error('Not implemented'); }
        };
        var options = $.extend({}, defaults, o || {});
        var dataTable = null;
        var that = this;

        var tr = document.createElement('tr');

        $(tr).addClass('dataTableFilterRow');
        $('thead', this)
            .each(function() {
                $(this).prepend(tr);
            });

        $('thead th', this)
            .each(function() {
                var column = $(this).data('column');
                var th = document.createElement('th');
                $('thead tr.dataTableFilterRow', that)
                    .each(function() {
                        $(this).append(th);
                    });
                var title = $(this).text();
                $(th).data('column', column);
                $(th).html('<select multiple="multiple" placeholder="Search ' + title + '" />');
            });

        var filter = {
            clear: function() {

                $('.dataTables_scrollHead thead tr.dataTableFilterRow select', that)
                    .each(function() {
                        $(this).empty();
                        try {
                            $(this).multiselect('uncheckAll');
                        } catch (x) {
                            console.warn(x);
                        }

                        if (dataTable) {
                            var column = dataTable.column($(this).parent('th').data('column') + ':name');
                            column.search('');
                        }
                    });
            },
            assign: function(dt) {
                dataTable = dt;
            },

            init: function() {

                $('thead tr.dataTableFilterRow select', that)
                    .on('keyup change',
                        function() {
                            if (dataTable !== null) {
                                var value = $(this).val();
                                var search = (value !== null) ? JSON.stringify(value) : '';

                                var column = dataTable.column($(this).parent('th').data('column') + ':name');
                                if (column.search() !== search) {
                                    column
                                        .search(search)
                                        .draw();
                                }
                            }
                        });

                $('thead tr.dataTableFilterRow select', that)
                    .multiselect({
                        beforeopen: function() {
                            var thatSelect = this;
                            if ($(this).children().length === 0) {
                                if (typeof options.getValuesUrlCallback === 'function') {
                                    var url = options.getValuesUrlCallback($(this).parent().data('column'));
                                    $.get(url,
                                        function(data) {
                                            var hasEmpty = false;
                                            for (var i = 0; i < data.length; i++) {
                                                var option = $('<option></option>');
                                                if (data[i] !== null && data[i] !== '') {
                                                    var text = $.trim(data[i]);
                                                    var br = text.indexOf('\n');
                                                    if (br > -1) {
                                                        text = $.trim(text.substr(0, br));
                                                    }
                                                    option.attr('value', text);
                                                    option.text(text);
                                                    $(thatSelect).append(option);
                                                } else {
                                                    option.attr('value', '');
                                                    option.text(options.NoValueText);
                                                    if (!hasEmpty) {
                                                        $(thatSelect).append(option);
                                                    }
                                                    hasEmpty = true;
                                                }
                                            }
                                            $(thatSelect).multiselect('refresh');
                                        },
                                        'json');
                                }
                            }
                        }
                    })
                    .multiselectfilter();
                $(window).trigger('resize');
            }
        };
        return filter;
    };

}(jQuery));