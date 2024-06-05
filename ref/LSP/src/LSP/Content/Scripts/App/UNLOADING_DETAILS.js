
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
    w = $(window).width() - 2;
    w_top = (Math.floor((w / 2) * 100) / 100) - 2;
    w_top2 = (Math.floor((w_top / 4) * 100) / 100) - 2;

    $(".UNLOADING").css({ "width": w });
    $(".UNLOADING .ST_TOP, .UNLOADING .ST_MAIN, .UNLOADING .ST_LINE").css({ "width": w - 4 });
    $(".UNLOADING .TOP_ITEM").css({ "width": w_top });
    $(".UNLOADING .TOP_ITEM .title").css({ "width": w_top2 - 4 });
    $(".UNLOADING .TOP_ITEM .val").css({ "width": w_top - w_top2 - 4 });
    
    

    //resize Height
    h = $(window).height() - 44;
    h_header = (Math.floor((h / 12) * 100) / 100) - 2;
    h_content = h - h_header - 2; 
    h_item = (Math.floor((h_content / 15) * 100) / 100) - 2;
    h_item2 = (Math.floor((h_item / 2) * 100) / 100) - 2;
    
    $(".UNLOADING").css({ "height": h, "line-height": h + "px" });
    $(".UNLOADING .ST_TOP").css({ "height": h_header + 10, "line-height": h_header +10 + "px" });
    //$(".UNLOADING .ST_TOP .TOP_ITEM").css({ "height": (h_header - 20), "line-height": (h_header - 20) + "px" });
    $(".UNLOADING .ST_TOP .TOP_ITEM .TOP_ITEM_SUB").css({ "height": (h_header - 20), "line-height": (h_header - 20) + "px" });

     
    $(".UNLOADING .ST_MAIN .ST_CONTENT, .UNLOADING .ST_MAIN .ST_CONTENT .WARP").css({ "height": h_content - h_item });
    $(".UNLOADING .ST_MAIN .ST_LINE ").css({ "height": h_item2 - 2, "line-height": (h_item2 - 2) + "px" });
    $(".UNLOADING .ST_MAIN .ST_LINE .ST_COL").css({ "height": h_item2, "line-height": (h_item2) + "px" });
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
                url: baseUrl + "/TB_R_CONTENT_LIST/UNLOADING_DETAILS_GETDATA",
                type: 'GET',
                data: {
                    //USER_ID: $("#_INPUT_TOP").val().trim()
                },
                success: function (datajson) { 
                    try {
                       
                        Navigator_Data(datajson); 
                    } catch (ex) {
                        console.log("ERROR (Navigator_Data): " + ex.name + " - " + ex.message + " - " + ex.description); //ex.message && ex.name
                    }
                    var request_time = new Date().getTime() - start_time;
                    request_time = (1000 - request_time) < 0 ? 0 : (1000 - request_time);
                    //setTimeout("runScreenJson()", request_time);
                },
                error: function () {
                    
                    console.log("ERROR Request : " + baseUrl + "/TB_R_CONTENT_LIST/STOCK_TAKING_GETDATA"); //ex.message && ex.name
                    setTimeout("runScreenJson()", 1000);
                }
            });
        } catch (ex) {
            console.log("ERROR (runScreenJson): " + ex.name + " - " + ex.message + " - " + ex.description); //ex.message && ex.name
            setTimeout("runScreenJson()", 1000);
        }
    }
    else {
        isfirstday = true; setTimeout("runScreenJson()", 10000);
    }

}


function Navigator_Data(jdata) {
     
    
        var objItems = $(".ST_CONTENT .WARP");
        var _html = "";
        for (var i = 0; i < jdata.length ; i++) {
            _html = _html + addItem(jdata[i]);

        }

        $(objItems).html(_html);
        reloadForm();
 
       
 


}
 
function addItem(jdata) {
  
    var _htm = "<div class='ST_LINE ITEM_" + jdata.ID + " STATUS_" + jdata.IS_ACTIVE + " version_no_" + jdata.ROW_NO + "' >" +
                        "        <div class='ST_COL STT1 NO'><span>" + jdata.ROW_NO + "</span></div>" +
                        "        <div class='ST_COL STT2 INVOICE_NO'><span>" + jdata.SUPPLIER_CODE + "</span></div>" +
                        "        <div class='ST_COL STT3 PLAN_PALLET'><span>" + jdata.SUPPLIER_CODE + "</span></div>" +
                        "        <div class='ST_COL STT4 ACTUAL_PALLET'><span>" + jdata.SUPPLIER_CODE + "</span></div>" +
                        "        <div class='ST_COL STT5 PC_ADDRESS'><span>" + jdata.SUPPLIER_CODE + "</span></div>" +
                        "        <div class='ST_COL STT6 OVER'><span>" + jdata.SUPPLIER_CODE + "</span></div>" +
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
 
 


 