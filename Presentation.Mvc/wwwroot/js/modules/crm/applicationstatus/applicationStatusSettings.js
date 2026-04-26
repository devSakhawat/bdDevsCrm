(function () {
    'use strict';

    window.ApplicationStatusModule = window.ApplicationStatusModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.ApplicationStatusModule.config = {
        moduleTitle: 'Application Status',
        pluralTitle: 'Application Statuses',
        idField: 'applicationStatusId',
        displayField: 'applicationStatusName',
        dom: {
            grid: '#applicationStatusGrid',
            window: '#applicationStatusWindow',
            form: '#applicationStatusForm',
            addButton: '#btnAddApplicationStatus',
            refreshButton: '#btnRefreshApplicationStatus',
            saveButton: '#btnSaveApplicationStatus',
            cancelButton: '#btnCancelApplicationStatus'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-application-status-summary`,
            create: `${apiRoot}/crm-application-status`,
            update: function (id) { return `${apiRoot}/crm-application-status/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-application-status/${id}`; },
            read: function (id) { return `${apiRoot}/crm-application-status/${id}`; }
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
            columns: [{ field: "applicationStatusId", title: "ID", width: 90, dataType: "number", filterable: false }, { field: "applicationStatusName", title: "Application Status Name", width: 230 }, { field: "sequenceNo", title: "Sequence No", width: 120, dataType: "number" }, { field: "isFinalStatus", title: "Final", width: 120, kind: "boolean", dataType: "boolean", trueText: "Yes", falseText: "No" }]
        },
        form: {
            fields: [{name: "applicationStatusId", label: "Application Status Id", type: "hidden"}, {name: "applicationStatusName", label: "Application Status Name", type: "text", required: true, maxLength: "100", placeholder: "Enter application status name"}, {name: "sequenceNo", label: "Sequence No", type: "number", placeholder: "0", min: 0, step: 1}, {name: "isFinalStatus", label: "Final Status", type: "checkbox", defaultValue: false}],
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
                        applicationStatusId: toInteger(values.applicationStatusId),
                        applicationStatusName: (values.applicationStatusName || "").trim(),
                        sequenceNo: toInteger(values.sequenceNo),
                        isFinalStatus: !!values.isFinalStatus,
                        createdDate: state.currentRecord?.createdDate || now,
                        createdBy: state.currentRecord?.createdBy || 1,
                        updatedDate: state.isEditMode ? now : null,
                        updatedBy: state.isEditMode ? 1 : null
                };
            }
        }
    };

    window.ApplicationStatusModule.config.moduleRef = window.ApplicationStatusModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.ApplicationStatusModule.Summary?.init?.();
        window.ApplicationStatusModule.Details?.init?.();

        $(window.ApplicationStatusModule.config.dom.addButton).on('click', function () {
            window.ApplicationStatusModule.Details?.openAddForm?.();
        });

        $(window.ApplicationStatusModule.config.dom.refreshButton).on('click', function () {
            window.ApplicationStatusModule.Summary?.refreshGrid?.();
        });
    });
})();
