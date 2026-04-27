(function () {
    'use strict';

    window.FollowUpModule = window.FollowUpModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.FollowUpModule.config = {
        moduleTitle: 'Follow Up',
        pluralTitle: 'Follow Ups',
        idField: 'followUpId',
        displayField: 'followUpType',
        dom: {
            grid: '#followUpGrid',
            window: '#followUpWindow',
            form: '#followUpForm',
            addButton: '#btnAddFollowUp',
            refreshButton: '#btnRefreshFollowUp',
            saveButton: '#btnSaveFollowUp',
            cancelButton: '#btnCancelFollowUp'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-follow-up-summary`,
            create: `${apiRoot}/crm-follow-up`,
            update: function (id) { return `${apiRoot}/crm-follow-up/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-follow-up/${id}`; },
            read: function (id) { return `${apiRoot}/crm-follow-up/${id}`; },
            counselorsDdl: `${apiRoot}/crm-counselors-ddl`
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
            columns: [{field: "followUpId", title: "#", width: 60, dataType: "number", filterable: false}, {field: "leadId", title: "Lead ID", width: 100, dataType: "number"}, {field: "followUpDate", title: "Follow Up Date", width: 130}, {field: "followUpType", title: "Type", width: 150}, {field: "isCompleted", title: "Completed", width: 120, dataType: "boolean", kind: "boolean", trueText: "Yes", falseText: "No"}]
        },
        form: {
            fields: [{name: "followUpId", label: "Follow Up Id", type: "hidden"}, {name: "leadId", label: "Lead Id", type: "number", placeholder: "Enter lead id", min: 0}, {name: "enquiryId", label: "Enquiry Id", type: "number", placeholder: "Enter enquiry id", min: 0}, {name: "followUpDate", label: "Follow Up Date", type: "date"}, {name: "followUpType", label: "Follow Up Type", type: "text", maxLength: 100, placeholder: "Enter follow up type"}, {name: "notes", label: "Notes", type: "textarea", maxLength: 1000, wide: true, placeholder: "Enter notes"}, {name: "nextFollowUpDate", label: "Next Follow Up Date", type: "date"}, {name: "isCompleted", label: "Completed", type: "checkbox", defaultValue: false}, {name: "counselorId", label: "Counselor", type: "select", dataTextField: "counselorName", dataValueField: "counselorId", dataSourceEndpoint: `${apiRoot}/crm-counselors-ddl`}],
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
                    followUpId: toInteger(values.followUpId),
                    leadId: toInteger(values.leadId),
                    enquiryId: toInteger(values.enquiryId),
                    followUpDate: normalizeString(values.followUpDate),
                    followUpType: normalizeString(values.followUpType),
                    notes: normalizeString(values.notes),
                    nextFollowUpDate: normalizeString(values.nextFollowUpDate),
                    isCompleted: !!values.isCompleted,
                    counselorId: toInteger(values.counselorId),
                    createdDate: state.currentRecord?.createdDate || now,
                    createdBy: state.currentRecord?.createdBy || 1,
                    updatedDate: state.isEditMode ? now : null,
                    updatedBy: state.isEditMode ? 1 : null
                };
            }
        }
    };

    window.FollowUpModule.config.moduleRef = window.FollowUpModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.FollowUpModule.Summary?.init?.();
        window.FollowUpModule.Details?.init?.();

        $(window.FollowUpModule.config.dom.addButton).on('click', function () {
            window.FollowUpModule.Details?.openAddForm?.();
        });

        $(window.FollowUpModule.config.dom.refreshButton).on('click', function () {
            window.FollowUpModule.Summary?.refreshGrid?.();
        });
    });
})();
