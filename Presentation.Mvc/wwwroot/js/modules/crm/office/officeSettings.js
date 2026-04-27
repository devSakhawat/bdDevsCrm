(function () {
    'use strict';

    window.OfficeModule = window.OfficeModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.OfficeModule.config = {
        moduleTitle: 'Office',
        pluralTitle: 'Offices',
        idField: 'officeId',
        displayField: 'officeName',
        dom: {
            grid: '#officeGrid',
            window: '#officeWindow',
            form: '#officeForm',
            addButton: '#btnAddOffice',
            refreshButton: '#btnRefreshOffice',
            saveButton: '#btnSaveOffice',
            cancelButton: '#btnCancelOffice'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-office-summary`,
            create: `${apiRoot}/crm-office`,
            update: function (id) { return `${apiRoot}/crm-office/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-office/${id}`; },
            read: function (id) { return `${apiRoot}/crm-office/${id}`; }
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
            width: '760px'
        },
        grid: {
            columns: [{field: "officeId", title: "#", width: 60, dataType: "number", filterable: false}, {field: "officeName", title: "Office Name", width: 200}, {field: "officeCode", title: "Code", width: 130}, {field: "city", title: "City", width: 150}, {field: "phone", title: "Phone", width: 130}, {field: "email", title: "Email", width: 200}, {field: "isActive", title: "Status", width: 100, dataType: "boolean", kind: "boolean", trueText: "Active", falseText: "Inactive"}]
        },
        form: {
            fields: [{name: "officeId", label: "Office Id", type: "hidden"}, {name: "officeName", label: "Office Name", type: "text", required: true, maxLength: 150, placeholder: "Enter office name"}, {name: "officeCode", label: "Office Code", type: "text", maxLength: 50, placeholder: "Enter office code"}, {name: "address", label: "Address", type: "textarea", maxLength: 255, wide: true, placeholder: "Enter address"}, {name: "city", label: "City", type: "text", maxLength: 100, placeholder: "Enter city"}, {name: "phone", label: "Phone", type: "text", maxLength: 50, placeholder: "Enter phone"}, {name: "email", label: "Email", type: "text", maxLength: 150, placeholder: "Enter email"}, {name: "isActive", label: "Active", type: "checkbox", defaultValue: true}],
            buildPayload: function (values, state) {
                function normalizeString(value) {
                    return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim();
                }

                function toInteger(value) {
                    const parsed = parseInt(value || 0, 10);
                    return Number.isNaN(parsed) ? 0 : parsed;
                }

                const now = new Date().toISOString();
                return {
                    officeId: toInteger(values.officeId),
                    officeName: (values.officeName || '').trim(),
                    officeCode: normalizeString(values.officeCode),
                    address: normalizeString(values.address),
                    city: normalizeString(values.city),
                    phone: normalizeString(values.phone),
                    email: normalizeString(values.email),
                    isActive: values.isActive !== false,
                    createdDate: state.currentRecord?.createdDate || now,
                    createdBy: state.currentRecord?.createdBy || 1,
                    updatedDate: state.isEditMode ? now : null,
                    updatedBy: state.isEditMode ? 1 : null
                };
            }
        }
    };

    window.OfficeModule.config.moduleRef = window.OfficeModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.OfficeModule.Summary?.init?.();
        window.OfficeModule.Details?.init?.();

        $(window.OfficeModule.config.dom.addButton).on('click', function () {
            window.OfficeModule.Details?.openAddForm?.();
        });

        $(window.OfficeModule.config.dom.refreshButton).on('click', function () {
            window.OfficeModule.Summary?.refreshGrid?.();
        });
    });
})();
