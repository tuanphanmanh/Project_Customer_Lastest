$(window).load(function (e) {
    try {
        //LoadForm();
        //runScreenJson();
        //bindTitleHeader(); //nó lấy 1 lần lúc refesh trang
        //loadAudio();
        GenerateQRCode();
    } catch (ex) {
        console.error(ex.message, null);
    }
});

/** QR Code generate function **/
function GenerateQRCode() {

    var sSize = 55; //fixed size = 70;    
    var _qr = $(".QRCode");
    for (var i = 0; i < _qr.length; i++) {
        var _qrcode = $(_qr[i]).text().trim();
         
        if (_qrcode != null && _qrcode !== '') { 
            $(_qr[i]).empty().qrcode({
                render: "canvas",
                text: "" + _qrcode,
                ecLevel: "H",
                size: sSize
            });
        }

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