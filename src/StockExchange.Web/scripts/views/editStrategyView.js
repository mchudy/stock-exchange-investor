/**
 * View for the edit strategy page
 */
(function ($) {
    'use strict';

    var $indicatorSelect = $('.indicator-select');

    /*
     * Shows an indicator when it is chosen from the select
     */
    $indicatorSelect.on('change', function () {
        var str = '';
        $('select option:selected').each(function () {
            $(this).addClass('hidden');
            str += $(this).val();
        });
        $('.indicator[data-id=' + str + ']').removeClass('hidden');
        $indicatorSelect.val('');
    });

    $('.indicator:not(.hidden)').each(function () {
        var indicatorType = $(this).data('id');
        $indicatorSelect.find('option[value=' + indicatorType + ']')
            .addClass('hidden');
    });

    /*
     * Hides indicator where the remove icon is clicked
     */
    $('.remove-indicator').on('click', function() {
        var $indicator = $(this).parents('.indicator');
        var indicatorType = $indicator.data('id');
        $indicator.addClass('hidden');
        $indicatorSelect.find('option[value=' + indicatorType + ']')
            .removeClass('hidden');
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
        var data = {
            id: config.strategyId,
            name: $('.strategy-name').val(),
            indicators: indicators,
            isConjunctiveStrategy: $('#IsConjunctiveStrategy').prop('checked'),
            signalDaysPeriod: $('#SignalDaysPeriod').val()
        };
        $.ajax(config.createStrategyUrl, {
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data)
        })
        .done(function(response) {
            window.location = response.redirectUrl;
        })
        .always(function() {
            $this.prop('disabled', false);
        });
    });

    $('.show-indicator-description').on('click', function () {
        var $description = $('.indicator-description', $(this).parent().parent());
        $description.toggleClass('hidden');
        var isHidden = $description.hasClass('hidden');
        $(this).text(isHidden ? 'Show description' : 'Hide description');
    });

    $("input[type='checkbox']").iCheck({
        checkboxClass: 'icheckbox_flat'
    });

    if ($('#IsConjunctiveStrategy').prop('checked')) {
        $('.indicatorsLimit').removeClass('hidden');
    }

    $('#IsConjunctiveStrategy')
        .on('ifChecked', function () {
            $('.indicatorsLimit').removeClass('hidden');
        })
        .on('ifUnchecked', function () {
            $('.indicatorsLimit').addClass('hidden');
        });

})(jQuery);