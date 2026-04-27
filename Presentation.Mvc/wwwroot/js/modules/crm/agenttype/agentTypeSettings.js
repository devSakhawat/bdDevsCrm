(function () {
    'use strict';

    window.AgentTypeModule = window.AgentTypeModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.AgentTypeModule.config = {
        moduleTitle: 'Agent Type',
        pluralTitle: 'Agent Types',
        idField: 'agentTypeId',
        displayField: 'agentTypeName',
        dom: {
            grid: '#agentTypeGrid',
            window: '#agentTypeWindow',
            form: '#agentTypeForm',
            addButton: '#btnAddAgentType',
            refreshButton: '#btnRefreshAgentType',
            saveButton: '#btnSaveAgentType',
            cancelButton: '#btnCancelAgentType'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-agent-type-summary`,
            create: `${apiRoot}/crm-agent-type`,
            update: function (id) { return `${apiRoot}/crm-agent-type/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-agent-type/${id}`; },
            read: function (id) { return `${apiRoot}/crm-agent-type/${id}`; }
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
            columns: [{field: "agentTypeId", title: "#", width: 60, dataType: "number", filterable: false}, {field: "agentTypeName", title: "Name", width: 200}, {field: "description", title: "Description", width: 250}, {field: "isActive", title: "Status", width: 100, dataType: "boolean", kind: "boolean", trueText: "Active", falseText: "Inactive"}]
        },
        form: {
            fields: [{name: "agentTypeId", label: "Agent Type Id", type: "hidden"}, {name: "agentTypeName", label: "Agent Type Name", type: "text", required: true, maxLength: 100, placeholder: "Enter agent type name"}, {name: "description", label: "Description", type: "textarea", maxLength: 255, wide: true, placeholder: "Enter description"}, {name: "isActive", label: "Active", type: "checkbox", defaultValue: true}],
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
                    agentTypeId: toInteger(values.agentTypeId),
                    agentTypeName: (values.agentTypeName || '').trim(),
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

    window.AgentTypeModule.config.moduleRef = window.AgentTypeModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.AgentTypeModule.Summary?.init?.();
        window.AgentTypeModule.Details?.init?.();

        $(window.AgentTypeModule.config.dom.addButton).on('click', function () {
            window.AgentTypeModule.Details?.openAddForm?.();
        });

        $(window.AgentTypeModule.config.dom.refreshButton).on('click', function () {
            window.AgentTypeModule.Summary?.refreshGrid?.();
        });
    });
})();
