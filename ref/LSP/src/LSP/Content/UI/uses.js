function pesanOk(s) {
    $.bootstrapGrowl(s, { type: 'success'
     , allowdismiss: 'true'
     , align: 'center'
    });
}

function pesanErr(s) {
    $.bootstrapGrowl(s, { type: 'danger'
    , allowdismiss: 'true'
    , delay: 10000
    , align: 'center'
    });
}

function pesan(s) {
    $.bootstrapGrowl(s, { type: 'info'
    , allowdismiss: 'true'
    , align: 'center'
    });
}

function parseDate(str) {
    if (str)
        str = str.trim();

    var m = str.match(/^(\d\d?)\-(\d\d?)\-(\d\d\d\d)$/);
    var d = null;
    if (m && m.length > 3)
        d = Date.parse(m[3] + '-' + m[2] + '-' + m[1]);
    else
        d = null;
    if (isNaN(d))
        d = null;
    return d;  
}

function AddValidators() {
    $.validator.addMethod("dmy",
        function(value, element) {
            return parseDate(value) !== null
        },
        "format tanggal yang benar dd-mm-yyyy."
    );

        $.validator.addMethod("Isi", function (value, element) {
            return value != null && value.trim() != '';
        }, "Harus diisi");



}

function blockNonNumbers(obj, e, allowDecimal, allowNegative) {
    var key;
    var isCtrl = false;
    var keychar;
    var reg;

    if (window.event) {
        key = e.keyCode;
        isCtrl = window.event.ctrlKey
    }
    else if (e.which) {
        key = e.which;
        isCtrl = e.ctrlKey;
    }

    if (isNaN(key)) return true;

    keychar = String.fromCharCode(key);

    // check for backspace or delete, or if Ctrl was pressed
    if (key == 8 || isCtrl) {
        return true;
    }
    
    reg = /\d/;
    var isFirstN = allowNegative ? keychar == '-' && obj.value.indexOf('-') == -1 : false;
    var isFirstD = allowDecimal ? keychar == '.' && obj.value.indexOf('.') == -1 : false;

    return isFirstN || isFirstD || reg.test(keychar);
}

function NumbersAss() {
    $('.numbers').keypress(function(e) { return blockNonNumbers($(this), e, false, false) });
}
function validatum(l) {
    var r = [];
    $.each(l, function (i, a) {
        var o = $('#' + a.name);
        // console.log(a.name);
        var msg;
        msg = '';
        if (o) {
            var t = o.val();
            if (t) {
                t = t.trim()
                if (t.length < 1) {
                    msg = a.desc + " harus diisi";
                }
                else if (!parseDate(t)) {
                    msg = a.desc + ' tidak baku';
                }
            } else {
                console.log(a.name + ' empty');
            }

        } else {
            console.log(a.name, ' not found');
        }
        if (msg && msg.length > 0)
            r.push(msg);
    });
    return r;
}

function DataTableAjaxErrHandler(xhr, status, thrown) {
    if (status == 'parsererror') {
        pesanErr("Timed Out,<br/> please <a href=\"/Login\">login</a> again");
    } else if (status == 'error' || thrown == null) {
        pesanErr("Disconnected,<br/>probably <br/>a data service failure");
    } else {
        pesanErr("Failed getting data cause: <br/>" + status + " " + thrown);
    }
}
