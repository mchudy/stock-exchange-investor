(function() {
    'use strict';

    $('.sidebar-toggle').click(function() {
        $('.sidebar').toggleClass('hide-sidebar');
        $('.main-content').toggleClass('hide-sidebar');
    });

})();