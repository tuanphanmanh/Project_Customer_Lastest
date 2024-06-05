var abp = abp || {};
(function () {
    var showMessage = function (type, message, title, options) {

        if (!title) {
            title = message;
            message = undefined;
        }

        options = options || {};
        options.title = title;
        options.icon = type;
        options.confirmButtonText = options.confirmButtonText || abp.localization.localize('Ok', 'esign');

        if (options.isHtml) {
            options.html = message;
        } else {
            options.text = message;
        }


        const { isHtml, ...optionsSafe } = options;
        let modal = document.getElementById('messageCustom');
        if(modal) {
            if(modal.querySelector('#messageImg'))
            switch (type) {
                case 'info':
                    modal.querySelector('#messageImg').src = '/assets/common/images/Icon/info.svg';
                    break;
                case 'success':
                    modal.querySelector('#messageImg').src = '/assets/common/images/Icon/success.svg';
                    break;
                case 'warning':
                    modal.querySelector('#messageImg').src = '/assets/common/images/Icon/warning.svg';
                    break;
                case 'error':
                    modal.querySelector('#messageImg').src = '/assets/common/images/Icon/error.svg';
                    break;
                default:
                    break;
            }
            modal.querySelector('.message-title').innerText = abp.localization.localize('Message', 'esign');
            modal.querySelector('.message-content').innerText = abp.localization.localize(options.title, 'esign');
            modal.style.display = 'block';
            return;
        }
        return new bootstrap.Modal(document.getElementById('messageCustom')).show();
    };

    abp.message.info = function (message, title, options) {
        return showMessage('info', message, title, options);
    };

    abp.message.success = function (message, title, options) {
        return showMessage('success', message, title, options);
    };

    abp.message.warn = function (message, title, options) {
        return showMessage('warning', message, title, options);
    };

    abp.message.error = function (message, title, options) {
        return showMessage('error', message, title, options);
    };

    abp.message.confirm = function (message, title, callback, options) {
        options = options || {};
        options.title = title ? title : abp.localization.localize('AreYouSure', 'esign');
        options.icon = 'question';

        options.confirmButtonText = options.confirmButtonText || abp.localization.localize('Yes', 'esign');
        options.cancelButtonText = options.cancelButtonText || abp.localization.localize('Cancel', 'esign');
        options.showCancelButton = true;

        if (options.isHtml) {
            options.html = message;
        } else {
            options.text = message;
        }
        const { isHtml, ...optionsSafe } = options;
        return Swal.fire(optionsSafe).then(function(result) {
            callback && callback(result.value, result);
        });
    };
})();
