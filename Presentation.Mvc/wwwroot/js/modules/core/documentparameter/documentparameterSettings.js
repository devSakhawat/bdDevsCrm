(function () {
    'use strict';

    window.DocumentParameterModule = window.DocumentParameterModule || {};

    window.DocumentParameterModule.config = {
        moduleTitle: 'Document Parameter',
        pluralTitle: 'Document Parameters',
        idField: 'parameterId',
        displayField: 'parameterName',
        dom: {
            grid: '#documentParameterGrid',
            window: '#documentParameterWindow',
            form: '#documentParameterForm',
            addButton: '#btnAddDocumentParameter',
            refreshButton: '#btnRefreshDocumentParameter',
            saveButton: '#btnSaveDocumentParameter',
            cancelButton: '#btnCancelDocumentParameter'
        },
        apiEndpoints: {
            summary: `${window.AppConfig.apiBaseUrl}/core/systemadmin/document-parameter-summary`,
            create: `${window.AppConfig.apiBaseUrl}/core/systemadmin/document-parameter`,
            update: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/document-parameter/${id}`; },
            delete: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/document-parameter/${id}`; },
            read: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/document-parameter/${id}`; }
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
            columns: [{field: 'parameterId', title: 'ID', width: 90, dataType: 'number', filterable: false}, {field: 'parameterName', title: 'Parameter Name', width: 220}, {field: 'parameterKey', title: 'Parameter Key', width: 180}, {field: 'controlRole', title: 'Control Role', width: 160}, {field: 'controlSequence', title: 'Sequence', width: 110, dataType: 'number'}]
        },
        form: {
            fields: [{name: 'parameterId', label: 'Parameter Id', type: 'hidden'}, {name: 'parameterName', label: 'Parameter Name', type: 'text', required: true, maxLength: 200, placeholder: 'Enter parameter name'}, {name: 'parameterKey', label: 'Parameter Key', type: 'text', required: true, maxLength: 200, placeholder: 'Enter parameter key'}, {name: 'controlRole', label: 'Control Role', type: 'text', maxLength: 100, placeholder: 'Enter control role'}, {name: 'dataSource', label: 'Data Source', type: 'text', maxLength: 255, wide: true, placeholder: 'Enter data source'}, {name: 'controlSequence', label: 'Control Sequence', type: 'number', min: 0, placeholder: 'Enter sequence'}, {name: 'dataTextField', label: 'Data Text Field', type: 'text', maxLength: 100, placeholder: 'Enter text field name'}, {name: 'dataValueField', label: 'Data Value Field', type: 'text', maxLength: 100, placeholder: 'Enter value field name'}, {name: 'caseCading', label: 'Cascading', type: 'text', maxLength: 100, placeholder: 'Enter cascading behavior'}],
            buildPayload: function (values) {
                function normalizeString(value) {
                    return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim();
                }

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
                    parameterId: toInteger(values.parameterId),
                    parameterName: normalizeString(values.parameterName),
                    parameterKey: normalizeString(values.parameterKey),
                    controlRole: normalizeString(values.controlRole),
                    dataSource: normalizeString(values.dataSource),
                    controlSequence: toNullableInteger(values.controlSequence),
                    dataTextField: normalizeString(values.dataTextField),
                    dataValueField: normalizeString(values.dataValueField),
                    caseCading: normalizeString(values.caseCading)
                };
            }
        }
    };

    window.DocumentParameterModule.config.moduleRef = window.DocumentParameterModule;
    window.DocumentParameterModule.config.form.fields.forEach(function (field) {
        field.moduleRef = window.DocumentParameterModule;
    });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.DocumentParameterModule.Summary?.init?.();
        window.DocumentParameterModule.Details?.init?.();

        $(window.DocumentParameterModule.config.dom.addButton).on('click', function () {
            window.DocumentParameterModule.Details?.openAddForm?.();
        });

        $(window.DocumentParameterModule.config.dom.refreshButton).on('click', function () {
            window.DocumentParameterModule.Summary?.refreshGrid?.();
        });
    });
})();
