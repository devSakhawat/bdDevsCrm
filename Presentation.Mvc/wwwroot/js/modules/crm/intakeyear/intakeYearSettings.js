(function () {
    'use strict';

    window.IntakeYearModule = window.IntakeYearModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.IntakeYearModule.config = {
        moduleTitle: 'Intake Year',
        pluralTitle: 'Intake Years',
        idField: 'intakeYearId',
        displayField: 'yearName',
        dom: {
            grid: '#intakeYearGrid',
            window: '#intakeYearWindow',
            form: '#intakeYearForm',
            addButton: '#btnAddIntakeYear',
            refreshButton: '#btnRefreshIntakeYear',
            saveButton: '#btnSaveIntakeYear',
            cancelButton: '#btnCancelIntakeYear'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-intake-year-summary`,
            create: `${apiRoot}/crm-intake-year`,
            update: function (id) { return `${apiRoot}/crm-intake-year/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-intake-year/${id}`; },
            read: function (id) { return `${apiRoot}/crm-intake-year/${id}`; }
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
            columns: [{field: "intakeYearId", title: "ID", width: 90, dataType: "number", filterable: false}, {field: "yearName", title: "Year Name", width: 220}, {field: "yearCode", title: "Code", width: 140}, {field: "yearValue", title: "Year Value", width: 140, dataType: "number"}, {field: "isActive", title: "Status", width: 120, kind: "boolean", dataType: "boolean", trueText: "Active", falseText: "Inactive"}]
        },
        form: {
            fields: [{name: "intakeYearId", label: "Intake Year Id", type: "hidden"}, {name: "yearName", label: "Year Name", type: "text", required: true, maxLength: 100, placeholder: "Enter intake year name"}, {name: "yearCode", label: "Year Code", type: "text", maxLength: 50, placeholder: "Enter intake year code"}, {name: "yearValue", label: "Year Value", type: "number", required: true, min: 1, step: 1, placeholder: "Enter numeric year"}, {name: "description", label: "Description", type: "textarea", maxLength: 255, wide: true, placeholder: "Enter description"}, {name: "isActive", label: "Active", type: "checkbox", defaultValue: true}],
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
                                intakeYearId: toInteger(values.intakeYearId),
                                yearName: (values.yearName || '').trim(),
                                yearCode: normalizeString(values.yearCode),
                                yearValue: Number(values.yearValue || 0),
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

    window.IntakeYearModule.config.moduleRef = window.IntakeYearModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.IntakeYearModule.Summary?.init?.();
        window.IntakeYearModule.Details?.init?.();

        $(window.IntakeYearModule.config.dom.addButton).on('click', function () {
            window.IntakeYearModule.Details?.openAddForm?.();
        });

        $(window.IntakeYearModule.config.dom.refreshButton).on('click', function () {
            window.IntakeYearModule.Summary?.refreshGrid?.();
        });
    });
})();
