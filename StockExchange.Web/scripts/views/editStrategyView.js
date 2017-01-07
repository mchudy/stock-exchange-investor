/**
 * View for the edit strategy page
 */
(function ($) {
    'use strict';

    /*
     * Shows an indicator when it is chosen from the select
     */
    $('.indicator-select').change(function () {
        var str = '';
        $('select option:selected').each(function () {
            str += $(this).val();
        });
        $('.indicator[data-id=' + str +']').removeClass('hidden');
    });

    /*
     * Hides indicator where the remove icon is clicked
     */
    $('.remove-indicator').on('click', function() {
        $(this).parents('.indicator').addClass('hidden');
    });

    /*
     * Send an AJAX request that updates or creates the strategy
     */ 
    $('.edit-strategy').click(function (e) {
        e.preventDefault();
        if (!$('#edit-strategy-form').valid()) {
            return;
        }
        var $this = $(this);
        $this.prop('disabled', true);

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