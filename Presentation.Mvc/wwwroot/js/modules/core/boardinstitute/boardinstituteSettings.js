(function () {
    'use strict';

    window.BoardInstituteModule = window.BoardInstituteModule || {};

    window.BoardInstituteModule.config = {
        moduleTitle: 'Board Institute',
        pluralTitle: 'Board Institutes',
        idField: 'id',
        displayField: 'instituteName',
        dom: {
            grid: '#boardInstituteGrid',
            window: '#boardInstituteWindow',
            form: '#boardInstituteForm',
            addButton: '#btnAddBoardInstitute',
            refreshButton: '#btnRefreshBoardInstitute',
            saveButton: '#btnSaveBoardInstitute',
            cancelButton: '#btnCancelBoardInstitute'
        },
        apiEndpoints: {
            summary: `${window.AppConfig.apiBaseUrl}/core/systemadmin/board-institute-summary`,
            create: `${window.AppConfig.apiBaseUrl}/core/systemadmin/board-institute`,
            update: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/board-institute/${id}`; },
            delete: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/board-institute/${id}`; },
            read: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/board-institute/${id}`; }
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
            columns: [{field: 'id', title: 'ID', width: 90, dataType: 'number', filterable: false}, {field: 'instituteName', title: 'Institute Name', width: 280}, {field: 'isActive', title: 'Status', width: 120, kind: 'boolean', dataType: 'boolean', trueText: 'Active', falseText: 'Inactive'}]
        },
        form: {
            fields: [{name: 'id', label: 'Board Institute Id', type: 'hidden'}, {name: 'instituteName', label: 'Institute Name', type: 'text', required: true, maxLength: 200, wide: true, placeholder: 'Enter institute name'}, {name: 'isActive', label: 'Active', type: 'checkbox', defaultValue: true}],
            buildPayload: function (values) {
                function normalizeString(value) {
                    return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim();
                }

                function toInteger(value) {
                    const parsed = parseInt(value || 0, 10);
                    return Number.isNaN(parsed) ? 0 : parsed;
                }

                return {
                    boardInstituteId: toInteger(values.id),
                    boardInstituteName: normalizeString(values.instituteName),
                    isActive: values.isActive !== false ? 1 : 0
                };
            }
        }
    };

    window.BoardInstituteModule.config.moduleRef = window.BoardInstituteModule;
    window.BoardInstituteModule.config.form.fields.forEach(function (field) {
        field.moduleRef = window.BoardInstituteModule;
    });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.BoardInstituteModule.Summary?.init?.();
        window.BoardInstituteModule.Details?.init?.();

        $(window.BoardInstituteModule.config.dom.addButton).on('click', function () {
            window.BoardInstituteModule.Details?.openAddForm?.();
        });

        $(window.BoardInstituteModule.config.dom.refreshButton).on('click', function () {
            window.BoardInstituteModule.Summary?.refreshGrid?.();
        });
    });
})();
