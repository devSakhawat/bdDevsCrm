(function () {
    'use strict';

    window.CounsellingSessionModule = window.CounsellingSessionModule || {};
    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.CounsellingSessionModule.config = {
        moduleTitle: 'Counselling Session',
        pluralTitle: 'Counselling Sessions',
        idField: 'counsellingSessionId',
        displayField: 'targetIntake',
        dom: {
            grid: '#counsellingSessionGrid',
            window: '#counsellingSessionWindow',
            form: '#counsellingSessionForm',
            addButton: '#btnAddCounsellingSession',
            refreshButton: '#btnRefreshCounsellingSession',
            saveButton: '#btnSaveCounsellingSession',
            cancelButton: '#btnCancelCounsellingSession'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-counselling-session-summary`,
            create: `${apiRoot}/crm-counselling-session`,
            update: function (id) { return `${apiRoot}/crm-counselling-session/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-counselling-session/${id}`; },
            read: function (id) { return `${apiRoot}/crm-counselling-session/${id}`; },
            byLead: function (leadId) { return `${apiRoot}/crm-counselling-sessions-by-lead/${leadId}`; },
            shortlistBySession: function (sessionId) { return `${apiRoot}/crm-session-program-shortlists-by-session/${sessionId}`; },
            eligibility: function (studentId) { return `${apiRoot}/crm-counselling-session-eligibility/${studentId}`; }
        },
        gridOptions: {
            pageSize: 20,
            sortable: true,
            filterable: true,
            height: 560,
            pageable: { refresh: true, pageSizes: [10, 20, 50, 100], buttonCount: 5 }
        },
        windowOptions: {
            width: '980px'
        },
        grid: {
            columns: [
                { field: 'counsellingSessionId', title: '#', width: 60, dataType: 'number', filterable: false },
                { field: 'leadId', title: 'Lead', width: 90, dataType: 'number' },
                { field: 'branchId', title: 'Branch', width: 90, dataType: 'number' },
                { field: 'counselorId', title: 'Counselor', width: 100, dataType: 'number' },
                { field: 'sessionDate', title: 'Session Date', width: 130 },
                { field: 'duration', title: 'Duration (min)', width: 120, dataType: 'number' },
                { field: 'sessionType', title: 'Type', width: 100, dataType: 'number' },
                { field: 'outcome', title: 'Outcome', width: 100, dataType: 'number' },
                { field: 'isDeleted', title: 'Deleted', width: 90, dataType: 'boolean', kind: 'boolean', trueText: 'Yes', falseText: 'No' }
            ]
        },
        form: {
            fields: [
                { name: 'counsellingSessionId', label: 'Id', type: 'hidden' },
                { name: 'leadId', label: 'Lead Id', type: 'number', required: true, min: 1 },
                { name: 'branchId', label: 'Branch Id', type: 'number', required: true, min: 1 },
                { name: 'counselorId', label: 'Counselor Id', type: 'number', required: true, min: 1 },
                { name: 'sessionDate', label: 'Session Date', type: 'date', required: true },
                { name: 'duration', label: 'Duration (minutes)', type: 'number', required: true, min: 10 },
                { name: 'sessionType', label: 'Session Type', type: 'number', required: true, min: 1, max: 5 },
                { name: 'budgetDiscussed', label: 'Budget Discussed', type: 'number', decimals: 2, min: 0 },
                { name: 'targetIntake', label: 'Target Intake', type: 'text', maxLength: 100, placeholder: 'e.g. Fall 2027' },
                { name: 'outcome', label: 'Outcome', type: 'number', required: true, min: 1, max: 5 },
                { name: 'needsAssessmentNotes', label: 'Needs Assessment Notes', type: 'textarea', wide: true, maxLength: 2000 },
                { name: 'outcomeNotes', label: 'Outcome Notes', type: 'textarea', wide: true, maxLength: 2000 },
                { name: 'nextSteps', label: 'Next Steps', type: 'textarea', wide: true, maxLength: 2000 },
                { name: 'isDeleted', label: 'Deleted', type: 'checkbox', defaultValue: false }
            ],
            buildPayload: function (values, state) {
                function normalizeString(value) {
                    return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim();
                }
                function toInteger(value) {
                    const parsed = parseInt(value || 0, 10);
                    return Number.isNaN(parsed) ? 0 : parsed;
                }
                function toDecimal(value) {
                    const parsed = parseFloat(value || 0);
                    return Number.isNaN(parsed) ? null : parsed;
                }
                const now = new Date().toISOString();
                return {
                    counsellingSessionId: toInteger(values.counsellingSessionId),
                    leadId: toInteger(values.leadId),
                    branchId: toInteger(values.branchId),
                    counselorId: toInteger(values.counselorId),
                    sessionDate: normalizeString(values.sessionDate),
                    duration: toInteger(values.duration),
                    sessionType: toInteger(values.sessionType),
                    needsAssessmentNotes: normalizeString(values.needsAssessmentNotes),
                    budgetDiscussed: toDecimal(values.budgetDiscussed),
                    targetIntake: normalizeString(values.targetIntake),
                    outcome: toInteger(values.outcome),
                    outcomeNotes: normalizeString(values.outcomeNotes),
                    nextSteps: normalizeString(values.nextSteps),
                    isDeleted: values.isDeleted === true,
                    createdDate: state.currentRecord?.createdDate || now,
                    createdBy: state.currentRecord?.createdBy || 1,
                    updatedDate: state.isEditMode ? now : null,
                    updatedBy: state.isEditMode ? 1 : null
                };
            }
        }
    };

    window.CounsellingSessionModule.config.moduleRef = window.CounsellingSessionModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () { window.location.href = '/Account/Login'; }, 1500);
            return;
        }

        window.CounsellingSessionModule.Summary?.init?.();
        window.CounsellingSessionModule.Details?.init?.();

        $(window.CounsellingSessionModule.config.dom.addButton).on('click', function () {
            window.CounsellingSessionModule.Details?.openAddForm?.();
        });

        $(window.CounsellingSessionModule.config.dom.refreshButton).on('click', function () {
            window.CounsellingSessionModule.Summary?.refreshGrid?.();
        });
    });
})();
