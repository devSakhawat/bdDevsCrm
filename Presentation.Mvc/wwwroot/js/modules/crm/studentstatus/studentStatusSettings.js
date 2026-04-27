(function () {
    'use strict';

    window.StudentStatusModule = window.StudentStatusModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.StudentStatusModule.config = {
        moduleTitle: 'Student Status',
        pluralTitle: 'Student Statuses',
        idField: 'studentStatusId',
        displayField: 'statusName',
        dom: {
            grid: '#studentStatusGrid',
            window: '#studentStatusWindow',
            form: '#studentStatusForm',
            addButton: '#btnAddStudentStatus',
            refreshButton: '#btnRefreshStudentStatus',
            saveButton: '#btnSaveStudentStatus',
            cancelButton: '#btnCancelStudentStatus'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-student-status-summary`,
            create: `${apiRoot}/crm-student-status`,
            update: function (id) { return `${apiRoot}/crm-student-status/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-student-status/${id}`; },
            read: function (id) { return `${apiRoot}/crm-student-status/${id}`; }
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
            width: '600px'
        },
        grid: {
            columns: [{field: "studentStatusId", title: "#", width: 60, dataType: "number", filterable: false}, {field: "statusName", title: "Status Name", width: 200}, {field: "statusCode", title: "Code", width: 130}, {field: "colorCode", title: "Color", width: 130}, {field: "isActive", title: "Status", width: 100, dataType: "boolean", kind: "boolean", trueText: "Active", falseText: "Inactive"}]
        },
        form: {
            fields: [{name: "studentStatusId", label: "Student Status Id", type: "hidden"}, {name: "statusName", label: "Status Name", type: "text", required: true, maxLength: 100, placeholder: "Enter status name"}, {name: "statusCode", label: "Status Code", type: "text", maxLength: 50, placeholder: "Enter status code"}, {name: "colorCode", label: "Color Code", type: "text", maxLength: 50, placeholder: "#FFFFFF"}, {name: "isActive", label: "Active", type: "checkbox", defaultValue: true}],
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
                    studentStatusId: toInteger(values.studentStatusId),
                    statusName: (values.statusName || '').trim(),
                    statusCode: normalizeString(values.statusCode),
                    colorCode: normalizeString(values.colorCode),
                    isActive: values.isActive !== false,
                    createdDate: state.currentRecord?.createdDate || now,
                    createdBy: state.currentRecord?.createdBy || 1,
                    updatedDate: state.isEditMode ? now : null,
                    updatedBy: state.isEditMode ? 1 : null
                };
            }
        }
    };

    window.StudentStatusModule.config.moduleRef = window.StudentStatusModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.StudentStatusModule.Summary?.init?.();
        window.StudentStatusModule.Details?.init?.();

        $(window.StudentStatusModule.config.dom.addButton).on('click', function () {
            window.StudentStatusModule.Details?.openAddForm?.();
        });

        $(window.StudentStatusModule.config.dom.refreshButton).on('click', function () {
            window.StudentStatusModule.Summary?.refreshGrid?.();
        });
    });
})();
