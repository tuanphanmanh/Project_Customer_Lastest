var _id = -1;
var _searchTimeout;
var _searchXhr;

function Add() {
    OnPopTB_R_UNLOADING_PLANCloseUp();
    popTB_R_UNLOADING_PLAN.Show();
    ActiveForm(true);
    return false;
}

function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_R_UNLOADING_PLAN/TB_R_UNLOADING_PLAN_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_R_UNLOADING_PLAN']").html("Edit UNLOADING PLAN");
         
        _DOCK.SetText(response.DOCK);
        _DOCK.SetEnabled(false);
        _TRUCK.SetText(response.TRUCK);
        _TRUCK.SetEnabled(false);
        _SUPPLIERS.SetText(response.SUPPLIERS);
        //_SUPPLIERS.SetEnabled(false);

        _SUPPLIERS_RETURN.SetText(response.SUPPLIERS_RETURN);
        //_SUPPLIERS_RETURN.SetEnabled(false);

        _WORKING_DATE.SetText(response.WORKING_DATE_Str_DDMMYYYY);
        _WORKING_DATE.SetEnabled(false);
        _SHIFT.SetText(response.SHIFT);
        _SHIFT.SetEnabled(false);
        _SEQUENCE_NO.SetText(response.SEQUENCE_NO);
        _SEQUENCE_NO.SetEnabled(false);
        _PLAN_START_UP_DATETIME.SetText(response.PLAN_START_UP_DATETIME_Str_DDMMYYYY);
        _PLAN_START_UP_DATETIME.SetEnabled(false);
        _PLAN_FINISH_UP_DATETIME.SetText(response.PLAN_FINISH_UP_DATETIME_Str_DDMMYYYY);
        _PLAN_FINISH_UP_DATETIME.SetEnabled(false);
        _ACTUAL_START_UP_DATETIME.SetText(response.ACTUAL_START_UP_DATETIME_Str_DDMMYYYY);
        _ACTUAL_START_UP_DATETIME.SetEnabled(false);
        _ACTUAL_FINISH_UP_DATETIME.SetText(response.ACTUAL_FINISH_UP_DATETIME_Str_DDMMYYYY);
        _ACTUAL_FINISH_UP_DATETIME.SetEnabled(false);
		_REVISED_PLAN_START_UP_DATETIME.SetText(response.REVISED_PLAN_START_UP_DATETIME_Str_DDMMYYYY);
		_REVISED_PLAN_FINISH_UP_DATETIME.SetText(response.REVISED_PLAN_FINISH_UP_DATETIME_Str_DDMMYYYY);
		_ACTUAL_START_UP_DELAY.SetText(response.ACTUAL_START_UP_DELAY);
		_ACTUAL_START_UP_DELAY.SetEnabled(false);
		_ACTUAL_FINISH_UP_DELAY.SetText(response.ACTUAL_FINISH_UP_DELAY);
		_ACTUAL_FINISH_UP_DELAY.SetEnabled(false);
		_STATUS.SetText(response.STATUS);
		_STATUS.SetEnabled(false);
		_ISSUES.SetText(response.ISSUES);
		_CAUSE.SetText(response.CAUSE);
		_COUTERMEASURE.SetText(response.COUTERMEASURE);
		_PIC_RECORDER.SetText(response.PIC_RECORDER);
		_PIC_ACTION.SetText(response.PIC_ACTION);
		_ACTION_DUEDATE.SetText(response.ACTION_DUEDATE_Str_DDMMYYYY);
		_RESULT.SetText(response.RESULT);
	 
		_IS_ACTIVE.SetChecked((response.IS_ACTIVE == "Y") ? 1 : 0);
		   
    }).fail(function () {
        OnPopTB_R_UNLOADING_PLANCloseUp();
    })
    popTB_R_UNLOADING_PLAN.Show();
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
        url: baseUrl + "/TB_R_UNLOADING_PLAN/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_R_UNLOADING_PLAN.Hide();
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
    popTB_R_UNLOADING_PLAN.Hide()
}

function OnPopTB_R_UNLOADING_PLANCloseUp() {
	_id = -1
	$("span[id^='popTB_R_UNLOADING_PLAN']").html("Add UNLOADING PLAN")
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
        url: baseUrl + "/TB_R_UNLOADING_PLAN/Delete",
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

// Reset Actual
function ResetActual(s) {

    if (gvList.GetFocusedRowIndex() >= 0) {

        //Re-confirm want to do
        if (!confirm("Có chắc muốn xóa thực tế Nhận?"))
            return;

        LoadingPanel.Show();

        $.ajax({
            url: baseUrl + "/TB_R_UNLOADING_PLAN/ResetActual",
            method: "post",
            dataType: "json",
            data: {
                sid: gvList.GetRowKey(gvList.GetFocusedRowIndex())
               
            }
        }).done(function (response) {

            if (response.success) {
                msgOk("Xóa thành công!");
                gvList.PerformCallback();
            }
            else {
                msgError("Có Lỗi, Hệ thống không thể cập nhật. <br/> chú ý: chỉ có thể xóa với xe trong ngày làm việc hiện tại.");
            }

        }).always(function () {

            LoadingPanel.Hide();
        });
    } else {
        msgError('Chưa chọn dòng dữ liệu!');
    }
    return false;
}


function Search() {
    _searchXhr = $.ajax({
        url: baseUrl + "/TB_R_UNLOADING_PLAN/SetObjectInfo",
        method: "post",
        data: {
            WORKING_DATE: ConvertDate(_txtWORKING_DATE.GetText().trim()),
            WORKING_DATE_FROM: ConvertDate(_txtWORKING_DATE_FROM.GetText().trim())
        }
    }).done(function (response) {
        gvList.PerformCallback();
    });
}

function ClearForm() {
	_DOCK.SetText("");
	_TRUCK.SetText("");
	_SUPPLIERS.SetText("");
	_SUPPLIERS_RETURN.SetText("");
	_WORKING_DATE.SetText("");
	_SHIFT.SetText("");
	_SEQUENCE_NO.SetText("");
	_PLAN_START_UP_DATETIME.SetText("");
	_PLAN_FINISH_UP_DATETIME.SetText("");
	_ACTUAL_START_UP_DATETIME.SetText("");
	_ACTUAL_FINISH_UP_DATETIME.SetText("");
	_REVISED_PLAN_START_UP_DATETIME.SetText("");
	_REVISED_PLAN_FINISH_UP_DATETIME.SetText("");
	_ACTUAL_START_UP_DELAY.SetText("");
	_ACTUAL_FINISH_UP_DELAY.SetText("");
	_STATUS.SetText("");
	_ISSUES.SetText("");
	_CAUSE.SetText("");
	_COUTERMEASURE.SetText("");
	_PIC_RECORDER.SetText("");
	_PIC_ACTION.SetText("");
	_ACTION_DUEDATE.SetText("");
	_RESULT.SetText("");
 
	_IS_ACTIVE.SetChecked(1);
	validatereset();
}

function ClearSearch() {
    DoSearch();
}

function ActiveForm(active) {
	_DOCK.SetEnabled(active);
	_TRUCK.SetEnabled(active);
	_SUPPLIERS.SetEnabled(active);
	_SUPPLIERS_RETURN.SetEnabled(active);
	_WORKING_DATE.SetEnabled(active);
	_SHIFT.SetEnabled(active);
	_SEQUENCE_NO.SetEnabled(active);
	_PLAN_START_UP_DATETIME.SetEnabled(active);
	_PLAN_FINISH_UP_DATETIME.SetEnabled(active);
	_ACTUAL_START_UP_DATETIME.SetEnabled(active);
	_ACTUAL_FINISH_UP_DATETIME.SetEnabled(active);
	_REVISED_PLAN_START_UP_DATETIME.SetEnabled(active);
	_REVISED_PLAN_FINISH_UP_DATETIME.SetEnabled(active);
	_ACTUAL_START_UP_DELAY.SetEnabled(active);
	_ACTUAL_FINISH_UP_DELAY.SetEnabled(active);
	_STATUS.SetEnabled(active);
	_ISSUES.SetEnabled(active);
	_CAUSE.SetEnabled(active);
	_COUTERMEASURE.SetEnabled(active);
	_PIC_RECORDER.SetEnabled(active);
	_PIC_ACTION.SetEnabled(active);
	_ACTION_DUEDATE.SetEnabled(active);
	_RESULT.SetEnabled(active);
 
	_IS_ACTIVE.SetEnabled(active);
	
}

function GetObject() {
 
    return {
		ID: _id, 
		DOCK: _DOCK.GetText().trim(), 
		TRUCK: _TRUCK.GetText().trim(), 
		SUPPLIERS: _SUPPLIERS.GetText().trim(),
		SUPPLIERS_RETURN: _SUPPLIERS_RETURN.GetText().trim(),        
		WORKING_DATE: ConvertDate(_WORKING_DATE.GetText().trim()), 
		SHIFT: _SHIFT.GetText().trim(), 
		SEQUENCE_NO: _SEQUENCE_NO.GetText().trim(), 
		PLAN_START_UP_DATETIME: ConvertDateTime(_PLAN_START_UP_DATETIME.GetText().trim()),
		PLAN_FINISH_UP_DATETIME: ConvertDateTime(_PLAN_FINISH_UP_DATETIME.GetText().trim()),
		ACTUAL_START_UP_DATETIME: ConvertDateTime(_ACTUAL_START_UP_DATETIME.GetText().trim()),
		ACTUAL_FINISH_UP_DATETIME: ConvertDateTime(_ACTUAL_FINISH_UP_DATETIME.GetText().trim()),
		REVISED_PLAN_START_UP_DATETIME: ConvertDateTime(_REVISED_PLAN_START_UP_DATETIME.GetText().trim()),
		REVISED_PLAN_FINISH_UP_DATETIME: ConvertDateTime(_REVISED_PLAN_FINISH_UP_DATETIME.GetText().trim()),
		ACTUAL_START_UP_DELAY: _ACTUAL_START_UP_DELAY.GetText().trim(), 
		ACTUAL_FINISH_UP_DELAY: _ACTUAL_FINISH_UP_DELAY.GetText().trim(), 
		STATUS: _STATUS.GetText().trim(), 
		ISSUES: _ISSUES.GetText().trim(), 
		CAUSE: _CAUSE.GetText().trim(), 
		COUTERMEASURE: _COUTERMEASURE.GetText().trim(), 
		PIC_RECORDER: _PIC_RECORDER.GetText().trim(), 
		PIC_ACTION: _PIC_ACTION.GetText().trim(), 
        ACTION_DUEDATE: ConvertDateTime(_ACTION_DUEDATE.GetText().trim()), 
		RESULT: _RESULT.GetText().trim(), 
		 
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

function ConvertDateTime(_date) {
    //dd/MM/yyy -> MM/dd/yyyy
    if (_date == null || _date == "") {
        return "";
    }
    var spec = " ";
    var _datetimes = _date.split(spec);
    var _times = "";
    if (_datetimes.length == 2) {
        _times = " " + _datetimes[1];
    }

    spec = "/";
    var item = _datetimes[0].split(spec);
    if (item.length != 3) {
        return _date;
    }

    return item[1] + spec + item[0] + spec + item[2] + _times;
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







