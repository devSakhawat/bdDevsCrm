(function () {
    'use strict';

    window.EducationHistoryModule = window.EducationHistoryModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.EducationHistoryModule.config = {
        moduleTitle: 'Education History',
        pluralTitle: 'Education Histories',
        idField: 'educationHistoryId',
        displayField: 'institution',
        dom: {
            grid: '#educationHistoryGrid',
            window: '#educationHistoryWindow',
            form: '#educationHistoryForm',
            addButton: '#btnAddEducationHistory',
            refreshButton: '#btnRefreshEducationHistory',
            saveButton: '#btnSaveEducationHistory',
            cancelButton: '#btnCancelEducationHistory'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-education-history-summary`,
            create: `${apiRoot}/crm-education-history`,
            update: function (id) { return `${apiRoot}/crm-education-history/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-education-history/${id}`; },
            read: function (id) { return `${apiRoot}/crm-education-history/${id}`; },

            applicants: `${apiRoot}/crm-applicant-infos-ddl`
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
            width: '900px'
        },
        grid: {
            columns: [
                { field: 'educationHistoryId', title: 'ID', width: 90, dataType: 'number', filterable: false },
                { field: 'applicantId', title: 'Applicant ID', width: 120, dataType: 'number' },
                { field: 'institution', title: 'Institution', width: 220 },
                { field: 'qualification', title: 'Qualification', width: 180 },
                { field: 'passingYear', title: 'Passing Year', width: 120, dataType: 'number' },
                { field: 'grade', title: 'Grade', width: 120 },
                { field: 'documentName', title: 'Document Name', width: 180 }
            ]
        },
        form: {
            fields: [

                { name: 'educationHistoryId', label: 'Education History Id', type: 'hidden' },
                {
                    name: 'applicantId',
                    label: 'Applicant',
                    type: 'select',
                    required: true,
                    dataSourceEndpoint: function () { return window.EducationHistoryModule.config.apiEndpoints.applicants; },
                    dataTextField: 'applicantName',
                    dataValueField: 'applicantId',
                    optionLabel: 'Select Applicant...'
                },
                { name: 'institution', label: 'Institution', type: 'text', required: true, maxLength: 255, placeholder: 'Enter institution' },
                { name: 'qualification', label: 'Qualification', type: 'text', required: true, maxLength: 255, placeholder: 'Enter qualification' },
                { name: 'passingYear', label: 'Passing Year', type: 'number', min: 1900, step: 1, placeholder: 'Enter passing year' },
                { name: 'grade', label: 'Grade', type: 'text', maxLength: 255, placeholder: 'Enter grade' },
                { name: 'documentName', label: 'Document Name', type: 'text', maxLength: 255, placeholder: 'Enter document name' },
                { name: 'documentPath', label: 'Document Path', type: 'text', maxLength: 255, wide: true, placeholder: 'Enter document path' }
            ],
            buildPayload: function (values, state) {
                function toInteger(value) {
                    const parsed = parseInt(value || 0, 10);
                    return Number.isNaN(parsed) ? 0 : parsed;
                }

                function normalizeString(value) {
                    return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim();
                }


                const now = new Date().toISOString();
                return {
                    educationHistoryId: toInteger(values.educationHistoryId),
                    applicantId: toInteger(values.applicantId),
                    institution: normalizeString(values.institution),
                    qualification: normalizeString(values.qualification),
                    passingYear: values.passingYear === null || values.passingYear === undefined || values.passingYear === '' ? null : toInteger(values.passingYear),
                    grade: normalizeString(values.grade),
                    documentPath: normalizeString(values.documentPath),
                    documentName: normalizeString(values.documentName),
                    createdDate: state.currentRecord?.createdDate || now,
                    createdBy: state.currentRecord?.createdBy || 1,
                    updatedDate: state.isEditMode ? now : null,
                    updatedBy: state.isEditMode ? 1 : null
                };
            }
        }
    };

    window.EducationHistoryModule.config.moduleRef = window.EducationHistoryModule;
    window.EducationHistoryModule.config.form.fields.forEach(function (field) {
        field.moduleRef = window.EducationHistoryModule;
    });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.EducationHistoryModule.Summary?.init?.();
        window.EducationHistoryModule.Details?.init?.();

        $(window.EducationHistoryModule.config.dom.addButton).on('click', function () {
            window.EducationHistoryModule.Details?.openAddForm?.();
        });

        $(window.EducationHistoryModule.config.dom.refreshButton).on('click', function () {
            window.EducationHistoryModule.Summary?.refreshGrid?.();
        });
    });
})();
