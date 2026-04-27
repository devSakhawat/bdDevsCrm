(function () {
    'use strict';

    window.VisaTypeModule = window.VisaTypeModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.VisaTypeModule.config = {
        moduleTitle: 'Visa Type',
        pluralTitle: 'Visa Types',
        idField: 'visaTypeId',
        displayField: 'visaTypeName',
        dom: {
            grid: '#visaTypeGrid',
            window: '#visaTypeWindow',
            form: '#visaTypeForm',
            addButton: '#btnAddVisaType',
            refreshButton: '#btnRefreshVisaType',
            saveButton: '#btnSaveVisaType',
            cancelButton: '#btnCancelVisaType'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-visa-type-summary`,
            create: `${apiRoot}/crm-visa-type`,
            update: function (id) { return `${apiRoot}/crm-visa-type/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-visa-type/${id}`; },
            read: function (id) { return `${apiRoot}/crm-visa-type/${id}`; }
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
            width: '600px'
        },
        grid: {
            columns: [{field: "visaTypeId", title: "#", width: 60, dataType: "number", filterable: false}, {field: "visaTypeName", title: "Visa Type", width: 200}, {field: "visaCode", title: "Code", width: 130}, {field: "description", title: "Description", width: 250}, {field: "isActive", title: "Status", width: 100, dataType: "boolean", kind: "boolean", trueText: "Active", falseText: "Inactive"}]
        },
        form: {
            fields: [{name: "visaTypeId", label: "Visa Type Id", type: "hidden"}, {name: "visaTypeName", label: "Visa Type Name", type: "text", required: true, maxLength: 100, placeholder: "Enter visa type name"}, {name: "visaCode", label: "Visa Code", type: "text", maxLength: 50, placeholder: "Enter visa code"}, {name: "description", label: "Description", type: "textarea", maxLength: 255, wide: true, placeholder: "Enter description"}, {name: "isActive", label: "Active", type: "checkbox", defaultValue: true}],
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
                    visaTypeId: toInteger(values.visaTypeId),
                    visaTypeName: (values.visaTypeName || '').trim(),
                    visaCode: normalizeString(values.visaCode),
                    description: normalizeString(values.description),
                    isActive: values.isActive !== false,
                    createdDate: state.currentRecord?.createdDate || now,
                    createdBy: state.currentRecord?.createdBy || 1,
                    updatedDate: state.isEditMode ? now : null,
                    updatedBy: state.isEditMode ? 1 : null
                };
            }
        }
    };

    window.VisaTypeModule.config.moduleRef = window.VisaTypeModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.VisaTypeModule.Summary?.init?.();
        window.VisaTypeModule.Details?.init?.();

        $(window.VisaTypeModule.config.dom.addButton).on('click', function () {
            window.VisaTypeModule.Details?.openAddForm?.();
        });

        $(window.VisaTypeModule.config.dom.refreshButton).on('click', function () {
            window.VisaTypeModule.Summary?.refreshGrid?.();
        });
    });
})();
