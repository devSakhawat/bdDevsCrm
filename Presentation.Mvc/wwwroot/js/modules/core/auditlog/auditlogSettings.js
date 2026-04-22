(function () {
    'use strict';

    window.AuditLogModule = window.AuditLogModule || {};

    window.AuditLogModule.config = {
        moduleTitle: 'Audit Log',
        pluralTitle: 'Audit Logs',
        idField: 'auditId',
        displayField: 'action',
        dom: {
            grid: '#auditLogGrid',
            window: '#auditLogWindow',
            form: '#auditLogForm',
            addButton: '#btnAddAuditLog',
            refreshButton: '#btnRefreshAuditLog',
            saveButton: '#btnSaveAuditLog',
            cancelButton: '#btnCancelAuditLog'
        },
        apiEndpoints: {
            summary: `${window.AppConfig.apiBaseUrl}/core/systemadmin/audit-log-summary`,
            create: `${window.AppConfig.apiBaseUrl}/core/systemadmin/audit-log`,
            update: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/audit-log/${id}`; },
            delete: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/audit-log/${id}`; },
            read: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/audit-log/${id}`; }
        },
        gridOptions: {
            pageSize: 20,
            sortable: true,
            filterable: true,
            height: 560,
            pageable: { refresh: true, pageSizes: [10, 20, 50, 100], buttonCount: 5 }
        },
        windowOptions: { width: '820px' },
        grid: { columns: [{field: 'auditId', title: 'ID', width: 100, dataType: 'number', filterable: false}, {field: 'username', title: 'Username', width: 160}, {field: 'action', title: 'Action', width: 160}, {field: 'entityType', title: 'Entity Type', width: 160}, {field: 'timestamp', title: 'Timestamp', width: 170}, {field: 'success', title: 'Success', width: 120, kind: 'boolean', dataType: 'boolean', trueText: 'Yes', falseText: 'No'}] },
        form: { fields: [{name: 'auditId', label: 'Audit Id', type: 'hidden'}, {name: 'userId', label: 'User Id', type: 'number', min: 0, placeholder: 'Enter user id'}, {name: 'username', label: 'Username', type: 'text', maxLength: 150, placeholder: 'Enter username'}, {name: 'ipAddress', label: 'IP Address', type: 'text', maxLength: 100, placeholder: 'Enter IP address'}, {name: 'userAgent', label: 'User Agent', type: 'textarea', wide: true, maxLength: 1000, placeholder: 'Enter user agent'}, {name: 'action', label: 'Action', type: 'text', required: true, maxLength: 150, placeholder: 'Enter action'}, {name: 'entityType', label: 'Entity Type', type: 'text', maxLength: 150, placeholder: 'Enter entity type'}, {name: 'entityId', label: 'Entity Id', type: 'text', maxLength: 100, placeholder: 'Enter entity id'}, {name: 'endpoint', label: 'Endpoint', type: 'text', maxLength: 300, wide: true, placeholder: 'Enter endpoint'}, {name: 'module', label: 'Module', type: 'text', maxLength: 150, placeholder: 'Enter module'}, {name: 'oldValue', label: 'Old Value', type: 'textarea', wide: true, maxLength: 4000, placeholder: 'Enter old value'}, {name: 'newValue', label: 'New Value', type: 'textarea', wide: true, maxLength: 4000, placeholder: 'Enter new value'}, {name: 'changes', label: 'Changes', type: 'textarea', wide: true, maxLength: 4000, placeholder: 'Enter changes'}, {name: 'timestamp', label: 'Timestamp', type: 'date'}, {name: 'correlationId', label: 'Correlation Id', type: 'text', maxLength: 200, placeholder: 'Enter correlation id'}, {name: 'sessionId', label: 'Session Id', type: 'text', maxLength: 200, placeholder: 'Enter session id'}, {name: 'requestId', label: 'Request Id', type: 'text', maxLength: 200, placeholder: 'Enter request id'}, {name: 'success', label: 'Success', type: 'checkbox', defaultValue: true}, {name: 'statusCode', label: 'Status Code', type: 'number', min: 0, placeholder: 'Enter status code'}, {name: 'errorMessage', label: 'Error Message', type: 'textarea', wide: true, maxLength: 2000, placeholder: 'Enter error message'}, {name: 'durationMs', label: 'Duration (ms)', type: 'number', min: 0, placeholder: 'Enter duration'}], buildPayload: function (values) {
                function toLong(value) { const parsed = parseInt(value || 0, 10); return Number.isNaN(parsed) ? 0 : parsed; }
                function toNullableInteger(value) { if (value === null || value === undefined || value === '') { return null; } const parsed = parseInt(value, 10); return Number.isNaN(parsed) ? null : parsed; }
                function normalizeString(value) { return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim(); }
                return { auditId: toLong(values.auditId), userId: toNullableInteger(values.userId), username: normalizeString(values.username), ipAddress: normalizeString(values.ipAddress), userAgent: normalizeString(values.userAgent), action: normalizeString(values.action), entityType: normalizeString(values.entityType), entityId: normalizeString(values.entityId), endpoint: normalizeString(values.endpoint), module: normalizeString(values.module), oldValue: normalizeString(values.oldValue), newValue: normalizeString(values.newValue), changes: normalizeString(values.changes), timestamp: values.timestamp || null, correlationId: normalizeString(values.correlationId), sessionId: normalizeString(values.sessionId), requestId: normalizeString(values.requestId), success: values.success !== false, statusCode: toNullableInteger(values.statusCode), errorMessage: normalizeString(values.errorMessage), durationMs: toNullableInteger(values.durationMs) };
            } }
    };

    window.AuditLogModule.config.moduleRef = window.AuditLogModule;
    window.AuditLogModule.config.form.fields.forEach(function (field) { field.moduleRef = window.AuditLogModule; });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () { window.location.href = '/Account/Login'; }, 1500);
            return;
        }

        window.AuditLogModule.Summary?.init?.();
        window.AuditLogModule.Details?.init?.();

        $(window.AuditLogModule.config.dom.addButton).on('click', function () { window.AuditLogModule.Details?.openAddForm?.(); });
        $(window.AuditLogModule.config.dom.refreshButton).on('click', function () { window.AuditLogModule.Summary?.refreshGrid?.(); });
    });
})();
