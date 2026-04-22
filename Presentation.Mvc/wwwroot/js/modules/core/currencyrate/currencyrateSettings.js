(function () {
    'use strict';

    window.CurrencyRateModule = window.CurrencyRateModule || {};

    window.CurrencyRateModule.config = {
        moduleTitle: 'Currency Rate',
        pluralTitle: 'Currency Rates',
        idField: 'curencyRateId',
        displayField: 'currencyRateRation',
        dom: {
            grid: '#currencyRateGrid',
            window: '#currencyRateWindow',
            form: '#currencyRateForm',
            addButton: '#btnAddCurrencyRate',
            refreshButton: '#btnRefreshCurrencyRate',
            saveButton: '#btnSaveCurrencyRate',
            cancelButton: '#btnCancelCurrencyRate'
        },
        apiEndpoints: {
            summary: `${window.AppConfig.apiBaseUrl}/core/systemadmin/currency-rate-summary`,
            create: `${window.AppConfig.apiBaseUrl}/core/systemadmin/currency-rate`,
            update: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/currency-rate/${id}`; },
            delete: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/currency-rate/${id}`; },
            read: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/currency-rate/${id}`; },
            currencies: `${window.AppConfig.apiBaseUrl}/core/systemadmin/currencies-ddl`
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
            columns: [{field: 'curencyRateId', title: 'ID', width: 90, dataType: 'number', filterable: false}, {field: 'currencyId', title: 'Currency', width: 120, dataType: 'number'}, {field: 'currencyRateRation', title: 'Rate', width: 140, kind: 'currency', dataType: 'number'}, {field: 'currencyMonth', title: 'Rate Month', width: 170}, {field: 'createdBy', title: 'Created By', width: 120, dataType: 'number'}]
        },
        form: {
            fields: [{name: 'curencyRateId', label: 'Currency Rate Id', type: 'hidden'}, {name: 'currencyId', label: 'Currency', type: 'select', required: true, dataSourceEndpoint: function () { return window.CurrencyRateModule.config.apiEndpoints.currencies; }, dataTextField: 'currencyName', dataValueField: 'currencyId', optionLabel: 'Select Currency...'}, {name: 'currencyRateRation', label: 'Currency Rate', type: 'number', decimals: 4, step: 0.0001, min: 0, placeholder: 'Enter rate'}, {name: 'currencyMonth', label: 'Rate Month', type: 'date', required: true}, {name: 'createdBy', label: 'Created By', type: 'number', min: 0, placeholder: 'Enter creator id'}],
            buildPayload: function (values, state) {
                function toInteger(value) {
                    const parsed = parseInt(value || 0, 10);
                    return Number.isNaN(parsed) ? 0 : parsed;
                }

                function toNullableInteger(value) {
                    if (value === null || value === undefined || value === '') {
                        return null;
                    }
                    const parsed = parseInt(value, 10);
                    return Number.isNaN(parsed) ? null : parsed;
                }

                function toNullableNumber(value) {
                    if (value === null || value === undefined || value === '') {
                        return null;
                    }
                    const parsed = Number(value);
                    return Number.isNaN(parsed) ? null : parsed;
                }

                const payload = {
                    curencyRateId: toInteger(values.curencyRateId),
                    currencyId: toInteger(values.currencyId),
                    currencyRateRation: toNullableNumber(values.currencyRateRation),
                    currencyMonth: values.currencyMonth || null
                };

                if (!state.isEditMode) {
                    payload.createdBy = toNullableInteger(values.createdBy);
                }

                return payload;
            }
        }
    };

    window.CurrencyRateModule.config.moduleRef = window.CurrencyRateModule;
    window.CurrencyRateModule.config.form.fields.forEach(function (field) {
        field.moduleRef = window.CurrencyRateModule;
    });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.CurrencyRateModule.Summary?.init?.();
        window.CurrencyRateModule.Details?.init?.();

        $(window.CurrencyRateModule.config.dom.addButton).on('click', function () {
            window.CurrencyRateModule.Details?.openAddForm?.();
        });

        $(window.CurrencyRateModule.config.dom.refreshButton).on('click', function () {
            window.CurrencyRateModule.Summary?.refreshGrid?.();
        });
    });
})();
