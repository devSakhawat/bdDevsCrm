(function () {
    'use strict';

    window.CounsellingTypeModule = window.CounsellingTypeModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.CounsellingTypeModule.config = {
        moduleTitle: 'Counselling Type',
        pluralTitle: 'Counselling Types',
        idField: 'counsellingTypeId',
        displayField: 'counsellingTypeName',
        dom: {
            grid: '#counsellingTypeGrid',
            window: '#counsellingTypeWindow',
            form: '#counsellingTypeForm',
            addButton: '#btnAddCounsellingType',
            refreshButton: '#btnRefreshCounsellingType',
            saveButton: '#btnSaveCounsellingType',
            cancelButton: '#btnCancelCounsellingType'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-counselling-type-summary`,
            create: `${apiRoot}/crm-counselling-type`,
            update: function (id) { return `${apiRoot}/crm-counselling-type/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-counselling-type/${id}`; },
            read: function (id) { return `${apiRoot}/crm-counselling-type/${id}`; }
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
            columns: [{ field: "counsellingTypeId", title: "ID", width: 90, dataType: "number", filterable: false }, { field: "counsellingTypeName", title: "Counselling Type Name", width: 260 }, { field: "isActive", title: "Status", width: 120, kind: "boolean", dataType: "boolean", trueText: "Active", falseText: "Inactive" }]
        },
        form: {
            fields: [{ name: "counsellingTypeId", label: "Counselling Type Id", type: "hidden" },
                { name: "counsellingTypeName", label: "Counselling Type Name", type: "text", required: true, maxLength: 100, placeholder: "Enter counselling type name" },
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
                    counsellingTypeId: toInteger(values.counsellingTypeId),
                    counsellingTypeName: (values.counsellingTypeName || '').trim(),
                    isActive: values.isActive !== false,
                    createdDate: state.currentRecord?.createdDate || now,
                    createdBy: state.currentRecord?.createdBy || 1,
                    updatedDate: state.isEditMode ? now : null,
                    updatedBy: state.isEditMode ? 1 : null
                };
            }
        }
    };

    window.CounsellingTypeModule.config.moduleRef = window.CounsellingTypeModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.CounsellingTypeModule.Summary?.init?.();
        window.CounsellingTypeModule.Details?.init?.();

        $(window.CounsellingTypeModule.config.dom.addButton).on('click', function () {
            window.CounsellingTypeModule.Details?.openAddForm?.();
        });

        $(window.CounsellingTypeModule.config.dom.refreshButton).on('click', function () {
            window.CounsellingTypeModule.Summary?.refreshGrid?.();
        });
    });
})();
