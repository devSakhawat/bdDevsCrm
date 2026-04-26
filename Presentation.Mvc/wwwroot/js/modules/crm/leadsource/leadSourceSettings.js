(function () {
    'use strict';

    window.LeadSourceModule = window.LeadSourceModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.LeadSourceModule.config = {
        moduleTitle: 'Lead Source',
        pluralTitle: 'Lead Sources',
        idField: 'leadSourceId',
        displayField: 'leadSourceName',
        dom: {
            grid: '#leadSourceGrid',
            window: '#leadSourceWindow',
            form: '#leadSourceForm',
            addButton: '#btnAddLeadSource',
            refreshButton: '#btnRefreshLeadSource',
            saveButton: '#btnSaveLeadSource',
            cancelButton: '#btnCancelLeadSource'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-lead-source-summary`,
            create: `${apiRoot}/crm-lead-source`,
            update: function (id) { return `${apiRoot}/crm-lead-source/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-lead-source/${id}`; },
            read: function (id) { return `${apiRoot}/crm-lead-source/${id}`; }
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
            columns: [{ field: "leadSourceId", title: "ID", width: 90, dataType: "number", filterable: false }, { field: "leadSourceName", title: "Lead Source Name", width: 240 }, { field: "sortOrder", title: "Sort Order", width: 120, dataType: "number" }, { field: "isActive", title: "Status", width: 120, kind: "boolean", dataType: "boolean", trueText: "Active", falseText: "Inactive" }]
        },
        form: {
            fields: [{name: "leadSourceId", label: "Lead Source Id", type: "hidden"}, {name: "leadSourceName", label: "Lead Source Name", type: "text", required: true, maxLength: "100", placeholder: "Enter lead source name"}, {name: "sortOrder", label: "Sort Order", type: "number", placeholder: "0", min: 0, step: 1}, {name: "isActive", label: "Active", type: "checkbox", defaultValue: true}],
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
                        leadSourceId: toInteger(values.leadSourceId),
                        leadSourceName: (values.leadSourceName || "").trim(),
                        sortOrder: toInteger(values.sortOrder),
                        isActive: values.isActive !== false,
                        createdDate: state.currentRecord?.createdDate || now,
                        createdBy: state.currentRecord?.createdBy || 1,
                        updatedDate: state.isEditMode ? now : null,
                        updatedBy: state.isEditMode ? 1 : null
                };
            }
        }
    };

    window.LeadSourceModule.config.moduleRef = window.LeadSourceModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.LeadSourceModule.Summary?.init?.();
        window.LeadSourceModule.Details?.init?.();

        $(window.LeadSourceModule.config.dom.addButton).on('click', function () {
            window.LeadSourceModule.Details?.openAddForm?.();
        });

        $(window.LeadSourceModule.config.dom.refreshButton).on('click', function () {
            window.LeadSourceModule.Summary?.refreshGrid?.();
        });
    });
})();
