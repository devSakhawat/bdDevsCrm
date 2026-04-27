(function () {
    'use strict';

    window.LeadStatusModule = window.LeadStatusModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.LeadStatusModule.config = {
        moduleTitle: 'Lead Status',
        pluralTitle: 'Lead Statuses',
        idField: 'leadStatusId',
        displayField: 'statusName',
        dom: {
            grid: '#leadStatusGrid',
            window: '#leadStatusWindow',
            form: '#leadStatusForm',
            addButton: '#btnAddLeadStatus',
            refreshButton: '#btnRefreshLeadStatus',
            saveButton: '#btnSaveLeadStatus',
            cancelButton: '#btnCancelLeadStatus'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-lead-status-summary`,
            create: `${apiRoot}/crm-lead-status`,
            update: function (id) { return `${apiRoot}/crm-lead-status/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-lead-status/${id}`; },
            read: function (id) { return `${apiRoot}/crm-lead-status/${id}`; }
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
            columns: [{field: "leadStatusId", title: "#", width: 60, dataType: "number", filterable: false}, {field: "statusName", title: "Status Name", width: 200}, {field: "statusCode", title: "Code", width: 130}, {field: "colorCode", title: "Color", width: 130}, {field: "isActive", title: "Status", width: 100, dataType: "boolean", kind: "boolean", trueText: "Active", falseText: "Inactive"}]
        },
        form: {
            fields: [{name: "leadStatusId", label: "Lead Status Id", type: "hidden"}, {name: "statusName", label: "Status Name", type: "text", required: true, maxLength: 100, placeholder: "Enter status name"}, {name: "statusCode", label: "Status Code", type: "text", maxLength: 50, placeholder: "Enter status code"}, {name: "colorCode", label: "Color Code", type: "text", maxLength: 50, placeholder: "#FFFFFF"}, {name: "isActive", label: "Active", type: "checkbox", defaultValue: true}],
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
                    leadStatusId: toInteger(values.leadStatusId),
                    statusName: (values.statusName || '').trim(),
                    statusCode: normalizeString(values.statusCode),
                    colorCode: normalizeString(values.colorCode),
                    isActive: values.isActive !== false,
                    createdDate: state.currentRecord?.createdDate || now,
                    createdBy: state.currentRecord?.createdBy || 1,
                    updatedDate: state.isEditMode ? now : null,
                    updatedBy: state.isEditMode ? 1 : null
                };
            }
        }
    };

    window.LeadStatusModule.config.moduleRef = window.LeadStatusModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.LeadStatusModule.Summary?.init?.();
        window.LeadStatusModule.Details?.init?.();

        $(window.LeadStatusModule.config.dom.addButton).on('click', function () {
            window.LeadStatusModule.Details?.openAddForm?.();
        });

        $(window.LeadStatusModule.config.dom.refreshButton).on('click', function () {
            window.LeadStatusModule.Summary?.refreshGrid?.();
        });
    });
})();
