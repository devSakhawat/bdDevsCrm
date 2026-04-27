(function () {
    'use strict';

    window.TaskModule = window.TaskModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.TaskModule.config = {
        moduleTitle: 'Task',
        pluralTitle: 'Tasks',
        idField: 'taskId',
        displayField: 'taskTitle',
        dom: {
            grid: '#taskGrid',
            window: '#taskWindow',
            form: '#taskForm',
            addButton: '#btnAddTask',
            refreshButton: '#btnRefreshTask',
            saveButton: '#btnSaveTask',
            cancelButton: '#btnCancelTask'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-task-summary`,
            create: `${apiRoot}/crm-task`,
            update: function (id) { return `${apiRoot}/crm-task/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-task/${id}`; },
            read: function (id) { return `${apiRoot}/crm-task/${id}`; }
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
            columns: [{field: "taskId", title: "#", width: 60, dataType: "number", filterable: false}, {field: "taskTitle", title: "Title", width: 250}, {field: "dueDate", title: "Due Date", width: 130}, {field: "priority", title: "Priority", width: 130}, {field: "isCompleted", title: "Completed", width: 120, dataType: "boolean", kind: "boolean", trueText: "Yes", falseText: "No"}]
        },
        form: {
            fields: [{name: "taskId", label: "Task Id", type: "hidden"}, {name: "taskTitle", label: "Task Title", type: "text", required: true, maxLength: 200, placeholder: "Enter task title"}, {name: "taskDescription", label: "Task Description", type: "textarea", maxLength: 1000, wide: true, placeholder: "Enter task description"}, {name: "dueDate", label: "Due Date", type: "date"}, {name: "assignedTo", label: "Assigned To", type: "number", placeholder: "Enter user id", min: 0}, {name: "relatedEntityType", label: "Related Entity Type", type: "text", maxLength: 100, placeholder: "Enter related entity type"}, {name: "relatedEntityId", label: "Related Entity Id", type: "number", placeholder: "Enter related entity id", min: 0}, {name: "priority", label: "Priority", type: "text", maxLength: 50, placeholder: "e.g. High, Medium, Low"}, {name: "isCompleted", label: "Completed", type: "checkbox", defaultValue: false}, {name: "completedDate", label: "Completed Date", type: "date"}],
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
                    taskId: toInteger(values.taskId),
                    taskTitle: (values.taskTitle || '').trim(),
                    taskDescription: normalizeString(values.taskDescription),
                    dueDate: normalizeString(values.dueDate),
                    assignedTo: toInteger(values.assignedTo),
                    relatedEntityType: normalizeString(values.relatedEntityType),
                    relatedEntityId: toInteger(values.relatedEntityId),
                    priority: normalizeString(values.priority),
                    isCompleted: !!values.isCompleted,
                    completedDate: normalizeString(values.completedDate),
                    createdDate: state.currentRecord?.createdDate || now,
                    createdBy: state.currentRecord?.createdBy || 1,
                    updatedDate: state.isEditMode ? now : null,
                    updatedBy: state.isEditMode ? 1 : null
                };
            }
        }
    };

    window.TaskModule.config.moduleRef = window.TaskModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.TaskModule.Summary?.init?.();
        window.TaskModule.Details?.init?.();

        $(window.TaskModule.config.dom.addButton).on('click', function () {
            window.TaskModule.Details?.openAddForm?.();
        });

        $(window.TaskModule.config.dom.refreshButton).on('click', function () {
            window.TaskModule.Summary?.refreshGrid?.();
        });
    });
})();
