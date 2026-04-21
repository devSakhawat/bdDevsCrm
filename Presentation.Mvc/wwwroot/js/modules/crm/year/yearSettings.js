(function () {
    'use strict';

    window.YearModule = window.YearModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.YearModule.config = {
        moduleTitle: 'Year',
        pluralTitle: 'Years',
        idField: 'yearId',
        displayField: 'yearName',
        dom: {
            grid: '#yearGrid',
            window: '#yearWindow',
            form: '#yearForm',
            addButton: '#btnAddYear',
            refreshButton: '#btnRefreshYear',
            saveButton: '#btnSaveYear',
            cancelButton: '#btnCancelYear'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-year-summary`,
            create: `${apiRoot}/crm-year`,
            update: function (id) { return `${apiRoot}/crm-year/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-year/${id}`; },
            read: function (id) { return `${apiRoot}/crm-year/${id}`; }
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
            columns: [{field: "yearId", title: "ID", width: 90, dataType: "number", filterable: false}, {field: "yearName", title: "Year Name", width: 220}, {field: "yearCode", title: "Code", width: 140}, {field: "status", title: "Status", width: 120, kind: "boolean", dataType: "boolean", trueText: "Active", falseText: "Inactive"}]
        },
        form: {
            fields: [{name: "yearId", label: "Year Id", type: "hidden"}, {name: "yearName", label: "Year Name", type: "text", required: true, maxLength: 100, placeholder: "Enter year name"}, {name: "yearCode", label: "Year Code", type: "text", maxLength: 50, placeholder: "Enter year code"}, {name: "status", label: "Active", type: "checkbox", defaultValue: true}],
            buildPayload: function (values, state) {
                function normalizeString(value) {
                    return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim();
                }

                function toInteger(value) {
                    const parsed = parseInt(value || 0, 10);
                    return Number.isNaN(parsed) ? 0 : parsed;
                }

                return {
                                yearId: toInteger(values.yearId),
                                yearName: (values.yearName || '').trim(),
                                yearCode: normalizeString(values.yearCode),
                                status: values.status !== false
                            };
            }
        }
    };

    window.YearModule.config.moduleRef = window.YearModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.YearModule.Summary?.init?.();
        window.YearModule.Details?.init?.();

        $(window.YearModule.config.dom.addButton).on('click', function () {
            window.YearModule.Details?.openAddForm?.();
        });

        $(window.YearModule.config.dom.refreshButton).on('click', function () {
            window.YearModule.Summary?.refreshGrid?.();
        });
    });
})();
