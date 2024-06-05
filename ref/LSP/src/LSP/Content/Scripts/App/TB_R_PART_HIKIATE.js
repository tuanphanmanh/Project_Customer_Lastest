var _id = -1;
var _searchTimeout;
var _searchXhr;

function Add() {
    OnPopTB_R_PART_HIKIATECloseUp();
    popTB_R_PART_HIKIATE.Show();
    return false;
}

function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_R_PART_HIKIATE/TB_R_PART_HIKIATE_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_R_PART_HIKIATE']").html("Edit PART HIKIATE");
       
			_CFC.SetText(response.CFC);
			_PROD_SFX.SetText(response.PROD_SFX);
			_PART_NO.SetText(response.PART_NO);
			_COLOR_SFX.SetText(response.COLOR_SFX);
			_PART_NAME.SetText(response.PART_NAME);
			_QTY_PER_VEHICLE.SetText(response.QTY_PER_VEHICLE);
			_BACK_NO.SetText(response.BACK_NO);
			_PARTS_MACHING_KEY.SetText(response.PARTS_MACHING_KEY);
			_SUPPLIER_CODE.SetText(response.SUPPLIER_CODE);
			_SHOP.SetText(response.SHOP);
			_DOCK.SetText(response.DOCK);
			_ORGANISATION.SetText(response.ORGANISATION);
			_RECEIVING_TIME.SetText(response.RECEIVING_TIME);
			_PLANT_TC_FROM.SetText(response.PLANT_TC_FROM_Str_DDMMYYYY);
			_PLANT_TC_TO.SetText(response.PLANT_TC_TO_Str_DDMMYYYY);
			_START_LOT.SetText(response.START_LOT);
			_END_LOT.SetText(response.END_LOT);
			_BOX_SIZE.SetText(response.BOX_SIZE);
			_PACKING_MIX.SetText(response.PACKING_MIX);
			_BOX_WEIGHT.SetText(response.BOX_WEIGHT);
			_BOX_W.SetText(response.BOX_W);
			_BOX_H.SetText(response.BOX_H);
			_BOX_L.SetText(response.BOX_L);
			_PALLET_WEIGHT.SetText(response.PALLET_WEIGHT);
			_QTY_BOX_PER_PALLET.SetText(response.QTY_BOX_PER_PALLET);
			_PALLET_W.SetText(response.PALLET_W);
			_PALLET_H.SetText(response.PALLET_H);
			_PALLET_L.SetText(response.PALLET_L);
			_UNIT.SetText(response.UNIT);
			_COST.SetText(response.COST);
			 
			_IS_ACTIVE.SetChecked((response.IS_ACTIVE == "Y") ? 1 : 0);
			_COLOR.SetText(response.COLOR);

			_COST.SetText(response.COST);
			_DELIVERY_PROCESS.SetText(response.DELIVERY_PROCESS);
			_PACKAGING_TYPE.SetText(response.PACKAGING_TYPE);

			_CFC.SetEnabled(false);
			_PROD_SFX.SetEnabled(false);
			_PART_NO.SetEnabled(false);
			_COLOR_SFX.SetEnabled(false);
			_SUPPLIER_CODE.SetEnabled(false);
			
		   
    }).fail(function () {
        OnPopTB_R_PART_HIKIATECloseUp();
    })
    popTB_R_PART_HIKIATE.Show();
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
        url: baseUrl + "/TB_R_PART_HIKIATE/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_R_PART_HIKIATE.Hide();
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
    ActiveForm(true);
    popTB_R_PART_HIKIATE.Hide()
}

function OnPopTB_R_PART_HIKIATECloseUp() {
    _id = -1;
	$("span[id^='popTB_R_PART_HIKIATE']").html("Add PART HIKIATE");
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
        url: baseUrl + "/TB_R_PART_HIKIATE/Delete",
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
        url: baseUrl + "/TB_R_PART_HIKIATE/SetObjectInfo",
        method: "post",
        data: {
            CFC: _txtCFC.GetText(),
            PROD_SFX: _txtPROD_SFX.GetText(),
            PART_NO: _txtPART_NO.GetText(),
            BACK_NO: _txtBACK_NO.GetText(),
            SUPPLIER_CODE: _txtSUPPLIER_CODE.GetText()
        }
    }).done(function (response) {
        gvList.PerformCallback();
    });
}

function ClearForm() {
	_CFC.SetText("");
	_PROD_SFX.SetText("");
	_PART_NO.SetText("");
	_COLOR_SFX.SetText("");
	_PART_NAME.SetText("");
	_QTY_PER_VEHICLE.SetText("");
	_BACK_NO.SetText("");
	_PARTS_MACHING_KEY.SetText("");
	_SUPPLIER_CODE.SetText("");
	_SHOP.SetText("");
	_DOCK.SetText("");
	_ORGANISATION.SetText("");
	_RECEIVING_TIME.SetText("");
	_PLANT_TC_FROM.SetText("");
	_PLANT_TC_TO.SetText("");
	_START_LOT.SetText("");
	_END_LOT.SetText("");
	_BOX_SIZE.SetText("");
	_PACKING_MIX.SetText("");
	_BOX_WEIGHT.SetText("");
	_BOX_W.SetText("");
	_BOX_H.SetText("");
	_BOX_L.SetText("");
	_PALLET_WEIGHT.SetText("");
	_QTY_BOX_PER_PALLET.SetText("");
	_PALLET_W.SetText("");
	_PALLET_H.SetText("");
	_PALLET_L.SetText("");
	_UNIT.SetText("");
	_COST.SetText("");
	 
	_IS_ACTIVE.SetChecked(1);
	_COLOR.SetText("");
	_DELIVERY_PROCESS.SetText("");
	_PACKAGING_TYPE.SetText("");

	validatereset();
}

function ClearSearch() {
    DoSearch();
}

function ActiveForm(active) {
	_CFC.SetEnabled(active);
	_PROD_SFX.SetEnabled(active);
	_PART_NO.SetEnabled(active);
	_COLOR_SFX.SetEnabled(active);
	_PART_NAME.SetEnabled(active);
	_QTY_PER_VEHICLE.SetEnabled(active);
	_BACK_NO.SetEnabled(active);
	_PARTS_MACHING_KEY.SetEnabled(active);
	_SUPPLIER_CODE.SetEnabled(active);
	_SHOP.SetEnabled(active);
	_DOCK.SetEnabled(active);
	_ORGANISATION.SetEnabled(active);
	_RECEIVING_TIME.SetEnabled(active);
	_PLANT_TC_FROM.SetEnabled(active);
	_PLANT_TC_TO.SetEnabled(active);
	_START_LOT.SetEnabled(active);
	_END_LOT.SetEnabled(active);
	_BOX_SIZE.SetEnabled(active);
	_PACKING_MIX.SetEnabled(active);
	_BOX_WEIGHT.SetEnabled(active);
	_BOX_W.SetEnabled(active);
	_BOX_H.SetEnabled(active);
	_BOX_L.SetEnabled(active);
	_PALLET_WEIGHT.SetEnabled(active);
	_QTY_BOX_PER_PALLET.SetEnabled(active);
	_PALLET_W.SetEnabled(active);
	_PALLET_H.SetEnabled(active);
	_PALLET_L.SetEnabled(active);
	_UNIT.SetEnabled(active);
	_COST.SetEnabled(active);	 
	_IS_ACTIVE.SetEnabled(active);
	_COLOR.SetEnabled(active);
	_DELIVERY_PROCESS.SetEnabled(active);
	_PACKAGING_TYPE.SetEnabled(active);
	
}

function GetObject() {
    return {
		ID: _id, 
		CFC: _CFC.GetText().trim(), 
		PROD_SFX: _PROD_SFX.GetText().trim(), 
		PART_NO: _PART_NO.GetText().trim(), 
		COLOR_SFX: _COLOR_SFX.GetText().trim(), 
		PART_NAME: _PART_NAME.GetText().trim(), 
		QTY_PER_VEHICLE: _QTY_PER_VEHICLE.GetText().trim(), 
		BACK_NO: _BACK_NO.GetText().trim(), 
		PARTS_MACHING_KEY: _PARTS_MACHING_KEY.GetText().trim(), 
		SUPPLIER_CODE: _SUPPLIER_CODE.GetText().trim(), 
		SHOP: _SHOP.GetText().trim(), 
		DOCK: _DOCK.GetText().trim(), 
		ORGANISATION: _ORGANISATION.GetText().trim(), 
		RECEIVING_TIME: _RECEIVING_TIME.GetText().trim(), 
		PLANT_TC_FROM: ConvertDate(_PLANT_TC_FROM.GetText().trim()), 
		PLANT_TC_TO: ConvertDate(_PLANT_TC_TO.GetText().trim()),
		START_LOT: _START_LOT.GetText().trim(), 
		END_LOT: _END_LOT.GetText().trim(), 
		BOX_SIZE: _BOX_SIZE.GetText().trim(), 
		PACKING_MIX: _PACKING_MIX.GetText().trim(), 
		BOX_WEIGHT: _BOX_WEIGHT.GetText().trim(), 
		BOX_W: _BOX_W.GetText().trim(), 
		BOX_H: _BOX_H.GetText().trim(), 
		BOX_L: _BOX_L.GetText().trim(), 
		PALLET_WEIGHT: _PALLET_WEIGHT.GetText().trim(), 
		QTY_BOX_PER_PALLET: _QTY_BOX_PER_PALLET.GetText().trim(), 
		PALLET_W: _PALLET_W.GetText().trim(), 
		PALLET_H: _PALLET_H.GetText().trim(), 
		PALLET_L: _PALLET_L.GetText().trim(), 
		UNIT: _UNIT.GetText().trim(), 
		COST: _COST.GetText().trim(), 		 
		IS_ACTIVE: (_IS_ACTIVE.GetChecked()) ? "Y" : "N",
		COLOR: _COLOR.GetText().trim(),
		DELIVERY_PROCESS: _DELIVERY_PROCESS.GetText().trim(),
        PACKAGING_TYPE: _PACKAGING_TYPE.GetText().trim()
	 
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


/***************IMPORT*******************/

function DOWNLOAD_PART_HIKIATE_TEMPLATE() {
    window.open("/Content/Template/PART_HIKIATE_TEMPLATE.xls", '_blank');
}


function IMPORT_PART_HIKIATE() {
    OnPopPART_HIKIATECloseUp();
    popPART_HIKIATE.Show();

    return false;
}

function OnPopPART_HIKIATECloseUp() {
    ClearFormPART_HIKIATE();
}

function ClearFormPART_HIKIATE() {
    //aspxUClearFileInputClick('_PART_HIKIATE', 0);
    //validatereset();
}

function PART_HIKIATE_OnFileUploadComplete(s, e) {
    if (e.isValid) {
        popPART_HIKIATE.Hide();
        gvList.PerformCallback();
        msgOk(e.callbackData);
    }
    else {
        msgWarning(e.callbackData);
    }

    LoadingPanel.Hide();   
}

function PART_HIKIATE_UPLOAD(s, e) {
    _IMPORT_PART_HIKIATE.Upload();
    LoadingPanel.Show();
}

function OnBtnImportCancelClicked() {
    popPART_HIKIATE.Hide()
}






//***************************************MASTER

var _STOCK_ID = 0;
var _PART_ID = 0;

function AssignNew(t) {
    OnPopSTOCKCloseUp();

    _PART_ID = $(t).attr("data-id");
    popSTOCK.Show();

}


function OnPopSTOCKCloseUp() {
    _STOCK_ID = 0;
    _PART_ID = 0;

    $("span[id^='popSTOCK']").html("Add HIKIATE STOCK STD");
    ClearFormSTOCK();
}

function ClearFormSTOCK() {

    //_PART_ID.SetText("");

    //_PART_ID.SetText("");
    //_DELIVERY_SEQ.SetText("");
    //_DELIVERY_TIME.SetText("");

    //_T_IS_ACTIVE.SetChecked(1);
     
    _MIN_STOCK.SetText("");
    _MAX_STOCK.SetText("");
    _TC_FROM.SetText("");
    _TC_TO.SetText("");
    _S_IS_ACTIVE.SetChecked(1);
     
    validatereset();
}

function OnBtnSTOCKCancelClicked() {
    popSTOCK.Hide()
}

function OnBtnSTOCKUpdateClicked(s) {
    var $btn = $(s);
    if ($btn.hasClass("saving")) {
        return;
    }

    if (!doValidateParam("STOCK")) {
        return;
    }

    ActiveFormSTOCK(false);
    $btn.addClass("saving");
    $.ajax({
        url: baseUrl + "/TB_R_PART_HIKIATE_STOCK_STD/SaveData",
        method: "post",
        data: GetObjectSTOCK(),
        dataType: "json"
    }).done(function (response) {
        ActiveFormSTOCK(true);
        if (response.success) {
            popSTOCK.Hide();
            gvList.PerformCallback();
        } else {
            alert(response.message);
        }
    }).always(function () {
        $btn.removeClass("saving");
    });
}

function GetObjectSTOCK() {
    return { 
        ID: _STOCK_ID,
        PART_ID: _PART_ID,
		MIN_STOCK: _MIN_STOCK.GetText().trim(),
		MAX_STOCK: _MAX_STOCK.GetText().trim(),
		TC_FROM: ConvertDate(_TC_FROM.GetText().trim()),
		TC_TO: ConvertDate(_TC_TO.GetText().trim()),
		IS_ACTIVE: (_S_IS_ACTIVE.GetChecked()) ? "Y" : "N"
    }
}

function ActiveFormSTOCK(active) {
     
    _MIN_STOCK.SetEnabled(active);
    _MAX_STOCK.SetEnabled(active);
    _TC_FROM.SetEnabled(active);
    _TC_TO.SetEnabled(active);
    _S_IS_ACTIVE.SetEnabled(active);
}



function STOCK_Delete(s) {
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

function STOCK_Edit(t) {

    OnPopSTOCKCloseUp();

    _STOCK_ID = $(t).attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_R_PART_HIKIATE_STOCK_STD/TB_R_PART_HIKIATE_STOCK_STD_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _STOCK_ID
        }
    }).done(function (response) {
        
        $("span[id^='popSTOCK']").html("Edit STOCK STD.");

        _PART_ID = response.PART_ID;
 
        _MIN_STOCK.SetText(response.MIN_STOCK);
        _MAX_STOCK.SetText(response.MAX_STOCK);
        _TC_FROM.SetText(response.TC_FROM_Str_DDMMYYYY);
        _TC_TO.SetText(response.TC_TO_Str_DDMMYYYY);

        _S_IS_ACTIVE.SetChecked((response.IS_ACTIVE == "Y") ? 1 : 0);


        //var d = new Date(new Date().toDateString() + ' ' + jsonTimeToString(response.DELIVERY_TIME));
        //_DELIVERY_TIME.SetValue(d);



        popSTOCK.Show();
    });
    return false;
}

/******************************* DETAIL *********************************/
function showdetail() {
    popDetails.Show();
    gvListDetails.PerformCallback();
}

function OnpopDetailsCloseUp() {

}

