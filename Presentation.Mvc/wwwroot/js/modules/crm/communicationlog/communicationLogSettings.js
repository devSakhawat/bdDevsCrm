(function () {
    'use strict';

    window.CommunicationLogModule = window.CommunicationLogModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.CommunicationLogModule.config = {
        moduleTitle: 'Communication Log',
        pluralTitle: 'Communication Logs',
        idField: 'communicationLogId',
        displayField: 'subject',
        dom: {
            grid: '#communicationLogGrid',
            window: '#communicationLogWindow',
            form: '#communicationLogForm',
            addButton: '#btnAddCommunicationLog',
            refreshButton: '#btnRefreshCommunicationLog',
            saveButton: '#btnSaveCommunicationLog',
            cancelButton: '#btnCancelCommunicationLog'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-communication-log-summary`,
            create: `${apiRoot}/crm-communication-log`,
            update: function (id) { return `${apiRoot}/crm-communication-log/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-communication-log/${id}`; },
            read: function (id) { return `${apiRoot}/crm-communication-log/${id}`; }
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
            width: '860px'
        },
        grid: {
            columns: [
                { field: 'communicationLogId', title: '#', width: 60, dataType: 'number', filterable: false },
                {
                    field: 'entityType', title: 'Entity Type', width: 110,
                    kind: 'enum',
                    enumMap: { 1: 'Lead', 2: 'Student', 3: 'Application' }
                },
                { field: 'entityId', title: 'Entity ID', width: 90, dataType: 'number' },
                {
                    field: 'communicationType', title: 'Type', width: 120,
                    kind: 'enum',
                    enumMap: { 1: 'Call', 2: 'WhatsApp', 3: 'Email', 4: 'SMS', 5: 'In Person' }
                },
                { field: 'direction', title: 'Direction', width: 100 },
                { field: 'subject', title: 'Subject', width: 200 },
                {
                    field: 'outcomeStatus', title: 'Outcome', width: 130,
                    kind: 'enum',
                    enumMap: { 1: 'Interested', 2: 'Not Interested', 3: 'Call Back Later', 4: 'No Answer', 5: 'Other' }
                },
                { field: 'loggedDate', title: 'Logged Date', width: 130, dataType: 'date', format: '{0:dd MMM yyyy}' },
                { field: 'durationSeconds', title: 'Duration (s)', width: 110, dataType: 'number' }
            ]
        },
        form: {
            fields: [
                { name: 'communicationLogId', label: 'Log Id', type: 'hidden' },
                {
                    name: 'entityType', label: 'Entity Type', type: 'select', required: true,
                    dataTextField: 'text', dataValueField: 'value',
                    dataSource: [
                        { value: 1, text: 'Lead' },
                        { value: 2, text: 'Student' },
                        { value: 3, text: 'Application' }
                    ]
                },
                { name: 'entityId', label: 'Entity ID', type: 'number', required: true, min: 1, placeholder: 'Entity ID' },
                { name: 'branchId', label: 'Branch ID', type: 'number', required: true, min: 1, placeholder: 'Branch ID' },
                {
                    name: 'communicationType', label: 'Communication Type', type: 'select', required: true,
                    dataTextField: 'text', dataValueField: 'value',
                    dataSource: [
                        { value: 1, text: 'Call' },
                        { value: 2, text: 'WhatsApp' },
                        { value: 3, text: 'Email' },
                        { value: 4, text: 'SMS' },
                        { value: 5, text: 'In Person' }
                    ]
                },
                {
                    name: 'direction', label: 'Direction', type: 'select', required: true,
                    dataTextField: 'text', dataValueField: 'value',
                    dataSource: [
                        { value: 'Inbound', text: 'Inbound' },
                        { value: 'Outbound', text: 'Outbound' }
                    ]
                },
                { name: 'subject', label: 'Subject', type: 'text', maxLength: 300, wide: true, placeholder: 'Subject of communication' },
                { name: 'bodyOrNotes', label: 'Notes / Body', type: 'textarea', maxLength: 4000, wide: true, placeholder: 'Detailed notes or body' },
                { name: 'durationSeconds', label: 'Duration (seconds)', type: 'number', min: 0, placeholder: '0' },
                {
                    name: 'outcomeStatus', label: 'Outcome', type: 'select', required: true,
                    dataTextField: 'text', dataValueField: 'value',
                    dataSource: [
                        { value: 1, text: 'Interested' },
                        { value: 2, text: 'Not Interested' },
                        { value: 3, text: 'Call Back Later' },
                        { value: 4, text: 'No Answer' },
                        { value: 5, text: 'Other' }
                    ]
                },
                { name: 'loggedBy', label: 'Logged By (User ID)', type: 'number', required: true, min: 1, placeholder: 'User ID' },
                { name: 'loggedDate', label: 'Logged Date', type: 'date', required: true, placeholder: 'dd MMM yyyy' },
                { name: 'isDeleted', label: 'Deleted', type: 'checkbox', defaultValue: false }
            ],
            buildPayload: function (values, state) {
                function toInt(value) {
                    const parsed = parseInt(value || 0, 10);
                    return Number.isNaN(parsed) ? 0 : parsed;
                }
                function toNullableInt(value) {
                    const parsed = parseInt(value, 10);
                    return Number.isNaN(parsed) || parsed === 0 ? null : parsed;
                }
                function toNullableString(value) {
                    return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim();
                }
                const now = new Date().toISOString();
                return {
                    communicationLogId: toInt(values.communicationLogId),
                    entityType: toInt(values.entityType),
                    entityId: toInt(values.entityId),
                    branchId: toInt(values.branchId),
                    communicationType: toInt(values.communicationType),
                    direction: String(values.direction || 'Outbound').trim(),
                    subject: toNullableString(values.subject),
                    bodyOrNotes: toNullableString(values.bodyOrNotes),
                    durationSeconds: toNullableInt(values.durationSeconds),
                    outcomeStatus: toInt(values.outcomeStatus) || 5,
                    loggedBy: toInt(values.loggedBy) || 1,
                    loggedDate: values.loggedDate || now,
                    isDeleted: values.isDeleted === true,
                    createdDate: state.currentRecord?.createdDate || now,
                    createdBy: state.currentRecord?.createdBy || 1,
                    updatedDate: state.isEditMode ? now : null,
                    updatedBy: state.isEditMode ? 1 : null
                };
            }
        }
    };

    window.CommunicationLogModule.config.moduleRef = window.CommunicationLogModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () { window.location.href = '/Account/Login'; }, 1500);
            return;
        }

        window.CommunicationLogModule.Summary?.init?.();
        window.CommunicationLogModule.Details?.init?.();

        $(window.CommunicationLogModule.config.dom.addButton).on('click', function () {
            window.CommunicationLogModule.Details?.openAddForm?.();
        });

        $(window.CommunicationLogModule.config.dom.refreshButton).on('click', function () {
            window.CommunicationLogModule.Summary?.refreshGrid?.();
        });
    });
})();
