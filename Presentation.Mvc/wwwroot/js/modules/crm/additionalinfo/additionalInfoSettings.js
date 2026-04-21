(function () {
    'use strict';

    window.AdditionalInfoModule = window.AdditionalInfoModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.AdditionalInfoModule.config = {
        moduleTitle: 'Additional Info',
        pluralTitle: 'Additional Info Records',
        idField: 'AdditionalInfoId',
        displayField: 'AdditionalInformationRemarks',
        dom: {
            grid: '#additionalInfoGrid',
            window: '#additionalInfoWindow',
            form: '#additionalInfoForm',
            addButton: '#btnAddAdditionalInfo',
            refreshButton: '#btnRefreshAdditionalInfo',
            saveButton: '#btnSaveAdditionalInfo',
            cancelButton: '#btnCancelAdditionalInfo'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-additional-info-summary`,
            create: `${apiRoot}/crm-additional-info`,
            update: function (id) { return `${apiRoot}/crm-additional-info/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-additional-info/${id}`; },
            read: function (id) { return `${apiRoot}/crm-additional-info/${id}`; },
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
                { field: 'AdditionalInfoId', title: 'ID', width: 90, dataType: 'number', filterable: false },
                { field: 'ApplicantId', title: 'Applicant ID', width: 120, dataType: 'number' },
                { field: 'RequireAccommodation', title: 'Accommodation', width: 150, kind: 'boolean', trueText: 'Required', falseText: 'Not Required' },
                { field: 'HealthNMedicalNeeds', title: 'Medical Needs', width: 150, kind: 'boolean', trueText: 'Yes', falseText: 'No' },
                { field: 'AdditionalInformationRemarks', title: 'Remarks', width: 300, kind: 'multiline' }
            ]
        },
        form: {
            fields: [

                { name: 'AdditionalInfoId', label: 'Additional Info Id', type: 'hidden' },
                {
                    name: 'ApplicantId',
                    label: 'Applicant',
                    type: 'select',
                    required: true,
                    dataSourceEndpoint: function () { return window.AdditionalInfoModule.config.apiEndpoints.applicants; },
                    dataTextField: 'applicantName',
                    dataValueField: 'applicantId',
                    optionLabel: 'Select Applicant...'
                },
                {
                    name: 'RequireAccommodation',
                    label: 'Require Accommodation',
                    type: 'checkbox',
                    wide: true,
                    defaultValue: false
                },
                {
                    name: 'HealthNMedicalNeeds',
                    label: 'Health And Medical Needs',
                    type: 'checkbox',
                    wide: true,
                    defaultValue: false
                },
                { name: 'HealthNMedicalNeedsRemarks', label: 'Medical Needs Remarks', type: 'textarea', maxLength: 255, wide: true, placeholder: 'Enter health and medical needs remarks' },
                { name: 'AdditionalInformationRemarks', label: 'Additional Information Remarks', type: 'textarea', maxLength: 255, wide: true, placeholder: 'Enter additional information remarks' }
        
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
                    AdditionalInfoId: toInteger(values.AdditionalInfoId),
                    ApplicantId: toInteger(values.ApplicantId),
                    RequireAccommodation: !!values.RequireAccommodation,
                    HealthNmedicalNeeds: !!values.HealthNMedicalNeeds,
                    HealthNmedicalNeedsRemarks: normalizeString(values.HealthNMedicalNeedsRemarks),
                    AdditionalInformationRemarks: normalizeString(values.AdditionalInformationRemarks),
                    createdDate: state.currentRecord?.CreatedDate || now,
                    createdBy: state.currentRecord?.CreatedBy || 1,
                    updatedDate: state.isEditMode ? now : null,
                    updatedBy: state.isEditMode ? 1 : null
                };
        
            }
        }
    };

    window.AdditionalInfoModule.config.moduleRef = window.AdditionalInfoModule;
    window.AdditionalInfoModule.config.form.fields.forEach(function (field) {
        field.moduleRef = window.AdditionalInfoModule;
    });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.AdditionalInfoModule.Summary?.init?.();
        window.AdditionalInfoModule.Details?.init?.();

        $(window.AdditionalInfoModule.config.dom.addButton).on('click', function () {
            window.AdditionalInfoModule.Details?.openAddForm?.();
        });

        $(window.AdditionalInfoModule.config.dom.refreshButton).on('click', function () {
            window.AdditionalInfoModule.Summary?.refreshGrid?.();
        });
    });
})();
