var _id = -1;
var _searchTimeout;
var _searchXhr;

function Add() {
    OnPopTB_R_UP_PLAN_DCloseUp();
    popTB_R_UP_PLAN_D.Show();
    return false;
}

function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_R_UP_PLAN_D/TB_R_UP_PLAN_D_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_R_UP_PLAN_D']").html("Edit TB_R_UP_PLAN_D");
         
			_UP_PLAN_H_ID.SetText(response.UP_PLAN_H_ID);
			_LINE.SetText(response.LINE);
			_NO.SetText(response.NO);
			_BACK_NO.SetText(response.BACK_NO);
			_CASE_NO.SetText(response.CASE_NO);
			_SUPPLIER_NO.SetText(response.SUPPLIER_NO);
			_MODEL.SetText(response.MODEL);
			_PART_NO.SetText(response.PART_NO);
			_PART_NAME.SetText(response.PART_NAME);
			_PC_ADDRESS.SetText(response.PC_ADDRESS);
			_QTY.SetText(response.QTY);
			_BOX_SIZE.SetText(response.BOX_SIZE);
			_QTY_BOX.SetText(response.QTY_BOX);
			_QTY_ACT.SetText(response.QTY_ACT);
			_PXP_LOCATION.SetText(response.PXP_LOCATION);
			_WORKING_DATE.SetText(response.WORKING_DATE_Str_DDMMYYYY);
			_SHIFT.SetText(response.SHIFT);
			_UP_STATUS.SetText(response.UP_STATUS);
			_INCOMP_REASON.SetText(response.INCOMP_REASON);
			_IS_ACTIVE.SetText(response.IS_ACTIVE);
			_IS_OVER.SetText(response.IS_OVER);
		   
    }).fail(function () {
        OnPopTB_R_UP_PLAN_DCloseUp();
    })
    popTB_R_UP_PLAN_D.Show();
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
        url: baseUrl + "/TB_R_UP_PLAN_D/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_R_UP_PLAN_D.Hide();
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
    popTB_R_UP_PLAN_D.Hide()
}

function OnPopTB_R_UP_PLAN_DCloseUp() {
	_id = -1
	$("span[id^='popTB_R_UP_PLAN_D']").html("Add New TB_R_UP_PLAN_D")
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
        url: baseUrl + "/TB_R_UP_PLAN_D/Delete",
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
        url: baseUrl + "/TB_R_UP_PLAN_D/SetObjectInfo",
        method: "post",
        data: {
            //UserName: _UserName.GetText(),
			//Password: _Password.GetText(),
        }
    }).done(function (response) {
        gvList.PerformCallback();
    });
}

function ClearForm() {
	_UP_PLAN_H_ID.SetText("");
	_LINE.SetText("");
	_NO.SetText("");
	_BACK_NO.SetText("");
	_CASE_NO.SetText("");
	_SUPPLIER_NO.SetText("");
	_MODEL.SetText("");
	_PART_NO.SetText("");
	_PART_NAME.SetText("");
	_PC_ADDRESS.SetText("");
	_QTY.SetText("");
	_BOX_SIZE.SetText("");
	_QTY_BOX.SetText("");
	_QTY_ACT.SetText("");
	_PXP_LOCATION.SetText("");
	_WORKING_DATE.SetText("");
	_SHIFT.SetText("");
	_UP_STATUS.SetText("");
	_INCOMP_REASON.SetText("");
	_IS_ACTIVE.SetText("");
	_IS_OVER.SetText("");
	validatereset();
}

function ClearSearch() {
    DoSearch();
}

function ActiveForm(active) {
	_UP_PLAN_H_ID.SetEnabled(active);
	_LINE.SetEnabled(active);
	_NO.SetEnabled(active);
	_BACK_NO.SetEnabled(active);
	_CASE_NO.SetEnabled(active);
	_SUPPLIER_NO.SetEnabled(active);
	_MODEL.SetEnabled(active);
	_PART_NO.SetEnabled(active);
	_PART_NAME.SetEnabled(active);
	_PC_ADDRESS.SetEnabled(active);
	_QTY.SetEnabled(active);
	_BOX_SIZE.SetEnabled(active);
	_QTY_BOX.SetEnabled(active);
	_QTY_ACT.SetEnabled(active);
	_PXP_LOCATION.SetEnabled(active);
	_WORKING_DATE.SetEnabled(active);
	_SHIFT.SetEnabled(active);
	_UP_STATUS.SetEnabled(active);
	_INCOMP_REASON.SetEnabled(active);
	_IS_ACTIVE.SetEnabled(active);
	_IS_OVER.SetEnabled(active);
	
}

function GetObject() {
    return {
		ID: _id, 
		UP_PLAN_H_ID: _UP_PLAN_H_ID.GetText().trim(), 
		LINE: _LINE.GetText().trim(), 
		NO: _NO.GetText().trim(), 
		BACK_NO: _BACK_NO.GetText().trim(), 
		CASE_NO: _CASE_NO.GetText().trim(), 
		SUPPLIER_NO: _SUPPLIER_NO.GetText().trim(), 
		MODEL: _MODEL.GetText().trim(), 
		PART_NO: _PART_NO.GetText().trim(), 
		PART_NAME: _PART_NAME.GetText().trim(), 
		PC_ADDRESS: _PC_ADDRESS.GetText().trim(), 
		QTY: _QTY.GetText().trim(), 
		BOX_SIZE: _BOX_SIZE.GetText().trim(), 
		QTY_BOX: _QTY_BOX.GetText().trim(), 
		QTY_ACT: _QTY_ACT.GetText().trim(), 
		PXP_LOCATION: _PXP_LOCATION.GetText().trim(), 
		WORKING_DATE: _WORKING_DATE.GetText().trim(), 
		SHIFT: _SHIFT.GetText().trim(), 
		UP_STATUS: _UP_STATUS.GetText().trim(), 
		INCOMP_REASON: _INCOMP_REASON.GetText().trim(), 
		IS_ACTIVE: _IS_ACTIVE.GetText().trim(), 
		IS_OVER: _IS_OVER.GetText().trim(), 
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