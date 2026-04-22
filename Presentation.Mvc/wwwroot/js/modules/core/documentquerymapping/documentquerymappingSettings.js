(function () {
    'use strict';

    window.DocumentQueryMappingModule = window.DocumentQueryMappingModule || {};

    window.DocumentQueryMappingModule.config = {
        moduleTitle: 'Document Query Mapping',
        pluralTitle: 'Document Query Mappings',
        idField: 'documentQueryId',
        displayField: 'documentTypeId',
        dom: {
            grid: '#documentQueryMappingGrid',
            window: '#documentQueryMappingWindow',
            form: '#documentQueryMappingForm',
            addButton: '#btnAddDocumentQueryMapping',
            refreshButton: '#btnRefreshDocumentQueryMapping',
            saveButton: '#btnSaveDocumentQueryMapping',
            cancelButton: '#btnCancelDocumentQueryMapping'
        },
        apiEndpoints: {
            summary: `${window.AppConfig.apiBaseUrl}/core/systemadmin/document-query-mapping-summary`,
            create: `${window.AppConfig.apiBaseUrl}/core/systemadmin/document-query-mapping`,
            update: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/document-query-mapping/${id}`; },
            delete: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/document-query-mapping/${id}`; },
            read: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/document-query-mapping/${id}`; },
            documentTypes: `${window.AppConfig.apiBaseUrl}/core/systemadmin/document-types-ddl`
        },
        gridOptions: {
            pageSize: 20,
            sortable: true,
            filterable: true,
            height: 560,
            pageable: { refresh: true, pageSizes: [10, 20, 50, 100], buttonCount: 5 }
        },
        windowOptions: { width: '820px' },
        grid: { columns: [{field: 'documentQueryId', title: 'ID', width: 90, dataType: 'number', filterable: false}, {field: 'reportHeaderId', title: 'Report Header Id', width: 140, dataType: 'number'}, {field: 'documentTypeId', title: 'Document Type', width: 140, dataType: 'number'}, {field: 'parameterDefination', title: 'Parameter Definition', width: 280}] },
        form: { fields: [{name: 'documentQueryId', label: 'Document Query Id', type: 'hidden'}, {name: 'reportHeaderId', label: 'Report Header Id', type: 'number', required: true, min: 1, placeholder: 'Enter report header id'}, {name: 'documentTypeId', label: 'Document Type', type: 'select', required: true, dataSourceEndpoint: function () { return window.DocumentQueryMappingModule.config.apiEndpoints.documentTypes; }, dataTextField: 'documentname', dataValueField: 'documenttypeid', optionLabel: 'Select Document Type...'}, {name: 'parameterDefination', label: 'Parameter Definition', type: 'textarea', wide: true, maxLength: 2000, placeholder: 'Enter parameter definition'}], buildPayload: function (values) {
                function toInteger(value) { const parsed = parseInt(value || 0, 10); return Number.isNaN(parsed) ? 0 : parsed; }
                function normalizeString(value) { return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim(); }
                return { documentQueryId: toInteger(values.documentQueryId), reportHeaderId: toInteger(values.reportHeaderId), documentTypeId: toInteger(values.documentTypeId), parameterDefination: normalizeString(values.parameterDefination) };
            } }
    };

    window.DocumentQueryMappingModule.config.moduleRef = window.DocumentQueryMappingModule;
    window.DocumentQueryMappingModule.config.form.fields.forEach(function (field) { field.moduleRef = window.DocumentQueryMappingModule; });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () { window.location.href = '/Account/Login'; }, 1500);
            return;
        }

        window.DocumentQueryMappingModule.Summary?.init?.();
        window.DocumentQueryMappingModule.Details?.init?.();

        $(window.DocumentQueryMappingModule.config.dom.addButton).on('click', function () { window.DocumentQueryMappingModule.Details?.openAddForm?.(); });
        $(window.DocumentQueryMappingModule.config.dom.refreshButton).on('click', function () { window.DocumentQueryMappingModule.Summary?.refreshGrid?.(); });
    });
})();
