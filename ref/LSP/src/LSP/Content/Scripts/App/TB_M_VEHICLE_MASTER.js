var _id = -1;
var _searchTimeout;
var _searchXhr;

function Add() {
    OnPopTB_M_VEHICLE_MASTERCloseUp();
    popTB_M_VEHICLE_MASTER.Show();
    return false;
}

function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_M_VEHICLE_MASTER/TB_M_VEHICLE_MASTER_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_M_VEHICLE_MASTER']").html("Edit TB_M_VEHICLE_MASTER");

        _MODEL_ID.SetValue(response.MODEL_ID);
        _CFC.SetText(response.CFC);
        _PROJECT_CODE.SetText(response.PROJECT_CODE);
        _KATASHIKI.SetText(response.KATASHIKI);
        _PROD_SFX.SetText(response.PROD_SFX);
        _MKT_SFX.SetText(response.MKT_SFX);
        _START_LOT.SetText(response.START_LOT);
        _START_PROD_DATE.SetText(response.START_PROD_DATE_Str_DDMMYYYY);
        _END_LOT.SetText(response.END_LOT);
        _END_PROD_DATE.SetText(response.END_PROD_DATE_Str_DDMMYYYY);
        _GRADE_MARK.SetText(response.GRADE_MARK);
        _IS_ACTIVE.SetChecked((response.IS_ACTIVE == "Y") ? 1 : 0);

    }).fail(function () {
        OnPopTB_M_VEHICLE_MASTERCloseUp();
    })
    popTB_M_VEHICLE_MASTER.Show();
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
        url: baseUrl + "/TB_M_VEHICLE_MASTER/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_M_VEHICLE_MASTER.Hide();
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
    popTB_M_VEHICLE_MASTER.Hide()
}

function OnPopTB_M_VEHICLE_MASTERCloseUp() {
    _id = -1
    $("span[id^='popTB_M_VEHICLE_MASTER']").html("Add New TB_M_VEHICLE_MASTER")
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
        url: baseUrl + "/TB_M_VEHICLE_MASTER/Delete",
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
        url: baseUrl + "/TB_M_VEHICLE_MASTER/SetObjectInfo",
        method: "post",
        data: {
            //UserName: _UserName.GetText(),
            //Password: _Password.GetText(),
        }
    }).done(function (response) {
        gvList.PerformCallback();
    });
}

function ClearForm() {
    _MODEL_ID.SetText("");
    _CFC.SetText("");
    _PROJECT_CODE.SetText("");
    _KATASHIKI.SetText("");
    _PROD_SFX.SetText("");
    _MKT_SFX.SetText("");
    _START_LOT.SetText("");
    _START_PROD_DATE.SetText("");
    _END_LOT.SetText("");
    _END_PROD_DATE.SetText("");
    _GRADE_MARK.SetText("");
    _IS_ACTIVE.SetChecked(1);

    validatereset();
}

function ClearSearch() {
    DoSearch();
}

function ActiveForm(active) {
    _MODEL_ID.SetEnabled(active);
    _CFC.SetEnabled(active);
    _PROJECT_CODE.SetEnabled(active);
    _KATASHIKI.SetEnabled(active);
    _PROD_SFX.SetEnabled(active);
    _MKT_SFX.SetEnabled(active);
    _START_LOT.SetEnabled(active);
    _START_PROD_DATE.SetEnabled(active);
    _END_LOT.SetEnabled(active);
    _END_PROD_DATE.SetEnabled(active);
    _GRADE_MARK.SetEnabled(active);
    _IS_ACTIVE.SetEnabled(active);

}

function GetObject() {
    return {
        ID: _id,
        MODEL_ID: _MODEL_ID.GetValue().trim(),
        CFC: _CFC.GetText().trim(),
        PROJECT_CODE: _PROJECT_CODE.GetText().trim(),
        KATASHIKI: _KATASHIKI.GetText().trim(),
        PROD_SFX: _PROD_SFX.GetText().trim(),
        MKT_SFX: _MKT_SFX.GetText().trim(),
        START_LOT: _START_LOT.GetText().trim(),
        START_PROD_DATE: _START_PROD_DATE.GetText().trim(),
        END_LOT: _END_LOT.GetText().trim(),
        END_PROD_DATE: _END_PROD_DATE.GetText().trim(),
        GRADE_MARK:_GRADE_MARK.GetText().trim(),
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
