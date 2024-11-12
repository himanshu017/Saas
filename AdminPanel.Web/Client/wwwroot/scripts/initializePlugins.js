
function homeSlider() {

    //========================================
    //       HOME CLASSIC BANNER SLIDER
    //========================================
    if ($('.home-classic-slider').children('.banner-part').length > 0) {
        $('.home-classic-slider').slick({
            dots: false,
            fade: false,
            infinite: true,
            autoplay: true,
            arrows: true,
            speed: 800,
            prevArrow: '<i class="icofont-arrow-right dandik"></i>',
            nextArrow: '<i class="icofont-arrow-left bamdik"></i>',
            slidesToShow: 1,
            slidesToScroll: 1,
            responsive: [
                {
                    breakpoint: 1200,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1,
                    }
                },
                {
                    breakpoint: 992,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1,
                    }
                },
                {
                    breakpoint: 768,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1,
                    }
                },
                {
                    breakpoint: 576,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1,
                        arrows: false,
                    }
                }
            ]
        });
    }
}

function categorySlider() {
    //========================================
    //      FOR SUGGEST CATEGORY SLIDER
    //========================================

    $('.suggest-slider').slick({
        dots: false,
        infinite: true,
        autoplay: true,
        arrows: true,
        speed: 1000,
        prevArrow: '<i class="icofont-arrow-right dandik"></i>',
        nextArrow: '<i class="icofont-arrow-left bamdik"></i>',
        slidesToShow: 5,
        slidesToScroll: 2,
        responsive: [
            {
                breakpoint: 1200,
                settings: {
                    slidesToShow: 4,
                    slidesToScroll: 4,
                }
            },
            {
                breakpoint: 992,
                settings: {
                    slidesToShow: 3,
                    slidesToScroll: 3,
                }
            },
            {
                breakpoint: 768,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 2,
                }
            },
            {
                breakpoint: 576,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 2,
                    arrows: false,
                }
            }
        ]
    });
}

function menuToggles() {
    //========================================
    //        DROPDOWN MENU FUNCTION
    //========================================
    $(function () {
        $(".dropdown-link").click(function () {
            $(this).next().toggle();
            $(this).toggleClass('active');
            if ($('.dropdown-list:visible').length > 1) {
                $('.dropdown-list:visible').hide();
                $(this).next().show();
                $('.dropdown-link').removeClass('active');
                $(this).addClass('active');
            }
        });
    });

    //========================================
    //       NAV SIDEBAR MENU ACTIVE
    //========================================
    $('.nav-link').on('click', function () {
        $('.nav-list li a').removeClass('active');
        $(this).addClass('active');
    });


    //========================================
    //        CATEGORY SIDEBAR FUNCTION
    //========================================
    $('.header-cate, .cate-btn').on('click', function () {
        $('body').css('overflow', 'hidden');
        $('.category-sidebar').addClass('active');
        $('.category-close').on('click', function () {
            $('body').css('overflow', 'inherit');
            $('.category-sidebar').removeClass('active');
            $('.backdrop').fadeOut();
        });
    });


    //========================================
    //         NAV SIDEBAR FUNCTION
    //========================================
    $('.header-user').on('click', function () {
        $('body').css('overflow', 'hidden');
        $('.nav-sidebar').addClass('active');
        $('.nav-close').on('click', function () {
            $('body').css('overflow', 'inherit');
            $('.nav-sidebar').removeClass('active');
            $('.backdrop').fadeOut();
        });
    });


    //========================================
    //         CART SIDEBAR FUNCTION
    //========================================
    //$('.header-cart, .cart-btn').on('click', function () {
    //    $('body').css('overflow', 'hidden');
    //    $('.cart-sidebar').addClass('active');
    //    $('.cart-close').on('click', function () {
    //        $('body').css('overflow', 'inherit');
    //        $('.cart-sidebar').removeClass('active');
    //        $('.backdrop').fadeOut();
    //    });
    //});



    //========================================
    //        PRODUCT VIEW IMAGE SHOW
    //========================================
    $('.modal').on('shown.bs.modal', function (e) {
        $('.preview-slider, .thumb-slider').slick('setPosition', 0);
    });

}

function ResponsiveSearchBar() {

    //========================================
    //       RESPONSIVE SEARCH BAR
    //========================================
    $('.header-src').on('click', function () {
        $('.header-form').toggleClass('active');
        $(this).children('.fa-search').toggleClass('fa-times');
    });
}

function setBodyModalClass() {
    $('body').toggleClass('modal-open');
}