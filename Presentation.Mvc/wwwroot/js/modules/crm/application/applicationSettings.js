(function () {
    'use strict';

    window.ApplicationModule = window.ApplicationModule || {};
    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.ApplicationModule.config = {
        moduleTitle: 'Application',
        pluralTitle: 'Applications',
        idField: 'applicationId',
        displayField: 'internalRefNo',
        dom: {
            grid: '#applicationGrid',
            window: '#applicationWindow',
            form: '#applicationForm',
            addButton: '#btnAddApplication',
            refreshButton: '#btnRefreshApplication',
            saveButton: '#btnSaveApplication',
            cancelButton: '#btnCancelApplication'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-application-summary`,
            create: `${apiRoot}/crm-application`,
            update: function (id) { return `${apiRoot}/crm-application/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-application/${id}`; },
            read: function (id) { return `${apiRoot}/crm-application/${id}`; },
            board: `${apiRoot}/crm-application-board`,
            conditionsByApplication: function (id) { return `${apiRoot}/crm-application-conditions-by-application/${id}`; },
            documentsByApplication: function (id) { return `${apiRoot}/crm-application-documents-by-application/${id}`; },
            statusTransition: `${apiRoot}/crm-application-status-transition`
        },
        gridOptions: {
            pageSize: 20,
            sortable: true,
            filterable: true,
            height: 560,
            pageable: { refresh: true, pageSizes: [10, 20, 50, 100], buttonCount: 5 }
        },
        windowOptions: {
            width: '1100px'
        },
        grid: {
            columns: [
                { field: 'applicationId', title: '#', width: 60, dataType: 'number', filterable: false },
                { field: 'internalRefNo', title: 'Ref No', width: 150 },
                { field: 'studentId', title: 'Student', width: 90, dataType: 'number' },
                { field: 'branchId', title: 'Branch', width: 90, dataType: 'number' },
                { field: 'countryId', title: 'Country', width: 90, dataType: 'number' },
                { field: 'programId', title: 'Program', width: 90, dataType: 'number' },
                { field: 'status', title: 'Status', width: 90, dataType: 'number' },
                { field: 'priority', title: 'Priority', width: 90, dataType: 'number' }
            ]
        },
        form: {
            fields: [
                { name: 'applicationId', label: 'Application Id', type: 'hidden' },
                { name: 'studentId', label: 'Student Id', type: 'number', required: true, min: 1 },
                { name: 'branchId', label: 'Branch Id', type: 'number', required: true, min: 1 },
                { name: 'processingOfficerId', label: 'Processing Officer Id', type: 'number', min: 0 },
                { name: 'countryId', label: 'Country Id', type: 'number', required: true, min: 1 },
                { name: 'universityId', label: 'University Id', type: 'number', required: true, min: 1 },
                { name: 'programId', label: 'Program Id', type: 'number', required: true, min: 1 },
                { name: 'intakeId', label: 'Intake Id', type: 'number', required: true, min: 1 },
                { name: 'status', label: 'Status', type: 'number', required: true, min: 1, max: 11 },
                { name: 'priority', label: 'Priority', type: 'number', required: true, min: 1, max: 3 },
                { name: 'appliedDate', label: 'Applied Date', type: 'date' },
                { name: 'offerReceivedDate', label: 'Offer Received Date', type: 'date' },
                { name: 'enrollmentDate', label: 'Enrollment Date', type: 'date' },
                { name: 'withdrawnDate', label: 'Withdrawn Date', type: 'date' },
                { name: 'offerDetails', label: 'Offer Details', type: 'textarea', wide: true, maxLength: 2000 },
                { name: 'withdrawalReason', label: 'Withdrawal Reason', type: 'textarea', wide: true, maxLength: 1000 },
                { name: 'rejectionReason', label: 'Rejection Reason', type: 'textarea', wide: true, maxLength: 1000 },
                { name: 'portalUsername', label: 'Portal Username', type: 'text', maxLength: 200 },
                { name: 'portalPassword', label: 'Portal Password', type: 'text', maxLength: 200 },
                { name: 'studentSnapshotJson', label: 'Student Snapshot', type: 'textarea', wide: true, maxLength: 4000 },
                { name: 'programSnapshotJson', label: 'Program Snapshot', type: 'textarea', wide: true, maxLength: 4000 },
                { name: 'offerSnapshotJson', label: 'Offer Snapshot', type: 'textarea', wide: true, maxLength: 4000 },
                { name: 'conditionSnapshotJson', label: 'Condition Snapshot', type: 'textarea', wide: true, maxLength: 4000 },
                { name: 'metaSnapshotJson', label: 'Meta Snapshot', type: 'textarea', wide: true, maxLength: 4000 },
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
                const now = new Date().toISOString();
                return {
                    applicationId: toInteger(values.applicationId),
                    studentId: toInteger(values.studentId),
                    branchId: toInteger(values.branchId),
                    processingOfficerId: toInteger(values.processingOfficerId),
                    countryId: toInteger(values.countryId),
                    universityId: toInteger(values.universityId),
                    programId: toInteger(values.programId),
                    intakeId: toInteger(values.intakeId),
                    studentSnapshotJson: normalizeString(values.studentSnapshotJson),
                    programSnapshotJson: normalizeString(values.programSnapshotJson),
                    offerSnapshotJson: normalizeString(values.offerSnapshotJson),
                    conditionSnapshotJson: normalizeString(values.conditionSnapshotJson),
                    metaSnapshotJson: normalizeString(values.metaSnapshotJson),
                    internalRefNo: state.currentRecord?.internalRefNo || null,
                    status: toInteger(values.status) || 1,
                    priority: toInteger(values.priority) || 1,
                    appliedDate: normalizeString(values.appliedDate),
                    offerReceivedDate: normalizeString(values.offerReceivedDate),
                    enrollmentDate: normalizeString(values.enrollmentDate),
                    withdrawnDate: normalizeString(values.withdrawnDate),
                    offerDetails: normalizeString(values.offerDetails),
                    withdrawalReason: normalizeString(values.withdrawalReason),
                    rejectionReason: normalizeString(values.rejectionReason),
                    portalUsername: normalizeString(values.portalUsername),
                    portalPassword: normalizeString(values.portalPassword),
                    isDeleted: values.isDeleted === true,
                    createdDate: state.currentRecord?.createdDate || now,
                    createdBy: state.currentRecord?.createdBy || 1,
                    updatedDate: state.isEditMode ? now : null,
                    updatedBy: state.isEditMode ? 1 : null
                };
            }
        }
    };

    window.ApplicationModule.config.moduleRef = window.ApplicationModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () { window.location.href = '/Account/Login'; }, 1500);
            return;
        }

        window.ApplicationModule.Summary?.init?.();
        window.ApplicationModule.Details?.init?.();

        $(window.ApplicationModule.config.dom.addButton).on('click', function () {
            window.ApplicationModule.Details?.openAddForm?.();
        });

        $(window.ApplicationModule.config.dom.refreshButton).on('click', function () {
            window.ApplicationModule.Summary?.refreshGrid?.();
        });
    });
})();
