(function () {
    'use strict';

    window.StudentModule = window.StudentModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.StudentModule.config = {
        moduleTitle: 'Student',
        pluralTitle: 'Students',
        idField: 'studentId',
        displayField: 'studentName',
        dom: {
            grid: '#studentGrid',
            window: '#studentWindow',
            form: '#studentForm',
            addButton: '#btnAddStudent',
            refreshButton: '#btnRefreshStudent',
            saveButton: '#btnSaveStudent',
            cancelButton: '#btnCancelStudent'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-student-summary`,
            create: `${apiRoot}/crm-student`,
            update: function (id) { return `${apiRoot}/crm-student/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-student/${id}`; },
            read: function (id) { return `${apiRoot}/crm-student/${id}`; },
            studentStatusesDdl: `${apiRoot}/crm-student-statuses-ddl`,
            agentsDdl: `${apiRoot}/crm-agents-ddl`,
            counselorsDdl: `${apiRoot}/crm-counselors-ddl`,
            visaTypesDdl: `${apiRoot}/crm-visa-types-ddl`
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
            columns: [{field: "studentId", title: "#", width: 60, dataType: "number", filterable: false}, {field: "studentName", title: "Student Name", width: 200}, {field: "studentCode", title: "Code", width: 130}, {field: "email", title: "Email", width: 180}, {field: "phone", title: "Phone", width: 130}, {field: "nationality", title: "Nationality", width: 150}, {field: "isActive", title: "Status", width: 100, dataType: "boolean", kind: "boolean", trueText: "Active", falseText: "Inactive"}]
        },
        form: {
            fields: [{name: "studentId", label: "Student Id", type: "hidden"}, {name: "studentName", label: "Student Name", type: "text", required: true, maxLength: 150, placeholder: "Enter student name"}, {name: "studentCode", label: "Student Code", type: "text", maxLength: 50, placeholder: "Enter student code"}, {name: "email", label: "Email", type: "text", maxLength: 150, placeholder: "Enter email"}, {name: "phone", label: "Phone", type: "text", maxLength: 50, placeholder: "Enter phone"}, {name: "leadId", label: "Lead Id", type: "number", placeholder: "Enter lead id", min: 0}, {name: "studentStatusId", label: "Student Status", type: "select", dataTextField: "statusName", dataValueField: "studentStatusId", dataSourceEndpoint: `${apiRoot}/crm-student-statuses-ddl`}, {name: "agentId", label: "Agent", type: "select", dataTextField: "agentName", dataValueField: "agentId", dataSourceEndpoint: `${apiRoot}/crm-agents-ddl`}, {name: "counselorId", label: "Counselor", type: "select", dataTextField: "counselorName", dataValueField: "counselorId", dataSourceEndpoint: `${apiRoot}/crm-counselors-ddl`}, {name: "dateOfBirth", label: "Date Of Birth", type: "date"}, {name: "passportNumber", label: "Passport Number", type: "text", maxLength: 50, placeholder: "Enter passport number"}, {name: "visaTypeId", label: "Visa Type", type: "select", dataTextField: "visaTypeName", dataValueField: "visaTypeId", dataSourceEndpoint: `${apiRoot}/crm-visa-types-ddl`}, {name: "nationality", label: "Nationality", type: "text", maxLength: 100, placeholder: "Enter nationality"}, {name: "isActive", label: "Active", type: "checkbox", defaultValue: true}],
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
                    studentId: toInteger(values.studentId),
                    studentName: (values.studentName || '').trim(),
                    studentCode: normalizeString(values.studentCode),
                    email: normalizeString(values.email),
                    phone: normalizeString(values.phone),
                    leadId: toInteger(values.leadId),
                    studentStatusId: toInteger(values.studentStatusId),
                    agentId: toInteger(values.agentId),
                    counselorId: toInteger(values.counselorId),
                    dateOfBirth: normalizeString(values.dateOfBirth),
                    passportNumber: normalizeString(values.passportNumber),
                    visaTypeId: toInteger(values.visaTypeId),
                    nationality: normalizeString(values.nationality),
                    isActive: values.isActive !== false,
                    createdDate: state.currentRecord?.createdDate || now,
                    createdBy: state.currentRecord?.createdBy || 1,
                    updatedDate: state.isEditMode ? now : null,
                    updatedBy: state.isEditMode ? 1 : null
                };
            }
        }
    };

    window.StudentModule.config.moduleRef = window.StudentModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.StudentModule.Summary?.init?.();
        window.StudentModule.Details?.init?.();

        $(window.StudentModule.config.dom.addButton).on('click', function () {
            window.StudentModule.Details?.openAddForm?.();
        });

        $(window.StudentModule.config.dom.refreshButton).on('click', function () {
            window.StudentModule.Summary?.refreshGrid?.();
        });
    });
})();
