var _id = -1;
var _searchTimeout;
var _searchXhr;

function Add() {
    OnPopTB_M_USER_ROLESCloseUp();
    popTB_M_USER_ROLES.Show();
    return false;
}

function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_M_USER_ROLES/TB_M_USER_ROLES_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_M_USER_ROLES']").html("Edit TB_M_USER_ROLES");
         
			_USER_CC.SetText(response.USER_CC);
			_TEAM_ID.SetText(response.TEAM_ID);
			_SHIFT.SetText(response.SHIFT);
			_CREATE_DATE.SetText(response.CREATE_DATE_Str_DDMMYYYY);
			_UPDATE_DATE.SetText(response.UPDATE_DATE_Str_DDMMYYYY);
		   
    }).fail(function () {
        OnPopTB_M_USER_ROLESCloseUp();
    })
    popTB_M_USER_ROLES.Show();
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
        url: baseUrl + "/TB_M_USER_ROLES/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_M_USER_ROLES.Hide();
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
    popTB_M_USER_ROLES.Hide()
}

function OnPopTB_M_USER_ROLESCloseUp() {
	_id = -1
	$("span[id^='popTB_M_USER_ROLES']").html("Add New TB_M_USER_ROLES")
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
        url: baseUrl + "/TB_M_USER_ROLES/Delete",
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
        url: baseUrl + "/TB_M_USER_ROLES/SetObjectInfo",
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
	_USER_CC.SetText("");
	_TEAM_ID.SetText("");
	_SHIFT.SetText("");
	_CREATE_DATE.SetText("");
	_UPDATE_DATE.SetText("");
	validatereset();
}

function ClearSearch() {
    DoSearch();
}

function ActiveForm(active) {
	_USER_CC.SetEnabled(active);
	_TEAM_ID.SetEnabled(active);
	_SHIFT.SetEnabled(active);
	_CREATE_DATE.SetEnabled(active);
	_UPDATE_DATE.SetEnabled(active);
	
}

function GetObject() {
    return {
		ID: _id, 
		USER_CC: _USER_CC.GetText().trim(), 
		TEAM_ID: _TEAM_ID.GetText().trim(), 
		SHIFT: _SHIFT.GetText().trim(), 
		CREATE_DATE: _CREATE_DATE.GetText().trim(), 
		UPDATE_DATE: _UPDATE_DATE.GetText().trim() 
	 
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

