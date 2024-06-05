var _id = -1;
var _searchTimeout;
var _searchXhr;

function Add() {
    OnPopTB_M_TRUCK_SUPPLIERCloseUp();
    popTB_M_TRUCK_SUPPLIER.Show();
    return false;
}

function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_M_TRUCK_SUPPLIER/TB_M_TRUCK_SUPPLIER_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_M_TRUCK_SUPPLIER']").html("Edit TB_M_TRUCK_SUPPLIER");
         
            _SUPPLIER_ID.SetValue(response.SUPPLIER_ID);
			_TRUCK_NAME.SetText(response.TRUCK_NAME);
			_IS_ACTIVE.SetChecked((response.IS_ACTIVE == "Y") ? 1 : 0);
			 
		   
    }).fail(function () {
        OnPopTB_M_TRUCK_SUPPLIERCloseUp();
    })
    popTB_M_TRUCK_SUPPLIER.Show();
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
        url: baseUrl + "/TB_M_TRUCK_SUPPLIER/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_M_TRUCK_SUPPLIER.Hide();
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
    popTB_M_TRUCK_SUPPLIER.Hide()
}

function OnPopTB_M_TRUCK_SUPPLIERCloseUp() {
	_id = -1
	$("span[id^='popTB_M_TRUCK_SUPPLIER']").html("Add New TB_M_TRUCK_SUPPLIER")
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
        url: baseUrl + "/TB_M_TRUCK_SUPPLIER/Delete",
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
        url: baseUrl + "/TB_M_TRUCK_SUPPLIER/SetObjectInfo",
        method: "post",
        data: {
            SUPPLIER_CODE: _txtSUPPLIER_CODE.GetText(),
            TRUCK_NAME: _txtTRUCK_NAME.GetText()
        }
    }).done(function (response) {
        gvList.PerformCallback();
    });
}

function ClearForm() {
    _SUPPLIER_ID.SelectIndex(0);
	_TRUCK_NAME.SetText("");
	_IS_ACTIVE.SetChecked(1);
 
	validatereset();
}

function ClearSearch() {
    DoSearch();
}

function ActiveForm(active) {
	_SUPPLIER_ID.SetEnabled(active);
	_TRUCK_NAME.SetEnabled(active);
	_IS_ACTIVE.SetEnabled(active);
	 
	
}

function GetObject() {
    return {
		ID: _id, 
		SUPPLIER_ID: _SUPPLIER_ID.GetValue(),
		TRUCK_NAME: _TRUCK_NAME.GetText().trim(), 
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







