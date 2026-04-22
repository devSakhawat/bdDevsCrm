(function () {
    'use strict';

    window.DocumentParameterMappingModule = window.DocumentParameterMappingModule || {};

    window.DocumentParameterMappingModule.config = {
        moduleTitle: 'Document Parameter Mapping',
        pluralTitle: 'Document Parameter Mappings',
        idField: 'mappingId',
        displayField: 'documentTypeId',
        dom: {
            grid: '#documentParameterMappingGrid',
            window: '#documentParameterMappingWindow',
            form: '#documentParameterMappingForm',
            addButton: '#btnAddDocumentParameterMapping',
            refreshButton: '#btnRefreshDocumentParameterMapping',
            saveButton: '#btnSaveDocumentParameterMapping',
            cancelButton: '#btnCancelDocumentParameterMapping'
        },
        apiEndpoints: {
            summary: `${window.AppConfig.apiBaseUrl}/core/systemadmin/document-parameter-mapping-summary`,
            create: `${window.AppConfig.apiBaseUrl}/core/systemadmin/document-parameter-mapping`,
            update: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/document-parameter-mapping/${id}`; },
            delete: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/document-parameter-mapping/${id}`; },
            read: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/document-parameter-mapping/${id}`; },
            documentTypes: `${window.AppConfig.apiBaseUrl}/core/systemadmin/document-types-ddl`,
            documentParameters: `${window.AppConfig.apiBaseUrl}/core/systemadmin/document-parameters-ddl`
        },
        gridOptions: {
            pageSize: 20,
            sortable: true,
            filterable: true,
            height: 560,
            pageable: { refresh: true, pageSizes: [10, 20, 50, 100], buttonCount: 5 }
        },
        windowOptions: { width: '820px' },
        grid: { columns: [{field: 'mappingId', title: 'ID', width: 90, dataType: 'number', filterable: false}, {field: 'documentTypeId', title: 'Document Type', width: 140, dataType: 'number'}, {field: 'parameterId', title: 'Parameter', width: 140, dataType: 'number'}, {field: 'isVisible', title: 'Visible', width: 120, kind: 'boolean', dataType: 'boolean', trueText: 'Yes', falseText: 'No'}] },
        form: { fields: [{name: 'mappingId', label: 'Mapping Id', type: 'hidden'}, {name: 'documentTypeId', label: 'Document Type', type: 'select', required: true, dataSourceEndpoint: function () { return window.DocumentParameterMappingModule.config.apiEndpoints.documentTypes; }, dataTextField: 'documentname', dataValueField: 'documenttypeid', optionLabel: 'Select Document Type...'}, {name: 'parameterId', label: 'Document Parameter', type: 'select', required: true, dataSourceEndpoint: function () { return window.DocumentParameterMappingModule.config.apiEndpoints.documentParameters; }, dataTextField: 'parameterName', dataValueField: 'parameterId', optionLabel: 'Select Parameter...'}, {name: 'isVisible', label: 'Visible', type: 'checkbox', defaultValue: true}], buildPayload: function (values) {
                function toInteger(value) { const parsed = parseInt(value || 0, 10); return Number.isNaN(parsed) ? 0 : parsed; }
                function toNullableInteger(value) { if (value === null || value === undefined || value === '') { return null; } const parsed = parseInt(value, 10); return Number.isNaN(parsed) ? null : parsed; }
                return { mappingId: toInteger(values.mappingId), documentTypeId: toNullableInteger(values.documentTypeId), parameterId: toNullableInteger(values.parameterId), isVisible: values.isVisible !== false };
            } }
    };

    window.DocumentParameterMappingModule.config.moduleRef = window.DocumentParameterMappingModule;
    window.DocumentParameterMappingModule.config.form.fields.forEach(function (field) { field.moduleRef = window.DocumentParameterMappingModule; });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () { window.location.href = '/Account/Login'; }, 1500);
            return;
        }

        window.DocumentParameterMappingModule.Summary?.init?.();
        window.DocumentParameterMappingModule.Details?.init?.();

        $(window.DocumentParameterMappingModule.config.dom.addButton).on('click', function () { window.DocumentParameterMappingModule.Details?.openAddForm?.(); });
        $(window.DocumentParameterMappingModule.config.dom.refreshButton).on('click', function () { window.DocumentParameterMappingModule.Summary?.refreshGrid?.(); });
    });
})();
