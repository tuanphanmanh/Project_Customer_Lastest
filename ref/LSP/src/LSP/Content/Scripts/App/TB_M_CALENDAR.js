var _id = -1;
var _searchTimeout;
var _searchXhr;

function Add() {
    OnPopTB_M_CALENDARCloseUp();
    popTB_M_CALENDAR.Show();
    return false;
}

function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_M_CALENDAR/TB_M_CALENDAR_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_M_CALENDAR']").html("Edit TB_M_CALENDAR");

        _SUPPLIER_CODE.SetText(response.SUPPLIER_CODE);
        _WORKING_DATE.SetText(response.WORKING_DATE_Str_DDMMYYYY);
        _WORKING_TYPE.SetText(response.WORKING_TYPE);
        _WORKING_STATUS.SetText(response.WORKING_STATUS);
        _IS_ACTIVE.SetChecked((response.IS_ACTIVE == "Y") ? 1 : 0);

    }).fail(function () {
        OnPopTB_M_CALENDARCloseUp();
    })
    popTB_M_CALENDAR.Show();
    return false;
}

function OnBtnUpdateClicked(s) {
    var $btn = $(s);
    if ($btn.hasClass("saving")) {
        return;
    }

    if (!Validate()) {
        return;
    }
    ActiveForm(false);
    $btn.addClass("saving");
    $.ajax({
        url: baseUrl + "/TB_M_CALENDAR/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_M_CALENDAR.Hide();
            DoSearch();
        }
        else {
            alert(response.message);
        }
    }).always(function () {
        $btn.removeClass("saving");
    });
}

function OnBtnCancelClicked() {
    popTB_M_CALENDAR.Hide()
}

function OnPopTB_M_CALENDARCloseUp() {
    _id = -1
    $("span[id^='popTB_M_CALENDAR']").html("Add New TB_M_CALENDAR")
    ClearForm();
}

function Delete(s) {
    var $btn = $(s)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    if (!confirm("Are you sure to delete selected record?")) {
        return false;
    }
    $btn.parent().addClass("processing");
    $btn.html("Deleting...");
    $.ajax({
        url: baseUrl + "/TB_M_CALENDAR/Delete",
        method: "post",
        dataType: "json",
        data: {
            sid: $btn.attr("data-id")
        }
    }).done(function (response) {
        if (response.success) {
            DoSearch();
        }
        else {
            alert(response.message);
        }
    }).always(function () {
        $btn.html("Delete");
        $btn.parent().removeClass("processing");
    });
    return false;
}

function DoSearch() {
    if (_searchTimeout) {
        clearTimeout(_searchTimeout)
    }
    if (_searchXhr) {
        _searchXhr.abort()
    }

    _searchTimeout = setTimeout("Search()", 100)
}

//Editer
function Search() {
    $.ajax({
        url: baseUrl + "/TB_M_CALENDAR/SetObjectInfo",
        method: "post",
        data: {
            SUPPLIER_CODE: _txtSUPPLIER_CODE.GetText(),
            WORKING_DATE: _txtWORKING_DATE.GetText()
        } 
    }).done(function (as) { 
        gvList.PerformCallback();
    });
}

function ClearForm() {
    _SUPPLIER_CODE.SetText("");
    _WORKING_DATE.SetText("");
    _WORKING_TYPE.SetText("");
    _WORKING_STATUS.SetText("");
    _IS_ACTIVE.SetChecked(1);
    validatereset();
}

function ActiveForm(active) {
    _SUPPLIER_CODE.SetEnabled(active);
    _WORKING_DATE.SetEnabled(active);
    _WORKING_TYPE.SetEnabled(active);
    _WORKING_STATUS.SetEnabled(active);
    _IS_ACTIVE.SetEnabled(active);
}

function GetObject() {
    return {
        ID: _id,
        SUPPLIER_CODE: _SUPPLIER_CODE.GetText().trim(),
        WORKING_DATE: ConvertDate(_WORKING_DATE.GetText().trim()),
        WORKING_TYPE: _WORKING_TYPE.GetText().trim(),
        WORKING_STATUS: _WORKING_STATUS.GetText().trim(),
        IS_ACTIVE: (_IS_ACTIVE.GetChecked()) ? "Y" : "N"
    }
}

function Validate() {
    return true;
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

/*IMPORT*/
function IMPORT_CALENDAR() {
    OnPopCALENDARCloseUp();
    popCALENDAR.Show();

    return false;
}

function OnPopCALENDARCloseUp() {
    ClearFormCALENDAR();
}

function ClearFormCALENDAR() {
    //aspxUClearFileInputClick('_IMPORT_CALENDAR', 0);
}

function CALENDAR_OnFileUploadComplete(s, e) {
    alert(e.callbackData);
    if (e.isValid) {
        popCALENDAR.Hide();
        gvList.PerformCallback();
    }
}

function OnBtnImportCancelClicked() {
    popCALENDAR.Hide()
}

/***************DOWNLOAD*******************/

function DOWNLOAD_CALENDAR_TEMPLATE() {

    window.open("/Content/Template/CALENDAR - INFO.xls");
}


/******************************* DETAIL *********************************/
 
function showdetail() {
     
    //SelectCFC();
    popDetails.Show();
}

function OnpopDetailsCloseUp() {

}

function SelectWORKING_MONTH() {

    $.ajax({
        url: baseUrl + "/TB_M_CALENDAR/TB_M_CALENDAR_SetDetails",
        method: "get",
        //dataType: "json",
        data: {
            WORKING_MONTH: _txtWORKING_MONTH.GetText().trim()
        }
    }).done(function (response) { 
        gvDetailsList.PerformCallback();
    });
}
 

/******************************* DETAIL ORDER - RECEIVE *********************************/

function showDetailsOrder() {
    
    popDetailsOrder.Show();
}

function OnpopDetailsOrderCloseUp() {

}

function SelectWORKING_MONTH_ORDER() {

    $.ajax({
        url: baseUrl + "/TB_M_CALENDAR/TB_M_CALENDAR_SetDetailsOrder",
        method: "get",        
        data: {
            WORKING_MONTH: _txtWORKING_MONTH_ORDER.GetText().trim()
        }
    }).done(function (response) {
        gvDetailsOrderList.PerformCallback();
    });
}


/******************** SCHEDULER *******************/
/*
function showCalendarScheduler() {

    popTB_M_CALENDAR_SCHEDULER.Show();
    //scheduler.PerformCallback();
}*/
