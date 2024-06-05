var _id = -1;
var _searchTimeout;
var _searchXhr;

function Add() {
    OnPopTB_M_SUPPLIER_PICCloseUp();
    popTB_M_SUPPLIER_PIC.Show();
    return false;
}

function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_M_SUPPLIER_PIC/TB_M_SUPPLIER_PIC_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_M_SUPPLIER_PIC']").html("Edit SUPPLIER PIC Info.");
         
			_SUPPLIER_ID.SetValue(response.SUPPLIER_ID);
			_PIC_NAME.SetText(response.PIC_NAME);
			_PIC_TELEPHONE.SetText(response.PIC_TELEPHONE);
			_PIC_EMAIL.SetText(response.PIC_EMAIL);
			_IS_SEND_EMAIL.SetChecked((response.IS_SEND_EMAIL == "Y") ? 1 : 0);
			_IS_MAIN_PIC.SetChecked((response.IS_MAIN_PIC == "Y") ? 1 : 0); 
			_IS_ACTIVE.SetChecked((response.IS_ACTIVE == "Y") ? 1 : 0);
		   
    }).fail(function () {
        OnPopTB_M_SUPPLIER_PICCloseUp();
    })
    popTB_M_SUPPLIER_PIC.Show();
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
        url: baseUrl + "/TB_M_SUPPLIER_PIC/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_M_SUPPLIER_PIC.Hide();
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
    popTB_M_SUPPLIER_PIC.Hide()
}

function OnPopTB_M_SUPPLIER_PICCloseUp() {
	_id = -1
	$("span[id^='popTB_M_SUPPLIER_PIC']").html("Add New SUPPLIER PIC Info.")
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
        url: baseUrl + "/TB_M_SUPPLIER_PIC/Delete",
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
        url: baseUrl + "/TB_M_SUPPLIER_PIC/SetObjectInfo",
        method: "post",
        data: {
            SUPPLIER_CODE: _txtSUPPLIER_CODE.GetText(),
            SUPPLIER_NAME: _txtSUPPLIER_NAME.GetText(),
        }
    }).done(function (response) {
        gvList.PerformCallback();
    });
}

function ClearForm() {
	_SUPPLIER_ID.SelectIndex(0);
	_PIC_NAME.SetText("");
	_PIC_TELEPHONE.SetText("");
	_PIC_EMAIL.SetText("");
	_IS_SEND_EMAIL.SetChecked(1);
	_IS_MAIN_PIC.SetChecked(1); 
	_IS_ACTIVE.SetChecked(1);
	validatereset();
}

function ClearSearch() {
    DoSearch();
}

function ActiveForm(active) {
	_SUPPLIER_ID.SetEnabled(active);
	_PIC_NAME.SetEnabled(active);
	_PIC_TELEPHONE.SetEnabled(active);
	_PIC_EMAIL.SetEnabled(active);
	_IS_SEND_EMAIL.SetEnabled(active);
	_IS_MAIN_PIC.SetEnabled(active);
 
	_IS_ACTIVE.SetEnabled(active);
	
}

function GetObject() {
    return {
		ID: _id, 
		SUPPLIER_ID: _SUPPLIER_ID.GetValue(), 
		PIC_NAME: _PIC_NAME.GetText().trim(), 
		PIC_TELEPHONE: _PIC_TELEPHONE.GetText().trim(), 
		PIC_EMAIL: _PIC_EMAIL.GetText().trim(),
		IS_SEND_EMAIL: (_IS_SEND_EMAIL.GetChecked()) ? "Y" : "N",
		IS_MAIN_PIC: (_IS_MAIN_PIC.GetChecked()) ? "Y" : "N", 
		IS_ACTIVE: (_IS_ACTIVE.GetChecked()) ? "Y" : "N"
	 
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
function DOWNLOAD_SUPPLIER_PIC_TEMPLATE() {
    window.open("/Content/Template/SUPPLIER_PIC_template.xls", '_blank');
}

function IMPORT_SUPPLIER_PIC() {

    OnPopSUPPLIER_PICCloseUp();
    popIMPORT_SUPPLIER_PIC.Show();

    return false;
}

function OnPopSUPPLIER_PICCloseUp() {
    ClearFormSUPPLIER_PIC();
}

function ClearFormSUPPLIER_PIC() {
    //aspxUClearFileInputClick('_IMPORT_SUPPLIER_PIC', 0);
    //validatereset();
}

function SUPPLIER_PIC_OnFileUploadComplete(s, e) {
    //alert(e.callbackData);
    alert(e.callbackData);
    if (e.isValid) {
        popIMPORT_SUPPLIER_PIC.Hide();
        gvList.PerformCallback();
        //gvList.PerformCallback({ isCustomCallback: false, args: "" });
    }
}

function OnBtnImportCancelClicked() {
    popIMPORT_SUPPLIER_PIC.Hide();
}








