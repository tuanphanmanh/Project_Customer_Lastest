
$(window).resize(function () {
    try {
        //LoadForm();
    } catch (ex) {
        console.error(ex.message, null);
    }
});

$(window).load(function (e) {
    try {                
        
        //Content No
        var sSize = 65; //fixed size = 100; 
        var _qrcode = $(".QRCode").text().trim();
        if (_qrcode != null && _qrcode !== '') {
            $(".QRCode").empty().qrcode({
                render: "canvas",
                text: "" + _qrcode,
                ecLevel: "H",
                size: sSize
            });
        }

        //For finish QR Code        
        var _qrcodeFN = $(".QRCode_FN").text().trim();
        if (_qrcodeFN != null && _qrcodeFN !== '') {
            $(".QRCode_FN").empty().qrcode({
                render: "canvas",
                text: "" + _qrcodeFN,
                ecLevel: "H",
                size: 55
            });
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
  