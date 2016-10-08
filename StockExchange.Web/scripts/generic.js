
var globalFunc = {};

(function ($) {

    $.elemValueGet = function (elem) {
        var self = $(elem);

        if (self.is('input') || self.is('textarea')) {
            return self.val();
        }
        else if (self.is('select')) {
            return self.find(':selected').text();
        }
        else if (self.is('label')) {

        }
    }
    $.AppendUrlParam = function (base, key, value) {
        var sep = (base.indexOf("?") > -1) ? "&" : "?";
        return base + sep + encodeURIComponent(key) + "=" + encodeURIComponent(value);
    }

    $.ValidateAll = function () {

        var isValid = true;

        $('.error').removeClass('error');

        $.each($('.isrequired'), function (i, v) {
            if ($.elemValueGet(v) === '') {
                $(v).addClass('error');
                isValid = false;
            }
        });

        return isValid;
    }

    $.Validate = function (elem, options) {

        var isValid = true,
            isValidDate = true,
            isValidCurrency = true,
            isComplete = true,
            self = $(elem);

        $('.error').removeClass('error');

        if (options) {
            options.preValidate(elem);
        }

        // check all date formats
        $.each(self.find('.datepicker'), function (i, v) {
            var dateRegex = /^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$/;
            if ($.elemValueGet(v) != '' && !dateRegex.test($.elemValueGet(v))) {
                $(v).addClass('error');
                isValidDate = false;
            }
        });

        if (!isValidDate) {
            isValid = false;
            globalFunc.notifier.alert('Invalid date format. Please enter a valid date in the format mm/dd/yyyy.', 'error');
        }

        // check all currency formats
        $.each(self.find('.iscurrency'), function (i, v) {
            var currencyRegex = /^-?\d{1,3}(,?\d{3})*(\.\d{2})?$/;
            if ($.elemValueGet(v) != '' && !currencyRegex.test($.elemValueGet(v))) {
                $(v).addClass('error');
                isValidCurrency = false;
            }
        });

        if (!isValidCurrency) {
            isValid = false;
            globalFunc.notifier.alert('Invalid currency format. Please enter a valid amount up to two decimal places.', 'error');
        }

        // check all required fields
        $.each(self.find('.isrequired'), function (i, v) {
            if ($.elemValueGet(v) === '') {
                $(v).addClass('error');
                isComplete = false;
            }
        });

        if (!isComplete) {
            isValid = false;
            globalFunc.notifier.alert('Please fill up required fields.', 'error');
        }

        return isValid;
    }

    $.toCurrency = function (amount) {
        var n = amount.toString().split(".");
        n[0] = n[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        return parseFloat(n.join(".")).toFixed(2);
    }

    $.fn.hasScrollBarY = function () {
        return this.get(0).scrollHeight > this.height();
    }

    $.fn.hasScrollBarX = function () {
        return this.get(0).scrollWidth > this.width();
    }

    $.fn.getScrollBarWidth = function () {
        var $outer = $('<div>').css({ visibility: 'hidden', width: 100, overflow: 'scroll' }).appendTo('body'),
            widthWithScroll = $('<div>').css({ width: '100%' }).appendTo($outer).outerWidth();
        $outer.remove();
        return 100 - widthWithScroll;
    };

    //$.fn.fixDataTableResize = function () {
    //    var $scroller = $('.DTFC_ScrollWrapper'),
    //        $bodyliner = $('.DTFC_LeftBodyLiner'),
    //        $bodywrapper = $('.DTFC_LeftBodyWrapper'),
    //        $headwrapper = $('.DTFC_LeftHeadWrapper');

    //    var linerHeight = Math.ceil($('.dataTables_scrollBody').height());
    //    $bodyliner.attr('style', function (i, s) {
    //        return s + 'height: ' + linerHeight + 'px !important;'
    //    });

    //    var linerWidth = Math.ceil($bodywrapper.width() + $bodyliner.getScrollBarWidth());
    //    $bodyliner.attr('style', function (i, s) {
    //        return s + 'width: ' + linerWidth + 'px !important;'
    //    });

    //    var wrapperHeight = Math.ceil($scroller.height() - $headwrapper.height() - $bodyliner.getScrollBarWidth());
    //    $bodywrapper.attr('style', function (i, s) {
    //        return s + 'height: ' + wrapperHeight + 'px !important;'
    //    });
    //}

    //$.fn.fixFooterFreezePane = function () {
    //    if ($('.dataTables_scrollFoot').length) {
    //        $('.DTFC_LeftFootWrapper').offset({
    //            top: $('.dataTables_scrollFoot').offset().top,
    //            left: $('.DTFC_LeftBodyWrapper').offset().left
    //        });
    //    }
    //}

    $.fn.fixFooterScrollbar = function () {
        $('.dataTables_scrollBody').css({ 'overflow-x': 'hidden' });
        $('.dataTables_scrollFoot').css('overflow', 'auto');

        $('.dataTables_scrollFoot').on('scroll', function () {
            $('.dataTables_scrollBody').scrollLeft($(this).scrollLeft());
        });
    }

    $(document).ready(function () {

        Initialize();

        $('.datepicker').datepicker().on('changeDate', function () {
            $(this).datepicker('hide');
        });

        $('.datepicker').css('cursor', 'pointer');

        function generateCategory() {

            var elements = [];
            elements.push('<option></option>',
                          '<option>Financials</option>',
                          '<option>Schedule</option>',
                          '<option>Customer</option>',
                          '<option>Product Quality</option>',
                          '<option>Resources</option>',
                          '<option>3rd Party</option>',
                          '<option>Process Compliance</option>');

            return $(elements.join(""));

        }

        function generateType() {

            var elements = [];
            elements.push('<option></option>',
                          '<option>Project</option>',
                          '<option>Hp</option>',
                          '<option>External</option>');

            return $(elements.join(""));

        }

        function generatePriority() {

            var elements = [];
            elements.push('<option></option>',
                          '<option>Low</option>',
                          '<option>Normal</option>',
                          '<option>High</option>',
                          '<option>Critical</option>');

            return $(elements.join(""));

        }

        function applyNameHPDomain() {
            $('.email').each(function () {

                var input = $(this), email = input.val(), pgEmail = "@pg.com", template_name = $('<div><input type="text" class="form-control temp" disabled /> </div>');

                if (name.toLowerCase().indexOf(pgEmail) != -1) return true;

                $.ajax({
                    url: emailCheckerUrl,
                    data: JSON.stringify(email),
                    type: 'GET',
                }).success(function (name) {

                    if (name != "") {
                        input.hide();
                        input.append(template_name.find('.temp').val(name));
                    }

                }).error(function (a, b, c) {
                    console.log(a + b + c);
                });

            });

        }

        function Initialize() {

            globalFunc.bodyOverlay = $('html').overlay();
            globalFunc.notifier = $('html').Notifier();
            globalFunc.bodyOverlay.hide();

            if ($('.panel-accordion').length > 0) {
                $('.panel-accordion').accordionPanel();
            }

            $.each($('select#category-form'), function (i, v) {
                $(v).append(generateCategory());
            });

            $.each($('select#type-form'), function (i, v) {
                $(v).append(generateType());
            });

            $.each($('select#priority-form'), function (i, v) {
                $(v).append(generatePriority());
            });


        }

    });

    $.fn.extend({
        globalFunc: function () {
            this.bodyOverlay;
            this.notifier;
            this.checkEmail;
        }

    });
})(jQuery);