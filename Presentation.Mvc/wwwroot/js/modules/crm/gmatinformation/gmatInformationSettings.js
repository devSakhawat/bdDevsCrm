(function () {
    'use strict';

    window.GmatInformationModule = window.GmatInformationModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.GmatInformationModule.config = {
        moduleTitle: 'GMAT Information',
        pluralTitle: 'GMAT Information Records',
        idField: 'GMATInformationId',
        displayField: 'GMATOverallScore',
        dom: {
            grid: '#gmatInformationGrid',
            window: '#gmatInformationWindow',
            form: '#gmatInformationForm',
            addButton: '#btnAddGmatInformation',
            refreshButton: '#btnRefreshGmatInformation',
            saveButton: '#btnSaveGmatInformation',
            cancelButton: '#btnCancelGmatInformation'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-gmat-information-summary`,
            create: `${apiRoot}/crm-gmat-information`,
            update: function (id) { return `${apiRoot}/crm-gmat-information/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-gmat-information/${id}`; },
            read: function (id) { return `${apiRoot}/crm-gmat-information/${id}`; },
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
            width: '960px'
        },
        grid: {
            columns: [
                { field: 'GMATInformationId', title: 'ID', width: 90, dataType: 'number', filterable: false },
                { field: 'ApplicantId', title: 'Applicant ID', width: 120, dataType: 'number' },
                { field: 'GMATListening', title: 'Listening', width: 120, dataType: 'number' },
                { field: 'GMATReading', title: 'Reading', width: 120, dataType: 'number' },
                { field: 'GMATWriting', title: 'Writing', width: 120, dataType: 'number' },
                { field: 'GMATSpeaking', title: 'Speaking', width: 120, dataType: 'number' },
                { field: 'GMATOverallScore', title: 'Overall', width: 120, dataType: 'number' },
                { field: 'GMATDate', title: 'Exam Date', width: 150 }
            ]
        },
        form: {
            fields: [

                { name: 'GMATInformationId', label: 'GMAT Information Id', type: 'hidden' },
                {
                    name: 'ApplicantId',
                    label: 'Applicant',
                    type: 'select',
                    required: true,
                    dataSourceEndpoint: function () { return window.GmatInformationModule.config.apiEndpoints.applicants; },
                    dataTextField: 'applicantName',
                    dataValueField: 'applicantId',
                    optionLabel: 'Select Applicant...'
                },
                { name: 'GMATListening', label: 'Listening Score', type: 'number', decimals: 1, step: 0.5, min: 0 },
                { name: 'GMATReading', label: 'Reading Score', type: 'number', decimals: 1, step: 0.5, min: 0 },
                { name: 'GMATWriting', label: 'Writing Score', type: 'number', decimals: 1, step: 0.5, min: 0 },
                { name: 'GMATSpeaking', label: 'Speaking Score', type: 'number', decimals: 1, step: 0.5, min: 0 },
                { name: 'GMATOverallScore', label: 'Overall Score', type: 'number', decimals: 1, step: 0.5, min: 0 },
                { name: 'GMATDate', label: 'Exam Date', type: 'date' },
                { name: 'GMATScannedCopyPath', label: 'Document Path', type: 'text', maxLength: 255, wide: true, placeholder: 'Enter scanned copy path' },
                { name: 'GMATAdditionalInformation', label: 'Additional Information', type: 'textarea', maxLength: 255, wide: true, placeholder: 'Enter additional information' }
        
            ],
            buildPayload: function (values, state) {
                function toInteger(value) {
                    const parsed = parseInt(value || 0, 10);
                    return Number.isNaN(parsed) ? 0 : parsed;
                }

                function toDecimal(value) {
                    if (value === null || value === undefined || value === '') {
                        return null;
                    }

                    const parsed = Number(value);
                    return Number.isNaN(parsed) ? null : parsed;
                }

                function normalizeString(value) {
                    return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim();
                }


                const now = new Date().toISOString();
                return {
                    GMATInformationId: toInteger(values.GMATInformationId),
                    ApplicantId: toInteger(values.ApplicantId),
                    Gmatlistening: toDecimal(values.GMATListening),
                    Gmatreading: toDecimal(values.GMATReading),
                    Gmatwriting: toDecimal(values.GMATWriting),
                    Gmatspeaking: toDecimal(values.GMATSpeaking),
                    GmatoverallScore: toDecimal(values.GMATOverallScore),
                    Gmatdate: values.GMATDate || null,
                    GmatscannedCopyPath: normalizeString(values.GMATScannedCopyPath),
                    GmatadditionalInformation: normalizeString(values.GMATAdditionalInformation),
                    createdDate: state.currentRecord?.CreatedDate || now,
                    createdBy: state.currentRecord?.CreatedBy || 1,
                    updatedDate: state.isEditMode ? now : null,
                    updatedBy: state.isEditMode ? 1 : null
                };
        
            }
        }
    };

    window.GmatInformationModule.config.moduleRef = window.GmatInformationModule;
    window.GmatInformationModule.config.form.fields.forEach(function (field) {
        field.moduleRef = window.GmatInformationModule;
    });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.GmatInformationModule.Summary?.init?.();
        window.GmatInformationModule.Details?.init?.();

        $(window.GmatInformationModule.config.dom.addButton).on('click', function () {
            window.GmatInformationModule.Details?.openAddForm?.();
        });

        $(window.GmatInformationModule.config.dom.refreshButton).on('click', function () {
            window.GmatInformationModule.Summary?.refreshGrid?.();
        });
    });
})();
