(function () {
    'use strict';

    window.AgentLeadModule = window.AgentLeadModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.AgentLeadModule.config = {
        moduleTitle: 'Agent Lead',
        pluralTitle: 'Agent Leads',
        idField: 'agentLeadId',
        displayField: 'agentLeadId',
        dom: {
            grid: '#agentLeadGrid',
            window: '#agentLeadWindow',
            form: '#agentLeadForm',
            addButton: '#btnAddAgentLead',
            refreshButton: '#btnRefreshAgentLead',
            saveButton: '#btnSaveAgentLead',
            cancelButton: '#btnCancelAgentLead'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-agent-lead-summary`,
            create: `${apiRoot}/crm-agent-lead`,
            update: function (id) { return `${apiRoot}/crm-agent-lead/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-agent-lead/${id}`; },
            read: function (id) { return `${apiRoot}/crm-agent-lead/${id}`; }
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
            columns: [
                { field: 'agentLeadId', title: '#', width: 60, dataType: 'number', filterable: false },
                { field: 'agentId', title: 'Agent ID', width: 90, dataType: 'number' },
                { field: 'leadId', title: 'Lead ID', width: 90, dataType: 'number' },
                { field: 'assignedDate', title: 'Assigned Date', width: 140, dataType: 'date', format: '{0:dd MMM yyyy}' },
                { field: 'assignedBy', title: 'Assigned By', width: 110, dataType: 'number' },
                { field: 'notes', title: 'Notes', width: 220 },
                { field: 'isActive', title: 'Active', width: 80, dataType: 'boolean', kind: 'boolean', trueText: 'Active', falseText: 'Inactive' }
            ]
        },
        form: {
            fields: [
                { name: 'agentLeadId', label: 'Agent Lead Id', type: 'hidden' },
                { name: 'agentId', label: 'Agent ID', type: 'number', required: true, min: 1, placeholder: 'Agent ID' },
                { name: 'leadId', label: 'Lead ID', type: 'number', required: true, min: 1, placeholder: 'Lead ID' },
                { name: 'assignedDate', label: 'Assigned Date', type: 'date', required: true, placeholder: 'dd MMM yyyy' },
                { name: 'assignedBy', label: 'Assigned By (User ID)', type: 'number', required: true, min: 1, placeholder: 'User ID' },
                { name: 'notes', label: 'Notes', type: 'textarea', maxLength: 500, wide: true, placeholder: 'Assignment notes' },
                { name: 'isActive', label: 'Active', type: 'checkbox', defaultValue: true }
            ],
            buildPayload: function (values, state) {
                function toInt(value) {
                    const parsed = parseInt(value || 0, 10);
                    return Number.isNaN(parsed) ? 0 : parsed;
                }
                function toNullableString(value) {
                    return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim();
                }
                const now = new Date().toISOString();
                return {
                    agentLeadId: toInt(values.agentLeadId),
                    agentId: toInt(values.agentId),
                    leadId: toInt(values.leadId),
                    assignedDate: values.assignedDate || now,
                    assignedBy: toInt(values.assignedBy) || 1,
                    notes: toNullableString(values.notes),
                    isActive: values.isActive !== false,
                    createdDate: state.currentRecord?.createdDate || now,
                    createdBy: state.currentRecord?.createdBy || 1,
                    updatedDate: state.isEditMode ? now : null,
                    updatedBy: state.isEditMode ? 1 : null
                };
            }
        }
    };

    window.AgentLeadModule.config.moduleRef = window.AgentLeadModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () { window.location.href = '/Account/Login'; }, 1500);
            return;
        }

        window.AgentLeadModule.Summary?.init?.();
        window.AgentLeadModule.Details?.init?.();

        $(window.AgentLeadModule.config.dom.addButton).on('click', function () {
            window.AgentLeadModule.Details?.openAddForm?.();
        });

        $(window.AgentLeadModule.config.dom.refreshButton).on('click', function () {
            window.AgentLeadModule.Summary?.refreshGrid?.();
        });
    });
})();
