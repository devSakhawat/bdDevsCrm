(function () {
    'use strict';

    window.EnquiryModule = window.EnquiryModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.EnquiryModule.config = {
        moduleTitle: 'Enquiry',
        pluralTitle: 'Enquiries',
        idField: 'enquiryId',
        displayField: 'enquiryId',
        dom: {
            grid: '#enquiryGrid',
            window: '#enquiryWindow',
            form: '#enquiryForm',
            addButton: '#btnAddEnquiry',
            refreshButton: '#btnRefreshEnquiry',
            saveButton: '#btnSaveEnquiry',
            cancelButton: '#btnCancelEnquiry'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-enquiry-summary`,
            create: `${apiRoot}/crm-enquiry`,
            update: function (id) { return `${apiRoot}/crm-enquiry/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-enquiry/${id}`; },
            read: function (id) { return `${apiRoot}/crm-enquiry/${id}`; }
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
            width: '900px'
        },
        grid: {
            columns: [{field: "enquiryId", title: "#", width: 60, dataType: "number", filterable: false}, {field: "leadId", title: "Lead ID", width: 100, dataType: "number"}, {field: "studentId", title: "Student ID", width: 100, dataType: "number"}, {field: "enquiryDate", title: "Date", width: 130}, {field: "expectedIntakeMonth", title: "Month", width: 100, dataType: "number"}, {field: "expectedIntakeYear", title: "Year", width: 100, dataType: "number"}, {field: "isActive", title: "Status", width: 100, dataType: "boolean", kind: "boolean", trueText: "Active", falseText: "Inactive"}]
        },
        form: {
            fields: [{name: "enquiryId", label: "Enquiry Id", type: "hidden"}, {name: "leadId", label: "Lead Id", type: "number", placeholder: "Enter lead id", min: 0}, {name: "studentId", label: "Student Id", type: "number", placeholder: "Enter student id", min: 0}, {name: "courseId", label: "Course Id", type: "number", placeholder: "Enter course id", min: 0}, {name: "instituteId", label: "Institute Id", type: "number", placeholder: "Enter institute id", min: 0}, {name: "countryId", label: "Country Id", type: "number", placeholder: "Enter country id", min: 0}, {name: "enquiryDate", label: "Enquiry Date", type: "date"}, {name: "expectedIntakeMonth", label: "Expected Intake Month", type: "number", placeholder: "1-12", min: 1, max: 12}, {name: "expectedIntakeYear", label: "Expected Intake Year", type: "number", placeholder: "e.g. 2025", min: 2000}, {name: "notes", label: "Notes", type: "textarea", maxLength: 1000, wide: true, placeholder: "Enter notes"}, {name: "isActive", label: "Active", type: "checkbox", defaultValue: true}],
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
                    enquiryId: toInteger(values.enquiryId),
                    leadId: toInteger(values.leadId),
                    studentId: toInteger(values.studentId),
                    courseId: toInteger(values.courseId),
                    instituteId: toInteger(values.instituteId),
                    countryId: toInteger(values.countryId),
                    enquiryDate: normalizeString(values.enquiryDate),
                    expectedIntakeMonth: toInteger(values.expectedIntakeMonth),
                    expectedIntakeYear: toInteger(values.expectedIntakeYear),
                    notes: normalizeString(values.notes),
                    isActive: values.isActive !== false,
                    createdDate: state.currentRecord?.createdDate || now,
                    createdBy: state.currentRecord?.createdBy || 1,
                    updatedDate: state.isEditMode ? now : null,
                    updatedBy: state.isEditMode ? 1 : null
                };
            }
        }
    };

    window.EnquiryModule.config.moduleRef = window.EnquiryModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.EnquiryModule.Summary?.init?.();
        window.EnquiryModule.Details?.init?.();

        $(window.EnquiryModule.config.dom.addButton).on('click', function () {
            window.EnquiryModule.Details?.openAddForm?.();
        });

        $(window.EnquiryModule.config.dom.refreshButton).on('click', function () {
            window.EnquiryModule.Summary?.refreshGrid?.();
        });
    });
})();
