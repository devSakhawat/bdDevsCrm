(function () {
    'use strict';

    window.FileUpdateHistoryModule = window.FileUpdateHistoryModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.FileUpdateHistoryModule.config = {
        moduleTitle: 'File Update History',
        pluralTitle: 'File Update Histories',
        idField: 'id',
        displayField: 'entityId',
        dom: {
            grid: '#fileUpdateHistoryGrid',
            window: '#fileUpdateHistoryWindow',
            form: '#fileUpdateHistoryForm',
            addButton: '#btnAddFileUpdateHistory',
            refreshButton: '#btnRefreshFileUpdateHistory',
            saveButton: '#btnSaveFileUpdateHistory',
            cancelButton: '#btnCancelFileUpdateHistory'
        },
        apiEndpoints: {
            summary: `${apiRoot}/dms-file-update-history-summary`,
            create: `${apiRoot}/dms-file-update-history`,
            update: function (id) { return `${apiRoot}/dms-file-update-history/${id}`; },
            delete: function (id) { return `${apiRoot}/dms-file-update-history/${id}`; },
            read: function (id) { return `${apiRoot}/dms-file-update-history/${id}`; }
        },
        gridOptions: {
            pageSize: 20,
            sortable: true,
            filterable: true,
            height: 560,
            pageable: { refresh: true, pageSizes: [10, 20, 50, 100], buttonCount: 5 }
        },
        windowOptions: { width: '1024px' },
        grid: {
            columns: [
                { field: 'id', title: 'ID', width: 90, dataType: 'number', filterable: false },
                { field: 'entityId', title: 'Entity Id', width: 120 },
                { field: 'entityType', title: 'Entity Type', width: 160 },
                { field: 'documentType', title: 'Document Type', width: 160 },
                { field: 'versionNumber', title: 'Version', width: 110, dataType: 'number' },
                { field: 'updatedBy', title: 'Updated By', width: 150 },
                { field: 'updatedDate', title: 'Updated Date', width: 160 },
                { field: 'updateReason', title: 'Update Reason', width: 220 }
            ]
        },
        form: {
            fields: [
                { name: 'id', label: 'Id', type: 'hidden' },
                { name: 'entityId', label: 'Entity Id', type: 'text', required: true, maxLength: 50, placeholder: 'Enter entity id' },
                { name: 'entityType', label: 'Entity Type', type: 'text', required: true, maxLength: 100, placeholder: 'Enter entity type' },
                { name: 'documentType', label: 'Document Type', type: 'text', maxLength: 100, placeholder: 'Enter document type' },
                { name: 'versionNumber', label: 'Version Number', type: 'number', min: 1, placeholder: 'Enter version number' },
                { name: 'updatedBy', label: 'Updated By', type: 'text', maxLength: 250, placeholder: 'Enter updated by' },
                { name: 'updatedDate', label: 'Updated Date', type: 'date', readonly: true },
                { name: 'oldFilePath', label: 'Old File Path', type: 'textarea', wide: true, maxLength: 250, placeholder: 'Enter old file path' },
                { name: 'newFilePath', label: 'New File Path', type: 'textarea', wide: true, maxLength: 250, placeholder: 'Enter new file path' },
                { name: 'updateReason', label: 'Update Reason', type: 'textarea', wide: true, maxLength: 250, placeholder: 'Enter update reason' },
                { name: 'notes', label: 'Notes', type: 'textarea', wide: true, maxLength: 250, placeholder: 'Enter notes' }
            ],
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

                function normalizeString(value) {
                    return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim();
                }

                return {
                    id: toInteger(values.id),
                    entityId: String(values.entityId || '').trim(),
                    entityType: String(values.entityType || '').trim(),
                    documentType: normalizeString(values.documentType),
                    oldFilePath: normalizeString(values.oldFilePath),
                    newFilePath: normalizeString(values.newFilePath),
                    versionNumber: toNullableInteger(values.versionNumber),
                    updatedBy: normalizeString(values.updatedBy),
                    updateReason: normalizeString(values.updateReason),
                    notes: normalizeString(values.notes)
                };
            }
        }
    };

    window.FileUpdateHistoryModule.config.moduleRef = window.FileUpdateHistoryModule;
    window.FileUpdateHistoryModule.config.form.fields.forEach(function (field) { field.moduleRef = window.FileUpdateHistoryModule; });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () { window.location.href = '/Account/Login'; }, 1500);
            return;
        }

        window.FileUpdateHistoryModule.Summary?.init?.();
        window.FileUpdateHistoryModule.Details?.init?.();

        $(window.FileUpdateHistoryModule.config.dom.addButton).on('click', function () { window.FileUpdateHistoryModule.Details?.openAddForm?.(); });
        $(window.FileUpdateHistoryModule.config.dom.refreshButton).on('click', function () { window.FileUpdateHistoryModule.Summary?.refreshGrid?.(); });
    });
})();
