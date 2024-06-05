
$(window).resize(function () {
    try {
        LoadForm();
    } catch (ex) {
        console.error(ex.message, null);
    }
});

$(window).load(function (e) {
    try {
        //LoadForm();
        //runScreenJson();
        //bindTitleHeader(); //nó lấy 1 lần lúc refesh trang
        //loadAudio();
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


var time_refesh = 600; //1 tiếng refesh 1 lần.
var current_refesh = 0;
function runScreenJson() {
    try {

        current_refesh++;
        if (current_refesh > time_refesh) { window.location.reload(); }
         
        $.ajax({
            url: baseUrl + "/TB_R_UNLOADING_PLAN/UNLOADING_MAIN_SCREEN_GETDATA",
            type: 'GET',
            data: {
            },
            success: function (datajson) {
                try {
                    bindData(datajson); 

                } catch (ex) {
                    console.log("ERROR (Navigator_Data): " + ex.name + " - " + ex.message + " - " + ex.description); //ex.message && ex.name
                }
                setTimeout("runScreenJson()", 100000);
            },
            error: function () {
                console.log("ERROR Request : " + baseUrl + "/TB_R_UNLOADING_PLAN/UNLOADING_MAIN_SCREEN_GETDATA"); //ex.message && ex.name
                setTimeout("runScreenJson()", 100000);
            }
        });
    } catch (ex) {
        console.log("ERROR (runScreenJson): " + ex.name + " - " + ex.message + " - " + ex.description); //ex.message && ex.name
        setTimeout("runScreenJson()", 100000);
    }
}


function bindData(jdata) {
    try {




      
    } catch (ex) {
        console.log("ERROR (bindData): " + ex.name + " - " + ex.message + " - " + ex.description); //ex.message && ex.name
    }
}


function bindTitleHeader() {
    //TITLE HEADER  
    var d = new Date();

    var strdatetmp = d.getDate() + "-" + getMonthEN(d.getMonth()).substring(0, 3); // + " (" + getTime(d).replace(" : ", ":") + ")";
    $(".YRD_SCREEN .LINE_HEADER .HEAD_TITLE").text(strdatetmp);

}

/**********************************TIME*******************************************/
function jsonDateToDate(dt) {
    return new Date(parseInt(dt.substr(6)));
}
function toHHMMSS(ttseconds) {
    var sec_num = parseInt(ttseconds, 0); // don't forget the second param
    var hours = Math.floor(sec_num / 3600);
    var minutes = Math.floor((sec_num - (hours * 3600)) / 60);
    var seconds = sec_num - (hours * 3600) - (minutes * 60);

    if (hours < 10) { hours = "0" + hours; }
    if (minutes < 10) { minutes = "0" + minutes; }
    if (seconds < 10) { seconds = "0" + seconds; }
    var time = hours + ':' + minutes + ':' + seconds;
    return time;
}
function toHHMM(ttseconds) {
    var sec_num = parseInt(ttseconds, 0); // don't forget the second param
    var hours = Math.floor(sec_num / 3600);
    var minutes = Math.floor((sec_num - (hours * 3600)) / 60);
    //var seconds = sec_num - (hours * 3600) - (minutes * 60);

    if (hours < 10) { hours = "0" + hours; }
    if (minutes < 10) { minutes = "0" + minutes; }
    //if (seconds < 10) { seconds = "0" + seconds; }
    var time = hours + ':' + minutes; //+ ':' + seconds;
    return time;
}
function toHHh_MMm(ttseconds) {
    var sec_num = parseInt(ttseconds, 0); // don't forget the second param
    var hours = Math.floor(sec_num / 3600);
    var minutes = Math.floor((sec_num - (hours * 3600)) / 60);
    //var seconds = sec_num - (hours * 3600) - (minutes * 60);

    //if (hours < 10) { hours = "0" + hours; }
    //if (minutes < 10) { minutes = "0" + minutes; }
    //if (seconds < 10) { seconds = "0" + seconds; }

    hours = (hours == 0) ? "" : hours + 'h ';
    //minutes = (minutes == 0) ? "" :  minutes;

    var time = hours + minutes + "m"; //+ ':' + seconds;
    return time;
}
function toMMSS(ttseconds) {
    var sec_num = parseInt(ttseconds, 0); // don't forget the second param
    //var hours = Math.floor(sec_num / 3600);
    var minutes = Math.floor(sec_num / 60);
    var seconds = sec_num - (minutes * 60);

    //if (hours < 10) { hours = "0" + hours; }
    if (minutes < 10) { minutes = "0" + minutes; }
    if (seconds < 10) { seconds = "0" + seconds; }
    var time = minutes + ':' + seconds;
    return time;
}
function getMonthEN(m) {
    switch (m) {
        case 0: return "January";
        case 1: return "February";
        case 2: return "March";
        case 3: return "April";
        case 4: return "May";
        case 5: return "June";
        case 6: return "July";
        case 7: return "August";
        case 8: return "September";
        case 9: return "October";
        case 10: return "November";
        case 11: return "December";
        default: return "";
    }
}

function getTime(dt) {
    var strtime = (((dt.getHours() + "").length == 1) ? ("0" + dt.getHours()) : dt.getHours()) + " : " + (((dt.getMinutes() + "").length == 1) ? ("0" + dt.getMinutes()) : dt.getMinutes())
    return strtime;
}
/**********************************PAUSE*******************************************/
function showDialog(myoverlay, mypopup) {
    var p = $("." + mypopup);
    var o = $("." + myoverlay);
    var wheight = $(document).height();
    $(o).fadeIn().css("height", "auto"); 
    $(p).fadeIn().css("margin-top", (wheight - p.height()) / 2 > 100 ? ((wheight - p.height()) / 2) : 100); //(wheight - p.height()) / 2 > 80 ? (wheight - p.height()) / 2 : 80);
    $("body").css('overflow', 'hidden'); 
};

function closeDialog(myoverlay, mypopup) {
    $('.' + myoverlay).fadeOut();
    $('.' + mypopup).fadeOut();
    $("body").css('overflow', 'auto');
};
/***********************************SOUND*************************************************/
var soundstart = "false";
var soundphaytime = 0;
var soundtype = '';
var audioElement;
var timer;

function audioplay() { audioElement.play(); }
function audiopause() { audioElement.pause(); }
function audiostop() {

    audioElement.pause();
    //audioElement.currentTime = 0;
    audioElement.setAttribute("src", "");
    $(".soundAttribute").attr({ "soundstart": "false", "soundtype": "", "src": "", "caseno": "" });
    stopTimeOut();
}
function stopTimeOut() {
    if (timer) {
        clearTimeout(timer);
        timer = 0;
    }
}
function loadAudio() { audioElement = document.createElement('audio'); }
function StartSound(urlsound, tacktime, stype, caseno) {

    soundstart = $(".soundAttribute").attr("soundstart");
    soundtype = $(".soundAttribute").attr("soundtype");

    if (soundstart == "false") {
        soundphaytime = 0;
        soundtype = stype;
        //Chạy Sound 

        audioElement.setAttribute("src", urlsound);
        audioElement.setAttribute("currentTime", tacktime);
        audioElement.setAttribute("LOOP", "true");
        audioplay();


        $(".soundAttribute").attr({ "soundstart": "true", "soundtype": stype, "src": urlsound, "caseno": caseno });
        soundstart = "true";

        timer = setTimeout("StartSound('" + urlsound + "'," + tacktime + ",'" + soundtype + "','" + caseno + "')", 1000);

    } else if (soundtype == stype && soundstart == "true" && soundphaytime <= tacktime) {

        soundphaytime++;
        //$(".message4").html(soundphaytime + " - " + soundstart + " - " + urlsound);
        timer = setTimeout("StartSound('" + urlsound + "','" + tacktime + "','" + soundtype + "','" + caseno + "')", 1000);
    } else {
        audiostop();
    }
}
/******************************END SOUND**************************************************/


/////////////////////////////////////function//////////////////////
function TryParseInt(str, defaultValue) {
    var retValue = defaultValue;
    if (str != null) {
        if (str.length > 0) {
            if (!isNaN(str)) {
                retValue = parseInt(str);
            }
        }
    }
    return retValue;
}

function TryParseFloat(str, defaultValue) {
    var retValue = defaultValue;
    if (str != null && str.length > 0) {
        if (!isNaN(str)) {
            retValue = parseFloat(str);
        }
    }
    return retValue;
}

function FLASH(obj) {
    //$(obj).animate({ opacity: 0 }, 300).animate({ opacity: 1 }, 300);
    //if ($(obj).hasClass("color_none")) {
    //    $(obj).removeClass("color_none");
    //} else {
    //    $(obj).addClass("color_none");
    //}
    $(obj).fadeOut().fadeIn();
}
function EndFLASH(obj) {
    //$(obj).removeClass("color_none");
    $(obj).clearQueue().fadeIn();
}

function EndFLASH_Out(obj) {
    //$(obj).removeClass("color_none");
    $(obj).clearQueue().fadeOut();
}

//console.log("Làm tròn lên: " + Math.ceil(myNumber));
//console.log("Làm tròn xuống: " + Math.floor(myNumber));



/** QR Code generate function **/
function GenerateQRCode(_qrcode) {
 
    var sSize = 115; //fixed size = 100;    
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