
//2018-08-24: Don't use bootstrap, use jquery confirm


function msgOk(s) {
    $.alert({
        title: 'Information!',
        icon: 'fa fa-info-circle blue',
        content: s
    });
}

function msgError(s) {
    $.alert({
        title: 'Error!',
        icon: 'fa fa-warning red',
        content: s
    });
}

function msgInfo(s) {
    msgOk(s);
}

function msgWarning(s) {
    $.alert({
        title: 'Warning!',
        icon: 'fa fa-warning red',
        content: s
    });
}

/*function msgOk(s) {
    $.bootstrapGrowl(s, {
        type: 'success'
     , allowdismiss: 'true'
     , width: 500
     , align: 'center'
    });
}

function msgError(s) {
    $.bootstrapGrowl(s, {
        type: 'danger'
    , allowdismiss: 'true'
    , delay: 3000
    , width: 500 
    , align: 'center'
    });
}

function msgInfo(s) {
    $.bootstrapGrowl(s, {
        type: 'info'
    , allowdismiss: 'true'
    , width: 500
    , align: 'center'
    });
}

function msgWarning(s) {
    $.bootstrapGrowl(s, {
        type: 'warning'
    , allowdismiss: 'true'
    , width: 500
    , align: 'center'
    });
}*/