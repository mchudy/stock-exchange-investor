(function ($) {

    $('.indicator-select').change(function () {
        var str = '';
        $('select option:selected').each(function () {
            str += $(this).val();
        });
        if ($('.' + str).hasClass('hidden')) {
            $('.' + str).removeClass('hidden');
        } else {
            $('.' + str).addClass('hidden');
        }
    });

    $('.create').click(function () {
        var list = [];
        list.push({
            indicator: $('.strategyname').val(),
            property: '',
            value: 0
        });
        //TODO: get rid of that
        $('.indicator')
            .each(function () {
                if (!$(this).hasClass('hidden')) {
                    var counter = 0;
                    var currentIndicatorType = $(this).attr('id');
                    $(this)
                        .children()
                        .each(function () {
                            if ($(this).hasClass('property')) {
                                $(this)
                                    .children()
                                    .each(function () {
                                        if ($(this).hasClass('propertyname')) {
                                            counter = 1;
                                            var propertyName = $(this).text();
                                            var propertyValue = $(this).next().val();
                                            list.push({
                                                indicator: currentIndicatorType,
                                                property: propertyName,
                                                value: propertyValue
                                            });
                                        }
                                    });
                            }
                        });
                    if (counter === 0) {
                        list.push({
                            indicator: currentIndicatorType,
                            property: '',
                            value: 0
                        });
                    }
                }
            });
        $.post('/Strategies/CreateStrategy', { indicators: list });
    });

})(jQuery);