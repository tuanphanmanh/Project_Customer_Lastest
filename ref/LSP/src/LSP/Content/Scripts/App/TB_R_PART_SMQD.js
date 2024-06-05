var _id = -1;
var _searchTimeout;
var _searchXhr;

function Add() {
    OnPopTB_R_PART_SMQDCloseUp();
    popTB_R_PART_SMQD.Show();
    return false;
}

function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_R_PART_SMQD/TB_R_PART_SMQD_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_R_PART_SMQD']").html("Edit PART SMQD");
         
			//_PART_NO.SetText(response.PART_NO);
			//_COLOR_SFX.SetText(response.COLOR_SFX);
            //_PART_NAME.SetText(response.PART_NAME);
            //_BACK_NO.SetText(response.BACK_NO);
            //_SUPPLIER_CODE.SetText(response.SUPPLIER_CODE);
            _PART_ID.SetValue(response.PART_ID);
			_SMQD_DATETIME.SetText(response.SMQD_DATETIME_Str_DDMMYYYY);
			_SMQD_QTY.SetText(response.SMQD_QTY);
			_SMQD_TYPE.SetText(response.SMQD_TYPE);
			_PIC.SetText(response.PIC);
			_RUN_NO.SetText(response.RUN_NO);
			_REASON.SetText(response.REASON);
			_STATUS.SetText(response.STATUS);			
			_IS_ACTIVE.SetChecked((response.IS_ACTIVE == "Y") ? 1 : 0);
			//_CREATED_BY.SetText(response.CREATED_BY);
			//_CREATED_DATE.SetText(response.CREATED_DATE_Str_DDMMYYYY);
			//_UPDATED_BY.SetText(response.UPDATED_BY);
			//_UPDATED_DATE.SetText(response.UPDATED_DATE_Str_DDMMYYYY);
		   
			_PART_ID.SetEnabled(false);
    }).fail(function () {
        OnPopTB_R_PART_SMQDCloseUp();
    })
    popTB_R_PART_SMQD.Show();
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
        url: baseUrl + "/TB_R_PART_SMQD/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_R_PART_SMQD.Hide();
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
    ActiveForm(true);
    popTB_R_PART_SMQD.Hide()
}

function OnPopTB_R_PART_SMQDCloseUp() {
	_id = -1
	$("span[id^='popTB_R_PART_SMQD']").html("Add New PART SMQD")
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
        url: baseUrl + "/TB_R_PART_SMQD/Delete",
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
        url: baseUrl + "/TB_R_PART_SMQD/SetObjectInfo",
        method: "post",
        data: {
            SUPPLIER_CODE: _txtSUPPLIER_CODE.GetText().trim(),
            YEAR: _txtYEAR.GetText().trim(),
            MONTH: _txtMONTH.GetText().trim()
        }
    }).done(function (response) {
        gvList.PerformCallback();
    });
}

function ClearForm() {
	//_PART_NO.SetText("");
	//_COLOR_SFX.SetText("");
	//_PART_NAME.SetText("");
	//_BACK_NO.SetText("");
    //_SUPPLIER_CODE.SetText("");
    _PART_ID.SetValue("");
	_SMQD_DATETIME.SetText("");
	_SMQD_QTY.SetText("");
	_SMQD_TYPE.SetText("");
	_PIC.SetText("");
	_RUN_NO.SetText("");
	_REASON.SetText("");
	_STATUS.SetText("");
	_IS_ACTIVE.SetChecked(1);
	//_IS_ACTIVE.SetText("");
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
	//_PART_NO.SetEnabled(active);
	//_COLOR_SFX.SetEnabled(active);
	//_PART_NAME.SetEnabled(active);
	//_BACK_NO.SetEnabled(active);
    //_SUPPLIER_CODE.SetEnabled(active);
    _PART_ID.SetEnabled(active);
	_SMQD_DATETIME.SetEnabled(active);
	_SMQD_QTY.SetEnabled(active);
	_SMQD_TYPE.SetEnabled(active);
	_PIC.SetEnabled(active);
	_RUN_NO.SetEnabled(active);
	_REASON.SetEnabled(active);
	_STATUS.SetEnabled(active);
	_IS_ACTIVE.SetEnabled(active);
	//_CREATED_BY.SetEnabled(active);
	//_CREATED_DATE.SetEnabled(active);
	//_UPDATED_BY.SetEnabled(active);
	//_UPDATED_DATE.SetEnabled(active);
	
}

function GetObject() {
    return {
		ID: _id, 
		//PART_NO: _PART_NO.GetText().trim(), 
		//COLOR_SFX: _COLOR_SFX.GetText().trim(), 
		//PART_NAME: _PART_NAME.GetText().trim(), 
		//BACK_NO: _BACK_NO.GetText().trim(), 
        //SUPPLIER_CODE: _SUPPLIER_CODE.GetText().trim(), 
		PART_ID:_PART_ID.GetValue(),
		SMQD_DATETIME: ConvertDate(_SMQD_DATETIME.GetText().trim()), 
		SMQD_QTY: _SMQD_QTY.GetText().trim(), 
		SMQD_TYPE: _SMQD_TYPE.GetText().trim(), 
		PIC: _PIC.GetText().trim(), 
		RUN_NO: _RUN_NO.GetText().trim(), 
		REASON: _REASON.GetText().trim(), 
		STATUS: _STATUS.GetText().trim(),
		IS_ACTIVE: (_IS_ACTIVE.GetChecked()) ? "Y" : "N"
		//IS_ACTIVE: _IS_ACTIVE.GetText().trim()
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






/***************IMPORT*******************/
function IMPORT_PART_SMQD() {

    OnPopPART_SMQDCloseUp();
    popIMPORT_PART_SMQD.Show();

    return false;
}

function OnPopPART_SMQDCloseUp() {
    ClearFormPART_SMQD();
}

function ClearFormPART_SMQD() {
    //aspxUClearFileInputClick('_IMPORT_PART_SMQD', 0);
    //validatereset();
}

function PART_SMQD_OnFileUploadComplete(s, e) {
    //alert(e.callbackData);
    alert(e.callbackData);
    if (e.isValid) {
        popIMPORT_PART_SMQD.Hide();
        gvList.PerformCallback();
        //gvList.PerformCallback({ isCustomCallback: false, args: "" });
    }
}


function OnBtnImportCancelClicked() {
    popIMPORT_PART_SMQD.Hide();
}








