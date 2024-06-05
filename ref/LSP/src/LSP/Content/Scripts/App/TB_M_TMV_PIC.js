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
        url: baseUrl + "/TB_M_TMV_PIC/SetObjectInfo",
        method: "post",
        data: {
            PIC_NAME: _txtPIC_NAME.GetText().trim(),
            SUPPLIERS: _txtSUPPLIERS.GetText().trim(),
            IS_ACTIVE: _txtIS_ACTIVE.GetValue()
        }
    }).done(function (response) {
        gvList.PerformCallback();
    });
}

function ClearSearch() {
    DoSearch();
}







