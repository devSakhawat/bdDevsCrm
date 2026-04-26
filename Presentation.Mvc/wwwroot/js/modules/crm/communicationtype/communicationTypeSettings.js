(function () {
    'use strict';

    window.CommunicationTypeModule = window.CommunicationTypeModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.CommunicationTypeModule.config = {
        moduleTitle: 'Communication Type',
        pluralTitle: 'Communication Types',
        idField: 'communicationTypeId',
        displayField: 'communicationTypeName',
        dom: {
            grid: '#communicationTypeGrid',
            window: '#communicationTypeWindow',
            form: '#communicationTypeForm',
            addButton: '#btnAddCommunicationType',
            refreshButton: '#btnRefreshCommunicationType',
            saveButton: '#btnSaveCommunicationType',
            cancelButton: '#btnCancelCommunicationType'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-communication-type-summary`,
            create: `${apiRoot}/crm-communication-type`,
            update: function (id) { return `${apiRoot}/crm-communication-type/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-communication-type/${id}`; },
            read: function (id) { return `${apiRoot}/crm-communication-type/${id}`; }
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
            columns: [{ field: "communicationTypeId", title: "ID", width: 90, dataType: "number", filterable: false }, { field: "communicationTypeName", title: "Communication Type Name", width: 250 }, { field: "isDigitalChannel", title: "Digital", width: 120, kind: "boolean", dataType: "boolean", trueText: "Yes", falseText: "No" }]
        },
        form: {
            fields: [{name: "communicationTypeId", label: "Communication Type Id", type: "hidden"}, {name: "communicationTypeName", label: "Communication Type Name", type: "text", required: true, maxLength: "100", placeholder: "Enter communication type name"}, {name: "isDigitalChannel", label: "Digital Channel", type: "checkbox", defaultValue: true}],
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
                        communicationTypeId: toInteger(values.communicationTypeId),
                        communicationTypeName: (values.communicationTypeName || "").trim(),
                        isDigitalChannel: values.isDigitalChannel !== false,
                        createdDate: state.currentRecord?.createdDate || now,
                        createdBy: state.currentRecord?.createdBy || 1,
                        updatedDate: state.isEditMode ? now : null,
                        updatedBy: state.isEditMode ? 1 : null
                };
            }
        }
    };

    window.CommunicationTypeModule.config.moduleRef = window.CommunicationTypeModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.CommunicationTypeModule.Summary?.init?.();
        window.CommunicationTypeModule.Details?.init?.();

        $(window.CommunicationTypeModule.config.dom.addButton).on('click', function () {
            window.CommunicationTypeModule.Details?.openAddForm?.();
        });

        $(window.CommunicationTypeModule.config.dom.refreshButton).on('click', function () {
            window.CommunicationTypeModule.Summary?.refreshGrid?.();
        });
    });
})();
