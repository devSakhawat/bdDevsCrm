(function () {
    'use strict';

    window.AdditionalDocumentModule = window.AdditionalDocumentModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.AdditionalDocumentModule.config = {
        moduleTitle: 'Additional Document',
        pluralTitle: 'Additional Document Records',
        idField: 'AdditionalDocumentId',
        displayField: 'DocumentTitle',
        dom: {
            grid: '#additionalDocumentGrid',
            window: '#additionalDocumentWindow',
            form: '#additionalDocumentForm',
            addButton: '#btnAddAdditionalDocument',
            refreshButton: '#btnRefreshAdditionalDocument',
            saveButton: '#btnSaveAdditionalDocument',
            cancelButton: '#btnCancelAdditionalDocument'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-additional-document-summary`,
            create: `${apiRoot}/crm-additional-document`,
            update: function (id) { return `${apiRoot}/crm-additional-document/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-additional-document/${id}`; },
            read: function (id) { return `${apiRoot}/crm-additional-document/${id}`; },
            applicants: `${apiRoot}/crm-applicant-infos-ddl`
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
            width: '960px'
        },
        grid: {
            columns: [
                { field: 'AdditionalDocumentId', title: 'ID', width: 90, dataType: 'number', filterable: false },
                { field: 'ApplicantId', title: 'Applicant ID', width: 120, dataType: 'number' },
                { field: 'DocumentTitle', title: 'Document Title', width: 220 },
                { field: 'DocumentName', title: 'Document Name', width: 220 },
                { field: 'RecordType', title: 'Record Type', width: 130 },
                { field: 'DocumentPath', title: 'Document Path', width: 260, kind: 'multiline' }
            ]
        },
        form: {
            fields: [

                { name: 'AdditionalDocumentId', label: 'Additional Document Id', type: 'hidden' },
                {
                    name: 'ApplicantId',
                    label: 'Applicant',
                    type: 'select',
                    dataSourceEndpoint: function () { return window.AdditionalDocumentModule.config.apiEndpoints.applicants; },
                    dataTextField: 'applicantName',
                    dataValueField: 'applicantId',
                    optionLabel: 'Select Applicant...'
                },
                { name: 'DocumentTitle', label: 'Document Title', type: 'text', required: true, maxLength: 255, placeholder: 'Enter document title' },
                { name: 'DocumentName', label: 'Document Name', type: 'text', required: true, maxLength: 255, placeholder: 'Enter document name' },
                { name: 'RecordType', label: 'Record Type', type: 'text', required: true, maxLength: 255, placeholder: 'Enter record type', defaultValue: 'Document' },
                { name: 'DocumentPath', label: 'Document Path', type: 'text', required: true, maxLength: 255, wide: true, placeholder: 'Enter document path' }
        
            ],
            buildPayload: function (values, state) {
                function toInteger(value) {
                    const parsed = parseInt(value || 0, 10);
                    return Number.isNaN(parsed) ? 0 : parsed;
                }

                function normalizeString(value) {
                    return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim();
                }


                const now = new Date().toISOString();
                return {
                    AdditionalDocumentId: toInteger(values.AdditionalDocumentId),
                    ApplicantId: values.ApplicantId ? toInteger(values.ApplicantId) : null,
                    DocumentTitle: normalizeString(values.DocumentTitle) || '',
                    DocumentPath: normalizeString(values.DocumentPath) || '',
                    DocumentName: normalizeString(values.DocumentName) || '',
                    RecordType: normalizeString(values.RecordType) || 'Document',
                    createdDate: state.currentRecord?.CreatedDate || now,
                    createdBy: state.currentRecord?.CreatedBy || 1,
                    updatedDate: state.isEditMode ? now : null,
                    updatedBy: state.isEditMode ? 1 : null
                };
        
            }
        }
    };

    window.AdditionalDocumentModule.config.moduleRef = window.AdditionalDocumentModule;
    window.AdditionalDocumentModule.config.form.fields.forEach(function (field) {
        field.moduleRef = window.AdditionalDocumentModule;
    });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.AdditionalDocumentModule.Summary?.init?.();
        window.AdditionalDocumentModule.Details?.init?.();

        $(window.AdditionalDocumentModule.config.dom.addButton).on('click', function () {
            window.AdditionalDocumentModule.Details?.openAddForm?.();
        });

        $(window.AdditionalDocumentModule.config.dom.refreshButton).on('click', function () {
            window.AdditionalDocumentModule.Summary?.refreshGrid?.();
        });
    });
})();
