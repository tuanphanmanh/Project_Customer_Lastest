var YRD_SCREEN = ".YRD_SCREEN";

$(window).resize(function () {
    try {
        LoadForm();
    } catch (ex) {
        console.error(ex.message, null);
    }
});
$(window).load(function (e) {
    try {
        LoadForm();
        loadAudio();
        runScreenJson();
        bindTitleHeader(); //nó lấy 1 lần lúc refesh trang
 
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

var wHour = 0;
var wContent = 0;
var hContent = 0;
var hitem = 0;
function LoadForm() {

    //width
    var w = $(window).width() - 4; 
    var w_title_left = Math.floor((w / 3) * 100) / 100;
    wContent = w - w_title_left;
    var w_title_txt = Math.floor((w_title_left / 11) * 100) / 100; 
    
    var counthour = $(".HEADER .HEADER_TITLE .HOUR .TIME_ITEM").length;
    wHour = Math.floor((wContent / counthour) * 100) / 100;
    wMinute = Math.floor(((wHour - 1)/ 6) * 100) / 100 - 1;

    $(".UNLOAD, .UNLOAD .HEADER").css({ "width": w + 4 });
    $(".UNLOAD .CONTENT .DATA").css({ "width": w });
    $(".UNLOAD .CONTENT").css({ "width": w + 24 });
    $(".HEADER .HEADER_TITLE, " + 
       ".DATA .DATA_TRUCK").css({ "width": w });

    $(".HEADER .REMARK").css({ "width": (w - w_title_left), "margin-left": w_title_left + "px" });

    $(".HEADER .HEADER_TITLE .H_TITLE.TXT, " +
       ".DATA .DATA_TRUCK .D_DATA.TITLE").css({ "width": w_title_left });
    $(".HEADER .HEADER_TITLE .H_TITLE.HOUR," +
       ".DATA .DATA_TRUCK .D_DATA.VALUE").css({ "width": wContent });
     

    $(".HEADER .HEADER_TITLE .H_TITLE.TXT .DOCK," +
       ".DATA .DATA_TRUCK .D_DATA.TITLE .DOCK, " +
       ".DATA .DATA_TRUCK .D_DATA.TITLE .DOCK span").css({ "width": (w_title_txt) - 2 });

    $(".HEADER .HEADER_TITLE .H_TITLE.TXT .TRUCK," + 
       ".DATA .DATA_TRUCK .D_DATA.TITLE .TRUCK").css({ "width": (w_title_txt * 3) - 2 });
    
    $(".HEADER .HEADER_TITLE .H_TITLE.TXT .SUPPLIER, " +
       ".DATA .DATA_TRUCK .D_DATA.TITLE .SUPPLIER").css({ "width": (w_title_txt * 7) - 2 });

    $(".HEADER .HEADER_TITLE .HOUR .TIME_ITEM, " +
       ".DATA .DATA_TRUCK .D_DATA.VALUE .TIME_ITEM").css({ "width": wHour - 1 });

    $(".HEADER .HEADER_TITLE .HOUR .TIME_ITEM .minute").css({ "margin-left": wMinute });

    //height 
    var h = $(window).height() - 4;
    var h_header2 = Math.floor((h / 35) * 100) / 100;

    var count = $(".UNLOAD .CONTENT .DATA.DOCK .DATA_TRUCK").length;

    hContent = h - h_header2;
    hitem = Math.floor(((hContent - (h_header2*2) + 5) / count) * 100) / 100;

    $(".HEADER .REMARK, .HEADER .HEADER_TITLE").css({ "height": (h_header2 - 2), "line-height": (h_header2 - 2) + "px" });
    $(".HEADER .REMARK table.tbremark").css({ "height": (h_header2 - 10), "line-height": (h_header2 - 10) + "px" });
    $(".HEADER .REMARK .tbremark .box_remark").css({ "height": (h_header2 - 14), "line-height": (h_header2 - 14) + "px" });
    $(".HEADER .HEADER_TITLE .HOUR .TIME_ITEM").css({ "height": (h_header2 - 2), "line-height": (h_header2 - 2) + "px" });
    $(".HEADER .HEADER_TITLE .HOUR .TIME_ITEM .minute").css({ "height": (h_header2 - 2) / 2, "line-height": (h_header2 - 2) / 2 + "px", "margin-top": (h_header2 - 2) / 2 + "px" });

    $(".UNLOAD .CONTENT").css({ "height": hContent });
        
    $(".CONTENT .DATA .DATA_TRUCK .D_DATA.TITLE .D_DATA_SUB, " +
       ".CONTENT .DATA .DATA_TRUCK .D_DATA.VALUE .TIME_ITEM," + 
       ".CONTENT .DATA .DATA_TRUCK .D_DATA.VALUE .item").css({ "height": (hitem - 2), "line-height": (hitem - 2) + "px" });

    //Load Shift 
    var objtime = $(".UNLOAD .worktime .shift .itemworktime");
    for (var i = 0; i < objtime.length; i++) {

        var start = $(objtime[i]).attr("start");
        var finish = $(objtime[i]).attr("finish");
        start = TryParseFloat(start, 0);
        finish = TryParseFloat(finish, 0);
        var wstart = start * wHour;
        var witem = finish * wHour;

        $(objtime[i]).css({ "margin-left": wstart + "px", "width": (witem) + "px", "height": (hContent + 2) + "px" });
    }

    //line-height DOCK TITLE 
    var objdock = $(".UNLOAD .CONTENT .DATA");
    for (var i = 0; i < objdock.length; i++) {
        var hdock = $(objdock[i]).height();
        $(objdock[i]).find(".FIRST_DOCK_ITEM").css({ "line-height": (hdock - 20) + "px" }); //20 height khoảng cách giữa 2 DOCK
    }
  
 }


var time_refesh = 300; //5 phút refesh 1 lần.
var current_refesh = 0;
function runScreenJson() {
    try {

        current_refesh++;
        if (current_refesh > time_refesh) { window.location.reload(); }
         
        $.ajax({
            url: baseUrl + "/TB_R_UNLOADING_PLAN/UNLOADING_MAIN_SCREEN_GETDATA2",
            type: 'GET',
            data: {
            },
            success: function (datajson) {
                try {
                    bindData(datajson); 
                    bindSound(datajson);
                } catch (ex) {
                    console.log("ERROR (Navigator_Data): " + ex.name + " - " + ex.message + " - " + ex.description); //ex.message && ex.name
                }
                setTimeout("runScreenJson()", 1000);
            },
            error: function () {
                console.log("ERROR Request : " + baseUrl + "/TB_R_UNLOADING_PLAN/UNLOADING_MAIN_SCREEN_GETDATA2"); //ex.message && ex.name
                setTimeout("runScreenJson()", 1000);
            }
        });
    } catch (ex) {
        console.log("ERROR (runScreenJson): " + ex.name + " - " + ex.message + " - " + ex.description); //ex.message && ex.name
        setTimeout("runScreenJson()", 1000);
    }
}


function bindData(jdata) {
 
    try {
        
        var NOW_DIFF = 0;
        if (jdata.length > 0) { NOW_DIFF = jdata[0].NOW_DIFF; } 
        //TIME LINE NOW 
        $(".UNLOAD .time_line_now").css({ "height": hContent + 2, "margin-left": (NOW_DIFF * wHour) - 1 + "px" });
        $(".UNLOAD .img_tamgiac img").css({ "margin-left": (NOW_DIFF * wHour) - 10 + "px" });
 
        for (var i = 0; i < jdata.length; i++) {
            
            var _key_truck = jdata[i].TRUCK.replace('.', '_') +
                                    jdata[i].SUPPLIERS.replaceAll('/', '').replaceAll(',', '').replaceAll(' ', '').replace("(", '').replace(")", '') + 
                                    jdata[i].ROW_NO;
            var _key = jdata[i].DOCK.replaceAll(" ", "").replace(".", "_") +
                           jdata[i].TRUCK.replace('.', '_') +
                           jdata[i].SUPPLIERS.replaceAll('/', '').replaceAll(',', '').replaceAll(' ', '').replace("(", '').replace(")", '') +
                           jdata[i].SEQUENCE_NO +
                           jdata[i].ROW_NO;
            
            //item
            var item = $(".CONTENT .DATA." + jdata[i].DOCK.replaceAll(' ', '').replace('.', '_') +
                               " .DATA_TRUCK." + _key_truck +
                               " .D_DATA .WARP .item." + _key); 
            if (item.length <= 0 && (jdata[i].PLAN_START_DIFF >= 0 && jdata[i].PLAN_FINISH_DIFF > 0)) {

                //add item 
                $(".CONTENT .DATA." + jdata[i].DOCK.replaceAll(' ', '').replace('.', '_') +
                               " .DATA_TRUCK." + _key_truck +
                               " .D_DATA .WARP").append(addItemHTML(jdata[i], _key));
                 
                item = $(".CONTENT .DATA." + jdata[i].DOCK.replaceAll(' ', '').replace('.', '_') +
                               " .DATA_TRUCK." + _key_truck +
                               " .D_DATA .WARP .item." + _key); 
            }
           
            //item actual
            var item_actual = $(".CONTENT .DATA." + jdata[i].DOCK.replaceAll(' ', '').replace('.', '_') +
                               " .DATA_TRUCK." + _key_truck +
                               " .D_DATA .WARP .item_actual." + _key);
            if (item_actual.length <= 0 && (jdata[i].ACTUAL_START_DIFF > 0)) {
                /*
                //add item actual 
                $(".CONTENT .DATA." + jdata[i].DOCK.replaceAll(' ', '').replace('.', '_') +
                               " .DATA_TRUCK." + _key_truck +
                               " .D_DATA .WARP").append(addItemActualHTML(jdata[i], _key));

                item_actual = $(".CONTENT .DATA." + jdata[i].DOCK.replaceAll(' ', '').replace('.', '_') +
                               " .DATA_TRUCK." + _key_truck +
                               " .D_DATA .WARP .item_actual." + _key);
                */
            }
           

            //item Revised
            var item_revised = $(".CONTENT .DATA." + jdata[i].DOCK.replaceAll(' ', '').replace('.', '_') +
                                           " .DATA_TRUCK." + _key_truck +
                                           " .D_DATA .WARP .item_revised." + _key);
            var item_muiten = $(".CONTENT .DATA." + jdata[i].DOCK.replaceAll(' ', '').replace('.', '_') +
                                           " .DATA_TRUCK." + _key_truck +
                                           " .D_DATA .WARP .item_muiten." + _key);
            if (item_revised.length <= 0 && (jdata[i].REVISED_START_DIFF > 0 && jdata[i].REVISED_FINISH_DIFF > 0)) {

                //add item Revised
                $(".CONTENT .DATA." + jdata[i].DOCK.replaceAll(' ', '').replace('.', '_') +
                               " .DATA_TRUCK." + _key_truck +
                               " .D_DATA .WARP").append(addItemRevisedHTML(jdata[i], _key));

                item_revised = $(".CONTENT .DATA." + jdata[i].DOCK.replaceAll(' ', '').replace('.', '_') +
                                           " .DATA_TRUCK." + _key_truck +
                                           " .D_DATA .WARP .item_revised." + _key);
                item_muiten = $(".CONTENT .DATA." + jdata[i].DOCK.replaceAll(' ', '').replace('.', '_') +
                                           " .DATA_TRUCK." + _key_truck +
                                           " .D_DATA .WARP .item_muiten." + _key);
            }

            var delaysecond = "";
            if (jdata[i].STATUS == "D" || jdata[i].STATUS == "E") {
                delaysecond = (jdata[i].ACTUAL_FINISH_UP_DELAY != 0) ? jdata[i].ACTUAL_FINISH_UP_DELAY : jdata[i].ACTUAL_START_UP_DELAY;
                delaysecond = (delaysecond == 0) ? "" : delaysecond;
            }
            
            if ($(item_revised).length > 0) {
                $(item).removeClass("STATUS_D STATUS_E STATUS_G STATUS_P");
                $(item).find("span").text("");

                $(item_revised).removeClass("STATUS_D STATUS_E STATUS_G STATUS_P");
                $(item_revised).addClass("STATUS_" + jdata[i].STATUS);
                $(item_revised).find("span").text(delaysecond);
            }
            else {
                $(item_revised).removeClass("STATUS_D STATUS_E STATUS_G STATUS_P");
                $(item_revised).find("span").text("");

                $(item).removeClass("STATUS_D STATUS_E STATUS_G STATUS_P");
                $(item).addClass("STATUS_" + jdata[i].STATUS);
                $(item).find("span").text(delaysecond); 
            }


            var _title = $(".CONTENT .DATA." + jdata[i].DOCK.replaceAll(' ', '').replace('.', '_') +
                               " .DATA_TRUCK." + _key_truck +
                               " .D_DATA_SUB.TRUCK, " +
                               ".CONTENT .DATA." + jdata[i].DOCK.replaceAll(' ', '').replace('.', '_') +
                               " .DATA_TRUCK." + _key_truck +
                               " .D_DATA_SUB.SUPPLIER");
            if (jdata[i].IS_CURRENT == "Y") {
                $(_title).addClass("IS_CURRENT");
            } else {
                $(_title).removeClass("IS_CURRENT");
            }


            //width
            bindWidthItem(item, item_actual, item_revised, item_muiten,  jdata[i]);
            
        }
         
        $(".CONTENT .DATA .DATA_TRUCK .D_DATA .item, " +
           ".CONTENT .DATA .DATA_TRUCK .D_DATA .item_actual, " + 
           ".CONTENT .DATA .DATA_TRUCK .D_DATA .item_revised").css({ "height": (hitem - 4), "line-height": (hitem - 4) + "px" });
        $(".CONTENT .DATA .DATA_TRUCK .D_DATA .item_muiten").css({ "margin-top": (hitem / 2) - 2 + "px" });

    } catch (ex) {
        console.log("ERROR (bindData): " + ex.name + " - " + ex.message + " - " + ex.description); //ex.message && ex.name 
    }
}


function addItemHTML(jdata, _key) { 
    var _htm = 
        '<div class="item ' + _key + ' STATUS_' + jdata.STATUS + ' " >' +
                    '    <div class="itemsub">' +
                    '       <span></span>' +
                    '    </div>' +                   
                    '</div>';
    return _htm;
}

function addItemActualHTML(jdata, _key) {
    var _htm = '<div class="item_actual ' + _key + '" >' +
                    '    <div class="itemsub">' +
                    '       <span></span>' +
                    '    </div>' +
                    '</div>';
    return _htm;
}

function addItemRevisedHTML(jdata, _key) {
    var _htm = '<div class="item_revised ' + _key + ' " >' +
                    '    <div class="itemsub">' +
                    '       <span></span>' +
                    '    </div>' +
                    '</div>'+
                    '<div class="item_muiten ' + _key + ' " >' +
                    '     <img src="/Content/Images/prev-2.png" />' +
                    '</div>';
    return _htm;
}


function bindWidthItem(item, item_actual, item_revised, item_muiten, jdata) {

    var wplanstart = (jdata.PLAN_START_DIFF * wHour);
    var wplanfinish = (jdata.PLAN_FINISH_DIFF * wHour);
    var wactualstart = (jdata.ACTUAL_START_DIFF * wHour);
    var wactualfinish = (jdata.ACTUAL_FINISH_DIFF * wHour);
    var wrevisedstart = (jdata.REVISED_START_DIFF * wHour);
    var wrevisedfinish = (jdata.REVISED_FINISH_DIFF * wHour);
    //var wnowdiff = (jdata.NOW_DIFF * wHour);

    if (wplanstart >= 0 || wplanfinish > 0) {
        $(item).css({ "margin-left": wplanstart + "px", "width": (wplanfinish - wplanstart) }).addClass("PLAN");
    }
    //if (wactualstart > 0) {
    //    if (wactualfinish <= 0) { wactualfinish = wnowdiff; }
    //    $(item_actual).css({ "margin-left": wactualstart + "px", "width": (wactualfinish - wactualstart) });
    //} 
    if (wrevisedstart > 0) {
        $(item_revised).css({ "margin-left": wrevisedstart + "px", "width": (wrevisedfinish - wrevisedstart) - 2 + "px" });
        //if (wactualstart > 0) { 
        //    $(item_muiten).css({ "margin-left": wactualfinish + 4 + "px", "width": (wrevisedstart - (wactualfinish + 5)) - 4 + "px" });
        //}
        if (wplanstart > 0) {
            $(item_muiten).css({ "margin-left": wplanfinish + 2 + "px", "width": (wrevisedstart - (wplanfinish + 3)) - 2 + "px" });
            if (((wrevisedstart - (wplanfinish + 3)) - 2) <= 15) {
                $(item_muiten).hide();
            } else {
                $(item_muiten).show();
            }
        }
    }


}


function CASE_LINE_TO(jdata) {


    var w = $(window).width();
    var h = $(window).height();

    $(".YRD .CaseCanvas").show();
    var _canvas = $(".YRD .CaseCanvas")[0];

    if (_canvas.getContext) {
        var ctx = _canvas.getContext("2d");

        ctx.lineWidth = 2;              // set line width
        ctx.strokeStyle = '#ff0000'; // set line color

        for (var i = 0; i < jdata.length; i++) {

            if (jdata[i].SHIFT_NOW != jdata[i].DATA_SHIFT) { continue; }

            //if (jdata[i].PIO_STATUS >= 1) {

            var plan_item = $(".YRD_SCREEN .LINE." + jdata[i].MEMBER + " .DATA .PLAN_LINE .item." + jdata[i].GRADE.replace(".", "_"));
            var plan_offset = $(plan_item).offset();
            var actual_item = $(".YRD_SCREEN .LINE." + jdata[i].MEMBER + " .DATA .ACTUAL_LINE .item." + jdata[i].GRADE.replace(".", "_"));
            var actual_offset = $(actual_item).offset();

            var p_height = Math.floor((plan_offset.top + $(plan_item).height()) * 100) / 100;
            var p_width = Math.floor((plan_offset.left + $(plan_item).width()) * 100) / 100;


            if ($(actual_item).length > 0 && $(actual_item).width() > 0) {

                var a_height = Math.floor((actual_offset.top) * 100) / 100;
                var a_width = Math.floor((actual_offset.left + $(actual_item).width()) * 100) / 100;

                ctx.beginPath();
                ctx.moveTo(p_width + 2, p_height + 2);
                ctx.lineTo(a_width + 2, a_height + 2);
                ctx.stroke();
            }
            //}
        }
    }
}


var audio_url_delay = "/Content/Audio/100-Delay-tacktime-line.mp3";
var audio_url_early = "/Content/Audio/103-Delay-finish-case.mp3";
function bindSound(jdata) {

    soundstart = audioElement.getAttribute("soundstart");
    var _type = "";
    for (var i = 0; i < jdata.length; i++) {

        if (jdata[i].STATUS == "D" || jdata[i].STATUS == "E")
        {
            _type = jdata[i].STATUS;
            break;
        }
    }


    if (_type = ! "") { 
        if (soundstart != "true") {
            //Start 
            var _url = (_type == "D") ? audio_url_delay : audio_url_delay;
            audioElement.setAttribute("src", _url);
            audioElement.setAttribute("soundstart", "true");
            audioElement.setAttribute("soundtype", _type);
            // audioElement.setAttribute("LOOP", "true");
            audioplay();
        }
    } else { 
        audiostop();
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
    audioElement.setAttribute("soundtype", "");
    audioElement.setAttribute("soundstart", "false");
    //stopTimeOut();
}
function stopTimeOut() {
    if (timer) {
        clearTimeout(timer);
        timer = 0;
    }
}
function loadAudio() {
    audioElement = document.createElement('audio');
    audioElement.setAttribute("LOOP", "true");
}
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



String.prototype.replaceAll = function(search, replacement) {
    var target = this;
    return target.replace(new RegExp(search, 'g'), replacement);
};
 

String.prototype.replaceAll2 = function(search, replacement) {
    var target = this;
    return target.split(search).join(replacement);
};