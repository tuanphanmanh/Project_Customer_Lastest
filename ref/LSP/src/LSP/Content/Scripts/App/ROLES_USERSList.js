var _id = -1;
var _idUserRole = -1;
var _searchTimeout;
var _searchXhr;

function Add() {
    OnPopTB_M_USERSCloseUp();
    popTB_M_USERS.Show();
    return false;
}

//Addnew Users Role 
function AssignCC(u_id, u_name) { 
    OnPopTB_M_USER_ROLESCloseUp(); 
    popTB_M_USER_ROLES.Show(); 
    _R_USER_NAME.SetText(u_name); 
    return false;
}


function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_M_USERS/TB_M_USERS_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_M_USERS']").html("Edit TB_M_USERS");
         
			_USER_CC.SetText(response.USER_CC);
			_USER_NAME.SetText(response.USER_NAME);
			_ACTIVE.SetChecked(response.ACTIVE);
		   
    }).fail(function () {
        OnPopTB_M_USERSCloseUp();
    })
    popTB_M_USERS.Show();
    return false;
}

function OnBtnUpdateClicked(s) {
    var $btn = $(s);
    if ($btn.hasClass("saving")) {
        return;
    }

    if (!ValidateParam("users")) {
        return;
    }
    ActiveForm(false);
    $btn.addClass("saving");
    //$("#dealerStatus").html("Saving...");
    $.ajax({
        url: baseUrl + "/TB_M_USERS/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_M_USERS.Hide();
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


//Users Role
function OnBtnUpdateUserRoleClicked(s) {
    var $btn = $(s);
    if ($btn.hasClass("saving")) {
        return;
    }
    if (!ValidateParam("user_role")) {
        return;
    } 
    ActiveFormUserRoles(false);
    $btn.addClass("saving");
    //$("#dealerStatus").html("Saving...");
    $.ajax({
        url: baseUrl + "/TB_M_USERS/SaveUserRoleData",
        method: "post",
        data: GetObjectUserRole(),
        dataType: "json"
    }).done(function (response) {
        ActiveFormUserRoles(true);
        if (response.success) {
            popTB_M_USER_ROLES.Hide();
            gvRolesUsersList.PerformCallback();
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
    popTB_M_USERS.Hide()
}

//Users Role
function OnBtnCancelUserRoleClicked() {
    popTB_M_USER_ROLES.Hide()
}


function OnPopTB_M_USERSCloseUp() {
	_id = -1
	$("span[id^='popTB_M_USERS']").html("Add New TB_M_USERS")
	ClearForm();
}

//Close Pop Users Role
function OnPopTB_M_USER_ROLESCloseUp() {
    _idUserRole = -1
    $("span[id^='popTB_M_USER_ROLES']").html("Add New TB_M_USER_ROLES");
	ClearFormUserRoles();
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
        url: baseUrl + "/TB_M_USERS/Delete",
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

function Delete_USER_ROLES(s) {
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
        url: baseUrl + "/TB_M_USERS/Delete_USER_ROLES",
        method: "post",
        dataType: "json",
        data: {
            sid: $btn.attr("data-id")
        }
    }).done(function (response) {
        if (response.success) {
            gvRolesUsersList.PerformCallback();
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
        url: baseUrl + "/TB_M_USERS/SetObjectInfo",
        method: "post",
        data: {
            //UserName: _UserName.GetText(),
			//Password: _Password.GetText(),
        }
    }).done(function (response) {
        gvRolesUsersList.PerformCallback();
    });
}

function ClearForm() { 
	_USER_NAME.SetText("");
	_ACTIVE.SetChecked(true);
	validatereset();
}

//Clear Form User Roles
function ClearFormUserRoles() { 
    _R_USER_NAME.SetText(""); 
    _TEAM_ID.SelectIndex(0);
    _SHIFT.SelectIndex(0); 
	validatereset();
}


function ClearSearch() {
    DoSearch();
}

function ActiveForm(active) { 
	_USER_NAME.SetEnabled(active);
	_ACTIVE.SetEnabled(active);
	  
}

function ActiveFormUserRoles(active) {
 
    _R_USER_NAME.SetEnabled(active);
    _TEAM_ID.SetEnabled(active);
    _SHIFT.SetEnabled(active);

}

function GetObject() {
    return {
		ID: _id,  
		USER_NAME: _USER_NAME.GetText().trim(), 
		ACTIVE: _ACTIVE.GetChecked() 
    }
}

function GetObjectUserRole() {
    return {
        ID: _idUserRole,
        USER_NAME: _R_USER_NAME.GetText().trim(),
        TEAM_ID: _TEAM_ID.GetValue().trim(),
        SHIFT: _SHIFT.GetValue().trim()
    }
}



function Validate() {

	return doValidate();
}

function ValidateParam(csskey) {

    return doValidateParam(csskey);
}

