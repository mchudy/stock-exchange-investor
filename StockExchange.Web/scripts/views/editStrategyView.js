(function ($) {

    $('.indicator-select').change(function () {
        var str = '';
        $('select option:selected').each(function () {
            str += $(this).val();
        });
        $('.' + str).toggleClass('hidden');
    });

    $('.remove-indicator').on('click', function() {
        $(this).parents('.indicator').addClass('hidden');
    });

    $('.edit-strategy').click(function (e) {
        e.preventDefault();
        if (!$('#edit-strategy-form').valid()) {
            return;
        }
        var $this = $(this);
        $this.prop('disabled', true);

        if (!$('#edit-strategy-form').validate()) {
            return;
        }

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
                id: config.strategyId,
                name: $('.strategy-name').val(),
                indicators: indicators
            })
        })
        .done(function(response) {
            window.location = response.redirectUrl;
        })
        .always(function() {
            $this.prop('disabled', false);
        });
    });

})(jQuery);