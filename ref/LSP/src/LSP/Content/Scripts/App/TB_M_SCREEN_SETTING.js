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
        url: baseUrl + "/TB_M_SCREEN_SETTING/SetObjectInfo",
        method: "post",
        data: {
            SCREEN_NAME: _txtSCREEN_NAME.GetText().trim()                       
        }
    }).done(function (response) {
        gvList.PerformCallback();        
    });
}
