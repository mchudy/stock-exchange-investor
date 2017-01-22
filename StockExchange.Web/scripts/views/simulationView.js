/**
 * View for the run simulation page
 */
(function ($) {
    'use strict';

    var $companySelect = $('.company-select');
    $companySelect.select2({
        placeholder: 'Choose companies'
    });

    $('.company-group-select').on('change', function () {
        var $selected = $(this).find(':selected');
        var companies = $selected.data('companies');

        $('option', $companySelect).each(function () {
            var value = parseInt($(this).val());
            if (companies && $.inArray(value, companies) < 0) {
                $(this).prop('disabled', true).prop('selected', false);
            } else {
                $(this).prop('disabled', false);
            }
        });
        $companySelect.select2({
            placeholder: 'Choose companies'
        });
    });

    $("input[type='checkbox']").iCheck({
        checkboxClass: 'icheckbox_flat'
    });

    $('#StartDate').datepicker();
    $('#EndDate').datepicker();

    // Disable companies select when AllCompanies checkbox is checked
    $('#HasTransactionLimit')
        .on('ifChecked ifUnchecked', function() {
            $('.transactionLimit').toggleClass('hidden');
        });

    $('#AndIndictaors')
    .on('ifChecked ifUnchecked', function () {
        $('.indicatorsLimit').toggleClass('hidden');
    });

    // Disables button on submit to prevent multiple clicks
    $('#run-simulation-form').on('submit', function () {
        var $button = $(this).find('[type="submit"]');
        if ($(this).valid()) {
            $button.prop('disabled', true);
        } else {
            $button.prop('disabled', false);
        }
    });

})(jQuery);