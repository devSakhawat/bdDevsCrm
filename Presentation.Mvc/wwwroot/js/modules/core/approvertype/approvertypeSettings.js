(function () {
    'use strict';

    window.ApproverTypeModule = window.ApproverTypeModule || {};

    window.ApproverTypeModule.config = {
        moduleTitle: 'Approver Type',
        pluralTitle: 'Approver Types',
        idField: 'approverTypeId',
        displayField: 'approverTypeName',
        dom: {
            grid: '#approverTypeGrid',
            window: '#approverTypeWindow',
            form: '#approverTypeForm',
            addButton: '#btnAddApproverType',
            refreshButton: '#btnRefreshApproverType',
            saveButton: '#btnSaveApproverType',
            cancelButton: '#btnCancelApproverType'
        },
        apiEndpoints: {
            summary: `${window.AppConfig.apiBaseUrl}/core/systemadmin/approver-type-summary`,
            create: `${window.AppConfig.apiBaseUrl}/core/systemadmin/approver-type`,
            update: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/approver-type/${id}`; },
            delete: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/approver-type/${id}`; },
            read: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/approver-type/${id}`; }
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
            columns: [{field: 'approverTypeId', title: 'ID', width: 90, dataType: 'number', filterable: false}, {field: 'approverTypeName', title: 'Approver Type Name', width: 280}]
        },
        form: {
            fields: [{name: 'approverTypeId', label: 'Approver Type Id', type: 'hidden'}, {name: 'approverTypeName', label: 'Approver Type Name', type: 'text', required: true, maxLength: 200, wide: true, placeholder: 'Enter approver type name'}],
            buildPayload: function (values) {
                function normalizeString(value) {
                    return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim();
                }

                function toInteger(value) {
                    const parsed = parseInt(value || 0, 10);
                    return Number.isNaN(parsed) ? 0 : parsed;
                }

                return {
                    approverTypeId: toInteger(values.approverTypeId),
                    approverTypeName: normalizeString(values.approverTypeName)
                };
            }
        }
    };

    window.ApproverTypeModule.config.moduleRef = window.ApproverTypeModule;
    window.ApproverTypeModule.config.form.fields.forEach(function (field) {
        field.moduleRef = window.ApproverTypeModule;
    });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.ApproverTypeModule.Summary?.init?.();
        window.ApproverTypeModule.Details?.init?.();

        $(window.ApproverTypeModule.config.dom.addButton).on('click', function () {
            window.ApproverTypeModule.Details?.openAddForm?.();
        });

        $(window.ApproverTypeModule.config.dom.refreshButton).on('click', function () {
            window.ApproverTypeModule.Summary?.refreshGrid?.();
        });
    });
})();
