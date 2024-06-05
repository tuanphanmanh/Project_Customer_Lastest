var _id = -1;
var _searchTimeout;
var _searchXhr;

function Add() {
    OnPopTB_R_NQC_RESULT_MCloseUp();
    popTB_R_NQC_RESULT_M.Show();
    return false;
}

function Edit(t) {
    var $btn = $(t)
    if ($btn.parent().hasClass("processing")) {
        return false;
    }
    _id = $btn.attr("data-id");
    $.ajax({
        url: baseUrl + "/TB_R_NQC_RESULT_M/TB_R_NQC_RESULT_M_Get",
        method: "post",
        dataType: "json",
        data: {
            sid: _id
        }
    }).done(function (response) {
        $("span[id^='popTB_R_NQC_RESULT_M']").html("Edit TB_R_NQC_RESULT_M");

        _CFC.SetText(response.CFC);
        _PART_NO.SetText(response.PART_NO);
        _PROD_SFX.SetText(response.PROD_SFX);
        _PRODUCTION_MONTH.SetText(response.PRODUCTION_MONTH_Str_DDMMYYYY);
        _PARTS_MATCHING_KEY.SetText(response.PARTS_MATCHING_KEY);
        _DAILY_QTY01.SetText(response.DAILY_QTY01);
        _DAILY_QTY02.SetText(response.DAILY_QTY02);
        _DAILY_QTY03.SetText(response.DAILY_QTY03);
        _DAILY_QTY04.SetText(response.DAILY_QTY04);
        _DAILY_QTY05.SetText(response.DAILY_QTY05);
        _DAILY_QTY06.SetText(response.DAILY_QTY06);
        _DAILY_QTY07.SetText(response.DAILY_QTY07);
        _DAILY_QTY08.SetText(response.DAILY_QTY08);
        _DAILY_QTY09.SetText(response.DAILY_QTY09);
        _DAILY_QTY10.SetText(response.DAILY_QTY10);
        _DAILY_QTY11.SetText(response.DAILY_QTY11);
        _DAILY_QTY12.SetText(response.DAILY_QTY12);
        _DAILY_QTY13.SetText(response.DAILY_QTY13);
        _DAILY_QTY14.SetText(response.DAILY_QTY14);
        _DAILY_QTY15.SetText(response.DAILY_QTY15);
        _DAILY_QTY16.SetText(response.DAILY_QTY16);
        _DAILY_QTY17.SetText(response.DAILY_QTY17);
        _DAILY_QTY18.SetText(response.DAILY_QTY18);
        _DAILY_QTY19.SetText(response.DAILY_QTY19);
        _DAILY_QTY20.SetText(response.DAILY_QTY20);
        _DAILY_QTY21.SetText(response.DAILY_QTY21);
        _DAILY_QTY22.SetText(response.DAILY_QTY22);
        _DAILY_QTY23.SetText(response.DAILY_QTY23);
        _DAILY_QTY24.SetText(response.DAILY_QTY24);
        _DAILY_QTY25.SetText(response.DAILY_QTY25);
        _DAILY_QTY26.SetText(response.DAILY_QTY26);
        _DAILY_QTY27.SetText(response.DAILY_QTY27);
        _DAILY_QTY28.SetText(response.DAILY_QTY28);
        _DAILY_QTY29.SetText(response.DAILY_QTY29);
        _DAILY_QTY30.SetText(response.DAILY_QTY30);
        _DAILY_QTY31.SetText(response.DAILY_QTY31);
        _TOTAL_QTY.SetText(response.TOTAL_QTY);

    }).fail(function () {
        OnPopTB_R_NQC_RESULT_MCloseUp();
    })
    popTB_R_NQC_RESULT_M.Show();
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
        url: baseUrl + "/TB_R_NQC_RESULT_M/SaveData",
        method: "post",
        data: GetObject(),
        dataType: "json"
    }).done(function (response) {
        ActiveForm(true);
        if (response.success) {
            popTB_R_NQC_RESULT_M.Hide();
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
    popTB_R_NQC_RESULT_M.Hide()
}

function OnPopTB_R_NQC_RESULT_MCloseUp() {
    _id = -1
    $("span[id^='popTB_R_NQC_RESULT_M']").html("Add New TB_R_NQC_RESULT_M")
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
        url: baseUrl + "/TB_R_NQC_RESULT_M/Delete",
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
        url: baseUrl + "/TB_R_NQC_RESULT_M/SetObjectInfo",
        method: "post",
        data: {
            CFC: _txtCFC.GetText(),
            PROD_SFX: _txtPROD_SFX.GetText(),
            PRODUCTION_MONTH: _txtPRODUCTION_MONTH.GetText()
        }
    }).done(function (response) {
        gvList.PerformCallback();
    });
}

function ClearForm() {
    _CFC.SetText("");
    _PART_NO.SetText("");
    _PROD_SFX.SetText("");
    _PRODUCTION_MONTH.SetText("");
    _PARTS_MATCHING_KEY.SetText("");
    _DAILY_QTY01.SetText("");
    _DAILY_QTY02.SetText("");
    _DAILY_QTY03.SetText("");
    _DAILY_QTY04.SetText("");
    _DAILY_QTY05.SetText("");
    _DAILY_QTY06.SetText("");
    _DAILY_QTY07.SetText("");
    _DAILY_QTY08.SetText("");
    _DAILY_QTY09.SetText("");
    _DAILY_QTY10.SetText("");
    _DAILY_QTY11.SetText("");
    _DAILY_QTY12.SetText("");
    _DAILY_QTY13.SetText("");
    _DAILY_QTY14.SetText("");
    _DAILY_QTY15.SetText("");
    _DAILY_QTY16.SetText("");
    _DAILY_QTY17.SetText("");
    _DAILY_QTY18.SetText("");
    _DAILY_QTY19.SetText("");
    _DAILY_QTY20.SetText("");
    _DAILY_QTY21.SetText("");
    _DAILY_QTY22.SetText("");
    _DAILY_QTY23.SetText("");
    _DAILY_QTY24.SetText("");
    _DAILY_QTY25.SetText("");
    _DAILY_QTY26.SetText("");
    _DAILY_QTY27.SetText("");
    _DAILY_QTY28.SetText("");
    _DAILY_QTY29.SetText("");
    _DAILY_QTY30.SetText("");
    _DAILY_QTY31.SetText("");
    _TOTAL_QTY.SetText("");
    validatereset();
}

function ClearSearch() {
    DoSearch();
}

function ActiveForm(active) {

    _CFC.SetEnabled(active);
    _PART_NO.SetEnabled(active);
    _PROD_SFX.SetEnabled(active);
    _PRODUCTION_MONTH.SetEnabled(active);
    _PARTS_MATCHING_KEY.SetEnabled(active);
    _DAILY_QTY01.SetEnabled(active);
    _DAILY_QTY02.SetEnabled(active);
    _DAILY_QTY03.SetEnabled(active);
    _DAILY_QTY04.SetEnabled(active);
    _DAILY_QTY05.SetEnabled(active);
    _DAILY_QTY06.SetEnabled(active);
    _DAILY_QTY07.SetEnabled(active);
    _DAILY_QTY08.SetEnabled(active);
    _DAILY_QTY09.SetEnabled(active);
    _DAILY_QTY10.SetEnabled(active);
    _DAILY_QTY11.SetEnabled(active);
    _DAILY_QTY12.SetEnabled(active);
    _DAILY_QTY13.SetEnabled(active);
    _DAILY_QTY14.SetEnabled(active);
    _DAILY_QTY15.SetEnabled(active);
    _DAILY_QTY16.SetEnabled(active);
    _DAILY_QTY17.SetEnabled(active);
    _DAILY_QTY18.SetEnabled(active);
    _DAILY_QTY19.SetEnabled(active);
    _DAILY_QTY20.SetEnabled(active);
    _DAILY_QTY21.SetEnabled(active);
    _DAILY_QTY22.SetEnabled(active);
    _DAILY_QTY23.SetEnabled(active);
    _DAILY_QTY24.SetEnabled(active);
    _DAILY_QTY25.SetEnabled(active);
    _DAILY_QTY26.SetEnabled(active);
    _DAILY_QTY27.SetEnabled(active);
    _DAILY_QTY28.SetEnabled(active);
    _DAILY_QTY29.SetEnabled(active);
    _DAILY_QTY30.SetEnabled(active);
    _DAILY_QTY31.SetEnabled(active);
    _TOTAL_QTY.SetEnabled(active);
}

function GetObject() {
    return {
        id: _id,
        CFC: _CFC.GetText().trim(),
        PART_NO: _PART_NO.GetText().trim(),
        PROD_SFX: _PROD_SFX.GetText().trim(),
        PRODUCTION_MONTH: _PRODUCTION_MONTH.GetText().trim(),
        PARTS_MATCHING_KEY: _PARTS_MATCHING_KEY.GetText().trim(),
        DAILY_QTY01: _DAILY_QTY01.GetText().trim(),
        DAILY_QTY02: _DAILY_QTY02.GetText().trim(),
        DAILY_QTY03: _DAILY_QTY03.GetText().trim(),
        DAILY_QTY04: _DAILY_QTY04.GetText().trim(),
        DAILY_QTY05: _DAILY_QTY05.GetText().trim(),
        DAILY_QTY06: _DAILY_QTY06.GetText().trim(),
        DAILY_QTY07: _DAILY_QTY07.GetText().trim(),
        DAILY_QTY08: _DAILY_QTY08.GetText().trim(),
        DAILY_QTY09: _DAILY_QTY09.GetText().trim(),
        DAILY_QTY10: _DAILY_QTY10.GetText().trim(),
        DAILY_QTY11: _DAILY_QTY11.GetText().trim(),
        DAILY_QTY12: _DAILY_QTY12.GetText().trim(),
        DAILY_QTY13: _DAILY_QTY13.GetText().trim(),
        DAILY_QTY14: _DAILY_QTY14.GetText().trim(),
        DAILY_QTY15: _DAILY_QTY15.GetText().trim(),
        DAILY_QTY16: _DAILY_QTY16.GetText().trim(),
        DAILY_QTY17: _DAILY_QTY17.GetText().trim(),
        DAILY_QTY18: _DAILY_QTY18.GetText().trim(),
        DAILY_QTY19: _DAILY_QTY19.GetText().trim(),
        DAILY_QTY20: _DAILY_QTY20.GetText().trim(),
        DAILY_QTY21: _DAILY_QTY21.GetText().trim(),
        DAILY_QTY22: _DAILY_QTY22.GetText().trim(),
        DAILY_QTY23: _DAILY_QTY23.GetText().trim(),
        DAILY_QTY24: _DAILY_QTY24.GetText().trim(),
        DAILY_QTY25: _DAILY_QTY25.GetText().trim(),
        DAILY_QTY26: _DAILY_QTY26.GetText().trim(),
        DAILY_QTY27: _DAILY_QTY27.GetText().trim(),
        DAILY_QTY28: _DAILY_QTY28.GetText().trim(),
        DAILY_QTY29: _DAILY_QTY29.GetText().trim(),
        DAILY_QTY30: _DAILY_QTY30.GetText().trim(),
        DAILY_QTY31: _DAILY_QTY31.GetText().trim(),
        TOTAL_QTY: _TOTAL_QTY.GetText().trim(),
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
function IMPORT_NQC_RESULT() {
    OnPopNQC_RESULTCloseUp();
    popNQC_RESULT.Show();

    return false;
}

function OnPopNQC_RESULTCloseUp() {
    ClearFormNQC_RESULT();
}

function ClearFormNQC_RESULT() {
    //aspxUClearFileInputClick('_IMPORT_NQC_RESULT', 0);
}

function NQC_RESULT_OnFileUploadComplete(s, e) {
    alert(e.callbackData);
    if (e.isValid) {
        popNQC_RESULT.Hide();
        gvList.PerformCallback();
    }
}

function OnBtnImportCancelClicked() {
    popNQC_RESULT.Hide()
}

/*Export data*/
function DOWNLOAD_NQC_RESULT_TEMPLATE() {

    $(".loadding").show();
    try {
        $.ajax({
            url: baseUrl + "/TB_R_NQC_RESULT_M/TB_R_NQC_RESULT_M_EXPORT",
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
                console.log("ERROR Request : " + baseUrl + "/TB_R_NQC_RESULT_M/TB_R_NQC_RESULT_M_EXPORT"); //ex.message && ex.name
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