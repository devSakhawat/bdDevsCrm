(function () {
    'use strict';

    window.DocumentTemplateModule = window.DocumentTemplateModule || {};

    window.DocumentTemplateModule.config = {
        moduleTitle: 'Document Template',
        pluralTitle: 'Document Templates',
        idField: 'documentId',
        displayField: 'templateName',
        dom: {
            grid: '#documentTemplateGrid',
            window: '#documentTemplateWindow',
            form: '#documentTemplateForm',
            addButton: '#btnAddDocumentTemplate',
            refreshButton: '#btnRefreshDocumentTemplate',
            saveButton: '#btnSaveDocumentTemplate',
            cancelButton: '#btnCancelDocumentTemplate'
        },
        apiEndpoints: {
            summary: `${window.AppConfig.apiBaseUrl}/core/systemadmin/document-template-summary`,
            create: `${window.AppConfig.apiBaseUrl}/core/systemadmin/document-template`,
            update: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/document-template/${id}`; },
            delete: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/document-template/${id}`; },
            read: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/document-template/${id}`; },
            documentTypes: `${window.AppConfig.apiBaseUrl}/core/systemadmin/document-types-ddl`
        },
        gridOptions: {
            pageSize: 20,
            sortable: true,
            filterable: true,
            height: 560,
            pageable: {
                refresh: true,
                pageSizes: [10, 20, 50, 100],
                buttonCount: 5
            }
        },
        windowOptions: {
            width: '760px'
        },
        grid: {
            columns: [{field: 'documentId', title: 'ID', width: 90, dataType: 'number', filterable: false}, {field: 'templateName', title: 'Template Name', width: 220}, {field: 'documentTitle', title: 'Document Title', width: 240}, {field: 'documentTypeId', title: 'Document Type', width: 140, dataType: 'number'}]
        },
        form: {
            fields: [{name: 'documentId', label: 'Document Id', type: 'hidden'}, {name: 'documentTitle', label: 'Document Title', type: 'text', required: true, maxLength: 200, wide: true, placeholder: 'Enter document title'}, {name: 'templateName', label: 'Template Name', type: 'text', required: true, maxLength: 150, placeholder: 'Enter template name'}, {name: 'documentTypeId', label: 'Document Type', type: 'select', dataSourceEndpoint: function () { return window.DocumentTemplateModule.config.apiEndpoints.documentTypes; }, dataTextField: 'documentname', dataValueField: 'documenttypeid', optionLabel: 'Select Document Type...'}, {name: 'documentText', label: 'Document Text', type: 'textarea', wide: true, maxLength: 4000, placeholder: 'Enter document template text'}],
            buildPayload: function (values) {
                function normalizeString(value) {
                    return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim();
                }

                function toInteger(value) {
                    const parsed = parseInt(value || 0, 10);
                    return Number.isNaN(parsed) ? 0 : parsed;
                }

                function toNullableInteger(value) {
                    if (value === null || value === undefined || value === '') {
                        return null;
                    }
                    const parsed = parseInt(value, 10);
                    return Number.isNaN(parsed) ? null : parsed;
                }

                return {
                    documentId: toInteger(values.documentId),
                    documentTitle: normalizeString(values.documentTitle),
                    documentText: normalizeString(values.documentText),
                    templateName: normalizeString(values.templateName),
                    documentTypeId: toNullableInteger(values.documentTypeId)
                };
            }
        }
    };

    window.DocumentTemplateModule.config.moduleRef = window.DocumentTemplateModule;
    window.DocumentTemplateModule.config.form.fields.forEach(function (field) {
        field.moduleRef = window.DocumentTemplateModule;
    });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.DocumentTemplateModule.Summary?.init?.();
        window.DocumentTemplateModule.Details?.init?.();

        $(window.DocumentTemplateModule.config.dom.addButton).on('click', function () {
            window.DocumentTemplateModule.Details?.openAddForm?.();
        });

        $(window.DocumentTemplateModule.config.dom.refreshButton).on('click', function () {
            window.DocumentTemplateModule.Summary?.refreshGrid?.();
        });
    });
})();
