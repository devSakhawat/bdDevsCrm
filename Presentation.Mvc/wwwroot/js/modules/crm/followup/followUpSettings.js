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
            summary: `${apiRoot}/crm-followup-summary`,
            create: `${apiRoot}/crm-followup`,
            update: function (id) { return `${apiRoot}/crm-followup/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-followup/${id}`; },
            read: function (id) { return `${apiRoot}/crm-followup/${id}`; },
            historyByFollowUp: function (followUpId) { return `${apiRoot}/crm-followup-histories-by-followup/${followUpId}`; }
        },
        gridOptions: {
            pageSize: 20,
            sortable: true,
            filterable: true,
            height: 560,
            pageable: { refresh: true, pageSizes: [10, 20, 50, 100], buttonCount: 5 }
        },
        windowOptions: {
            width: '860px'
        },
        grid: {
            columns: [
                { field: 'followUpId', title: '#', width: 60, dataType: 'number', filterable: false },
                { field: 'leadId', title: 'Lead', width: 90, dataType: 'number' },
                { field: 'followUpDate', title: 'Follow Up Date', width: 130 },
                { field: 'scheduledTime', title: 'Scheduled Time', width: 110 },
                { field: 'followUpType', title: 'Type', width: 130 },
                { field: 'contactMethod', title: 'Contact', width: 90, dataType: 'number' },
                { field: 'status', title: 'Status', width: 90, dataType: 'number' },
                { field: 'missedReason', title: 'Missed Reason', width: 180 }
            ]
        },
        form: {
            fields: [
                { name: 'followUpId', label: 'Id', type: 'hidden' },
                { name: 'leadId', label: 'Lead Id', type: 'number', min: 0 },
                { name: 'enquiryId', label: 'Enquiry Id', type: 'number', min: 0 },
                { name: 'followUpDate', label: 'Follow Up Date', type: 'date', required: true },
                { name: 'scheduledTime', label: 'Scheduled Time', type: 'text', maxLength: 20, placeholder: 'HH:mm' },
                { name: 'followUpType', label: 'Follow Up Type', type: 'text', maxLength: 100, placeholder: 'Call / Visit / WhatsApp' },
                { name: 'contactMethod', label: 'Contact Method', type: 'number', required: true, min: 1, max: 4 },
                { name: 'status', label: 'Status', type: 'number', required: true, min: 1, max: 5 },
                { name: 'notes', label: 'Remarks / Notes', type: 'textarea', wide: true, maxLength: 1000 },
                { name: 'missedReason', label: 'Missed Reason', type: 'textarea', wide: true, maxLength: 500 },
                { name: 'nextFollowUpDate', label: 'Next Follow Up Date', type: 'date' },
                { name: 'overriddenById', label: 'Overridden By Id', type: 'number', min: 0 },
                { name: 'cancelledById', label: 'Cancelled By Id', type: 'number', min: 0 },
                { name: 'cancelledDate', label: 'Cancelled Date', type: 'date' },
                { name: 'counselorId', label: 'Counselor Id', type: 'number', min: 0 }
            ],
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
                    scheduledTime: normalizeString(values.scheduledTime),
                    followUpType: normalizeString(values.followUpType),
                    contactMethod: toInteger(values.contactMethod),
                    notes: normalizeString(values.notes),
                    nextFollowUpDate: normalizeString(values.nextFollowUpDate),
                    status: toInteger(values.status) || 1,
                    missedReason: normalizeString(values.missedReason),
                    overriddenById: toInteger(values.overriddenById),
                    cancelledById: toInteger(values.cancelledById),
                    cancelledDate: normalizeString(values.cancelledDate),
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
            setTimeout(function () { window.location.href = '/Account/Login'; }, 1500);
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
