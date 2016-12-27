(function ($) {

    $('.indicator-select').change(function () {
        var str = '';
        $('select option:selected').each(function () {
            str += $(this).val();
        });
        $('.' + str).toggleClass('hidden');
    });

    $('.create-strategy').click(function (e) {
        e.preventDefault();

        var indicators = [];
        $('.indicator:not(.hidden)').each(function () {
            var indicator = {
                type: $(this).data('id'),
                properties: []
            };
            $('.property', $(this)).each(function () {
                indicator.properties.push({
                    name: $(this).data('name'),
                    value: $(this).find('.property-value').val()
                });
            });
            indicators.push(indicator);
        });
        $.ajax(config.createStrategyUrl, {
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                name: $('.strategy-name').val(),
                indicators: indicators
            })
        })
        .done(function() {
            toastr.success('Strategy has been added');
        });
    });

})(jQuery);