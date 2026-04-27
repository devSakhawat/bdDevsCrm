(function () {
    'use strict';

    window.LeadStageModule = window.LeadStageModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.LeadStageModule.config = {
        moduleTitle: 'Lead Stage',
        pluralTitle: 'Lead Stages',
        idField: 'leadStageId',
        displayField: 'leadStageName',
        dom: {
            grid: '#leadStageGrid',
            window: '#leadStageWindow',
            form: '#leadStageForm',
            addButton: '#btnAddLeadStage',
            refreshButton: '#btnRefreshLeadStage',
            saveButton: '#btnSaveLeadStage',
            cancelButton: '#btnCancelLeadStage'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-lead-stage-summary`,
            create: `${apiRoot}/crm-lead-stage`,
            update: function (id) { return `${apiRoot}/crm-lead-stage/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-lead-stage/${id}`; },
            read: function (id) { return `${apiRoot}/crm-lead-stage/${id}`; }
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
            columns: [{ field: "leadStageId", title: "ID", width: 90, dataType: "number", filterable: false }, { field: "leadStageName", title: "Lead Stage Name", width: 220 }, { field: "stageType", title: "Stage Type", width: 180 }, { field: "isClosedStage", title: "Closed", width: 120, kind: "boolean", dataType: "boolean", trueText: "Yes", falseText: "No" }]
        },
        form: {
            fields: [{name: "leadStageId", label: "Lead Stage Id", type: "hidden"}, {name: "leadStageName", label: "Lead Stage Name", type: "text", required: true, maxLength: "100", placeholder: "Enter lead stage name"}, {name: "stageType", label: "Stage Type", type: "text", maxLength: "50", placeholder: "Inquiry / Qualified / Converted / Lost"}, {name: "isClosedStage", label: "Closed Stage", type: "checkbox", defaultValue: false}],
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
                        leadStageId: toInteger(values.leadStageId),
                        leadStageName: (values.leadStageName || "").trim(),
                        stageType: normalizeString(values.stageType),
                        isClosedStage: !!values.isClosedStage,
                        createdDate: state.currentRecord?.createdDate || now,
                        createdBy: state.currentRecord?.createdBy || 1,
                        updatedDate: state.isEditMode ? now : null,
                        updatedBy: state.isEditMode ? 1 : null
                };
            }
        }
    };

    window.LeadStageModule.config.moduleRef = window.LeadStageModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.LeadStageModule.Summary?.init?.();
        window.LeadStageModule.Details?.init?.();

        $(window.LeadStageModule.config.dom.addButton).on('click', function () {
            window.LeadStageModule.Details?.openAddForm?.();
        });

        $(window.LeadStageModule.config.dom.refreshButton).on('click', function () {
            window.LeadStageModule.Summary?.refreshGrid?.();
        });
    });
})();
