var baseUrl;

function bookmark() {
    var title = document.title;
    var url = document.location.href;

    if (window.sidebar && window.sidebar.addPanel) {
        /* Mozilla Firefox Bookmark - works with opening in a side panel only ? */
        window.sidebar.addPanel(title, url, "");
    } else if (window.opera && window.print) {
        /* Opera Hotlist */
        alert("Press Control + D to bookmark");
        return true;
    } else if (window.external) {
        /* IE Favorite */
        try {
            window.external.AddFavorite(url, title);
        } catch (e) {
            alert("Press Control + D to bookmark");
        }
    } else {
        /* Other */
        alert("Press Control + D to bookmark");
    }
}

function formatDate(dateObj) {
    try {
        if (!dateObj.getDate) {
            dateObj = new Date(parseFloat(dateObj.replace("/Date(", "").replace(")/", "")))
        }
        var date = "0" + dateObj.getDate()
        date = date.substr(date.length - 2)
        var month = "0" + (dateObj.getMonth() + 1)
        month = month.substr(month.length - 2)
        var year = "000" + dateObj.getFullYear()
        year = year.substr(year.length - 4)
        var dateFormated = date + "/" + month + "/" + year
        return dateFormated == "01/01/0001" ? "" : dateFormated
    }
    catch (ex) {
        return dateObj;
    }
}

function ConvertDate(_date) {
    //dd/MM/yyy -> MM/dd/yyyy
    if (_date == null || _date == "") {
        return "";
    }

    var spec = "/";
    var item = _date.split(spec);
    if (item.length != 3) {
        return _date;
    }

    return item[1] + spec + item[0] + spec + item[2];
}

$(document).ready(function () {
    loadTimeClock();
    loadUserProfile();
    loadMenu();
    fixHeightForm();
    JOB_AddUrlFunction();
    //Sys.Application.add_load(LoadSecurityUrlFunction);
    //getSecurityButton();
    //alert("load Security");
});

function loadTimeClock() {
    if (document.getElementById("systemTime")) {
        timerClock();
    }
}

function loadUserProfile() {
    // alert(1);
    //
    $("#btnProfile").click(function () {
        $.ajax({
            url: baseUrl + "/User/GetUserProfile",
            method: "post",
            dataType: "json"
        }).done(function (response) {
            $("#lblDealer").html(response.DEALER)
            txtFullName1.SetText(response.FULLNAME)
            txtUsername1.SetText(response.ACCOUNT_NAME)
            txtLastLogin.SetText(formatDate(response.LAST_LOGIN_DATE))
            txtLastChangePass.SetText(formatDate(response.LAST_CHANGE_PASS))
            txtRegisterDate.SetText(formatDate(response.REGISTER_DATE))
            cboStatus1.SetValue(response.IS_ACTIVE)
        }).fail(function () {

        });
        popProfile.Show();
        return false;
    });
    //
    $("#btnChangePass").click(function () {
        txtOldPassword.SetText("")
        txtChangePassword.SetText("")
        txtReChangePassword.SetText("")
        popChangePass.Show()
        return false
    });
    //
}

function loadMenu() {
    SetSelectedMenuItem("dxm-item", "dxm-selected", "?");
    SetSelectedMenuItem("dxnb-item", "dxnb-itemSelected", "&");
    SetSelectedMenuItem("system-menu-item", "selected", "&");
}

function SetSelectedMenuItem(itemClass, selectedClass, spliter) {
    $("." + itemClass).each(function () {
        if ($(this).hasClass(selectedClass)) {
            return
        }
        var currUrl = location.href
        var itemLink = $(this).find("a").attr("href")
        if (!itemLink) {
            return
        }
        if (currUrl.toLowerCase().split(spliter)[0].replace(baseUrl.toLowerCase(), "") == itemLink.toLowerCase()) {
            $(this).addClass(selectedClass)
        }
    })
}

function timerClock() {
    $.ajax({
        type: "GET",
        url: baseUrl + "/Home/TimerClock"
    }).done(function (feedback) {
        $("#systemTime").text(feedback)
        setTimeout("timerClock()", 10000)
    });
    ajaxfunction = 1;
}

//var is_process = false;
function OnBtnChangePassClicked(s) {


    //if (is_process) {
    //    return
    //}
    //is_process = true;

    //var $btn = $(s)
    //if ($btn.hasClass("saving")) {
    //    return
    //}
    //$btn.addClass("saving")
    //$("#changePassStatus").html("Changing...")

    try {
        $.ajax({
            url: baseUrl + "/ChangePassword/ChangePass",
            method: "post",
            data: {
                oldPass: "", // $("#txtOldPassword").val(),
                newpassword: $("#txtNewPassword").val(),
                confirmpassword: $("#txtConfirmPassword").val()
            },
            success: function (data) {
                if (data.ResultCode) {
                    alert(data.ResultDesc);
                    window.location.href = baseUrl;// @Html.Toyota().Page.HomeUrl;
                }
                else {
                    alert(data.ResultDesc);
                    $("#txtNewPassword").val('');
                    $("#txtConfirmPassword").val('');
                    $("#txtNewPassword").focus();
                }

                //is_process = false;
                //$btn.removeClass("saving");
            }
        });
    } catch (ex) {
        //is_process = false;
    }

}
//    },
//    dataType: "json",
//    success: function (data) {
//    if(data.ResultCode)
//    {
//        msgOk(data.ResultDesc);
//        window.location.href = "@Html.Toyota().Page.HomeUrl";
//    }
//    else
//    {
//        msgError(data.ResultDesc);
//        $("#txtNewPassword").val('');
//        $("#txtConfirmPassword").val('');
//        $("#txtNewPassword").focus();
//    }  
//} 
//})
//    //.done(function (response) {
//    //$("#changePassStatus").html("")
//    //$btn.removeClass("saving")
//    //alert(response.message)
//    //if (response.success) {
//    //    // popChangePass.Hide()
//    //}
//})


function fixHeightForm() {
    var h = $(window).height();
    $("#sidebar").css("min-height", h - 166);
}

function validatereset() {
    $(".messageerror").html("");
    $(".errorValidate, .errorValidateInteger, .errorValidateTime").removeClass("errorValidate errorValidateInteger errorValidateTime");
}

function doValidate() {
    var isValid = true;
    //Reset message
    validatereset();

    //required
    var ValRequired = $(".val-required input[type='text']");
    for (var i = 0; i < ValRequired.length; i++) {
        if ($(ValRequired[i]).parents().filter(".hide").length > 0 && ($(ValRequired[i]).parents().filter(".hide").css('display') == 'none')) {
            continue;
        }

        var objerror = $(ValRequired[i]).parent().parent().parent().parent();
        var messerror = $(objerror).parent().find(".messageerror");

        if ($(ValRequired[i]).val() == "") {
            isValid = false;
            //alert($(ValRequired[i]).attr("id"));
            $(objerror).addClass("errorValidate");
            $(messerror).html("This field is required.");
        } else {
            $(ValRequired[i]).parent().parent().parent().parent().removeClass("errorValidate").parent().find(".messageerror").html("");
        }
    }

    //required integer
    var ValRequiredInt = $(".val-integer input[type='text']");
    for (var i = 0; i < ValRequiredInt.length; i++) {
        if ($(ValRequiredInt[i]).parents().filter(".hide").length > 0 && ($(ValRequiredInt[i]).parents().filter(".hide").css('display') == 'none')) {
            continue;
        }
        var ivalue = $(ValRequiredInt[i]).val();
        var objerror = $(ValRequiredInt[i]).parent().parent().parent().parent();
        var messerror = $(objerror).parent().find(".messageerror");

        if (ivalue != "") {
            try {
                if (ivalue == parseInt(ivalue)) {
                    $(objerror).removeClass("errorValidateInteger");
                }
                else {
                    isValid = false;
                    //alert($(ValRequiredInt[i]).attr("id"));
                    $(objerror).addClass("errorValidateInteger");
                    var messa = ($(messerror).html().trim() == "") ? "" : ($(messerror).html().trim() + "<br />");
                    $(messerror).html(messa + "This field is format Integer.");
                }
            }
            catch (err) {
                isValid = false;
                //alert($(ValRequiredInt[i]).attr("id"));
                $(objerror).addClass("errorValidateInteger");
                var messa = ($(messerror).html().trim() == "") ? "" : ($(messerror).html().trim() + "<br />");
                $(messerror).html(messa + "This field is format Integer.");
            }
        } else {
            $(objerror).removeClass("errorValidateInteger");
        }
    }

    //required time , val-time
    var ValRequiredTime = $(".val-time:not(.hide) input[type='text']");
    for (var i = 0; i < ValRequiredTime.length; i++) {
        if ($(ValRequiredTime[i]).parents().filter(".hide").length > 0 && ($(ValRequiredTime[i]).parents().filter(".hide").css('display') == 'none')) {
            continue;
        }
        var ivalue = $(ValRequiredTime[i]).val();
        var objerror = $(ValRequiredTime[i]).parent().parent().parent().parent();
        var messerror = $(objerror).parent().find(".messageerror");

        if (ivalue != "") {
            var ivals = ivalue.split(":");
            if (ivals.length <= 1 || ivals.length > 3) {
                isValid = false;
                //alert($(ValRequiredTime[i]).attr("id"));
                $(objerror).addClass("errorValidateTime");
                var messa = ($(messerror).html().trim() == "") ? "" : ($(messerror).html().trim() + "<br />");
                $(messerror).html(messa + "This field is format Time .Ex: 00:00, 00:00:00 ...");
            } else {
                for (var d = 0; d < ivals.length; d++) {
                    if (ivals[d] == parseInt(ivals[d])) {
                        $(objerror).removeClass("errorValidateTime");
                    }
                    else {
                        isValid = false;
                        //alert($(ValRequiredTime[i]).attr("id"));
                        $(objerror).addClass("errorValidateTime");
                        var messa = ($(messerror).html().trim() == "") ? "" : ($(messerror).html().trim() + "<br />");
                        $(messerror).html(messa + "This field is format Time .Ex: 00:00, 00:00:00 ...");
                    }
                }
            }
        } else {
            $(objerror).removeClass("errorValidateTime");
        }
    }

    return isValid;
}

function doValidateParam(csskey) {
    var isValid = true;
    //Reset message
    validatereset();

    //required
    var ValRequired = $("." + csskey + " .val-required input[type='text']");
    for (var i = 0; i < ValRequired.length; i++) {
        if ($(ValRequired[i]).parents().filter(".hide").length > 0 && ($(ValRequired[i]).parents().filter(".hide").css('display') == 'none')) {
            continue;
        }

        var objerror = $(ValRequired[i]).parent().parent().parent().parent();
        var messerror = $(objerror).parent().find(".messageerror");

        if ($(ValRequired[i]).val() == "") {
            isValid = false;
            //alert($(ValRequired[i]).attr("id"));
            $(objerror).addClass("errorValidate");
            $(messerror).html("This field is required.");
        } else {
            $(ValRequired[i]).parent().parent().parent().parent().removeClass("errorValidate").parent().find(".messageerror").html("");
        }
    }

    //required integer
    var ValRequiredInt = $("." + csskey + " .val-integer input[type='text']");
    for (var i = 0; i < ValRequiredInt.length; i++) {
        if ($(ValRequiredInt[i]).parents().filter(".hide").length > 0 && ($(ValRequiredInt[i]).parents().filter(".hide").css('display') == 'none')) {
            continue;
        }
        var ivalue = $(ValRequiredInt[i]).val();
        var objerror = $(ValRequiredInt[i]).parent().parent().parent().parent();
        var messerror = $(objerror).parent().find(".messageerror");

        if (ivalue != "") {
            try {
                if (ivalue == parseInt(ivalue)) {
                    $(objerror).removeClass("errorValidateInteger");
                }
                else {
                    isValid = false;
                    //alert($(ValRequiredInt[i]).attr("id"));
                    $(objerror).addClass("errorValidateInteger");
                    var messa = ($(messerror).html().trim() == "") ? "" : ($(messerror).html().trim() + "<br />");
                    $(messerror).html(messa + "This field is format Integer.");
                }
            }
            catch (err) {
                isValid = false;
                //alert($(ValRequiredInt[i]).attr("id"));
                $(objerror).addClass("errorValidateInteger");
                var messa = ($(messerror).html().trim() == "") ? "" : ($(messerror).html().trim() + "<br />");
                $(messerror).html(messa + "This field is format Integer.");
            }
        } else {
            $(objerror).removeClass("errorValidateInteger");
        }
    }

    //required time , val-time
    var ValRequiredTime = $("." + csskey + " .val-time:not(.hide) input[type='text']");
    for (var i = 0; i < ValRequiredTime.length; i++) {
        if ($(ValRequiredTime[i]).parents().filter(".hide").length > 0 && ($(ValRequiredTime[i]).parents().filter(".hide").css('display') == 'none')) {
            continue;
        }
        var ivalue = $(ValRequiredTime[i]).val();
        var objerror = $(ValRequiredTime[i]).parent().parent().parent().parent();
        var messerror = $(objerror).parent().find(".messageerror");

        if (ivalue != "") {
            var ivals = ivalue.split(":");
            if (ivals.length <= 1 || ivals.length > 3) {
                isValid = false;
                //alert($(ValRequiredTime[i]).attr("id"));
                $(objerror).addClass("errorValidateTime");
                var messa = ($(messerror).html().trim() == "") ? "" : ($(messerror).html().trim() + "<br />");
                $(messerror).html(messa + "This field is format Time .Ex: 00:00, 00:00:00 ...");
            } else {
                for (var d = 0; d < ivals.length; d++) {
                    if (ivals[d] == parseInt(ivals[d])) {
                        $(objerror).removeClass("errorValidateTime");
                    }
                    else {
                        isValid = false;
                        //alert($(ValRequiredTime[i]).attr("id"));
                        $(objerror).addClass("errorValidateTime");
                        var messa = ($(messerror).html().trim() == "") ? "" : ($(messerror).html().trim() + "<br />");
                        $(messerror).html(messa + "This field is format Time .Ex: 00:00, 00:00:00 ...");
                    }
                }
            }
        } else {
            $(objerror).removeClass("errorValidateTime");
        }
    }

    return isValid;
}

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

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
/*****************************************************/
/*
    window.location.href returns the href (URL) of the current page
    window.location.hostname returns the domain name of the web host
    window.location.pathname returns the path and filename of the current page
    window.location.protocol returns the web protocol used (http:// or https://)
    window.location.assign loads a new document
*/
var ajaxfunction = 0;
function JOB_AddUrlFunction() {
    /*
    $.ajax({
        url: baseUrl + "/SY_FUNCTION/JOB_AddUrlFunction",
        method: "post",
        data: {
            parturl: window.location.pathname
        }
    });
    */
}

function LoadSecurityUrlFunction() {
    /*
    $.ajax({
        url: baseUrl + "/SY_FUNCTION/SecurityUrlFunction",
        method: "post",
        data: {
            parturl: window.location.pathname
        }
    }).done(function (response) {
        BindControl(response); // LƯU CHUỖI VÀO SESSION
    }).fail(function () {
    });
    ajaxfunction = 1;
    */
}

function getSecurityButton() {
    
    $.ajax({
        url: baseUrl + "/Home/getSecurityButton",
        method: "post",
        data: {
            FUNCTION: window.location.pathname.split('/')[1]
        }
    }).done(function (response) {
         
        if (response != null && response.length > 0) {
            for (var i = 0; i < response.length; i++) {
                 
                if (response[i].BTN_ID != null && response[i].BTN_ID != "") {
                    if (response[i].STATUS) { $("#" + response[i].BTN_ID).show(); }
                    else { $("#" + response[i].BTN_ID).hide(); }
                }
                else if (response[i].BTN_CLASS != null && response[i].BTN_CLASS != "") {
                    if (response[i].STATUS) { $("." + response[i].BTN_CLASS).show(); }
                    else { $("." + response[i].BTN_CLASS).hide(); }
                }
            }
        }

    }).fail(function () {
    });
    ajaxfunction = 1;
    
}

function BindControl(JSecurity) {

    for (i = 0; i < JSecurity.length ; i++) {
        if (JSecurity[i].DATA_KEY.indexOf("id_") == 0) {
            if (JSecurity[i].QUALIFIER == "1") { $("#" + JSecurity[i].DATA_KEY.substring(3)).show(); } else { $("#" + JSecurity[i].DATA_KEY.substring(3)).hide(); }
        }
        if (JSecurity[i].DATA_KEY.indexOf("class_") == 0) {
            if (JSecurity[i].QUALIFIER == "1") { $("." + JSecurity[i].DATA_KEY.substring(6)).show(); } else { $("." + JSecurity[i].DATA_KEY.substring(6)).hide(); }
        }
        if (JSecurity[i].DATA_KEY.indexOf("id_Cus_") == 0) {
            if (JSecurity[i].QUALIFIER == "1") { $("#" + JSecurity[i].DATA_KEY.substring(7)).show(); } else { $("#" + JSecurity[i].DATA_KEY.substring(7)).hide(); }
        }
    }
}

$(document).ajaxComplete(function () {
    if (ajaxfunction != 1) {
        LoadSecurityUrlFunction();
    } else {
        ajaxfunction = 0; //VÌ KHÔNG CHECK QUYỀN KHI GỌI HÀM CLOCK
    }
});

function Security_NotCheck() {
    ajaxfunction = 1;
}

/*****************************************************/
