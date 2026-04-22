(function () {
    'use strict';

    window.HolidayModule = window.HolidayModule || {};

    window.HolidayModule.config = {
        moduleTitle: 'Holiday',
        pluralTitle: 'Holidays',
        idField: 'holidayId',
        displayField: 'description',
        dom: {
            grid: '#holidayGrid',
            window: '#holidayWindow',
            form: '#holidayForm',
            addButton: '#btnAddHoliday',
            refreshButton: '#btnRefreshHoliday',
            saveButton: '#btnSaveHoliday',
            cancelButton: '#btnCancelHoliday'
        },
        apiEndpoints: {
            summary: `${window.AppConfig.apiBaseUrl}/core/systemadmin/holiday-summary`,
            create: `${window.AppConfig.apiBaseUrl}/core/systemadmin/holiday`,
            update: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/holiday/${id}`; },
            delete: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/holiday/${id}`; },
            read: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/holiday/${id}`; }
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
            columns: [{field: 'holidayId', title: 'ID', width: 90, dataType: 'number', filterable: false}, {field: 'holidayDate', title: 'Holiday Date', width: 150}, {field: 'dayName', title: 'Day', width: 120}, {field: 'description', title: 'Description', width: 240}, {field: 'holidayType', title: 'Type', width: 110, dataType: 'number'}, {field: 'monthName', title: 'Month', width: 120}, {field: 'yearName', title: 'Year', width: 110, dataType: 'number'}, {field: 'rosterReschedule', title: 'Reschedule', width: 130, kind: 'boolean', dataType: 'boolean', trueText: 'Yes', falseText: 'No'}]
        },
        form: {
            fields: [{name: 'holidayId', label: 'Holiday Id', type: 'hidden'}, {name: 'holidayType', label: 'Holiday Type', type: 'number', min: 0, placeholder: 'Enter holiday type'}, {name: 'holidayDate', label: 'Holiday Date', type: 'date', required: true}, {name: 'shiftid', label: 'Shift Id', type: 'number', min: 0, placeholder: 'Enter shift id'}, {name: 'month', label: 'Month', type: 'number', min: 1, maxLength: 2, placeholder: 'Enter month number'}, {name: 'monthName', label: 'Month Name', type: 'text', maxLength: 50, placeholder: 'Enter month name'}, {name: 'yearName', label: 'Year', type: 'number', min: 0, placeholder: 'Enter year'}, {name: 'dayName', label: 'Day Name', type: 'text', maxLength: 50, placeholder: 'Enter day name'}, {name: 'description', label: 'Description', type: 'textarea', wide: true, maxLength: 255, placeholder: 'Enter description'}, {name: 'rosterReschedule', label: 'Roster Reschedule', type: 'checkbox', defaultValue: false}],
            buildPayload: function (values) {
                function normalizeString(value) {
                    return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim();
                }

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

                function toDateOnly(value) {
                    return value ? String(value).slice(0, 10) : null;
                }

                return {
                    holidayId: toInteger(values.holidayId),
                    holidayType: toNullableInteger(values.holidayType),
                    holidayDate: toDateOnly(values.holidayDate),
                    shiftid: toNullableInteger(values.shiftid),
                    month: toNullableInteger(values.month),
                    monthName: normalizeString(values.monthName),
                    yearName: toNullableInteger(values.yearName),
                    dayName: normalizeString(values.dayName),
                    description: normalizeString(values.description),
                    rosterReschedule: values.rosterReschedule ? 1 : 0
                };
            }
        }
    };

    window.HolidayModule.config.moduleRef = window.HolidayModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.HolidayModule.Summary?.init?.();
        window.HolidayModule.Details?.init?.();

        $(window.HolidayModule.config.dom.addButton).on('click', function () {
            window.HolidayModule.Details?.openAddForm?.();
        });

        $(window.HolidayModule.config.dom.refreshButton).on('click', function () {
            window.HolidayModule.Summary?.refreshGrid?.();
        });
    });
})();
