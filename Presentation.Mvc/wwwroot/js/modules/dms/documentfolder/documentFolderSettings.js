(function () {
    'use strict';

    window.DocumentFolderModule = window.DocumentFolderModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.DocumentFolderModule.config = {
        moduleTitle: 'Document Folder',
        pluralTitle: 'Document Folders',
        idField: 'folderId',
        displayField: 'folderName',
        dom: {
            grid: '#documentFolderGrid',
            window: '#documentFolderWindow',
            form: '#documentFolderForm',
            addButton: '#btnAddDocumentFolder',
            refreshButton: '#btnRefreshDocumentFolder',
            saveButton: '#btnSaveDocumentFolder',
            cancelButton: '#btnCancelDocumentFolder'
        },
        apiEndpoints: {
            summary: `${apiRoot}/dms-document-folder-summary`,
            create: `${apiRoot}/dms-document-folder`,
            update: function (id) { return `${apiRoot}/dms-document-folder/${id}`; },
            delete: function (id) { return `${apiRoot}/dms-document-folder/${id}`; },
            read: function (id) { return `${apiRoot}/dms-document-folder/${id}`; },
            folders: `${apiRoot}/dms-document-folder-ddl`
        },
        gridOptions: {
            pageSize: 20,
            sortable: true,
            filterable: true,
            height: 560,
            pageable: { refresh: true, pageSizes: [10, 20, 50, 100], buttonCount: 5 }
        },
        windowOptions: { width: '860px' },
        grid: {
            columns: [
                { field: 'folderId', title: 'ID', width: 90, dataType: 'number', filterable: false },
                { field: 'folderName', title: 'Folder Name', width: 220 },
                { field: 'parentFolderId', title: 'Parent Folder Id', width: 140, dataType: 'number' },
                { field: 'ownerUserId', title: 'Owner User Id', width: 140, dataType: 'number' },
                { field: 'referenceEntityType', title: 'Reference Entity Type', width: 180 },
                { field: 'referenceEntityId', title: 'Reference Entity Id', width: 180 }
            ]
        },
        form: {
            fields: [
                { name: 'folderId', label: 'Folder Id', type: 'hidden' },
                {
                    name: 'parentFolderId',
                    label: 'Parent Folder',
                    type: 'select',
                    dataSourceEndpoint: function () { return window.DocumentFolderModule.config.apiEndpoints.folders; },
                    dataTextField: 'folderName',
                    dataValueField: 'folderId',
                    optionLabel: 'Select Parent Folder...'
                },
                { name: 'folderName', label: 'Folder Name', type: 'text', required: true, maxLength: 100, placeholder: 'Enter folder name' },
                { name: 'ownerUserId', label: 'Owner User Id', type: 'number', min: 0, placeholder: 'Enter owner user id' },
                { name: 'referenceEntityType', label: 'Reference Entity Type', type: 'text', maxLength: 250, placeholder: 'Enter reference entity type' },
                { name: 'referenceEntityId', label: 'Reference Entity Id', type: 'text', maxLength: 250, placeholder: 'Enter reference entity id' }
            ],
            buildPayload: function (values) {
                function toInteger(value, fallback) {
                    const parsed = parseInt(value ?? fallback ?? 0, 10);
                    return Number.isNaN(parsed) ? (fallback ?? 0) : parsed;
                }

                function toNullableInteger(value) {
                    if (value === null || value === undefined || value === '') {
                        return null;
                    }

                    const parsed = parseInt(value, 10);
                    return Number.isNaN(parsed) ? null : parsed;
                }

                function normalizeString(value) {
                    return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim();
                }

                return {
                    folderId: toInteger(values.folderId, 0),
                    parentFolderId: toNullableInteger(values.parentFolderId),
                    folderName: String(values.folderName || '').trim(),
                    ownerUserId: toInteger(values.ownerUserId, 0),
                    referenceEntityType: normalizeString(values.referenceEntityType),
                    referenceEntityId: normalizeString(values.referenceEntityId)
                };
            }
        }
    };

    window.DocumentFolderModule.config.moduleRef = window.DocumentFolderModule;
    window.DocumentFolderModule.config.form.fields.forEach(function (field) { field.moduleRef = window.DocumentFolderModule; });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () { window.location.href = '/Account/Login'; }, 1500);
            return;
        }

        window.DocumentFolderModule.Summary?.init?.();
        window.DocumentFolderModule.Details?.init?.();

        $(window.DocumentFolderModule.config.dom.addButton).on('click', function () { window.DocumentFolderModule.Details?.openAddForm?.(); });
        $(window.DocumentFolderModule.config.dom.refreshButton).on('click', function () { window.DocumentFolderModule.Summary?.refreshGrid?.(); });
    });
})();
