
$(document).ready(function () {
    

        /* affix the navbar after scroll below header */
        $('#nav').affix({
              offset: {
                top: $('header').height()-$('#nav').height()
              }
        });	

        /* highlight the top nav as scrolling occurs */
        $('body').scrollspy({ target: '#nav' })

        /* smooth scrolling for scroll to top */
        $('.scroll-top').click(function(){
          $('body,html').animate({scrollTop:0},1000);
        })

        /* smooth scrolling for nav sections 
        $('#nav .navbar-nav li>a').click(function(){
          var link = $(this).attr('href');
          var posi = $(link).offset().top;
          $('body,html').animate({scrollTop:posi},700);
        });*/




        $('.experiences-home .text-right').mouseover(function () {
            $(this).animate({ 'opacity': 0.5 }, "slow");
        });
        $('.experiences-home .text-right').mouseout(function () {
            $(this).animate({ 'opacity': 1 }, "slow");
        });/**/

        $('.experiences-home .text-left').mouseover(function () {
            $(this).animate({ 'opacity': 0.5 }, "slow");
        });
        $('.experiences-home .text-left').mouseout(function () {
            $(this).animate({ 'opacity': 1}, "slow");
        });/**/


        /*$('.panel').on('load', function () {

        }).each(function (i) {
            $(this).mouseenter(function () {
                $(this).animate({ 'opacity': 0.5, 'width':'=-20px' });
            });
            $(this).mouseout(function () {
                $(this).animate({ 'opacity': 1, 'width': '=+20px' });
            });
        });*/



        /* activate the carousel */
        //$('#modalCarousel').carousel({interval:false});

        /* change modal title when slide changes */
        /*$('#modalCarousel').on('slid.bs.carousel', function () {
          $('.modal-title').html($(this).find('.active').attr("title"));
        })*/

        /* when clicking a thumbnail */
        $('.panel-thumbnail>a').click(function(e){
  
            e.preventDefault();
            var idx = $(this).parents('.panel').parent().index();
  	        var id = parseInt(idx);
  	
  	        $('#myModal').modal('show'); // show the modal
            $('#modalCarousel').carousel(id); // slide carousel to selected
  	        return false;
        });

        /*$("canvas[id^=demo]").each(function () {
            $(this)
        });*/


      tinymce.init({
          selector: 'textarea',
          plugin: 'a_tinymce_plugin',
          a_plugin_option: true,
          a_configuration_option: 400
      });



});