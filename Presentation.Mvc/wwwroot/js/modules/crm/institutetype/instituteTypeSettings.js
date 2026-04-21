(function () {
    'use strict';

    window.InstituteTypeModule = window.InstituteTypeModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.InstituteTypeModule.config = {
        moduleTitle: 'Institute Type',
        pluralTitle: 'Institute Types',
        idField: 'instituteTypeId',
        displayField: 'instituteTypeName',
        dom: {
            grid: '#instituteTypeGrid',
            window: '#instituteTypeWindow',
            form: '#instituteTypeForm',
            addButton: '#btnAddInstituteType',
            refreshButton: '#btnRefreshInstituteType',
            saveButton: '#btnSaveInstituteType',
            cancelButton: '#btnCancelInstituteType'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-institute-type-summary`,
            create: `${apiRoot}/crm-institute-type`,
            update: function (id) { return `${apiRoot}/crm-institute-type/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-institute-type/${id}`; },
            read: function (id) { return `${apiRoot}/crm-institute-type/${id}`; }
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
            columns: [{field: "instituteTypeId", title: "ID", width: 90, dataType: "number", filterable: false}, {field: "instituteTypeName", title: "Institute Type Name", width: 260}]
        },
        form: {
            fields: [{name: "instituteTypeId", label: "Institute Type Id", type: "hidden"}, {name: "instituteTypeName", label: "Institute Type Name", type: "text", required: true, maxLength: 100, wide: true, placeholder: "Enter institute type name"}],
            buildPayload: function (values, state) {
                function normalizeString(value) {
                    return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim();
                }

                function toInteger(value) {
                    const parsed = parseInt(value || 0, 10);
                    return Number.isNaN(parsed) ? 0 : parsed;
                }

                return {
                                instituteTypeId: toInteger(values.instituteTypeId),
                                instituteTypeName: (values.instituteTypeName || '').trim()
                            };
            }
        }
    };

    window.InstituteTypeModule.config.moduleRef = window.InstituteTypeModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.InstituteTypeModule.Summary?.init?.();
        window.InstituteTypeModule.Details?.init?.();

        $(window.InstituteTypeModule.config.dom.addButton).on('click', function () {
            window.InstituteTypeModule.Details?.openAddForm?.();
        });

        $(window.InstituteTypeModule.config.dom.refreshButton).on('click', function () {
            window.InstituteTypeModule.Summary?.refreshGrid?.();
        });
    });
})();
