var _id = -1;
var _searchTimeout;
var _searchXhr;

function Add() {
    OnPopTB_R_UP_PLAN_HCloseUp();
    popTB_R_UP_PLAN_H.Show();
    return false;
}

function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_R_UP_PLAN_H/TB_R_UP_PLAN_H_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_R_UP_PLAN_H']").html("Edit TB_R_UP_PLAN_H");

        _ORDER_NO.SetText(response.ORDER_NO);
        _LINE.SetText(response.LINE);
        _CASE_NO.SetText(response.CASE_NO);
        _SUPPLIER_CODE.SetText(response.SUPPLIER_CODE);

        var d = new Date(new Date().toDateString() + ' ' + jsonTimeToString(response.UNPACKING_TIME));
        _UNPACKING_TIME.SetValue(d);

        _UNPACKING_DATE.SetText(response.UNPACKING_DATE_Str_DDMMYYYY);
        _NO_IN_DATE.SetText(response.NO_IN_DATE);
        _WORKING_DATE.SetText(response.WORKING_DATE_Str_DDMMYYYY);
        _SHIFT.SetText(response.SHIFT);
        _INCOMP_REASON.SetText(response.INCOMP_REASON);
        _UP_STATUS.SetText(response.UP_STATUS);
        _IS_ACTIVE.SetChecked((response.IS_ACTIVE == "Y") ? 1 : 0);
        _IS_CURRENT.SetChecked((response.IS_ACTIVE == "Y") ? 1 : 0);

    }).fail(function () {
        OnPopTB_R_UP_PLAN_HCloseUp();
    })
    popTB_R_UP_PLAN_H.Show();
    return false;
}

function jsonTimeToString(dt) {
    if (dt != null) {
        var strtime = (((dt.Hours + "").length == 1) ? ("0" + dt.Hours) : dt.Hours) + ":" +
                            (((dt.Minutes + "").length == 1) ? ("0" + dt.Minutes) : dt.Minutes) + ":" +
                            (((dt.Seconds + "").length == 1) ? ("0" + dt.Seconds) : dt.Seconds)
        return strtime;
    } else { return ""; }
}


function getTime(dt) {
    var strtime = (((dt.getHours() + "").length == 1) ? ("0" + dt.getHours()) : dt.getHours()) + " : " +
                    (((dt.getMinutes() + "").length == 1) ? ("0" + dt.getMinutes()) : dt.getMinutes())
    return strtime;
}

function OnBtnUpdateClicked(s) {
    var $btn = $(s);
    if ($btn.hasClass("saving")) {
        return;
    }
    if (!doValidateParam("PLAN_H")) {
        return;
    }
    ActiveForm(false);
    $btn.addClass("saving");
    $.ajax({
        url: baseUrl + "/TB_R_UP_PLAN_H/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_R_UP_PLAN_H.Hide();
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
    popTB_R_UP_PLAN_H.Hide()
}

function OnPopTB_R_UP_PLAN_HCloseUp() {
    _id = -1
    $("span[id^='popTB_R_UP_PLAN_H']").html("Add New TB_R_UP_PLAN_H")
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
        url: baseUrl + "/TB_R_UP_PLAN_H/Delete",
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
        url: baseUrl + "/TB_R_UP_PLAN_H/SetObjectInfo",
        method: "post",
        data: {
            SUPPLIER_CODE: _txtSUPPLIER_CODE.GetText(),
            WORKING_DATE: _txtWORKING_DATE.GetText(),
        }
    }).done(function (response) {
        gvList.PerformCallback();
    });
}

function ClearForm() {
    _ORDER_NO.SetText("");
    _LINE.SetText("");
    _CASE_NO.SetText("");
    _SUPPLIER_CODE.SetText("");
    _UNPACKING_TIME.SetText("");
    _UNPACKING_DATE.SetText("");
    _NO_IN_DATE.SetText("");
    _WORKING_DATE.SetText("");
    _SHIFT.SetText("");
    _INCOMP_REASON.SetText("");
    _UP_STATUS.SetText("");
    _IS_ACTIVE.SetChecked(1);
    _IS_CURRENT.SetChecked(0);

    validatereset();
}

function ClearSearch() {
    DoSearch();
}

function ActiveForm(active) {
    _ORDER_NO.SetEnabled(active);
    _LINE.SetEnabled(active);
    _CASE_NO.SetEnabled(active);
    _SUPPLIER_CODE.SetEnabled(active);
    _UNPACKING_TIME.SetEnabled(active);
    _UNPACKING_DATE.SetEnabled(active);
    _NO_IN_DATE.SetEnabled(active);
    _WORKING_DATE.SetEnabled(active);
    _SHIFT.SetEnabled(active);
    _INCOMP_REASON.SetEnabled(active);
    _UP_STATUS.SetEnabled(active);
    _IS_ACTIVE.SetEnabled(active);
    _IS_CURRENT.SetEnabled(active);

}

function GetObject() {
    return {
        ID: _id,
        ORDER_NO: _ORDER_NO.GetText().trim(),
        LINE: _LINE.GetText().trim(),
        CASE_NO: _CASE_NO.GetText().trim(),
        SUPPLIER_CODE: _SUPPLIER_CODE.GetText().trim(),
        UNPACKING_TIME: _UNPACKING_TIME.GetText().trim(),
        UNPACKING_DATE: _UNPACKING_DATE.GetText().trim(),
        NO_IN_DATE: _NO_IN_DATE.GetText().trim(),
        WORKING_DATE: ConvertDate(_WORKING_DATE.GetText().trim()),
        SHIFT: _SHIFT.GetText().trim(),
        INCOMP_REASON: _INCOMP_REASON.GetText().trim(),
        UP_STATUS: _UP_STATUS.GetText().trim(),
        IS_ACTIVE: (_IS_ACTIVE.GetChecked()) ? "Y" : "N",
        IS_CURRENT: (_IS_CURRENT.GetChecked()) ? "Y" : "N"

    }
}

function Validate() {
    return doValidate();
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

//***************************************PLAN D

var _PLAN_D_ID = 0;
var _PLAN_H_ID = 0;

function PLAN_DNew(t) {
    OnPopPLAN_DCloseUp();

    _PLAN_H_ID = $(t).attr("data-id");
    popPLAN_D.Show();

}

function OnPopPLAN_DCloseUp() {
    _PLAN_D_ID = 0;
    _PLAN_H_ID = 0;

    $("span[id^='popPLAN_D']").html("Add New Plan D");
    ClearFormPLAN_D();
}

function ClearFormPLAN_D() {

    _D_LINE.SetText("");
    _NO.SetText("");
    _BACK_NO.SetText("");
    _D_CASE_NO.SetText("");
    _SUPPLIER_NO.SetText("");
    _MODEL.SetText("");
    _PART_NO.SetText("");
    _PART_NAME.SetText("");
    _PC_ADDRESS.SetText("");
    _QTY.SetText("");
    _BOX_SIZE.SetText("");
    _QTY_BOX.SetText("");
    _QTY_ACT.SetText("");
    _PXP_LOCATION.SetText("");
    _D_WORKING_DATE.SetText("");
    _D_SHIFT.SetText("");
    _D_UP_STATUS.SetText("");
    _D_INCOMP_REASON.SetText("");

    _D_IS_ACTIVE.SetChecked(1);
    _IS_OVER.SetChecked(0);

    validatereset();
}

function OnBtnPLAN_DCancelClicked() {
    popPLAN_D.Hide()
}

function OnBtnPLAN_DUpdateClicked(s) {
    var $btn = $(s);
    if ($btn.hasClass("saving")) {
        return;
    }
    if (!doValidateParam("PLAN_D")) {
        return;
    }
    ActiveFormPLAN_D(false);
    $btn.addClass("saving");
    $.ajax({
        url: baseUrl + "/TB_R_UP_PLAN_D/SaveData",
        method: "post",
        data: GetObjectPLAN_D(),
        dataType: "json"
    }).done(function (response) {
        ActiveFormPLAN_D(true);
        if (response.success) {
            popPLAN_D.Hide();
            gvList.PerformCallback();
        } else {
            alert(response.message);
        }
    }).always(function () {
        $btn.removeClass("saving");
    });
}

function GetObjectPLAN_D() {
    return {
        ID: _PLAN_D_ID,
        UP_PLAN_H_ID: _PLAN_H_ID,
        LINE: _D_LINE.GetText().trim(),
        NO: _NO.GetText().trim(),
        BACK_NO: _BACK_NO.GetText().trim(),
        CASE_NO: _D_CASE_NO.GetText().trim(),
        SUPPLIER_NO: _SUPPLIER_NO.GetText().trim(),
        MODEL: _MODEL.GetText().trim(),
        PART_NO: _PART_NO.GetText().trim(),
        PART_NAME: _PART_NAME.GetText().trim(),
        PC_ADDRESS: _PC_ADDRESS.GetText().trim(),
        QTY: _QTY.GetText().trim(),
        BOX_SIZE: _BOX_SIZE.GetText().trim(),
        QTY_BOX: _QTY_BOX.GetText().trim(),
        QTY_ACT: _QTY_ACT.GetText().trim(),
        PXP_LOCATION: _PXP_LOCATION.GetText().trim(),
        WORKING_DATE: ConvertDate(_D_WORKING_DATE.GetText().trim()),
        SHIFT: _D_SHIFT.GetText().trim(),
        UP_STATUS: _D_UP_STATUS.GetText().trim(),
        INCOMP_REASON: _D_INCOMP_REASON.GetText().trim(),
        IS_ACTIVE: (_D_IS_ACTIVE.GetChecked()) ? "Y" : "N",
        IS_OVER: (_IS_OVER.GetChecked()) ? "Y" : "N"

    }
}

function ActiveFormPLAN_D(active) {

    _D_LINE.SetEnabled(active);
    _NO.SetEnabled(active);
    _BACK_NO.SetEnabled(active);
    _D_CASE_NO.SetEnabled(active);
    _SUPPLIER_NO.SetEnabled(active);
    _MODEL.SetEnabled(active);
    _PART_NO.SetEnabled(active);
    _PART_NAME.SetEnabled(active);
    _PC_ADDRESS.SetEnabled(active);
    _QTY.SetEnabled(active);
    _BOX_SIZE.SetEnabled(active);
    _QTY_BOX.SetEnabled(active);
    _QTY_ACT.SetEnabled(active);
    _PXP_LOCATION.SetEnabled(active);
    _D_WORKING_DATE.SetEnabled(active);
    _D_SHIFT.SetEnabled(active);
    _D_UP_STATUS.SetEnabled(active);
    _D_INCOMP_REASON.SetEnabled(active);
    _D_IS_ACTIVE.SetEnabled(active);
    _IS_OVER.SetEnabled(active);

}

function PLAN_DDelete(s) {
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
        url: baseUrl + "/TB_R_UP_PLAN_D/Delete",
        method: "post",
        dataType: "json",
        data: {
            ID: $btn.attr("data-id")
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

function PLAN_DEdit(t) {

    OnPopPLAN_DCloseUp();

    _PLAN_D_ID = $(t).attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_R_UP_PLAN_D/TB_R_UP_PLAN_D_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _PLAN_D_ID
        }
    }).done(function (response) {

        _D_LINE.SetText(response.LINE);
        _NO.SetText(response.NO);
        _BACK_NO.SetText(response.BACK_NO);
        _D_CASE_NO.SetText(response.CASE_NO);
        _SUPPLIER_NO.SetText(response.SUPPLIER_NO);
        _MODEL.SetText(response.MODEL);
        _PART_NO.SetText(response.PART_NO);
        _PART_NAME.SetText(response.PART_NAME);
        _PC_ADDRESS.SetText(response.PC_ADDRESS);
        _QTY.SetText(response.QTY);
        _BOX_SIZE.SetText(response.BOX_SIZE);
        _QTY_BOX.SetText(response.QTY_BOX);
        _QTY_ACT.SetText(response.QTY_ACT);
        _PXP_LOCATION.SetText(response.PXP_LOCATION);
        _D_WORKING_DATE.SetText(response.WORKING_DATE_Str_DDMMYYYY);
        _D_SHIFT.SetText(response.SHIFT);
        _D_UP_STATUS.SetText(response.UP_STATUS);
        _D_INCOMP_REASON.SetText(response.INCOMP_REASON);
        _IS_ACTIVE.SetChecked((response._D_IS_ACTIVE == "Y") ? 1 : 0);
        _IS_OVER.SetChecked((response.IS_ACTIVE == "Y") ? 1 : 0);

        popPLAN_D.Show();
    });
    return false;
}

