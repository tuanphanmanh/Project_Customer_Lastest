var _id = -1;
var _searchTimeout;
var _searchXhr;

function Add() {
    OnPopTB_R_KANBANCloseUp();
    popTB_R_KANBAN.Show();
    return false;
}

function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_R_KANBAN/TB_R_KANBAN_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_R_KANBAN']").html("Edit TB_R_KANBAN");
         
			_CONTENT_LIST_ID.SetText(response.CONTENT_LIST_ID);
			_BACK_NO.SetText(response.BACK_NO);
			_PART_NO.SetText(response.PART_NO);
			_COLOR_SFX.SetText(response.COLOR_SFX);
			_PART_NAME.SetText(response.PART_NAME);
			_BOX_SIZE.SetText(response.BOX_SIZE);
			_BOX_QTY.SetText(response.BOX_QTY);
			_PC_ADDRESS.SetText(response.PC_ADDRESS);
			_WH_SPS_PICKING.SetText(response.WH_SPS_PICKING);
			_IS_ACTIVE.SetText(response.IS_ACTIVE);
		   
    }).fail(function () {
        OnPopTB_R_KANBANCloseUp();
    })
    popTB_R_KANBAN.Show();
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
        url: baseUrl + "/TB_R_KANBAN/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_R_KANBAN.Hide();
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
    popTB_R_KANBAN.Hide()
}

function OnPopTB_R_KANBANCloseUp() {
	_id = -1
	$("span[id^='popTB_R_KANBAN']").html("Add New TB_R_KANBAN")
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
        url: baseUrl + "/TB_R_KANBAN/Delete",
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
        url: baseUrl + "/TB_R_KANBAN/SetObjectInfo",
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
	_CONTENT_LIST_ID.SetText("");
	_BACK_NO.SetText("");
	_PART_NO.SetText("");
	_COLOR_SFX.SetText("");
	_PART_NAME.SetText("");
	_BOX_SIZE.SetText("");
	_BOX_QTY.SetText("");
	_PC_ADDRESS.SetText("");
	_WH_SPS_PICKING.SetText("");
	_IS_ACTIVE.SetText("");
	validatereset();
}

function ClearSearch() {
    DoSearch();
}

function ActiveForm(active) {
	_CONTENT_LIST_ID.SetEnabled(active);
	_BACK_NO.SetEnabled(active);
	_PART_NO.SetEnabled(active);
	_COLOR_SFX.SetEnabled(active);
	_PART_NAME.SetEnabled(active);
	_BOX_SIZE.SetEnabled(active);
	_BOX_QTY.SetEnabled(active);
	_PC_ADDRESS.SetEnabled(active);
	_WH_SPS_PICKING.SetEnabled(active);
	_IS_ACTIVE.SetEnabled(active);
	
}

function GetObject() {
    return {
		ID: _id, 
		CONTENT_LIST_ID: _CONTENT_LIST_ID.GetText().trim(), 
		BACK_NO: _BACK_NO.GetText().trim(), 
		PART_NO: _PART_NO.GetText().trim(), 
		COLOR_SFX: _COLOR_SFX.GetText().trim(), 
		PART_NAME: _PART_NAME.GetText().trim(), 
		BOX_SIZE: _BOX_SIZE.GetText().trim(), 
		BOX_QTY: _BOX_QTY.GetText().trim(), 
		PC_ADDRESS: _PC_ADDRESS.GetText().trim(), 
		WH_SPS_PICKING: _WH_SPS_PICKING.GetText().trim(), 
		IS_ACTIVE: _IS_ACTIVE.GetText().trim() 
	 
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