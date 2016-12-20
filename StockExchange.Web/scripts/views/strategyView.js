(function ($) {
    $(".indicator-select").change(function () {
        var str = "";
        $("select option:selected").each(function () {
            str += $(this).val();
        });
        if ($("." + str).hasClass("hidden")) {
            $("." + str).removeClass("hidden");
        } else {
            $("." + str).addClass("hidden");
        }
 
    });
})(jQuery);