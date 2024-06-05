var _id = -1;
var _searchTimeout;
var _searchXhr;

function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_R_KANBAN_REPORT/TB_R_KANBAN_REPORT_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_R_KANBAN_REPORT']").html("Edit RECEIVING REPORT");                    
			_SUPPLIER_CODE.SetText(response.SUPPLIER_CODE);
			_ORDER_NO.SetText(response.ORDER_NO);
			_CONTENT_NO.SetText(response.CONTENT_NO);
			_RECEIVING_PIC.SetText(response.RECEIVING_PIC);
			_RECEIVING_CAUSE.SetText(response.RECEIVING_CAUSE);
			_RECEIVING_COUTERMEASURE.SetText(response.RECEIVING_COUTERMEASURE);
			_RECEIVING_PIC_ACTION.SetText(response.RECEIVING_PIC_ACTION);
			_RECEIVING_PIC_RESULT.SetText(response.RECEIVING_PIC_RESULT);
			
    }).fail(function () {
        OnPopTB_R_KANBAN_REPORTCloseUp();
    })
    popTB_R_KANBAN_REPORT.Show();
    return false;
}

function ReflectAlarm(t) {
    var $btn = $(t)    
    _id = $btn.attr("data-id");
    _RECEIVING_STATUS = $btn.attr("data-type");    

    //ask to reflect & alarm
    $.confirm({
        title: 'Do you want to Reflect and turn off Alarm?',
        icon: 'fa fa-warning',        
        content: 'Please be aware that Daily Stock will be changed!' +
               '<form action="" class="formName">' +
               '<div class="form-group">' +
               '<label>LEADER nhập mã xác nhận:</label>' +
               '<input type="text" placeholder="Mã xác nhận" class="CONFIRM_CODE form-control" required />' +
               '</div>' +
               '</form>',
        buttons: {
            confirm: {
                btnClass: 'btn-warning',
                text: 'Confirm',
                action: function () {
                    var _confirm_code = this.$content.find('.CONFIRM_CODE').val();
                    if (!_confirm_code) {
                        $.alert('Chưa nhập mã xác nhận!');
                        return false;
                    }

                    $.ajax({
                        url: baseUrl + "/TB_R_KANBAN_REPORT/TB_R_KANBAN_REPORT_ALARM",
                        method: "post",
                        dataType: "json",
                        data: {
                            sid: _id,
                            RECEIVING_STATUS: _RECEIVING_STATUS,
                            CONFIRM_CODE: _confirm_code
                        }
                    }).done(function (response) {
                        if (response.success) {
                            msgOk("Reflect and turn off Alarm successfully!");                            
                            gvKanbanListReport.PerformCallback();
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

function OnBtnUpdateClicked(s) {
    var $btn = $(s);
    if ($btn.hasClass("saving")) {
        return;
    }
    if (!doValidateParam("KANBAN_REPORT")) {
        return;
    }
    ActiveForm(false);
    $btn.addClass("saving");
    //$("#dealerStatus").html("Saving...");
    $.ajax({
        url: baseUrl + "/TB_R_KANBAN_REPORT/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_R_KANBAN_REPORT.Hide();
            //DoSearch();
            gvKanbanListReport.PerformCallback();
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
    popTB_R_KANBAN_REPORT.Hide()
}

function OnPopTB_R_KANBAN_REPORTCloseUp() {
    _id = -1;
    $("span[id^='popTB_R_KANBAN_REPORT']").html("Add RECEIVING REPORT")
	ClearForm();
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

function Search() {
    _searchXhr = $.ajax({
        url: baseUrl + "/TB_R_KANBAN_REPORT/SetObjectInfo",
        method: "post",
        data: {
            SUPPLIER_CODE: _txtSUPPLIER_CODE.GetText().trim(),
            ORDER_NO: _txtORDER_NO.GetText().trim(),
            CONTENT_NO: _txtCONTENT_NO.GetText().trim(),
            PART_NO: _txtPART_NO.GetText().trim(),
            WORKING_DATE: ConvertDate(_txtWORKING_DATE.GetText().trim()),
            WORKING_DATE_TO: ConvertDate(_txtWORKING_DATE_TO.GetText().trim()),
            RECEIVING_ISSUE: _txtRECEIVING_ISSUE.GetValue()
        }
    }).done(function (response) {
        gvKanbanListReport.PerformCallback();
    });
}

function ClearForm() {	
	_SUPPLIER_CODE.SetText("");
	_ORDER_NO.SetText("");
	_CONTENT_NO.SetText("");
	_RECEIVING_PIC.SetText("");
	_RECEIVING_CAUSE.SetText("");
	_RECEIVING_COUTERMEASURE.SetText("");
	_RECEIVING_PIC_ACTION.SetText("");
	_RECEIVING_PIC_RESULT.SetText(""); 
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
    _txtWORKING_DATE_TO.SetText("");
    _txtCONTENT_NO.SetText("");
    _txtPART_NO.SetText("");
}

function ActiveForm(active) {
    _SUPPLIER_CODE.SetEnabled(active);
    _ORDER_NO.SetEnabled(active);
    _CONTENT_NO.SetEnabled(active);
    _RECEIVING_PIC.SetEnabled(active);
    _RECEIVING_CAUSE.SetEnabled(active);
    _RECEIVING_COUTERMEASURE.SetEnabled(active);
    _RECEIVING_PIC_ACTION.SetEnabled(active);
    _RECEIVING_PIC_RESULT.SetEnabled(active);	
}

function GetObject() {
    return {
        ID: _id,        		
        RECEIVING_PIC: _RECEIVING_PIC.GetText().trim(),
        RECEIVING_CAUSE: _RECEIVING_CAUSE.GetText().trim(),
        RECEIVING_COUTERMEASURE: _RECEIVING_COUTERMEASURE.GetText().trim(),
        RECEIVING_PIC_ACTION: _RECEIVING_PIC_ACTION.GetText().trim(),
        RECEIVING_PIC_RESULT: _RECEIVING_PIC_RESULT.GetText().trim()		 
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
