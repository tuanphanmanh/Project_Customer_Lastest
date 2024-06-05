var _id = -1;
var _searchTimeout;
var _searchXhr;

function Add() {
    OnPopTB_M_COLORCloseUp();
    popTB_M_COLOR.Show();
    return false;
}

function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_M_COLOR/TB_M_COLOR_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_M_COLOR']").html("Edit TB_M_COLOR");

        _CODE.SetText(response.CODE);
        _NAME_EN.SetText(response.NAME_EN);
        _NAME_VN.SetText(response.NAME_VN);
        _TYPE.SetText(response.TYPE);
        _IS_ACTIVE.SetChecked((response.IS_ACTIVE == "Y") ? 1 : 0);

    }).fail(function () {
        OnPopTB_M_COLORCloseUp();
    })
    popTB_M_COLOR.Show();
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
        url: baseUrl + "/TB_M_COLOR/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_M_COLOR.Hide();
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
    popTB_M_COLOR.Hide()
}

function OnPopTB_M_COLORCloseUp() {
    _id = -1
    $("span[id^='popTB_M_COLOR']").html("Add New TB_M_COLOR")
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
        url: baseUrl + "/TB_M_COLOR/Delete",
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
        url: baseUrl + "/TB_M_COLOR/SetObjectInfo",
        method: "post",
        data: {
            CODE: _txtCODE.GetText(),
            NAME_EN: _txtNAME_EN.GetText(),
            NAME_VN: _txtNAME_VN.GetText()
        }
    }).done(function (as) {
        gvList.PerformCallback();
    });
}

function ClearForm() {
    _CODE.SetText("");
    _NAME_EN.SetText("");
    _NAME_VN.SetText("");
    _TYPE.SetText("");
    _IS_ACTIVE.SetChecked(1);
    validatereset();
}

function ActiveForm(active) {
    _CODE.SetEnabled(active);
    _NAME_EN.SetEnabled(active);
    _NAME_VN.SetEnabled(active);
    _TYPE.SetEnabled(active);
    _IS_ACTIVE.SetEnabled(active);
}

function GetObject() {
    return {
        ID: _id,
        CODE: _CODE.GetText().trim(),
        NAME_EN: _NAME_EN.GetText().trim(),
        NAME_VN: _NAME_VN.GetText().trim(),
        TYPE: _TYPE.GetText().trim(),
        IS_ACTIVE: (_IS_ACTIVE.GetChecked()) ? "Y" : "N"
    }
}

function Validate() {
    return doValidate();
}