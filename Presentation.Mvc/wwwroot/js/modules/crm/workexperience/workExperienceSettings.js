(function () {
    'use strict';

    window.WorkExperienceModule = window.WorkExperienceModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.WorkExperienceModule.config = {
        moduleTitle: 'Work Experience',
        pluralTitle: 'Work Experiences',
        idField: 'workExperienceId',
        displayField: 'nameOfEmployer',
        dom: {
            grid: '#workExperienceGrid',
            window: '#workExperienceWindow',
            form: '#workExperienceForm',
            addButton: '#btnAddWorkExperience',
            refreshButton: '#btnRefreshWorkExperience',
            saveButton: '#btnSaveWorkExperience',
            cancelButton: '#btnCancelWorkExperience'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-work-experience-summary`,
            create: `${apiRoot}/crm-work-experience`,
            update: function (id) { return `${apiRoot}/crm-work-experience/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-work-experience/${id}`; },
            read: function (id) { return `${apiRoot}/crm-work-experience/${id}`; },

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
                { field: 'workExperienceId', title: 'ID', width: 90, dataType: 'number', filterable: false },
                { field: 'applicantId', title: 'Applicant ID', width: 120, dataType: 'number' },
                { field: 'nameOfEmployer', title: 'Employer', width: 200 },
                { field: 'position', title: 'Position', width: 160 },
                { field: 'startDate', title: 'Start Date', width: 140 },
                { field: 'endDate', title: 'End Date', width: 140 },
                { field: 'period', title: 'Period', width: 120 },
                { field: 'documentName', title: 'Document Name', width: 180 }
            ]
        },
        form: {
            fields: [

                { name: 'workExperienceId', label: 'Work Experience Id', type: 'hidden' },
                {
                    name: 'applicantId',
                    label: 'Applicant',
                    type: 'select',
                    required: true,
                    dataSourceEndpoint: function () { return window.WorkExperienceModule.config.apiEndpoints.applicants; },
                    dataTextField: 'applicantName',
                    dataValueField: 'applicantId',
                    optionLabel: 'Select Applicant...'
                },
                { name: 'nameOfEmployer', label: 'Employer', type: 'text', required: true, maxLength: 100, placeholder: 'Enter employer name' },
                { name: 'position', label: 'Position', type: 'text', maxLength: 255, placeholder: 'Enter position' },
                { name: 'startDate', label: 'Start Date', type: 'date' },
                { name: 'endDate', label: 'End Date', type: 'date' },
                { name: 'period', label: 'Period', type: 'number', decimals: 2, step: 0.5, min: 0, placeholder: 'Enter period in years' },
                {
                    name: 'mainResponsibility',
                    label: 'Main Responsibility',
                    type: 'textarea',
                    maxLength: 255,
                    wide: true,
                    placeholder: 'Enter key responsibilities'
                },
                { name: 'documentName', label: 'Document Name', type: 'text', maxLength: 255, placeholder: 'Enter document name' },
                { name: 'scannedCopyPath', label: 'Document Path', type: 'text', maxLength: 255, wide: true, placeholder: 'Enter scanned copy path' }
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
                    workExperienceId: toInteger(values.workExperienceId),
                    applicantId: toInteger(values.applicantId),
                    nameOfEmployer: normalizeString(values.nameOfEmployer),
                    position: normalizeString(values.position),
                    startDate: values.startDate || null,
                    endDate: values.endDate || null,
                    period: values.period === null || values.period === undefined || values.period === '' ? null : Number(values.period),
                    mainResponsibility: normalizeString(values.mainResponsibility),
                    scannedCopyPath: normalizeString(values.scannedCopyPath),
                    documentName: normalizeString(values.documentName),
                    createdDate: state.currentRecord?.createdDate || now,
                    createdBy: state.currentRecord?.createdBy || 1,
                    updatedDate: state.isEditMode ? now : null,
                    updatedBy: state.isEditMode ? 1 : null
                };
            }
        }
    };

    window.WorkExperienceModule.config.moduleRef = window.WorkExperienceModule;
    window.WorkExperienceModule.config.form.fields.forEach(function (field) {
        field.moduleRef = window.WorkExperienceModule;
    });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.WorkExperienceModule.Summary?.init?.();
        window.WorkExperienceModule.Details?.init?.();

        $(window.WorkExperienceModule.config.dom.addButton).on('click', function () {
            window.WorkExperienceModule.Details?.openAddForm?.();
        });

        $(window.WorkExperienceModule.config.dom.refreshButton).on('click', function () {
            window.WorkExperienceModule.Summary?.refreshGrid?.();
        });
    });
})();
