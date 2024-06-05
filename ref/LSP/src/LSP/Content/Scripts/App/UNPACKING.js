
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
var w_orderno;
var w_qty;

var h;
var h_header;
var h_content;
var h_item;
function LoadForm() {

    //resize Width
    w = $(window).width() - 2;
    w_title = (Math.floor((w / 10) * 100) / 100) - 2;
    w_orderno = (Math.floor(((w * 2) / 3) * 100) / 100) - 2;
    w_qty = w - w_orderno - 2;

    //w_top = (Math.floor((w / 2) * 100) / 100) - 2;
    //w_top2 = (Math.floor((w_top / 4) * 100) / 100) - 2;

    $(".UNLOADING").css({ "width": w });
    $(".UNLOADING .ST_TOP, .UNLOADING .ST_MAIN, .UNLOADING .ST_LINE").css({ "width": w - 4 });

    //$(".UNLOADING .TOP_ITEM.ORDER_NO").css({ "width": w_top });
    $(".UNLOADING .TOP_ITEM.ORDER_NO .title").css({ "width": w_title - 4 });
    $(".UNLOADING .TOP_ITEM.ORDER_NO .val").css({ "width": w_orderno - w_title - 4 });

    //$(".UNLOADING .TOP_ITEM.TOTAL_QTY").css({ "width": w_top });
    $(".UNLOADING .TOP_ITEM.TOTAL_QTY .title").css({ "width": w_title - 4 });
    $(".UNLOADING .TOP_ITEM.TOTAL_QTY .val").css({ "width": w_qty - w_title - 4 });
    
    

    //resize Height
    h = $(window).height() - 44;
    h_header = (Math.floor((h / 12) * 100) / 100) - 2;
    h_content = h - h_header - 2; 
    h_item = (Math.floor((h_content / 15) * 100) / 100) - 2;
    h_item2 = (Math.floor(((h_item * 2) / 3) * 100) / 100) - 2;
    
    $(".UNLOADING").css({ "height": h, "line-height": h + "px" });
    $(".UNLOADING .ST_TOP").css({ "height": h_header + 10, "line-height": h_header +10 + "px" });
    //$(".UNLOADING .ST_TOP .TOP_ITEM").css({ "height": (h_header - 20), "line-height": (h_header - 20) + "px" });
    $(".UNLOADING .ST_TOP .TOP_ITEM .TOP_ITEM_SUB").css({ "height": (h_header - 20), "line-height": (h_header - 20) + "px" });

     
    $(".UNLOADING .ST_MAIN .ST_CONTENT, .UNLOADING .ST_MAIN .ST_CONTENT .WARP").css({ "height": h_content - h_item });
    $(".UNLOADING .ST_MAIN .ST_HEADER .ST_LINE ").css({ "height": h_item2 - 2, "line-height": (h_item2 - 2) + "px" });
    $(".UNLOADING .ST_MAIN .ST_HEADER .ST_LINE .ST_COL").css({ "height": h_item2, "line-height": (h_item2) + "px" });
    //$(".UNLOADING .ST_MAIN .ST_HEADER .ST_LINE .ST_COL.s2").css({ "height": h_item, "line-height": (h_item / 2) + "px" });


    var h_3 = (Math.floor((h / 3) * 100) / 100) - 2;
    var h_input = (h_3 * 2); 
    var w_input2 = h_input - 60;
    var h_inputNumber = (Math.floor(((w_input2 - 30) / 6) * 100) / 100) -2;
    var h_inputNum = (Math.floor(((h_inputNumber * 5) / 4) * 100) / 100) - 2;
    var w_inputNum = (Math.floor(((w_input2) / 3) * 100) / 100) - 2; 
 
}
function reloadForm() {

    $(".UNLOADING").css({ "width": w });
    $(".UNLOADING .ST_TOP, .UNLOADING .ST_MAIN, .UNLOADING .ST_LINE").css({ "width": w - 4 });

    //$(".UNLOADING .TOP_ITEM.ORDER_NO").css({ "width": w_top });
    $(".UNLOADING .TOP_ITEM.ORDER_NO .title").css({ "width": w_title - 4 });
    $(".UNLOADING .TOP_ITEM.ORDER_NO .val").css({ "width": w_orderno - w_title - 4 });

    //$(".UNLOADING .TOP_ITEM.TOTAL_QTY").css({ "width": w_top });
    $(".UNLOADING .TOP_ITEM.TOTAL_QTY .title").css({ "width": w_title - 4 });
    $(".UNLOADING .TOP_ITEM.TOTAL_QTY .val").css({ "width": w_qty - w_title - 4 });
 

    $(".UNLOADING").css({ "height": h, "line-height": h + "px" });
    $(".UNLOADING .ST_TOP").css({ "height": h_header + 10, "line-height": h_header + 10 + "px" });
    //$(".UNLOADING .ST_TOP .TOP_ITEM").css({ "height": (h_header - 20), "line-height": (h_header - 20) + "px" });
    $(".UNLOADING .ST_TOP .TOP_ITEM .TOP_ITEM_SUB").css({ "height": (h_header - 20), "line-height": (h_header - 20) + "px" });
     
    $(".UNLOADING .ST_MAIN .ST_CONTENT").css({ "height": h_content });
    $(".UNLOADING .ST_MAIN .ST_CONTENT .ST_LINE, .UNLOADING .ST_MAIN .ST_CONTENT .ST_LINE .ST_COL").css({ "height": h_item, "line-height": (h_item) + "px" });
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
                url: baseUrl + "/TB_R_CONTENT_LIST/UNPACKING_GETDATA",
                type: 'GET',
                data: { 
                },
                success: function (datajson) { 
                    try {
                        
                        //BindCURRENT_SCAN(datajson[0]);
                        BindKANBAN(datajson);

                    } catch (ex) {
                        console.log("ERROR (CURRENT_SCAN): " + ex.name + " - " + ex.message + " - " + ex.description); //ex.message && ex.name
                    }
                    var request_time = new Date().getTime() - start_time;
                    request_time = (3000 - request_time) < 0 ? 0 : (3000 - request_time);
                    setTimeout("runScreenJson()", request_time);
                },
                error: function () {
                    
                    console.log("ERROR Request : " + baseUrl + "/TB_R_CONTENT_LIST/UNPACKING_GETDATA"); //ex.message && ex.name
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


function BindCURRENT_SCAN(jdata) {

    $(".ORDER_NO .val").text(jdata.ORDER_NO);
    $(".TOTAL_QTY .val").text(jdata.SCAN_QTY + "/" + jdata.PALLET_BOX_QTY);

}

function BindKANBAN(jdata) {
        //$(".ORDER_NO .val").text(jdata.ORDER_NO);
        //$(".TOTAL_QTY .val").text(jdata.ACTUAL_BOX_QTY + "/" + jdata.BOX_QTY);
    
        var objItems = $(".ST_CONTENT .WARP");
        var _html = "";
        var _sum_ACTUAL_BOX_QTY = 0;
        var _sum_BOX_QTY = 0;

        for (var i = 0; i < jdata.length ; i++) {
           
            if (i == 0) { $(".ORDER_NO .val").text(jdata[i].CONTENT_NO); }

            _html = _html + addItem(jdata[i]);

            _sum_ACTUAL_BOX_QTY = _sum_ACTUAL_BOX_QTY + jdata[i].ACTUAL_BOX_QTY;
            _sum_BOX_QTY = _sum_BOX_QTY + jdata[i].BOX_QTY; 
        }

        $(".TOTAL_QTY .val").text(_sum_ACTUAL_BOX_QTY + "/" + _sum_BOX_QTY);
        $(objItems).html(_html);
        reloadForm();

        //warning
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
        css = "W";
    }
    else if (jdata.ACTUAL_BOX_QTY >= jdata.BOX_QTY) { css = "D"; }


    var _htm = "<div class='ST_LINE ITEM_" + jdata.ID + " STATUS_" + css + " version_no_" + jdata.ROW_NO + " ROW_NO_" + jdata.ROW_NO + "'>" +
                        "        <div class='ST_COL STT1 NO'><span>" + jdata.ROW_NO + "</span></div>" +
                        "        <div class='ST_COL STT2 MODEL'><span>" + "" + "</span></div>" +
                        "        <div class='ST_COL STT3 BACK_NO'><span>" + jdata.BACK_NO + "</span></div>" +
                        "        <div class='ST_COL STT4 PART_NO'><span>" + jdata.PART_NO + "</span></div>" +
                        "        <div class='ST_COL STT5 PART_NAME'><span>" + jdata.PART_NAME + "</span></div>" +
                        "        <div class='ST_COL STT6 QTY_BOX'><span>" + jdata.BOX_SIZE + "</span></div>" +
                        "        <div class='ST_COL STT7 PLAN_BOX'><span>" + jdata.BOX_QTY + "</span></div>" +
                        "        <div class='ST_COL STT8 ACTUAL_BOX'><span>" + jdata.ACTUAL_BOX_QTY + "</span></div>" +
                        "        <div class='ST_COL STT9 QTY'><span>" + (jdata.BOX_SIZE * jdata.ACTUAL_BOX_QTY) + "</span></div>" +
                        "        <div class='ST_COL STT10 PC_ADDRESS'><span>" + jdata.PC_ADDRESS + "</span></div>" +
                        "        <div class='ST_COL STT11 OVER'><span>" + isnull(jdata.OVER,'') + "</span></div>" +
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


 
