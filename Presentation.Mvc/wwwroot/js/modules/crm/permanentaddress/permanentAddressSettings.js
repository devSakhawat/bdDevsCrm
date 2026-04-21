(function () {
    'use strict';

    window.PermanentAddressModule = window.PermanentAddressModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.PermanentAddressModule.config = {
        moduleTitle: 'Permanent Address',
        pluralTitle: 'Permanent Addresses',
        idField: 'permanentAddressId',
        displayField: 'address',
        dom: {
            grid: '#permanentAddressGrid',
            window: '#permanentAddressWindow',
            form: '#permanentAddressForm',
            addButton: '#btnAddPermanentAddress',
            refreshButton: '#btnRefreshPermanentAddress',
            saveButton: '#btnSavePermanentAddress',
            cancelButton: '#btnCancelPermanentAddress'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-permanent-address-summary`,
            create: `${apiRoot}/crm-permanent-address`,
            update: function (id) { return `${apiRoot}/crm-permanent-address/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-permanent-address/${id}`; },
            read: function (id) { return `${apiRoot}/crm-permanent-address/${id}`; },

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
                { field: 'permanentAddressId', title: 'ID', width: 90, dataType: 'number', filterable: false },
                { field: 'applicantId', title: 'Applicant ID', width: 120, dataType: 'number' },
                { field: 'address', title: 'Address', width: 240, kind: 'multiline' },
                { field: 'city', title: 'City', width: 140 },
                { field: 'countryName', title: 'Country', width: 160 },
                { field: 'postalCode', title: 'Postal Code', width: 130 }
            ]
        },
        form: {
            fields: [

                { name: 'permanentAddressId', label: 'Permanent Address Id', type: 'hidden' },
                {
                    name: 'applicantId',
                    label: 'Applicant',
                    type: 'select',
                    required: true,
                    dataSourceEndpoint: function () { return window.PermanentAddressModule.config.apiEndpoints.applicants; },
                    dataTextField: 'applicantName',
                    dataValueField: 'applicantId',
                    optionLabel: 'Select Applicant...'
                },
                {
                    name: 'address',
                    label: 'Address',
                    type: 'textarea',
                    required: true,
                    maxLength: 500,
                    wide: true,
                    placeholder: 'Enter permanent address'
                },
                { name: 'city', label: 'City', type: 'text', maxLength: 255, placeholder: 'Enter city' },
                { name: 'state', label: 'State', type: 'text', maxLength: 255, placeholder: 'Enter state' },
                {
                    name: 'countryId',
                    label: 'Country',
                    type: 'select',
                    required: true,
                    dataSourceEndpoint: function () { return window.PermanentAddressModule.config.apiEndpoints.countries; },
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
                    permanentAddressId: toInteger(values.permanentAddressId),
                    applicantId: toInteger(values.applicantId),
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

    window.PermanentAddressModule.config.moduleRef = window.PermanentAddressModule;
    window.PermanentAddressModule.config.form.fields.forEach(function (field) {
        field.moduleRef = window.PermanentAddressModule;
    });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.PermanentAddressModule.Summary?.init?.();
        window.PermanentAddressModule.Details?.init?.();

        $(window.PermanentAddressModule.config.dom.addButton).on('click', function () {
            window.PermanentAddressModule.Details?.openAddForm?.();
        });

        $(window.PermanentAddressModule.config.dom.refreshButton).on('click', function () {
            window.PermanentAddressModule.Summary?.refreshGrid?.();
        });
    });
})();
