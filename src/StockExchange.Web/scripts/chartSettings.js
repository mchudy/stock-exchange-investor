(function () {
    'use strict';

    if (typeof Highcharts !== 'undefined') {
        Highcharts.setOptions({
            global: {
                useUTC: true
            },
            exporting: {
                libURL: 'https://code.highcharts.com/5.0.7/lib/'
            }
        });
    }
})();