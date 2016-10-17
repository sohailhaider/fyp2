;(function ( jQuery, window, document, undefined ) {

    "use strict";

    // For plugin you need: jQuery, device.js, browser.js, stellar.js, smoothScroll.js  !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    // smoothScroll.js and stellar.js need only for parallax


    var pluginName = "mediaSection",
        defaults = {

            parallax        : {
                backgroundRatio       : 0.3,
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
            },

            kenburns        : {
                speed : 7000
            },

            video           : {
                startAt   : 1,
                volume    : 0

            },

            height          : 1,
            background      : "",
            backgroundMobile: "",
            disableMobile   : true

        };

    // The actual plugin constructor
    function Plugin (element, options) {
        this.element = jQuery(element);
        this.settings = jQuery.extend(true, {}, defaults, options);
        this.selfdefaults = defaults;
        this.selfname = pluginName;

        if (!Function.prototype.bind) {
            Function.prototype.bind = function(oThis) {
                if (typeof this !== 'function') {
                    // closest thing possible to the ECMAScript 5
                    // internal IsCallable function
                    throw new TypeError('Function.prototype.bind - what is trying to be bound is not callable');
                }

                var aArgs   = Array.prototype.slice.call(arguments, 1),
                    fToBind = this,
                    fNOP    = function() {},
                    fBound  = function() {
                        return fToBind.apply(this instanceof fNOP && oThis
                                ? this
                                : oThis,
                            aArgs.concat(Array.prototype.slice.call(arguments)));
                    };

                fNOP.prototype = this.prototype;
                fBound.prototype = new fNOP();

                return fBound;
            };
        }

        this.init();
    }

    jQuery.extend(Plugin.prototype, {
        init: function() {
            var self           = this,
                el         = self.element,
                mediaType   = el.attr("data-type") || "",
                $htmlel     = jQuery("html");

            if (jQuery.browser.mozilla){$htmlel.addClass('browser-mozilla') }
            if (jQuery.browser.webkit){$htmlel.addClass('browser-webkit') }
            if (jQuery.browser.msie){$htmlel.addClass('browser-msie') }
            if (jQuery.browser.safari){$htmlel.addClass('browser-safari') }

            self.setBackground();
            self.setHeight();

            if(!(device.mobile() && ((el.attr('data-disable-mobile') == 'true') || (self.settings.disableMobile)))){
                if(mediaType === "parallax"){
                    self.createParallax();
                }else if(mediaType === "kenburns"){
                    self.createKenBurns();
                }else if(mediaType === "video"){
                    self.createVideo();
                }
            }

            if (((el.attr('data-disable-mobile') == 'true') || (self.settings.disableMobile)) && device.mobile()) {
                el.find(".ct-mediaSection-video").css('display', 'none');
                el.find(".ct-mediaSection-kenburns").css('display', 'none');
            }
        },

        setBackground: function(){
            var self                 = this,
                el               = self.element,
                background        = "",
                backgroundMobile  = "",
                $htmlel       = jQuery("html");


            if(el.attr('data-background')){
                background = el.attr('data-background');
            }else if(self.settings.background){
                background = self.settings.background;
            }

            if(el.attr('data-background-mobile')){
                backgroundMobile = el.attr('data-background-mobile');
            }else if(self.settings.backgroundMobile){
                backgroundMobile = self.settings.backgroundMobile;
            }


            if(background.substr(0, 1) === '#'){
                el.css('background-color', background);
            }else if(backgroundMobile && device.mobile()){
                el.css('background-image', 'url(' + backgroundMobile + ')');
            }else{
                if($htmlel.hasClass("ie8")){
                    el.css('background', 'url(' + background + ')');
                }else{
                    el.css('background-image', 'url(' + background + ')');
                }
            }
        },

        setHeight: function(){
            var self             = this,
                el           = self.element,
                height        = "0",
                $deviceheight = (window.innerHeight > 0) ? window.innerHeight : screen.height,
                $htmlel       = jQuery("html");

            if(el.attr("data-height")){
                height = el.attr('data-height');
            }else{
                height = self.settings.height;
            }

            if (height.indexOf("%") > -1) {
                var heightResult = $deviceheight * (parseInt(height, 10) / 100) + "px";
                if($htmlel.hasClass("browser-safari"))
                    el.css('height', heightResult);
                else{
                    el.css('min-height', heightResult);
                }
            } else {
                var heightResult = parseInt(height, 10) + "px";
                if($htmlel.hasClass("browser-safari")){
                    el.css('height', heightResult);
                }else{
                    el.css('min-height', heightResult);
                }
            }
        },

        makekenburns: function(el){
            var self = this;

            el.find('img')[0].className = "fx";
            var images = el.find('img'),
                numberOfImages = images.length,
                i = 1,
            // data-attributes //
                interval = parseInt(self.settings.kenburns.speed ,10);

            if (numberOfImages === 1) {
                images[0].className = "singlefx";
            }
            window.setInterval(kenBurns, interval);

            function kenBurns() {
                if (numberOfImages !== 1) {
                    if (i === numberOfImages) {
                        i = 0;
                    }
                    images[i].className = "fx";
                    if (i === 0) {
                        images[numberOfImages - 2].className = "";
                    }
                    if (i === 1) {
                        images[numberOfImages - 1].className = "";
                    }
                    if (i > 1) {
                        images[i - 2].className = "";
                    }
                    i++;
                }
            }
        },

        createKenBurns: function(){
            var self         = this,
                el       = self.element,
                kenburnEl = el.find('.ct-mediaSection-kenburns'),
                images    =  kenburnEl.find('img');

            if (!(device.mobile() || device.ipad() || device.androidTablet())) {
                self.makekenburns(kenburnEl);
            } else {
                images.each(function () {
                    jQuery(this).remove();
                })
            }
        },

        createVideo: function(){
            var self             = this,
                el           = self.element,
                time          = 1,
                $devicewidth  = (window.innerWidth > 0) ? window.innerWidth : screen.width;;

            if (!el.hasClass("html5")) {
                var videoframe = el.find('iframe');
                if (videoframe.attr('data-startat')) {
                    time = videoframe.attr('data-startat');
                }else if(self.settings.video.startAt){
                    time = self.settings.video.startAt;
                }
                if (!($devicewidth < 992) && !device.mobile()) {
                    if (typeof $f != 'undefined') {
                        var video   = '#' + $videoframe.attr('id'),
                            iframe  = jQuery(video)[0],
                            player  = $f(iframe),
                            status = jQuery('.status');
                        player.addEvent('ready', function () {
                            player.api('setVolume', self.settings.video.volume);
                            player.api('seekTo', time);
                        })
                    }
                }
            } else {
                //THIS IS WHERE YOU CALL THE VIDEO ID AND AUTO PLAY IT. CHROME HAS SOME KIND OF ISSUE AUTOPLAYING HTML5 VIDEOS, SO THIS IS NEEDED

                document.getElementById('video1').play();
            }
        },

        createParallax: function(){
            var self         = this,
                el       = self.element;


            if(!(el.attr("data-stellar-background-ratio"))){
                el.attr("data-stellar-background-ratio", self.settings.parallax.backgroundRatio);
            }



            jQuery(window).on("load", function(){
                if(!(jQuery(window).stellar())){
                    jQuery(window).stellar({
                        horizontalScrolling   : self.settings.parallax.horizontalScrolling,
                        verticalScrolling     : self.settings.parallax.verticalScrolling,
                        responsive            : self.settings.parallax.responsive,
                        horizontalOffset      : self.settings.parallax.horizontalOffset,
                        verticalOffset        : self.settings.parallax.verticalOffset,
                        parallaxBackgrounds   : self.settings.parallax.parallaxBackgrounds,
                        parallaxElements      : self.settings.parallax.parallaxElements,
                        scrollProperty        : self.settings.parallax.scrollProperty,
                        positionProperty      : self.settings.parallax.positionProperty,
                        hideDistantElements   : self.settings.parallax.hideDistantElements
                    });
                }
            });
        }

    });

    jQuery.fn[ pluginName ] = function ( options ) {
        return this.each(function() {
            if ( !jQuery.data( this, "pluginself" + pluginName ) ) {
                jQuery.data( this, "pluginself" + pluginName, new Plugin( this, options ) );
            }
        });
    };

})( jQuery, window, document );
