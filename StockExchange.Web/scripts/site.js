(function() {
    'use strict';

    $('.sidebar-toggle').click(function() {
        $('.sidebar').toggleClass('hide-sidebar');
        $('.main-content').toggleClass('hide-sidebar');
    });

    $.validator.setDefaults({
        highlight: function (e, v) {
            var $formGroup = $(e).parents('.form-group');
            $formGroup.addClass('has-error')
                .find('.field-validation-error').addClass('text-danger');
            $formGroup.find('.form-control-feedback')
                .removeClass('glyphicon-ok').addClass('glyphicon-remove');
        },
        unhighlight: function (e) {
            var $formGroup = $(e).parents('.form-group');
            $formGroup.removeClass('has-error')
                .find('.field-validation-error').removeClass('text-danger');
            $formGroup.find('.form-control-feedback')
                .removeClass('glyphicon-remove').addClass('glyphicon-ok');
        }
    });

    $.fn.datepicker.defaults.format = 'dd/mm/yyyy';
    $.fn.datepicker.defaults.autoclose = true;

})();