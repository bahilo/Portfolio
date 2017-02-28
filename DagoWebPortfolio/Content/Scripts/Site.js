
$(document).ready(function () {

    //alert("test");


    /*----[ Welcome Page ]*/
    //setInterval(function () {
    //    $('.welcome-page').animate({ 'background-size': '400%' }, 30000, 'linear');
    //    $('.welcome-page').animate({ 'background-size': '105%' }, 30000, 'linear');
    //}, 1000);

    $('.welcome-page').hover(
     function () {
         $(this).stop().animate({
             backgroundPositionX: '100%',
             backgroundPositionY: '100%'
         });
     },
     function () {
         $(this).stop().animate({
             backgroundPositionX: '10%',
             backgroundPositionY: '10%'
         });/**/
     }
   );
    
    $('.intro').children("*").mouseenter(function () {
        $(this).children("*").addClass('animated infinite pulse');
    });/**/
    $('.intro').children("*").mouseout(function () {
        $(this).children("*").removeClass('animated infinite pulse');
    });




        /*----[ Skills Page ]*/
    $('canvas').mouseenter(function () {
        $(this).addClass('animated infinite pulse');
    });
    $('canvas').mouseout(function () {
        $(this).removeClass('animated infinite pulse');
    });/**/

    $('.modal').dialog({
        /*height: 300,*/
        width: 600
    });
    
});

function setMenuActive(element) {
    resetMenuActive();
    element.classList.add("active");
}

function resetMenuActive() {
    menuElements = document.getElementsByClassName("mega-menu-item");
    for (i = 0; i < menuElements.length; i++) {
        menuElements[i].classList.remove("active");
    }
}





