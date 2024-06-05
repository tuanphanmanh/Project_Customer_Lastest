var _id = -1;
var _searchTimeout;
var _searchXhr;

function Add() {
    OnPopTB_R_TRUCK_BOOKING_DCloseUp();
    popTB_R_TRUCK_BOOKING_D.Show();
    return false;
}

function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_R_TRUCK_BOOKING_D/TB_R_TRUCK_BOOKING_D_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_R_TRUCK_BOOKING_D']").html("Edit TB_R_TRUCK_BOOKING_D");

        _BOOKING_H_ID.SetText(response.BOOKING_H_ID);
        _SUPPLIER_CODE.SetText(response.SUPPLIER_CODE);
        _ORDER_NO.SetText(response.ORDER_NO);

    }).fail(function () {
        OnPopTB_R_TRUCK_BOOKING_DCloseUp();
    })
    popTB_R_TRUCK_BOOKING_D.Show();
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
        url: baseUrl + "/TB_R_TRUCK_BOOKING_D/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_R_TRUCK_BOOKING_D.Hide();
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
    popTB_R_TRUCK_BOOKING_D.Hide()
}

function OnPopTB_R_TRUCK_BOOKING_DCloseUp() {
    _id = -1
    $("span[id^='popTB_R_TRUCK_BOOKING_D']").html("Add New TB_R_TRUCK_BOOKING_D")
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
        url: baseUrl + "/TB_R_TRUCK_BOOKING_D/Delete",
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
        url: baseUrl + "/TB_R_TRUCK_BOOKING_D/SetObjectInfo",
        method: "post",
        data: {
            BOOKING_H_ID: _txtBOOKING_H_ID.GetText(),
            SUPPLIER_CODE: _txtSUPPLIER_CODE.GetText(),
            ORDER_NO: _txtORDER_NO.GetText(),
        }
    }).done(function (response) {
        gvList.PerformCallback();
    });
}

function ClearForm() {
    _BOOKING_H_ID.SetText("");
    _SUPPLIER_CODE.SetText("");
    _ORDER_NO.SetText("");

    validatereset();
}

function ClearSearch() {
    DoSearch();
}

function ActiveForm(active) {

    _BOOKING_H_ID.SetEnabled(active);
    _SUPPLIER_CODE.SetEnabled(active);
    _ORDER_NO.SetEnabled(active);

}

function GetObject() {
    return {
        id: _id,
        BOOKING_H_ID: _BOOKING_H_ID.GetText().trim(),
        SUPPLIER_CODE: _SUPPLIER_CODE.GetText().trim(),
        ORDER_NO: _ORDER_NO.GetText().trim(),
    }
}

function Validate() {
    return doValidate();
}