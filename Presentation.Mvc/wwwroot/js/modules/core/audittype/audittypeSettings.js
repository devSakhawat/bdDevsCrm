(function () {
    'use strict';

    window.AuditTypeModule = window.AuditTypeModule || {};

    window.AuditTypeModule.config = {
        moduleTitle: 'Audit Type',
        pluralTitle: 'Audit Types',
        idField: 'auditTypeId',
        displayField: 'auditType1',
        dom: {
            grid: '#auditTypeGrid',
            window: '#auditTypeWindow',
            form: '#auditTypeForm',
            addButton: '#btnAddAuditType',
            refreshButton: '#btnRefreshAuditType',
            saveButton: '#btnSaveAuditType',
            cancelButton: '#btnCancelAuditType'
        },
        apiEndpoints: {
            summary: `${window.AppConfig.apiBaseUrl}/core/systemadmin/audit-type-summary`,
            create: `${window.AppConfig.apiBaseUrl}/core/systemadmin/audit-type`,
            update: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/audit-type/${id}`; },
            delete: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/audit-type/${id}`; },
            read: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/audit-type/${id}`; }
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
            columns: [{field: 'auditTypeId', title: 'ID', width: 90, dataType: 'number', filterable: false}, {field: 'auditType1', title: 'Audit Type', width: 260}]
        },
        form: {
            fields: [{name: 'auditTypeId', label: 'Audit Type Id', type: 'hidden'}, {name: 'auditType1', label: 'Audit Type', type: 'text', required: true, maxLength: 255, wide: true, placeholder: 'Enter audit type name'}],
            buildPayload: function (values) {
                function normalizeString(value) {
                    return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim();
                }

                function toNullableInteger(value) {
                    if (value === null || value === undefined || value === '') {
                        return null;
                    }
                    const parsed = parseInt(value, 10);
                    return Number.isNaN(parsed) ? null : parsed;
                }

                return {
                    auditTypeId: toNullableInteger(values.auditTypeId),
                    auditType1: normalizeString(values.auditType1)
                };
            }
        }
    };

    window.AuditTypeModule.config.moduleRef = window.AuditTypeModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.AuditTypeModule.Summary?.init?.();
        window.AuditTypeModule.Details?.init?.();

        $(window.AuditTypeModule.config.dom.addButton).on('click', function () {
            window.AuditTypeModule.Details?.openAddForm?.();
        });

        $(window.AuditTypeModule.config.dom.refreshButton).on('click', function () {
            window.AuditTypeModule.Summary?.refreshGrid?.();
        });
    });
})();
