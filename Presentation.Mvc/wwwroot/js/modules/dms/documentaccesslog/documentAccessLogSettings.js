(function () {
    'use strict';

    window.DocumentAccessLogModule = window.DocumentAccessLogModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.DocumentAccessLogModule.config = {
        moduleTitle: 'Document Access Log',
        pluralTitle: 'Document Access Logs',
        idField: 'logId',
        displayField: 'action',
        dom: {
            grid: '#documentAccessLogGrid',
            window: '#documentAccessLogWindow',
            form: '#documentAccessLogForm',
            addButton: '#btnAddDocumentAccessLog',
            refreshButton: '#btnRefreshDocumentAccessLog',
            saveButton: '#btnSaveDocumentAccessLog',
            cancelButton: '#btnCancelDocumentAccessLog'
        },
        apiEndpoints: {
            summary: `${apiRoot}/dms-document-access-log-summary`,
            create: `${apiRoot}/dms-document-access-log`,
            update: function (id) { return `${apiRoot}/dms-document-access-log/${id}`; },
            delete: function (id) { return `${apiRoot}/dms-document-access-log/${id}`; },
            read: function (id) { return `${apiRoot}/dms-document-access-log/${id}`; },
            documents: `${apiRoot}/dms-document-ddl`
        },
        gridOptions: {
            pageSize: 20,
            sortable: true,
            filterable: true,
            height: 560,
            pageable: { refresh: true, pageSizes: [10, 20, 50, 100], buttonCount: 5 }
        },
        windowOptions: { width: '980px' },
        grid: {
            columns: [
                { field: 'logId', title: 'ID', width: 100, dataType: 'number', filterable: false },
                { field: 'documentId', title: 'Document Id', width: 120, dataType: 'number' },
                { field: 'action', title: 'Action', width: 160 },
                { field: 'accessedByUserId', title: 'Accessed By', width: 150 },
                { field: 'accessDateTime', title: 'Access Date Time', width: 180 },
                { field: 'ipAddress', title: 'IP Address', width: 150 },
                { field: 'deviceInfo', title: 'Device Info', width: 220 },
                { field: 'macAddress', title: 'MAC Address', width: 160 }
            ]
        },
        form: {
            fields: [
                { name: 'logId', label: 'Log Id', type: 'hidden' },
                {
                    name: 'documentId',
                    label: 'Document',
                    type: 'select',
                    required: true,
                    dataSourceEndpoint: function () { return window.DocumentAccessLogModule.config.apiEndpoints.documents; },
                    dataTextField: 'title',
                    dataValueField: 'documentId',
                    optionLabel: 'Select Document...'
                },
                { name: 'action', label: 'Action', type: 'text', required: true, maxLength: 250, placeholder: 'Enter action name' },
                { name: 'accessedByUserId', label: 'Accessed By User Id', type: 'text', maxLength: 250, placeholder: 'Enter user id or code' },
                { name: 'accessDateTime', label: 'Access Date Time', type: 'text', placeholder: '2026-04-22T10:30:00' },
                { name: 'ipAddress', label: 'IP Address', type: 'text', maxLength: 50, placeholder: 'Enter IP address' },
                { name: 'macAddress', label: 'MAC Address', type: 'text', maxLength: 50, placeholder: 'Enter MAC address' },
                { name: 'deviceInfo', label: 'Device Info', type: 'textarea', wide: true, maxLength: 250, placeholder: 'Enter device information' },
                { name: 'notes', label: 'Notes', type: 'textarea', wide: true, maxLength: 250, placeholder: 'Enter notes' }
            ],
            buildPayload: function (values) {
                function toInteger(value) {
                    const parsed = parseInt(value || 0, 10);
                    return Number.isNaN(parsed) ? 0 : parsed;
                }

                function normalizeString(value) {
                    return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim();
                }

                return {
                    logId: toInteger(values.logId),
                    documentId: toInteger(values.documentId),
                    accessedByUserId: normalizeString(values.accessedByUserId),
                    ipAddress: normalizeString(values.ipAddress),
                    deviceInfo: normalizeString(values.deviceInfo),
                    macAddress: normalizeString(values.macAddress),
                    notes: normalizeString(values.notes),
                    accessDateTime: normalizeString(values.accessDateTime),
                    action: String(values.action || '').trim()
                };
            }
        }
    };

    window.DocumentAccessLogModule.config.moduleRef = window.DocumentAccessLogModule;
    window.DocumentAccessLogModule.config.form.fields.forEach(function (field) { field.moduleRef = window.DocumentAccessLogModule; });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () { window.location.href = '/Account/Login'; }, 1500);
            return;
        }

        window.DocumentAccessLogModule.Summary?.init?.();
        window.DocumentAccessLogModule.Details?.init?.();

        $(window.DocumentAccessLogModule.config.dom.addButton).on('click', function () { window.DocumentAccessLogModule.Details?.openAddForm?.(); });
        $(window.DocumentAccessLogModule.config.dom.refreshButton).on('click', function () { window.DocumentAccessLogModule.Summary?.refreshGrid?.(); });
    });
})();
