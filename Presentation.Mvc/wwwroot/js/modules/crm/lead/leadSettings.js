(function () {
    'use strict';

    window.LeadModule = window.LeadModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.LeadModule.config = {
        moduleTitle: 'Lead',
        pluralTitle: 'Leads',
        idField: 'leadId',
        displayField: 'leadName',
        dom: {
            grid: '#leadGrid',
            window: '#leadWindow',
            form: '#leadForm',
            addButton: '#btnAddLead',
            refreshButton: '#btnRefreshLead',
            saveButton: '#btnSaveLead',
            cancelButton: '#btnCancelLead'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-lead-summary`,
            create: `${apiRoot}/crm-lead`,
            update: function (id) { return `${apiRoot}/crm-lead/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-lead/${id}`; },
            read: function (id) { return `${apiRoot}/crm-lead/${id}`; },
            leadSourcesDdl: `${apiRoot}/crm-lead-sources-ddl`,
            leadStatusesDdl: `${apiRoot}/crm-lead-statuses-ddl`,
            counselorsDdl: `${apiRoot}/crm-counselors-ddl`,
            agentsDdl: `${apiRoot}/crm-agents-ddl`
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
            columns: [{field: "leadId", title: "#", width: 60, dataType: "number", filterable: false}, {field: "leadName", title: "Lead Name", width: 200}, {field: "email", title: "Email", width: 180}, {field: "phone", title: "Phone", width: 130}, {field: "countryOfInterest", title: "Country", width: 150}, {field: "courseOfInterest", title: "Course", width: 150}, {field: "isActive", title: "Status", width: 100, dataType: "boolean", kind: "boolean", trueText: "Active", falseText: "Inactive"}]
        },
        form: {
            fields: [{name: "leadId", label: "Lead Id", type: "hidden"}, {name: "leadName", label: "Lead Name", type: "text", required: true, maxLength: 150, placeholder: "Enter lead name"}, {name: "email", label: "Email", type: "text", maxLength: 150, placeholder: "Enter email"}, {name: "phone", label: "Phone", type: "text", maxLength: 50, placeholder: "Enter phone"}, {name: "leadSourceId", label: "Lead Source", type: "select", dataTextField: "sourceName", dataValueField: "leadSourceId", dataSourceEndpoint: `${apiRoot}/crm-lead-sources-ddl`}, {name: "leadStatusId", label: "Lead Status", type: "select", dataTextField: "statusName", dataValueField: "leadStatusId", dataSourceEndpoint: `${apiRoot}/crm-lead-statuses-ddl`}, {name: "assignedCounselorId", label: "Assigned Counselor", type: "select", dataTextField: "counselorName", dataValueField: "counselorId", dataSourceEndpoint: `${apiRoot}/crm-counselors-ddl`}, {name: "agentId", label: "Agent", type: "select", dataTextField: "agentName", dataValueField: "agentId", dataSourceEndpoint: `${apiRoot}/crm-agents-ddl`}, {name: "countryOfInterest", label: "Country Of Interest", type: "text", maxLength: 100, placeholder: "Enter country of interest"}, {name: "courseOfInterest", label: "Course Of Interest", type: "text", maxLength: 200, placeholder: "Enter course of interest"}, {name: "budget", label: "Budget", type: "number", placeholder: "0.00", decimals: 2, step: 0.01, min: 0}, {name: "notes", label: "Notes", type: "textarea", maxLength: 1000, wide: true, placeholder: "Enter notes"}, {name: "isActive", label: "Active", type: "checkbox", defaultValue: true}],
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
                    leadId: toInteger(values.leadId),
                    leadName: (values.leadName || '').trim(),
                    email: normalizeString(values.email),
                    phone: normalizeString(values.phone),
                    leadSourceId: toInteger(values.leadSourceId),
                    leadStatusId: toInteger(values.leadStatusId),
                    assignedCounselorId: toInteger(values.assignedCounselorId),
                    agentId: toInteger(values.agentId),
                    countryOfInterest: normalizeString(values.countryOfInterest),
                    courseOfInterest: normalizeString(values.courseOfInterest),
                    budget: values.budget === null ? null : Number(values.budget),
                    notes: normalizeString(values.notes),
                    isActive: values.isActive !== false,
                    createdDate: state.currentRecord?.createdDate || now,
                    createdBy: state.currentRecord?.createdBy || 1,
                    updatedDate: state.isEditMode ? now : null,
                    updatedBy: state.isEditMode ? 1 : null
                };
            }
        }
    };

    window.LeadModule.config.moduleRef = window.LeadModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.LeadModule.Summary?.init?.();
        window.LeadModule.Details?.init?.();

        $(window.LeadModule.config.dom.addButton).on('click', function () {
            window.LeadModule.Details?.openAddForm?.();
        });

        $(window.LeadModule.config.dom.refreshButton).on('click', function () {
            window.LeadModule.Summary?.refreshGrid?.();
        });
    });
})();
