(function () {
    'use strict';

    window.AccessRestrictionModule = window.AccessRestrictionModule || {};

    window.AccessRestrictionModule.config = {
        moduleTitle: 'Access Restriction',
        pluralTitle: 'Access Restrictions',
        idField: 'accessRestrictionId',
        displayField: 'referenceId',
        dom: {
            grid: '#accessRestrictionGrid',
            window: '#accessRestrictionWindow',
            form: '#accessRestrictionForm',
            addButton: '#btnAddAccessRestriction',
            refreshButton: '#btnRefreshAccessRestriction',
            saveButton: '#btnSaveAccessRestriction',
            cancelButton: '#btnCancelAccessRestriction'
        },
        apiEndpoints: {
            summary: `${window.AppConfig.apiBaseUrl}/core/systemadmin/access-restriction-summary`,
            create: `${window.AppConfig.apiBaseUrl}/core/systemadmin/access-restriction`,
            update: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/access-restriction/${id}`; },
            delete: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/access-restriction/${id}`; },
            read: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/access-restriction/${id}`; }
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
            columns: [{field: 'accessRestrictionId', title: 'ID', width: 90, dataType: 'number', filterable: false}, {field: 'hrRecordId', title: 'HR Record', width: 130, dataType: 'number'}, {field: 'referenceId', title: 'Reference', width: 130, dataType: 'number'}, {field: 'referenceType', title: 'Reference Type', width: 150, dataType: 'number'}, {field: 'accessDate', title: 'Access Date', width: 160}, {field: 'accessBy', title: 'Access By', width: 120, dataType: 'number'}, {field: 'groupId', title: 'Group', width: 120, dataType: 'number'}]
        },
        form: {
            fields: [{name: 'accessRestrictionId', label: 'Access Restriction Id', type: 'hidden'}, {name: 'hrRecordId', label: 'HR Record Id', type: 'number', required: true, min: 1, placeholder: 'Enter HR record id'}, {name: 'referenceId', label: 'Reference Id', type: 'number', required: true, min: 1, placeholder: 'Enter reference id'}, {name: 'referenceType', label: 'Reference Type', type: 'number', required: true, min: 1, placeholder: 'Enter reference type'}, {name: 'accessDate', label: 'Access Date', type: 'date'}, {name: 'accessBy', label: 'Access By', type: 'number', min: 1, placeholder: 'Enter user id'}, {name: 'parentReference', label: 'Parent Reference', type: 'number', min: 0, placeholder: 'Enter parent reference'}, {name: 'chiledParentReference', label: 'Child Parent Reference', type: 'number', min: 0, placeholder: 'Enter child parent reference'}, {name: 'restrictionType', label: 'Restriction Type', type: 'number', min: 0, placeholder: 'Enter restriction type'}, {name: 'groupId', label: 'Group Id', type: 'number', min: 1, placeholder: 'Enter group id'}],
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

                function toDateOnly(value) {
                    return value ? String(value).slice(0, 10) : null;
                }

                return {
                    accessRestrictionId: toInteger(values.accessRestrictionId),
                    hrRecordId: toInteger(values.hrRecordId),
                    referenceId: toInteger(values.referenceId),
                    referenceType: toInteger(values.referenceType),
                    accessDate: toDateOnly(values.accessDate),
                    accessBy: toNullableInteger(values.accessBy),
                    parentReference: toNullableInteger(values.parentReference),
                    chiledParentReference: toNullableInteger(values.chiledParentReference),
                    restrictionType: toNullableInteger(values.restrictionType),
                    groupId: toNullableInteger(values.groupId)
                };
            }
        }
    };

    window.AccessRestrictionModule.config.moduleRef = window.AccessRestrictionModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.AccessRestrictionModule.Summary?.init?.();
        window.AccessRestrictionModule.Details?.init?.();

        $(window.AccessRestrictionModule.config.dom.addButton).on('click', function () {
            window.AccessRestrictionModule.Details?.openAddForm?.();
        });

        $(window.AccessRestrictionModule.config.dom.refreshButton).on('click', function () {
            window.AccessRestrictionModule.Summary?.refreshGrid?.();
        });
    });
})();
