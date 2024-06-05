var _id = -1;
var _searchTimeout;
var _searchXhr;

function Add() {
    OnPopTB_R_PRODUCTION_PLAN_MCloseUp();
    popTB_R_PRODUCTION_PLAN_M.Show();
    return false;
}

function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_R_PRODUCTION_PLAN_M/TB_R_PRODUCTION_PLAN_M_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_R_PRODUCTION_PLAN_M']").html("Edit TB_R_PRODUCTION_PLAN_M");

        _CFC.SetText(response.CFC);
        _KATASHIKI.SetText(response.KATASHIKI);
        _PROD_SFX.SetText(response.PROD_SFX);
        _INT_COLOR.SetText(response.INT_COLOR);
        _EXT_COLOR.SetText(response.EXT_COLOR);
        _PRODUCTION_MONTH.SetText(response.PRODUCTION_MONTH_Str_DDMMYYYY);
        _LO_VOLUME.SetText(response.LO_VOLUME);
        _LO_VOLUME_DAY01.SetText(response.LO_VOLUME_DAY01);
        _LO_VOLUME_DAY02.SetText(response.LO_VOLUME_DAY02);
        _LO_VOLUME_DAY03.SetText(response.LO_VOLUME_DAY03);
        _LO_VOLUME_DAY04.SetText(response.LO_VOLUME_DAY04);
        _LO_VOLUME_DAY05.SetText(response.LO_VOLUME_DAY05);
        _LO_VOLUME_DAY06.SetText(response.LO_VOLUME_DAY06);
        _LO_VOLUME_DAY07.SetText(response.LO_VOLUME_DAY07);
        _LO_VOLUME_DAY08.SetText(response.LO_VOLUME_DAY08);
        _LO_VOLUME_DAY09.SetText(response.LO_VOLUME_DAY09);
        _LO_VOLUME_DAY10.SetText(response.LO_VOLUME_DAY10);
        _LO_VOLUME_DAY11.SetText(response.LO_VOLUME_DAY11);
        _LO_VOLUME_DAY12.SetText(response.LO_VOLUME_DAY12);
        _LO_VOLUME_DAY13.SetText(response.LO_VOLUME_DAY13);
        _LO_VOLUME_DAY14.SetText(response.LO_VOLUME_DAY14);
        _LO_VOLUME_DAY15.SetText(response.LO_VOLUME_DAY15);
        _LO_VOLUME_DAY16.SetText(response.LO_VOLUME_DAY16);
        _LO_VOLUME_DAY17.SetText(response.LO_VOLUME_DAY17);
        _LO_VOLUME_DAY18.SetText(response.LO_VOLUME_DAY18);
        _LO_VOLUME_DAY19.SetText(response.LO_VOLUME_DAY19);
        _LO_VOLUME_DAY20.SetText(response.LO_VOLUME_DAY20);
        _LO_VOLUME_DAY21.SetText(response.LO_VOLUME_DAY21);
        _LO_VOLUME_DAY22.SetText(response.LO_VOLUME_DAY22);
        _LO_VOLUME_DAY23.SetText(response.LO_VOLUME_DAY23);
        _LO_VOLUME_DAY24.SetText(response.LO_VOLUME_DAY24);
        _LO_VOLUME_DAY25.SetText(response.LO_VOLUME_DAY25);
        _LO_VOLUME_DAY26.SetText(response.LO_VOLUME_DAY26);
        _LO_VOLUME_DAY27.SetText(response.LO_VOLUME_DAY27);
        _LO_VOLUME_DAY28.SetText(response.LO_VOLUME_DAY28);
        _LO_VOLUME_DAY29.SetText(response.LO_VOLUME_DAY29);
        _LO_VOLUME_DAY30.SetText(response.LO_VOLUME_DAY30);
        _LO_VOLUME_DAY31.SetText(response.LO_VOLUME_DAY31);
        _IS_NQC_REQ_PROCESSED.SetText(response.IS_NQC_REQ_PROCESSED);
        _IS_NQC_RES_PROCESSED.SetText(response.IS_NQC_RES_PROCESSED);

    }).fail(function () {
        OnPopTB_R_PRODUCTION_PLAN_MCloseUp();
    })
    popTB_R_PRODUCTION_PLAN_M.Show();
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
    $.ajax({
        url: baseUrl + "/TB_R_PRODUCTION_PLAN_M/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_R_PRODUCTION_PLAN_M.Hide();
            DoSearch();
        }
        else {
            alert(response.message);
        }
    }).always(function () {
        $btn.removeClass("saving");
    });
}

function OnBtnCancelClicked() {
    popTB_R_PRODUCTION_PLAN_M.Hide()
}

function OnPopTB_R_PRODUCTION_PLAN_MCloseUp() {
    _id = -1
    $("span[id^='popTB_R_PRODUCTION_PLAN_M']").html("Add New TB_R_PRODUCTION_PLAN_M")
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
        url: baseUrl + "/TB_R_PRODUCTION_PLAN_M/Delete",
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

//Editer
function Search() {
    _searchXhr = $.ajax({
    url: baseUrl + "/TB_R_PRODUCTION_PLAN_M/SetObjectInfo",
    method: "post",
    data: {
        CFC: _txtCFC.GetText(),
        KATASHIKI: _txtKATASHIKI.GetText(),
        PROD_SFX: _txtPROD_SFX.GetText(),
        INT_COLOR: _txtINT_COLOR.GetText(),
        EXT_COLOR: _txtEXT_COLOR.GetText(),
        PRODUCTION_MONTH: _txtPRODUCTION_MONTH.GetText()
    }
    }).done(function (response) {
        gvList.PerformCallback();
    });
}

function ClearForm() {
    _CFC.SetText("");
    _KATASHIKI.SetText("");
    _PROD_SFX.SetText("");
    _INT_COLOR.SetText("");
    _EXT_COLOR.SetText("");
    _PRODUCTION_MONTH.SetText("");
    _LO_VOLUME.SetText("");
    _LO_VOLUME_DAY01.SetText("");
    _LO_VOLUME_DAY02.SetText("");
    _LO_VOLUME_DAY03.SetText("");
    _LO_VOLUME_DAY04.SetText("");
    _LO_VOLUME_DAY05.SetText("");
    _LO_VOLUME_DAY06.SetText("");
    _LO_VOLUME_DAY07.SetText("");
    _LO_VOLUME_DAY08.SetText("");
    _LO_VOLUME_DAY09.SetText("");
    _LO_VOLUME_DAY10.SetText("");
    _LO_VOLUME_DAY11.SetText("");
    _LO_VOLUME_DAY12.SetText("");
    _LO_VOLUME_DAY13.SetText("");
    _LO_VOLUME_DAY14.SetText("");
    _LO_VOLUME_DAY15.SetText("");
    _LO_VOLUME_DAY16.SetText("");
    _LO_VOLUME_DAY17.SetText("");
    _LO_VOLUME_DAY18.SetText("");
    _LO_VOLUME_DAY19.SetText("");
    _LO_VOLUME_DAY20.SetText("");
    _LO_VOLUME_DAY21.SetText("");
    _LO_VOLUME_DAY22.SetText("");
    _LO_VOLUME_DAY23.SetText("");
    _LO_VOLUME_DAY24.SetText("");
    _LO_VOLUME_DAY25.SetText("");
    _LO_VOLUME_DAY26.SetText("");
    _LO_VOLUME_DAY27.SetText("");
    _LO_VOLUME_DAY28.SetText("");
    _LO_VOLUME_DAY29.SetText("");
    _LO_VOLUME_DAY30.SetText("");
    _LO_VOLUME_DAY31.SetText("");
    _IS_NQC_REQ_PROCESSED.SetText("");
    _IS_NQC_RES_PROCESSED.SetText("");
    validatereset();
}

function ClearSearch() {
    DoSearch();
}

function ActiveForm(active) {

    _CFC.SetEnabled(active);
    _KATASHIKI.SetEnabled(active);
    _PROD_SFX.SetEnabled(active);
    _INT_COLOR.SetEnabled(active);
    _EXT_COLOR.SetEnabled(active);
    _PRODUCTION_MONTH.SetEnabled(active);
    _LO_VOLUME.SetEnabled(active);
    _LO_VOLUME_DAY01.SetEnabled(active);
    _LO_VOLUME_DAY02.SetEnabled(active);
    _LO_VOLUME_DAY03.SetEnabled(active);
    _LO_VOLUME_DAY04.SetEnabled(active);
    _LO_VOLUME_DAY05.SetEnabled(active);
    _LO_VOLUME_DAY06.SetEnabled(active);
    _LO_VOLUME_DAY07.SetEnabled(active);
    _LO_VOLUME_DAY08.SetEnabled(active);
    _LO_VOLUME_DAY09.SetEnabled(active);
    _LO_VOLUME_DAY10.SetEnabled(active);
    _LO_VOLUME_DAY11.SetEnabled(active);
    _LO_VOLUME_DAY12.SetEnabled(active);
    _LO_VOLUME_DAY13.SetEnabled(active);
    _LO_VOLUME_DAY14.SetEnabled(active);
    _LO_VOLUME_DAY15.SetEnabled(active);
    _LO_VOLUME_DAY16.SetEnabled(active);
    _LO_VOLUME_DAY17.SetEnabled(active);
    _LO_VOLUME_DAY18.SetEnabled(active);
    _LO_VOLUME_DAY19.SetEnabled(active);
    _LO_VOLUME_DAY20.SetEnabled(active);
    _LO_VOLUME_DAY21.SetEnabled(active);
    _LO_VOLUME_DAY22.SetEnabled(active);
    _LO_VOLUME_DAY23.SetEnabled(active);
    _LO_VOLUME_DAY24.SetEnabled(active);
    _LO_VOLUME_DAY25.SetEnabled(active);
    _LO_VOLUME_DAY26.SetEnabled(active);
    _LO_VOLUME_DAY27.SetEnabled(active);
    _LO_VOLUME_DAY28.SetEnabled(active);
    _LO_VOLUME_DAY29.SetEnabled(active);
    _LO_VOLUME_DAY30.SetEnabled(active);
    _LO_VOLUME_DAY31.SetEnabled(active);
    _IS_NQC_REQ_PROCESSED.SetEnabled(active);
    _IS_NQC_RES_PROCESSED.SetEnabled(active);
}

function GetObject() {
    return {
        id: _id,
        CFC: _CFC.GetText().trim(),
        KATASHIKI: _KATASHIKI.GetText().trim(),
        PROD_SFX: _PROD_SFX.GetText().trim(),
        INT_COLOR: _INT_COLOR.GetText().trim(),
        EXT_COLOR: _EXT_COLOR.GetText().trim(),
        PRODUCTION_MONTH: ConvertDate(_PRODUCTION_MONTH.GetText().trim()),
        LO_VOLUME: _LO_VOLUME.GetText().trim(),
        LO_VOLUME_DAY01: _LO_VOLUME_DAY01.GetText().trim(),
        LO_VOLUME_DAY02: _LO_VOLUME_DAY02.GetText().trim(),
        LO_VOLUME_DAY03: _LO_VOLUME_DAY03.GetText().trim(),
        LO_VOLUME_DAY04: _LO_VOLUME_DAY04.GetText().trim(),
        LO_VOLUME_DAY05: _LO_VOLUME_DAY05.GetText().trim(),
        LO_VOLUME_DAY06: _LO_VOLUME_DAY06.GetText().trim(),
        LO_VOLUME_DAY07: _LO_VOLUME_DAY07.GetText().trim(),
        LO_VOLUME_DAY08: _LO_VOLUME_DAY08.GetText().trim(),
        LO_VOLUME_DAY09: _LO_VOLUME_DAY09.GetText().trim(),
        LO_VOLUME_DAY10: _LO_VOLUME_DAY10.GetText().trim(),
        LO_VOLUME_DAY11: _LO_VOLUME_DAY11.GetText().trim(),
        LO_VOLUME_DAY12: _LO_VOLUME_DAY12.GetText().trim(),
        LO_VOLUME_DAY13: _LO_VOLUME_DAY13.GetText().trim(),
        LO_VOLUME_DAY14: _LO_VOLUME_DAY14.GetText().trim(),
        LO_VOLUME_DAY15: _LO_VOLUME_DAY15.GetText().trim(),
        LO_VOLUME_DAY16: _LO_VOLUME_DAY16.GetText().trim(),
        LO_VOLUME_DAY17: _LO_VOLUME_DAY17.GetText().trim(),
        LO_VOLUME_DAY18: _LO_VOLUME_DAY18.GetText().trim(),
        LO_VOLUME_DAY19: _LO_VOLUME_DAY19.GetText().trim(),
        LO_VOLUME_DAY20: _LO_VOLUME_DAY20.GetText().trim(),
        LO_VOLUME_DAY21: _LO_VOLUME_DAY21.GetText().trim(),
        LO_VOLUME_DAY22: _LO_VOLUME_DAY22.GetText().trim(),
        LO_VOLUME_DAY23: _LO_VOLUME_DAY23.GetText().trim(),
        LO_VOLUME_DAY24: _LO_VOLUME_DAY24.GetText().trim(),
        LO_VOLUME_DAY25: _LO_VOLUME_DAY25.GetText().trim(),
        LO_VOLUME_DAY26: _LO_VOLUME_DAY26.GetText().trim(),
        LO_VOLUME_DAY27: _LO_VOLUME_DAY27.GetText().trim(),
        LO_VOLUME_DAY28: _LO_VOLUME_DAY28.GetText().trim(),
        LO_VOLUME_DAY29: _LO_VOLUME_DAY29.GetText().trim(),
        LO_VOLUME_DAY30: _LO_VOLUME_DAY30.GetText().trim(),
        LO_VOLUME_DAY31: _LO_VOLUME_DAY31.GetText().trim(),
        IS_NQC_REQ_PROCESSED: _IS_NQC_REQ_PROCESSED.GetText().trim(),
        IS_NQC_RES_PROCESSED: _IS_NQC_RES_PROCESSED.GetText().trim(),
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

/*IMPORT*/
function IMPORT_PRODUCTION_PLAN() {
    OnPopPRODUCTION_PLANCloseUp();
    popPRODUCTION_PLAN.Show();

    return false;
}

function OnPopPRODUCTION_PLANCloseUp() {
    ClearFormPRODUCTION_PLAN();
}

function ClearFormPRODUCTION_PLAN() {
    //aspxUClearFileInputClick('_IMPORT_PRODUCTION_PLAN', 0);
}

function PRODUCTION_PLAN_OnFileUploadComplete(s, e) {
    alert(e.callbackData);
    if (e.isValid) {
        popPRODUCTION_PLAN.Hide();
        gvList.PerformCallback();
    }
}

function OnBtnImportCancelClicked() {
    popPRODUCTION_PLAN.Hide()
}

/*Export data*/
function DOWNLOAD_PP_TEMPLATE() {

    $(".loadding").show();
    try {
        $.ajax({
            url: baseUrl + "/TB_R_PRODUCTION_PLAN_M/TB_R_PRODUCTION_PLAN_M_EXPORT",
            type: 'GET',
            data: {
                //part_no: _txtpart_no.GetText().trim(),
                //back_no: _txtback_no.GetText().trim(),
                //part_name: _txtpart_name.GetText().trim()
            },
            success: function (datajson) {

                var str = datajson.split(";");
                if (str[0] == "true") {
                    DownLoadFileExcel(str[1]);
                }
                else { alert(str[1]); }

                $(".loadding").hide();
            },
            error: function () {
                console.log("ERROR Request : " + baseUrl + "/TB_R_PRODUCTION_PLAN_M/TB_R_PRODUCTION_PLAN_M_EXPORT"); //ex.message && ex.name
                $(".loadding").hide();
            }
        });
    } catch (ex) {
        console.log("ERROR (runScreenJson): " + ex.name + " - " + ex.message + " - " + ex.description); //ex.message && ex.name
        $(".loadding").hide();
        alert("Error!");
    }
}

function DownLoadFileExcel(urllink) {
    window.open(urllink);
}