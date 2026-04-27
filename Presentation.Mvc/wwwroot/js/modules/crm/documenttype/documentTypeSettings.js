(function () {
    'use strict';

    window.DocumentTypeModule = window.DocumentTypeModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.DocumentTypeModule.config = {
        moduleTitle: 'Document Type',
        pluralTitle: 'Document Types',
        idField: 'documentTypeId',
        displayField: 'documentTypeName',
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
            summary: `${apiRoot}/crm-document-type-summary`,
            create: `${apiRoot}/crm-document-type`,
            update: function (id) { return `${apiRoot}/crm-document-type/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-document-type/${id}`; },
            read: function (id) { return `${apiRoot}/crm-document-type/${id}`; }
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
        windowOptions: { width: '760px' },
        grid: {
            columns: [{ field: "documentTypeId", title: "ID", width: 90, dataType: "number", filterable: false }, { field: "documentTypeName", title: "Document Type Name", width: 220 }, { field: "code", title: "Code", width: 140 }, { field: "isMandatoryForApplication", title: "Mandatory", width: 130, kind: "boolean", dataType: "boolean", trueText: "Yes", falseText: "No" }, { field: "isActive", title: "Status", width: 120, kind: "boolean", dataType: "boolean", trueText: "Active", falseText: "Inactive" }]
        },
        form: {
            fields: [{name: "documentTypeId", label: "Document Type Id", type: "hidden"}, {name: "documentTypeName", label: "Document Type Name", type: "text", required: true, maxLength: "150", placeholder: "Enter document type name"}, {name: "code", label: "Code", type: "text", maxLength: "50", placeholder: "Enter code"}, {name: "isMandatoryForApplication", label: "Mandatory For Application", type: "checkbox", defaultValue: false}, {name: "isActive", label: "Active", type: "checkbox", defaultValue: true}],
            buildPayload: function (values, state) {
                function normalizeString(value) {
                    return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim();
                }

                function toInteger(value) {
                    const parsed = parseInt(value || 0, 10);
                    return Number.isNaN(parsed) ? 0 : parsed;
                }

                const now = new Date().toISOString();
                return {
                        documentTypeId: toInteger(values.documentTypeId),
                        documentTypeName: (values.documentTypeName || "").trim(),
                        code: normalizeString(values.code),
                        isMandatoryForApplication: !!values.isMandatoryForApplication,
                        isActive: values.isActive !== false,
                        createdDate: state.currentRecord?.createdDate || now,
                        createdBy: state.currentRecord?.createdBy || 1,
                        updatedDate: state.isEditMode ? now : null,
                        updatedBy: state.isEditMode ? 1 : null
                };
            }
        }
    };

    window.DocumentTypeModule.config.moduleRef = window.DocumentTypeModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.DocumentTypeModule.Summary?.init?.();
        window.DocumentTypeModule.Details?.init?.();

        $(window.DocumentTypeModule.config.dom.addButton).on('click', function () {
            window.DocumentTypeModule.Details?.openAddForm?.();
        });

        $(window.DocumentTypeModule.config.dom.refreshButton).on('click', function () {
            window.DocumentTypeModule.Summary?.refreshGrid?.();
        });
    });
})();
