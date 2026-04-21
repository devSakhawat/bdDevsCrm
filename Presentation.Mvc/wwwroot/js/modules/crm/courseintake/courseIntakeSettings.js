(function () {
    'use strict';

    window.CourseIntakeModule = window.CourseIntakeModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.CourseIntakeModule.config = {
        moduleTitle: 'Course Intake',
        pluralTitle: 'Course Intakes',
        idField: 'courseIntakeId',
        displayField: 'intakeTitile',
        dom: {
            grid: '#courseIntakeGrid',
            window: '#courseIntakeWindow',
            form: '#courseIntakeForm',
            addButton: '#btnAddCourseIntake',
            refreshButton: '#btnRefreshCourseIntake',
            saveButton: '#btnSaveCourseIntake',
            cancelButton: '#btnCancelCourseIntake'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-course-intake-summary`,
            create: `${apiRoot}/crm-course-intake`,
            update: function (id) { return `${apiRoot}/crm-course-intake/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-course-intake/${id}`; },
            read: function (id) { return `${apiRoot}/crm-course-intake/${id}`; },
            courses: `${apiRoot}/crm-courses-ddl`,
            months: `${apiRoot}/crm-months`
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
            columns: [
                { field: 'courseIntakeId', title: 'ID', width: 90, dataType: 'number', filterable: false },
                { field: 'courseId', title: 'Course ID', width: 120, dataType: 'number' },
                { field: 'monthId', title: 'Month ID', width: 120, dataType: 'number' },
                { field: 'intakeTitile', title: 'Intake Title', width: 260 }
            ]
        },
        form: {
            fields: [
                { name: 'courseIntakeId', label: 'Course Intake Id', type: 'hidden' },
                {
                    name: 'courseId',
                    label: 'Course',
                    type: 'select',
                    required: true,
                    dataSourceEndpoint: function () { return window.CourseIntakeModule.config.apiEndpoints.courses; },
                    dataTextField: 'courseTitle',
                    dataValueField: 'courseId',
                    optionLabel: 'Select Course...'
                },
                {
                    name: 'monthId',
                    label: 'Month',
                    type: 'select',
                    dataSourceEndpoint: function () { return window.CourseIntakeModule.config.apiEndpoints.months; },
                    dataTextField: 'monthName',
                    dataValueField: 'monthId',
                    optionLabel: 'Select Month...'
                },
                {
                    name: 'intakeTitile',
                    label: 'Intake Title',
                    type: 'text',
                    maxLength: 100,
                    wide: true,
                    placeholder: 'Enter intake title'
                }
            ],
            buildPayload: function (values, state) {
                function toInteger(value) {
                    const parsed = parseInt(value || 0, 10);
                    return Number.isNaN(parsed) ? 0 : parsed;
                }

                function normalizeString(value) {
                    return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim();
                }

                return {
                    courseIntakeId: toInteger(values.courseIntakeId),
                    courseId: toInteger(values.courseId),
                    monthId: values.monthId ? toInteger(values.monthId) : null,
                    intakeTitile: normalizeString(values.intakeTitile)
                };
            }
        }
    };

    window.CourseIntakeModule.config.moduleRef = window.CourseIntakeModule;
    window.CourseIntakeModule.config.form.fields.forEach(function (field) {
        field.moduleRef = window.CourseIntakeModule;
    });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.CourseIntakeModule.Summary?.init?.();
        window.CourseIntakeModule.Details?.init?.();

        $(window.CourseIntakeModule.config.dom.addButton).on('click', function () {
            window.CourseIntakeModule.Details?.openAddForm?.();
        });

        $(window.CourseIntakeModule.config.dom.refreshButton).on('click', function () {
            window.CourseIntakeModule.Summary?.refreshGrid?.();
        });
    });
})();
