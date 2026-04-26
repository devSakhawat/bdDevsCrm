(function () {
    'use strict';

    window.ApplicantReferenceModule = window.ApplicantReferenceModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.ApplicantReferenceModule.config = {
        moduleTitle: 'Applicant Reference',
        pluralTitle: 'Applicant References',
        idField: 'applicantReferenceId',
        displayField: 'name',
        dom: {
            grid: '#applicantReferenceGrid',
            window: '#applicantReferenceWindow',
            form: '#applicantReferenceForm',
            addButton: '#btnAddApplicantReference',
            refreshButton: '#btnRefreshApplicantReference',
            saveButton: '#btnSaveApplicantReference',
            cancelButton: '#btnCancelApplicantReference'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-applicant-reference-summary`,
            create: `${apiRoot}/crm-applicant-reference`,
            update: function (id) { return `${apiRoot}/crm-applicant-reference/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-applicant-reference/${id}`; },
            read: function (id) { return `${apiRoot}/crm-applicant-reference/${id}`; },

            applicants: `${apiRoot}/crm-applicant-infos-ddl`,
            countries: `${apiRoot}/countryddl`
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
                { field: 'applicantReferenceId', title: 'ID', width: 90, dataType: 'number', filterable: false },
                { field: 'applicantId', title: 'Applicant ID', width: 120, dataType: 'number' },
                { field: 'name', title: 'Name', width: 180 },
                { field: 'designation', title: 'Designation', width: 160 },
                { field: 'institution', title: 'Institution', width: 180 },
                { field: 'emailID', title: 'Email', width: 200 },
                { field: 'phoneNo', title: 'Phone', width: 140 }
            ]
        },
        form: {
            fields: [

                { name: 'applicantReferenceId', label: 'Applicant Reference Id', type: 'hidden' },
                {
                    name: 'applicantId',
                    label: 'Applicant',
                    type: 'select',
                    required: true,
                    dataSourceEndpoint: function () { return window.ApplicantReferenceModule.config.apiEndpoints.applicants; },
                    dataTextField: 'applicantName',
                    dataValueField: 'applicantId',
                    optionLabel: 'Select Applicant...'
                },
                { name: 'name', label: 'Reference Name', type: 'text', required: true, maxLength: 100, placeholder: 'Enter reference name' },
                { name: 'designation', label: 'Designation', type: 'text', maxLength: 255, placeholder: 'Enter designation' },
                { name: 'institution', label: 'Institution', type: 'text', maxLength: 255, placeholder: 'Enter institution' },
                { name: 'emailID', label: 'Email', type: 'email', maxLength: 256, placeholder: 'Enter email address' },
                { name: 'phoneNo', label: 'Phone', type: 'text', maxLength: 20, placeholder: 'Enter phone number' },
                { name: 'faxNo', label: 'Fax', type: 'text', maxLength: 20, placeholder: 'Enter fax number' },
                {
                    name: 'address',
                    label: 'Address',
                    type: 'textarea',
                    maxLength: 500,
                    wide: true,
                    placeholder: 'Enter address'
                },
                { name: 'city', label: 'City', type: 'text', maxLength: 255, placeholder: 'Enter city' },
                { name: 'state', label: 'State', type: 'text', maxLength: 255, placeholder: 'Enter state' },
                {
                    name: 'countryId',
                    label: 'Country',
                    type: 'select',
                    dataSourceEndpoint: function () { return window.ApplicantReferenceModule.config.apiEndpoints.countries; },
                    dataTextField: 'countryName',
                    dataValueField: 'countryId',
                    optionLabel: 'Select Country...'
                },
                { name: 'postOrZipCode', label: 'Post / Zip Code', type: 'text', maxLength: 255, placeholder: 'Enter post or zip code' }
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
                    applicantReferenceId: toInteger(values.applicantReferenceId),
                    applicantId: toInteger(values.applicantId),
                    name: normalizeString(values.name),
                    designation: normalizeString(values.designation),
                    institution: normalizeString(values.institution),
                    emailId: normalizeString(values.emailID),
                    phoneNo: normalizeString(values.phoneNo),
                    faxNo: normalizeString(values.faxNo),
                    address: normalizeString(values.address),
                    city: normalizeString(values.city),
                    state: normalizeString(values.state),
                    countryId: values.countryId ? toInteger(values.countryId) : null,
                    postOrZipCode: normalizeString(values.postOrZipCode),
                    createdDate: state.currentRecord?.createdDate || now,
                    createdBy: state.currentRecord?.createdBy || 1,
                    updatedDate: state.isEditMode ? now : null,
                    updatedBy: state.isEditMode ? 1 : null
                };
            }
        }
    };

    window.ApplicantReferenceModule.config.moduleRef = window.ApplicantReferenceModule;
    window.ApplicantReferenceModule.config.form.fields.forEach(function (field) {
        field.moduleRef = window.ApplicantReferenceModule;
    });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.ApplicantReferenceModule.Summary?.init?.();
        window.ApplicantReferenceModule.Details?.init?.();

        $(window.ApplicantReferenceModule.config.dom.addButton).on('click', function () {
            window.ApplicantReferenceModule.Details?.openAddForm?.();
        });

        $(window.ApplicantReferenceModule.config.dom.refreshButton).on('click', function () {
            window.ApplicantReferenceModule.Summary?.refreshGrid?.();
        });
    });
})();
