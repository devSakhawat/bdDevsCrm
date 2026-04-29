(function () {
    'use strict';

    window.CommissionTypeModule = window.CommissionTypeModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.CommissionTypeModule.config = {
        moduleTitle: 'Commission Type',
        pluralTitle: 'Commission Types',
        idField: 'commissionTypeId',
        displayField: 'commissionTypeName',
        dom: {
            grid: '#commissionTypeGrid',
            window: '#commissionTypeWindow',
            form: '#commissionTypeForm',
            addButton: '#btnAddCommissionType',
            refreshButton: '#btnRefreshCommissionType',
            saveButton: '#btnSaveCommissionType',
            cancelButton: '#btnCancelCommissionType'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-commission-type-summary`,
            create: `${apiRoot}/crm-commission-type`,
            update: function (id) { return `${apiRoot}/crm-commission-type/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-commission-type/${id}`; },
            read: function (id) { return `${apiRoot}/crm-commission-type/${id}`; }
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
            columns: [{ field: "commissionTypeId", title: "ID", width: 90, dataType: "number", filterable: false }, { field: "commissionTypeName", title: "Commission Type Name", width: 220 }, { field: "calculationMode", title: "Calculation Mode", width: 160 }, { field: "isActive", title: "Status", width: 120, kind: "boolean", dataType: "boolean", trueText: "Active", falseText: "Inactive" }]
        },
        form: {
            fields: [{ name: "commissionTypeId", label: "Commission Type Id", type: "hidden" },
                { name: "commissionTypeName", label: "Commission Type Name", type: "text", required: true, maxLength: 100, placeholder: "Enter commission type name" },
                { name: "calculationMode", label: "Calculation Mode", type: "text", maxLength: 50, placeholder: "Flat / Percentage" },
                { name: "isActive", label: "Active", type: "checkbox", defaultValue: true }],
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
                    commissionTypeId: toInteger(values.commissionTypeId),
                    commissionTypeName: (values.commissionTypeName || '').trim(),
                    calculationMode: normalizeString(values.calculationMode),
                    isActive: values.isActive !== false,
                    createdDate: state.currentRecord?.createdDate || now,
                    createdBy: state.currentRecord?.createdBy || 1,
                    updatedDate: state.isEditMode ? now : null,
                    updatedBy: state.isEditMode ? 1 : null
                };
            }
        }
    };

    window.CommissionTypeModule.config.moduleRef = window.CommissionTypeModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.CommissionTypeModule.Summary?.init?.();
        window.CommissionTypeModule.Details?.init?.();

        $(window.CommissionTypeModule.config.dom.addButton).on('click', function () {
            window.CommissionTypeModule.Details?.openAddForm?.();
        });

        $(window.CommissionTypeModule.config.dom.refreshButton).on('click', function () {
            window.CommissionTypeModule.Summary?.refreshGrid?.();
        });
    });
})();
