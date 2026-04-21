(function () {
    'use strict';

    window.MonthModule = window.MonthModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.MonthModule.config = {
        moduleTitle: 'Month',
        pluralTitle: 'Months',
        idField: 'monthId',
        displayField: 'monthName',
        dom: {
            grid: '#monthGrid',
            window: '#monthWindow',
            form: '#monthForm',
            addButton: '#btnAddMonth',
            refreshButton: '#btnRefreshMonth',
            saveButton: '#btnSaveMonth',
            cancelButton: '#btnCancelMonth'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-month-summary`,
            create: `${apiRoot}/crm-month`,
            update: function (id) { return `${apiRoot}/crm-month/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-month/${id}`; },
            read: function (id) { return `${apiRoot}/crm-month/${id}`; }
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
            columns: [{field: "monthId", title: "ID", width: 90, dataType: "number", filterable: false}, {field: "monthName", title: "Month Name", width: 220}, {field: "monthCode", title: "Code", width: 140}, {field: "status", title: "Status", width: 120, kind: "boolean", dataType: "boolean", trueText: "Active", falseText: "Inactive"}]
        },
        form: {
            fields: [{name: "monthId", label: "Month Id", type: "hidden"}, {name: "monthName", label: "Month Name", type: "text", required: true, maxLength: 100, placeholder: "Enter month name"}, {name: "monthCode", label: "Month Code", type: "text", maxLength: 50, placeholder: "Enter month code"}, {name: "status", label: "Active", type: "checkbox", defaultValue: true}],
            buildPayload: function (values, state) {
                function normalizeString(value) {
                    return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim();
                }

                function toInteger(value) {
                    const parsed = parseInt(value || 0, 10);
                    return Number.isNaN(parsed) ? 0 : parsed;
                }

                return {
                                monthId: toInteger(values.monthId),
                                monthName: (values.monthName || '').trim(),
                                monthCode: normalizeString(values.monthCode),
                                status: values.status !== false
                            };
            }
        }
    };

    window.MonthModule.config.moduleRef = window.MonthModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.MonthModule.Summary?.init?.();
        window.MonthModule.Details?.init?.();

        $(window.MonthModule.config.dom.addButton).on('click', function () {
            window.MonthModule.Details?.openAddForm?.();
        });

        $(window.MonthModule.config.dom.refreshButton).on('click', function () {
            window.MonthModule.Summary?.refreshGrid?.();
        });
    });
})();
