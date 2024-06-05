var _id = -1;
var _searchTimeout;
var _searchXhr;

function Add() {
    OnPopTB_M_TRUCK_TYPECloseUp();
    popTB_M_TRUCK_TYPE.Show();
    return false;
}

function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_M_TRUCK_TYPE/TB_M_TRUCK_TYPE_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_M_TRUCK_TYPE']").html("Edit TB_M_TRUCK_TYPE");

        _NAME.SetText(response.NAME);
        _WEIGHT.SetText(response.WEIGHT);
        _COST.SetText(response.COST);
        _OT.SetText(response.OT);
        _IS_ACTIVE.SetChecked((response.IS_ACTIVE == "Y") ? 1 : 0);

    }).fail(function () {
        OnPopTB_M_TRUCK_TYPECloseUp();
    })
    popTB_M_TRUCK_TYPE.Show();
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
        url: baseUrl + "/TB_M_TRUCK_TYPE/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_M_TRUCK_TYPE.Hide();
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
    popTB_M_TRUCK_TYPE.Hide()
}

function OnPopTB_M_TRUCK_TYPECloseUp() {
    _id = -1
    $("span[id^='popTB_M_TRUCK_TYPE']").html("Add New TB_M_TRUCK_TYPE")
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
        url: baseUrl + "/TB_M_TRUCK_TYPE/Delete",
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
    url: baseUrl + "/TB_M_TRUCK_TYPE/SetObjectInfo",
    method: "post",
    data: {
        NAME: _txtNAME.GetText(),
        WEIGHT: _txtWEIGHT.GetText(),
        COST: _txtCOST.GetText()
    }
    }).done(function (response) {
        gvList.PerformCallback();
    });
}

function ClearForm() {
    _NAME.SetText("");
    _WEIGHT.SetText("");
    _COST.SetText("");
    _OT.SetText("");
    _IS_ACTIVE.SetChecked(1);
    
    validatereset();
}

function ClearSearch() {
    DoSearch();
}

function ActiveForm(active) {

    _NAME.SetEnabled(active);
    _WEIGHT.SetEnabled(active);
    _COST.SetEnabled(active);
    _OT.SetEnabled(active);
    _IS_ACTIVE.SetEnabled(active);
    
}

function GetObject() {
    return {
        id: _id,
        NAME: _NAME.GetText().trim(),
        WEIGHT: _WEIGHT.GetText().trim(),
        COST: _COST.GetText().trim(),
        OT: _OT.GetText().trim(),
        IS_ACTIVE: (_IS_ACTIVE.GetChecked()) ? "Y" : "N"
    }
}

function Validate() {
    return doValidate();
}