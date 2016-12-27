(function($) {
    'use strict';

    $('.company-select').select2({
        placeholder: 'Choose companies'
    });

    $("input[type='checkbox']").iCheck({
        checkboxClass: 'icheckbox_flat'
    });

    $('#StartDate').datepicker();
    $('#EndDate').datepicker();

})(jQuery);