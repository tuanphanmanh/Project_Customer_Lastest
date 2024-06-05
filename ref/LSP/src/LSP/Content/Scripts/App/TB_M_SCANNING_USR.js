var _id = -1;
var _searchTimeout;
var _searchXhr;

function Add() {
    OnPopTB_M_SCANNING_USRCloseUp();
    popTB_M_SCANNING_USR.Show();
    return false;
}

function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_M_SCANNING_USR/TB_M_SCANNING_USR_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_M_SCANNING_USR']").html("Edit TB_M_SCANNING_USR");

			_USER_ID.SetText(response.USER_ID);
			_USER_NAME.SetText(response.USER_NAME);
			_IS_ACTIVE.SetChecked((response.IS_ACTIVE == "Y") ? 1 : 0);
		   
    }).fail(function () {
        OnPopTB_M_SCANNING_USRCloseUp();
    })
    popTB_M_SCANNING_USR.Show();
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
    $.ajax({
        url: baseUrl + "/TB_M_SCANNING_USR/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_M_SCANNING_USR.Hide();
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
    popTB_M_SCANNING_USR.Hide()
}

function OnPopTB_M_SCANNING_USRCloseUp() {
	_id = -1
	$("span[id^='popTB_M_SCANNING_USR']").html("Add New TB_M_SCANNING_USR")
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
        url: baseUrl + "/TB_M_SCANNING_USR/Delete",
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
        url: baseUrl + "/TB_M_SCANNING_USR/SetObjectInfo",
        method: "post",
        data: {
            USER_ID: _txtUSER_ID.GetText(),
            USER_NAME: _txtUSER_NAME.GetText()
        }
    }).done(function (response) {
        gvList.PerformCallback();
    });
}

function ClearForm() {
	_USER_ID.SetText("");
	_USER_NAME.SetText("");
	_IS_ACTIVE.SetChecked(1);
	validatereset();
}

function ClearSearch() {
    DoSearch();
}

function ActiveForm(active) {
	_USER_ID.SetEnabled(active);
	_USER_NAME.SetEnabled(active);
	_IS_ACTIVE.SetEnabled(active);
	
}

function GetObject() {
    return {
		id: _id, 
		USER_ID: _USER_ID.GetText().trim(), 
		USER_NAME: _USER_NAME.GetText().trim(), 
		IS_ACTIVE: (_IS_ACTIVE.GetChecked()) ? "Y" : "N"
    }
}

function Validate() {
	return doValidate();
}

