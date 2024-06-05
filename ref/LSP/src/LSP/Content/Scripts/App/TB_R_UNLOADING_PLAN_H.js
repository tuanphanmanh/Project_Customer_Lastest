var _id = -1;
var _searchTimeout;
var _searchXhr;

function Add() {
    OnPopTB_R_UNLOADING_PLAN_HCloseUp();
    popTB_R_UNLOADING_PLAN_H.Show();
    return false;
}

function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_R_UNLOADING_PLAN_H/TB_R_UNLOADING_PLAN_H_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_R_UNLOADING_PLAN_H']").html("Edit TB_R_UNLOADING_PLAN_H");
         
			_DOCK.SetText(response.DOCK);
			_TRUCK.SetText(response.TRUCK);
			_SUPPLIERS.SetText(response.SUPPLIERS);
			_FROM_DATE.SetText(response.FROM_DATE_Str_DDMMYYYY);
			_PLAN_START_UP_DATETIME.SetText(response.PLAN_START_UP_DATETIME_Str_DDMMYYYY);
			_PLAN_FINISH_UP_DATETIME.SetText(response.PLAN_FINISH_UP_DATETIME_Str_DDMMYYYY);
			_ANDON_NO.SetText(response.ANDON_NO);
			_CREATED_BY.SetText(response.CREATED_BY);
			_CREATED_DATE.SetText(response.CREATED_DATE_Str_DDMMYYYY);
			_UPDATED_BY.SetText(response.UPDATED_BY);
			_UPDATED_DATE.SetText(response.UPDATED_DATE_Str_DDMMYYYY);
			_IS_ACTIVE.SetText(response.IS_ACTIVE);
		   
    }).fail(function () {
        OnPopTB_R_UNLOADING_PLAN_HCloseUp();
    })
    popTB_R_UNLOADING_PLAN_H.Show();
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
    //$("#dealerStatus").html("Saving...");
    $.ajax({
        url: baseUrl + "/TB_R_UNLOADING_PLAN_H/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_R_UNLOADING_PLAN_H.Hide();
            DoSearch();
        }
        else {
            alert(response.message);
        }
    }).always(function () {
        //$("#dealerStatus").html("");
        $btn.removeClass("saving");
    });
}

function OnBtnCancelClicked() {
    popTB_R_UNLOADING_PLAN_H.Hide()
}

function OnPopTB_R_UNLOADING_PLAN_HCloseUp() {
	_id = -1
	$("span[id^='popTB_R_UNLOADING_PLAN_H']").html("Add New TB_R_UNLOADING_PLAN_H")
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
        url: baseUrl + "/TB_R_UNLOADING_PLAN_H/Delete",
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

/////////Editer
function Search() {
    _searchXhr = $.ajax({
        url: baseUrl + "/TB_R_UNLOADING_PLAN_H/SetObjectInfo",
        method: "post",
        data: {
            DOCK: _txtDOCK.GetText().trim(),
            TRUCK: _txtTRUCK.GetText().trim(),
            SUPPLIERS: _txtSUPPLIERS.GetText().trim(),
            FROM_DATE: _txtFROM_DATE.GetText().trim(), 
            IS_ACTIVE: _txtIS_ACTIVE.GetValue()
        }
    }).done(function (response) {
        gvList.PerformCallback();
    });
}

function ClearForm() {
	_DOCK.SetText("");
	_TRUCK.SetText("");
	_SUPPLIERS.SetText("");
	_FROM_DATE.SetText("");
	_PLAN_START_UP_DATETIME.SetText("");
	_PLAN_FINISH_UP_DATETIME.SetText("");
	_ANDON_NO.SetText("");
	_CREATED_BY.SetText("");
	_CREATED_DATE.SetText("");
	_UPDATED_BY.SetText("");
	_UPDATED_DATE.SetText("");
	_IS_ACTIVE.SetText("");
	validatereset();
}

function ClearSearch() {
    DoSearch();
}

function ActiveForm(active) {
	_DOCK.SetEnabled(active);
	_TRUCK.SetEnabled(active);
	_SUPPLIERS.SetEnabled(active);
	_FROM_DATE.SetEnabled(active);
	_PLAN_START_UP_DATETIME.SetEnabled(active);
	_PLAN_FINISH_UP_DATETIME.SetEnabled(active);
	_ANDON_NO.SetEnabled(active);
	_CREATED_BY.SetEnabled(active);
	_CREATED_DATE.SetEnabled(active);
	_UPDATED_BY.SetEnabled(active);
	_UPDATED_DATE.SetEnabled(active);
	_IS_ACTIVE.SetEnabled(active);
	
}

function GetObject() {
    return {
		ID: _id, 
		DOCK: _DOCK.GetText().trim(), 
		TRUCK: _TRUCK.GetText().trim(), 
		SUPPLIERS: _SUPPLIERS.GetText().trim(), 
		FROM_DATE: _FROM_DATE.GetText().trim(), 
		PLAN_START_UP_DATETIME: _PLAN_START_UP_DATETIME.GetText().trim(), 
		PLAN_FINISH_UP_DATETIME: _PLAN_FINISH_UP_DATETIME.GetText().trim(), 
		ANDON_NO: _ANDON_NO.GetText().trim(), 
		CREATED_BY: _CREATED_BY.GetText().trim(), 
		CREATED_DATE: _CREATED_DATE.GetText().trim(), 
		UPDATED_BY: _UPDATED_BY.GetText().trim(), 
		UPDATED_DATE: _UPDATED_DATE.GetText().trim(), 
		IS_ACTIVE: _IS_ACTIVE.GetText().trim() 
	 
    }
}

function Validate() {
    //var isValid = true
    //var obj = GetObject()
    //if (obj.ABBREVIATION.length === 0) {
    //    alert("Please enter Abbreviation!")
    //    isValid = false
    //} 
    //else if (isNaN(obj.ORDERING) || parseInt(obj.ORDERING) < 0) {
    //    alert("Ordering is invalid!")
    //    isValid = false
    //}
    //return isValid
	
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



/*IMPORT*/
function DOWNLOAD_UNLOADING_PLAN_H_TEMPLATE() {

    window.open("/Content/Template/LSP_Plan Unloading Template.xlsx");
}

function IMPORT_UNLOADING_H() {
    OnPopUNLOADING_PLAN_HCloseUp();
    popUNLOADING_PLAN_H.Show();

    return false;
}

function OnPopUNLOADING_PLAN_HCloseUp() {
    ClearFormUNLOADING_PLAN_H();
}

function ClearFormUNLOADING_PLAN_H() {
    //aspxUClearFileInputClick('_IMPORT_UNLOADING_PLAN_H', 0);
}

function UNLOADING_PLAN_H_OnFileUploadComplete(s, e) {
    alert(e.callbackData);
    if (e.isValid) {
        popUNLOADING_PLAN_H.Hide();
        gvList.PerformCallback();
    }
}

function OnBtnImportCancelClicked() {
    popUNLOADING_PLAN_H.Hide()
}

//V2 to support EPE
function DOWNLOAD_UNLOADING_PLAN_H_TEMPLATE_V2() {

    window.open("/Content/Template/LSP_Plan Unloading Template_V2.xlsx");
}
function UNLOADING_PLAN_H_OnFileUploadComplete_V2(s, e) {
    alert(e.callbackData);
    if (e.isValid) {
        popUNLOADING_PLAN_H_V2.Hide();
        gvList.PerformCallback();
    }
}

function OnBtnImportCancelClicked_V2() {
    popUNLOADING_PLAN_H_V2.Hide();
}

function IMPORT_UNLOADING_H_V2() {
    OnPopUNLOADING_PLAN_HCloseUp_V2();
    popUNLOADING_PLAN_H_V2.Show();

    return false;
}

function OnPopUNLOADING_PLAN_HCloseUp_V2() {
    ClearFormUNLOADING_PLAN_H_V2();
}

function ClearFormUNLOADING_PLAN_H_V2() {
    //aspxUClearFileInputClick('_IMPORT_UNLOADING_PLAN_H_V2', 0);
}

/** QR Code generate function **/
function GenerateQRCode(s) {
    var sQR = $(s).text();
    var sSize = 60; //fixed size = 100;    
    if (sQR != null && sQR !== '') {
        $(s).empty().qrcode({
            render: "canvas",
            text: "UL-" + sQR.substring(0,4), // 4 digits
            ecLevel: "H",
            size: sSize
        });
    }
}

/*To fix Callback issue*/
function ReGenerateQRCode() {
    var divs = $('[id^=QR_CODE_]').toArray().reverse();

    for (var i = 0; i < divs.length; i++) {
        var Idselector = '#' + divs[i].id;
        GenerateQRCode(Idselector);
    }
}