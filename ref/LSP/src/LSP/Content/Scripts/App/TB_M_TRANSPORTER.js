var _id = -1;
var _searchTimeout;
var _searchXhr;

function Add() {
    OnPopTB_M_TRANSPORTERCloseUp();
    popTB_M_TRANSPORTER.Show();
    return false;
}

function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_M_TRANSPORTER/TB_M_TRANSPORTER_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_M_TRANSPORTER']").html("Edit TB_M_TRANSPORTER");

        _NAME.SetText(response.NAME);
        _ABBREVIATION.SetText(response.ABBREVIATION);
        _IS_ACTIVE.SetChecked((response.IS_ACTIVE == "Y") ? 1 : 0);

    }).fail(function () {
        OnPopTB_M_TRANSPORTERCloseUp();
    })
    popTB_M_TRANSPORTER.Show();
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
        url: baseUrl + "/TB_M_TRANSPORTER/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_M_TRANSPORTER.Hide();
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
    popTB_M_TRANSPORTER.Hide()
}

function OnPopTB_M_TRANSPORTERCloseUp() {
    _id = -1
    $("span[id^='popTB_M_TRANSPORTER']").html("Add New TB_M_TRANSPORTER")
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
        url: baseUrl + "/TB_M_TRANSPORTER/Delete",
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
        url: baseUrl + "/TB_M_TRANSPORTER/SetObjectInfo",
        method: "post",
        data: {
            NAME: _txtNAME.GetText(),
            ABBREVIATION: _txtABBREVIATION.GetText()
        }
    }).done(function (response) {
        gvList.PerformCallback();
    });
}

function ClearForm() {
    _NAME.SetText("");
    _ABBREVIATION.SetText("");
    _IS_ACTIVE.SetChecked(1);

    validatereset();
}

function ClearSearch() {
    DoSearch();
}

function ActiveForm(active) {

    _NAME.SetEnabled(active);
    _ABBREVIATION.SetEnabled(active);
    _IS_ACTIVE.SetEnabled(active);

}

function GetObject() {
    return {
        id: _id,
        NAME: _NAME.GetText().trim(),
        ABBREVIATION: _ABBREVIATION.GetText().trim(),
        IS_ACTIVE: (_IS_ACTIVE.GetChecked()) ? "Y" : "N"
    }
}

function Validate() {
    return doValidate();
}