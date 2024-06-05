var _id = -1;
var _searchTimeout;
var _searchXhr;

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
        url: baseUrl + "/TB_R_PART_RUNDOWN_V2/SetObjectInfo",
        method: "post",
        data: {
            SUPPLIER_CODE: _txtSUPPLIER_CODE.GetText(),
            PART_NO: _txtPART_NO.GetText(),
            STOCK_MONTH_FROM: _txtSTOCK_MONTH_FROM.GetText().trim(),
            SHOP: _txtSHOP.GetValue()
        }
    }).done(function (response) {
        gvList.PerformCallback();
    });
}

function ClearSearch() {
    DoSearch();
}

function Validate() {
    
	return doValidate();
}

/*IMPORT*/
function DOWNLOAD_PART_RUNDOWN_TEMPLATE() {

    window.open("/Content/Template/Initial Rundown Stock_Template.xls");
}

function IMPORT_PART_RUNDOWN() {
    OnPopIMPORT_PART_RUNDOWNCloseUp();
    popIMPORT_PART_RUNDOWN.Show();

    return false;
}

function OnPopIMPORT_PART_RUNDOWNCloseUp() {
    ClearFormIMPORT_PART_RUNDOWN();
}

function ClearFormIMPORT_PART_RUNDOWN() {
    //aspxUClearFileInputClick('_IMPORT_INITIAL_RD', 0);
}

function IMPORT_PART_RUNDOWN_START(s, e) {        
    _IMPORT_PART_RUNDOWN.Upload();
    LoadingPanel.Show();
}
function IMPORT_PART_RUNDOWN_OnFileUploadComplete(s, e) {        
    msgOk(e.callbackData);
    if (e.isValid) {
        popIMPORT_PART_RUNDOWN.Hide();
        gvList.PerformCallback();
    }
    LoadingPanel.Hide();
}

function OnBtnImportCancelClicked() {
    popIMPORT_PART_RUNDOWN.Hide()
}







