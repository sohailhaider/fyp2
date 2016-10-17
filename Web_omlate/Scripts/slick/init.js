(function ($) {
  "use strict";
  var $slick = $(".ct-js-slick");

  var $devicewidth = (window.innerWidth > 0) ? window.innerWidth : screen.width;

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

  $(document).ready(function () {

    if ($().slick) {
      
    // Responsive Breakpoins
      var $widthLG = 1200,
        $widthMD = 900,
        $widthSM = 600,
        $widthXS = 0;

      if ($slick.length > 0) {
        $slick.each(function () {
          var $this = $(this),
            ctanimations = validatedata($this.attr("data-animations"), true), // variable from main.js
            $slickheight = $this.attr('data-height');

          if (ctanimations === false || $devicewidth < 768 || device.mobile() || device.ipad() || device.androidTablet()) { // Disable scroll animation on mobile
            $slick.find('.animated').each(function () {
              $(this).removeClass('animated');
            });
          } else {
            // Slider init animations
            $('.cssAnimate .animated').appear(function () {
              var $this = $(this);

              $this.each(function () {
                if ($this.data('time') !== undefined) {
                  setTimeout(function () {
                    $this.addClass('activate');
                    $this.addClass($this.data('fx'));
                  }, $this.data('time'));
                } else {
                  $this.addClass('activate');
                  $this.addClass($this.data('fx'));
                }
              });
            }, {accX: 50, accY: -200});
          }

          // slider height
          if ($this.attr('data-height')) {
            $this.css({height: $slickheight});
            $this.find('.slick-list').css({height: $slickheight});
            $this.find('.slick-track').css({height: $slickheight});
            $this.find('.item').each(function () {
              $(this).css({height: $slickheight})
            });
          }


          // item height
          $this.find('.item').each(function () {
            var $item = $(this),
                $height = $item.attr('data-height');

            $item.css({
              height: $height
            })

          });

          // Background Image // -------------------------------
          $this.find(".item").each(function () {
            var $slide_item = $(this);
            var bg = validatedata($slide_item.attr('data-bg'), false);
            if (bg) {
              if($("html").hasClass("ie8")){
                $slide_item.css('background', 'url("' + bg + '")');
              }else{
                $slide_item.css('background-image', 'url("' + bg + '")');
              }
            }
          });

            // Items Settings
          var ctslidesToShow =  parseInt(validatedata($this.attr("data-items"), 1), 10), // Non Responsive
            slidesXS = parseInt(validatedata($this.attr("data-XSitems"), ctslidesToShow), 10),
            slidesSM = parseInt(validatedata($this.attr("data-SMitems"), slidesXS), 10), // Default Item from data-items;
            slidesMD = parseInt(validatedata($this.attr("data-MDitems"), slidesSM), 10), // Default Item from smaller Device;
            slidesLG = parseInt(validatedata($this.attr("data-LGitems"), slidesMD), 10), // Default Item from smaller Device;

            ctaccessibility = parseBoolean($this.attr("data-accessibility"), true),
            ctadaptiveHeight = parseBoolean($this.attr("data-adaptiveHeight"), false),
            ctautoplay = parseBoolean($this.attr("data-autoplay"), false),
            ctautoplaySpeed = parseInt(validatedata($this.attr("data-autoplaySpeed"), 5000), 10),
            ctarrows = parseBoolean($this.attr("data-arrows"), true),
            ctasNavFor = validatedata($this.attr("data-asNavFor")),
            ctappendArrows = validatedata($this.attr("data-appendArrows")),
            ctprevArrow = validatedata($this.attr("data-prevArrow"), '<button type="button" class="slick-prev">Previous</button>'),
            ctnextArrow = validatedata($this.attr("data-nextArrow"), '<button type="button" class="slick-next">Next</button>'),
            ctcenterMode = parseBoolean($this.attr("data-centerMode"), false),
            ctcenterPadding = validatedata($this.attr("data-centerPadding"), '50px'),
            ctcssEase = validatedata($this.attr("data-cssEase"), 'ease'),
            ctdots = parseBoolean($this.attr("data-dots"), false),
            ctdraggable = parseBoolean($this.attr("data-draggable"), true),
            ctfade = parseBoolean($this.attr("data-fade"), false),
            ctfocusOnSelect = parseBoolean($this.attr("data-focusOnSelect"), false),
            cteasing = validatedata($this.attr("data-easing"), 'linear'),
            ctedgeFriction = parseInt(validatedata($this.attr("data-edgeFriction"), 0.15), 10),
            ctinfinite = parseBoolean($this.attr("data-infinite"), true),
            ctinitialSlide = parseInt(validatedata($this.attr("data-initialSlide"), 0), 10),
            ctlazyLoad = validatedata($this.attr("data-lazyLoad"), 'ondemand'),
            ctmobileFirst = parseBoolean($this.attr("data-mobileFirst"), true),
            ctpauseOnHover = parseBoolean($this.attr("data-pauseOnHover"), true),
            ctpauseOnDotsHover = parseBoolean($this.attr("data-pauseOnDotsHover"), false),
            ctrespondTo = validatedata($this.attr("data-respondTo"), 'window'),
            ctslide = validatedata($this.attr("data-slide")),
            ctslidesToScroll = parseInt(validatedata($this.attr("data-slidesToScroll"), 1), 10),
            ctspeed = parseInt(validatedata($this.attr("data-speed"), 300), 10),
            ctswipe = parseBoolean($this.attr("data-swipe"), true),
            ctswipeToSlide =  parseBoolean($this.attr("data-swipeToSlide"), false),
            cttouchMove = parseBoolean($this.attr("data-touchMove"), true),
            cttouchThreshold = parseInt(validatedata($this.attr("data-touchThreshold"), 5), 10),
            ctuseCSS = parseBoolean($this.attr("data-useCSS"), true),
            ctvariableWidth = parseBoolean($this.attr("data-variableWidth"), false),
            ctvertical = parseBoolean($this.attr("data-vertical"), false),
            ctrtl = parseBoolean($this.attr("data-rtl"), false);
          
          // Slick Init
          $this.slick({
            slidesToShow: ctslidesToShow,
            accessibility: ctaccessibility,      // Enables tabbing and arrow key navigation
            adaptiveHeight: ctadaptiveHeight,    // Enables adaptive height for single slide horizontal carousels.
            autoplay: ctautoplay,                // Enables Autoplay
            autoplaySpeed: ctautoplaySpeed,      // Autoplay Speed in milliseconds
            arrows: ctarrows,                    // Prev/Next Arrows
            asNavFor: ctasNavFor,                // Set the slider to be the navigation of other slider (Class or ID Name)
            appendArrows: ctappendArrows,        // Change where the navigation arrows are attached (Selector, htmlString, Array, Element, jQuery object)
            prevArrow: ctprevArrow,              // Allows you to select a node or customize the HTML for the "Previous" arrow.
            nextArrow: ctnextArrow,              // Allows you to select a node or customize the HTML for the "Next" arrow.
            centerMode: ctcenterMode,            // Enables centered view with partial prev/next slides. Use with odd numbered slidesToShow counts.
            centerPadding: ctcenterPadding,      // Side padding when in center mode (px or %)
            cssEase: ctcssEase,                  // CSS3 Animation Easing
            dots: ctdots,                        // Show dot indicators
            draggable: ctdraggable,              // Enable mouse dragging
            fade: ctfade,                        // Enable fade
            focusOnSelect: ctfocusOnSelect,      // Enable focus on selected element (click)
            easing: cteasing,                    // Add easing for jQuery animate. Use with easing libraries or default easing methods
            edgeFriction: ctedgeFriction,        // Resistance when swiping edges of non-infinite carousels
            infinite: ctinfinite,                // Infinite loop sliding
            initialSlide: ctinitialSlide,        // Slide to start on
            lazyLoad: ctlazyLoad,                // Set lazy loading technique. Accepts 'ondemand' or 'progressive'
            mobileFirst: ctmobileFirst,          // Responsive settings use mobile first calculation
            pauseOnHover: ctpauseOnHover,        // Pause Autoplay On Hover
            pauseOnDotsHover: ctpauseOnDotsHover,// Pause Autoplay when a dot is hovered
            respondTo: ctrespondTo,              // Width that responsive object responds to. Can be 'window', 'slider' or 'min' (the smaller of the two)
            slide: ctslide,                      // Element query to use as slide
            slidesToScroll: ctslidesToScroll,    // Number of slides to scroll
            speed: ctspeed,                      // Slide/Fade animation speed
            swipe: ctswipe,                      // Enable swiping
            swipeToSlide: ctswipeToSlide,        // Allow users to drag or swipe directly to a slide irrespective of slidesToScroll
            touchMove: cttouchMove,              // Enable slide motion with touch
            touchThreshold: cttouchThreshold,    // To advance slides, the user must swipe a length of (1/touchThreshold) * the width of the slide
            useCSS: ctuseCSS,                    // Enable/Disable CSS Transitions
            variableWidth: ctvariableWidth,      // Variable width slides
            vertical: ctvertical,                // Vertical slide mode
            rtl: ctrtl,                           // Change the slider's direction to become right-to-left
            responsive: [ // Responsive Breakpoints
              {
                breakpoint: $widthLG, // Desktop
                settings: {
                  slidesToShow: slidesLG
                }
              },
              {
                breakpoint: $widthMD,  // Laptop
                settings: {
                  slidesToShow: slidesMD
                }
              },
              {
                breakpoint: $widthSM, // Tablet
                settings: {
                  slidesToShow: slidesSM
                }
              },
              {
                breakpoint: $widthXS, // Mobile
                settings: {
                  slidesToShow: slidesXS
                }
              }
            ] // end Responsive Breakpoints

          }); //end $this.slick

          $this.on('beforeChange', function () {
            if (ctanimations) {
              $this.find(".slick-slide [data-fx]").each(function () {
                var $content = $(this);
                $content.removeClass($content.data('fx')).removeClass("activate");
              });
              setTimeout(function () {
                $this.find(".slick-active [data-fx]").each(function () {
                  var $content = $(this);
                  if ($content.data('time') !== undefined) {
                    setTimeout(function () {
                      $content.addClass($content.data('fx')).addClass("activate");
                    }, $content.data('time'));
                  } else {
                    $content.addClass($content.data('fx')).addClass("activate");
                  }
                });
              }, 150);
            }
          });

        }); // end each functions
      } // end length if
    } // end Slick
  }); // end Doc Ready

}(jQuery));