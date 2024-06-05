var _id = -1;
var _searchTimeout;
var _searchXhr;

function Add() {
    OnPopTB_M_MODELCloseUp();
    popTB_M_MODEL.Show();
    return false;
}

function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_M_MODEL/TB_M_MODEL_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_M_MODEL']").html("Edit TB_M_MODEL");
         
			_NAME.SetText(response.NAME);
			_ABBREVIATION.SetText(response.ABBREVIATION);
			_IS_ACTIVE.SetChecked((response.IS_ACTIVE == "Y") ? 1 : 0);
		   
    }).fail(function () {
        OnPopTB_M_MODELCloseUp();
    })
    popTB_M_MODEL.Show();
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
        url: baseUrl + "/TB_M_MODEL/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_M_MODEL.Hide();
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
    popTB_M_MODEL.Hide()
}

function OnPopTB_M_MODELCloseUp() {
	_id = -1
	$("span[id^='popTB_M_MODEL']").html("Add New TB_M_MODEL")
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
        url: baseUrl + "/TB_M_MODEL/Delete",
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
        url: baseUrl + "/TB_M_MODEL/SetObjectInfo",
        method: "post",
        data: {
            NAME: _txtNAME.GetText(),
            ABBREVIATION: _txtABBREVIATION.GetText()
        }
    }).done(function (response) {
        gvList.PerformCallback();
    });
}

function ClearForm() {
	_NAME.SetText("");
	_ABBREVIATION.SetText("");
	_IS_ACTIVE.SetChecked(1);
	 
	validatereset();
}

function ActiveForm(active) {
	_NAME.SetEnabled(active);
	_ABBREVIATION.SetEnabled(active);
	_IS_ACTIVE.SetEnabled(active);
}

function GetObject() {
    return {
		ID: _id, 
		NAME: _NAME.GetText().trim(), 
		ABBREVIATION: _ABBREVIATION.GetText().trim(), 
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


//VEHICLE MASTER
var _MODEL_ID = 0;
var _ID = 0;

function VEHICLE_New(t) {
    OnPopTB_M_VEHICLE_MASTERCloseUp();
    _MODEL_ID = $(t).attr("data-id");
    popTB_M_VEHICLE_MASTER.Show();
}

function OnPopTB_M_VEHICLE_MASTERCloseUp() {
    _MODEL_ID = 0;
    _ID = 0;
    $("span[id^='popTB_M_VEHICLE_MASTER']").html("Add new TB_M_VEHICLE_MASTER");
    ClearFormVEHICLE_MASTER();
}

function ClearFormVEHICLE_MASTER() {
    
    _MODEL_ID = 0;
    _ID = 0;

    _CFC.SetText("");
    _PROJECT_CODE.SetText("");
    _KATASHIKI.SetText("");
    _PROD_SFX.SetText("");
    _MKT_SFX.SetText("");
    _START_LOT.SetText("");
    _START_PROD_DATE.SetText("");
    _END_LOT.SetText("");
    _END_PROD_DATE.SetText("");
    _GRADE_MARK.SetText("");
    _IS_ACTIVE_V.SetText("");

    validatereset();
}


function OnBtnCancelVClicked() {
    popTB_M_VEHICLE_MASTER.Hide()
}

function OnBtnUpdateVClicked(s) {
    var $btn = $(s);
    if ($btn.hasClass("saving")) {
        return;
    }
    if (!ValidateParam("VEHICLE_MASTER")) {
        return;
    }
    ActiveForm_VEHICLE_MASTER(false);
    $btn.addClass("saving");
    $.ajax({
        url: baseUrl + "/TB_M_VEHICLE_MASTER/SaveData",
        method: "post",
        data: GetObject_VEHICLE_MASTER(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm_VEHICLE_MASTER(true);
        if (response.success) {
            popTB_M_VEHICLE_MASTER.Hide();
            gvList.PerformCallback();
        } else {
            alert(response.message);
        }
    }).always(function () {
        $btn.removeClass("saving");
    });
}

function ActiveForm_VEHICLE_MASTER(active) {

    _CFC.SetEnabled(active);
    _PROJECT_CODE.SetEnabled(active);
    _KATASHIKI.SetEnabled(active);
    _PROD_SFX.SetEnabled(active);
    _MKT_SFX.SetEnabled(active);
    _START_LOT.SetEnabled(active);
    _START_PROD_DATE.SetEnabled(active);
    _END_LOT.SetEnabled(active);
    _END_PROD_DATE.SetEnabled(active);
    _GRADE_MARK.SetEnabled(active);
    _IS_ACTIVE_V.SetEnabled(active);

}

function GetObject_VEHICLE_MASTER() {
    return {
        
        ID: _ID,
        MODEL_ID: _MODEL_ID,
        CFC: _CFC.GetText().trim(),
        PROJECT_CODE: _PROJECT_CODE.GetText().trim(),
        KATASHIKI: _KATASHIKI.GetText().trim(),
        PROD_SFX: _PROD_SFX.GetText().trim(),
        MKT_SFX: _MKT_SFX.GetText().trim(),
        START_LOT: _START_LOT.GetText().trim(),
        START_PROD_DATE: _START_PROD_DATE.GetText().trim(),
        END_LOT: _END_LOT.GetText().trim(),
        END_PROD_DATE: _END_PROD_DATE.GetText().trim(),
        GRADE_MARK: _GRADE_MARK.GetText().trim(),
        IS_ACTIVE: _IS_ACTIVE_V.GetText().trim()
    }
}

function VEHICLEDelete(s) {
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
        url: baseUrl + "/TB_M_VEHICLE_MASTER/Delete",
        method: "post",
        dataType: "json",
        data: {
            sid: $btn.attr("data-id")
        }
    }).done(function (response) {
        if (response.success) {
            gvList.PerformCallback();
        } else {
            alert(response.message);
        }
    }).always(function () {
        $btn.html("Delete");
        $btn.parent().removeClass("processing");
    });
    return false;
}

function VEHICLEEdit(t) {

    OnPopTB_M_VEHICLE_MASTERCloseUp();

    _ID = $(t).attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_M_VEHICLE_MASTER/TB_M_VEHICLE_MASTER_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _ID
        }
    }).done(function (response) {

        _MODEL_ID = response.MODEL_ID;

        _CFC.SetText(response.CFC);
        _PROJECT_CODE.SetText(response.PROJECT_CODE);
        _KATASHIKI.SetText(response.KATASHIKI);
        _PROD_SFX.SetText(response.PROD_SFX);
        _MKT_SFX.SetText(response.MKT_SFX);
        _START_LOT.SetText(response.START_LOT);
        _START_PROD_DATE.SetText(response.START_PROD_DATE_Str_DDMMYYYY);
        _END_LOT.SetText(response.END_LOT);
        _END_PROD_DATE.SetText(response.END_PROD_DATE_Str_DDMMYYYY);
        _GRADE_MARK.SetText(response.GRADE_MARK);
        _IS_ACTIVE_V.SetText(response.IS_ACTIVE );

        popTB_M_VEHICLE_MASTER.Show();
    }).fail(function () {
        OnPopTB_M_VEHICLE_MASTERCloseUp();
    });
    return false;
}

function ValidateParam() {
    return doValidate();
}