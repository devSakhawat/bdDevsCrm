(function () {
    'use strict';

    window.CommissionModule = window.CommissionModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.CommissionModule.config = {
        moduleTitle: 'Commission',
        pluralTitle: 'Commissions',
        idField: 'commissionId',
        displayField: 'invoiceNo',
        dom: {
            grid: '#commissionGrid',
            window: '#commissionWindow',
            form: '#commissionForm',
            addButton: '#btnAddCommission',
            refreshButton: '#btnRefreshCommission',
            saveButton: '#btnSaveCommission',
            cancelButton: '#btnCancelCommission'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-commission-summary`,
            create: `${apiRoot}/crm-commission`,
            update: function (id) { return `${apiRoot}/crm-commission/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-commission/${id}`; },
            read: function (id) { return `${apiRoot}/crm-commission/${id}`; }
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
            width: '960px'
        },
        grid: {
            columns: [
                { field: 'commissionId', title: '#', width: 60, dataType: 'number', filterable: false },
                { field: 'invoiceNo', title: 'Invoice No', width: 180 },
                { field: 'studentNameSnapshot', title: 'Student', width: 180 },
                { field: 'universityNameSnapshot', title: 'University', width: 180 },
                { field: 'grossAmount', title: 'Gross', width: 120, dataType: 'number', format: '{0:n2}' },
                { field: 'netAmount', title: 'Net', width: 120, dataType: 'number', format: '{0:n2}' },
                { field: 'currency', title: 'Currency', width: 80 },
                {
                    field: 'status', title: 'Status', width: 110,
                    kind: 'enum',
                    enumMap: { 1: 'Pending', 2: 'Due', 3: 'Invoiced', 4: 'Paid', 5: 'Disputed', 6: 'Written Off' }
                },
                { field: 'dueDate', title: 'Due Date', width: 120, dataType: 'date', format: '{0:dd MMM yyyy}' },
                { field: 'paidDate', title: 'Paid Date', width: 120, dataType: 'date', format: '{0:dd MMM yyyy}' }
            ]
        },
        form: {
            fields: [
                { name: 'commissionId', label: 'Commission Id', type: 'hidden' },
                { name: 'applicationId', label: 'Application ID', type: 'number', required: true, min: 1, placeholder: 'Application ID' },
                { name: 'universityId', label: 'University ID', type: 'number', required: true, min: 1, placeholder: 'University ID' },
                { name: 'agentId', label: 'Agent ID', type: 'number', placeholder: 'Agent ID (optional)' },
                { name: 'branchId', label: 'Branch ID', type: 'number', required: true, min: 1, placeholder: 'Branch ID' },
                { name: 'studentNameSnapshot', label: 'Student Name', type: 'text', maxLength: 200, placeholder: 'Student name snapshot', wide: true },
                { name: 'universityNameSnapshot', label: 'University Name', type: 'text', maxLength: 200, placeholder: 'University name snapshot', wide: true },
                { name: 'tuitionFeeBase', label: 'Tuition Fee Base', type: 'number', required: true, decimals: 2, step: 0.01, min: 0, placeholder: '0.00' },
                { name: 'commissionRate', label: 'Commission Rate (%)', type: 'number', required: true, decimals: 4, step: 0.0001, min: 0, placeholder: '0.0000' },
                {
                    name: 'commissionType', label: 'Commission Type', type: 'select', required: true,
                    dataTextField: 'text', dataValueField: 'value',
                    dataSource: [{ value: 1, text: 'Percentage' }, { value: 2, text: 'Fixed' }]
                },
                { name: 'grossAmount', label: 'Gross Amount', type: 'number', required: true, decimals: 2, step: 0.01, min: 0, placeholder: '0.00' },
                { name: 'scholarshipDeduction', label: 'Scholarship Deduction', type: 'number', decimals: 2, step: 0.01, min: 0, placeholder: '0.00' },
                { name: 'netAmount', label: 'Net Amount', type: 'number', required: true, decimals: 2, step: 0.01, min: 0, placeholder: '0.00' },
                { name: 'currency', label: 'Currency', type: 'text', maxLength: 10, placeholder: 'BDT' },
                { name: 'exchangeRate', label: 'Exchange Rate', type: 'number', decimals: 6, step: 0.000001, min: 0, placeholder: '1.000000' },
                { name: 'netAmountBdt', label: 'Net Amount (BDT)', type: 'number', decimals: 2, step: 0.01, min: 0, placeholder: '0.00' },
                {
                    name: 'status', label: 'Status', type: 'select', required: true,
                    dataTextField: 'text', dataValueField: 'value',
                    dataSource: [
                        { value: 1, text: 'Pending' }, { value: 2, text: 'Due' },
                        { value: 3, text: 'Invoiced' }, { value: 4, text: 'Paid' },
                        { value: 5, text: 'Disputed' }, { value: 6, text: 'Written Off' }
                    ]
                },
                { name: 'dueDate', label: 'Due Date', type: 'date', placeholder: 'dd MMM yyyy' },
                { name: 'paidDate', label: 'Paid Date', type: 'date', placeholder: 'dd MMM yyyy' },
                { name: 'paidAmount', label: 'Paid Amount', type: 'number', decimals: 2, step: 0.01, min: 0, placeholder: '0.00' },
                { name: 'invoiceNo', label: 'Invoice No', type: 'text', maxLength: 50, placeholder: 'Auto-generated on invoicing' },
                { name: 'notes', label: 'Notes', type: 'textarea', maxLength: 1000, wide: true, placeholder: 'Additional notes' },
                { name: 'isDeleted', label: 'Deleted', type: 'checkbox', defaultValue: false }
            ],
            buildPayload: function (values, state) {
                function toNullableInt(value) {
                    const parsed = parseInt(value, 10);
                    return Number.isNaN(parsed) || parsed === 0 ? null : parsed;
                }
                function toInt(value) {
                    const parsed = parseInt(value || 0, 10);
                    return Number.isNaN(parsed) ? 0 : parsed;
                }
                function toDecimal(value) {
                    const parsed = parseFloat(value || 0);
                    return Number.isNaN(parsed) ? 0 : parsed;
                }
                function toNullableDecimal(value) {
                    if (value === null || value === undefined || String(value).trim() === '') return null;
                    const parsed = parseFloat(value);
                    return Number.isNaN(parsed) ? null : parsed;
                }
                function toNullableString(value) {
                    return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim();
                }
                const now = new Date().toISOString();
                return {
                    commissionId: toInt(values.commissionId),
                    applicationId: toInt(values.applicationId),
                    universityId: toInt(values.universityId),
                    agentId: toNullableInt(values.agentId),
                    branchId: toInt(values.branchId),
                    studentNameSnapshot: toNullableString(values.studentNameSnapshot),
                    universityNameSnapshot: toNullableString(values.universityNameSnapshot),
                    tuitionFeeBase: toDecimal(values.tuitionFeeBase),
                    commissionRate: toDecimal(values.commissionRate),
                    commissionType: toInt(values.commissionType),
                    grossAmount: toDecimal(values.grossAmount),
                    scholarshipDeduction: toDecimal(values.scholarshipDeduction),
                    netAmount: toDecimal(values.netAmount),
                    currency: String(values.currency || 'BDT').trim(),
                    exchangeRate: toDecimal(values.exchangeRate) || 1,
                    netAmountBdt: toDecimal(values.netAmountBdt),
                    status: toInt(values.status) || 1,
                    dueDate: values.dueDate || null,
                    paidDate: values.paidDate || null,
                    paidAmount: toNullableDecimal(values.paidAmount),
                    invoiceNo: toNullableString(values.invoiceNo),
                    notes: toNullableString(values.notes),
                    isDeleted: values.isDeleted === true,
                    createdDate: state.currentRecord?.createdDate || now,
                    createdBy: state.currentRecord?.createdBy || 1,
                    updatedDate: state.isEditMode ? now : null,
                    updatedBy: state.isEditMode ? 1 : null
                };
            }
        }
    };

    window.CommissionModule.config.moduleRef = window.CommissionModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () { window.location.href = '/Account/Login'; }, 1500);
            return;
        }

        window.CommissionModule.Summary?.init?.();
        window.CommissionModule.Details?.init?.();

        $(window.CommissionModule.config.dom.addButton).on('click', function () {
            window.CommissionModule.Details?.openAddForm?.();
        });

        $(window.CommissionModule.config.dom.refreshButton).on('click', function () {
            window.CommissionModule.Summary?.refreshGrid?.();
        });
    });
})();
