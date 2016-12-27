(function ($) {
    'use strict';

    $('#confirm-delete-modal').on('show.bs.modal', function (e) {
        var target = $(e.relatedTarget);
        var url = target.data('url');
        $('.btn-confirm-delete', this).data('url', url);
    });

    $('.btn-confirm-delete').on('click', function() {
        var url = $(this).data('url');

        $.ajax(url, {
            type: 'POST'
        }).done(function() {
            window.location.reload(false);
        });
    });

})(jQuery);