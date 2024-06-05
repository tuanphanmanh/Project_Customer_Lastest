var _id = -1;
var _searchTimeout;
var _searchXhr;

function Add() {
    OnPopTB_M_LOOKUPCloseUp();
    popTB_M_LOOKUP.Show();
    return false;
}

function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_M_LOOKUP/TB_M_LOOKUP_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_M_LOOKUP']").html("Edit LOOKUP");
        _DOMAIN_CODE.SetText(response.DOMAIN_CODE);
        _ITEM_CODE.SetText(response.ITEM_CODE);
        _ITEM_VALUE.SetText(response.ITEM_VALUE);
        _DESCRIPTION.SetText(response.DESCRIPTION);
        _IS_USE.SetChecked((response.IS_USE == "Y") ? 1 : 0);
        _IS_RESTRICT.SetChecked((response.IS_RESTRICT == "Y") ? 1 : 0);
        /*_IS_RESTRICT.SetText(response.IS_RESTRICT);*/
    }).fail(function () {
        OnPopTB_M_LOOKUPCloseUp();
    })
    popTB_M_LOOKUP.Show();
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
        url: baseUrl + "/TB_M_LOOKUP/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_M_LOOKUP.Hide();
            DoSearch();
        } else {
            alert(response.message);
        }
    }).always(function () {
        $btn.removeClass("saving");
    });
}

function OnBtnCancelClicked() {
    popTB_M_LOOKUP.Hide()
}

function OnPopTB_M_LOOKUPCloseUp() {
    _id = -1
    $("span[id^='popTB_M_LOOKUP']").html("Add New LOOKUP")
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
        url: baseUrl + "/TB_M_LOOKUP/Delete",
        method: "post",
        dataType: "json",
        data: {
            sid: $btn.attr("data-id")
        }
    }).done(function (response) {
        if (response.success) {
            DoSearch();
        } else {
            alert(response.message);
        }
    }).always(function () {
        $btn.html("Delete");
        $btn.parent().removeClass("processing");
    });
    return false;
}

function SavePdf(s) {
    var $btn = $(s)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    
    $btn.parent().addClass("processing");
    $btn.html("Saving...");
    $.ajax({
        url: baseUrl + "/TB_M_LOOKUP/SaveViewAsPDF",
        method: "post",
        dataType: "json",
        data: {
           
        }
    }).done(function (response) {
        //alert("test");
        if (response.success != "")
        {
            window.open(baseUrl + "/Content/Download/" + response.FileName);
        }
        
    }).always(function () {
        
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

/** QR Code generate function **/
function GenerateQRCode(s) {
    /*
    var sQR = $(s).text();
    var sSize = 70; //fixed size = 100;    
    if (sQR != null && sQR !== '') {
        $(s).empty().qrcode({
            render: "canvas",
            text: "" + sQR,
            ecLevel: "H",
            size: sSize
        });
    }*/
}

/*To fix Callback issue*/
function ReGenerateQRCode() {
   /* var divs = $('[id^=QR_CODE_]').toArray().reverse();
    for (var i = 0; i < divs.length; i++) {
        var Idselector = '#' + divs[i].id;
        GenerateQRCode(Idselector);
    }*/
}


/////////Editer
function Search() {
    _searchXhr = $.ajax({
        url: baseUrl + "/TB_M_LOOKUP/SetObjectInfo",
        method: "post",
        data: {
            DOMAIN_CODE: _txtDOMAIN_CODE.GetText(),
            ITEM_CODE: _txtITEM_CODE.GetText()
        }
    }).done(function (response) {
        gvList.PerformCallback();
    });
}

function ClearForm() {
    _DOMAIN_CODE.SetText("");
    _ITEM_CODE.SetText("");
    _ITEM_VALUE.SetText("");
    _DESCRIPTION.SetText("");
    _IS_USE.SetChecked(1);
    _IS_RESTRICT.SetChecked(0);
    /*_IS_RESTRICT.SetText("");*/
    validatereset();
}

function ClearSearch() {
    _txtUserName.SetText("");
    DoSearch();
}

function ActiveForm(active) {
    _DOMAIN_CODE.SetEnabled(active);
    _ITEM_CODE.SetEnabled(active);
    _ITEM_VALUE.SetEnabled(active);
    _DESCRIPTION.SetEnabled(active);
    _IS_USE.SetEnabled(active);
    _IS_RESTRICT.SetEnabled();
}

function GetObject() {
    return {
        ID: _id,
        DOMAIN_CODE: _DOMAIN_CODE.GetText().trim(),
        ITEM_CODE: _ITEM_CODE.GetText().trim(),
        ITEM_VALUE: _ITEM_VALUE.GetText().trim(),
        DESCRIPTION: _DESCRIPTION.GetText().trim(),
        IS_USE: (_IS_USE.GetChecked()) ? "Y" : "N",
        IS_RESTRICT: (_IS_RESTRICT.GetChecked()) ? "Y" : "N"
        /*IS_RESTRICT: _IS_RESTRICT.GetText().trim()*/
    }
}

function Validate() {
    return doValidate();
}