(function() {
    'use strict';

    initSidebarToggle();
    setValidatorDefaults();
    setDatepickerDefaults();
    initModals();

    function initSidebarToggle() {
        $('.sidebar-toggle').click(function () {
            $('.sidebar').toggleClass('hide-sidebar');
            $('.main-content').toggleClass('hide-sidebar');
        });
    }

    function setValidatorDefaults() {
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
    }

    function setDatepickerDefaults() {
        $.fn.datepicker.defaults.format = 'dd/mm/yyyy';
        $.fn.datepicker.defaults.autoclose = true;
    }

    function initModals() {
        $('body').on('click', '.modal-link', function (e) {
            e.preventDefault();
            $(this).attr('data-target', '#modal-container');
            $(this).attr('data-toggle', 'modal');
        });
        $('body').on('click', '.modal-close-btn', function () {
            $('#modal-container').modal('hide');
        });
        $('#modal-container').on('hidden.bs.modal', function () {
            $(this).removeData('bs.modal');
        });
        $('#CancelModal').on('click', function () {
            return false;
        });
    }

})();