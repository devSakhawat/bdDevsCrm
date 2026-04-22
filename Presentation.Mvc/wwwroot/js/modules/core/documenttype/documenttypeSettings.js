(function () {
    'use strict';

    window.DocumentTypeModule = window.DocumentTypeModule || {};

    window.DocumentTypeModule.config = {
        moduleTitle: 'Document Type',
        pluralTitle: 'Document Types',
        idField: 'documenttypeid',
        displayField: 'documentname',
        dom: {
            grid: '#documentTypeGrid',
            window: '#documentTypeWindow',
            form: '#documentTypeForm',
            addButton: '#btnAddDocumentType',
            refreshButton: '#btnRefreshDocumentType',
            saveButton: '#btnSaveDocumentType',
            cancelButton: '#btnCancelDocumentType'
        },
        apiEndpoints: {
            summary: `${window.AppConfig.apiBaseUrl}/core/systemadmin/document-type-summary`,
            create: `${window.AppConfig.apiBaseUrl}/core/systemadmin/document-type`,
            update: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/document-type/${id}`; },
            delete: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/document-type/${id}`; },
            read: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/document-type/${id}`; }
        },
        gridOptions: {
            pageSize: 20,
            sortable: true,
            filterable: true,
            height: 560,
            pageable: { refresh: true, pageSizes: [10, 20, 50, 100], buttonCount: 5 }
        },
        windowOptions: { width: '820px' },
        grid: { columns: [{field: 'documenttypeid', title: 'ID', width: 90, dataType: 'number', filterable: false}, {field: 'documentname', title: 'Document Name', width: 220}, {field: 'initiationdate', title: 'Initiation Date', width: 170}, {field: 'useType', title: 'Use Type', width: 120, dataType: 'number'}] },
        form: { fields: [{name: 'documenttypeid', label: 'Document Type Id', type: 'hidden'}, {name: 'documentname', label: 'Document Name', type: 'text', required: true, maxLength: 200, placeholder: 'Enter document name'}, {name: 'initiationdate', label: 'Initiation Date', type: 'date'}, {name: 'useType', label: 'Use Type', type: 'number', min: 0, placeholder: 'Enter use type'}, {name: 'description', label: 'Description', type: 'textarea', wide: true, maxLength: 1000, placeholder: 'Enter description'}], buildPayload: function (values) {
                function normalizeString(value) { return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim(); }
                function toInteger(value) { const parsed = parseInt(value || 0, 10); return Number.isNaN(parsed) ? 0 : parsed; }
                function toNullableInteger(value) { if (value === null || value === undefined || value === '') { return null; } const parsed = parseInt(value, 10); return Number.isNaN(parsed) ? null : parsed; }
                return { documenttypeid: toInteger(values.documenttypeid), documentname: normalizeString(values.documentname), initiationdate: values.initiationdate || null, description: normalizeString(values.description), useType: toNullableInteger(values.useType) };
            } }
    };

    window.DocumentTypeModule.config.moduleRef = window.DocumentTypeModule;
    window.DocumentTypeModule.config.form.fields.forEach(function (field) { field.moduleRef = window.DocumentTypeModule; });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () { window.location.href = '/Account/Login'; }, 1500);
            return;
        }

        window.DocumentTypeModule.Summary?.init?.();
        window.DocumentTypeModule.Details?.init?.();

        $(window.DocumentTypeModule.config.dom.addButton).on('click', function () { window.DocumentTypeModule.Details?.openAddForm?.(); });
        $(window.DocumentTypeModule.config.dom.refreshButton).on('click', function () { window.DocumentTypeModule.Summary?.refreshGrid?.(); });
    });
})();
