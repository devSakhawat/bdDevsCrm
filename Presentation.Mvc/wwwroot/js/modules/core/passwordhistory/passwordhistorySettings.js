(function () {
    'use strict';

    window.PasswordHistoryModule = window.PasswordHistoryModule || {};

    window.PasswordHistoryModule.config = {
        moduleTitle: 'Password History',
        pluralTitle: 'Password Histories',
        idField: 'historyId',
        displayField: 'userId',
        dom: {
            grid: '#passwordHistoryGrid',
            window: '#passwordHistoryWindow',
            form: '#passwordHistoryForm',
            addButton: '#btnAddPasswordHistory',
            refreshButton: '#btnRefreshPasswordHistory',
            saveButton: '#btnSavePasswordHistory',
            cancelButton: '#btnCancelPasswordHistory'
        },
        apiEndpoints: {
            summary: `${window.AppConfig.apiBaseUrl}/core/systemadmin/password-history-summary`,
            create: `${window.AppConfig.apiBaseUrl}/core/systemadmin/password-history`,
            update: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/password-history/${id}`; },
            delete: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/password-history/${id}`; },
            read: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/password-history/${id}`; }
        },
        gridOptions: {
            pageSize: 20,
            sortable: true,
            filterable: true,
            height: 560,
            pageable: { refresh: true, pageSizes: [10, 20, 50, 100], buttonCount: 5 }
        },
        windowOptions: { width: '820px' },
        grid: { columns: [{field: 'historyId', title: 'ID', width: 90, dataType: 'number', filterable: false}, {field: 'userId', title: 'User Id', width: 120, dataType: 'number'}, {field: 'passwordChangeDate', title: 'Changed On', width: 170}] },
        form: { fields: [{name: 'historyId', label: 'History Id', type: 'hidden'}, {name: 'userId', label: 'User Id', type: 'number', min: 1, placeholder: 'Enter user id'}, {name: 'oldPassword', label: 'Old Password', type: 'textarea', wide: true, maxLength: 500, placeholder: 'Enter old password'}, {name: 'passwordChangeDate', label: 'Password Change Date', type: 'date'}], buildPayload: function (values) {
                function toInteger(value) { const parsed = parseInt(value || 0, 10); return Number.isNaN(parsed) ? 0 : parsed; }
                function toNullableInteger(value) { if (value === null || value === undefined || value === '') { return null; } const parsed = parseInt(value, 10); return Number.isNaN(parsed) ? null : parsed; }
                function normalizeString(value) { return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim(); }
                return { historyId: toInteger(values.historyId), userId: toNullableInteger(values.userId), oldPassword: normalizeString(values.oldPassword), passwordChangeDate: values.passwordChangeDate || null };
            } }
    };

    window.PasswordHistoryModule.config.moduleRef = window.PasswordHistoryModule;
    window.PasswordHistoryModule.config.form.fields.forEach(function (field) { field.moduleRef = window.PasswordHistoryModule; });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () { window.location.href = '/Account/Login'; }, 1500);
            return;
        }

        window.PasswordHistoryModule.Summary?.init?.();
        window.PasswordHistoryModule.Details?.init?.();

        $(window.PasswordHistoryModule.config.dom.addButton).on('click', function () { window.PasswordHistoryModule.Details?.openAddForm?.(); });
        $(window.PasswordHistoryModule.config.dom.refreshButton).on('click', function () { window.PasswordHistoryModule.Summary?.refreshGrid?.(); });
    });
})();
