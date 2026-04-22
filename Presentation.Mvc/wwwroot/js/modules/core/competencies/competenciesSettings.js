(function () {
    'use strict';

    window.CompetenciesModule = window.CompetenciesModule || {};

    window.CompetenciesModule.config = {
        moduleTitle: 'Competency',
        pluralTitle: 'Competencies',
        idField: 'id',
        displayField: 'competencyName',
        dom: {
            grid: '#competenciesGrid',
            window: '#competenciesWindow',
            form: '#competenciesForm',
            addButton: '#btnAddCompetency',
            refreshButton: '#btnRefreshCompetency',
            saveButton: '#btnSaveCompetency',
            cancelButton: '#btnCancelCompetency'
        },
        apiEndpoints: {
            summary: `${window.AppConfig.apiBaseUrl}/core/systemadmin/competencies-summary`,
            create: `${window.AppConfig.apiBaseUrl}/core/systemadmin/competency`,
            update: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/competency/${id}`; },
            delete: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/competency/${id}`; },
            read: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/competency/${id}`; }
        },
        gridOptions: {
            pageSize: 20,
            sortable: true,
            filterable: true,
            height: 560,
            pageable: { refresh: true, pageSizes: [10, 20, 50, 100], buttonCount: 5 }
        },
        windowOptions: { width: '820px' },
        grid: { columns: [{field: 'id', title: 'ID', width: 90, dataType: 'number', filterable: false}, {field: 'competencyName', title: 'Competency Name', width: 220}, {field: 'competencyType', title: 'Type', width: 140, dataType: 'number'}, {field: 'isDepartmentHead', title: 'Department Head', width: 140, kind: 'boolean', dataType: 'boolean', trueText: 'Yes', falseText: 'No'}, {field: 'isActive', title: 'Status', width: 120, kind: 'boolean', dataType: 'boolean', trueText: 'Active', falseText: 'Inactive'}] },
        form: { fields: [{name: 'id', label: 'Competency Id', type: 'hidden'}, {name: 'competencyName', label: 'Competency Name', type: 'text', required: true, maxLength: 200, placeholder: 'Enter competency name'}, {name: 'competencyType', label: 'Competency Type', type: 'number', min: 0, placeholder: 'Enter competency type'}, {name: 'isDepartmentHead', label: 'Department Head', type: 'checkbox', defaultValue: false}, {name: 'isActive', label: 'Active', type: 'checkbox', defaultValue: true}], buildPayload: function (values) {
                function toInteger(value) { const parsed = parseInt(value || 0, 10); return Number.isNaN(parsed) ? 0 : parsed; }
                function toNullableInteger(value) { if (value === null || value === undefined || value === '') { return null; } const parsed = parseInt(value, 10); return Number.isNaN(parsed) ? null : parsed; }
                function toBit(value) { return value ? 1 : 0; }
                function normalizeString(value) { return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim(); }
                return { id: toInteger(values.id), competencyName: normalizeString(values.competencyName), competencyType: toNullableInteger(values.competencyType), isDepartmentHead: toBit(values.isDepartmentHead), isActive: toBit(values.isActive !== false) };
            } }
    };

    window.CompetenciesModule.config.moduleRef = window.CompetenciesModule;
    window.CompetenciesModule.config.form.fields.forEach(function (field) { field.moduleRef = window.CompetenciesModule; });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () { window.location.href = '/Account/Login'; }, 1500);
            return;
        }

        window.CompetenciesModule.Summary?.init?.();
        window.CompetenciesModule.Details?.init?.();

        $(window.CompetenciesModule.config.dom.addButton).on('click', function () { window.CompetenciesModule.Details?.openAddForm?.(); });
        $(window.CompetenciesModule.config.dom.refreshButton).on('click', function () { window.CompetenciesModule.Summary?.refreshGrid?.(); });
    });
})();
