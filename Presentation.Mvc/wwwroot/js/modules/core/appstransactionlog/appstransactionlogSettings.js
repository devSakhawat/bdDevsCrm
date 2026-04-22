(function () {
    'use strict';

    window.AppsTransactionLogModule = window.AppsTransactionLogModule || {};

    window.AppsTransactionLogModule.config = {
        moduleTitle: 'Apps Transaction Log',
        pluralTitle: 'Apps Transaction Logs',
        idField: 'transactionLogId',
        displayField: 'transactionType',
        dom: {
            grid: '#appsTransactionLogGrid',
            window: '#appsTransactionLogWindow',
            form: '#appsTransactionLogForm',
            addButton: '#btnAddAppsTransactionLog',
            refreshButton: '#btnRefreshAppsTransactionLog',
            saveButton: '#btnSaveAppsTransactionLog',
            cancelButton: '#btnCancelAppsTransactionLog'
        },
        apiEndpoints: {
            summary: `${window.AppConfig.apiBaseUrl}/core/systemadmin/apps-transaction-log-summary`,
            create: `${window.AppConfig.apiBaseUrl}/core/systemadmin/apps-transaction-log`,
            update: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/apps-transaction-log/${id}`; },
            delete: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/apps-transaction-log/${id}`; },
            read: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/apps-transaction-log/${id}`; }
        },
        gridOptions: {
            pageSize: 20,
            sortable: true,
            filterable: true,
            height: 560,
            pageable: { refresh: true, pageSizes: [10, 20, 50, 100], buttonCount: 5 }
        },
        windowOptions: { width: '820px' },
        grid: { columns: [{field: 'transactionLogId', title: 'ID', width: 90, dataType: 'number', filterable: false}, {field: 'transactionDate', title: 'Transaction Date', width: 170}, {field: 'transactionType', title: 'Transaction Type', width: 180}, {field: 'responseCode', title: 'Response Code', width: 130}, {field: 'appsUserId', title: 'Apps User Id', width: 120, dataType: 'number'}, {field: 'employeeId', title: 'Employee Id', width: 120, dataType: 'number'}] },
        form: { fields: [{name: 'transactionLogId', label: 'Transaction Log Id', type: 'hidden'}, {name: 'transactionDate', label: 'Transaction Date', type: 'date'}, {name: 'transactionType', label: 'Transaction Type', type: 'text', required: true, maxLength: 150, placeholder: 'Enter transaction type'}, {name: 'responseCode', label: 'Response Code', type: 'text', maxLength: 50, placeholder: 'Enter response code'}, {name: 'request', label: 'Request', type: 'textarea', wide: true, maxLength: 4000, placeholder: 'Enter request payload'}, {name: 'response', label: 'Response', type: 'textarea', wide: true, maxLength: 4000, placeholder: 'Enter response payload'}, {name: 'remarks', label: 'Remarks', type: 'textarea', wide: true, maxLength: 2000, placeholder: 'Enter remarks'}, {name: 'appsUserId', label: 'Apps User Id', type: 'number', min: 0, placeholder: 'Enter apps user id'}, {name: 'employeeId', label: 'Employee Id', type: 'number', min: 0, placeholder: 'Enter employee id'}], buildPayload: function (values) {
                function toInteger(value) { const parsed = parseInt(value || 0, 10); return Number.isNaN(parsed) ? 0 : parsed; }
                function toNullableInteger(value) { if (value === null || value === undefined || value === '') { return null; } const parsed = parseInt(value, 10); return Number.isNaN(parsed) ? null : parsed; }
                function normalizeString(value) { return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim(); }
                return { transactionLogId: toInteger(values.transactionLogId), transactionDate: values.transactionDate || null, transactionType: normalizeString(values.transactionType), responseCode: normalizeString(values.responseCode), request: normalizeString(values.request), response: normalizeString(values.response), remarks: normalizeString(values.remarks), appsUserId: toNullableInteger(values.appsUserId), employeeId: toNullableInteger(values.employeeId) };
            } }
    };

    window.AppsTransactionLogModule.config.moduleRef = window.AppsTransactionLogModule;
    window.AppsTransactionLogModule.config.form.fields.forEach(function (field) { field.moduleRef = window.AppsTransactionLogModule; });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () { window.location.href = '/Account/Login'; }, 1500);
            return;
        }

        window.AppsTransactionLogModule.Summary?.init?.();
        window.AppsTransactionLogModule.Details?.init?.();

        $(window.AppsTransactionLogModule.config.dom.addButton).on('click', function () { window.AppsTransactionLogModule.Details?.openAddForm?.(); });
        $(window.AppsTransactionLogModule.config.dom.refreshButton).on('click', function () { window.AppsTransactionLogModule.Summary?.refreshGrid?.(); });
    });
})();
