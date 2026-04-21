(function () {
    'use strict';

    window.StatementOfPurposeModule = window.StatementOfPurposeModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.StatementOfPurposeModule.config = {
        moduleTitle: 'Statement Of Purpose',
        pluralTitle: 'Statement Of Purpose Records',
        idField: 'StatementOfPurposeId',
        displayField: 'StatementOfPurposeRemarks',
        dom: {
            grid: '#statementOfPurposeGrid',
            window: '#statementOfPurposeWindow',
            form: '#statementOfPurposeForm',
            addButton: '#btnAddStatementOfPurpose',
            refreshButton: '#btnRefreshStatementOfPurpose',
            saveButton: '#btnSaveStatementOfPurpose',
            cancelButton: '#btnCancelStatementOfPurpose'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-statement-of-purpose-summary`,
            create: `${apiRoot}/crm-statement-of-purpose`,
            update: function (id) { return `${apiRoot}/crm-statement-of-purpose/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-statement-of-purpose/${id}`; },
            read: function (id) { return `${apiRoot}/crm-statement-of-purpose/${id}`; },
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
                { field: 'StatementOfPurposeId', title: 'ID', width: 90, dataType: 'number', filterable: false },
                { field: 'ApplicantId', title: 'Applicant ID', width: 120, dataType: 'number' },
                { field: 'StatementOfPurposeRemarks', title: 'Remarks', width: 320, kind: 'multiline' },
                { field: 'StatementOfPurposeFilePath', title: 'File Path', width: 220, kind: 'multiline' }
            ]
        },
        form: {
            fields: [

                { name: 'StatementOfPurposeId', label: 'Statement Of Purpose Id', type: 'hidden' },
                {
                    name: 'ApplicantId',
                    label: 'Applicant',
                    type: 'select',
                    required: true,
                    dataSourceEndpoint: function () { return window.StatementOfPurposeModule.config.apiEndpoints.applicants; },
                    dataTextField: 'applicantName',
                    dataValueField: 'applicantId',
                    optionLabel: 'Select Applicant...'
                },
                { name: 'StatementOfPurposeRemarks', label: 'Remarks', type: 'textarea', maxLength: 255, wide: true, placeholder: 'Enter statement of purpose remarks' },
                { name: 'StatementOfPurposeFilePath', label: 'File Path', type: 'text', maxLength: 255, wide: true, placeholder: 'Enter statement of purpose file path' }
        
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
                    StatementOfPurposeId: toInteger(values.StatementOfPurposeId),
                    ApplicantId: toInteger(values.ApplicantId),
                    StatementOfPurposeRemarks: normalizeString(values.StatementOfPurposeRemarks),
                    StatementOfPurposeFilePath: normalizeString(values.StatementOfPurposeFilePath),
                    createdDate: state.currentRecord?.CreatedDate || now,
                    createdBy: state.currentRecord?.CreatedBy || 1,
                    updatedDate: state.isEditMode ? now : null,
                    updatedBy: state.isEditMode ? 1 : null
                };
        
            }
        }
    };

    window.StatementOfPurposeModule.config.moduleRef = window.StatementOfPurposeModule;
    window.StatementOfPurposeModule.config.form.fields.forEach(function (field) {
        field.moduleRef = window.StatementOfPurposeModule;
    });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.StatementOfPurposeModule.Summary?.init?.();
        window.StatementOfPurposeModule.Details?.init?.();

        $(window.StatementOfPurposeModule.config.dom.addButton).on('click', function () {
            window.StatementOfPurposeModule.Details?.openAddForm?.();
        });

        $(window.StatementOfPurposeModule.config.dom.refreshButton).on('click', function () {
            window.StatementOfPurposeModule.Summary?.refreshGrid?.();
        });
    });
})();
