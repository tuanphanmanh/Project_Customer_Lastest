var _id = -1;
var _searchTimeout;
var _searchXhr;

function Add() {
    OnPopTB_M_ROUTINGCloseUp();
    popTB_M_ROUTING.Show();
    return false;
}

function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_M_ROUTING/TB_M_ROUTING_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_M_ROUTING']").html("Edit TB_M_ROUTING");
         
			_SUPPLIER_CODE.SetText(response.SUPPLIER_CODE);
			_DOCK.SetText(response.DOCK);
			_ADDRESS.SetText(response.ADDRESS);
			_ROUTING.SetText(response.ROUTING);
            
			var d = new Date(new Date().toDateString() + ' ' + jsonTimeToString(response.PICKING_TIME));
			_PICKING_TIME.SetValue(d);

			_TRUCK_NAME.SetText(response.TRUCK_NAME);
			_IS_ACTIVE.SetChecked((response.IS_ACTIVE == "Y") ? 1 : 0);
		   
    }).fail(function () {
        OnPopTB_M_ROUTINGCloseUp();
    })
    popTB_M_ROUTING.Show();
    return false;
}

function jsonTimeToString(dt) {
    if (dt != null) {
        var strtime = (((dt.Hours + "").length == 1) ? ("0" + dt.Hours) : dt.Hours) + ":" +
                            (((dt.Minutes + "").length == 1) ? ("0" + dt.Minutes) : dt.Minutes) + ":" +
                            (((dt.Seconds + "").length == 1) ? ("0" + dt.Seconds) : dt.Seconds)
        return strtime;
    } else { return ""; }
}


function getTime(dt) {
    var strtime = (((dt.getHours() + "").length == 1) ? ("0" + dt.getHours()) : dt.getHours()) + " : " +
                    (((dt.getMinutes() + "").length == 1) ? ("0" + dt.getMinutes()) : dt.getMinutes())
    return strtime;
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
        url: baseUrl + "/TB_M_ROUTING/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_M_ROUTING.Hide();
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
    popTB_M_ROUTING.Hide()
}

function OnPopTB_M_ROUTINGCloseUp() {
	_id = -1
	$("span[id^='popTB_M_ROUTING']").html("Add New TB_M_ROUTING")
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
        url: baseUrl + "/TB_M_ROUTING/Delete",
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

//Editer
function Search() {
    _searchXhr = $.ajax({
        url: baseUrl + "/TB_M_ROUTING/SetObjectInfo",
        method: "post",
        data: {
            SUPPLIER_CODE: _txtSUPPLIER_CODE.GetText(),
            DOCK: _txtDOCK.GetText(),
            ROUTING: _txtROUTING.GetText(),
            TRUCK_NAME: _txtTRUCK_NAME.GetText()
        }
    }).done(function (response) {
        gvList.PerformCallback();
    });
}

function ClearForm() {
	_SUPPLIER_CODE.SetText("");
	_DOCK.SetText("");
	_ADDRESS.SetText("");
	_ROUTING.SetText("");
	_PICKING_TIME.SetText("");
	_TRUCK_NAME.SetText("");
	_IS_ACTIVE.SetChecked(1);
	validatereset();
}

function ActiveForm(active) {
	_SUPPLIER_CODE.SetEnabled(active);
	_DOCK.SetEnabled(active);
	_ADDRESS.SetEnabled(active);
	_ROUTING.SetEnabled(active);
	_PICKING_TIME.SetEnabled(active);
	_TRUCK_NAME.SetEnabled(active);
	_IS_ACTIVE.SetEnabled(active);
	
}

function GetObject() {
    return {
		ID: _id, 
		SUPPLIER_CODE: _SUPPLIER_CODE.GetText().trim(), 
		DOCK: _DOCK.GetText().trim(), 
		ADDRESS: _ADDRESS.GetText().trim(), 
		ROUTING: _ROUTING.GetText().trim(), 
		PICKING_TIME: _PICKING_TIME.GetText().trim(), 
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