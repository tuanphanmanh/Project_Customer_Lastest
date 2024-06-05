
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
var w_title;
var w_contentno;
var w_qty;

var h;
var h_header;
var h_content;
var h_item;
function LoadForm() {

    //resize Width
    w = $(window).width() - 2 - 40;
    w_top = (Math.floor((w) * 100) / 100) - 2;
    w_top_item = (Math.floor((w_top / 20) * 100) / 100) - 2;

    //w_topl = w - w_top - 4;
    //w_topl2 = (Math.floor((w_topl / 4) * 100) / 100) - 2;

    $(".UNLOADING").css({ "width": w, "margin-left": 20 + "px" });
    $(".UNLOADING .ST_TOP, .UNLOADING .ST_MAIN, .UNLOADING .ST_LINE").css({ "width": w - 4 });
    $(".UNLOADING .ST_MAIN .MAIN_LEFT, " +
       ".UNLOADING .ST_MAIN .MAIN_RIGHT").css({ "width": (w / 2) - 6 });

    //$(".UNLOADING .TOP_ITEM.TRUCK_NAME").css({ "width": w_topl });
    //$(".UNLOADING .TOP_ITEM.TRUCK_NAME .title").css({ "width": w_topl2 - 4 });
    //$(".UNLOADING .TOP_ITEM.TRUCK_NAME .val").css({ "width": w_topl - w_topl2 - 4 });

    $(".UNLOADING .TOP_ITEM.TOTAL_QTY.BOX").css({ "width": w_top_item * 7 });
    $(".UNLOADING .TOP_ITEM.TOTAL_QTY.FORSHIFT").css({ "width": w_top_item * 13, "margin-left": w_top_item / 4});
    $(".UNLOADING .TOP_ITEM.TOTAL_QTY .title").css({ "width": w_top_item * 3 - 4 });
    $(".UNLOADING .TOP_ITEM.TOTAL_QTY .val").css({ "width": w_top_item * 10 - 4 });
    $(".UNLOADING .TOP_ITEM.TOTAL_QTY .val1").css({ "width": w_top_item * 4 - 4 });


    //resize Height
    h = $(window).height() - 2;
    h_header = (Math.floor((h / 12) * 100) / 100) - 2;
    h_content = h - h_header*2 - 2;
    h_item = (Math.floor(((h_content -12) / 14) * 100) / 100) - 2;
    h_item2 = (Math.floor((h_item / 2) * 100) / 100) - 2;

    $(".UNLOADING").css({ "height": h, "line-height": h + "px" });
    $(".UNLOADING .ST_TOP").css({ "height": h_header + 10, "line-height": h_header + 10 + "px" });
    //$(".UNLOADING .ST_TOP .TOP_ITEM").css({ "height": (h_header - 20), "line-height": (h_header - 20) + "px" });
    $(".UNLOADING .ST_TOP .TOP_ITEM .TOP_ITEM_SUB").css({ "height": (h_header - 20), "line-height": (h_header - 20) + "px" });


    $(".UNLOADING .ST_MAIN .ST_CONTENT, .UNLOADING .ST_MAIN .ST_CONTENT .WARP").css({ "height": h_content });
    $(".UNLOADING .ST_MAIN .ST_LINE ").css({ "height": h_item2 - 2, "line-height": (h_item2 - 2) + "px" });
    $(".UNLOADING .ST_MAIN .ST_LINE .ST_COL").css({ "height": h_item2, "line-height": (h_item2) + "px" });
    //$(".UNLOADING .ST_MAIN .ST_HEADER .ST_LINE .ST_COL.s2").css({ "height": h_item, "line-height": (h_item / 2) + "px" });


    var h_3 = (Math.floor((h / 3) * 100) / 100) - 2;
    var h_input = (h_3 * 2);
    var w_input2 = h_input - 60;
    var h_inputNumber = (Math.floor(((w_input2 - 30) / 6) * 100) / 100) - 2;
    var h_inputNum = (Math.floor(((h_inputNumber * 5) / 4) * 100) / 100) - 2;
    var w_inputNum = (Math.floor(((w_input2) / 3) * 100) / 100) - 2;

 
}
function reloadForm() {

    $(".UNLOADING").css({ "width": w });
    $(".UNLOADING .ST_TOP, .UNLOADING .ST_MAIN, .UNLOADING .ST_LINE").css({ "width": w - 4 });
    $(".UNLOADING .ST_MAIN .MAIN_LEFT, " +
       ".UNLOADING .ST_MAIN .MAIN_RIGHT").css({ "width": (w / 2) - 6 });

    $(".UNLOADING .TOP_ITEM.INPUT, .UNLOADING .TOP_ITEM.FINISH").css({ "width": w_top });
    $(".UNLOADING .TOP_ITEM.FLOOR_ADDRESS").css({ "width": (w_top * 2) - (w_top / 1.5) });
    $(".UNLOADING .TOP_ITEM.RACK_NO").css({ "width": (w_top / 1.5) });


    $(".UNLOADING").css({ "height": h, "line-height": h + "px" });
    $(".UNLOADING .ST_TOP").css({ "height": h_header + 10, "line-height": h_header + 10 + "px" });
    //$(".UNLOADING .ST_TOP .TOP_ITEM").css({ "height": (h_header - 20), "line-height": (h_header - 20) + "px" });
    $(".UNLOADING .ST_TOP .TOP_ITEM .TOP_ITEM_SUB").css({ "height": (h_header - 20), "line-height": (h_header - 20) + "px" });

    $(".UNLOADING .ST_MAIN .HEADER, .UNLOADING .ST_MAIN .ST_CONTENT").css({ "height": h_content });
    $(".UNLOADING .ST_MAIN .ST_LINE, .UNLOADING .ST_MAIN .ST_LINE .ST_COL").css({ "height": h_item, "line-height": (h_item) + "px" });
    //$(".UNLOADING .ST_MAIN .ST_HEADER .ST_LINE .ST_COL.s2").css({ "height": h_item, "line-height": (h_item / 2) + "px" });
      
}

var isfirstday = false;

function runScreenJson() {
    var d = new Date();
    var n = d.getHours();

    if ((n > 6 || n < 5)) {
        if (isfirstday) { window.location.reload(); }

        var start_time = new Date().getTime();
        try {
            
            $.ajax({
                url: baseUrl + "/TB_R_CONTENT_LIST/UNPACKING_GETDATA2",
                type: 'GET',
                data: { 
                },
                success: function (datajson) { 
                    try {
                                                
                        BindKANBAN(datajson[0], datajson[1]);

                    } catch (ex) {
                        console.log("ERROR (CURRENT_SCAN): " + ex.name + " - " + ex.message + " - " + ex.description); //ex.message && ex.name
                    }
                    var request_time = new Date().getTime() - start_time;
                    request_time = (3000 - request_time) < 0 ? 0 : (3000 - request_time);
                    setTimeout("runScreenJson()", request_time);
                },
                error: function () {
                    
                    console.log("ERROR Request : " + baseUrl + "/TB_R_CONTENT_LIST/UNPACKING_GETDATA2"); //ex.message && ex.name
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

function BindKANBAN(jdata, jdataHeader) {
    
    var objItems1 = $(".MAIN_LEFT .ST_CONTENT .WARP");
    var objItems2 = $(".MAIN_RIGHT .ST_CONTENT .WARP");
    var _html1 = "";
    var _html2 = "";
    var _count = jdata.length;
    var _plan = 0;
    var _actual = 0;
    for (var i = 0; i < jdata.length ; i++) {       
        if (i <= 13) {
            _html1 = _html1 + addItem(jdata[i], i + 1);
        } else {
            _html2 = _html2 + addItem(jdata[i], i + 1);
        }      
    }

    for (var i = jdata.length; i < 26 ; i++) {        
        if (i <= 13) {
            _html1 = _html1 + addItemNone((i + 1));
        } else {
            _html2 = _html2 + addItemNone((i + 1));
        }
    }
    _planBoxNotScan = jdataHeader.PLAN_BOX_QTY_NOT_SCAN
    _plan = jdataHeader.PALLET_BOX_QTY;
    _actual = jdataHeader.BOX_QTY_2;
    $(".TOTAL_QTY .TOP_ITEM_SUB.val1").text(isnull(_planBoxNotScan,""));
    $(".TOTAL_QTY .TOP_ITEM_SUB.val").text(isnull(_actual + " / " + _plan,""));

    $(objItems1).html(_html1);
    $(objItems2).html(_html2);
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

function addItem(jdata) {

    var css = "";
    
    if (jdata.IS_ALARM_ON == 'Y')
    {      
        css = "W"; //alarm on
    }
    else if (jdata.ACTUAL_BOX_QTY >= jdata.BOX_QTY && jdata.UNPACK_STATUS =="D") { css = "D"; } //Done
    else if (jdata.ACTUAL_BOX_QTY > 0) { css = "S"; } // its mean: on going


    var _htm = "<div class='ST_LINE ITEM_" + jdata.ID + " STATUS_" + css + " version_no_" + jdata.ROW_NO + " ROW_NO_" + jdata.ROW_NO + "'>" +
                        "        <div class='ST_COL STT1 NO'><span>" + jdata.ROW_NO + "</span></div>" +
                        "        <div class='ST_COL STT2 CONTENT_NO'><span>" + jdata.CONTENT_NO + "</span></div>" +
                        "        <div class='ST_COL STT3 ETA'><span>" + jdata.UP_EST_DATETIME_HHMMSS.substring(0, 5) + "</span></div>" +
                        "        <div class='ST_COL STT4 PLAN_BOX'><span>" + jdata.BOX_QTY + "</span></div>" +
                        "        <div class='ST_COL STT5 ACTUAL_BOX'><span>" + jdata.ACTUAL_BOX_QTY + "</span></div>" +
                        "        <div class='ST_COL STT6 REMARK'><span>" + jdata.REMARK + "</span></div>" +
                       
                        "</div>";
    return _htm;
}
 
function addItemNone(no) {

    var _htm = "<div class='ST_LINE ITEM_ STATUS_ version_no_' >" +
                        "        <div class='ST_COL STT1 NO'><span></span>" + no + "</div>" +
                        "        <div class='ST_COL STT2 CONTENT_NO'><span> </span></div>" +
                        "        <div class='ST_COL STT3 ETA'><span> </span></div>" +
                        "        <div class='ST_COL STT4 PLAN_BOX'><span> </span></div>" +
                        "        <div class='ST_COL STT5 ACTUAL_BOX'><span> </span></div>" +
                        "        <div class='ST_COL STT6 REMARK'><span></span></div>" +
                    "</div>";
    return _htm;
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
 
function isnull(obj, str) {
    if (obj == null) {
        return str;
    }
    return obj;
}


 
