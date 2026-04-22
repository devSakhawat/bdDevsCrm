(function () {
    'use strict';

    window.AssignApproverModule = window.AssignApproverModule || {};

    window.AssignApproverModule.config = {
        moduleTitle: 'Assign Approver',
        pluralTitle: 'Assign Approvers',
        idField: 'assignApproverId',
        displayField: 'approverId',
        dom: {
            grid: '#assignApproverGrid',
            window: '#assignApproverWindow',
            form: '#assignApproverForm',
            addButton: '#btnAddAssignApprover',
            refreshButton: '#btnRefreshAssignApprover',
            saveButton: '#btnSaveAssignApprover',
            cancelButton: '#btnCancelAssignApprover'
        },
        apiEndpoints: {
            summary: `${window.AppConfig.apiBaseUrl}/core/systemadmin/assign-approver-summary`,
            create: `${window.AppConfig.apiBaseUrl}/core/systemadmin/assign-approver`,
            update: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/assign-approver/${id}`; },
            delete: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/assign-approver/${id}`; },
            read: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/assign-approver/${id}`; },
            modules: `${window.AppConfig.apiBaseUrl}/core/systemadmin/modules-ddl`
        },
        gridOptions: {
            pageSize: 20,
            sortable: true,
            filterable: true,
            height: 560,
            pageable: { refresh: true, pageSizes: [10, 20, 50, 100], buttonCount: 5 }
        },
        windowOptions: { width: '820px' },
        grid: { columns: [{field: 'assignApproverId', title: 'ID', width: 90, dataType: 'number', filterable: false}, {field: 'approverId', title: 'Approver Id', width: 120, dataType: 'number'}, {field: 'hrRecordId', title: 'HR Record Id', width: 120, dataType: 'number'}, {field: 'moduleId', title: 'Module', width: 120, dataType: 'number'}, {field: 'type', title: 'Type', width: 100, dataType: 'number'}, {field: 'sortOrder', title: 'Sort Order', width: 120, dataType: 'number'}, {field: 'isActive', title: 'Status', width: 120, kind: 'boolean', dataType: 'boolean', trueText: 'Active', falseText: 'Inactive'}] },
        form: { fields: [{name: 'assignApproverId', label: 'Assign Approver Id', type: 'hidden'}, {name: 'approverId', label: 'Approver Id', type: 'number', required: true, min: 1, placeholder: 'Enter approver id'}, {name: 'hrRecordId', label: 'HR Record Id', type: 'number', required: true, min: 1, placeholder: 'Enter HR record id'}, {name: 'moduleId', label: 'Module', type: 'select', required: true, dataSourceEndpoint: function () { return window.AssignApproverModule.config.apiEndpoints.modules; }, dataTextField: 'moduleName', dataValueField: 'moduleId', optionLabel: 'Select Module...'}, {name: 'type', label: 'Type', type: 'number', required: true, min: 0, placeholder: 'Enter type'}, {name: 'isNew', label: 'Is New', type: 'checkbox', defaultValue: true}, {name: 'sortOrder', label: 'Sort Order', type: 'number', min: 0, placeholder: 'Enter sort order'}, {name: 'isActive', label: 'Active', type: 'checkbox', defaultValue: true}], buildPayload: function (values, state) {
                function toInteger(value) { const parsed = parseInt(value || 0, 10); return Number.isNaN(parsed) ? 0 : parsed; }
                function toNullableInteger(value) { if (value === null || value === undefined || value === '') { return null; } const parsed = parseInt(value, 10); return Number.isNaN(parsed) ? null : parsed; }
                return { assignApproverId: toInteger(values.assignApproverId), approverId: toInteger(values.approverId), hrRecordId: toInteger(values.hrRecordId), moduleId: toInteger(values.moduleId), type: toInteger(values.type), isNew: values.isNew ? 1 : 0, sortOrder: toNullableInteger(values.sortOrder), isActive: values.isActive !== false, createdBy: state.currentRecord?.createdBy ?? null, createdDate: state.currentRecord?.createdDate ?? null, updatedBy: state.currentRecord?.updatedBy ?? null, updatedDate: state.isEditMode ? new Date().toISOString() : null };
            } }
    };

    window.AssignApproverModule.config.moduleRef = window.AssignApproverModule;
    window.AssignApproverModule.config.form.fields.forEach(function (field) { field.moduleRef = window.AssignApproverModule; });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () { window.location.href = '/Account/Login'; }, 1500);
            return;
        }

        window.AssignApproverModule.Summary?.init?.();
        window.AssignApproverModule.Details?.init?.();

        $(window.AssignApproverModule.config.dom.addButton).on('click', function () { window.AssignApproverModule.Details?.openAddForm?.(); });
        $(window.AssignApproverModule.config.dom.refreshButton).on('click', function () { window.AssignApproverModule.Summary?.refreshGrid?.(); });
    });
})();
