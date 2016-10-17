(function ($) {
    "use strict";

    var $devicewidth = (window.innerWidth > 0) ? window.innerWidth : screen.width;
    var $deviceheight = (window.innerHeight > 0) ? window.innerHeight : screen.height;
    var $bodyel = jQuery("body");
    var $navbarel = jQuery(".navbar");

    var $lgWidth = 1199;
    var $mdWidth = 991;
    var $smWidth = 767;
    var $xsWidth = 479;

    /* ========================== */
    /* ==== HELPER FUNCTIONS ==== */

    function validatedata($attr, $defaultValue) {
        "use strict";
        if ($attr !== undefined) {
            return $attr
        }
        return $defaultValue;
    }

    function parseBoolean(str, $defaultValue) {
        "use strict";
        if (str == 'true') {
            return true;
        } else if (str == "false") {
            return false;
        }
        return $defaultValue;
    }

    if(document.getElementById('ct-js-wrapper')){
        var snapper = new Snap({
            element: document.getElementById('ct-js-wrapper')
        });

        snapper.settings({
            easing: 'ease',
            addBodyClasses: true,
            slideIntent: 20
        });
    }
    $(document).ready(function () {

        $(".navbar .navbar-nav > li").on("mouseenter", function(){
            $(this).addClass("ct-active").siblings().removeClass('ct-active');
        });

        $(".navbar .navbar-nav > li >a").on("mouseenter", function(){
            $(".navbar .navbar-nav").addClass('ct-navbar--fadeInUp');
        });
        $(".navbar .navbar-nav > li >a").on("mouseleave", function(){
            $(".navbar .navbar-nav").removeClass('ct-navbar--fadeInUp');
        });

        $(".navbar .navbar-nav > li .dropdown-menu").on("mouseleave", function(){
            $(".navbar .navbar-nav > li").removeClass('ct-active');
        });

        $(".navbar, .ct-topBar").wrapAll("<div class='navbar-wrapper'></div>");
        if($navbarel.hasClass("navbar-parts")){
            $navbarel.parent().addClass("navbar-wrapperBig");
        }

        $(".ct-mediaSection").mediaSection();

        //Position IMG

        if ($devicewidth >= 1200 && document.getElementById('ct-js-wrapper')) {
            $(".ct-js-imageOffset").each(function(){
                $(this).css("position", "absolute");
                $(this).css("top", $(this).attr("data-top")+'px');
                $(this).css("bottom", $(this).attr("data-bottom")+'px');
                $(this).css("left", $(this).attr("data-left")+'px');
                $(this).css("right", $(this).attr("data-right")+'px');
            });
        }

        // Add Color // -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        $(".ct-js-color").each(function(){
            $(this).css("color", '#' + $(this).attr("data-color"))
        });

        // Navbar Search // -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        var $searchform = $(".ct-navbar-search");
        $('#ct-js-navSearch').on("click", function(e){
            e.preventDefault();
            $navbarel.addClass('is-inactive');


            $searchform.fadeIn();

            if (($searchform).is(":visible")) {
                $searchform.find("[type=text]").focus();
            }

            return false;
        })

        // Snap Navigation in Mobile // -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        if ($devicewidth > 767 && document.getElementById('ct-js-wrapper')) {
            snapper.disable();
        }

        $(".navbar-toggle").click(function () {
            if($bodyel.hasClass('snapjs-left')){
                snapper.close();
            } else{
                snapper.open('left');
            }
        });

        $(".ct-navbarCart--mobileIcon").on("click", function () {
            if($bodyel.hasClass('snapjs-right')){
                snapper.close();
            } else{
                snapper.open('right');
            }
        });

        $('.ct-js-slick').attr('data-snap-ignore', 'true'); // Ignore Slick

        $('.ct-menuMobile .ct-menuMobile-navbar .dropdown > a').click(function(e) {
            return false; // iOS SUCKS
        });
        $('.ct-menuMobile .ct-menuMobile-navbar .dropdown > a').click(function(e){
            var $this = $(this);
            if($this.parent().hasClass('open')){
                $(this).parent().removeClass('open');
            } else{
                $('.ct-menuMobile .ct-menuMobile-navbar .dropdown.open').toggleClass('open');
                $(this).parent().addClass('open');
            }
        });

        $('.ct-progressPath .dropdown > a').click(function(e) {
            return false; // iOS SUCKS
        });
        $('.ct-progressPath .dropdown > a').click(function(e){
            var $this = $(this);
            if($this.parent().hasClass('open')){
                $(this).parent().removeClass('open');
            } else{
                $('.ct-progressPath .dropdown.open').toggleClass('open');
                $(this).parent().addClass('open');
            }
        });

        $('.ct-menuMobile .ct-menuMobile-navbar .onepage > a').click(function(e) {
            snapper.close();
        });
        // Tooltips and Popovers // -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        $("[data-toggle='tooltip']").tooltip();

        $("[data-toggle='popover']").popover({trigger: "hover", html: true});


        // Link Scroll to Section // -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        function goToByScroll(id) {
            $('html,body').animate({scrollTop: $(id).offset().top - 70}, 'slow');
        }

        $('body .ct-js-btnScroll').click(function () {
            goToByScroll($(this).attr('href'));
            return false;
        });

        $('.ct-js-btnScrollUp').click(function (e) {
            e.preventDefault();
            $("body,html").animate({scrollTop: 0}, 1200);
            console.log($navbarel);
            $navbarel.find('.onepage').removeClass('active');
            $navbarel.find('.onepage:first-child').addClass('active');
            return false;
        });

        // Placeholder Fallback // -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        if ($().placeholder) {
            $("input[placeholder],textarea[placeholder]").placeholder();
        }

        // Animations Init // -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        if ($().appear) {
            if (device.mobile() || device.tablet()) {
                $bodyel.removeClass("cssAnimate");
            } else {

                $('.cssAnimate .animated').appear(function () {
                    var $this = $(this);

                    $this.each(function () {
                        if ($this.data('time') != undefined) {
                            setTimeout(function () {
                                $this.addClass('activate');
                                $this.addClass($this.data('fx'));
                            }, $this.data('time'));
                        } else {
                            $this.addClass('activate');
                            $this.addClass($this.data('fx'));
                        }
                    });
                }, {accX: 50, accY: -350});

                // Skrollr // -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                var s = skrollr.init({
                    forceHeight: false
                });
            }
        }
        $(".ct-navbar-search-closeIcon").on("click", function(){
            $navbarel.removeClass('is-inactive');
            $searchform.fadeOut();
        })

        //Search

        $(".ct-searchCourse").each(function(){
            $(this).on("keydown",function search(e) {
                if(e.keyCode == 13) {
                    $('.ct-js-searchModal').each(function(){
                        $(this).modal({
                            show: true
                        })
                    });
                    var searchArray = [];
                    var inputValue = $(this).val();
                    $('.ct-js-searchModal .modal-body *').remove();
                    $.ajax({
                        url: 'assets/json/search.json',
                        success: function(data) {
                            for(var i = 0; data.length > i; i++ ){
                                var arrayCourseName = data[i].courseName;

                                if(arrayCourseName.toLowerCase().indexOf(inputValue.toLowerCase()) != -1){
                                    searchArray.push(data[i]);
                                    $('.ct-js-searchModal .modal-body').append(
                                        "<div class='ct-productBox ct-productBox--inline ct-u-displayTable ct-u-marginBottom30'>"+
                                            "<div class='ct-u-displayTableCell'>"+
                                                "<div class='ct-productImage'>"+
                                                    "<a href='#'>"+
                                                        "<img src='"+data[i].thumbnailImage+"' alt='Product'>"+
                                                    "</a>"+
                                                "</div>"+
                                            "</div>"+
                                            "<div class='ct-u-displayTableCell'>"+
                                                "<div class='ct-productDescription'>"+
                                                    "<a href='#'>"+
                                                        "<h5 class='ct-fw-600 ct-u-marginBottom10'>"+data[i].courseName+"</h5>"+
                                                    "</a>"+
                                                    "<span>"+data[i].courseDescription+"</span>"+
                                                "</div>"+
                                                "<div class='ct-productMeta'>"+
                                                    "<div class='ct-u-displayTableVertical'>"+
                                                        "<div class='ct-u-displayTableCell'>"+
                                                            "<div class='starrr' data-rating='"+data[i].stars+"'></div>"+
                                                        "</div>"+
                                                        "<div class='ct-u-displayTableCell'>"+
                                                            "<span class='ct-u-colorMotive'>"+data[i].price+"</span>"+
                                                            "<span class='ct-u-textLineThrough'>290$</span>"+
                                                        "</div>"+
                                                        "<div class='ct-u-displayTableCell'>"+
                                                            "<a href='#'>"+
                                                            +data[i].commentsCourse+" <i class='fa fa-user'></i>"+
                                                            "</a>"+
                                                        "</div>"+
                                                "</div>"+
                                            "</div>"+
                                        "</div>"
                                    )
                                }
                            }
                            if(searchArray.length == 0){
                                $(".ct-js-searchModal .modal-header").hide();
                                $('.ct-js-searchModal .modal-body *').remove();
                                $('.ct-js-searchModal .modal-body').append("<h2 class='ct-u-paddingBoth100 text-center'>No results found</h2>")
                            }else{
                                $(".ct-js-searchModal .modal-header").show();
                                $('.ct-js-searchModal .modal-title').text("We found "+searchArray.length+" courses.")
                            }
                            $(".ct-js-searchModal .modal-body .starrr").starrr({
                                noSet: true
                            });
                        }
                    });
                }
            });
        });

        //Triger keydown event on search button
        var e = jQuery.Event("keydown");
        e.which = 13;
        e.keyCode = 13;
        $(".ct-search-button, .ct-navbar-search-button").on("click", function(){
            $(".ct-searchCourse").trigger(e)
        })

        // Log in & Sign up modal

        $('.ct-js-login').on("click", function () {
            $(".ct-js-modal-login").modal({
                show: true
            })
        });

        $('.ct-js-signup').on("click", function () {
            $(".ct-js-modal-signup").modal({
                show: true
            })
        });

    });

    //Extended search input in navbar // ----------------------------------------

    $(document).mouseup(function (e) {
        var $searchform = $(".ct-navbar-search");

        if(!$('#ct-js-navSearch').is(e.target)){
            if (!$searchform.is(e.target) // if the target of the click isn't the container...
                && $searchform.has(e.target).length === 0) // ... nor a descendant of the container
            {
                $navbarel.removeClass('is-inactive');
                //$(".ct-navbar-search form").append("<i class='fa fa-times ct-navbar-search-closeIcon'></i>");
                $searchform.fadeOut();
            }
        }
    });
    $(window).on('resize', function() {
        if ($(window).width() < 768) {
            snapper.enable();
        } else{
            snapper.disable();
        }
    });


    $(window).scroll(function() {
        if ($(window).scrollTop() == 0) {
            $(".ct-topBar--transparent").css("position", "fixed").css("top", "0");
            $(".navbar-transparent .navbar-header img").attr("src", "assets/images/content/logo-light.png")
        } else{
            $(".ct-topBar--transparent").css("position", "absolute").css("top", "-40px");
            $(".navbar-transparent .navbar-header img").attr("src", $navbarel.attr("data-changeLogo"))
        }
    });
    $(window).on("load", function(){
        var $preloader = $('.ct-preloader');
        var $content = $('.ct-preloader-content');

        var $timeout = setTimeout(function(){
            $($preloader).addClass('animated').addClass('fadeOut');
            $($content).addClass('animated').addClass('fadeOut');
        }, 0);
        var $timeout2 = setTimeout(function(){
            $($preloader).css('display', 'none').css('z-index', '-9999');
        }, 500);
        if (!device.mobile() && !device.tablet()) {
            $(window).stellar({
                horizontalScrolling   : false,
                verticalScrolling     : true,
                responsive            : true,
                horizontalOffset      : 0,
                verticalOffset        : 0,
                parallaxBackgrounds   : true,
                parallaxElements      : true,
                scrollProperty        : 'scroll',
                positionProperty      : 'position',
                hideDistantElements   : true
            });
        }

        if ($().appear) {
            if (device.mobile() || device.tablet()) {
                $bodyel.removeClass("cssAnimate");
            } else {
                // Skrollr Refresh// -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                setTimeout(function () {
                    $(window).trigger('resize');
                    skrollr.get().refresh();
                }, 100);
            }
        }

    });
    $(window).trigger('resize');
})(jQuery);