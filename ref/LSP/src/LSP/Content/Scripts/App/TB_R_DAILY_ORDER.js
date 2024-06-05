var _id = -1;
var _searchTimeout;
var _searchXhr;

function Add() {
    OnPopTB_R_DAILY_ORDERCloseUp();
    popTB_R_DAILY_ORDER.Show();
    return false;
}

function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_R_DAILY_ORDER/TB_R_DAILY_ORDER_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_R_DAILY_ORDER']").html("Edit TB_R_DAILY_ORDER");
         
			_WORKING_DATE.SetText(response.WORKING_DATE_Str_DDMMYYYY);
			_SHIFT.SetText(response.SHIFT);
			_SUPPLIER_NAME.SetText(response.SUPPLIER_NAME);
			_SUPPLIER_CODE.SetText(response.SUPPLIER_CODE);
			_ORDER_NO.SetText(response.ORDER_NO);
			_ORDER_DATETIME.SetText(response.ORDER_DATETIME_Str_DDMMYYYY);
			_TRIP_NO.SetText(response.TRIP_NO);
			_TRUCK_NO.SetText(response.TRUCK_NO);
			_EST_ARRIVAL_DATETIME.SetText(response.EST_ARRIVAL_DATETIME_Str_DDMMYYYY);
			_CREATED_BY.SetText(response.CREATED_BY);
			_CREATED_DATE.SetText(response.CREATED_DATE_Str_DDMMYYYY);
			_UPDATED_BY.SetText(response.UPDATED_BY);
			_UPDATED_DATE.SetText(response.UPDATED_DATE_Str_DDMMYYYY);
			_IS_ACTIVE.SetText(response.IS_ACTIVE);
			_STATUS.SetText(response.STATUS);
		   
    }).fail(function () {
        OnPopTB_R_DAILY_ORDERCloseUp();
    })
    popTB_R_DAILY_ORDER.Show();
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
        url: baseUrl + "/TB_R_DAILY_ORDER/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_R_DAILY_ORDER.Hide();
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
    popTB_R_DAILY_ORDER.Hide()
}

function OnPopTB_R_DAILY_ORDERCloseUp() {
	_id = -1
	$("span[id^='popTB_R_DAILY_ORDER']").html("Add New TB_R_DAILY_ORDER")
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
        url: baseUrl + "/TB_R_DAILY_ORDER/Delete",
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
        url: baseUrl + "/TB_R_DAILY_ORDER/SetObjectInfo",
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
	_WORKING_DATE.SetText("");
	_SHIFT.SetText("");
	_SUPPLIER_NAME.SetText("");
	_SUPPLIER_CODE.SetText("");
	_ORDER_NO.SetText("");
	_ORDER_DATETIME.SetText("");
	_TRIP_NO.SetText("");
	_TRUCK_NO.SetText("");
	_EST_ARRIVAL_DATETIME.SetText("");
	_CREATED_BY.SetText("");
	_CREATED_DATE.SetText("");
	_UPDATED_BY.SetText("");
	_UPDATED_DATE.SetText("");
	_IS_ACTIVE.SetText("");
	_STATUS.SetText("");
	validatereset();
}

function ClearSearch() {
    DoSearch();
}

function ActiveForm(active) {
	_WORKING_DATE.SetEnabled(active);
	_SHIFT.SetEnabled(active);
	_SUPPLIER_NAME.SetEnabled(active);
	_SUPPLIER_CODE.SetEnabled(active);
	_ORDER_NO.SetEnabled(active);
	_ORDER_DATETIME.SetEnabled(active);
	_TRIP_NO.SetEnabled(active);
	_TRUCK_NO.SetEnabled(active);
	_EST_ARRIVAL_DATETIME.SetEnabled(active);
	_CREATED_BY.SetEnabled(active);
	_CREATED_DATE.SetEnabled(active);
	_UPDATED_BY.SetEnabled(active);
	_UPDATED_DATE.SetEnabled(active);
	_IS_ACTIVE.SetEnabled(active);
	_STATUS.SetEnabled(active);
	
}

function GetObject() {
    return {
		ID: _id, 
		WORKING_DATE: _WORKING_DATE.GetText().trim(), 
		SHIFT: _SHIFT.GetText().trim(), 
		SUPPLIER_NAME: _SUPPLIER_NAME.GetText().trim(), 
		SUPPLIER_CODE: _SUPPLIER_CODE.GetText().trim(), 
		ORDER_NO: _ORDER_NO.GetText().trim(), 
		ORDER_DATETIME: _ORDER_DATETIME.GetText().trim(), 
		TRIP_NO: _TRIP_NO.GetText().trim(), 
		TRUCK_NO: _TRUCK_NO.GetText().trim(), 
		EST_ARRIVAL_DATETIME: _EST_ARRIVAL_DATETIME.GetText().trim(), 
		CREATED_BY: _CREATED_BY.GetText().trim(), 
		CREATED_DATE: _CREATED_DATE.GetText().trim(), 
		UPDATED_BY: _UPDATED_BY.GetText().trim(), 
		UPDATED_DATE: _UPDATED_DATE.GetText().trim(), 
		IS_ACTIVE: _IS_ACTIVE.GetText().trim(), 
		STATUS: _STATUS.GetText().trim() 
	 
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







