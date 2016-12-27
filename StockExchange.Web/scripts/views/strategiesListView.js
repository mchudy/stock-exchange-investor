(function ($) {
    'use strict';

    $('.delete-strategy').on('click', function() {
        var id = $(this).parents('.strategy-item').data('id');
        var url = config.deleteStrategyUrl + '/' + id;

        $.ajax(url, {
            type: 'POST'
        }).done(function() {
            window.location.reload(false);
        });
    });

})(jQuery);