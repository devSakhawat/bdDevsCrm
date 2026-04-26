(function () {
    'use strict';

    window.ApplicantCourseModule = window.ApplicantCourseModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.ApplicantCourseModule.config = {
        moduleTitle: 'Applicant Course',
        pluralTitle: 'Applicant Courses',
        idField: 'applicantCourseId',
        displayField: 'courseTitle',
        dom: {
            grid: '#applicantCourseGrid',
            window: '#applicantCourseWindow',
            form: '#applicantCourseForm',
            addButton: '#btnAddApplicantCourse',
            refreshButton: '#btnRefreshApplicantCourse',
            saveButton: '#btnSaveApplicantCourse',
            cancelButton: '#btnCancelApplicantCourse'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-applicant-course-summary`,
            create: `${apiRoot}/crm-applicant-course`,
            update: function (id) { return `${apiRoot}/crm-applicant-course/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-applicant-course/${id}`; },
            read: function (id) { return `${apiRoot}/crm-applicant-course/${id}`; },
            applicants: `${apiRoot}/crm-applicant-infos-ddl`,
            countries: `${apiRoot}/countryddl`,
            institutesByCountry: function (countryId) { return countryId ? `${apiRoot}/crm-institutes-by-country/${countryId}` : null; },
            coursesByInstitute: function (instituteId) { return instituteId ? `${apiRoot}/crm-courses-by-institute/${instituteId}` : null; },
            intakeMonths: `${apiRoot}/crm-intake-months`,
            intakeYears: `${apiRoot}/crm-intake-years`,
            paymentMethods: `${apiRoot}/crm-payment-methods`,
            currencies: `${apiRoot}/currencies-ddl`
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
            width: '920px'
        },
        grid: {
            columns: [
                { field: 'applicantCourseId', title: 'ID', width: 90, dataType: 'number', filterable: false },
                { field: 'applicantId', title: 'Applicant ID', width: 120, dataType: 'number' },
                { field: 'countryName', title: 'Country', width: 160 },
                { field: 'instituteName', title: 'Institute', width: 180 },
                { field: 'courseTitle', title: 'Course Title', width: 220 },
                { field: 'intakeMonth', title: 'Intake Month', width: 130 },
                { field: 'intakeYear', title: 'Intake Year', width: 130 },
                { field: 'paymentMethod', title: 'Payment Method', width: 150 }
            ]
        },
        form: {
            fields: [
                { name: 'applicantCourseId', label: 'Applicant Course Id', type: 'hidden' },
                {
                    name: 'applicantId',
                    label: 'Applicant',
                    type: 'select',
                    required: true,
                    dataSourceEndpoint: function () { return window.ApplicantCourseModule.config.apiEndpoints.applicants; },
                    dataTextField: 'applicantName',
                    dataValueField: 'applicantId',
                    optionLabel: 'Select Applicant...'
                },
                {
                    name: 'countryId',
                    label: 'Country',
                    type: 'select',
                    required: true,
                    dataSourceEndpoint: function () { return window.ApplicantCourseModule.config.apiEndpoints.countries; },
                    dataTextField: 'countryName',
                    dataValueField: 'countryId',
                    optionLabel: 'Select Country...'
                },
                {
                    name: 'instituteId',
                    label: 'Institute',
                    type: 'select',
                    required: true,
                    dependsOn: 'countryId',
                    dataSourceEndpoint: function (values) { return window.ApplicantCourseModule.config.apiEndpoints.institutesByCountry(values.countryId); },
                    dataTextField: 'instituteName',
                    dataValueField: 'instituteId',
                    optionLabel: 'Select Institute...'
                },
                {
                    name: 'courseId',
                    label: 'Course',
                    type: 'select',
                    dependsOn: 'instituteId',
                    dataSourceEndpoint: function (values) { return window.ApplicantCourseModule.config.apiEndpoints.coursesByInstitute(values.instituteId); },
                    dataTextField: 'courseTitle',
                    dataValueField: 'courseId',
                    optionLabel: 'Select Course...',
                    onChange: function (context) {
                        const item = context.dataItem;
                        if (!item) {
                            context.setFieldValue('courseTitle', '');
                            context.setFieldValue('applicationFee', '');
                            return;
                        }

                        context.setFieldValue('courseTitle', item.courseTitle || '');
                        context.setFieldValue('applicationFee', item.applicationFee ?? '');
                        if (item.currencyId) {
                            context.setFieldValue('currencyId', item.currencyId);
                        }
                    }
                },
                {
                    name: 'courseTitle',
                    label: 'Course Title',
                    type: 'text',
                    maxLength: 255,
                    readonly: true,
                    wide: true,
                    placeholder: 'Auto populated from selected course'
                },
                {
                    name: 'intakeMonthId',
                    label: 'Intake Month',
                    type: 'select',
                    required: true,
                    dataSourceEndpoint: function () { return window.ApplicantCourseModule.config.apiEndpoints.intakeMonths; },
                    dataTextField: 'monthName',
                    dataValueField: 'intakeMonthId',
                    optionLabel: 'Select Intake Month...'
                },
                {
                    name: 'intakeYearId',
                    label: 'Intake Year',
                    type: 'select',
                    required: true,
                    dataSourceEndpoint: function () { return window.ApplicantCourseModule.config.apiEndpoints.intakeYears; },
                    dataTextField: 'yearName',
                    dataValueField: 'intakeYearId',
                    optionLabel: 'Select Intake Year...'
                },
                {
                    name: 'applicationFee',
                    label: 'Application Fee',
                    type: 'number',
                    decimals: 2,
                    step: 0.01,
                    min: 0,
                    placeholder: '0.00'
                },
                {
                    name: 'currencyId',
                    label: 'Currency',
                    type: 'select',
                    required: true,
                    dataSourceEndpoint: function () { return window.ApplicantCourseModule.config.apiEndpoints.currencies; },
                    dataTextField: 'currencyName',
                    dataValueField: 'currencyId',
                    optionLabel: 'Select Currency...'
                },
                {
                    name: 'paymentMethodId',
                    label: 'Payment Method',
                    type: 'select',
                    required: true,
                    dataSourceEndpoint: function () { return window.ApplicantCourseModule.config.apiEndpoints.paymentMethods; },
                    dataTextField: 'paymentMethodName',
                    dataValueField: 'paymentMethodId',
                    optionLabel: 'Select Payment Method...'
                },
                {
                    name: 'paymentReferenceNumber',
                    label: 'Payment Reference Number',
                    type: 'text',
                    maxLength: 255,
                    placeholder: 'Enter payment reference number'
                },
                {
                    name: 'paymentDate',
                    label: 'Payment Date',
                    type: 'date'
                },
                {
                    name: 'remarks',
                    label: 'Remarks',
                    type: 'textarea',
                    maxLength: 255,
                    wide: true,
                    placeholder: 'Enter remarks'
                }
            ],
            buildPayload: function (values, state, helpers) {
                function toInteger(value) {
                    const parsed = parseInt(value || 0, 10);
                    return Number.isNaN(parsed) ? 0 : parsed;
                }

                function normalizeString(value) {
                    return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim();
                }

                const now = new Date().toISOString();
                const countryItem = helpers.getDataItem('countryId');
                const intakeMonthItem = helpers.getDataItem('intakeMonthId');
                const intakeYearItem = helpers.getDataItem('intakeYearId');
                const paymentMethodItem = helpers.getDataItem('paymentMethodId');

                return {
                    applicantCourseId: toInteger(values.applicantCourseId),
                    applicantId: toInteger(values.applicantId),
                    countryId: toInteger(values.countryId),
                    countryName: countryItem?.countryName || helpers.getText('countryId') || null,
                    instituteId: toInteger(values.instituteId),
                    courseTitle: normalizeString(values.courseTitle),
                    intakeMonthId: toInteger(values.intakeMonthId),
                    intakeMonth: intakeMonthItem?.monthName || helpers.getText('intakeMonthId') || null,
                    intakeYearId: toInteger(values.intakeYearId),
                    intakeYear: intakeYearItem?.yearName || helpers.getText('intakeYearId') || null,
                    applicationFee: values.applicationFee === null || values.applicationFee === undefined || values.applicationFee === '' ? null : String(values.applicationFee),
                    currencyId: toInteger(values.currencyId),
                    paymentMethodId: toInteger(values.paymentMethodId),
                    paymentMethod: paymentMethodItem?.paymentMethodName || helpers.getText('paymentMethodId') || null,
                    paymentReferenceNumber: normalizeString(values.paymentReferenceNumber),
                    paymentDate: values.paymentDate || null,
                    remarks: normalizeString(values.remarks),
                    createdDate: state.currentRecord?.createdDate || now,
                    createdBy: state.currentRecord?.createdBy || 1,
                    updatedDate: state.isEditMode ? now : null,
                    updatedBy: state.isEditMode ? 1 : null,
                    courseId: values.courseId ? toInteger(values.courseId) : null
                };
            }
        }
    };

    window.ApplicantCourseModule.config.moduleRef = window.ApplicantCourseModule;
    window.ApplicantCourseModule.config.form.fields.forEach(function (field) {
        field.moduleRef = window.ApplicantCourseModule;
    });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.ApplicantCourseModule.Summary?.init?.();
        window.ApplicantCourseModule.Details?.init?.();

        $(window.ApplicantCourseModule.config.dom.addButton).on('click', function () {
            window.ApplicantCourseModule.Details?.openAddForm?.();
        });

        $(window.ApplicantCourseModule.config.dom.refreshButton).on('click', function () {
            window.ApplicantCourseModule.Summary?.refreshGrid?.();
        });
    });
})();
