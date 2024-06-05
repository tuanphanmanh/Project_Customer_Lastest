var _id = -1;
var _searchTimeout;
var _searchXhr;

function Add() {
    OnPopTB_R_TRUCK_BOOKING_HCloseUp();

    //Reset session for UNLOADING_PLAN_H_SetID
    $.ajax({
        url: baseUrl + "/TB_R_TRUCK_BOOKING_H/UNLOADING_PLAN_H_SetID",
        method: "post",
        data: {
            ID: ""
        }
    }).done(function (response) {
        _UNLOADING_PLAN_H_ID.PerformCallback();
        popTB_R_TRUCK_BOOKING_H.Show();
    });
   
    return false;
}

function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_R_TRUCK_BOOKING_H/TB_R_TRUCK_BOOKING_H_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_R_TRUCK_BOOKING_H']").html("Edit TRUCK BOOKING");

        //_UNLOADING_PLAN_H_ID.SetValue(response.UNLOADING_PLAN_H_ID);        
        _PATH.SetText(response.PATH);
        _TRANSPORTER_ABBR.SetText(response.TRANSPORTER_ABBR);
        _TRUCK_TYPE.SetText(response.TRUCK_TYPE);
        _IS_ACTIVE.SetChecked((response.IS_ACTIVE == "Y") ? 1 : 0);

        //_UNLOADING_PLAN_H_ID.SetEnabled(false);

    }).fail(function () {
        OnPopTB_R_TRUCK_BOOKING_HCloseUp();
    })
    _UNLOADING_PLAN_H_ID.PerformCallback();
    popTB_R_TRUCK_BOOKING_H.Show();
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
        url: baseUrl + "/TB_R_TRUCK_BOOKING_H/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_R_TRUCK_BOOKING_H.Hide();
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
    popTB_R_TRUCK_BOOKING_H.Hide()
}

function OnPopTB_R_TRUCK_BOOKING_HCloseUp() {
    _id = -1
    $("span[id^='popTB_R_TRUCK_BOOKING_H']").html("Add TRUCK BOOKING")
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
        url: baseUrl + "/TB_R_TRUCK_BOOKING_H/Delete",
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
    _searchXhr = $.ajax({
        url: baseUrl + "/TB_R_TRUCK_BOOKING_H/SetObjectInfo",
        method: "post",
        data: {
            TRUCK: _txtTRUCK.GetText(),
            SUPPLIERS: _txtSUPPLIERS.GetText(),
            TRANSPORTER_ABBR: _txtTRANSPORTER_ABBR.GetText(),
            IS_ACTIVE: _txtIS_ACTIVE.GetValue()
        }
    }).done(function (response) {
        gvList.PerformCallback();
    });
}

function ClearForm() {        
    //_UNLOADING_PLAN_H_ID.SelectIndex(0);
    _PATH.SetText("");
    _TRANSPORTER_ABBR.SetText("");
    _TRUCK_TYPE.SetText("");
    _IS_ACTIVE.SetChecked(1);

    validatereset();
}

function ClearSearch() {
    DoSearch();
}

function ActiveForm(active) {
    //_UNLOADING_PLAN_H_ID.SetEnabled(active);
    _PATH.SetEnabled(active);
    _TRANSPORTER_ABBR.SetEnabled(active);
    _TRUCK_TYPE.SetEnabled(active);
    _IS_ACTIVE.SetEnabled(active);

}

function GetObject() {
    return {
        id: _id,
        UNLOADING_PLAN_H_ID: _UNLOADING_PLAN_H_ID.GetValue(),
        PATH: _PATH.GetText().trim(),
        TRANSPORTER_ABBR: _TRANSPORTER_ABBR.GetText().trim(),
        TRUCK_TYPE: _TRUCK_TYPE.GetText().trim(),        
        IS_ACTIVE: (_IS_ACTIVE.GetChecked()) ? "Y" : "N" 
    }
}

function Validate() {
    return doValidate();
}

/***************************** BOOKING DETAILS ***********************************************/
var _BOOKING_H_ID = 0;
var _ID = 0;

function BOOKING_DETAILS_New(t) {
    OnPopTB_R_TRUCK_BOOKING_DCloseUp();
    //Reset session for TB_M_SUPPLIER_OR_TIME ID
    $.ajax({
        url: baseUrl + "/TB_R_TRUCK_BOOKING_H/SUPPLIER_OR_TIME_SetID",
        method: "post",
        data: {
            ID: ""
        }
    }).done(function (response) {
        _BOOKING_H_ID = $(t).attr("data-id");
        _TB_M_SUPPLIER_OR_TIME.PerformCallback();
        popTB_R_TRUCK_BOOKING_DETAILS.Show();
    });    
}

function OnPopTB_R_TRUCK_BOOKING_DCloseUp() {
    _BOOKING_H_ID = 0;
    _ID = 0;
    $("span[id^='popTB_R_TRUCK_BOOKING_DETAILS']").html("Add new TRUCK BOOKING DETAILS");
    ClearFormBOOKING_D();
}

function ClearFormBOOKING_D() {

    _BOOKING_H_ID = 0;
    _ID = 0;
    _IS_ACTIVE_DETAIL.SetChecked(1);
    validatereset();
}


function OnBtnCancelDetailsClicked() {
    popTB_R_TRUCK_BOOKING_DETAILS.Hide()
}

function OnBtnUpdateDetailsClicked(s) {
    var $btn = $(s);
    if ($btn.hasClass("saving")) {
        return;
    }
    if (!doValidateParam("BOOKING_DETAIL")) {
        return;
    }
    ActiveForm_BOOKING_D(false);
    $btn.addClass("saving");
    $.ajax({
        url: baseUrl + "/TB_R_TRUCK_BOOKING_D/SaveData",
        method: "post",
        data: GetObject_BOOKING_D(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm_BOOKING_D(true);
        if (response.success) {
            popTB_R_TRUCK_BOOKING_DETAILS.Hide();
            gvList.PerformCallback();
        } else {
            alert(response.message);
        }
    }).always(function () {
        $btn.removeClass("saving");
    });
}

function ActiveForm_BOOKING_D(active) {   
    _IS_ACTIVE_DETAIL.SetEnabled(active);
}

function GetObject_BOOKING_D() {
    return {
        ID: _ID,
        BOOKING_H_ID: _BOOKING_H_ID,
        SUPPLIER_OR_TIME_ID: _TB_M_SUPPLIER_OR_TIME.GetValue(),                                
        IS_ACTIVE: (_IS_ACTIVE_DETAIL.GetChecked()) ? "Y" : "N"
    }
}


function BOOKING_DDelete(s) {
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
        url: baseUrl + "/TB_R_TRUCK_BOOKING_D/Delete",
        method: "post",
        dataType: "json",
        data: {
            sid: $btn.attr("data-id")
        }
    }).done(function (response) {
        if (response.success) {
            gvList.PerformCallback();
        } else {
            alert(response.message);
        }
    }).always(function () {
        $btn.html("Delete");
        $btn.parent().removeClass("processing");
    });
    return false;
}


function BOOKING_DEdit(t) {

    OnPopTB_R_TRUCK_BOOKING_DCloseUp();

    _ID = $(t).attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_R_TRUCK_BOOKING_D/TB_R_TRUCK_BOOKING_D_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _ID
        }
    }).done(function (response) {
        
        _IS_ACTIVE_DETAIL.SetChecked((response.IS_ACTIVE == "Y") ? 1 : 0);
        
        _TB_M_SUPPLIER_OR_TIME.PerformCallback();
        popTB_R_TRUCK_BOOKING_DETAILS.Show();
    }).fail(function () {
        OnPopTB_R_TRUCK_BOOKING_DCloseUp();
    });
    return false;
}