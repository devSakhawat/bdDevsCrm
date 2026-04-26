(function () {
    'use strict';

    window.AppsTokenInfoModule = window.AppsTokenInfoModule || {};

    window.AppsTokenInfoModule.config = {
        moduleTitle: 'Apps Token Info',
        pluralTitle: 'Apps Token Infos',
        idField: 'appsTokenInfoId',
        displayField: 'tokenNumber',
        dom: {
            grid: '#appsTokenInfoGrid',
            window: '#appsTokenInfoWindow',
            form: '#appsTokenInfoForm',
            addButton: '#btnAddAppsTokenInfo',
            refreshButton: '#btnRefreshAppsTokenInfo',
            saveButton: '#btnSaveAppsTokenInfo',
            cancelButton: '#btnCancelAppsTokenInfo'
        },
        apiEndpoints: {
            summary: `${window.AppConfig.apiBaseUrl}/core/systemadmin/apps-token-info-summary`,
            create: `${window.AppConfig.apiBaseUrl}/core/systemadmin/apps-token-info`,
            update: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/apps-token-info/${id}`; },
            delete: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/apps-token-info/${id}`; },
            read: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/apps-token-info/${id}`; }
        },
        gridOptions: {
            pageSize: 20,
            sortable: true,
            filterable: true,
            height: 560,
            pageable: { refresh: true, pageSizes: [10, 20, 50, 100], buttonCount: 5 }
        },
        windowOptions: { width: '820px' },
        grid: { columns: [{field: 'appsTokenInfoId', title: 'ID', width: 90, dataType: 'number', filterable: false}, {field: 'appsUserId', title: 'Apps User Id', width: 120, dataType: 'number'}, {field: 'employeeId', title: 'Employee Id', width: 120, dataType: 'number'}, {field: 'tokenNumber', title: 'Token Number', width: 180}, {field: 'issueDate', title: 'Issue Date', width: 170}, {field: 'expiredDate', title: 'Expired Date', width: 170}] },
        form: { fields: [{name: 'appsTokenInfoId', label: 'Apps Token Info Id', type: 'hidden'}, {name: 'appsUserId', label: 'Apps User Id', type: 'number', min: 0, placeholder: 'Enter apps user id'}, {name: 'employeeId', label: 'Employee Id', type: 'number', min: 0, placeholder: 'Enter employee id'}, {name: 'tokenNumber', label: 'Token Number', type: 'text', required: true, maxLength: 250, placeholder: 'Enter token number'}, {name: 'issueDate', label: 'Issue Date', type: 'date'}, {name: 'expiredDate', label: 'Expired Date', type: 'date'}], buildPayload: function (values) {
                function toInteger(value) { const parsed = parseInt(value || 0, 10); return Number.isNaN(parsed) ? 0 : parsed; }
                function toNullableInteger(value) { if (value === null || value === undefined || value === '') { return null; } const parsed = parseInt(value, 10); return Number.isNaN(parsed) ? null : parsed; }
                function normalizeString(value) { return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim(); }
                return { appsTokenInfoId: toInteger(values.appsTokenInfoId), appsUserId: toNullableInteger(values.appsUserId), employeeId: toNullableInteger(values.employeeId), tokenNumber: normalizeString(values.tokenNumber), issueDate: values.issueDate || null, expiredDate: values.expiredDate || null };
            } }
    };

    window.AppsTokenInfoModule.config.moduleRef = window.AppsTokenInfoModule;
    window.AppsTokenInfoModule.config.form.fields.forEach(function (field) { field.moduleRef = window.AppsTokenInfoModule; });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () { window.location.href = '/Account/Login'; }, 1500);
            return;
        }

        window.AppsTokenInfoModule.Summary?.init?.();
        window.AppsTokenInfoModule.Details?.init?.();

        $(window.AppsTokenInfoModule.config.dom.addButton).on('click', function () { window.AppsTokenInfoModule.Details?.openAddForm?.(); });
        $(window.AppsTokenInfoModule.config.dom.refreshButton).on('click', function () { window.AppsTokenInfoModule.Summary?.refreshGrid?.(); });
    });
})();
