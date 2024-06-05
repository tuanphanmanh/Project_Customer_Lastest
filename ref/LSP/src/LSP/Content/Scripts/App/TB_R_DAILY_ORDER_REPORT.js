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

/////////Editer
function Search() {
    _searchXhr = $.ajax({
        url: baseUrl + "/TB_R_DAILY_ORDER_REPORT/SetObjectInfo",
        method: "post",
        data: {
            ORDER_MONTH: _txtORDER_MONTH.GetText().trim(),
            WORKING_DATE:  _txtWORKING_DATE.GetText().trim(),
            SUPPLIER_CODE: _txtSUPPLIER_CODE.GetText().trim(),
            ORDER_NO:      _txtORDER_NO.GetText().trim(),
            PART_NO:       _txtPART_NO.GetText().trim()           
        }
    }).done(function (response) {
        gvList.PerformCallback();
    });
}

function ClearSearch() {
    DoClearSearch();
    DoSearch();
}

function DoClearSearch() {
    _txtORDER_MONTH.SetText("");
    _txtWORKING_DATE.SetText("");
    _txtSUPPLIER_CODE.SetText("");
    _txtORDER_NO.SetText("");
    _txtPART_NO.SetText("");
}