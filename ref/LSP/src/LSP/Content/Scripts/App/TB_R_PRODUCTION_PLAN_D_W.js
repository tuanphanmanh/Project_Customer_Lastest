var _id = -1;
var _searchTimeout;
var _searchXhr;

function Add() {
    OnPopTB_R_PRODUCTION_PLAN_D_WCloseUp();
    popTB_R_PRODUCTION_PLAN_D_W.Show();
    return false;
}

function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_R_PRODUCTION_PLAN_D_W/TB_R_PRODUCTION_PLAN_D_W_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_R_PRODUCTION_PLAN_D_W']").html("Edit TB_R_PRODUCTION_PLAN_D_W");

        _CFC.SetText(response.CFC);
        _KATASHIKI.SetText(response.KATASHIKI);
        _PROD_SFX.SetText(response.PROD_SFX);
        _LOT_NO.SetText(response.LOT_NO);
        _NO_IN_LOT.SetText(response.NO_IN_LOT);
        _BODY_NO.SetText(response.BODY_NO);
        _EXT_COLOR.SetText(response.EXT_COLOR);
        _VIN_NO.SetText(response.VIN_NO);
        _PRODUCTION_LINE.SetText(response.PRODUCTION_LINE);
        _SHIFT.SetText(response.SHIFT);
        _SEQUENCE_NO.SetText(response.SEQUENCE_NO);
        _WORKING_DATE.SetText(response.WORKING_DATE_Str_DDMMYYYY);
        _NO_IN_DAY.SetText(response.NO_IN_DAY);
        _W_IN_DATE_PLAN.SetText(response.W_IN_DATE_PLAN_Str_DDMMYYYY);

        var d = new Date(new Date().toDateString() + ' ' + jsonTimeToString(response.W_IN_TIME_PLAN));
        _W_IN_TIME_PLAN.SetValue(d);

        _W_IN_DATE_ACTUAL.SetText(response.W_IN_DATE_ACTUAL_Str_DDMMYYYY);

        var d = new Date(new Date().toDateString() + ' ' + jsonTimeToString(response.W_IN_TIME_ACTUAL));
        _W_IN_TIME_ACTUAL.SetValue(d);

        _W_OUT_DATE_PLAN.SetText(response.W_OUT_DATE_PLAN_Str_DDMMYYYY);

        var d = new Date(new Date().toDateString() + ' ' + jsonTimeToString(response.W_OUT_TIME_PLAN));
        _W_OUT_TIME_PLAN.SetValue(d);

        _W_OUT_DATE_ACTUAL.SetText(response.W_OUT_DATE_ACTUAL_Str_DDMMYYYY);

        var d = new Date(new Date().toDateString() + ' ' + jsonTimeToString(response.W_OUT_TIME_ACTUAL));
        _W_OUT_TIME_ACTUAL.SetValue(d);

        _VERSION_NO.SetText(response.VERSION_NO);
        _IS_NQC_PROCESSED.SetText(response.IS_NQC_PROCESSED);
        _IS_ACTIVE.SetChecked((response.IS_ACTIVE == "Y") ? 1 : 0);

    }).fail(function () {
        OnPopTB_R_PRODUCTION_PLAN_D_WCloseUp();
    })
    popTB_R_PRODUCTION_PLAN_D_W.Show();
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
    if (!Validate()) {
        return;
    }
    ActiveForm(false);
    $btn.addClass("saving");
    $.ajax({
        url: baseUrl + "/TB_R_PRODUCTION_PLAN_D_W/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_R_PRODUCTION_PLAN_D_W.Hide();
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
    popTB_R_PRODUCTION_PLAN_D_W.Hide()
}

function OnPopTB_R_PRODUCTION_PLAN_D_WCloseUp() {
    _id = -1
    $("span[id^='popTB_R_PRODUCTION_PLAN_D_W']").html("Add New TB_R_PRODUCTION_PLAN_D_W")
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
        url: baseUrl + "/TB_R_PRODUCTION_PLAN_D_W/Delete",
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
        url: baseUrl + "/TB_R_PRODUCTION_PLAN_D_W/SetObjectInfo",
        method: "post",
        data: {            
            CFC: _txtCFC.GetText(),
            PROD_SFX: _txtPROD_SFX.GetText(),
            PRODUCTION_LINE: _txtPRODUCTION_LINE.GetText(),
            WORKING_DATE: _txtWORKING_DATE.GetText(),
            W_IN_DATE_PLAN: _txtW_IN_DATE_PLAN.GetText(),
            PRODUCTION_MONTH: _txtPRODUCTION_MONTH.GetText().trim()
        }
    }).done(function (response) {
        gvList.PerformCallback();
    });
}

function ClearForm() {
    _CFC.SetText("");
    _KATASHIKI.SetText("");
    _PROD_SFX.SetText("");
    _LOT_NO.SetText("");
    _NO_IN_LOT.SetText("");
    _BODY_NO.SetText("");
    _EXT_COLOR.SetText("");
    _VIN_NO.SetText("");
    _PRODUCTION_LINE.SetText("");
    _SHIFT.SetText("");
    _SEQUENCE_NO.SetText("");
    _WORKING_DATE.SetText("");
    _NO_IN_DAY.SetText("");
    _W_IN_DATE_PLAN.SetText("");
    _W_IN_TIME_PLAN.SetText("");
    _W_IN_DATE_ACTUAL.SetText("");
    _W_IN_TIME_ACTUAL.SetText("");
    _W_OUT_DATE_PLAN.SetText("");
    _W_OUT_TIME_PLAN.SetText("");
    _W_OUT_DATE_ACTUAL.SetText("");
    _W_OUT_TIME_ACTUAL.SetText("");
    _VERSION_NO.SetText("");
    _IS_NQC_PROCESSED.SetText("");
    _IS_ACTIVE.SetChecked(1);
    validatereset();
}

function ClearSearch() {
    DoSearch();
}

function ActiveForm(active) {

    _CFC.SetEnabled(active);
    _KATASHIKI.SetEnabled(active);
    _PROD_SFX.SetEnabled(active);
    _LOT_NO.SetEnabled(active);
    _NO_IN_LOT.SetEnabled(active);
    _BODY_NO.SetEnabled(active);
    _EXT_COLOR.SetEnabled(active);
    _VIN_NO.SetEnabled(active);
    _PRODUCTION_LINE.SetEnabled(active);
    _SHIFT.SetEnabled(active);
    _SEQUENCE_NO.SetEnabled(active);
    _WORKING_DATE.SetEnabled(active);
    _NO_IN_DAY.SetEnabled(active);
    _W_IN_DATE_PLAN.SetEnabled(active);
    _W_IN_TIME_PLAN.SetEnabled(active);
    _W_IN_DATE_ACTUAL.SetEnabled(active);
    _W_IN_TIME_ACTUAL.SetEnabled(active);
    _W_OUT_DATE_PLAN.SetEnabled(active);
    _W_OUT_TIME_PLAN.SetEnabled(active);
    _W_OUT_DATE_ACTUAL.SetEnabled(active);
    _W_OUT_TIME_ACTUAL.SetEnabled(active);
    _VERSION_NO.SetEnabled(active);
    _IS_NQC_PROCESSED.SetEnabled(active);
    _IS_ACTIVE.SetEnabled(active);
}

function GetObject() {
    return {
        id: _id,
        CFC: _CFC.GetText().trim(),
        KATASHIKI: _KATASHIKI.GetText().trim(),
        PROD_SFX: _PROD_SFX.GetText().trim(),
        LOT_NO: _LOT_NO.GetText().trim(),
        NO_IN_LOT: _NO_IN_LOT.GetText().trim(),
        BODY_NO: _BODY_NO.GetText().trim(),
        EXT_COLOR: _EXT_COLOR.GetText().trim(),
        VIN_NO: _VIN_NO.GetText().trim(),
        PRODUCTION_LINE: _PRODUCTION_LINE.GetText().trim(),
        SHIFT: _SHIFT.GetText().trim(),
        SEQUENCE_NO: _SEQUENCE_NO.GetText().trim(),
        WORKING_DATE: ConvertDate(_WORKING_DATE.GetText().trim()),
        NO_IN_DAY: _NO_IN_DAY.GetText().trim(),
        W_IN_DATE_PLAN: ConvertDate(_W_IN_DATE_PLAN.GetText().trim()),
        W_IN_TIME_PLAN: _W_IN_TIME_PLAN.GetText().trim(),
        W_IN_DATE_ACTUAL: ConvertDate(_W_IN_DATE_ACTUAL.GetText().trim()),
        W_IN_TIME_ACTUAL: _W_IN_TIME_ACTUAL.GetText().trim(),
        W_OUT_DATE_PLAN: ConvertDate(_W_OUT_DATE_PLAN.GetText().trim()),
        W_OUT_TIME_PLAN: _W_OUT_TIME_PLAN.GetText().trim(),
        W_OUT_DATE_ACTUAL: ConvertDate(_W_OUT_DATE_ACTUAL.GetText().trim()),
        W_OUT_TIME_ACTUAL: _W_OUT_TIME_ACTUAL.GetText().trim(),
        VERSION_NO: _VERSION_NO.GetText().trim(),
        IS_NQC_PROCESSED: _IS_NQC_PROCESSED.GetText().trim(),
        IS_ACTIVE: (_IS_ACTIVE.GetChecked()) ? "Y" : "N"
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
