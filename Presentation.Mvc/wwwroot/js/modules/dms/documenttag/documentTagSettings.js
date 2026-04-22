(function () {
    'use strict';

    window.DocumentTagModule = window.DocumentTagModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.DocumentTagModule.config = {
        moduleTitle: 'Document Tag',
        pluralTitle: 'Document Tags',
        idField: 'tagId',
        displayField: 'name',
        dom: {
            grid: '#documentTagGrid',
            window: '#documentTagWindow',
            form: '#documentTagForm',
            addButton: '#btnAddDocumentTag',
            refreshButton: '#btnRefreshDocumentTag',
            saveButton: '#btnSaveDocumentTag',
            cancelButton: '#btnCancelDocumentTag'
        },
        apiEndpoints: {
            summary: `${apiRoot}/dms-document-tag-summary`,
            create: `${apiRoot}/dms-document-tag`,
            update: function (id) { return `${apiRoot}/dms-document-tag/${id}`; },
            delete: function (id) { return `${apiRoot}/dms-document-tag/${id}`; },
            read: function (id) { return `${apiRoot}/dms-document-tag/${id}`; }
        },
        gridOptions: {
            pageSize: 20,
            sortable: true,
            filterable: true,
            height: 560,
            pageable: { refresh: true, pageSizes: [10, 20, 50, 100], buttonCount: 5 }
        },
        windowOptions: { width: '520px' },
        grid: {
            columns: [
                { field: 'tagId', title: 'ID', width: 90, dataType: 'number', filterable: false },
                { field: 'name', title: 'Tag Name', width: 320 }
            ]
        },
        form: {
            fields: [
                { name: 'tagId', label: 'Tag Id', type: 'hidden' },
                { name: 'name', label: 'Tag Name', type: 'text', required: true, maxLength: 100, wide: true, placeholder: 'Enter document tag name' }
            ],
            buildPayload: function (values) {
                function toInteger(value) {
                    const parsed = parseInt(value || 0, 10);
                    return Number.isNaN(parsed) ? 0 : parsed;
                }

                return {
                    tagId: toInteger(values.tagId),
                    name: String(values.name || '').trim()
                };
            }
        }
    };

    window.DocumentTagModule.config.moduleRef = window.DocumentTagModule;
    window.DocumentTagModule.config.form.fields.forEach(function (field) { field.moduleRef = window.DocumentTagModule; });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () { window.location.href = '/Account/Login'; }, 1500);
            return;
        }

        window.DocumentTagModule.Summary?.init?.();
        window.DocumentTagModule.Details?.init?.();

        $(window.DocumentTagModule.config.dom.addButton).on('click', function () { window.DocumentTagModule.Details?.openAddForm?.(); });
        $(window.DocumentTagModule.config.dom.refreshButton).on('click', function () { window.DocumentTagModule.Summary?.refreshGrid?.(); });
    });
})();
