(function($) {
    'use strict';

    var $companySelect = $('.company-select');

    $companySelect.select2({
        placeholder: 'Choose companies'
    });

    $("input[type='checkbox']").iCheck({
        checkboxClass: 'icheckbox_flat'
    });

    $('#StartDate').datepicker();
    $('#EndDate').datepicker();

    $('#AllCompanies')
        .on('ifChecked ', function() {
            $companySelect.prop('disabled', true);
        })
        .on('ifUnchecked', function() {
            $companySelect.prop('disabled', false);
        });

    $('#run-simulation-form').on('submit', function () {
        var buttons = $(this).find('[type="submit"]');
        if ($(this).valid()) {
            buttons.each(function(btn) {
                $(buttons[btn]).prop('disabled', true);
            });
        } else {
            buttons.each(function (btn) {
                $(buttons[btn]).prop('disabled', false);
            });
        }
    });

})(jQuery);