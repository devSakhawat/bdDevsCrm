(function () {
    'use strict';

    window.AuditTrailModule = window.AuditTrailModule || {};

    window.AuditTrailModule.config = {
        moduleTitle: 'Audit Trail',
        pluralTitle: 'Audit Trails',
        idField: 'auditId',
        displayField: 'shortdescription',
        dom: {
            grid: '#auditTrailGrid',
            window: '#auditTrailWindow',
            form: '#auditTrailForm',
            addButton: '#btnAddAuditTrail',
            refreshButton: '#btnRefreshAuditTrail',
            saveButton: '#btnSaveAuditTrail',
            cancelButton: '#btnCancelAuditTrail'
        },
        apiEndpoints: {
            summary: `${window.AppConfig.apiBaseUrl}/core/systemadmin/audit-trail-summary`,
            create: `${window.AppConfig.apiBaseUrl}/core/systemadmin/audit-trail`,
            update: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/audit-trail/${id}`; },
            delete: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/audit-trail/${id}`; },
            read: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/audit-trail/${id}`; }
        },
        gridOptions: {
            pageSize: 20,
            sortable: true,
            filterable: true,
            height: 560,
            pageable: { refresh: true, pageSizes: [10, 20, 50, 100], buttonCount: 5 }
        },
        windowOptions: { width: '820px' },
        grid: { columns: [{field: 'auditId', title: 'ID', width: 90, dataType: 'number', filterable: false}, {field: 'clientUser', title: 'Client User', width: 180}, {field: 'shortdescription', title: 'Short Description', width: 240}, {field: 'auditType', title: 'Audit Type', width: 140}, {field: 'actionDate', title: 'Action Date', width: 170}, {field: 'auditStatus', title: 'Status', width: 120}] },
        form: { fields: [{name: 'auditId', label: 'Audit Id', type: 'hidden'}, {name: 'userId', label: 'User Id', type: 'number', min: 0, placeholder: 'Enter user id'}, {name: 'clientUser', label: 'Client User', type: 'text', maxLength: 150, placeholder: 'Enter client user'}, {name: 'clientIp', label: 'Client IP', type: 'text', maxLength: 100, placeholder: 'Enter client IP'}, {name: 'shortdescription', label: 'Short Description', type: 'text', required: true, maxLength: 250, placeholder: 'Enter short description'}, {name: 'auditType', label: 'Audit Type', type: 'text', maxLength: 100, placeholder: 'Enter audit type'}, {name: 'auditDescription', label: 'Audit Description', type: 'textarea', wide: true, maxLength: 3000, placeholder: 'Enter audit description'}, {name: 'actionDate', label: 'Action Date', type: 'date'}, {name: 'requestedUrl', label: 'Requested Url', type: 'text', maxLength: 500, wide: true, placeholder: 'Enter requested url'}, {name: 'auditStatus', label: 'Audit Status', type: 'text', maxLength: 100, placeholder: 'Enter audit status'}], buildPayload: function (values) {
                function toInteger(value) { const parsed = parseInt(value || 0, 10); return Number.isNaN(parsed) ? 0 : parsed; }
                function toNullableInteger(value) { if (value === null || value === undefined || value === '') { return null; } const parsed = parseInt(value, 10); return Number.isNaN(parsed) ? null : parsed; }
                function normalizeString(value) { return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim(); }
                return { auditId: toInteger(values.auditId), userId: toNullableInteger(values.userId), clientUser: normalizeString(values.clientUser), clientIp: normalizeString(values.clientIp), shortdescription: normalizeString(values.shortdescription), auditType: normalizeString(values.auditType), auditDescription: normalizeString(values.auditDescription), actionDate: values.actionDate || null, requestedUrl: normalizeString(values.requestedUrl), auditStatus: normalizeString(values.auditStatus) };
            } }
    };

    window.AuditTrailModule.config.moduleRef = window.AuditTrailModule;
    window.AuditTrailModule.config.form.fields.forEach(function (field) { field.moduleRef = window.AuditTrailModule; });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () { window.location.href = '/Account/Login'; }, 1500);
            return;
        }

        window.AuditTrailModule.Summary?.init?.();
        window.AuditTrailModule.Details?.init?.();

        $(window.AuditTrailModule.config.dom.addButton).on('click', function () { window.AuditTrailModule.Details?.openAddForm?.(); });
        $(window.AuditTrailModule.config.dom.refreshButton).on('click', function () { window.AuditTrailModule.Summary?.refreshGrid?.(); });
    });
})();
