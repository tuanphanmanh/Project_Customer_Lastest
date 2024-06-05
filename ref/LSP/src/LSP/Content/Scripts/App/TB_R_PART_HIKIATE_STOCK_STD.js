var _id = -1;
var _searchTimeout;
var _searchXhr;

function Add() {
    OnPopTB_R_PART_HIKIATE_STOCK_STDCloseUp();
    popTB_R_PART_HIKIATE_STOCK_STD.Show();
    return false;
}

function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_R_PART_HIKIATE_STOCK_STD/TB_R_PART_HIKIATE_STOCK_STD_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_R_PART_HIKIATE_STOCK_STD']").html("Edit TB_R_PART_HIKIATE_STOCK_STD");
         
			_PART_ID.SetText(response.PART_ID);
			_MIN_STOCK.SetText(response.MIN_STOCK);
			_MAX_STOCK.SetText(response.MAX_STOCK);
			_TC_FROM.SetText(response.TC_FROM_Str_DDMMYYYY);
			_TC_TO.SetText(response.TC_TO_Str_DDMMYYYY);
			_IS_ACTIVE.SetText(response.IS_ACTIVE);
			_CREATED_BY.SetText(response.CREATED_BY);
			_CREATED_DATE.SetText(response.CREATED_DATE_Str_DDMMYYYY);
			_UPDATED_BY.SetText(response.UPDATED_BY);
			_UPDATED_DATE.SetText(response.UPDATED_DATE_Str_DDMMYYYY);
		   
    }).fail(function () {
        OnPopTB_R_PART_HIKIATE_STOCK_STDCloseUp();
    })
    popTB_R_PART_HIKIATE_STOCK_STD.Show();
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
        url: baseUrl + "/TB_R_PART_HIKIATE_STOCK_STD/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_R_PART_HIKIATE_STOCK_STD.Hide();
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
    popTB_R_PART_HIKIATE_STOCK_STD.Hide()
}

function OnPopTB_R_PART_HIKIATE_STOCK_STDCloseUp() {
	_id = -1
	$("span[id^='popTB_R_PART_HIKIATE_STOCK_STD']").html("Add New TB_R_PART_HIKIATE_STOCK_STD")
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
        url: baseUrl + "/TB_R_PART_HIKIATE_STOCK_STD/Delete",
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
        url: baseUrl + "/TB_R_PART_HIKIATE_STOCK_STD/SetObjectInfo",
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
	_PART_ID.SetText("");
	_MIN_STOCK.SetText("");
	_MAX_STOCK.SetText("");
	_TC_FROM.SetText("");
	_TC_TO.SetText("");
	_IS_ACTIVE.SetText("");
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
	_PART_ID.SetEnabled(active);
	_MIN_STOCK.SetEnabled(active);
	_MAX_STOCK.SetEnabled(active);
	_TC_FROM.SetEnabled(active);
	_TC_TO.SetEnabled(active);
	_IS_ACTIVE.SetEnabled(active);
	_CREATED_BY.SetEnabled(active);
	_CREATED_DATE.SetEnabled(active);
	_UPDATED_BY.SetEnabled(active);
	_UPDATED_DATE.SetEnabled(active);
	
}

function GetObject() {
    return {
		ID: _id, 
		PART_ID: _PART_ID.GetText().trim(), 
		MIN_STOCK: _MIN_STOCK.GetText().trim(), 
		MAX_STOCK: _MAX_STOCK.GetText().trim(), 
		TC_FROM: _TC_FROM.GetText().trim(), 
		TC_TO: _TC_TO.GetText().trim(), 
		IS_ACTIVE: _IS_ACTIVE.GetText().trim(), 
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



