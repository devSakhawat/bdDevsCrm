/* ================================================================
   bdDevCRM — app.toast.js
   Toast notification module: success · error · warning · info
================================================================ */

'use strict';

const app = window.app || {};

app.toast = (function () {

    const ICONS = {
        success: 'fa-circle-check',
        error:   'fa-circle-xmark',
        warning: 'fa-triangle-exclamation',
        info:    'fa-circle-info'
    };

    const DURATIONS = {
        success: 3000,
        error:   0,      // manual close
        warning: 4000,
        info:    3000
    };

    function _show(type, message, title) {
        const container = document.getElementById('toastContainer');
        if (!container) return;

        const icon     = ICONS[type]    || 'fa-info-circle';
        const duration = DURATIONS[type] ?? 3000;

        const defaultTitles = { success: 'Success', error: 'Error', warning: 'Warning', info: 'Info' };
        const toastTitle    = title || defaultTitles[type] || '';

        const toast = document.createElement('div');
        toast.className = `toast toast--${type}`;
        toast.innerHTML = `
            <i class="fa ${icon} toast__icon"></i>
            <div class="toast__body">
                ${toastTitle ? `<div class="toast__title">${toastTitle}</div>` : ''}
                <div class="toast__msg">${message}</div>
            </div>
            <button class="toast__close" title="Dismiss"><i class="fa fa-xmark"></i></button>
        `;

        toast.querySelector('.toast__close').addEventListener('click', () => _hide(toast));
        container.appendChild(toast);

        if (duration > 0) {
            setTimeout(() => _hide(toast), duration);
        }

        return toast;
    }

    function _hide(toast) {
        if (!toast || toast.classList.contains('is-hiding')) return;
        toast.classList.add('is-hiding');
        setTimeout(() => toast.remove(), 220);
    }

    return {
        success: (msg, title) => _show('success', msg, title),
        error:   (msg, title) => _show('error',   msg, title),
        warning: (msg, title) => _show('warning', msg, title),
        info:    (msg, title) => _show('info',    msg, title)
    };

})();

window.app = app;
