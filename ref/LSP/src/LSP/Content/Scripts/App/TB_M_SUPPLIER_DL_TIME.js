var _id = -1;
var _searchTimeout;
var _searchXhr;

function Add() {
    OnPopTB_M_SUPPLIER_DL_TIMECloseUp();
    popTB_M_SUPPLIER_DL_TIME.Show();
    return false;
}

function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_M_SUPPLIER_DL_TIME/TB_M_SUPPLIER_DL_TIME_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_M_SUPPLIER_DL_TIME']").html("Edit TB_M_SUPPLIER_DL_TIME");
         
			_SUPPLIER_ID.SetText(response.SUPPLIER_ID);
			_DELIVERY_SEQ.SetText(response.DELIVERY_SEQ);

			var d = new Date(new Date().toDateString() + ' ' + jsonTimeToString(response.DELIVERY_TIME));
			_DELIVERY_TIME.SetValue(d);
			_IS_ACTIVE.SetChecked((response.IS_ACTIVE == "Y") ? 1 : 0);
		   
    }).fail(function () {
        OnPopTB_M_SUPPLIER_DL_TIMECloseUp();
    })
    popTB_M_SUPPLIER_DL_TIME.Show();
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
    $.ajax({
        url: baseUrl + "/TB_M_SUPPLIER_DL_TIME/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_M_SUPPLIER_DL_TIME.Hide();
            DoSearch();
        }
        else {
            alert(response.message);
        }
    }).always(function () {
        $btn.removeClass("saving");
    });
}

function OnBtnCancelClicked() {
    popTB_M_SUPPLIER_DL_TIME.Hide()
}

function OnPopTB_M_SUPPLIER_DL_TIMECloseUp() {
	_id = -1
	$("span[id^='popTB_M_SUPPLIER_DL_TIME']").html("Add New TB_M_SUPPLIER_DL_TIME")
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
        url: baseUrl + "/TB_M_SUPPLIER_DL_TIME/Delete",
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
        url: baseUrl + "/TB_M_SUPPLIER_DL_TIME/SetObjectInfo",
        method: "post",
        data: {
            SUPPLIER_ID: _txtSUPPLIER_ID.GetText(),
            DELIVERY_SEQ: _txtDELIVERY_SEQ.GetText()
        }
    }).done(function (response) {
        gvList.PerformCallback();
    });
}

function ClearForm() {
	_SUPPLIER_ID.SetText("");
	_DELIVERY_SEQ.SetText("");
	_DELIVERY_TIME.SetText("");
	_IS_ACTIVE.SetChecked(1);
	validatereset();
}

function ActiveForm(active) {
	_SUPPLIER_ID.SetEnabled(active);
	_DELIVERY_SEQ.SetEnabled(active);
	_DELIVERY_TIME.SetEnabled(active);
	_IS_ACTIVE.SetEnabled(active);
}

function GetObject() {
    return {
		ID: _id, 
		SUPPLIER_ID: _SUPPLIER_ID.GetText().trim(), 
		DELIVERY_SEQ: _DELIVERY_SEQ.GetText().trim(), 
		DELIVERY_TIME: _DELIVERY_TIME.GetText().trim(), 
		IS_ACTIVE: (_IS_ACTIVE.GetChecked()) ? "Y" : "N"
    }
}

function Validate() {
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