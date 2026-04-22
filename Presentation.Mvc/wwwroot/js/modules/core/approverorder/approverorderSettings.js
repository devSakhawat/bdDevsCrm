(function () {
    'use strict';

    window.ApproverOrderModule = window.ApproverOrderModule || {};

    window.ApproverOrderModule.config = {
        moduleTitle: 'Approver Order',
        pluralTitle: 'Approver Orders',
        idField: 'approverOrderId',
        displayField: 'orderTitle',
        dom: {
            grid: '#approverOrderGrid',
            window: '#approverOrderWindow',
            form: '#approverOrderForm',
            addButton: '#btnAddApproverOrder',
            refreshButton: '#btnRefreshApproverOrder',
            saveButton: '#btnSaveApproverOrder',
            cancelButton: '#btnCancelApproverOrder'
        },
        apiEndpoints: {
            summary: `${window.AppConfig.apiBaseUrl}/core/systemadmin/approver-order-summary`,
            create: `${window.AppConfig.apiBaseUrl}/core/systemadmin/approver-order`,
            update: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/approver-order/${id}`; },
            delete: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/approver-order/${id}`; },
            read: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/approver-order/${id}`; },
            modules: `${window.AppConfig.apiBaseUrl}/core/systemadmin/modules-ddl`,
            approverTypes: `${window.AppConfig.apiBaseUrl}/core/systemadmin/approver-types-ddl`
        },
        gridOptions: {
            pageSize: 20,
            sortable: true,
            filterable: true,
            height: 560,
            pageable: { refresh: true, pageSizes: [10, 20, 50, 100], buttonCount: 5 }
        },
        windowOptions: { width: '820px' },
        grid: { columns: [{field: 'approverOrderId', title: 'ID', width: 90, dataType: 'number', filterable: false}, {field: 'orderTitle', title: 'Order Title', width: 220}, {field: 'moduleId', title: 'Module', width: 120, dataType: 'number'}, {field: 'approverTypeId', title: 'Approver Type', width: 140, dataType: 'number'}, {field: 'isEditable', title: 'Editable', width: 120, kind: 'boolean', dataType: 'boolean', trueText: 'Yes', falseText: 'No'}] },
        form: { fields: [{name: 'approverOrderId', label: 'Approver Order Id', type: 'hidden'}, {name: 'orderTitle', label: 'Order Title', type: 'text', required: true, maxLength: 200, placeholder: 'Enter order title'}, {name: 'moduleId', label: 'Module', type: 'select', required: true, dataSourceEndpoint: function () { return window.ApproverOrderModule.config.apiEndpoints.modules; }, dataTextField: 'moduleName', dataValueField: 'moduleId', optionLabel: 'Select Module...'}, {name: 'approverTypeId', label: 'Approver Type', type: 'select', required: true, dataSourceEndpoint: function () { return window.ApproverOrderModule.config.apiEndpoints.approverTypes; }, dataTextField: 'approverTypeName', dataValueField: 'approverTypeId', optionLabel: 'Select Approver Type...'}, {name: 'isEditable', label: 'Editable', type: 'checkbox', defaultValue: false}], buildPayload: function (values) {
                function toInteger(value) { const parsed = parseInt(value || 0, 10); return Number.isNaN(parsed) ? 0 : parsed; }
                return { approverOrderId: toInteger(values.approverOrderId), orderTitle: values.orderTitle ? String(values.orderTitle).trim() : null, moduleId: toInteger(values.moduleId), approverTypeId: toInteger(values.approverTypeId), isEditable: values.isEditable !== false };
            } }
    };

    window.ApproverOrderModule.config.moduleRef = window.ApproverOrderModule;
    window.ApproverOrderModule.config.form.fields.forEach(function (field) { field.moduleRef = window.ApproverOrderModule; });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () { window.location.href = '/Account/Login'; }, 1500);
            return;
        }

        window.ApproverOrderModule.Summary?.init?.();
        window.ApproverOrderModule.Details?.init?.();

        $(window.ApproverOrderModule.config.dom.addButton).on('click', function () { window.ApproverOrderModule.Details?.openAddForm?.(); });
        $(window.ApproverOrderModule.config.dom.refreshButton).on('click', function () { window.ApproverOrderModule.Summary?.refreshGrid?.(); });
    });
})();
