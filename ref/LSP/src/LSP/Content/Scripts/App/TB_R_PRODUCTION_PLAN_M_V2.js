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
    url: baseUrl + "/TB_R_PRODUCTION_PLAN_M_V2/SetObjectInfo",
    method: "post",
    data: {
        CFC: _txtCFC.GetText(),
        PROD_SFX: _txtPROD_SFX.GetText(),
        PRODUCTION_MONTH: _txtPRODUCTION_MONTH.GetText().trim()
    }
    }).done(function (response) {
        gvList.PerformCallback();
    });
}

//2021
/*******************IMPORT JOBS***************/
function DOWNLOAD_PP_TEMPLATE_V2() {
    window.open("/Content/Template/PRODUCTION_PLAN_Template.xlsx", '_blank');
}

function IMPORT_PRODUCTION_PLAN_V2() {
    OnPopPRODUCTION_PLAN_V2CloseUp();
    popPRODUCTION_PLAN_V2.Show();

    return false;
}

function OnPopPRODUCTION_PLAN_V2CloseUp() {
    ClearFormPRODUCTION_PLAN_V2();
}

function ClearFormPRODUCTION_PLAN_V2() {
}

function PRODUCTION_PLAN_V2_OnFileUploadComplete(s, e) {
    if (e.isValid) {
        popPRODUCTION_PLAN_V2.Hide();
        gvList.PerformCallback();
        msgOk("UPLOAD 'PRODUCTION PLAN V2' THÀNH CÔNG.");
    }
    else {
        var sError = e.callbackData;
        msgError("UPLOAD 'PRODUCTION PLAN V2' KHÔNG THÀNH CÔNG. <BR/> CÓ THỂ DỮ LIỆU BỊ SAI HOẶC TRÙNG LẶP! <br/> Thông tin Lỗi: <br/>" + sError);
    }
    LoadingPanel.Hide();
}

function PRODUCTION_PLAN_V2_UPLOAD(s, e) {
    _IMPORT_PRODUCTION_PLAN_V2.Upload();
    LoadingPanel.Show();
}

function OnBtnImportCancelClicked() {
    popPRODUCTION_PLAN_V2.Hide()
}
