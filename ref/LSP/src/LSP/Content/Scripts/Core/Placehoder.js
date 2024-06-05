$(document).ready(function () {
    $(".dxcontent_search .placehoder").click(function () {
        $(this).parent().find("input").focus();
    });

    //Textbox
    $(".dxcontent_search .textbox input").focus(function () {
        $(this).parent().parent().parent().parent().parent().find(".placehoder").addClass("hide");
    });
    $(".dxcontent_search .textbox input").focusout(function () {
        $(this).parent().parent().parent().parent().parent().find(".placehoder").removeClass("hide");
    });
    $(".dxcontent_search .textbox input").keyup(function () {
        if ($(this).val() != "") {
            $(this).parent().parent().parent().parent().parent().find(".placehoder").hide();
        } else {
            $(this).parent().parent().parent().parent().parent().find(".placehoder").show();
        }
    });

    //DateTime
    $(".dxcontent_search .textdate input").focus(function () {
        $(this).parent().parent().parent().parent().parent().find(".placehoder").addClass("hide");
        if ($(this).val() != "") {
            $(this).parent().parent().parent().parent().parent().find(".placehoder").hide();
        } else {
            $(this).parent().parent().parent().parent().parent().find(".placehoder").show();
        }
    });
    $(".dxcontent_search .textdate input").focusout(function () {
        $(this).parent().parent().parent().parent().parent().find(".placehoder").removeClass("hide");
        if ($(this).val() != "") { 
            $(this).parent().parent().parent().parent().parent().find(".placehoder").hide();
        } else { 
            $(this).parent().parent().parent().parent().parent().find(".placehoder").show();
        }
    });
    $(".dxcontent_search .textdate input").keyup(function () {
        if ($(this).val() != "") {
            $(this).parent().parent().parent().parent().parent().find(".placehoder").hide();
        } else {
            $(this).parent().parent().parent().parent().parent().find(".placehoder").show();
        }
    });


    //ComboBox
    $(".dxcontent_search .textcombobox input").focus(function () {
        $(this).parent().parent().parent().parent().parent().find(".placehoder").addClass("hide");
        if ($(this).val() != "") {
            $(this).parent().parent().parent().parent().parent().find(".placehoder").hide();
        } else {
            $(this).parent().parent().parent().parent().parent().find(".placehoder").show();
        }
    });
    $(".dxcontent_search .textcombobox input").focusout(function () {
        $(this).parent().parent().parent().parent().parent().find(".placehoder").removeClass("hide");
        if ($(this).val() != "") {
            $(this).parent().parent().parent().parent().parent().find(".placehoder").hide();
        } else {
            $(this).parent().parent().parent().parent().parent().find(".placehoder").show();
        }
    });
    $(".dxcontent_search .textcombobox input").keyup(function () {
        if ($(this).val() != "") {
            $(this).parent().parent().parent().parent().parent().find(".placehoder").hide();
        } else {
            $(this).parent().parent().parent().parent().parent().find(".placehoder").show();
        }
    });


    $(".dxcontent_search .autobind input").keyup();
});

