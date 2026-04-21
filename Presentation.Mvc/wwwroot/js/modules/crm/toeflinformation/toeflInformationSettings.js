(function () {
    'use strict';

    window.ToeflInformationModule = window.ToeflInformationModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.ToeflInformationModule.config = {
        moduleTitle: 'TOEFL Information',
        pluralTitle: 'TOEFL Information Records',
        idField: 'TOEFLInformationId',
        displayField: 'TOEFLOverallScore',
        dom: {
            grid: '#toeflInformationGrid',
            window: '#toeflInformationWindow',
            form: '#toeflInformationForm',
            addButton: '#btnAddToeflInformation',
            refreshButton: '#btnRefreshToeflInformation',
            saveButton: '#btnSaveToeflInformation',
            cancelButton: '#btnCancelToeflInformation'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-toefl-information-summary`,
            create: `${apiRoot}/crm-toefl-information`,
            update: function (id) { return `${apiRoot}/crm-toefl-information/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-toefl-information/${id}`; },
            read: function (id) { return `${apiRoot}/crm-toefl-information/${id}`; },
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
                { field: 'TOEFLInformationId', title: 'ID', width: 90, dataType: 'number', filterable: false },
                { field: 'ApplicantId', title: 'Applicant ID', width: 120, dataType: 'number' },
                { field: 'TOEFLListening', title: 'Listening', width: 120, dataType: 'number' },
                { field: 'TOEFLReading', title: 'Reading', width: 120, dataType: 'number' },
                { field: 'TOEFLWriting', title: 'Writing', width: 120, dataType: 'number' },
                { field: 'TOEFLSpeaking', title: 'Speaking', width: 120, dataType: 'number' },
                { field: 'TOEFLOverallScore', title: 'Overall', width: 120, dataType: 'number' },
                { field: 'TOEFLDate', title: 'Exam Date', width: 150 }
            ]
        },
        form: {
            fields: [

                { name: 'TOEFLInformationId', label: 'TOEFL Information Id', type: 'hidden' },
                {
                    name: 'ApplicantId',
                    label: 'Applicant',
                    type: 'select',
                    required: true,
                    dataSourceEndpoint: function () { return window.ToeflInformationModule.config.apiEndpoints.applicants; },
                    dataTextField: 'applicantName',
                    dataValueField: 'applicantId',
                    optionLabel: 'Select Applicant...'
                },
                { name: 'TOEFLListening', label: 'Listening Score', type: 'number', decimals: 1, step: 0.5, min: 0 },
                { name: 'TOEFLReading', label: 'Reading Score', type: 'number', decimals: 1, step: 0.5, min: 0 },
                { name: 'TOEFLWriting', label: 'Writing Score', type: 'number', decimals: 1, step: 0.5, min: 0 },
                { name: 'TOEFLSpeaking', label: 'Speaking Score', type: 'number', decimals: 1, step: 0.5, min: 0 },
                { name: 'TOEFLOverallScore', label: 'Overall Score', type: 'number', decimals: 1, step: 0.5, min: 0 },
                { name: 'TOEFLDate', label: 'Exam Date', type: 'date' },
                { name: 'TOEFLScannedCopyPath', label: 'Document Path', type: 'text', maxLength: 255, wide: true, placeholder: 'Enter scanned copy path' },
                { name: 'TOEFLAdditionalInformation', label: 'Additional Information', type: 'textarea', maxLength: 255, wide: true, placeholder: 'Enter additional information' }
        
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
                    TOEFLInformationId: toInteger(values.TOEFLInformationId),
                    ApplicantId: toInteger(values.ApplicantId),
                    Toefllistening: toDecimal(values.TOEFLListening),
                    Toeflreading: toDecimal(values.TOEFLReading),
                    Toeflwriting: toDecimal(values.TOEFLWriting),
                    Toeflspeaking: toDecimal(values.TOEFLSpeaking),
                    ToefloverallScore: toDecimal(values.TOEFLOverallScore),
                    Toefldate: values.TOEFLDate || null,
                    ToeflscannedCopyPath: normalizeString(values.TOEFLScannedCopyPath),
                    ToefladditionalInformation: normalizeString(values.TOEFLAdditionalInformation),
                    createdDate: state.currentRecord?.CreatedDate || now,
                    createdBy: state.currentRecord?.CreatedBy || 1,
                    updatedDate: state.isEditMode ? now : null,
                    updatedBy: state.isEditMode ? 1 : null
                };
        
            }
        }
    };

    window.ToeflInformationModule.config.moduleRef = window.ToeflInformationModule;
    window.ToeflInformationModule.config.form.fields.forEach(function (field) {
        field.moduleRef = window.ToeflInformationModule;
    });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.ToeflInformationModule.Summary?.init?.();
        window.ToeflInformationModule.Details?.init?.();

        $(window.ToeflInformationModule.config.dom.addButton).on('click', function () {
            window.ToeflInformationModule.Details?.openAddForm?.();
        });

        $(window.ToeflInformationModule.config.dom.refreshButton).on('click', function () {
            window.ToeflInformationModule.Summary?.refreshGrid?.();
        });
    });
})();
