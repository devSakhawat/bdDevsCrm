(function () {
    'use strict';

    window.CurrencyModule = window.CurrencyModule || {};

    window.CurrencyModule.config = {
        moduleTitle: 'Currency',
        pluralTitle: 'Currencies',
        idField: 'currencyId',
        displayField: 'currencyName',
        dom: {
            grid: '#currencyGrid',
            window: '#currencyWindow',
            form: '#currencyForm',
            addButton: '#btnAddCurrency',
            refreshButton: '#btnRefreshCurrency',
            saveButton: '#btnSaveCurrency',
            cancelButton: '#btnCancelCurrency'
        },
        apiEndpoints: {
            summary: `${window.AppConfig.apiBaseUrl}/core/systemadmin/currency-summary`,
            create: `${window.AppConfig.apiBaseUrl}/core/systemadmin/currency`,
            update: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/currency/${id}`; },
            delete: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/currency/${id}`; },
            read: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/currency/${id}`; }
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
            columns: [{field: 'currencyId', title: 'ID', width: 90, dataType: 'number', filterable: false}, {field: 'currencyName', title: 'Currency Name', width: 240}, {field: 'isDefault', title: 'Default', width: 120, kind: 'boolean', dataType: 'boolean', trueText: 'Default', falseText: 'No'}, {field: 'isActive', title: 'Status', width: 120, kind: 'boolean', dataType: 'boolean', trueText: 'Active', falseText: 'Inactive'}]
        },
        form: {
            fields: [{name: 'currencyId', label: 'Currency Id', type: 'hidden'}, {name: 'currencyName', label: 'Currency Name', type: 'text', required: true, maxLength: 100, wide: true, placeholder: 'Enter currency name'}, {name: 'isDefault', label: 'Default Currency', type: 'checkbox', defaultValue: false}, {name: 'isActive', label: 'Active', type: 'checkbox', defaultValue: true}],
            buildPayload: function (values) {
                function normalizeString(value) {
                    return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim();
                }

                function toInteger(value) {
                    const parsed = parseInt(value || 0, 10);
                    return Number.isNaN(parsed) ? 0 : parsed;
                }

                return {
                    currencyId: toInteger(values.currencyId),
                    currencyName: normalizeString(values.currencyName),
                    isDefault: values.isDefault ? 1 : 0,
                    isActive: values.isActive !== false ? 1 : 0
                };
            }
        }
    };

    window.CurrencyModule.config.moduleRef = window.CurrencyModule;
    window.CurrencyModule.config.form.fields.forEach(function (field) {
        field.moduleRef = window.CurrencyModule;
    });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.CurrencyModule.Summary?.init?.();
        window.CurrencyModule.Details?.init?.();

        $(window.CurrencyModule.config.dom.addButton).on('click', function () {
            window.CurrencyModule.Details?.openAddForm?.();
        });

        $(window.CurrencyModule.config.dom.refreshButton).on('click', function () {
            window.CurrencyModule.Summary?.refreshGrid?.();
        });
    });
})();
