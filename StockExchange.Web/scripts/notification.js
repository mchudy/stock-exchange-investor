(function ($) {

    $.Notifier = {};

    $.fn.extend({

        Notifier: function (options) {

            var _self,
            defaults = {
                delayTime: 3
            },
                nContainer,
                classes = {
                    container: 'notify-container',
                    item: 'notifier'
                },
                template = {
                    div: $('<div></div>'),
                    button: $('<button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>')
                },
                settings = $.extend({}, defaults, options),
                elemItems = [];

            init = function () {

                nContainer = template.div.clone().addClass(classes.container);
                _self.append(nContainer);
                timer();

                return this;

            },
            timer = function () {

                var a = setInterval(function () {

                    if (elemItems.length > 0) {

                        var eItem = elemItems[0];
                        elemItems.shift();

                        eItem.fadeOut((settings.delayTime * 1000), function () {
                            eItem.remove();
                        });
                    }

                }, (settings.delayTime * 1000));

            };

            this.add = function ($elem) {
                var elem = $($elem);

                nContainer.append(elem.addClass(classes.item));

                //elem.fadeOut((settings.delayTime * 1000), function () {
                //    elem.remove();
                //});

                elemItems.push(elem);
            };

            this.alert = function (message, type) {
                var elem = template.div.clone();

                switch (type) {
                    case 'warning':
                        elem.addClass('alert alert-warning');
                        break;
                    case 'success':
                        elem.addClass('alert alert-success');
                        break;
                    case 'error':
                        elem.addClass('alert alert-danger');
                        break;
                    case 'info':
                        elem.addClass('alert alert-info');
                        break;
                    default:
                        break;
                }

                elem.text(message);

                this.add(elem);
            };

            return this.each(function () {

                _self = $(this);
                init();

            });
        }

    });

})(jQuery);