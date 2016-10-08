(function ($) {

    var Methods = function (element) {

        this.self = $(element);
        this.obj = this;
        this.template = {
            button: $('<span class="accordion-button" >-</span>')
        }

        var buttoncache;

        var privFunc = $.extend(this, {
            me: $(element),
            hidenAccord: function () {
                privFunc.me.find('.panel-content').toggle();
                buttoncache.text((buttoncache.text() == '+' ? '-' : '+'));
            }
        });

        this.init = function () {

            buttoncache = this.template.button.clone();
            this.self.find('.panel-heading').append(buttoncache);

            buttoncache.click(function () {
                privFunc.hidenAccord();
            });

            return this;
        }

        this.accord = function () {
            privFunc.hidenAccord();
        }
    }

    $.fn.accordionPanel = function () {
        return this.each(function () {
            var self = $(this);

            if (self.data('data')) return;

            var data = new Methods(this).init();

            self.data('data', data);

        });
    };

})(jQuery);