(function () {
    'use strict';

    window.VisaStatusModule = window.VisaStatusModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.VisaStatusModule.config = {
        moduleTitle: 'Visa Status',
        pluralTitle: 'Visa Statuses',
        idField: 'visaStatusId',
        displayField: 'visaStatusName',
        dom: {
            grid: '#visaStatusGrid',
            window: '#visaStatusWindow',
            form: '#visaStatusForm',
            addButton: '#btnAddVisaStatus',
            refreshButton: '#btnRefreshVisaStatus',
            saveButton: '#btnSaveVisaStatus',
            cancelButton: '#btnCancelVisaStatus'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-visa-status-summary`,
            create: `${apiRoot}/crm-visa-status`,
            update: function (id) { return `${apiRoot}/crm-visa-status/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-visa-status/${id}`; },
            read: function (id) { return `${apiRoot}/crm-visa-status/${id}`; }
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
            columns: [{ field: "visaStatusId", title: "ID", width: 90, dataType: "number", filterable: false }, { field: "visaStatusName", title: "Visa Status Name", width: 230 }, { field: "sequenceNo", title: "Sequence No", width: 120, dataType: "number" }, { field: "isFinalStatus", title: "Final", width: 120, kind: "boolean", dataType: "boolean", trueText: "Yes", falseText: "No" }]
        },
        form: {
            fields: [{name: "visaStatusId", label: "Visa Status Id", type: "hidden"}, {name: "visaStatusName", label: "Visa Status Name", type: "text", required: true, maxLength: "100", placeholder: "Enter visa status name"}, {name: "sequenceNo", label: "Sequence No", type: "number", placeholder: "0", min: 0, step: 1}, {name: "isFinalStatus", label: "Final Status", type: "checkbox", defaultValue: false}],
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
                        visaStatusId: toInteger(values.visaStatusId),
                        visaStatusName: (values.visaStatusName || "").trim(),
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

    window.VisaStatusModule.config.moduleRef = window.VisaStatusModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.VisaStatusModule.Summary?.init?.();
        window.VisaStatusModule.Details?.init?.();

        $(window.VisaStatusModule.config.dom.addButton).on('click', function () {
            window.VisaStatusModule.Details?.openAddForm?.();
        });

        $(window.VisaStatusModule.config.dom.refreshButton).on('click', function () {
            window.VisaStatusModule.Summary?.refreshGrid?.();
        });
    });
})();
