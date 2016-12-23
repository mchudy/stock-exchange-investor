(function ($) {

    $('.indicator-select').change(function () {
        var str = '';
        $('select option:selected').each(function () {
            str += $(this).val();
        });
        $('.' + str).toggleClass('hidden');
    });

    //TODO: some terrible hacks here
    $('.create-strategy').click(function (e) {
        e.preventDefault();
        var list = [];
        list.push({
            indicator: $('.strategy-name').val(),
            property: '',
            value: 0
        });
        $('.indicator:not(.hidden)').each(function () {
            var counter = 0;
            var currentIndicatorType = $(this).data('id');
            $('.property', $(this)).each(function () {
                    counter = 1;
                    var propertyName = $(this).data('name');
                    var propertyValue = $(this).find('.property-value').val();
                    list.push({
                        indicator: currentIndicatorType,
                        property: propertyName,
                        value: propertyValue
                    });
                });
            if (counter === 0) {
                list.push({
                    indicator: currentIndicatorType,
                    property: '',
                    value: 0
                });
            }
        });
        $.post('/Strategies/CreateStrategy', { indicators: list })
            .done(function() {
                toastr.success('Strategy has been added');
            });
    });

})(jQuery);