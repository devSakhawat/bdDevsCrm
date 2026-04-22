(function () {
    'use strict';

    window.CompetencyLevelModule = window.CompetencyLevelModule || {};

    window.CompetencyLevelModule.config = {
        moduleTitle: 'Competency Level',
        pluralTitle: 'Competency Levels',
        idField: 'levelId',
        displayField: 'levelTitle',
        dom: {
            grid: '#competencyLevelGrid',
            window: '#competencyLevelWindow',
            form: '#competencyLevelForm',
            addButton: '#btnAddCompetencyLevel',
            refreshButton: '#btnRefreshCompetencyLevel',
            saveButton: '#btnSaveCompetencyLevel',
            cancelButton: '#btnCancelCompetencyLevel'
        },
        apiEndpoints: {
            summary: `${window.AppConfig.apiBaseUrl}/core/systemadmin/competency-level-summary`,
            create: `${window.AppConfig.apiBaseUrl}/core/systemadmin/competency-level`,
            update: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/competency-level/${id}`; },
            delete: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/competency-level/${id}`; },
            read: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/competency-level/${id}`; }
        },
        gridOptions: {
            pageSize: 20,
            sortable: true,
            filterable: true,
            height: 560,
            pageable: { refresh: true, pageSizes: [10, 20, 50, 100], buttonCount: 5 }
        },
        windowOptions: { width: '820px' },
        grid: { columns: [{field: 'levelId', title: 'ID', width: 90, dataType: 'number', filterable: false}, {field: 'levelTitle', title: 'Level Title', width: 220}, {field: 'remarks', title: 'Remarks', width: 280}] },
        form: { fields: [{name: 'levelId', label: 'Level Id', type: 'hidden'}, {name: 'levelTitle', label: 'Level Title', type: 'text', required: true, maxLength: 150, placeholder: 'Enter level title'}, {name: 'remarks', label: 'Remarks', type: 'textarea', wide: true, maxLength: 1000, placeholder: 'Enter remarks'}], buildPayload: function (values) {
                function toInteger(value) { const parsed = parseInt(value || 0, 10); return Number.isNaN(parsed) ? 0 : parsed; }
                function normalizeString(value) { return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim(); }
                return { levelId: toInteger(values.levelId), levelTitle: normalizeString(values.levelTitle), remarks: normalizeString(values.remarks) };
            } }
    };

    window.CompetencyLevelModule.config.moduleRef = window.CompetencyLevelModule;
    window.CompetencyLevelModule.config.form.fields.forEach(function (field) { field.moduleRef = window.CompetencyLevelModule; });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () { window.location.href = '/Account/Login'; }, 1500);
            return;
        }

        window.CompetencyLevelModule.Summary?.init?.();
        window.CompetencyLevelModule.Details?.init?.();

        $(window.CompetencyLevelModule.config.dom.addButton).on('click', function () { window.CompetencyLevelModule.Details?.openAddForm?.(); });
        $(window.CompetencyLevelModule.config.dom.refreshButton).on('click', function () { window.CompetencyLevelModule.Summary?.refreshGrid?.(); });
    });
})();
