(function () {
    'use strict';

    window.TimesheetModule = window.TimesheetModule || {};

    window.TimesheetModule.config = {
        moduleTitle: 'Timesheet',
        pluralTitle: 'Timesheets',
        idField: 'timesheetid',
        displayField: 'timesheetid',
        dom: {
            grid: '#timesheetGrid',
            window: '#timesheetWindow',
            form: '#timesheetForm',
            addButton: '#btnAddTimesheet',
            refreshButton: '#btnRefreshTimesheet',
            saveButton: '#btnSaveTimesheet',
            cancelButton: '#btnCancelTimesheet'
        },
        apiEndpoints: {
            summary: `${window.AppConfig.apiBaseUrl}/core/systemadmin/timesheet-summary`,
            create: `${window.AppConfig.apiBaseUrl}/core/systemadmin/timesheet`,
            update: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/timesheet/${id}`; },
            delete: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/timesheet/${id}`; },
            read: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/timesheet/${id}`; }
        },
        gridOptions: {
            pageSize: 20,
            sortable: true,
            filterable: true,
            height: 560,
            pageable: { refresh: true, pageSizes: [10, 20, 50, 100], buttonCount: 5 }
        },
        windowOptions: { width: '920px' },
        grid: {
            columns: [
                { field: 'timesheetid', title: 'ID', width: 90, dataType: 'number', filterable: false },
                { field: 'hrrecordid', title: 'HR Record', width: 120, dataType: 'number' },
                { field: 'projectid', title: 'Project', width: 110, dataType: 'number' },
                { field: 'taskid', title: 'Task', width: 110, dataType: 'number' },
                { field: 'workingLogDate', title: 'Working Date', width: 160 },
                { field: 'workedLogHour', title: 'Worked Hours', width: 130, dataType: 'number', kind: 'currency' },
                { field: 'billableLogHour', title: 'Billable Hours', width: 130, dataType: 'number', kind: 'currency' },
                { field: 'isapprove', title: 'Approved', width: 110, kind: 'boolean', dataType: 'boolean', trueText: 'Yes', falseText: 'No' }
            ]
        },
        form: {
            fields: [
                { name: 'timesheetid', label: 'Timesheet Id', type: 'hidden' },
                { name: 'hrrecordid', label: 'HR Record Id', type: 'number', required: true, min: 1, placeholder: 'Enter HR record id' },
                { name: 'projectid', label: 'Project Id', type: 'number', min: 0, placeholder: 'Enter project id' },
                { name: 'taskid', label: 'Task Id', type: 'number', min: 0, placeholder: 'Enter task id' },
                { name: 'workingLogDate', label: 'Working Log Date', type: 'date', required: true },
                { name: 'workedLogHour', label: 'Worked Log Hour', type: 'number', min: 0, decimals: 2, step: 0.5, placeholder: 'Enter worked hours' },
                { name: 'billableLogHour', label: 'Billable Log Hour', type: 'number', min: 0, decimals: 2, step: 0.5, placeholder: 'Enter billable hours' },
                { name: 'noBillableLogHour', label: 'Non-billable Log Hour', type: 'number', min: 0, decimals: 2, step: 0.5, placeholder: 'Enter non-billable hours' },
                    { name: 'isapprove', label: 'Approved', type: 'checkbox', defaultValue: false }
                ],
            buildPayload: function (values) {
                function toInteger(value) { const parsed = parseInt(value || 0, 10); return Number.isNaN(parsed) ? 0 : parsed; }
                function toNullableInteger(value) { if (value === null || value === undefined || value === '') { return null; } const parsed = parseInt(value, 10); return Number.isNaN(parsed) ? null : parsed; }
                function toNullableNumber(value) { if (value === null || value === undefined || value === '') { return null; } const parsed = parseFloat(value); return Number.isNaN(parsed) ? null : parsed; }
                return {
                    timesheetid: toInteger(values.timesheetid),
                    hrrecordid: toNullableInteger(values.hrrecordid),
                    projectid: toNullableInteger(values.projectid),
                    taskid: toNullableInteger(values.taskid),
                    workingLogDate: values.workingLogDate || null,
                    workedLogHour: toNullableNumber(values.workedLogHour),
                    billableLogHour: toNullableNumber(values.billableLogHour),
                    noBillableLogHour: toNullableNumber(values.noBillableLogHour),
                    isapprove: values.isapprove ? 1 : 0
                };
            }
        }
    };

    window.TimesheetModule.config.moduleRef = window.TimesheetModule;
    window.TimesheetModule.config.form.fields.forEach(function (field) { field.moduleRef = window.TimesheetModule; });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () { window.location.href = '/Account/Login'; }, 1500);
            return;
        }

        window.TimesheetModule.Summary?.init?.();
        window.TimesheetModule.Details?.init?.();

        $(window.TimesheetModule.config.dom.addButton).on('click', function () { window.TimesheetModule.Details?.openAddForm?.(); });
        $(window.TimesheetModule.config.dom.refreshButton).on('click', function () { window.TimesheetModule.Summary?.refreshGrid?.(); });
    });
})();
