

$(document).ready(function () {

    //alert("test carousel");

    var AjaxRequest = function (targetElement, targetCOntroller, sendMethod) {
        var loading = $("#loading-pages").html();
        targetElement.html(loading);
        $.ajax({
            type: sendMethod,
            url: targetCOntroller,
            datatype: "html",
            /*error: function (status) {
                targetElement.html(status.responseText);
            },*/
            success: function (data) {
                targetElement.html(data);
            }
        });
    };


    $(function () {
        //alert($('.carousel item').length);
        $('.carousel').carousel({ interval: 7000 });
        $('.carousel-home-body').carousel({ interval: false });
        

        $('.av').click(function () {
            //alert("av!!");
            $('.carousel-home-body').carousel('next');
        })

        $('.ar').click(function () {
            $('.carousel-home-body').carousel('prev');
        })


    })


    $(function () {

        //Skills
        $("#Skills").click(function () {
            $('.carousel-home-body').carousel(0);
            AjaxRequest($('#page0'), "/Skills/_Index", "POST");
        });


        //Experiences
        $("#Experiences").click(function () {
            $('.carousel-home-body').carousel(1);
            AjaxRequest($('#page1'), "/Experiences/_Index", "POST");
        });

        //Projects
        $("#Projects").click(function () {
            $('.carousel-home-body').carousel(2);
            AjaxRequest($('#page2'), "/Projects/_Index", "POST");
        });

        //Education
        $("#Education").click(function () {
            $('.carousel-home-body').carousel(3);
            AjaxRequest($('#page3'), "/Education/_Index", "POST");
        });

        //Contact
        $("#Contact").click(function () {
            $('.carousel-home-body').carousel(4);
            //alert("Contact");
            AjaxRequest($('#page4'), "/Home/Contact", "GET");
        });

        //About
        $("#About").click(function () {
            $('.carousel-home-body').carousel(5);
            AjaxRequest($('#page5'), "/Home/About", "POST");
        });


    });




});