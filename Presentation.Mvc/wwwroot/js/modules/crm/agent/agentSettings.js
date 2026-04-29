(function () {
    'use strict';

    window.AgentModule = window.AgentModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.AgentModule.config = {
        moduleTitle: 'Agent',
        pluralTitle: 'Agents',
        idField: 'agentId',
        displayField: 'agentName',
        dom: {
            grid: '#agentGrid',
            window: '#agentWindow',
            form: '#agentForm',
            addButton: '#btnAddAgent',
            refreshButton: '#btnRefreshAgent',
            saveButton: '#btnSaveAgent',
            cancelButton: '#btnCancelAgent'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-agent-summary`,
            create: `${apiRoot}/crm-agent`,
            update: function (id) { return `${apiRoot}/crm-agent/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-agent/${id}`; },
            read: function (id) { return `${apiRoot}/crm-agent/${id}`; }
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
            columns: [{ field: "agentId", title: "ID", width: 90, dataType: "number", filterable: false }, { field: "agentName", title: "Agent Name", width: 200 }, { field: "agencyName", title: "Agency Name", width: 180 }, { field: "primaryPhone", title: "Primary Phone", width: 150 }, { field: "primaryEmail", title: "Primary Email", width: 220 }, { field: "commissionTypeName", title: "Commission Type", width: 170 }, { field: "countryName", title: "Country", width: 160 }, { field: "defaultCommissionValue", title: "Default Commission", width: 150, kind: "currency", dataType: "number" }, { field: "isActive", title: "Status", width: 120, kind: "boolean", dataType: "boolean", trueText: "Active", falseText: "Inactive" }]
        },
        form: {
            fields: [{ name: "agentId", label: "Agent Id", type: "hidden" },
                { name: "agentName", label: "Agent Name", type: "text", required: true, maxLength: 200, placeholder: "Enter agent name" },
                { name: "agencyName", label: "Agency Name", type: "text", maxLength: 200, placeholder: "Enter agency name" },
                { name: "primaryPhone", label: "Primary Phone", type: "text", required: true, maxLength: 30, placeholder: "Enter primary phone" },
                { name: "primaryEmail", label: "Primary Email", type: "email", maxLength: 150, placeholder: "Enter primary email" },
                { name: "commissionTypeId", label: "Commission Type", type: "select", required: true, dataSourceEndpoint: `${apiRoot}/crm-commission-types-ddl`, dataTextField: "commissionTypeName", dataValueField: "commissionTypeId", optionLabel: "Select commission type..." },
                { name: "countryId", label: "Country", type: "select", required: true, dataSourceEndpoint: `${apiRoot}/countryddl`, dataTextField: "countryName", dataValueField: "countryId", optionLabel: "Select country..." },
                { name: "defaultCommissionValue", label: "Default Commission Value", type: "number", decimals: 2, step: 0.01, min: 0, placeholder: "0.00" },
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
                    agentId: toInteger(values.agentId),
                    agentName: (values.agentName || '').trim(),
                    agencyName: normalizeString(values.agencyName),
                    primaryPhone: (values.primaryPhone || '').trim(),
                    primaryEmail: normalizeString(values.primaryEmail),
                    commissionTypeId: toInteger(values.commissionTypeId),
                    defaultCommissionValue: values.defaultCommissionValue === null || values.defaultCommissionValue === '' ? null : Number(values.defaultCommissionValue),
                    countryId: toInteger(values.countryId),
                    isActive: values.isActive !== false,
                    createdDate: state.currentRecord?.createdDate || now,
                    createdBy: state.currentRecord?.createdBy || 1,
                    updatedDate: state.isEditMode ? now : null,
                    updatedBy: state.isEditMode ? 1 : null,
                    commissionTypeName: state.currentRecord?.commissionTypeName || null,
                    countryName: state.currentRecord?.countryName || null
                };
            }
        }
    };

    window.AgentModule.config.moduleRef = window.AgentModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.AgentModule.Summary?.init?.();
        window.AgentModule.Details?.init?.();

        $(window.AgentModule.config.dom.addButton).on('click', function () {
            window.AgentModule.Details?.openAddForm?.();
        });

        $(window.AgentModule.config.dom.refreshButton).on('click', function () {
            window.AgentModule.Summary?.refreshGrid?.();
        });
    });
})();
