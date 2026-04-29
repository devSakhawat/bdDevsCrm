(function () {
    'use strict';

    window.CommunicationTemplateModule = window.CommunicationTemplateModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.CommunicationTemplateModule.config = {
        moduleTitle: 'Communication Template',
        pluralTitle: 'Communication Templates',
        idField: 'communicationTemplateId',
        displayField: 'templateName',
        dom: {
            grid: '#communicationTemplateGrid',
            window: '#communicationTemplateWindow',
            form: '#communicationTemplateForm',
            addButton: '#btnAddCommunicationTemplate',
            refreshButton: '#btnRefreshCommunicationTemplate',
            saveButton: '#btnSaveCommunicationTemplate',
            cancelButton: '#btnCancelCommunicationTemplate'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-communication-template-summary`,
            create: `${apiRoot}/crm-communication-template`,
            update: function (id) { return `${apiRoot}/crm-communication-template/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-communication-template/${id}`; },
            read: function (id) { return `${apiRoot}/crm-communication-template/${id}`; }
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
            width: '900px'
        },
        grid: {
            columns: [{ field: "communicationTemplateId", title: "ID", width: 90, dataType: "number", filterable: false }, { field: "templateName", title: "Template Name", width: 220 }, { field: "communicationTypeName", title: "Communication Type", width: 180 }, { field: "subject", title: "Subject", width: 220 }, { field: "isActive", title: "Status", width: 120, kind: "boolean", dataType: "boolean", trueText: "Active", falseText: "Inactive" }]
        },
        form: {
            fields: [{ name: "communicationTemplateId", label: "Communication Template Id", type: "hidden" },
                { name: "communicationTypeId", label: "Communication Type", type: "select", required: true, dataSourceEndpoint: `${apiRoot}/crm-communication-types-ddl`, dataTextField: "communicationTypeName", dataValueField: "communicationTypeId", optionLabel: "Select communication type..." },
                { name: "templateName", label: "Template Name", type: "text", required: true, maxLength: 150, placeholder: "Enter template name" },
                { name: "subject", label: "Subject", type: "text", maxLength: 250, wide: true, placeholder: "Enter subject" },
                { name: "templateBody", label: "Template Body", type: "textarea", required: true, wide: true, placeholder: "Enter template body" },
                { name: "isActive", label: "Active", type: "checkbox", defaultValue: true }],
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
                    communicationTemplateId: toInteger(values.communicationTemplateId),
                    communicationTypeId: toInteger(values.communicationTypeId),
                    templateName: (values.templateName || '').trim(),
                    subject: normalizeString(values.subject),
                    templateBody: (values.templateBody || '').trim(),
                    isActive: values.isActive !== false,
                    createdDate: state.currentRecord?.createdDate || now,
                    createdBy: state.currentRecord?.createdBy || 1,
                    updatedDate: state.isEditMode ? now : null,
                    updatedBy: state.isEditMode ? 1 : null,
                    communicationTypeName: state.currentRecord?.communicationTypeName || null
                };
            }
        }
    };

    window.CommunicationTemplateModule.config.moduleRef = window.CommunicationTemplateModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.CommunicationTemplateModule.Summary?.init?.();
        window.CommunicationTemplateModule.Details?.init?.();

        $(window.CommunicationTemplateModule.config.dom.addButton).on('click', function () {
            window.CommunicationTemplateModule.Details?.openAddForm?.();
        });

        $(window.CommunicationTemplateModule.config.dom.refreshButton).on('click', function () {
            window.CommunicationTemplateModule.Summary?.refreshGrid?.();
        });
    });
})();
