(function () {
    'use strict';

    window.StudentModule = window.StudentModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.StudentModule.config = {
        moduleTitle: 'Student',
        pluralTitle: 'Students',
        idField: 'studentId',
        displayField: 'studentName',
        dom: {
            grid: '#studentGrid',
            window: '#studentWindow',
            form: '#studentForm',
            addButton: '#btnAddStudent',
            refreshButton: '#btnRefreshStudent',
            saveButton: '#btnSaveStudent',
            cancelButton: '#btnCancelStudent'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-student-summary`,
            create: `${apiRoot}/crm-student`,
            update: function (id) { return `${apiRoot}/crm-student/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-student/${id}`; },
            read: function (id) { return `${apiRoot}/crm-student/${id}`; },
            applicationReady: function (id) { return `${apiRoot}/crm-student-application-ready/${id}`; },
            academicByStudent: function (id) { return `${apiRoot}/crm-student-academic-profiles-by-student/${id}`; },
            statusHistoryByStudent: function (id) { return `${apiRoot}/crm-student-status-histories-by-student/${id}`; }
        },
        gridOptions: {
            pageSize: 20,
            sortable: true,
            filterable: true,
            height: 560,
            pageable: { refresh: true, pageSizes: [10, 20, 50, 100], buttonCount: 5 }
        },
        windowOptions: {
            width: '1200px'
        },
        grid: {
            columns: [
                { field: 'studentId', title: '#', width: 60, dataType: 'number', filterable: false },
                { field: 'studentName', title: 'Student Name', width: 180 },
                { field: 'studentCode', title: 'Code', width: 110 },
                { field: 'email', title: 'Email', width: 180 },
                { field: 'phone', title: 'Phone', width: 120 },
                { field: 'branchId', title: 'Branch', width: 90, dataType: 'number' },
                { field: 'preferredCountryId', title: 'Preferred Country', width: 120, dataType: 'number' },
                { field: 'isApplicationReady', title: 'App Ready', width: 100, dataType: 'boolean', kind: 'boolean', trueText: 'Ready', falseText: 'Pending' },
                { field: 'isDeleted', title: 'Deleted', width: 90, dataType: 'boolean', kind: 'boolean', trueText: 'Yes', falseText: 'No' }
            ]
        },
        form: {
            fields: [
                { name: 'studentId', label: 'Student Id', type: 'hidden' },
                { name: 'studentName', label: 'Student Name', type: 'text', required: true, maxLength: 150 },
                { name: 'studentCode', label: 'Student Code', type: 'text', maxLength: 50 },
                { name: 'email', label: 'Email', type: 'text', maxLength: 150 },
                { name: 'phone', label: 'Phone', type: 'text', maxLength: 50 },
                { name: 'leadId', label: 'Lead Id', type: 'number', min: 0 },
                { name: 'studentStatusId', label: 'Student Status Id', type: 'number', min: 0 },
                { name: 'agentId', label: 'Agent Id', type: 'number', min: 0 },
                { name: 'counselorId', label: 'Counselor Id', type: 'number', min: 0 },
                { name: 'branchId', label: 'Branch Id', type: 'number', min: 0 },
                { name: 'processingOfficerId', label: 'Processing Officer Id', type: 'number', min: 0 },
                { name: 'dateOfBirth', label: 'Date Of Birth', type: 'date' },
                { name: 'gender', label: 'Gender (1-3)', type: 'number', min: 1, max: 3 },
                { name: 'passportNumber', label: 'Passport Number', type: 'text', maxLength: 50 },
                { name: 'passportExpiryDate', label: 'Passport Expiry Date', type: 'date' },
                { name: 'passportIssueDate', label: 'Passport Issue Date', type: 'date' },
                { name: 'passportIssueCountryId', label: 'Passport Issue Country Id', type: 'number', min: 0 },
                { name: 'visaTypeId', label: 'Visa Type Id', type: 'number', min: 0 },
                { name: 'nationality', label: 'Nationality', type: 'text', maxLength: 100 },
                { name: 'nationalityCountryId', label: 'Nationality Country Id', type: 'number', min: 0 },
                { name: 'emergencyContactName', label: 'Emergency Contact Name', type: 'text', maxLength: 150 },
                { name: 'emergencyContactPhone', label: 'Emergency Contact Phone', type: 'text', maxLength: 50 },
                { name: 'emergencyContactRelation', label: 'Emergency Contact Relation', type: 'text', maxLength: 50 },
                { name: 'preferredCountryId', label: 'Preferred Country Id', type: 'number', min: 0 },
                { name: 'preferredDegreeLevelId', label: 'Preferred Degree Level Id', type: 'number', min: 0 },
                { name: 'desiredIntake', label: 'Desired Intake', type: 'text', maxLength: 100 },
                { name: 'ieltsStatus', label: 'IELTS Status (1-3)', type: 'number', min: 0, max: 3 },
                { name: 'ieltsScore', label: 'IELTS Score', type: 'number', decimals: 2, step: 0.5, min: 0 },
                { name: 'ieltsExamDate', label: 'IELTS Exam Date', type: 'date' },
                { name: 'isApplicationReady', label: 'Application Ready', type: 'checkbox', defaultValue: false },
                { name: 'applicationReadyDate', label: 'Application Ready Date', type: 'date' },
                { name: 'applicationReadySetBy', label: 'Application Ready Set By', type: 'number', min: 0 },
                { name: 'consentPersonalData', label: 'Consent Personal Data', type: 'checkbox', defaultValue: false },
                { name: 'consentMarketing', label: 'Consent Marketing', type: 'checkbox', defaultValue: false },
                { name: 'consentDocumentProcessing', label: 'Consent Document Processing', type: 'checkbox', defaultValue: false },
                { name: 'consentInternationalSharing', label: 'Consent International Sharing', type: 'checkbox', defaultValue: false },
                { name: 'consentTermsAccepted', label: 'Consent Terms Accepted', type: 'checkbox', defaultValue: false },
                { name: 'isDeleted', label: 'Deleted', type: 'checkbox', defaultValue: false },
                { name: 'isActive', label: 'Active', type: 'checkbox', defaultValue: true }
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
                    studentId: toInteger(values.studentId),
                    studentName: (values.studentName || '').trim(),
                    studentCode: normalizeString(values.studentCode),
                    email: normalizeString(values.email),
                    phone: normalizeString(values.phone),
                    leadId: toInteger(values.leadId),
                    studentStatusId: toInteger(values.studentStatusId),
                    agentId: toInteger(values.agentId),
                    counselorId: toInteger(values.counselorId),
                    branchId: toInteger(values.branchId),
                    processingOfficerId: toInteger(values.processingOfficerId),
                    dateOfBirth: normalizeString(values.dateOfBirth),
                    gender: toInteger(values.gender),
                    passportNumber: normalizeString(values.passportNumber),
                    passportExpiryDate: normalizeString(values.passportExpiryDate),
                    passportIssueDate: normalizeString(values.passportIssueDate),
                    passportIssueCountryId: toInteger(values.passportIssueCountryId),
                    visaTypeId: toInteger(values.visaTypeId),
                    nationality: normalizeString(values.nationality),
                    nationalityCountryId: toInteger(values.nationalityCountryId),
                    emergencyContactName: normalizeString(values.emergencyContactName),
                    emergencyContactPhone: normalizeString(values.emergencyContactPhone),
                    emergencyContactRelation: normalizeString(values.emergencyContactRelation),
                    preferredCountryId: toInteger(values.preferredCountryId),
                    preferredDegreeLevelId: toInteger(values.preferredDegreeLevelId),
                    desiredIntake: normalizeString(values.desiredIntake),
                    ieltsStatus: toInteger(values.ieltsStatus),
                    ieltsScore: toDecimal(values.ieltsScore),
                    ieltsExamDate: normalizeString(values.ieltsExamDate),
                    isApplicationReady: values.isApplicationReady === true,
                    applicationReadyDate: normalizeString(values.applicationReadyDate),
                    applicationReadySetBy: toInteger(values.applicationReadySetBy),
                    consentPersonalData: values.consentPersonalData === true,
                    consentMarketing: values.consentMarketing === true,
                    consentDocumentProcessing: values.consentDocumentProcessing === true,
                    consentInternationalSharing: values.consentInternationalSharing === true,
                    consentTermsAccepted: values.consentTermsAccepted === true,
                    isDeleted: values.isDeleted === true,
                    isActive: values.isActive !== false,
                    createdDate: state.currentRecord?.createdDate || now,
                    createdBy: state.currentRecord?.createdBy || 1,
                    updatedDate: state.isEditMode ? now : null,
                    updatedBy: state.isEditMode ? 1 : null
                };
            }
        }
    };

    window.StudentModule.config.moduleRef = window.StudentModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () { window.location.href = '/Account/Login'; }, 1500);
            return;
        }

        window.StudentModule.Summary?.init?.();
        window.StudentModule.Details?.init?.();

        $(window.StudentModule.config.dom.addButton).on('click', function () {
            window.StudentModule.Details?.openAddForm?.();
        });

        $(window.StudentModule.config.dom.refreshButton).on('click', function () {
            window.StudentModule.Summary?.refreshGrid?.();
        });
    });
})();
