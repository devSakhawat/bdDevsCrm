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
            read: function (id) { return `${apiRoot}/crm-agent/${id}`; },
            agentTypesDdl: `${apiRoot}/crm-agent-types-ddl`
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
            columns: [{field: "agentId", title: "#", width: 60, dataType: "number", filterable: false}, {field: "agentName", title: "Agent Name", width: 200}, {field: "agentCode", title: "Code", width: 130}, {field: "agentTypeId", title: "Type ID", width: 100, dataType: "number"}, {field: "contactPerson", title: "Contact", width: 150}, {field: "email", title: "Email", width: 180}, {field: "phone", title: "Phone", width: 130}, {field: "isActive", title: "Status", width: 100, dataType: "boolean", kind: "boolean", trueText: "Active", falseText: "Inactive"}]
        },
        form: {
            fields: [{name: "agentId", label: "Agent Id", type: "hidden"}, {name: "agentName", label: "Agent Name", type: "text", required: true, maxLength: 150, placeholder: "Enter agent name"}, {name: "agentCode", label: "Agent Code", type: "text", maxLength: 50, placeholder: "Enter agent code"}, {name: "agentTypeId", label: "Agent Type", type: "select", required: true, dataTextField: "agentTypeName", dataValueField: "agentTypeId", dataSourceEndpoint: `${apiRoot}/crm-agent-types-ddl`}, {name: "contactPerson", label: "Contact Person", type: "text", maxLength: 100, placeholder: "Enter contact person"}, {name: "email", label: "Email", type: "text", maxLength: 150, placeholder: "Enter email"}, {name: "phone", label: "Phone", type: "text", maxLength: 50, placeholder: "Enter phone"}, {name: "address", label: "Address", type: "textarea", maxLength: 255, wide: true, placeholder: "Enter address"}, {name: "country", label: "Country", type: "text", maxLength: 100, placeholder: "Enter country"}, {name: "commissionRate", label: "Commission Rate", type: "number", placeholder: "0.00", decimals: 2, step: 0.01, min: 0}, {name: "isActive", label: "Active", type: "checkbox", defaultValue: true}],
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
                    agentCode: normalizeString(values.agentCode),
                    agentTypeId: toInteger(values.agentTypeId),
                    contactPerson: normalizeString(values.contactPerson),
                    email: normalizeString(values.email),
                    phone: normalizeString(values.phone),
                    address: normalizeString(values.address),
                    country: normalizeString(values.country),
                    commissionRate: values.commissionRate === null ? null : Number(values.commissionRate),
                    isActive: values.isActive !== false,
                    createdDate: state.currentRecord?.createdDate || now,
                    createdBy: state.currentRecord?.createdBy || 1,
                    updatedDate: state.isEditMode ? now : null,
                    updatedBy: state.isEditMode ? 1 : null
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
