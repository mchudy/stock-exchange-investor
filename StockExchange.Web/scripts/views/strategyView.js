(function($) {
    'use strict';

    var chosenCompanies = [21];


    $('.company-select').select2();
    $('.company-select').val(chosenCompanies).trigger('change');



    $('.company-select').on('change', function () {
        chosenCompanies = $(this).val();
    });



    function addCompaniesToUrl(baseUrl, companyIds) {
        var newUrl = baseUrl + '?';
        for (var i = 0; i < companyIds.length; i++) {
            newUrl += 'companyIds=' + companyIds[i] + '&';
        }
        return newUrl.slice(0, -1);
    }


})(jQuery);