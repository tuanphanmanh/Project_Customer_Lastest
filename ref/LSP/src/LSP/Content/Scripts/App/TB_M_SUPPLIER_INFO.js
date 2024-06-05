var _id = -1;
var _searchTimeout;
var _searchXhr;

function Add() {
    OnPopTB_M_SUPPLIER_INFOCloseUp();
    popTB_M_SUPPLIER_INFO.Show();
    return false;
}

function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }

    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_M_SUPPLIER_INFO/TB_M_SUPPLIER_INFO_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_M_SUPPLIER_INFO']").html("Edit TB_M_SUPPLIER_INFO");
         
			_SUPPLIER_CODE.SetText(response.SUPPLIER_CODE);
			_SUPPLIER_PLANT_CODE.SetText(response.SUPPLIER_PLANT_CODE);
			_SUPPLIER_NAME.SetText(response.SUPPLIER_NAME);
			_SUPPLIER_NAME_EN.SetText(response.SUPPLIER_NAME_EN);
			_ADDRESS.SetText(response.ADDRESS);
			_DOCK_X.SetText(response.DOCK_X);
			_DOCK_X_ADDRESS.SetText(response.DOCK_X_ADDRESS);
			_DELIVERY_METHOD.SetText(response.DELIVERY_METHOD);
			_DELIVERY_FREQUENCY.SetText(response.DELIVERY_FREQUENCY);
			_CD.SetText(response.CD);
			_ORDER_DATE_TYPE.SetText(response.ORDER_DATE_TYPE);
			_KEIHEN_TYPE.SetText(response.KEIHEN_TYPE);
			_STK_CONCEPT_TMV_MIN.SetText(response.STK_CONCEPT_TMV_MIN);
			_STK_CONCEPT_TMV_MAX.SetText(response.STK_CONCEPT_TMV_MAX);
			_STK_CONCEPT_SUP_M_MIN.SetText(response.STK_CONCEPT_SUP_M_MIN);
			_STK_CONCEPT_SUP_M_MAX.SetText(response.STK_CONCEPT_SUP_M_MAX);
			_STK_CONCEPT_SUP_P_MIN.SetText(response.STK_CONCEPT_SUP_P_MIN);
			_STK_CONCEPT_SUP_P_MAX.SetText(response.STK_CONCEPT_SUP_P_MAX);
			_TMV_PRODUCT_PERCENTAGE.SetText(response.TMV_PRODUCT_PERCENTAGE);
			_PIC_MAIN_ID.SetText(response.PIC_MAIN_ID);
			_DELIVERY_LT.SetText(response.DELIVERY_LT);
			_PRODUCTION_SHIFT.SetText(response.PRODUCTION_SHIFT);
			_TC_FROM.SetText(response.TC_FROM_Str_DDMMYYYY);
			_TC_TO.SetText(response.TC_TO_Str_DDMMYYYY);
 
			_IS_ACTIVE.SetChecked((response.IS_ACTIVE == "Y") ? 1 : 0);
		   
    }).fail(function () {
        OnPopTB_M_SUPPLIER_INFOCloseUp();
    })
    popTB_M_SUPPLIER_INFO.Show();
    return false;
}

function OnBtnUpdateClicked(s) {
    var $btn = $(s);
    if ($btn.hasClass("saving")) {
        return;
    }
    if (!doValidateParam("SUPPLIER_INFO")) {
        return;
    }
    ActiveForm(false);
    $btn.addClass("saving");
    $.ajax({
        url: baseUrl + "/TB_M_SUPPLIER_INFO/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_M_SUPPLIER_INFO.Hide();
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
    popTB_M_SUPPLIER_INFO.Hide()
}

function OnPopTB_M_SUPPLIER_INFOCloseUp() {
	_id = -1
	$("span[id^='popTB_M_SUPPLIER_INFO']").html("Add New TB_M_SUPPLIER_INFO")
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
        url: baseUrl + "/TB_M_SUPPLIER_INFO/Delete",
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
        url: baseUrl + "/TB_M_SUPPLIER_INFO/SetObjectInfo",
        method: "post",
        data: {
            SUPPLIER_CODE: _txtSUPPLIER_CODE.GetText(),
			SUPPLIER_NAME: _txtSUPPLIER_NAME.GetText()
        }
    }).done(function (response) {
        gvList.PerformCallback();
    });
}

function ClearForm() {
	_SUPPLIER_CODE.SetText("");
	_SUPPLIER_PLANT_CODE.SetText("");
	_SUPPLIER_NAME.SetText("");
	_SUPPLIER_NAME_EN.SetText("");
	_ADDRESS.SetText("");
	_DOCK_X.SetText("");
	_DOCK_X_ADDRESS.SetText("");
	_DELIVERY_METHOD.SetText("");
	_DELIVERY_FREQUENCY.SetText("");
	_CD.SetText("");
	_ORDER_DATE_TYPE.SetText("");
	_KEIHEN_TYPE.SetText("");
	_STK_CONCEPT_TMV_MIN.SetText("");
	_STK_CONCEPT_TMV_MAX.SetText("");
	_STK_CONCEPT_SUP_M_MIN.SetText("");
	_STK_CONCEPT_SUP_M_MAX.SetText("");
	_STK_CONCEPT_SUP_P_MIN.SetText("");
	_STK_CONCEPT_SUP_P_MAX.SetText("");
	_TMV_PRODUCT_PERCENTAGE.SetText("");
	_PIC_MAIN_ID.SetText("");
	_DELIVERY_LT.SetText("");
	_PRODUCTION_SHIFT.SetText("");
	_TC_FROM.SetText("");
	_TC_TO.SetText("");
	 
	_IS_ACTIVE.SetChecked(1);
	validatereset();
}

function ClearSearch() {
    DoSearch();
}

function ActiveForm(active) {
	_SUPPLIER_CODE.SetEnabled(active);
	_SUPPLIER_PLANT_CODE.SetEnabled(active);
	_SUPPLIER_NAME.SetEnabled(active);
	_SUPPLIER_NAME_EN.SetEnabled(active);
	_ADDRESS.SetEnabled(active);
	_DOCK_X.SetEnabled(active);
	_DOCK_X_ADDRESS.SetEnabled(active);
	_DELIVERY_METHOD.SetEnabled(active);
	_DELIVERY_FREQUENCY.SetEnabled(active);
	_CD.SetEnabled(active);
	_ORDER_DATE_TYPE.SetEnabled(active);
	_KEIHEN_TYPE.SetEnabled(active);
	_STK_CONCEPT_TMV_MIN.SetEnabled(active);
	_STK_CONCEPT_TMV_MAX.SetEnabled(active);
	_STK_CONCEPT_SUP_M_MIN.SetEnabled(active);
	_STK_CONCEPT_SUP_M_MAX.SetEnabled(active);
	_STK_CONCEPT_SUP_P_MIN.SetEnabled(active);
	_STK_CONCEPT_SUP_P_MAX.SetEnabled(active);
	_TMV_PRODUCT_PERCENTAGE.SetEnabled(active);
	_PIC_MAIN_ID.SetEnabled(active);
	_DELIVERY_LT.SetEnabled(active);
	_PRODUCTION_SHIFT.SetEnabled(active);
	_TC_FROM.SetEnabled(active);
	_TC_TO.SetEnabled(active);
 
	_IS_ACTIVE.SetEnabled(active);
	
}

function GetObject() {
    return {
		ID: _id, 
		SUPPLIER_CODE: _SUPPLIER_CODE.GetText().trim(), 
		SUPPLIER_PLANT_CODE: _SUPPLIER_PLANT_CODE.GetText().trim(), 
		SUPPLIER_NAME: _SUPPLIER_NAME.GetText().trim(),
		SUPPLIER_NAME_EN: _SUPPLIER_NAME_EN.GetText().trim(),
		ADDRESS: _ADDRESS.GetText().trim(), 
		DOCK_X: _DOCK_X.GetText().trim(), 
		DOCK_X_ADDRESS: _DOCK_X_ADDRESS.GetText().trim(), 
		DELIVERY_METHOD: _DELIVERY_METHOD.GetText().trim(), 
		DELIVERY_FREQUENCY: _DELIVERY_FREQUENCY.GetText().trim(), 
		CD: _CD.GetText().trim(), 
		ORDER_DATE_TYPE: _ORDER_DATE_TYPE.GetText().trim(), 
		KEIHEN_TYPE: _KEIHEN_TYPE.GetText().trim(), 
		STK_CONCEPT_TMV_MIN: _STK_CONCEPT_TMV_MIN.GetText().trim(), 
		STK_CONCEPT_TMV_MAX: _STK_CONCEPT_TMV_MAX.GetText().trim(), 
		STK_CONCEPT_SUP_M_MIN: _STK_CONCEPT_SUP_M_MIN.GetText().trim(), 
		STK_CONCEPT_SUP_M_MAX: _STK_CONCEPT_SUP_M_MAX.GetText().trim(), 
		STK_CONCEPT_SUP_P_MIN: _STK_CONCEPT_SUP_P_MIN.GetText().trim(), 
		STK_CONCEPT_SUP_P_MAX: _STK_CONCEPT_SUP_P_MAX.GetText().trim(), 
		TMV_PRODUCT_PERCENTAGE: _TMV_PRODUCT_PERCENTAGE.GetText().trim(), 
		PIC_MAIN_ID: _PIC_MAIN_ID.GetText().trim(), 
		DELIVERY_LT: _DELIVERY_LT.GetText().trim(), 
		PRODUCTION_SHIFT: _PRODUCTION_SHIFT.GetText().trim(), 
		TC_FROM: ConvertDate(_TC_FROM.GetText().trim()),
		TC_TO: ConvertDate(_TC_TO.GetText().trim()),
 
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


//***************************************MASTER ORDER TIME

var _OR_TIME_ID = 0;
var _SUPPLIER_ID = 0;

function OR_TimeNew(t) {
    OnPopOR_TIMECloseUp();

    _SUPPLIER_ID = $(t).attr("data-id");
    popOR_Time.Show();

}


function OnPopOR_TIMECloseUp() {
    _OR_TIME_ID = 0;
    _SUPPLIER_ID = 0;

    $("span[id^='popOR_Time']").html("Add new Supplier Order Time");
    ClearFormOR_Time();
}

function ClearFormOR_Time() {

    _ORDER_SEQ.SetText("");
    _ORDER_TIME.SetText("");
    _RECEIVE_TIME.SetText("");
    _KEIHEN_TIME.SetText("");    
    _ORDER_TYPE.SetValue("D");
    _RECEIVING_DAY.SetText("");
    _KEIHEN_DAY.SetText("");
    _T_IS_ACTIVE.SetChecked(1);

    validatereset();
}


function OnBtnOR_TimeCancelClicked() {
    popOR_Time.Hide()
}

function OnBtnOR_TimeUpdateClicked(s) {
    var $btn = $(s);
    if ($btn.hasClass("saving")) {
        return;
    }

    if (!doValidateParam("OR_TIME")) {
        return;
    }

    ActiveFormOR_Time(false);
    $btn.addClass("saving");
    $.ajax({
        url: baseUrl + "/TB_M_SUPPLIER_OR_TIME/SaveData",
        method: "post",
        data: GetObjectOR_Time(),
        dataType: "json"
    }).done(function (response) {
        ActiveFormOR_Time(true);
        if (response.success) {
            popOR_Time.Hide();
            gvList.PerformCallback();
        } else {
            alert(response.message);
        }
    }).always(function () {
        $btn.removeClass("saving");
    });
}

function GetObjectOR_Time() {
    return {
        ID: _OR_TIME_ID,
        SUPPLIER_ID: _SUPPLIER_ID,
        ORDER_SEQ: _ORDER_SEQ.GetText().trim(),
        ORDER_TIME: _ORDER_TIME.GetText().trim(),
        RECEIVE_TIME: _RECEIVE_TIME.GetText().trim(),
        KEIHEN_TIME: _KEIHEN_TIME.GetText().trim(),
        KEIHEN_DAY: _KEIHEN_DAY.GetText().trim(),
        ORDER_TYPE: _ORDER_TYPE.GetValue(),//_ORDER_TYPE.GetText().trim(),
        RECEIVING_DAY: _RECEIVING_DAY.GetText().trim(),

        IS_ACTIVE: (_T_IS_ACTIVE.GetChecked()) ? "Y" : "N"
    }
}

function ActiveFormOR_Time(active) {

    _ORDER_SEQ.SetEnabled(active);
    _ORDER_TIME.SetEnabled(active);
    _RECEIVE_TIME.SetEnabled(active);
    _KEIHEN_TIME.SetEnabled(active);
    _KEIHEN_DAY.SetEnabled(active);
    _ORDER_TYPE.SetEnabled(active);
    _RECEIVING_DAY.SetEnabled(active);
    _T_IS_ACTIVE.SetEnabled(active);
}

function OR_TimeDelete(s) {
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
        url: baseUrl + "/TB_M_SUPPLIER_OR_TIME/Delete",
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

function OR_TimeEdit(t) {

    OnPopOR_TIMECloseUp();

    _OR_TIME_ID = $(t).attr("data-id");

    $("span[id^='popOR_Time']").html("Edit Order Time");

    $.ajax({
        url: baseUrl + "/TB_M_SUPPLIER_OR_TIME/TB_M_SUPPLIER_OR_TIME_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _OR_TIME_ID
        }
    }).done(function (response) {

        _SUPPLIER_ID = response.SUPPLIER_ID;
        _ORDER_SEQ.SetText(response.ORDER_SEQ);
        //_ORDER_TIME.SetText(response.ORDER_TIME);
        _ORDER_TYPE.SetValue(response.ORDER_TYPE);
        _RECEIVING_DAY.SetText(response.RECEIVING_DAY);
        _T_IS_ACTIVE.SetChecked((response.IS_ACTIVE == "Y") ? 1 : 0);
        _KEIHEN_DAY.SetText(response.KEIHEN_DAY);

        var d = new Date(new Date().toDateString() + ' ' + jsonTimeToString(response.ORDER_TIME));
        _ORDER_TIME.SetValue(d);

        var rt = new Date(new Date().toDateString() + ' ' + jsonTimeToString(response.RECEIVE_TIME));
        _RECEIVE_TIME.SetValue(rt);
        
        var kh = new Date(new Date().toDateString() + ' ' + jsonTimeToString(response.KEIHEN_TIME));
        _KEIHEN_TIME.SetValue(kh);

        popOR_Time.Show();
    });
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
    var strtime = (((dt.getHours() + "").length == 1) ? ("0" + dt.getHours()) : dt.getHours()) + " : " + (((dt.getMinutes() + "").length == 1) ? ("0" + dt.getMinutes()) : dt.getMinutes())
    return strtime;
}

function getMonthEN(m) {
    switch (m) {
        case 0: return "January";
        case 1: return "February";
        case 2: return "March";
        case 3: return "April";
        case 4: return "May";
        case 5: return "June";
        case 6: return "July";
        case 7: return "August";
        case 8: return "September";
        case 9: return "October";
        case 10: return "November";
        case 11: return "December";
        default: return m;
    }
}



/***************IMPORT*******************/
function IMPORT_SUPPLIER_INFO() {

    OnPopSUPPLIER_INFOCloseUp();
    popIMPORT_SUPPLIER_INFO.Show();

    return false;
}

function OnPopSUPPLIER_INFOCloseUp() {
    ClearFormSUPPLIER_INFO();
}

function ClearFormSUPPLIER_INFO() {
    //aspxUClearFileInputClick('_IMPORT_SUPPLIER_INFO', 0);
    //validatereset();
}

function SUPPLIER_INFO_OnFileUploadComplete(s, e) {
    //alert(e.callbackData);
    alert(e.callbackData);
    if (e.isValid) {
        popIMPORT_SUPPLIER_INFO.Hide();
        gvList.PerformCallback();
        //gvList.PerformCallback({ isCustomCallback: false, args: "" });
    }
}


function OnBtnImportCancelClicked() {
    popIMPORT_SUPPLIER_INFO.Hide();
}


