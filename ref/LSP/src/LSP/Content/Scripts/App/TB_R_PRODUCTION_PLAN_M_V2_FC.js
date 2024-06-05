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
        url: baseUrl + "/TB_R_PRODUCTION_PLAN_M_V2_FC/SetObjectInfo",
    method: "post",
    data: {
        CFC: _txtCFC.GetText(),
        PROD_SFX: _txtPROD_SFX.GetText(),
        PART_NO:_txtPART_NO.GetText(),
        SUPPLIER_CODE:_txtSUPPLIER_CODE.GetText(),
        PRODUCTION_MONTH: _txtPRODUCTION_MONTH.GetText().trim()
    }
    }).done(function (response) {
        gvList.PerformCallback();
    });
}

