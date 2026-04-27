(function () {
    'use strict';

    window.NoteModule = window.NoteModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.NoteModule.config = {
        moduleTitle: 'Note',
        pluralTitle: 'Notes',
        idField: 'noteId',
        displayField: 'entityType',
        dom: {
            grid: '#noteGrid',
            window: '#noteWindow',
            form: '#noteForm',
            addButton: '#btnAddNote',
            refreshButton: '#btnRefreshNote',
            saveButton: '#btnSaveNote',
            cancelButton: '#btnCancelNote'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-note-summary`,
            create: `${apiRoot}/crm-note`,
            update: function (id) { return `${apiRoot}/crm-note/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-note/${id}`; },
            read: function (id) { return `${apiRoot}/crm-note/${id}`; }
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
            width: '600px'
        },
        grid: {
            columns: [{field: "noteId", title: "#", width: 60, dataType: "number", filterable: false}, {field: "entityType", title: "Entity Type", width: 150}, {field: "entityId", title: "Entity ID", width: 100, dataType: "number"}, {field: "noteDate", title: "Date", width: 130}, {field: "isPrivate", title: "Private", width: 120, dataType: "boolean", kind: "boolean", trueText: "Yes", falseText: "No"}]
        },
        form: {
            fields: [{name: "noteId", label: "Note Id", type: "hidden"}, {name: "entityType", label: "Entity Type", type: "text", maxLength: 100, placeholder: "Enter entity type"}, {name: "entityId", label: "Entity Id", type: "number", placeholder: "Enter entity id", min: 0}, {name: "noteText", label: "Note Text", type: "textarea", maxLength: 2000, wide: true, placeholder: "Enter note text"}, {name: "noteDate", label: "Note Date", type: "date"}, {name: "isPrivate", label: "Private", type: "checkbox", defaultValue: false}],
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
                    noteId: toInteger(values.noteId),
                    entityType: normalizeString(values.entityType),
                    entityId: toInteger(values.entityId),
                    noteText: normalizeString(values.noteText),
                    noteDate: normalizeString(values.noteDate),
                    isPrivate: !!values.isPrivate,
                    createdDate: state.currentRecord?.createdDate || now,
                    createdBy: state.currentRecord?.createdBy || 1,
                    updatedDate: state.isEditMode ? now : null,
                    updatedBy: state.isEditMode ? 1 : null
                };
            }
        }
    };

    window.NoteModule.config.moduleRef = window.NoteModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.NoteModule.Summary?.init?.();
        window.NoteModule.Details?.init?.();

        $(window.NoteModule.config.dom.addButton).on('click', function () {
            window.NoteModule.Details?.openAddForm?.();
        });

        $(window.NoteModule.config.dom.refreshButton).on('click', function () {
            window.NoteModule.Summary?.refreshGrid?.();
        });
    });
})();
