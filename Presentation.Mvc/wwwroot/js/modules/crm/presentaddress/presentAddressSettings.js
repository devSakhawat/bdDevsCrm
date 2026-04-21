(function () {
    'use strict';

    window.PresentAddressModule = window.PresentAddressModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.PresentAddressModule.config = {
        moduleTitle: 'Present Address',
        pluralTitle: 'Present Addresses',
        idField: 'presentAddressId',
        displayField: 'address',
        dom: {
            grid: '#presentAddressGrid',
            window: '#presentAddressWindow',
            form: '#presentAddressForm',
            addButton: '#btnAddPresentAddress',
            refreshButton: '#btnRefreshPresentAddress',
            saveButton: '#btnSavePresentAddress',
            cancelButton: '#btnCancelPresentAddress'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-present-address-summary`,
            create: `${apiRoot}/crm-present-address`,
            update: function (id) { return `${apiRoot}/crm-present-address/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-present-address/${id}`; },
            read: function (id) { return `${apiRoot}/crm-present-address/${id}`; },

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
            width: '840px'
        },
        grid: {
            columns: [
                { field: 'presentAddressId', title: 'ID', width: 90, dataType: 'number', filterable: false },
                { field: 'applicantId', title: 'Applicant ID', width: 120, dataType: 'number' },
                { field: 'sameAsPermanentAddress', title: 'Same As Permanent', width: 160, filterable: false, kind: 'boolean' },
                { field: 'address', title: 'Address', width: 220, kind: 'multiline' },
                { field: 'city', title: 'City', width: 140 },
                { field: 'countryName', title: 'Country', width: 160 },
                { field: 'postalCode', title: 'Postal Code', width: 130 }
            ]
        },
        form: {
            fields: [

                { name: 'presentAddressId', label: 'Present Address Id', type: 'hidden' },
                {
                    name: 'applicantId',
                    label: 'Applicant',
                    type: 'select',
                    required: true,
                    dataSourceEndpoint: function () { return window.PresentAddressModule.config.apiEndpoints.applicants; },
                    dataTextField: 'applicantName',
                    dataValueField: 'applicantId',
                    optionLabel: 'Select Applicant...'
                },
                {
                    name: 'sameAsPermanentAddress',
                    label: 'Same as permanent address',
                    type: 'checkbox',
                    wide: true,
                    defaultValue: false
                },
                {
                    name: 'address',
                    label: 'Address',
                    type: 'textarea',
                    required: true,
                    maxLength: 500,
                    wide: true,
                    placeholder: 'Enter present address'
                },
                { name: 'city', label: 'City', type: 'text', maxLength: 255, placeholder: 'Enter city' },
                { name: 'state', label: 'State', type: 'text', maxLength: 255, placeholder: 'Enter state' },
                {
                    name: 'countryId',
                    label: 'Country',
                    type: 'select',
                    required: true,
                    dataSourceEndpoint: function () { return window.PresentAddressModule.config.apiEndpoints.countries; },
                    dataTextField: 'countryName',
                    dataValueField: 'countryId',
                    optionLabel: 'Select Country...'
                },
                { name: 'postalCode', label: 'Postal Code', type: 'text', maxLength: 255, placeholder: 'Enter postal code' }
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
                    presentAddressId: toInteger(values.presentAddressId),
                    applicantId: toInteger(values.applicantId),
                    sameAsPermanentAddress: !!values.sameAsPermanentAddress,
                    address: normalizeString(values.address),
                    city: normalizeString(values.city),
                    state: normalizeString(values.state),
                    countryId: toInteger(values.countryId),
                    postalCode: normalizeString(values.postalCode),
                    createdDate: state.currentRecord?.createdDate || now,
                    createdBy: state.currentRecord?.createdBy || 1,
                    updatedDate: state.isEditMode ? now : null,
                    updatedBy: state.isEditMode ? 1 : null
                };
            }
        }
    };

    window.PresentAddressModule.config.moduleRef = window.PresentAddressModule;
    window.PresentAddressModule.config.form.fields.forEach(function (field) {
        field.moduleRef = window.PresentAddressModule;
    });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.PresentAddressModule.Summary?.init?.();
        window.PresentAddressModule.Details?.init?.();

        $(window.PresentAddressModule.config.dom.addButton).on('click', function () {
            window.PresentAddressModule.Details?.openAddForm?.();
        });

        $(window.PresentAddressModule.config.dom.refreshButton).on('click', function () {
            window.PresentAddressModule.Summary?.refreshGrid?.();
        });
    });
})();
