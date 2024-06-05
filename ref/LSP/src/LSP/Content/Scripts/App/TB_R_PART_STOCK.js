var _id = -1;
var _searchTimeout;
var _searchXhr;

function Add() {
    OnPopTB_R_PART_STOCKCloseUp();
    popTB_R_PART_STOCK.Show();
    return false;
}

function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_R_PART_STOCK/TB_R_PART_STOCK_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_R_PART_STOCK']").html("Edit PART STOCK");

        _PART_ID_S.SetText(response.PART_ID);
        _STOCK_QTY.SetText(response.STOCK_QTY);
        _SUPPLIER_CODE.SetText(response.SUPPLIER_CODE);
        _PART_NO.SetText(response.PART_NO);
        _IS_ACTIVE.SetChecked((response.IS_ACTIVE == "Y") ? 1 : 0);

        ActiveForm(true);
    }).fail(function () {
        OnPopTB_R_PART_STOCKCloseUp();
    })
    popTB_R_PART_STOCK.Show();
    return false;
}

function OnBtnUpdateClicked(s) {
    var $btn = $(s);
   
    if ($btn.hasClass("saving")) {
        return;
    }
           
    ActiveForm(false);
    $btn.addClass("saving");
    
    $.ajax({
        url: baseUrl + "/TB_R_PART_STOCK/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_R_PART_STOCK.Hide();
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
    popTB_R_PART_STOCK.Hide()
}

function OnPopTB_R_PART_STOCKCloseUp() {
    _id = -1
    $("span[id^='popTB_R_PART_STOCK']").html("Add New PART STOCK")
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
        url: baseUrl + "/TB_R_PART_STOCK/Delete",
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
        url: baseUrl + "/TB_R_PART_STOCK/SetObjectInfo",
        method: "post",
        data: {
            PART_NO: _txtPART_NO.GetText(),
            SUPPLIER_CODE: _txtSUPPLIER_CODE.GetText(),
            SHOP: _txtSHOP.GetValue()
        }
    }).done(function (response) {
        gvList.PerformCallback();
    });
}

function ClearForm() {
    _PART_ID_S.SetText("");
    _STOCK_QTY.SetText("");
    _SUPPLIER_CODE.SetText("");
    _PART_NO.SetText("");
    _IS_ACTIVE.SetChecked(1);

    validatereset();
}

function ClearSearch() {
    DoSearch();
}

function ActiveForm(active) {
    _PART_ID_S.SetEnabled(false);
    _STOCK_QTY.SetEnabled(active);
    _IS_ACTIVE.SetEnabled(active);
    _SUPPLIER_CODE.SetEnabled(false);
    _PART_NO.SetEnabled(false);
}

function GetObject() {
    return {
        ID: _id,
        PART_ID: _PART_ID_S.GetText().trim(),
        STOCK_QTY: _STOCK_QTY.GetText().trim(),
        IS_ACTIVE: (_IS_ACTIVE.GetChecked()) ? "Y" : "N"
    }
}

function Validate() {

    return doValidate();
}

//PART_IN_OUT
var _PART_ID = 0;
var _ID = 0;

function PART_IN_OUT_New(t) {
    OnPopTB_R_PART_IN_OUTCloseUp();
    _PART_ID = $(t).attr("data-part-id");
    popTB_R_PART_IN_OUT.Show();
}

function OnPopTB_R_PART_IN_OUTCloseUp() {
    _PART_ID = 0;
    _ID = 0;
    $("span[id^='popTB_R_PART_IN_OUT']").html("Add new TB_R_PART_IN_OUT");
    ClearFormPART_IN_OUT();
}

function ClearFormPART_IN_OUT() {

    _PART_ID = 0;
    _ID = 0;

    _IS_IN_OUT.SetText("");
    _QTY.SetText("");
    _IN_OUT_BY.SetText("");
    _IN_ORDER_NO.SetText("");
    _OUT_PROD_VEHICLE_ID.SetText("");
    _IS_PROCESS_STOCK.SetText("");
    _IS_ACTIVE_V.SetText("");

    validatereset();
}

function OnBtnCancelPClicked() {
    popTB_R_PART_IN_OUT.Hide()
}

function OnBtnUpdatePClicked(s) {
    var $btn = $(s);
    if ($btn.hasClass("saving")) {
        return;
    }
    if (!doValidateParam("PART_IN_OUT")) {
        return;
    }
    ActiveFormPART_IN_OUT(false);
    $btn.addClass("saving");
    $.ajax({
        url: baseUrl + "/TB_R_PART_IN_OUT/SaveData",
        method: "post",
        data: GetObjectPART_IN_OUT(),
        dataType: "json"
    }).done(function (response) {
        ActiveFormPART_IN_OUT(true);
        if (response.success) {
            popTB_R_PART_IN_OUT.Hide();
            gvList.PerformCallback();
        } else {
            alert(response.message);
        }
    }).always(function () {
        $btn.removeClass("saving");
    });
}

function ActiveFormPART_IN_OUT(active) {

    _IS_IN_OUT.SetEnabled(active)
    _QTY.SetEnabled(active);
    _IN_OUT_BY.SetEnabled(active);
    _IN_ORDER_NO.SetEnabled(active);
    _OUT_PROD_VEHICLE_ID.SetEnabled(active);
    _IS_PROCESS_STOCK.SetEnabled(active);
    _IS_ACTIVE_V.SetEnabled(active);

}

function GetObjectPART_IN_OUT() {
    return {

        ID: _ID,
        PART_ID: _PART_ID,
        IS_IN_OUT: _IS_IN_OUT.GetText().trim(),
        QTY: _QTY.GetText().trim(),
        IN_OUT_BY: _IN_OUT_BY.GetText().trim(),
        IN_ORDER_NO: _IN_ORDER_NO.GetText().trim(),
        OUT_PROD_VEHICLE_ID: _OUT_PROD_VEHICLE_ID.GetText().trim(),
        IS_PROCESS_STOCK: _IS_PROCESS_STOCK.GetText().trim(),
        IS_ACTIVE: _IS_ACTIVE_V.GetText().trim()
    }
}

function PART_IN_OUTDelete(s) {
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

function PART_IN_OUTEdit(t) {

    OnPopTB_R_PART_IN_OUTCloseUp();

    _ID = $(t).attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_R_PART_IN_OUT/TB_R_PART_IN_OUT_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _ID
        }
    }).done(function (response) {

        _PART_ID = response.PART_ID;

        _IS_IN_OUT.SetText(response.IS_IN_OUT);
        _QTY.SetText(response.QTY);
        _IN_OUT_BY.SetText(response.IN_OUT_BY);
        _IN_ORDER_NO.SetText(response.IN_ORDER_NO);
        _OUT_PROD_VEHICLE_ID.SetText(response.OUT_PROD_VEHICLE_ID);
        _IS_PROCESS_STOCK.SetText(response.IS_PROCESS_STOCK);
        _IS_ACTIVE_V.SetText(response.IS_ACTIVE);

        popTB_R_PART_IN_OUT.Show();
    }).fail(function () {
        OnPopTB_R_PART_IN_OUTCloseUp();
    });
    return false;
}

/*******************************STOCK MONTH DETAIL *********************************/
function showStockDetail() {
    popSTOCKDetails.Show();
}

function OnpopSTOCKDetailsCloseUp() {

}

function SelectSTOCK_MONTH() {
    $.ajax({
        url: baseUrl + "/TB_R_PART_STOCK/TB_R_PART_STOCK_SetDetails",
        method: "get",
        //dataType: "json",
        data: {
            STOCK_MONTH: _txtSTOCK_MONTH.GetText().trim(),
            SUPPLIER_CODE: _txtSUPPLIER_DETAILS.GetText().trim()            
        }
    }).done(function (response) {
        gvStockDetailsList.PerformCallback();
    });
}

/*******************************STOCK I/O DETAIL *********************************/
function showStockDetailIO() {
    popSTOCKDetailsIO.Show();
}

function OnpopSTOCKDetailsIOCloseUp() {

}

function SelectSTOCK_DetailsIO() {
    $.ajax({
        url: baseUrl + "/TB_R_PART_STOCK/TB_R_PART_STOCK_SetDetailsIO",
        method: "get",        
        data: {
            PART_NO: _txtPART_NO_IO.GetText().trim(),
            SUPPLIER_CODE: _txtSUPPLIER_CODE_IO.GetText().trim(),
            STOCK_DATE_FROM: ConvertDate(_txtSTOCK_DATE_FROM.GetText().trim()),
            STOCK_DATE_TO: ConvertDate(_txtSTOCK_DATE_TO.GetText().trim()),
            IS_IN_OUT: _txtIS_IN_OUT.GetValue()
            
        }
    }).done(function (response) {
        gvStockDetailsIOList.PerformCallback();
    });
}

/*******************************STOCK IMPORT *********************************/
function DOWNLOAD_PART_STOCK_TEMPLATE() {

    window.open("/Content/Template/Upload_Stock_Template.xls");
}

function IMPORT_PART_STOCK() {
    OnPopIMPORT_PART_STOCKCloseUp();
    popIMPORT_PART_STOCK.Show();

    return false;
}

function OnPopIMPORT_PART_STOCKCloseUp() {
    ClearFormIMPORT_PART_STOCK();
}

function ClearFormIMPORT_PART_STOCK() {
    //aspxUClearFileInputClick('_IMPORT_INITIAL_RD', 0);
}

function IMPORT_PART_STOCK_START(s, e) {
    _IMPORT_PART_STOCK.Upload();
    LoadingPanel.Show();
}
function IMPORT_PART_STOCK_OnFileUploadComplete(s, e) {
    msgOk(e.callbackData);
    if (e.isValid) {
        popIMPORT_PART_STOCK.Hide();
        gvList.PerformCallback();
    }
    LoadingPanel.Hide();
}

function OnBtnImportCancelClicked() {
    popIMPORT_PART_STOCK.Hide()
}
