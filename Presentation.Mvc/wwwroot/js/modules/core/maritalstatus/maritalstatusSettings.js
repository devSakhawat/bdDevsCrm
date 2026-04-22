(function () {
    'use strict';

    window.MaritalStatusModule = window.MaritalStatusModule || {};

    window.MaritalStatusModule.config = {
        moduleTitle: 'Marital Status',
        pluralTitle: 'Marital Statuses',
        idField: 'maritalStatusId',
        displayField: 'maritalStatusName',
        dom: {
            grid: '#maritalStatusGrid',
            window: '#maritalStatusWindow',
            form: '#maritalStatusForm',
            addButton: '#btnAddMaritalStatus',
            refreshButton: '#btnRefreshMaritalStatus',
            saveButton: '#btnSaveMaritalStatus',
            cancelButton: '#btnCancelMaritalStatus'
        },
        apiEndpoints: {
            summary: `${window.AppConfig.apiBaseUrl}/core/systemadmin/marital-status-summary`,
            create: `${window.AppConfig.apiBaseUrl}/core/systemadmin/marital-status`,
            update: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/marital-status/${id}`; },
            delete: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/marital-status/${id}`; },
            read: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/marital-status/${id}`; }
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
            columns: [{field: 'maritalStatusId', title: 'ID', width: 90, dataType: 'number', filterable: false}, {field: 'maritalStatusName', title: 'Marital Status', width: 240}, {field: 'isActive', title: 'Status', width: 120, kind: 'boolean', dataType: 'boolean', trueText: 'Active', falseText: 'Inactive'}]
        },
        form: {
            fields: [{name: 'maritalStatusId', label: 'Marital Status Id', type: 'hidden'}, {name: 'maritalStatusName', label: 'Marital Status', type: 'text', required: true, maxLength: 100, wide: true, placeholder: 'Enter marital status name'}, {name: 'isActive', label: 'Active', type: 'checkbox', defaultValue: true}],
            buildPayload: function (values) {
                function normalizeString(value) {
                    return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim();
                }

                function toInteger(value) {
                    const parsed = parseInt(value || 0, 10);
                    return Number.isNaN(parsed) ? 0 : parsed;
                }

                return {
                    maritalStatusId: toInteger(values.maritalStatusId),
                    maritalStatusName: normalizeString(values.maritalStatusName),
                    isActive: values.isActive !== false ? 1 : 0
                };
            }
        }
    };

    window.MaritalStatusModule.config.moduleRef = window.MaritalStatusModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.MaritalStatusModule.Summary?.init?.();
        window.MaritalStatusModule.Details?.init?.();

        $(window.MaritalStatusModule.config.dom.addButton).on('click', function () {
            window.MaritalStatusModule.Details?.openAddForm?.();
        });

        $(window.MaritalStatusModule.config.dom.refreshButton).on('click', function () {
            window.MaritalStatusModule.Summary?.refreshGrid?.();
        });
    });
})();
