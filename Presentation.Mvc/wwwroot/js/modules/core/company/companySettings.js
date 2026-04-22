(function () {
    'use strict';

    window.CompanyModule = window.CompanyModule || {};

    window.CompanyModule.config = {
        moduleTitle: 'Company',
        pluralTitle: 'Companies',
        idField: 'companyId',
        displayField: 'companyName',
        dom: {
            grid: '#companyGrid',
            window: '#companyWindow',
            form: '#companyForm',
            addButton: '#btnAddCompany',
            refreshButton: '#btnRefreshCompany',
            saveButton: '#btnSaveCompany',
            cancelButton: '#btnCancelCompany'
        },
        apiEndpoints: {
            list: `${window.AppConfig.apiBaseUrl}/core/systemadmin/companies`,
            ddl: `${window.AppConfig.apiBaseUrl}/core/systemadmin/companies-ddl`,
            create: `${window.AppConfig.apiBaseUrl}/core/systemadmin/company`,
            update: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/company/${id}`; },
            delete: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/company/${id}`; },
            read: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/company/key/${id}`; }
        },
        gridOptions: {
            pageSize: 20,
            sortable: true,
            filterable: true,
            height: 560,
            serverOperations: false,
            pageable: { refresh: true, pageSizes: [10, 20, 50, 100], buttonCount: 5 }
        },
        windowOptions: { width: '960px' },
        grid: {
            columns: [
                { field: 'companyId', title: 'ID', width: 90, dataType: 'number', filterable: false },
                { field: 'companyCode', title: 'Code', width: 120 },
                { field: 'companyName', title: 'Company Name', width: 220 },
                { field: 'companyAlias', title: 'Alias', width: 160 },
                { field: 'email', title: 'Email', width: 200 },
                { field: 'phone', title: 'Phone', width: 150 },
                { field: 'isActive', title: 'Status', width: 110, kind: 'boolean', dataType: 'boolean', trueText: 'Active', falseText: 'Inactive' }
            ]
        },
        form: {
            fields: [
                { name: 'companyId', label: 'Company Id', type: 'hidden' },
                { name: 'companyCode', label: 'Company Code', type: 'text', required: true, maxLength: 50, placeholder: 'Enter company code' },
                { name: 'companyName', label: 'Company Name', type: 'text', required: true, maxLength: 200, placeholder: 'Enter company name', wide: true },
                { name: 'companyAlias', label: 'Company Alias', type: 'text', maxLength: 150, placeholder: 'Enter company alias' },
                { name: 'primaryContact', label: 'Primary Contact', type: 'text', maxLength: 150, placeholder: 'Enter primary contact' },
                { name: 'email', label: 'Email', type: 'email', maxLength: 150, placeholder: 'Enter email address' },
                { name: 'phone', label: 'Phone', type: 'text', maxLength: 50, placeholder: 'Enter phone number' },
                { name: 'fax', label: 'Fax', type: 'text', maxLength: 50, placeholder: 'Enter fax number' },
                { name: 'address', label: 'Address', type: 'textarea', wide: true, maxLength: 1000, placeholder: 'Enter address' },
                { name: 'motherId', label: 'Mother Company', type: 'select', dataSourceEndpoint: function () { return window.CompanyModule.config.apiEndpoints.ddl; }, dataTextField: 'companyName', dataValueField: 'companyId', optionLabel: 'Select mother company...' },
                { name: 'fiscalYearStart', label: 'Fiscal Year Start', type: 'number', min: 0, placeholder: 'Enter fiscal year start' },
                { name: 'flag', label: 'Flag', type: 'number', min: 0, placeholder: 'Enter flag' },
                { name: 'companyTin', label: 'Company TIN', type: 'text', maxLength: 100, placeholder: 'Enter company TIN' },
                { name: 'companyRegisterNo', label: 'Register No', type: 'text', maxLength: 100, placeholder: 'Enter registration number' },
                { name: 'companyZone', label: 'Zone', type: 'text', maxLength: 150, placeholder: 'Enter zone' },
                { name: 'companyCircle', label: 'Circle', type: 'text', maxLength: 150, placeholder: 'Enter circle' },
                { name: 'companySortOrder', label: 'Sort Order', type: 'number', min: 0, placeholder: 'Enter sort order' },
                { name: 'gratuityStartDate', label: 'Gratuity Start Date', type: 'date' },
                { name: 'isCostCentre', label: 'Cost Centre Enabled', type: 'checkbox', defaultValue: false },
                { name: 'isActive', label: 'Active', type: 'checkbox', defaultValue: true },
                { name: 'isPfApplicable', label: 'PF Applicable', type: 'checkbox', defaultValue: false },
                { name: 'isEwfApplicable', label: 'EWF Applicable', type: 'checkbox', defaultValue: false }
            ],
            buildPayload: function (values) {
                function toInteger(value) { const parsed = parseInt(value || 0, 10); return Number.isNaN(parsed) ? 0 : parsed; }
                function toNullableInteger(value) { if (value === null || value === undefined || value === '') { return null; } const parsed = parseInt(value, 10); return Number.isNaN(parsed) ? null : parsed; }
                function normalizeString(value) { return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim(); }
                return {
                    companyId: toInteger(values.companyId),
                    companyCode: normalizeString(values.companyCode),
                    companyName: normalizeString(values.companyName),
                    address: normalizeString(values.address),
                    phone: normalizeString(values.phone),
                    fax: normalizeString(values.fax),
                    email: normalizeString(values.email),
                    primaryContact: normalizeString(values.primaryContact),
                    flag: toInteger(values.flag),
                    fiscalYearStart: toInteger(values.fiscalYearStart),
                    motherId: toNullableInteger(values.motherId),
                    isCostCentre: values.isCostCentre ? 1 : 0,
                    isActive: values.isActive ? 1 : 0,
                    gratuityStartDate: values.gratuityStartDate || null,
                    companyTin: normalizeString(values.companyTin),
                    isPfApplicable: !!values.isPfApplicable,
                    isEwfApplicable: !!values.isEwfApplicable,
                    companyAlias: normalizeString(values.companyAlias),
                    companyZone: normalizeString(values.companyZone),
                    companyCircle: normalizeString(values.companyCircle),
                    companyRegisterNo: normalizeString(values.companyRegisterNo),
                    companySortOrder: toNullableInteger(values.companySortOrder)
                };
            }
        }
    };

    window.CompanyModule.config.moduleRef = window.CompanyModule;
    window.CompanyModule.config.form.fields.forEach(function (field) { field.moduleRef = window.CompanyModule; });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () { window.location.href = '/Account/Login'; }, 1500);
            return;
        }

        window.CompanyModule.Summary?.init?.();
        window.CompanyModule.Details?.init?.();

        $(window.CompanyModule.config.dom.addButton).on('click', function () { window.CompanyModule.Details?.openAddForm?.(); });
        $(window.CompanyModule.config.dom.refreshButton).on('click', function () { window.CompanyModule.Summary?.refreshGrid?.(); });
    });
})();
