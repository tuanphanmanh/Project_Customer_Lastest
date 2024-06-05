var _id = -1;
var _searchTimeout;
var _searchXhr;

function Add() {
    OnPopTB_M_SUPPLIER_STK_CONCEPTCloseUp();
    popTB_M_SUPPLIER_STK_CONCEPT.Show();
    return false;
}

function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_M_SUPPLIER_STK_CONCEPT/TB_M_SUPPLIER_STK_CONCEPT_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_M_SUPPLIER_STK_CONCEPT']").html("Edit SUPPLIER STK CONCEPT");
         
     
            _SUPPLIER_CODE.SetValue(response.SUPPLIER_CODE);
            _MONTH_STK.SetText(response.MONTH_STK_Str_MMYYYY);
            _STK_CONCEPT.SetValue(response.STK_CONCEPT);
            _STK_CONCEPT_FRQ.SetValue(response.STK_CONCEPT_FRQ);
			_MIN_STK_1.SetText(response.MIN_STK_1);
			_MIN_STK_2.SetText(response.MIN_STK_2);
			_MIN_STK_3.SetText(response.MIN_STK_3);
			_MIN_STK_4.SetText(response.MIN_STK_4);
			_MIN_STK_5.SetText(response.MIN_STK_5);
			_MIN_STK_6.SetText(response.MIN_STK_6);
			_MIN_STK_7.SetText(response.MIN_STK_7);
			_MIN_STK_8.SetText(response.MIN_STK_8);
			_MIN_STK_9.SetText(response.MIN_STK_9);
			_MIN_STK_10.SetText(response.MIN_STK_10);
			_MIN_STK_11.SetText(response.MIN_STK_11);
			_MIN_STK_12.SetText(response.MIN_STK_12);
			_MIN_STK_13.SetText(response.MIN_STK_13);
			_MIN_STK_14.SetText(response.MIN_STK_14);
			_MIN_STK_15.SetText(response.MIN_STK_15);
			_MAX_STK_1.SetText(response.MAX_STK_1);
			_MAX_STK_2.SetText(response.MAX_STK_2);
			_MAX_STK_3.SetText(response.MAX_STK_3);
			_MAX_STK_4.SetText(response.MAX_STK_4);
			_MAX_STK_5.SetText(response.MAX_STK_5);
			//_MIN_STK_CONCEPT.SetText(response.MIN_STK_CONCEPT);
			//_MAX_STK_CONCEPT.SetText(response.MAX_STK_CONCEPT);
			_IS_ACTIVE.SetChecked((response.IS_ACTIVE == "Y") ? 1 : 0);
			//_CREATED_BY.SetText(response.CREATED_BY);
			//_CREATED_DATE.SetText(response.CREATED_DATE_Str_DDMMYYYY);
			//_UPDATED_BY.SetText(response.UPDATED_BY);
			//_UPDATED_DATE.SetText(response.UPDATED_DATE_Str_DDMMYYYY);
		   
    }).fail(function () {
        OnPopTB_M_SUPPLIER_STK_CONCEPTCloseUp();
    })
    popTB_M_SUPPLIER_STK_CONCEPT.Show();
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
        url: baseUrl + "/TB_M_SUPPLIER_STK_CONCEPT/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_M_SUPPLIER_STK_CONCEPT.Hide();
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
    popTB_M_SUPPLIER_STK_CONCEPT.Hide()
}

function OnPopTB_M_SUPPLIER_STK_CONCEPTCloseUp() {
	_id = -1
	$("span[id^='popTB_M_SUPPLIER_STK_CONCEPT']").html("Add New SUPPLIER STK CONCEPT")
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
        url: baseUrl + "/TB_M_SUPPLIER_STK_CONCEPT/Delete",
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
        url: baseUrl + "/TB_M_SUPPLIER_STK_CONCEPT/SetObjectInfo",
        method: "post",
        data: {
            SUPPLIER_CODE: _txtSUPPLIER_CODE.GetText().trim(),
            MONTH_STK: _txtMONTH_STK.GetText().trim()
        }
    }).done(function (response) {
        gvList.PerformCallback();
    });
}

function ClearForm() { 
    _SUPPLIER_CODE.SelectIndex(0);
    _MONTH_STK.SetText("");
    _STK_CONCEPT.SelectIndex(0);
    _STK_CONCEPT_FRQ.SetText("");
	_MIN_STK_1.SetText("");
	_MIN_STK_2.SetText("");
	_MIN_STK_3.SetText("");
	_MIN_STK_4.SetText("");
	_MIN_STK_5.SetText("");
	_MIN_STK_6.SetText("");
	_MIN_STK_7.SetText("");
	_MIN_STK_8.SetText("");
	_MIN_STK_9.SetText("");
	_MIN_STK_10.SetText("");
	_MIN_STK_11.SetText("");
	_MIN_STK_12.SetText("");
	_MIN_STK_13.SetText("");
	_MIN_STK_14.SetText("");
	_MIN_STK_15.SetText("");
	_MAX_STK_1.SetText("");
	_MAX_STK_2.SetText("");
	_MAX_STK_3.SetText("");
	_MAX_STK_4.SetText("");
	_MAX_STK_5.SetText("");
	//_MIN_STK_CONCEPT.SetText("");
	//_MAX_STK_CONCEPT.SetText("");
	_IS_ACTIVE.SetChecked(1);
	//_CREATED_BY.SetText("");
	//_CREATED_DATE.SetText("");
	//_UPDATED_BY.SetText("");
	//_UPDATED_DATE.SetText("");
	validatereset();
}

function ClearSearch() {
    DoSearch();
}

function ActiveForm(active) {
	_SUPPLIER_CODE.SetEnabled(active);
	_MONTH_STK.SetEnabled(active);
	_STK_CONCEPT.SetEnabled(active);
	_STK_CONCEPT_FRQ.SetEnabled(active);
	_MIN_STK_1.SetEnabled(active);
	_MIN_STK_2.SetEnabled(active);
	_MIN_STK_3.SetEnabled(active);
	_MIN_STK_4.SetEnabled(active);
	_MIN_STK_5.SetEnabled(active);
	_MIN_STK_6.SetEnabled(active);
	_MIN_STK_7.SetEnabled(active);
	_MIN_STK_8.SetEnabled(active);
	_MIN_STK_9.SetEnabled(active);
	_MIN_STK_10.SetEnabled(active);
	_MIN_STK_11.SetEnabled(active);
	_MIN_STK_12.SetEnabled(active);
	_MIN_STK_13.SetEnabled(active);
	_MIN_STK_14.SetEnabled(active);
	_MIN_STK_15.SetEnabled(active);
	_MAX_STK_1.SetEnabled(active);
	_MAX_STK_2.SetEnabled(active);
	_MAX_STK_3.SetEnabled(active);
	_MAX_STK_4.SetEnabled(active);
	_MAX_STK_5.SetEnabled(active);
	//_MIN_STK_CONCEPT.SetEnabled(active);
	//_MAX_STK_CONCEPT.SetEnabled(active);
	_IS_ACTIVE.SetEnabled(active);
	//_CREATED_BY.SetEnabled(active);
	//_CREATED_DATE.SetEnabled(active);
	//_UPDATED_BY.SetEnabled(active);
	//_UPDATED_DATE.SetEnabled(active);
	
}

function GetObject() {
    return {
		ID: _id,  
		SUPPLIER_CODE: _SUPPLIER_CODE.GetValue(),
		MONTH_STK: ConvertDate(_MONTH_STK.GetText().trim()),
		STK_CONCEPT: _STK_CONCEPT.GetValue().trim(),
		STK_CONCEPT_FRQ: _STK_CONCEPT_FRQ.GetText().trim(),
		MIN_STK_1: _MIN_STK_1.GetText().trim(), 
		MIN_STK_2: _MIN_STK_2.GetText().trim(), 
		MIN_STK_3: _MIN_STK_3.GetText().trim(), 
		MIN_STK_4: _MIN_STK_4.GetText().trim(), 
		MIN_STK_5: _MIN_STK_5.GetText().trim(), 
		MIN_STK_6: _MIN_STK_6.GetText().trim(), 
		MIN_STK_7: _MIN_STK_7.GetText().trim(), 
		MIN_STK_8: _MIN_STK_8.GetText().trim(), 
		MIN_STK_9: _MIN_STK_9.GetText().trim(), 
		MIN_STK_10: _MIN_STK_10.GetText().trim(), 
		MIN_STK_11: _MIN_STK_11.GetText().trim(), 
		MIN_STK_12: _MIN_STK_12.GetText().trim(), 
		MIN_STK_13: _MIN_STK_13.GetText().trim(), 
		MIN_STK_14: _MIN_STK_14.GetText().trim(), 
		MIN_STK_15: _MIN_STK_15.GetText().trim(), 
		MAX_STK_1: _MAX_STK_1.GetText().trim(), 
		MAX_STK_2: _MAX_STK_2.GetText().trim(), 
		MAX_STK_3: _MAX_STK_3.GetText().trim(), 
		MAX_STK_4: _MAX_STK_4.GetText().trim(), 
		MAX_STK_5: _MAX_STK_5.GetText().trim(), 
		//MIN_STK_CONCEPT: _MIN_STK_CONCEPT.GetText().trim(), 
		//MAX_STK_CONCEPT: _MAX_STK_CONCEPT.GetText().trim(), 
		IS_ACTIVE: (_IS_ACTIVE.GetChecked()) ? "Y" : "N"
		//CREATED_BY: _CREATED_BY.GetText().trim(), 
		//CREATED_DATE: _CREATED_DATE.GetText().trim(), 
		//UPDATED_BY: _UPDATED_BY.GetText().trim(), 
		//UPDATED_DATE: _UPDATED_DATE.GetText().trim() 
	 
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
function IMPORT_SUPPLIER_STK_CONCEPT() {

    OnPopSUPPLIER_STK_CONCEPTCloseUp();
    popIMPORT_SUPPLIER_STK_CONCEPT.Show();

    return false;
}

function OnPopSUPPLIER_STK_CONCEPTCloseUp() {
    ClearFormSUPPLIER_STK_CONCEPT();
}

function ClearFormSUPPLIER_STK_CONCEPT() {
    //aspxUClearFileInputClick('_IMPORT_SUPPLIER_STK_CONCEPT', 0);
    //validatereset();
}

function SUPPLIER_STK_CONCEPT_OnFileUploadComplete(s, e) {
    //alert(e.callbackData);
    alert(e.callbackData);
    if (e.isValid) {
        popIMPORT_SUPPLIER_STK_CONCEPT.Hide();
        gvList.PerformCallback();
        //gvList.PerformCallback({ isCustomCallback: false, args: "" });
    }
}


function OnBtnImportCancelClicked() {
    popIMPORT_SUPPLIER_STK_CONCEPT.Hide();
}


/***************GENERATE_PART_STKCONCEPT_DETAILS*******************/
function GENERATE_PART_STKCONCEPT_DETAILS() {
    $.ajax({
        url: baseUrl + "/TB_M_LOOKUP/TB_M_LOOKUP_GetByDOMAIN_ITEMCODE",
        method: "post",
        dataType: "json",
        data: {
            DOMAIN_CODE: "STOCK",
            ITEM_CODE:"STOCK_CONCEPT"
        }
    }).done(function (response) {

        var _ITEM_VALUE = response.ITEM_VALUE;
        var _stkConceptSettig = "";
        switch (_ITEM_VALUE) {
            case "M":
                _stkConceptSettig = "M: Max-Min";                
                break;
            case "A":
                _stkConceptSettig = "A: Avg";
                break;
            case "MA":
                _stkConceptSettig = "MA: Max-min & Avg";
                break;
            default:
                _stkConceptSettig = "";
        }

        $.confirm({
            title: 'Are you sure want to continue?',
            icon: 'fa fa-warning',
            content: 'Current Stock Concept is "' + _stkConceptSettig + '". Please be aware that it will change STK MIN/MAX for each parts!',
            buttons: {
                confirm: {
                    btnClass: 'btn-warning',
                    text: 'Confirm',
                    action: function () {
                        $.ajax({
                            url: baseUrl + "/TB_M_SUPPLIER_STK_CONCEPT/GENERATE_PART_STKCONCEPT_DETAILS",
                            method: "post",
                            data: {
                            }
                        }).done(function (response) {
                            msgOk(response.message);
                            //gvList.PerformCallback();                        
                        });
                    }
                },
                cancel: function () {
                    //$.alert('Canceled!');
                }
            }
        });
               
    }).fail(function () {
        msgError("Error! Can't get information and do process");
    })    
}

/***************GENERATE_BYCOPY_NEW_MONTH*******************/
function GENERATE_BYCOPY_NEW_MONTH() {   
    $.confirm({
        title: 'Generate Stock Concept to month!',
        icon: 'fa fa-warning',
        content: 'Please choose target month to continue- CURRENT MONTH or to NEXT MONTH',
        boxWidth: '40%',
        useBootstrap: false,

        buttons: {
            confirm: {
                btnClass: 'btn-warning',
                text: 'Current Month',
                action: function () {
                    $.ajax({
                        url: baseUrl + "/TB_M_SUPPLIER_STK_CONCEPT/GENERATE_BYCOPY_MONTH",
                        method: "post",
                        data: {
                            Month_Type: "C" //Current
                        }
                    }).done(function (response) {
                        msgOk(response.message);
                        //gvList.PerformCallback();                        
                    });
                }
            },      
            somethingElse: {
                btnClass: 'btn-warning',
                text: 'Next Month',
                action: function () {
                    $.ajax({
                        url: baseUrl + "/TB_M_SUPPLIER_STK_CONCEPT/GENERATE_BYCOPY_MONTH",
                        method: "post",
                        data: {
                            Month_Type: "N" //Current
                        }
                    }).done(function (response) {
                        msgOk(response.message);
                        //gvList.PerformCallback();                        
                    });
                }
            },
            cancel: function () {
                //$.alert('Canceled!');
            }
        }
    });
}

