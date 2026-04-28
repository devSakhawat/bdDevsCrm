(function () {
    'use strict';

    window.BranchTransferModule = window.BranchTransferModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    const ENTITY_TYPE_OPTIONS = [
        { value: 1, text: 'Lead' },
        { value: 2, text: 'Student' },
        { value: 3, text: 'Application' }
    ];

    const TRANSFER_STATUS_OPTIONS = [
        { value: 1, text: 'Pending' },
        { value: 2, text: 'Approved' },
        { value: 3, text: 'Rejected' },
        { value: 4, text: 'Completed' }
    ];

    window.BranchTransferModule.config = {
        moduleTitle: 'Branch Transfer',
        pluralTitle: 'Branch Transfers',
        idField: 'transferId',
        displayField: 'transferId',
        dom: {
            grid: '#branchTransferGrid',
            window: '#branchTransferWindow',
            form: '#branchTransferForm',
            addButton: '#btnAddBranchTransfer',
            refreshButton: '#btnRefreshBranchTransfer',
            saveButton: '#btnSaveBranchTransfer',
            cancelButton: '#btnCancelBranchTransfer'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-branch-transfer-summary`,
            create: `${apiRoot}/crm-branch-transfer`,
            update: function (id) { return `${apiRoot}/crm-branch-transfer/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-branch-transfer/${id}`; },
            read: function (id) { return `${apiRoot}/crm-branch-transfer/${id}`; }
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
            width: '680px'
        },
        grid: {
            columns: [
                { field: 'transferId', title: '#', width: 60, dataType: 'number', filterable: false },
                {
                    field: 'entityType', title: 'Entity Type', width: 120,
                    template: function (row) {
                        const found = ENTITY_TYPE_OPTIONS.find(x => x.value === row.entityType);
                        return found ? found.text : row.entityType;
                    }
                },
                { field: 'entityId', title: 'Entity ID', width: 90, dataType: 'number' },
                { field: 'fromBranchId', title: 'From Branch', width: 110, dataType: 'number' },
                { field: 'toBranchId', title: 'To Branch', width: 110, dataType: 'number' },
                {
                    field: 'transferStatus', title: 'Status', width: 110,
                    template: function (row) {
                        const found = TRANSFER_STATUS_OPTIONS.find(x => x.value === row.transferStatus);
                        const cls = row.transferStatus === 2 ? 'badge badge--success'
                            : row.transferStatus === 3 ? 'badge badge--danger'
                                : row.transferStatus === 4 ? 'badge badge--info'
                                    : 'badge badge--warning';
                        return `<span class="${cls}">${found ? found.text : row.transferStatus}</span>`;
                    }
                },
                { field: 'requestedDate', title: 'Requested', width: 130, dataType: 'date', format: '{0:yyyy-MM-dd}' },
                { field: 'approvedDate', title: 'Approved', width: 130, dataType: 'date', format: '{0:yyyy-MM-dd}' },
                { field: 'transferReason', title: 'Reason', width: 200 }
            ]
        },
        form: {
            fields: [
                { name: 'transferId', label: 'Transfer Id', type: 'hidden' },
                {
                    name: 'entityType', label: 'Entity Type', type: 'select', required: true,
                    options: ENTITY_TYPE_OPTIONS, placeholder: 'Select entity type'
                },
                { name: 'entityId', label: 'Entity ID', type: 'number', required: true, min: 1, placeholder: 'Enter entity ID' },
                { name: 'fromBranchId', label: 'From Branch ID', type: 'number', required: true, min: 1 },
                { name: 'toBranchId', label: 'To Branch ID', type: 'number', required: true, min: 1 },
                {
                    name: 'transferStatus', label: 'Transfer Status', type: 'select', required: true,
                    options: TRANSFER_STATUS_OPTIONS, placeholder: 'Select status'
                },
                { name: 'transferReason', label: 'Transfer Reason', type: 'textarea', maxLength: 500, wide: true, placeholder: 'Enter reason for transfer' },
                { name: 'requestedBy', label: 'Requested By', type: 'number', required: true, min: 1 },
                { name: 'approvedBy', label: 'Approved By', type: 'number' },
                { name: 'requestedDate', label: 'Requested Date', type: 'date', required: true },
                { name: 'approvedDate', label: 'Approved Date', type: 'date' },
                { name: 'notes', label: 'Notes', type: 'textarea', maxLength: 1000, wide: true }
            ],
            buildPayload: function (values, state) {
                function toInt(v) { const p = parseInt(v || 0, 10); return Number.isNaN(p) ? 0 : p; }
                function toIntOrNull(v) { if (!v && v !== 0) return null; const p = parseInt(v, 10); return Number.isNaN(p) ? null : p; }
                function toDateOrNull(v) { return v ? new Date(v).toISOString() : null; }
                const now = new Date().toISOString();
                return {
                    transferId: toInt(values.transferId),
                    entityType: toInt(values.entityType),
                    entityId: toInt(values.entityId),
                    fromBranchId: toInt(values.fromBranchId),
                    toBranchId: toInt(values.toBranchId),
                    transferReason: values.transferReason || null,
                    transferStatus: toInt(values.transferStatus),
                    requestedBy: toInt(values.requestedBy),
                    approvedBy: toIntOrNull(values.approvedBy),
                    requestedDate: toDateOrNull(values.requestedDate) || now,
                    approvedDate: toDateOrNull(values.approvedDate),
                    notes: values.notes || null,
                    isDeleted: false,
                    createdDate: state.currentRecord?.createdDate || now,
                    createdBy: state.currentRecord?.createdBy || 1,
                    updatedDate: state.isEditMode ? now : null,
                    updatedBy: state.isEditMode ? 1 : null
                };
            }
        }
    };

    window.BranchTransferModule.config.moduleRef = window.BranchTransferModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () { window.location.href = '/Account/Login'; }, 1500);
            return;
        }

        window.BranchTransferModule.Summary?.init?.();
        window.BranchTransferModule.Details?.init?.();

        $(window.BranchTransferModule.config.dom.addButton).on('click', function () {
            window.BranchTransferModule.Details?.openAddForm?.();
        });
        $(window.BranchTransferModule.config.dom.refreshButton).on('click', function () {
            window.BranchTransferModule.Summary?.refreshGrid?.();
        });
    });
})();
