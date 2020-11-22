var Portfolio = function () {


    return {

        init: function () {
            $('.sorting-grid').mixitup({
                onMixEnd: function () {
                    try { $("img.lazy").show().lazyload().update(); } catch(ex) {}
                }
            });

            $("img.lazy").show().lazyload({
                threshold: 50,
                effect: "fadeIn"
            });
        }

    };

}();