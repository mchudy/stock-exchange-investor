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
    }).trigger('change');

    $('.select-all-group').on('click', function(e) {
        e.preventDefault();
        var groupCompanies = $('.company-group-select').find(':selected').data('companies');
        if (groupCompanies && groupCompanies.length > 0) {
            $('.company-select').val(groupCompanies).trigger('change');
        }
    });

    $("input[type='checkbox']").iCheck({
        checkboxClass: 'icheckbox_flat'
    });

    $('#StartDate').datepicker();
    $('#EndDate').datepicker();

    if ($('#HasMaximalTransactonLimit').prop('checked')) {
        $('.transactionLimit').removeClass('hidden');
    }

    if ($('#HasMinimalTransactionLimit').prop('checked')) {
        $('.minimalTransactionLimit').removeClass('hidden');
    }

    $('#HasMinimalTransactionLimit')
        .on('ifChecked', function () {
            $('.minimalTransactionLimit').removeClass('hidden');
        })
        .on('ifUnchecked', function () {
            $('.minimalTransactionLimit').addClass('hidden');
        });


    $('#HasMaximalTransactonLimit')
        .on('ifChecked', function() {
            $('.transactionLimit').removeClass('hidden');
        })
        .on('ifUnchecked', function () {
            $('.transactionLimit').addClass('hidden');
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