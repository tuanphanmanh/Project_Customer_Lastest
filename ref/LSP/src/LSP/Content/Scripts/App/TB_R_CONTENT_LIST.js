var _id = -1;
var _searchTimeout;
var _searchXhr;

function ContentListNew(id) {

    _ORDER_ID = id;
    OnPopTB_R_CONTENT_LISTCloseUp(); 
    popTB_R_CONTENT_LIST.Show();
    return false;
}

function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_R_CONTENT_LIST/TB_R_CONTENT_LIST_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_R_CONTENT_LIST']").html("Edit TB_R_CONTENT_LIST");
            
            _ORDER_ID = response.ORDER_ID;
			_SUPPLIER_NAME.SetText(response.SUPPLIER_NAME);
			_SUPPLIER_CODE.SetText(response.SUPPLIER_CODE);
			_RENBAN_NO.SetText(response.RENBAN_NO);
			_PC_ADDRESS.SetText(response.PC_ADDRESS);
			_DOCK_NO.SetText(response.DOCK_NO);
			_ORDER_NO.SetText(response.ORDER_NO);
			_ORDER_DATETIME.SetText(response.ORDER_DATETIME_Str_DDMMYYYY_HHMM);
			_TRIP_NO.SetText(response.TRIP_NO);
			_PALLET_BOX_QTY.SetText(response.PALLET_BOX_QTY);
			_EST_PACKING_DATETIME.SetText(response.EST_PACKING_DATETIME_Str_DDMMYYYY_HHMM);
			_EST_ARRIVAL_DATETIME.SetText(response.EST_ARRIVAL_DATETIME_Str_DDMMYYYY_HHMM);
			 
			_IS_ACTIVE.SetChecked((response.IS_ACTIVE == "Y") ? 1 : 0);

		   
    }).fail(function () {
        OnPopTB_R_CONTENT_LISTCloseUp();
    })
    popTB_R_CONTENT_LIST.Show();
    return false;
}

function OnBtnUpdateClicked(s) {
    var $btn = $(s);
    if ($btn.hasClass("saving")) {
        return;
    }
    if (!doValidateParam("CONTENT_LIST")) { 
        return;
    }
    ActiveForm(false);
    $btn.addClass("saving");
    //$("#dealerStatus").html("Saving...");
    $.ajax({
        url: baseUrl + "/TB_R_CONTENT_LIST/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_R_CONTENT_LIST.Hide();
            //DoSearch();
            gvOrderList.PerformCallback();
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
    popTB_R_CONTENT_LIST.Hide()
}

function OnPopTB_R_CONTENT_LISTCloseUp() {
    _id = -1;
	$("span[id^='popTB_R_CONTENT_LIST']").html("Add New TB_R_CONTENT_LIST")
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
        url: baseUrl + "/TB_R_CONTENT_LIST/Delete",
        method: "post",
        dataType: "json",
        data: {
            sid: $btn.attr("data-id")
        }
    }).done(function (response) {
        if (response.success) {
            //DoSearch();
            gvOrderList.PerformCallback();
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
        url: baseUrl + "/TB_R_CONTENT_LIST/SetObjectInfoOrder",
        method: "post",
        data: {
            SUPPLIER_CODE: _txtSUPPLIER_CODE.GetText().trim(),
            ORDER_NO:      _txtORDER_NO.GetText().trim(),
            WORKING_DATE: ConvertDate(_txtWORKING_DATE.GetText().trim()),
            IS_BY_RECEIVING_DAY_BOL: _txtIS_BY_RECEIVING_DAY_BOL.GetChecked()
        }
    }).done(function (response) {
        gvOrderList.PerformCallback();
    });
}

function ClearForm() {
	_SUPPLIER_NAME.SetText("");
	_SUPPLIER_CODE.SetText("");
	_RENBAN_NO.SetText("");
	_PC_ADDRESS.SetText("");
	_DOCK_NO.SetText("");
	_ORDER_NO.SetText("");
	_ORDER_DATETIME.SetText("");
	_TRIP_NO.SetText("");
	_PALLET_BOX_QTY.SetText("");
	_EST_PACKING_DATETIME.SetText("");
	_EST_ARRIVAL_DATETIME.SetText("");
 
	_IS_ACTIVE.SetChecked(1);
	validatereset();
}

function ClearSearch() {
    DoClearSearch();
    DoSearch();
}

function DoClearSearch() {
    _txtSUPPLIER_CODE.SetText("");
    _txtORDER_NO.SetText("");     
    _txtWORKING_DATE.SetText("");    
    _txtIS_BY_RECEIVING_DAY_BOL.SetChecked(1);
}

function ActiveForm(active) {
	_SUPPLIER_NAME.SetEnabled(active);
	_SUPPLIER_CODE.SetEnabled(active);
	_RENBAN_NO.SetEnabled(active);
	_PC_ADDRESS.SetEnabled(active);
	_DOCK_NO.SetEnabled(active);
	_ORDER_NO.SetEnabled(active);
	_ORDER_DATETIME.SetEnabled(active);
	_TRIP_NO.SetEnabled(active);
	_PALLET_BOX_QTY.SetEnabled(active);
	_EST_PACKING_DATETIME.SetEnabled(active);
	_EST_ARRIVAL_DATETIME.SetEnabled(active);
 
	_IS_ACTIVE.SetEnabled(active); 
}

function GetObject() {
    return {
        ID: _id,
        ORDER_ID: _ORDER_ID,
		SUPPLIER_NAME: _SUPPLIER_NAME.GetText().trim(), 
		SUPPLIER_CODE: _SUPPLIER_CODE.GetText().trim(), 
		RENBAN_NO: _RENBAN_NO.GetText().trim(), 
		PC_ADDRESS: _PC_ADDRESS.GetText().trim(), 
		DOCK_NO: _DOCK_NO.GetText().trim(), 
		ORDER_NO: _ORDER_NO.GetText().trim(), 
		ORDER_DATETIME: ConvertDate( _ORDER_DATETIME.GetText().trim()), 
		TRIP_NO: _TRIP_NO.GetText().trim(), 
		PALLET_BOX_QTY: _PALLET_BOX_QTY.GetText().trim(), 
		EST_PACKING_DATETIME: ConvertDate(_EST_PACKING_DATETIME.GetText().trim()), 
		EST_ARRIVAL_DATETIME: ConvertDate(_EST_ARRIVAL_DATETIME.GetText().trim()),
		 
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

function ConvertDate2(_date) { 
    //dd/MM/yyy -> MM/dd/yyyy
    if (_date == null || _date == "") {
        return "";
    } 
    var _dt = _date.split(" ");
    var _time = _dt[1];
    var _date1 = _dt[0];

    var spec = "/";
    var item = _date1.split(spec);
    if (item.length != 3) {
        return _date;
    }

    return item[1] + spec + item[0] + spec + item[2] + " " + _time;
}

//***************************************Kanban******************/

var _KANBAN_ID = 0; 
var _CONTENT_LIST_ID = 0;

function KanbanNew(t) {
    OnPopKANBANCloseUp();
 
    _CONTENT_LIST_ID = $(t).attr("data-id");
    popKANBAN.Show();

}

function OnPopKANBANCloseUp() {
    _KANBAN_ID = 0;
    _CONTENT_LIST_ID = 0;
    
    $("span[id^='popKANBAN']").html("Add new Kanban");
    ClearFormKANBAN();
}

function ClearFormKANBAN() {
    
    //_CONTENT_LIST_ID.SetText("");
    _BACK_NO.SetText("");
    _PART_NO.SetText("");
    _COLOR_SFX.SetText("");
    _PART_NAME.SetText("");
    _BOX_SIZE.SetText("");
    _BOX_QTY.SetText("");
    _K_PC_ADDRESS.SetText("");
    _WH_SPS_PICKING.SetText("");
  
    _K_IS_ACTIVE.SetChecked(1);
     
    validatereset();
}

function OnBtnKanbanCancelClicked() {
    popKANBAN.Hide()
}

function OnBtnKanbanUpdateClicked(s) {
    var $btn = $(s);
    if ($btn.hasClass("saving")) {
        return;
    }
   
    if (!Validate()) {
        return;
    }

    if (!confirm("Are you sure want to continue? \nPlease be aware that it will change STOCK QTY!")) {
        return false;
    }

    ActiveFormKanban(false);
    var BASE_ORDER_ID = gvOrderList.GetRowKey(gvOrderList.GetFocusedRowIndex());
    $btn.addClass("saving");
    $.ajax({
        url: baseUrl + "/TB_R_KANBAN/SaveData",
        method: "post",
        data: GetObjectKanban(),
        dataType: "json"
    }).done(function (response) {
        ActiveFormKanban(true);
        if (response.success) {
            popKANBAN.Hide();
            //Call Re-generate monthly order 
            GENERATE_KEIHEN_MONTHLY_ORDER(BASE_ORDER_ID);
            gvOrderList.PerformCallback();
        } else {
            msgError(response.message);
        }
    }).always(function () {
        $btn.removeClass("saving");
    });
               
}

function Validate() {
    
    return doValidate();
}

function GetObjectKanban() {    
    return {
        ID: _KANBAN_ID,
        CONTENT_LIST_ID: _CONTENT_LIST_ID,
        BACK_NO: _BACK_NO.GetText().trim(),
        PART_NO: _PART_NO.GetText().trim(),
        COLOR_SFX: _COLOR_SFX.GetText().trim(),
        PART_NAME: _PART_NAME.GetText().trim(),
        BOX_SIZE: _BOX_SIZE.GetText().trim(),
        BOX_QTY: _BOX_QTY.GetText().trim(),
        PC_ADDRESS: _K_PC_ADDRESS.GetText().trim(),
        WH_SPS_PICKING: _WH_SPS_PICKING.GetText().trim(),      
        IS_ACTIVE: (_K_IS_ACTIVE.GetChecked()) ? "Y" : "N"
    }
}

function ActiveFormKanban(active) {
    //_CONTENT_LIST_ID.SetEnabled(active);
    _BACK_NO.SetEnabled(active);
    _PART_NO.SetEnabled(active);
    _COLOR_SFX.SetEnabled(active);
    _PART_NAME.SetEnabled(active);
    _BOX_SIZE.SetEnabled(active);
    _BOX_QTY.SetEnabled(active);
    _K_PC_ADDRESS.SetEnabled(active);
    _WH_SPS_PICKING.SetEnabled(active);
 
    _K_IS_ACTIVE.SetEnabled(active);
}

function KanbanDelete(s) {
    var $btn = $(s)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    if (!confirm("Are you sure to delete selected record?\nPlease be aware that it will change STOCK QTY!")) {
        return false;
    }

    var BASE_ORDER_ID = gvOrderList.GetRowKey(gvOrderList.GetFocusedRowIndex());

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
            //Call Re-generate monthly order 
            GENERATE_KEIHEN_MONTHLY_ORDER(BASE_ORDER_ID);
            gvOrderList.PerformCallback();
        } else {
            msgError(response.message);
        }
    }).always(function () {
        $btn.html("Delete");
        $btn.parent().removeClass("processing");
    });
    return false;
}

function KanbanEdit(t) {

    OnPopKANBANCloseUp();
    
    _KANBAN_ID = $(t).attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_R_KANBAN/TB_R_KANBAN_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _KANBAN_ID
        }
    }).done(function (response) {

        _CONTENT_LIST_ID = response.CONTENT_LIST_ID; 
        $("span[id^='popKANBAN']").html("Edit Kanban");
        //_CONTENT_LIST_ID.SetText(response.CONTENT_LIST_ID);
        _BACK_NO.SetText(response.BACK_NO);
        _PART_NO.SetText(response.PART_NO);
        _COLOR_SFX.SetText(response.COLOR_SFX);
        _PART_NAME.SetText(response.PART_NAME);
        _BOX_SIZE.SetText(response.BOX_SIZE);
        _BOX_QTY.SetText(response.BOX_QTY);
        _K_PC_ADDRESS.SetText(response.PC_ADDRESS);
        _WH_SPS_PICKING.SetText(response.WH_SPS_PICKING);
 
        _K_IS_ACTIVE.SetChecked((response.IS_ACTIVE == "Y") ? 1 : 0);
         
        popKANBAN.Show();
    });
    return false;
}

////SAVE PDF

//Order Delivery
function OrderDelivery_OpenPdf(_idcontent) {

    //var _idcontent = gvContentList.GetRowKey(gvContentList.GetFocusedRowIndex());
 
    $.ajax({
        url: baseUrl + "/TB_R_CONTENT_LIST/CONTENT_LIST_SETID",
        method: "post",
        data: {
            ID: _idcontent
        }
    }).done(function (response) {

        window.open(baseUrl + "/TB_R_CONTENT_LIST/PDF_ORDER_DELIVERY");
    });
    return false;
}

function OrderDelivery_SaveViewAsPDF(_idcontent) {
    //var $btn = $(s)
    //if ($btn.parent().hasClass("processing")) {
    //    return false;
    //}

    //$btn.parent().addClass("processing");
    //$btn.html("Saving...");
    $.ajax({
        url: baseUrl + "/TB_R_CONTENT_LIST/PDF_ORDER_DELIVERY_SaveViewAsPDF",
        method: "post",
        dataType: "json",
        data: {
            ID: _idcontent //gvContentList.GetRowKey(gvContentList.GetFocusedRowIndex())
        }
    }).done(function (response) { 
         
        if (response.success != "") {
            window.open(baseUrl + "/Content/Download/" + response.FileName);
        }

    }).always(function () {

        //$btn.parent().removeClass("processing");
    });
    return false;
}

//Content List by Order iD
function ContentListMulti_SaveViewAsPDF(s) {
    var $btn = $(s)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
     
    $btn.parent().addClass("processing");
    $btn.html("Saving...");
    $.ajax({
        url: baseUrl + "/TB_R_CONTENT_LIST/PDF_CONTENT_LIST_MULTI_SaveViewAsPDF",
        method: "post",
        dataType: "json",
        data: {
            ORDER_ID: gvOrderList.GetRowKey(gvOrderList.GetFocusedRowIndex())
        }
    }).done(function (response) {

        if (response.success != "") {
            window.open(baseUrl + "/Content/Download/" + response.FileName);
        }

    }).always(function () {

        $btn.parent().removeClass("processing");
    });
    return false;
}

function ContentListMulti_OpenPdf(s) {
    var _idOrder = gvOrderList.GetRowKey(gvOrderList.GetFocusedRowIndex());
    
    $.ajax({
        url: baseUrl + "/TB_R_CONTENT_LIST/ORDER_SET_ID",
        method: "post",
        data: {
            ID: _idOrder
        }
    }).done(function (response) {

        window.open(baseUrl + "/TB_R_CONTENT_LIST/PDF_CONTENT_LIST_MULTI");
    });
    return false;
}

//Order Delivery 
function OrderDelivery_OpenPdf2(_idcontent) {

    var _idcontent = gvOrderList.GetRowKey(gvOrderList.GetFocusedRowIndex());

    $.ajax({
        url: baseUrl + "/TB_R_CONTENT_LIST/CONTENT_LIST_SETID",
        method: "post",
        data: {
            ID: _idcontent
        }
    }).done(function (response) {

        window.open(baseUrl + "/TB_R_CONTENT_LIST/PDF_ORDER_DELIVERY");
    });
    return false;
}

function OrderDelivery_SaveViewAsPDF2(s) {
    var $btn = $(s)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    
    $btn.parent().addClass("processing");
    $btn.html("Saving...");
    $.ajax({
        url: baseUrl + "/TB_R_CONTENT_LIST/PDF_ORDER_DELIVERY_SaveViewAsPDF2",
        method: "post",
        dataType: "json",
        data: {
            ORDER_ID: gvOrderList.GetRowKey(gvOrderList.GetFocusedRowIndex())
        }
    }).done(function (response) {

        if (response.success != "") {
            window.open(baseUrl + "/Content/Download/" + response.FileName);
        }

    }).always(function () {

        $btn.parent().removeClass("processing");
    });
    return false;
}

function Send_Email_Order(s) {

    if (gvOrderList.GetFocusedRowIndex() >= 0) {
        //Re-confirm want to do
        if (!confirm("Are you sure to Send Order Email to Supplier?"))
            return;

        var $btn = $(s)
        if ($btn.parent().hasClass("processing")) {
            return false;
        }

        $btn.parent().addClass("processing");
        $btn.html("Sending...");
        LoadingPanel.Show();
        $.ajax({
            url: baseUrl + "/TB_R_CONTENT_LIST/EMAIL_SEND_ORDER_V2",
            method: "post",
            dataType: "json",
            data: {
                ORDER_ID: gvOrderList.GetRowKey(gvOrderList.GetFocusedRowIndex())
            }
        }).done(function (response) {
            if (response.success) {
                msgOk(response.message);
            }
            else {
                msgError(response.message);
            }
        }).always(function () {

            $btn.parent().removeClass("processing");
            LoadingPanel.Hide();
        });

    } else {
        msgError('Please select an Order to Send!');
    }
    return false;
}

//Content List
function ContentList_OpenPdf(_idcontent) {

    //var _idcontent = gvContentList.GetRowKey(gvContentList.GetFocusedRowIndex());
    
    $.ajax({
        url: baseUrl + "/TB_R_CONTENT_LIST/CONTENT_LIST_SETID",
        method: "post",
        data: {
            ID: _idcontent
        }
    }).done(function (response) {

        window.open(baseUrl + "/TB_R_CONTENT_LIST/PDF_CONTENT_LIST");
    });
    return false;
}

function ContentList_SaveViewAsPDF(_idcontent) {
   
    $.ajax({
        url: baseUrl + "/TB_R_CONTENT_LIST/PDF_CONTENT_LIST_SaveViewAsPDF",
        method: "post",
        dataType: "json",
        data: {
            ID: _idcontent
        }
    }).done(function (response) {

        if (response.success != "") {
            window.open(baseUrl + "/Content/Download/" + response.FileName);
        }

    }).always(function () {
        
    });
    return false;
}

//Kanban
function Kanban_OpenPdf(_idcontent) {

    //var _idcontent = gvContentList.GetRowKey(gvContentList.GetFocusedRowIndex());

    $.ajax({
        url: baseUrl + "/TB_R_CONTENT_LIST/CONTENT_LIST_SETID",
        method: "post",
        data: {
            ID: _idcontent
        }
    }).done(function (response) {

        window.open(baseUrl + "/TB_R_CONTENT_LIST/PDF_KANBAN");
    });
    return false;
}

function Kanban_SaveViewAsPDF(_idcontent) {
    //var $btn = $(s)
    //if ($btn.parent().hasClass("processing")) {
    //    return false;
    //}

    //$btn.parent().addClass("processing");
    //$btn.html("Saving...");
    $.ajax({
        url: baseUrl + "/TB_R_CONTENT_LIST/PDF_KANBAN_SaveViewAsPDF",
        method: "post",
        dataType: "json",
        data: {
            ID: _idcontent //gvContentList.GetRowKey(gvContentList.GetFocusedRowIndex())
        }
    }).done(function (response) {

        if (response.success != "") {
            window.open(baseUrl + "/Content/Download/" + response.FileName);
        }

    }).always(function () {

        //$btn.parent().removeClass("processing");
    });
    return false;
}

function Kanban_SaveViewAsPDF2(s) {
    var $btn = $(s)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }

    $btn.parent().addClass("processing");
    $btn.html("Saving...");
    $.ajax({
        url: baseUrl + "/TB_R_CONTENT_LIST/PDF_KANBAN_SaveViewAsPDF2",
        method: "post",
        dataType: "json",
        data: {
            ORDER_ID: gvOrderList.GetRowKey(gvOrderList.GetFocusedRowIndex())
        }
    }).done(function (response) {

        if (response.success != "") {
            window.open(baseUrl + "/Content/Download/" + response.FileName);
        }

    }).always(function () {

        $btn.parent().removeClass("processing");
    });
    return false;
}

/*IMPORT*/
function IMPORT_CONTENT_LIST() {
    OnPopCONTENT_LISTCloseUp();
    popCONTENT_LIST.Show();

    return false;
}

function CONTENT_LIST_UPLOAD(s, e) {
    _IMPORT_CONTENT_LIST.Upload();
    LoadingPanel.Show();
}

function OnPopCONTENT_LISTCloseUp() {
    ClearFormCONTENT_LIST();
}

function ClearFormCONTENT_LIST() {
    //aspxUClearFileInputClick('_IMPORT_CONTENT_LIST', 0);
}

function CONTENT_LIST_OnFileUploadComplete(s, e) {
    LoadingPanel.Hide();
    alert(e.callbackData);
    if (e.isValid) {
        popCONTENT_LIST.Hide();
        gvOrderList.PerformCallback();
    }    
}

function OnBtnImportCancelClicked() {
    popCONTENT_LIST.Hide()
}

function DOWNLOAD_DAILY_ORDER_TEMPLATE() {
    window.open("/Content/Template/Daily Order_template.xls", '_blank');
}


//ORDER DAILY
var _ORDER_ID = 0;
function EditOrder(t) {

    var $btn = $(t);
    if ($btn.parent().hasClass("processing")) { return false; }
    _ORDER_ID = $btn.attr("data-id");

    $.ajax({
        url: baseUrl + "/TB_R_DAILY_ORDER/TB_R_DAILY_ORDER_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _ORDER_ID
        }
    }).done(function (response) {
        $("span[id^='popTB_R_DAILY_ORDER']").html("Edit TB_R_DAILY_ORDER");

        _WORKING_DATE.SetText(response.WORKING_DATE_Str_DDMMYYYY);
        _SHIFT.SetText(response.SHIFT);
        _O_SUPPLIER_NAME.SetText(response.SUPPLIER_NAME);
        _O_SUPPLIER_CODE.SetText(response.SUPPLIER_CODE);
        _O_ORDER_NO.SetText(response.ORDER_NO);
        _O_ORDER_DATETIME.SetText(response.ORDER_DATETIME_Str_DDMMYYYY_HHMMSS);
        _O_TRIP_NO.SetText(response.TRIP_NO);
        _TRUCK_NO.SetText(response.TRUCK_NO);
        _O_EST_ARRIVAL_DATETIME.SetText(response.EST_ARRIVAL_DATETIME_Str_DDMMYYYY_HHMMSS);
         
        _O_IS_ACTIVE.SetChecked((response.IS_ACTIVE == "Y") ? 1 : 0); 
        _STATUS.SetText(response.STATUS);

    }).fail(function () {
        OnPopTB_R_DAILY_ORDERCloseUp();
    })
    popTB_R_DAILY_ORDER.Show();
    return false;
}

function OnPopTB_R_DAILY_ORDERCloseUp() {
    _ORDER_ID = -1
    $("span[id^='popTB_R_DAILY_ORDER']").html("Add New TB_R_DAILY_ORDER")
    ClearFormOrder();
}

function ClearFormOrder() {
    _WORKING_DATE.SetText("");
    _SHIFT.SetText("");
    _O_SUPPLIER_NAME.SetText("");
    _O_SUPPLIER_CODE.SetText("");
    _O_ORDER_NO.SetText("");
    _O_ORDER_DATETIME.SetText("");
    _O_TRIP_NO.SetText("");
    _TRUCK_NO.SetText("");
    _O_EST_ARRIVAL_DATETIME.SetText("");
 
    _O_IS_ACTIVE.SetChecked(1);
    _STATUS.SetText("");
    validatereset();
}

function ActiveFormOrder(active) {
    _WORKING_DATE.SetEnabled(active);
    _SHIFT.SetEnabled(active);
    _O_SUPPLIER_NAME.SetEnabled(active);
    _O_SUPPLIER_CODE.SetEnabled(active);
    _O_ORDER_NO.SetEnabled(active);
    _O_ORDER_DATETIME.SetEnabled(active);
    _O_TRIP_NO.SetEnabled(active);
    _TRUCK_NO.SetEnabled(active);
    _O_EST_ARRIVAL_DATETIME.SetEnabled(active);
 
    _O_IS_ACTIVE.SetEnabled(active);
    _STATUS.SetEnabled(active);

}

function GetObjectOrder() {
    return {
        ID: _ORDER_ID,
        WORKING_DATE: ConvertDate(_WORKING_DATE.GetText().trim()),
        SHIFT: _SHIFT.GetText().trim(),
        SUPPLIER_NAME: _O_SUPPLIER_NAME.GetText().trim(),
        SUPPLIER_CODE: _O_SUPPLIER_CODE.GetText().trim(),
        ORDER_NO: _O_ORDER_NO.GetText().trim(),
        ORDER_DATETIME: ConvertDate2(_O_ORDER_DATETIME.GetText().trim()),
        TRIP_NO: _O_TRIP_NO.GetText().trim(),
        TRUCK_NO: _TRUCK_NO.GetText().trim(),
        EST_ARRIVAL_DATETIME: ConvertDate2(_O_EST_ARRIVAL_DATETIME.GetText().trim()),
     
        IS_ACTIVE: (_O_IS_ACTIVE.GetChecked()) ? "Y" : "N",
        STATUS: _STATUS.GetText().trim()

    }
}

function DeleteOrder(s) {
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
        url: baseUrl + "/TB_R_DAILY_ORDER/Delete",
        method: "post",
        dataType: "json",
        data: {
            sid: $btn.attr("data-id")
        }
    }).done(function (response) {
        if (response.success) {
            //DoSearch();
            gvOrderList.PerformCallback();
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

function OnBtnOrderCancelClicked() {
    popTB_R_DAILY_ORDER.Hide()
}

function OnBtnOrderUpdateClicked(s) {
    var $btn = $(s);
    if ($btn.hasClass("saving")) {
        return;
    }
    if (!doValidateParam("ORDER")) {
        return;
    }
    ActiveFormOrder(false);
    $btn.addClass("saving");
    //$("#dealerStatus").html("Saving...");
    
    $.ajax({
        url: baseUrl + "/TB_R_DAILY_ORDER/SaveData",
        method: "post",
        data: GetObjectOrder(),
        dataType: "json"
    }).done(function (response) {
        ActiveFormOrder(true);
        if (response.success) {
            popTB_R_DAILY_ORDER.Hide();
            gvOrderList.PerformCallback();
        }
        else {
            alert(response.message);
        }
    }).always(function () {
        //$("#dealerStatus").html("");
        $btn.removeClass("saving");
 
    });
    
}

function AddOrder() {
    OnPopTB_R_DAILY_ORDERCloseUp();
    popTB_R_DAILY_ORDER.Show();
    return false;
}

/******************************* DETAIL *********************************/
function showdetail() {
    popDetails.Show();
}

function OnpopDetailsCloseUp() {

}

function SelectORDER_MONTH() {

    var Is_show_order = "N";
    if (_cbPIVOT_DETAILS.GetChecked()) {
        Is_show_order = "Y";
    }

    $.ajax({
        url: baseUrl + "/TB_R_CONTENT_LIST/DAILY_ORDER_SetDetails",
        method: "get",
        //dataType: "json",
        data: {
            WORKING_MONTH: _txtWORKING_MONTH.GetText().trim(),
            SUPPLIER_CODE: _txtSUPPLIER_DETAILS.GetText().trim(),
            IS_SHOW_ORDER: Is_show_order
        }
    }).done(function (response) {
        gvDetailsList.PerformCallback();
    });


}

/******************************* Generate Monthly Order *********************************/ 
function SHOW_GENERATE_MONTHLY_ORDER() {

    OnPopGENERATE_MONTHLY_ORDERCloseUp();
    popGENERATE_MONTHLY_ORDER.Show();

    return false;
}

function OnPopGENERATE_MONTHLY_ORDERCloseUp() {
    ClearFormGENERATE();
}

function ClearFormGENERATE() {    
    _SUPPLIER_TO_ORDER.SetSelectedIndex(-1);
    _ORDER_FROM_DATE.SetText("");
    popGENERATE_MONTHLY_ORDER.Hide();
    //aspxUClearFileInputClick('_IMPORT_SUPPLIER_INFO', 0);
    //validatereset();
}
 
function OnBtnMONTHLY_ORDERUpdateClicked(s) {
   
    //Re-confirm want to do
    if (!confirm("Are you sure to do Generating Order?"))
        return;

    var $btn = $(s);
    if ($btn.hasClass("saving")) {
        return;
    }
    if (!doValidateParam("MONTHLY_ORDER")) {
        return;
    }

    if (_SUPPLIER_TO_ORDER.GetSelectedValues().length == 0) {
        msgError("Please select Suppliers!");
        return;
    }

    ActiveFormMONTHLY_ORDER(false);
    $btn.addClass("saving");
    //$("#dealerStatus").html("Saving...");
    LoadingPanel_GENERATE_MONTHLY_ORDER.Show();

    $.ajax({
        url: baseUrl + "/TB_R_DAILY_ORDER/GENERATE_MONTHLY_V2", //2020-08-13: use V2 - GENERATE_MONTHLY
        method: "post",
        data: GetObjectMONTHLY_ORDER(),
        dataType: "json"
    }).done(function (response) {
        
        if (response.success) {
            popGENERATE_MONTHLY_ORDER.Hide();
            msgOk(response.message);
        }
        else {
            msgError(response.message);            
        }
       
    }).always(function () {
        //$("#dealerStatus").html("");
        ActiveFormMONTHLY_ORDER(true);
        $btn.removeClass("saving");
        LoadingPanel_GENERATE_MONTHLY_ORDER.Hide();
    });
}

function ActiveFormMONTHLY_ORDER(active) {
 
    _SUPPLIER_TO_ORDER.SetEnabled(active);
    _ORDER_FROM_DATE.SetEnabled(active);

    BtnMONTHLY_ORDER.SetEnabled(active);

}

function SUPPLIER_ORDERSelectionChanged(s, e) {
    var selectedSUPPLIER = s.GetSelectedValues();
    var removeIndices = new Array();
    // allow 3 suppliers only
    if (selectedSUPPLIER.length > 1) {
        msgError("Can't select more than 1 Suppliers!");
        //remove last selected item
        removeIndices.push(e.index);
        s.UnselectIndices(removeIndices);
    }        
}

function GetObjectMONTHLY_ORDER() {    
    return {        
        SUPPLIER_NAME: _SUPPLIER_TO_ORDER.GetSelectedValues().join(),
        //SUPPLIER_NAME: _SUPPLIER_TO_ORDER.GetValue(),
        ORDER_FROM_DATE: ConvertDate(_ORDER_FROM_DATE.GetText().trim()),
        IS_PP_OUT_CAL: (_IS_PP_OUT_CAL.GetChecked()) ? "Y" : "N"
    }
}

function GENERATE_KEIHEN_MONTHLY_ORDER(BASE_ORDER_ID) {
    //ask to re-run generate daily order
    $.confirm({
        title: 'Do you want to re-run generate Daily Order?',
        icon: 'fa fa-warning',
        content: 'Please be aware that Daily Order of next day could be changed!',
        buttons: {
            confirm: {
                btnClass: 'btn-warning',
                text: 'Confirm',
                action: function () {                           
                    $.ajax({
                        url: baseUrl + "/TB_R_DAILY_ORDER/GENERATE_KEIHEN_MONTHLY",
                        method: "post",
                        dataType: "json",
                        data: {
                            BASE_ORDER_ID: BASE_ORDER_ID
                        }
                    }).done(function (response) {
                        if (response.success) {
                            msgOk(response.message);
                        }
                        else {
                            msgError(response.message);
                        }
                    });
                }
            },
            cancel: function () {
                //$.alert('Canceled!');
            }
        }
    });
}

/******************************* Download Monthly Order *********************************/
var is_W_Forecast = "";
function SHOW_DOWNLOAD_MONTHLY_ORDER() {
    is_W_Forecast = ""; //without FC
    OnPopDOWNLOAD_MONTHLY_ORDERCloseUp();
    popDOWNLOAD_MONTHLY_ORDER.Show();
    return false;
}

function SHOW_DOWNLOAD_MONTHLY_ORDER_FC() {
    is_W_Forecast = "Y"; //with FC
    OnPopDOWNLOAD_MONTHLY_ORDERCloseUp();
    popDOWNLOAD_MONTHLY_ORDER.Show();
    return false;
}

function OnPopDOWNLOAD_MONTHLY_ORDERCloseUp() {
    ClearFormDOWNLOAD_ORDER();
}

function ClearFormDOWNLOAD_ORDER() {
    _txtSUPPLIER_ID.SelectIndex(0);
    _txtMONTH_ORDER.SetText("");
    popDOWNLOAD_MONTHLY_ORDER.Hide();
}

function OnBtnDownloadORDERClicked(s) {

    //Re-confirm want to do
    if (!confirm("Are you sure to do Download Order?"))
        return;

    var $btn = $(s);    
    if ($btn.hasClass("saving") || _txtSUPPLIER_ID.GetValue() == "" || _txtMONTH_ORDER.GetText().trim() == "") {
        return;
    }
        
    $btn.addClass("saving");
    ActiveFormDOWNLOAD_MONTHLY_ORDER(false);
    LoadingPanel_DOWNLOAD_MONTHLY_ORDER.Show();
     
    $.ajax({
        url: baseUrl + "/TB_R_DAILY_ORDER/DOWNLOAD_MONTHLY_ORDER",
        method: "post",
        data: GetObjectDOWNLOAD_MONTHLY_ORDER(),
        dataType: "json"
    }).done(function (response) {

        if (response.success) {
            window.open(response.DOWNLOAD_URL);
            popDOWNLOAD_MONTHLY_ORDER.Hide();
        }
        else {
            alert(response.message);
        }

    }).always(function () {
        ActiveFormDOWNLOAD_MONTHLY_ORDER(true);
        $btn.removeClass("saving");
        LoadingPanel_DOWNLOAD_MONTHLY_ORDER.Hide();
    });
}

function ActiveFormDOWNLOAD_MONTHLY_ORDER(active) {
    _txtSUPPLIER_ID.SetEnabled(active);
    _txtMONTH_ORDER.SetEnabled(active);

    BtnDownloadORDER.SetEnabled(active);
}

function GetObjectDOWNLOAD_MONTHLY_ORDER() {
    return {
        SUPPLIER_ID: _txtSUPPLIER_ID.GetValue(),
        MONTH_ORDER: _txtMONTH_ORDER.GetText().trim(),
        W_FC: is_W_Forecast
    }
}

/******************************* Download Monthly GRN *********************************/
function SHOW_DOWNLOAD_MONTHLY_GRN() {

    OnPopDOWNLOAD_MONTHLY_GRNCloseUp();
    popDOWNLOAD_MONTHLY_GRN.Show();
    return false;
}

function OnPopDOWNLOAD_MONTHLY_GRNCloseUp() {
    ClearFormDOWNLOAD_GRN();
}

function ClearFormDOWNLOAD_GRN() {
    _txtSUPPLIER_ID_GRN.SelectIndex(0);
    _txtMONTH_GRN.SetText("");
    popDOWNLOAD_MONTHLY_GRN.Hide();
}

function OnBtnDownloadGRNClicked(s) {

    //Re-confirm want to do
    if (!confirm("Are you sure to do Download GRN?"))
        return;
    
    if (_txtSUPPLIER_ID_GRN.GetValue() == "" || _txtMONTH_GRN.GetText().trim() == "") {
        return;
    }
    
    ActiveFormDOWNLOAD_MONTHLY_GRN(false);
    LoadingPanel.Show();

    $.ajax({
        url: baseUrl + "/TB_R_DAILY_ORDER/DOWNLOAD_MONTHLY_GRN",
        method: "post",
        data: GetObjectDOWNLOAD_MONTHLY_GRN(),
        dataType: "json"
    }).done(function (response) {

        if (response.success) {
            window.open(response.DOWNLOAD_URL);
            popDOWNLOAD_MONTHLY_GRN.Hide();
        }
        else {
            alert(response.message);
        }

    }).always(function () {
        ActiveFormDOWNLOAD_MONTHLY_GRN(true);        
        LoadingPanel.Hide();
    });

}

function ActiveFormDOWNLOAD_MONTHLY_GRN(active) {
    _txtSUPPLIER_ID_GRN.SetEnabled(active);
    _txtMONTH_GRN.SetEnabled(active);

    BtnDownloadGRN.SetEnabled(active);
}

function GetObjectDOWNLOAD_MONTHLY_GRN() {
    return {
        SUPPLIER_ID: _txtSUPPLIER_ID_GRN.GetValue(),
        MONTH_GRN: _txtMONTH_GRN.GetText().trim()
    }
}

/******************************* Download Order to Excel*********************************/
function OrderDelivery_SaveToExcel(s) {
    
    LoadingPanel.Show();
    $.ajax({
        url: baseUrl + "/TB_R_DAILY_ORDER/ORDER_DELIVERY_SaveToExcel",
        method: "post",
        dataType: "json",
        data: {
            ORDER_ID: gvOrderList.GetRowKey(gvOrderList.GetFocusedRowIndex())
        }
    }).done(function (response) {

        if (response.success != "") {
            window.open(response.DOWNLOAD_URL);
        }

    }).always(function () {

        LoadingPanel.Hide();
    });
    return false;
}

/******************************* Send Multiple Order *********************************/
function SHOW_SEND_MULTIPLE_ORDER() {

    OnPopSEND_MULTIPLE_ORDERCloseUp();
    popSEND_MULTIPLE_ORDER.Show();

    return false;
}

function OnPopSEND_MULTIPLE_ORDERCloseUp() {
    ClearFormSEND_MULTIPLE_ORDER();
}

function ClearFormSEND_MULTIPLE_ORDER() {
    //_SUPPLIER_TO_SEND_ORDER.SetSelectedIndex(-1);
    _ORDER_SEND_DATE.SetText("");
    popSEND_MULTIPLE_ORDER.Hide();   
}

function OnBtnSEND_MULTIPLE_ORDERUpdateClicked(s) {
   
    if (!doValidateParam("SEND_ORDER")) {
        return;
    }

    if (_SUPPLIER_TO_SEND_ORDER.GetSelectedValues().length == 0) {
        msgError("Please select Suppliers!");
        return;
    }

    //Re-confirm want to do
    var selectedSuppliers = _SUPPLIER_TO_SEND_ORDER.GetSelectedValues().join();
    if (!confirm("Are you sure to do Send MULTIPLE Orders? " + selectedSuppliers))
        return;

    ActiveFormSEND_MULTIPLE_ORDER(false);
    LoadingPanel_SEND_MULTIPLE_ORDER.Show();

    $.ajax({
        url: baseUrl + "/TB_R_CONTENT_LIST/EMAIL_SEND_MULTIPLE_ORDER",
        method: "post",
        data: GetObjectSEND_MULTIPLE_ORDER(),
        dataType: "json"
    }).done(function (response) {

        if (response.success) {
            popSEND_MULTIPLE_ORDER.Hide();
            msgOk(response.message);
        }
        else {
            msgError(response.message);
        }

    }).always(function () {        
        ActiveFormSEND_MULTIPLE_ORDER(true);        
        LoadingPanel_SEND_MULTIPLE_ORDER.Hide();
    });
}

function ActiveFormSEND_MULTIPLE_ORDER(active) {

    _SUPPLIER_TO_SEND_ORDER.SetEnabled(active);
    _ORDER_SEND_DATE.SetEnabled(active);

    BtnSendMultiORDER.SetEnabled(active);

}

function SUPPLIER_SEND_ORDERSelectionChanged(s, e) {
    var selectedSUPPLIER = s.GetSelectedValues();
    var removeIndices = new Array();
    // allow 50 suppliers only
    if (selectedSUPPLIER.length > 50) {
        msgError("Can't select more than 50 Suppliers!");
        //remove last selected item
        removeIndices.push(e.index);
        s.UnselectIndices(removeIndices);
    }
}

function CheckBoxList_Init(s, e) {  
    var indicesToSelect = new Array();  
    for (var i = 0; i < s.GetItemCount() ; i++) {        
        indicesToSelect.push(i);         
    }  
    s.SelectIndices(indicesToSelect);
}  

function GetObjectSEND_MULTIPLE_ORDER() {
    return {
        SUPPLIER_NAME: _SUPPLIER_TO_SEND_ORDER.GetSelectedValues().join(),
        ORDER_SEND_DATE: ConvertDate(_ORDER_SEND_DATE.GetText().trim())
    }
}