(function () {
    'use strict';

    window.IeltsInformationModule = window.IeltsInformationModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.IeltsInformationModule.config = {
        moduleTitle: 'IELTS Information',
        pluralTitle: 'IELTS Information Records',
        idField: 'IELTSInformationId',
        displayField: 'IELTSOverallScore',
        dom: {
            grid: '#ieltsInformationGrid',
            window: '#ieltsInformationWindow',
            form: '#ieltsInformationForm',
            addButton: '#btnAddIeltsInformation',
            refreshButton: '#btnRefreshIeltsInformation',
            saveButton: '#btnSaveIeltsInformation',
            cancelButton: '#btnCancelIeltsInformation'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-ielts-information-summary`,
            create: `${apiRoot}/crm-ielts-information`,
            update: function (id) { return `${apiRoot}/crm-ielts-information/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-ielts-information/${id}`; },
            read: function (id) { return `${apiRoot}/crm-ielts-information/${id}`; },
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
                { field: 'IELTSInformationId', title: 'ID', width: 90, dataType: 'number', filterable: false },
                { field: 'ApplicantId', title: 'Applicant ID', width: 120, dataType: 'number' },
                { field: 'IELTSListening', title: 'Listening', width: 120, dataType: 'number' },
                { field: 'IELTSReading', title: 'Reading', width: 120, dataType: 'number' },
                { field: 'IELTSWriting', title: 'Writing', width: 120, dataType: 'number' },
                { field: 'IELTSSpeaking', title: 'Speaking', width: 120, dataType: 'number' },
                { field: 'IELTSOverallScore', title: 'Overall', width: 120, dataType: 'number' },
                { field: 'IELTSDate', title: 'Exam Date', width: 150 }
            ]
        },
        form: {
            fields: [

                { name: 'IELTSInformationId', label: 'IELTS Information Id', type: 'hidden' },
                {
                    name: 'ApplicantId',
                    label: 'Applicant',
                    type: 'select',
                    required: true,
                    dataSourceEndpoint: function () { return window.IeltsInformationModule.config.apiEndpoints.applicants; },
                    dataTextField: 'applicantName',
                    dataValueField: 'applicantId',
                    optionLabel: 'Select Applicant...'
                },
                { name: 'IELTSListening', label: 'Listening Score', type: 'number', decimals: 1, step: 0.5, min: 0 },
                { name: 'IELTSReading', label: 'Reading Score', type: 'number', decimals: 1, step: 0.5, min: 0 },
                { name: 'IELTSWriting', label: 'Writing Score', type: 'number', decimals: 1, step: 0.5, min: 0 },
                { name: 'IELTSSpeaking', label: 'Speaking Score', type: 'number', decimals: 1, step: 0.5, min: 0 },
                { name: 'IELTSOverallScore', label: 'Overall Score', type: 'number', decimals: 1, step: 0.5, min: 0 },
                { name: 'IELTSDate', label: 'Exam Date', type: 'date' },
                { name: 'IELTSScannedCopyPath', label: 'Document Path', type: 'text', maxLength: 255, wide: true, placeholder: 'Enter scanned copy path' },
                { name: 'IELTSAdditionalInformation', label: 'Additional Information', type: 'textarea', maxLength: 255, wide: true, placeholder: 'Enter additional information' }
        
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
                    IELTSInformationId: toInteger(values.IELTSInformationId),
                    ApplicantId: toInteger(values.ApplicantId),
                    Ieltslistening: toDecimal(values.IELTSListening),
                    Ieltsreading: toDecimal(values.IELTSReading),
                    Ieltswriting: toDecimal(values.IELTSWriting),
                    Ieltsspeaking: toDecimal(values.IELTSSpeaking),
                    IeltsoverallScore: toDecimal(values.IELTSOverallScore),
                    Ieltsdate: values.IELTSDate || null,
                    IeltsscannedCopyPath: normalizeString(values.IELTSScannedCopyPath),
                    IeltsadditionalInformation: normalizeString(values.IELTSAdditionalInformation),
                    createdDate: state.currentRecord?.CreatedDate || now,
                    createdBy: state.currentRecord?.CreatedBy || 1,
                    updatedDate: state.isEditMode ? now : null,
                    updatedBy: state.isEditMode ? 1 : null
                };
        
            }
        }
    };

    window.IeltsInformationModule.config.moduleRef = window.IeltsInformationModule;
    window.IeltsInformationModule.config.form.fields.forEach(function (field) {
        field.moduleRef = window.IeltsInformationModule;
    });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.IeltsInformationModule.Summary?.init?.();
        window.IeltsInformationModule.Details?.init?.();

        $(window.IeltsInformationModule.config.dom.addButton).on('click', function () {
            window.IeltsInformationModule.Details?.openAddForm?.();
        });

        $(window.IeltsInformationModule.config.dom.refreshButton).on('click', function () {
            window.IeltsInformationModule.Summary?.refreshGrid?.();
        });
    });
})();
