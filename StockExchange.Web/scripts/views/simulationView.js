(function($) {
    'use strict';

    $('.company-select').select2({
        placeholder: 'Choose companies'
    });

    $('#StartDate').datepicker();
    $('#EndDate').datepicker();

})(jQuery);