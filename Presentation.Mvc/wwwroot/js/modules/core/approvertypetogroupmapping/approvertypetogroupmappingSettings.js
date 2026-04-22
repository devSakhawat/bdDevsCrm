(function () {
    'use strict';

    window.ApproverTypeToGroupMappingModule = window.ApproverTypeToGroupMappingModule || {};

    window.ApproverTypeToGroupMappingModule.config = {
        moduleTitle: 'Approver Type To Group Mapping',
        pluralTitle: 'Approver Type To Group Mappings',
        idField: 'approverTypeMapId',
        displayField: 'approverTypeMapId',
        dom: {
            grid: '#approverTypeToGroupMappingGrid',
            window: '#approverTypeToGroupMappingWindow',
            form: '#approverTypeToGroupMappingForm',
            addButton: '#btnAddApproverTypeToGroupMapping',
            refreshButton: '#btnRefreshApproverTypeToGroupMapping',
            saveButton: '#btnSaveApproverTypeToGroupMapping',
            cancelButton: '#btnCancelApproverTypeToGroupMapping'
        },
        apiEndpoints: {
            summary: `${window.AppConfig.apiBaseUrl}/core/systemadmin/approver-type-to-group-mapping-summary`,
            create: `${window.AppConfig.apiBaseUrl}/core/systemadmin/approver-type-to-group-mapping`,
            update: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/approver-type-to-group-mapping/${id}`; },
            delete: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/approver-type-to-group-mapping/${id}`; },
            read: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/approver-type-to-group-mapping/${id}`; },
            approverTypes: `${window.AppConfig.apiBaseUrl}/core/systemadmin/approver-types-ddl`,
            modules: `${window.AppConfig.apiBaseUrl}/core/systemadmin/modules-ddl`,
            groups: `${window.AppConfig.apiBaseUrl}/core/systemadmin/groups-ddl`
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
            columns: [{field: 'approverTypeMapId', title: 'ID', width: 90, dataType: 'number', filterable: false}, {field: 'approverTypeId', title: 'Approver Type', width: 140, dataType: 'number'}, {field: 'moduleId', title: 'Module', width: 120, dataType: 'number'}, {field: 'groupId', title: 'Group', width: 120, dataType: 'number'}]
        },
        form: {
            fields: [{name: 'approverTypeMapId', label: 'Approver Type Map Id', type: 'hidden'}, {name: 'approverTypeId', label: 'Approver Type', type: 'select', required: true, dataSourceEndpoint: function () { return window.ApproverTypeToGroupMappingModule.config.apiEndpoints.approverTypes; }, dataTextField: 'approverTypeName', dataValueField: 'approverTypeId', optionLabel: 'Select Approver Type...'}, {name: 'moduleId', label: 'Module', type: 'select', required: true, dataSourceEndpoint: function () { return window.ApproverTypeToGroupMappingModule.config.apiEndpoints.modules; }, dataTextField: 'moduleName', dataValueField: 'moduleId', optionLabel: 'Select Module...'}, {name: 'groupId', label: 'Group', type: 'select', required: true, dataSourceEndpoint: function () { return window.ApproverTypeToGroupMappingModule.config.apiEndpoints.groups; }, dataTextField: 'groupName', dataValueField: 'groupId', optionLabel: 'Select Group...'}],
            buildPayload: function (values) {
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

                return {
                    approverTypeMapId: toInteger(values.approverTypeMapId),
                    approverTypeId: toNullableInteger(values.approverTypeId),
                    moduleId: toNullableInteger(values.moduleId),
                    groupId: toNullableInteger(values.groupId)
                };
            }
        }
    };

    window.ApproverTypeToGroupMappingModule.config.moduleRef = window.ApproverTypeToGroupMappingModule;
    window.ApproverTypeToGroupMappingModule.config.form.fields.forEach(function (field) {
        field.moduleRef = window.ApproverTypeToGroupMappingModule;
    });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.ApproverTypeToGroupMappingModule.Summary?.init?.();
        window.ApproverTypeToGroupMappingModule.Details?.init?.();

        $(window.ApproverTypeToGroupMappingModule.config.dom.addButton).on('click', function () {
            window.ApproverTypeToGroupMappingModule.Details?.openAddForm?.();
        });

        $(window.ApproverTypeToGroupMappingModule.config.dom.refreshButton).on('click', function () {
            window.ApproverTypeToGroupMappingModule.Summary?.refreshGrid?.();
        });
    });
})();
