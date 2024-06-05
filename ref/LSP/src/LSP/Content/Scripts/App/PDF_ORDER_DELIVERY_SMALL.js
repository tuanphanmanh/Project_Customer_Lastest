
$(window).resize(function () {
    try {
        LoadForm();
    } catch (ex) {
        console.error(ex.message, null);
    }
});

$(window).load(function (e) {
    try {       
        GenerateQRCode($(".QRCode").text().trim());
    } catch (ex) {
        console.error(ex.message, null);
    }
});

$(document).ready(function () {
    try {
        

    } catch (ex) {
        console.error(ex.message, null);
    }
});

function LoadForm() {

    //width
    var w = $(window).width() - 4; 
    var w_title_left = Math.floor((w / 8) * 100) / 100;
    wContent = w - w_title_left;
    var w_title_txt = Math.floor((w_title_left / 4) * 100) / 100;
    
    $(".MAIN").css({ "width": w });

    //height 
    var h = $(window).height() - 4;
    var h_header2 = Math.floor((h / 35) * 100) / 100;
    
 }

/** QR Code generate function **/
function GenerateQRCode(_qrcode) {
 
    var sSize = 110; //fixed size = 100;    
    if (_qrcode != null && _qrcode !== '') {
        $(".QRCode").empty().qrcode({
            render: "canvas",
            text: "" + _qrcode,
            ecLevel: "H",
            size: sSize
        });
    }
}

/*To fix Callback issue*/
function ReGenerateQRCode() {
    var divs = $('[id^=QR_CODE_]').toArray().reverse();
    for (var i = 0; i < divs.length; i++) {
        var Idselector = '#' + divs[i].id;
        GenerateQRCode(Idselector);
    }
}