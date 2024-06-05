
$(window).resize(function () {
    try {
        //LoadForm();
    } catch (ex) {
        console.error(ex.message, null);
    }
});

$(window).load(function (e) {
    try {
                 
        var obj = $(".QRCode");
        var objFN = $(".QRCode_FN");
        var sSize = 65; //fixed size = 100;    

        for (var i = 0; i < obj.length; i++) {
            //Content No
            var _qrcode = $(obj[i]).text().trim();                           
            if (_qrcode != null && _qrcode !== '') {
                $(obj[i]).empty().qrcode({
                    render: "canvas",
                    text: "" + _qrcode,
                    ecLevel: "H",
                    size: sSize
                });
            }

            //For finish QR Code
            var _qrcodeFN = $(objFN[i]).text().trim();
            if (_qrcodeFN != null && _qrcodeFN !== '') {
                $(objFN[i]).empty().qrcode({
                    render: "canvas",
                    text: "" + _qrcodeFN,
                    ecLevel: "H",
                    size: 55
                });
            }
        } 

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

