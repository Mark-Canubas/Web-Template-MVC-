(function (window, $) {
    'use strict';

    function escapeHtml(s) {
        return String(s == null ? '' : s)
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#39;');
    }

    function getSwal() {
        var swal = window.Swal || window.sweetAlert;
        return (swal && typeof swal.fire === 'function') ? swal : null;
    }

    function fireSwal(options) {
        var swal = getSwal();
        if (!swal) {
            if (options && (options.text || options.title)) {
                alert([options.title, options.text].filter(Boolean).join('\n'));
            }
            return;
        }

        options = options || {};
        if (options.showConfirmButton !== false) {
            options.confirmButtonColor = options.confirmButtonColor || '#000000';
            options.customClass = options.customClass || {};
            options.customClass.confirmButton = options.customClass.confirmButton || 'swal2-confirm btn btn-dark';
            options.buttonsStyling = false;
        }

        swal.fire(options);
    }

    if ($ && $.validator) {
        $.validator.setDefaults({
            highlight: function (element) {
                $(element).addClass('is-invalid');
            },
            unhighlight: function (element) {
                $(element).removeClass('is-invalid');
            },
            errorElement: 'div',
            errorClass: 'invalid-feedback',
            errorPlacement: function (error, element) {
                error.insertAfter(element);
            }
        });
    }

    window.getMvcValidationSummaryErrors = function (root) {
        var container = root || document;
        var summary = container.querySelector('.validation-summary-errors');
        if (!summary) return [];

        var items = summary.querySelectorAll('li');
        var errors = [];
        for (var i = 0; i < items.length; i++) {
            var txt = (items[i].textContent || '').trim();
            if (txt) errors.push(txt);
        }
        return errors;
    };

    window.loginSwalSuccess = function (title, text) {
        fireSwal({
            title: title || 'Good job!',
            text: text || 'You clicked the button!',
            icon: 'success'
        });
    };

    window.loginSwalError = function (title, text) {
        fireSwal({
            title: title || 'Login failed',
            text: text || '',
            icon: 'error'
        });
    };

    window.showLoginErrors = function (errors) {
        if (!errors || !errors.length) return;

        fireSwal({
            icon: 'error',
            title: 'Login failed',
            html: errors.map(function (e) { return '<div>' + escapeHtml(e) + '</div>'; }).join(''),
            confirmButtonText: 'OK'
        });
    };
})(window, window.jQuery);