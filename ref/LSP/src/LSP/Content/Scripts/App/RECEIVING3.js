
$(window).resize(function () {
    LoadForm();
});
$(window).load(function (e) {
    LoadForm();
    runScreenJson();
    //loadAudio();  
});
$(document).ready(function () {

});
var typefocus = 0;

var w;
var w_top;

var h;
var h_header;
var h_content;
var h_item;
function LoadForm() {

    //resize Width
    w = $(window).width() - 2 - 40;
    w_top = (Math.floor((w) * 100) / 100) - 2;
    w_top2 = (Math.floor((w_top / 4) * 100) / 100) - 2;
  
    $(".UNLOADING").css({ "width": w, "margin-left" : 20 + "px" });
    $(".UNLOADING .ST_TOP, .UNLOADING .ST_MAIN, .UNLOADING .ST_LINE").css({ "width": w - 4 });
    $(".UNLOADING .ST_MAIN .MAIN_LEFT").css({ "width": w  - 15 });
   
    $(".UNLOADING .TOP_ITEM.TOTAL_QTY").css({ "width": w_top });
    $(".UNLOADING .TOP_ITEM.TOTAL_QTY .title").css({ "width": w_top2 - 4 });
    $(".UNLOADING .TOP_ITEM.TOTAL_QTY .val").css({ "width": w_top - w_top2 - 4 });

    //resize Height
    h = $(window).height() - 2;
    h_header = (Math.floor((h / 12) * 100) / 100) - 2;
    h_content = h - h_header - 32;
    h_item = (Math.floor((h_content / 14) * 100) / 100) - 2;
    h_item2 = (Math.floor((h_item / 2) * 100) / 100) - 2;

    $(".UNLOADING").css({ "height": h, "line-height": h + "px" });
    $(".UNLOADING .ST_TOP").css({ "height": h_header + 10, "line-height": h_header + 10 + "px" });    
    $(".UNLOADING .ST_TOP .TOP_ITEM .TOP_ITEM_SUB").css({ "height": (h_header - 20), "line-height": (h_header - 20) + "px" });

    $(".UNLOADING .ST_MAIN .ST_CONTENT, .UNLOADING .ST_MAIN .ST_CONTENT .WARP").css({ "height": h_content  });
    $(".UNLOADING .ST_MAIN .ST_LINE ").css({ "height": h_item2 - 2, "line-height": (h_item2 - 2) + "px" });
    $(".UNLOADING .ST_MAIN .ST_LINE .ST_COL").css({ "height": h_item2, "line-height": (h_item2) + "px" });        
}
function reloadForm() {

    $(".UNLOADING").css({ "width": w });
    $(".UNLOADING .ST_TOP, .UNLOADING .ST_MAIN, .UNLOADING .ST_LINE").css({ "width": w - 4 });
    $(".UNLOADING .ST_MAIN .MAIN_LEFT").css({ "width": w  - 15 });
           
    $(".UNLOADING").css({ "height": h, "line-height": h + "px" });
    $(".UNLOADING .ST_TOP").css({ "height": h_header + 10, "line-height": h_header + 10 + "px" });    
    $(".UNLOADING .ST_TOP .TOP_ITEM .TOP_ITEM_SUB").css({ "height": (h_header - 20), "line-height": (h_header - 20) + "px" });

    $(".UNLOADING .ST_MAIN .HEADER, .UNLOADING .ST_MAIN .ST_CONTENT").css({ "height": h_content  });
    $(".UNLOADING .ST_MAIN .ST_LINE, .UNLOADING .ST_MAIN .ST_LINE .ST_COL").css({ "height": h_item, "line-height": (h_item) + "px" });   
}

var isfirstday = false;
var time_refesh = 300; //10 phút refesh 1 lần.
var current_refesh = 0;

function runScreenJson() {
    var d = new Date();
    var n = d.getHours();
 
    if ((n > 6 || n < 5)) {
        if (isfirstday) { window.location.reload(); }

        current_refesh++;
        if (current_refesh > time_refesh) { window.location.reload(); }

        var start_time = new Date().getTime();
        try {

            $.ajax({
                url: baseUrl + "/TB_R_CONTENT_LIST/RECEIVING3_GETDATA",
                type: 'GET',
                data: {                    
                },
                success: function (datajson) {
                    try {
                        //datajson[0] for all list, datajson[1] for actual/plan header
                        Navigator_Data(datajson[0], datajson[1]);
                    } catch (ex) {
                        console.log("ERROR (Navigator_Data): " + ex.name + " - " + ex.message + " - " + ex.description); //ex.message && ex.name
                    }
                    var request_time = new Date().getTime() - start_time;
                    request_time = (3000 - request_time) < 0 ? 0 : (3000 - request_time);
                    setTimeout("runScreenJson()", request_time);
                },
                error: function () {

                    console.log("ERROR Request : " + baseUrl + "/TB_R_CONTENT_LIST/RECEIVING3_GETDATA"); //ex.message && ex.name
                    setTimeout("runScreenJson()", 3000);
                }
            });
        } catch (ex) {
            console.log("ERROR (runScreenJson): " + ex.name + " - " + ex.message + " - " + ex.description); //ex.message && ex.name
            setTimeout("runScreenJson()", 3000);
        }
    }
    else {
        isfirstday = true; setTimeout("runScreenJson()", 3000);
    }

}

function Navigator_Data(jdata, jdataHeader) {

    var objItems1 = $(".MAIN_LEFT .ST_CONTENT .WARP");    
    var _html1 = "";
    var _html2 = "";
    var _count = jdata.length;
    var _plan = 0;
    var _actual = 0;
    for (var i = 0; i < jdata.length ; i++) {                    
        _html1 = _html1 + addItem(jdata[i], i+1);       
    }

    for (var i = jdata.length; i < 13 ; i++) {                      
       _html1 = _html1 + addItemNone((i + 1));      
    }

    _plan = jdataHeader.PLAN_PALLET_QTY;
    _actual =  jdataHeader.ACTUAL_PALLET_QTY;

    $(".TOTAL_QTY .TOP_ITEM_SUB.val").text(_actual + " / " + _plan);

    $(objItems1).html(_html1);    
    reloadForm(); 

    flashing_warning(jdata);    
}

function flashing_warning(jdata) {
    for (var i = 0; i < jdata.length ; i++) {
        if (jdata[i].IS_ALARM_ON == 'Y') {
            var obj = $(".ROW_NO_" + jdata[i].ROW_NO);
            FLASH(obj);
        }       
    }   
}

function addItem(jdata, no) {

    var status = "";
    if (jdata.IS_ALARM_ON == "Y") {
        status = "W";
    }
    else if (jdata.ACTUAL_PALLET_QTY == jdata.PLAN_PALLET_QTY) {
        status = "D";
    } else if ((jdata.ACTUAL_PALLET_QTY > 0 && jdata.ACTUAL_PALLET_QTY < jdata.PLAN_PALLET_QTY) || (jdata.STATUS == 'R' && jdata.ACTUAL_PALLET_QTY == 0))
    {
        status = "S";  // its mean: on going
    }
    else if (jdata.ACTUAL_PALLET_QTY > jdata.PLAN_PALLET_QTY && jdata.IS_ALARM_ON == 'N') {
        status = "D";
    }    
    
    var _htm = "<div class='ST_LINE ITEM_" + jdata.ROW_NO + " STATUS_" + status + " version_no_" + jdata.ROW_NO + " ROW_NO_" + jdata.ROW_NO + "'>" +
                        "        <div class='ST_COL STT1 NO'><span>" + no + "</span></div>" +
                        "        <div class='ST_COL STT2 PC_ADDRESS'><span>" + isnull(jdata.DOCK_NO, '') + "</span></div>" +
                        "        <div class='ST_COL STT3 INVOICE_NO'><span>" + jdata.ORDER_NO + "</span></div>" +
                        "        <div class='ST_COL STT4 OVER'><span>" + jdata.ETA + "</span></div>" +
                        "        <div class='ST_COL STT5 PLAN_PALLET'><span>" + jdata.PLAN_PALLET_QTY + "</span></div>" +
                        "        <div class='ST_COL STT6 ACTUAL_PALLET STATUS_'><span>" + jdata.ACTUAL_PALLET_QTY + "</span></div>" +
                        "        <div class='ST_COL STT7 REMARK'><span>" + jdata.RECEIVING_ISSUE + "</span></div>" +
                    "</div>";
    return _htm;
}


function addItemNone(no) {

    var _htm = "<div class='ST_LINE ITEM_ STATUS_ version_no_' >" +
                        "        <div class='ST_COL STT1 NO'><span></span>" + no + "</div>" +
                        "        <div class='ST_COL STT2 PC_ADDRESS'><span> </span></div>" +
                        "        <div class='ST_COL STT3 INVOICE_NO'><span> </span></div>" +
                        "        <div class='ST_COL STT4 OVER'><span> </span></div>" +
                        "        <div class='ST_COL STT5 PLAN_PALLET'><span></span></div>" +
                        "        <div class='ST_COL STT6 ACTUAL_PALLET '><span></span></div>" +
                        "        <div class='ST_COL STT7 REMARK '><span></span></div>" +
                    "</div>";
    return _htm;
}

function isnull(_val, _default) {
    if (_val == null) {
        return _default
    }
    return _val;
}
/**********************************TIME*******************************************/
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
/**********************************PAUSE*******************************************/
function showDialog(myoverlay, mypopup) {
    var p = $('.' + mypopup);
    var o = $("." + myoverlay);
    var wheight = $(document).height();
    p.fadeIn();
    o.fadeIn();
    o.css("height", "auto");
    p.css("margin-top", (wheight - p.height()) / 2 > 100 ? ((wheight - p.height()) / 2) : 100); //(wheight - p.height()) / 2 > 80 ? (wheight - p.height()) / 2 : 80);
    $("body").css('overflow', 'hidden');

};
function closeDialog(myoverlay, mypopup) {
    $('.' + myoverlay).fadeOut();
    $('.' + mypopup).fadeOut();
    $("body").css('overflow', 'auto');
};
/***********************************SOUND*************************************************/


/////////////////////////////////////function//////////////////////
function TryParseInt(str, defaultValue) {
    var retValue = defaultValue;
    if (str !== null) {
        if (str.length > 0) {
            if (!isNaN(str)) {
                retValue = parseInt(str);
            }
        }
    }
    return retValue;
}

function FLASH(obj) {
    $(obj).fadeOut().fadeIn();
}
function END_FLASH(obj) {
    $(obj).clearQueue().fadeIn();
}


///////////////////////////////////////////////////////////




