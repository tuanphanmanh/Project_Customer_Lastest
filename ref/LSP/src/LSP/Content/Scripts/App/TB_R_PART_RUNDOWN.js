var _id = -1;
var _searchTimeout;
var _searchXhr;

function Add() {
    OnPopTB_R_PART_RUNDOWNCloseUp();
    popTB_R_PART_RUNDOWN.Show();
    return false;
}

function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_R_PART_RUNDOWN/TB_R_PART_RUNDOWN_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_R_PART_RUNDOWN']").html("Edit TB_R_PART_RUNDOWN");
         
			_PART_ID.SetText(response.PART_ID);
			_STOCK_QTY.SetText(response.STOCK_QTY);
			_STOCK_DATE.SetText(response.STOCK_DATE_Str_DDMMYYYY);
			_IS_ACTIVE.SetChecked((response.IS_ACTIVE == "Y") ? 1 : 0);
			//_CREATED_BY.SetText(response.CREATED_BY);
			//_CREATED_DATE.SetText(response.CREATED_DATE_Str_DDMMYYYY);
			//_UPDATED_BY.SetText(response.UPDATED_BY);
			//_UPDATED_DATE.SetText(response.UPDATED_DATE_Str_DDMMYYYY);
		   
    }).fail(function () {
        OnPopTB_R_PART_RUNDOWNCloseUp();
    })
    popTB_R_PART_RUNDOWN.Show();
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
        url: baseUrl + "/TB_R_PART_RUNDOWN/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_R_PART_RUNDOWN.Hide();
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
    popTB_R_PART_RUNDOWN.Hide()
}

function OnPopTB_R_PART_RUNDOWNCloseUp() {
	_id = -1
	$("span[id^='popTB_R_PART_RUNDOWN']").html("Add New TB_R_PART_RUNDOWN")
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
        url: baseUrl + "/TB_R_PART_RUNDOWN/Delete",
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

function Search() {    
    _searchXhr = $.ajax({
        url: baseUrl + "/TB_R_PART_RUNDOWN/SetObjectInfo",
        method: "post",
        data: {
            SUPPLIER_CODE: _txtSUPPLIER_CODE.GetText(),
            PART_NO: _txtPART_NO.GetText(),
            STOCK_MONTH_FROM: _txtSTOCK_MONTH_FROM.GetText().trim()
        }
    }).done(function (response) {
        gvList.PerformCallback();
    });
}

function ClearForm() {
	_PART_ID.SetText("");
	_STOCK_QTY.SetText("");
	_STOCK_DATE.SetText("");
	_IS_ACTIVE.SetChecked(1);
	//_CREATED_BY.SetText("");
	//_CREATED_DATE.SetText("");
	//_UPDATED_BY.SetText("");
	//_UPDATED_DATE.SetText("");
	validatereset();
}

function ClearSearch() {
    DoSearch();
}

function ActiveForm(active) {
	_PART_ID.SetEnabled(active);
	_STOCK_QTY.SetEnabled(active);
	_STOCK_DATE.SetEnabled(active);
	_IS_ACTIVE.SetEnabled(active);
	//_CREATED_BY.SetEnabled(active);
	//_CREATED_DATE.SetEnabled(active);
	//_UPDATED_BY.SetEnabled(active);
	//_UPDATED_DATE.SetEnabled(active);
	
}

function GetObject() {
    return {
		ID: _id, 
		PART_ID: _PART_ID.GetText().trim(), 
		STOCK_QTY: _STOCK_QTY.GetText().trim(), 
		STOCK_DATE: _STOCK_DATE.GetText().trim(), 
		IS_ACTIVE: (_IS_ACTIVE.GetChecked()) ? "Y" : "N"
		//CREATED_BY: _CREATED_BY.GetText().trim(), 
		//CREATED_DATE: _CREATED_DATE.GetText().trim(), 
		//UPDATED_BY: _UPDATED_BY.GetText().trim(), 
		//UPDATED_DATE: _UPDATED_DATE.GetText().trim() 
	 
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
function DOWNLOAD_PART_RUNDOWN_TEMPLATE() {

    window.open("/Content/Template/Initial Rundown Stock_Template.xls");
}

function IMPORT_PART_RUNDOWN() {
    OnPopIMPORT_PART_RUNDOWNCloseUp();
    popIMPORT_PART_RUNDOWN.Show();

    return false;
}

function OnPopIMPORT_PART_RUNDOWNCloseUp() {
    ClearFormIMPORT_PART_RUNDOWN();
}

function ClearFormIMPORT_PART_RUNDOWN() {
    //aspxUClearFileInputClick('_IMPORT_INITIAL_RD', 0);
}

function IMPORT_PART_RUNDOWN_START(s, e) {        
    _IMPORT_PART_RUNDOWN.Upload();
    LoadingPanel.Show();
}
function IMPORT_PART_RUNDOWN_OnFileUploadComplete(s, e) {        
    msgOk(e.callbackData);
    if (e.isValid) {
        popIMPORT_PART_RUNDOWN.Hide();
        gvList.PerformCallback();
    }
    LoadingPanel.Hide();
}

function OnBtnImportCancelClicked() {
    popIMPORT_PART_RUNDOWN.Hide()
}







