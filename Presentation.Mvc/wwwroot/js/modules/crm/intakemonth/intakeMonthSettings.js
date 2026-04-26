(function () {
    'use strict';

    window.IntakeMonthModule = window.IntakeMonthModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.IntakeMonthModule.config = {
        moduleTitle: 'Intake Month',
        pluralTitle: 'Intake Months',
        idField: 'intakeMonthId',
        displayField: 'monthName',
        dom: {
            grid: '#intakeMonthGrid',
            window: '#intakeMonthWindow',
            form: '#intakeMonthForm',
            addButton: '#btnAddIntakeMonth',
            refreshButton: '#btnRefreshIntakeMonth',
            saveButton: '#btnSaveIntakeMonth',
            cancelButton: '#btnCancelIntakeMonth'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-intake-month-summary`,
            create: `${apiRoot}/crm-intake-month`,
            update: function (id) { return `${apiRoot}/crm-intake-month/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-intake-month/${id}`; },
            read: function (id) { return `${apiRoot}/crm-intake-month/${id}`; }
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
            columns: [{field: "intakeMonthId", title: "ID", width: 90, dataType: "number", filterable: false}, {field: "monthName", title: "Month Name", width: 220}, {field: "monthCode", title: "Code", width: 140}, {field: "monthNumber", title: "Month Number", width: 140, dataType: "number"}, {field: "isActive", title: "Status", width: 120, kind: "boolean", dataType: "boolean", trueText: "Active", falseText: "Inactive"}]
        },
        form: {
            fields: [{name: "intakeMonthId", label: "Intake Month Id", type: "hidden"}, {name: "monthName", label: "Month Name", type: "text", required: true, maxLength: 100, placeholder: "Enter intake month name"}, {name: "monthCode", label: "Month Code", type: "text", maxLength: 50, placeholder: "Enter intake month code"}, {name: "monthNumber", label: "Month Number", type: "number", required: true, min: 1, step: 1, placeholder: "Enter month number"}, {name: "description", label: "Description", type: "textarea", maxLength: 255, wide: true, placeholder: "Enter description"}, {name: "isActive", label: "Active", type: "checkbox", defaultValue: true}],
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
                                intakeMonthId: toInteger(values.intakeMonthId),
                                monthName: (values.monthName || '').trim(),
                                monthCode: normalizeString(values.monthCode),
                                monthNumber: Number(values.monthNumber || 0),
                                description: normalizeString(values.description),
                                isActive: values.isActive !== false,
                                createdDate: state.currentRecord?.createdDate || now,
                                createdBy: state.currentRecord?.createdBy || 1,
                                updatedDate: state.isEditMode ? now : null,
                                updatedBy: state.isEditMode ? 1 : null
                            };
            }
        }
    };

    window.IntakeMonthModule.config.moduleRef = window.IntakeMonthModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.IntakeMonthModule.Summary?.init?.();
        window.IntakeMonthModule.Details?.init?.();

        $(window.IntakeMonthModule.config.dom.addButton).on('click', function () {
            window.IntakeMonthModule.Details?.openAddForm?.();
        });

        $(window.IntakeMonthModule.config.dom.refreshButton).on('click', function () {
            window.IntakeMonthModule.Summary?.refreshGrid?.();
        });
    });
})();
