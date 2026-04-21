(function () {
    'use strict';

    window.OthersInformationModule = window.OthersInformationModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.OthersInformationModule.config = {
        moduleTitle: 'Others Information',
        pluralTitle: 'Others Information Records',
        idField: 'OthersInformationId',
        displayField: 'AdditionalInformation',
        dom: {
            grid: '#othersInformationGrid',
            window: '#othersInformationWindow',
            form: '#othersInformationForm',
            addButton: '#btnAddOthersInformation',
            refreshButton: '#btnRefreshOthersInformation',
            saveButton: '#btnSaveOthersInformation',
            cancelButton: '#btnCancelOthersInformation'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-others-information-summary`,
            create: `${apiRoot}/crm-others-information`,
            update: function (id) { return `${apiRoot}/crm-others-information/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-others-information/${id}`; },
            read: function (id) { return `${apiRoot}/crm-others-information/${id}`; },
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
                { field: 'OthersInformationId', title: 'ID', width: 90, dataType: 'number', filterable: false },
                { field: 'ApplicantId', title: 'Applicant ID', width: 120, dataType: 'number' },
                { field: 'AdditionalInformation', title: 'Additional Information', width: 300, kind: 'multiline' },
                { field: 'OTHERSScannedCopyPath', title: 'Document Path', width: 220, kind: 'multiline' }
            ]
        },
        form: {
            fields: [

                { name: 'OthersInformationId', label: 'Others Information Id', type: 'hidden' },
                {
                    name: 'ApplicantId',
                    label: 'Applicant',
                    type: 'select',
                    required: true,
                    dataSourceEndpoint: function () { return window.OthersInformationModule.config.apiEndpoints.applicants; },
                    dataTextField: 'applicantName',
                    dataValueField: 'applicantId',
                    optionLabel: 'Select Applicant...'
                },
                { name: 'AdditionalInformation', label: 'Additional Information', type: 'textarea', maxLength: 255, wide: true, placeholder: 'Enter additional information' },
                { name: 'OTHERSScannedCopyPath', label: 'Document Path', type: 'text', maxLength: 255, wide: true, placeholder: 'Enter scanned copy path' }
        
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
                    OthersInformationId: toInteger(values.OthersInformationId),
                    ApplicantId: toInteger(values.ApplicantId),
                    AdditionalInformation: normalizeString(values.AdditionalInformation),
                    OthersScannedCopyPath: normalizeString(values.OTHERSScannedCopyPath),
                    createdDate: state.currentRecord?.CreatedDate || now,
                    createdBy: state.currentRecord?.CreatedBy || 1,
                    updatedDate: state.isEditMode ? now : null,
                    updatedBy: state.isEditMode ? 1 : null
                };
        
            }
        }
    };

    window.OthersInformationModule.config.moduleRef = window.OthersInformationModule;
    window.OthersInformationModule.config.form.fields.forEach(function (field) {
        field.moduleRef = window.OthersInformationModule;
    });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.OthersInformationModule.Summary?.init?.();
        window.OthersInformationModule.Details?.init?.();

        $(window.OthersInformationModule.config.dom.addButton).on('click', function () {
            window.OthersInformationModule.Details?.openAddForm?.();
        });

        $(window.OthersInformationModule.config.dom.refreshButton).on('click', function () {
            window.OthersInformationModule.Summary?.refreshGrid?.();
        });
    });
})();
