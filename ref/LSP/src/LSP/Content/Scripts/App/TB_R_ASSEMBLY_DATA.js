var _id = -1;
var _searchTimeout;
var _searchXhr;

function Add() {
    OnPopTB_R_ASSEMBLY_DATACloseUp();
    popTB_R_ASSEMBLY_DATA.Show();
    return false;
}

function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_R_ASSEMBLY_DATA/TB_R_ASSEMBLY_DATA_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_R_ASSEMBLY_DATA']").html("Edit TB_R_ASSEMBLY_DATA");
         
			_LINE.SetText(response.LINE);
			_PROCESS.SetText(response.PROCESS);
			_MODEL.SetText(response.MODEL);
			_BODY_NO.SetText(response.BODY_NO);
			_SEQ_NO.SetText(response.SEQ_NO);
			_GRADE.SetText(response.GRADE);
			_LOT_NO.SetText(response.LOT_NO);
			_NO_IN_LOT.SetText(response.NO_IN_LOT);
			_COLOR.SetText(response.COLOR);
			_WORKING_DATE.SetText(response.WORKING_DATE_Str_DDMMYYYY);
			_NO_IN_DATE.SetText(response.NO_IN_DATE);
			_A_IN_DATE_PLAN.SetText(response.A_IN_DATE_PLAN_Str_DDMMYYYY);
			_A_IN_TIME_PLAN.SetText(response.A_IN_TIME_PLAN);
			_A_IN_DATE_ACTUAL.SetText(response.A_IN_DATE_ACTUAL_Str_DDMMYYYY);
			_A_IN_TIME_ACTUAL.SetText(response.A_IN_TIME_ACTUAL);
			_CREATED_BY.SetText(response.CREATED_BY);
			_CREATED_DATE.SetText(response.CREATED_DATE_Str_DDMMYYYY);
			_UPDATED_BY.SetText(response.UPDATED_BY);
			_UPDATED_DATE.SetText(response.UPDATED_DATE_Str_DDMMYYYY);
		   
    }).fail(function () {
        OnPopTB_R_ASSEMBLY_DATACloseUp();
    })
    popTB_R_ASSEMBLY_DATA.Show();
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
        url: baseUrl + "/TB_R_ASSEMBLY_DATA/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_R_ASSEMBLY_DATA.Hide();
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
    popTB_R_ASSEMBLY_DATA.Hide()
}

function OnPopTB_R_ASSEMBLY_DATACloseUp() {
	_id = -1
	$("span[id^='popTB_R_ASSEMBLY_DATA']").html("Add New TB_R_ASSEMBLY_DATA")
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
        url: baseUrl + "/TB_R_ASSEMBLY_DATA/Delete",
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
        url: baseUrl + "/TB_R_ASSEMBLY_DATA/SetObjectInfo",
        method: "post",
        data: {
            LINE: _txtLINE.GetText().trim(),
            WORKING_DATE: ConvertDate(_txtWORKING_DATE.GetText().trim())
        }
    }).done(function (response) {
        gvList.PerformCallback();
    });
}

function ClearForm() {
	_LINE.SetText("");
	_PROCESS.SetText("");
	_MODEL.SetText("");
	_BODY_NO.SetText("");
	_SEQ_NO.SetText("");
	_GRADE.SetText("");
	_LOT_NO.SetText("");
	_NO_IN_LOT.SetText("");
	_COLOR.SetText("");
	_WORKING_DATE.SetText("");
	_NO_IN_DATE.SetText("");
	_A_IN_DATE_PLAN.SetText("");
	_A_IN_TIME_PLAN.SetText("");
	_A_IN_DATE_ACTUAL.SetText("");
	_A_IN_TIME_ACTUAL.SetText("");
	_CREATED_BY.SetText("");
	_CREATED_DATE.SetText("");
	_UPDATED_BY.SetText("");
	_UPDATED_DATE.SetText("");
	validatereset();
}

function ClearSearch() {
    DoSearch();
}

function ActiveForm(active) {
	_LINE.SetEnabled(active);
	_PROCESS.SetEnabled(active);
	_MODEL.SetEnabled(active);
	_BODY_NO.SetEnabled(active);
	_SEQ_NO.SetEnabled(active);
	_GRADE.SetEnabled(active);
	_LOT_NO.SetEnabled(active);
	_NO_IN_LOT.SetEnabled(active);
	_COLOR.SetEnabled(active);
	_WORKING_DATE.SetEnabled(active);
	_NO_IN_DATE.SetEnabled(active);
	_A_IN_DATE_PLAN.SetEnabled(active);
	_A_IN_TIME_PLAN.SetEnabled(active);
	_A_IN_DATE_ACTUAL.SetEnabled(active);
	_A_IN_TIME_ACTUAL.SetEnabled(active);
	_CREATED_BY.SetEnabled(active);
	_CREATED_DATE.SetEnabled(active);
	_UPDATED_BY.SetEnabled(active);
	_UPDATED_DATE.SetEnabled(active);
	
}

function GetObject() {
    return {
		ID: _id, 
		LINE: _LINE.GetText().trim(), 
		PROCESS: _PROCESS.GetText().trim(), 
		MODEL: _MODEL.GetText().trim(), 
		BODY_NO: _BODY_NO.GetText().trim(), 
		SEQ_NO: _SEQ_NO.GetText().trim(), 
		GRADE: _GRADE.GetText().trim(), 
		LOT_NO: _LOT_NO.GetText().trim(), 
		NO_IN_LOT: _NO_IN_LOT.GetText().trim(), 
		COLOR: _COLOR.GetText().trim(), 
		WORKING_DATE: _WORKING_DATE.GetText().trim(), 
		NO_IN_DATE: _NO_IN_DATE.GetText().trim(), 
		A_IN_DATE_PLAN: _A_IN_DATE_PLAN.GetText().trim(), 
		A_IN_TIME_PLAN: _A_IN_TIME_PLAN.GetText().trim(), 
		A_IN_DATE_ACTUAL: _A_IN_DATE_ACTUAL.GetText().trim(), 
		A_IN_TIME_ACTUAL: _A_IN_TIME_ACTUAL.GetText().trim(), 
		CREATED_BY: _CREATED_BY.GetText().trim(), 
		CREATED_DATE: _CREATED_DATE.GetText().trim(), 
		UPDATED_BY: _UPDATED_BY.GetText().trim(), 
		UPDATED_DATE: _UPDATED_DATE.GetText().trim() 
	 
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







