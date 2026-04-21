(function () {
    'use strict';

    window.PaymentMethodModule = window.PaymentMethodModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.PaymentMethodModule.config = {
        moduleTitle: 'Payment Method',
        pluralTitle: 'Payment Methods',
        idField: 'paymentMethodId',
        displayField: 'paymentMethodName',
        dom: {
            grid: '#paymentMethodGrid',
            window: '#paymentMethodWindow',
            form: '#paymentMethodForm',
            addButton: '#btnAddPaymentMethod',
            refreshButton: '#btnRefreshPaymentMethod',
            saveButton: '#btnSavePaymentMethod',
            cancelButton: '#btnCancelPaymentMethod'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-payment-method-summary`,
            create: `${apiRoot}/crm-payment-method`,
            update: function (id) { return `${apiRoot}/crm-payment-method/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-payment-method/${id}`; },
            read: function (id) { return `${apiRoot}/crm-payment-method/${id}`; }
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
            columns: [{field: "paymentMethodId", title: "ID", width: 90, dataType: "number", filterable: false}, {field: "paymentMethodName", title: "Payment Method Name", width: 220}, {field: "paymentMethodCode", title: "Code", width: 130}, {field: "processingFee", title: "Processing Fee", width: 130, kind: "currency", dataType: "number"}, {field: "processingFeeType", title: "Fee Type", width: 130}, {field: "isOnlinePayment", title: "Online", width: 120, kind: "boolean", dataType: "boolean", trueText: "Yes", falseText: "No"}, {field: "isActive", title: "Status", width: 120, kind: "boolean", dataType: "boolean", trueText: "Active", falseText: "Inactive"}]
        },
        form: {
            fields: [{name: "paymentMethodId", label: "Payment Method Id", type: "hidden"}, {name: "paymentMethodName", label: "Payment Method Name", type: "text", required: true, maxLength: 100, placeholder: "Enter payment method name"}, {name: "paymentMethodCode", label: "Payment Method Code", type: "text", maxLength: 50, placeholder: "Enter payment method code"}, {name: "processingFee", label: "Processing Fee", type: "number", decimals: 2, step: 0.01, min: 0, placeholder: "0.00"}, {name: "processingFeeType", label: "Processing Fee Type", type: "text", maxLength: 255, placeholder: "Flat / Percent"}, {name: "description", label: "Description", type: "textarea", maxLength: 255, wide: true, placeholder: "Enter description"}, {name: "isOnlinePayment", label: "Online Payment", type: "checkbox", defaultValue: false}, {name: "isActive", label: "Active", type: "checkbox", defaultValue: true}],
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
                                paymentMethodId: toInteger(values.paymentMethodId),
                                paymentMethodName: (values.paymentMethodName || '').trim(),
                                paymentMethodCode: normalizeString(values.paymentMethodCode),
                                description: normalizeString(values.description),
                                processingFee: values.processingFee === null ? null : Number(values.processingFee),
                                processingFeeType: normalizeString(values.processingFeeType),
                                isOnlinePayment: !!values.isOnlinePayment,
                                isActive: values.isActive !== false,
                                createdDate: state.currentRecord?.createdDate || now,
                                createdBy: state.currentRecord?.createdBy || 1,
                                updatedDate: state.isEditMode ? now : null,
                                updatedBy: state.isEditMode ? 1 : null
                            };
            }
        }
    };

    window.PaymentMethodModule.config.moduleRef = window.PaymentMethodModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.PaymentMethodModule.Summary?.init?.();
        window.PaymentMethodModule.Details?.init?.();

        $(window.PaymentMethodModule.config.dom.addButton).on('click', function () {
            window.PaymentMethodModule.Details?.openAddForm?.();
        });

        $(window.PaymentMethodModule.config.dom.refreshButton).on('click', function () {
            window.PaymentMethodModule.Summary?.refreshGrid?.();
        });
    });
})();
