(function ($) {
    $.overlay = {};


    $.fn.extend({
        overlay: function () {
            var _self;
            var overlayCache;
            var template =
                    {
                        div: $('<div class="msp-overlay"><span class="loader-img"></span></div>')
                    };

            init = function () {

                overlayCache = template.div.clone();
                overlayCache.height(_self.height());
                overlayCache.width(_self.width());

                _self.append(overlayCache);

                return this;

            };

            this.hide = function () {

                overlayCache.hide();

            },
            this.show = function () {

                overlayCache.height($(document).height());
                overlayCache.width($(document).width());

                overlayCache.show();

            };

            return this.each(function () {

                _self = $(this);
                init();

            });

        }
    });

})(jQuery);