var _id = -1;
var _searchTimeout;
var _searchXhr;

function Add() {
    OnPopTB_R_PART_IN_OUTCloseUp();
    popTB_R_PART_IN_OUT.Show();
    return false;
}

function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_R_PART_IN_OUT/TB_R_PART_IN_OUT_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_R_PART_IN_OUT']").html("Edit TB_R_PART_IN_OUT");

        _PART_ID.SetText(response.PART_ID);
        _IS_IN_OUT.SetText(response.IS_IN_OUT);
        _QTY.SetText(response.QTY);
        _IN_OUT_BY.SetText(response.IN_OUT_BY);
        _IN_ORDER_NO.SetText(response.IN_ORDER_NO);
        _OUT_PROD_VEHICLE_ID.SetText(response.OUT_PROD_VEHICLE_ID);
        _IS_PROCESS_STOCK.SetText(response.IS_PROCESS_STOCK);
        _IS_ACTIVE.SetChecked((response.IS_ACTIVE == "Y") ? 1 : 0);

    }).fail(function () {
        OnPopTB_R_PART_IN_OUTCloseUp();
    })
    popTB_R_PART_IN_OUT.Show();
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
        url: baseUrl + "/TB_R_PART_IN_OUT/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_R_PART_IN_OUT.Hide();
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
    popTB_R_PART_IN_OUT.Hide()
}

function OnPopTB_R_PART_IN_OUTCloseUp() {
    _id = -1
    $("span[id^='popTB_R_PART_IN_OUT']").html("Add New TB_R_PART_IN_OUT")
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
        url: baseUrl + "/TB_R_PART_IN_OUT/Delete",
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
        url: baseUrl + "/TB_R_PART_IN_OUT/SetObjectInfo",
        method: "post",
        data: {}
    }).done(function (response) {
        gvList.PerformCallback();
    });
}

function ClearForm() {
    _PART_ID.SetText("");
    _IS_IN_OUT.SetText("");
    _QTY.SetText("");
    _IN_OUT_BY.SetText("");
    _IN_ORDER_NO.SetText("");
    _OUT_PROD_VEHICLE_ID.SetText("");
    _IS_PROCESS_STOCK.SetText("");
    _IS_ACTIVE.SetChecked(1);

    validatereset();
}

function ClearSearch() {
    DoSearch();
}

function ActiveForm(active) {
    _PART_ID.SetEnabled(active);
    _IS_IN_OUT.SetEnabled(active)
    _QTY.SetEnabled(active);
    _IN_OUT_BY.SetEnabled(active);
    _IN_ORDER_NO.SetEnabled(active);
    _OUT_PROD_VEHICLE_ID.SetEnabled(active);
    _IS_PROCESS_STOCK.SetEnabled(active);
    _IS_ACTIVE.SetEnabled(active);
}

function GetObject() {
    return {
        ID: _id,
        PART_ID: _PART_ID.GetText().trim(),
        IS_IN_OUT: _IS_IN_OUT.GetText().trim(),
        QTY: _QTY.GetText().trim(),
        IN_OUT_BY: _IN_OUT_BY.GetText().trim(),
        IN_ORDER_NO: _IN_ORDER_NO.GetText().trim(),
        OUT_PROD_VEHICLE_ID: _OUT_PROD_VEHICLE_ID.GetText().trim(),
        IS_PROCESS_STOCK: _IS_PROCESS_STOCK.GetText().trim(),
        IS_ACTIVE: (_IS_ACTIVE.GetChecked()) ? "Y" : "N"
    }
}

function Validate() {

    return doValidate();
}