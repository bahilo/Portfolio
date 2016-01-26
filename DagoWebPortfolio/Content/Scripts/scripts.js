
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


        /* when clicking a thumbnail */
        $('.panel-thumbnail>a').click(function(e){
  
            e.preventDefault();
            var idx = $(this).parents('.panel').parent().index();
  	        var id = parseInt(idx);
  	
  	        $('#myModal').modal('show'); // show the modal
            $('#modalCarousel').carousel(id); // slide carousel to selected
  	        return false;
        });

       
       /**/
      tinymce.init({
          selector: 'textarea',
          plugin: 'a_tinymce_plugin',
          a_plugin_option: true,
          a_configuration_option: 400
      });

   

});