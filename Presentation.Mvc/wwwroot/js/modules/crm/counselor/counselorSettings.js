(function () {
    'use strict';

    window.CounselorModule = window.CounselorModule || {};

    const apiRoot = window.CrmSimpleCrudFactory.getApiRoot();

    window.CounselorModule.config = {
        moduleTitle: 'Counselor',
        pluralTitle: 'Counselors',
        idField: 'counselorId',
        displayField: 'counselorName',
        dom: {
            grid: '#counselorGrid',
            window: '#counselorWindow',
            form: '#counselorForm',
            addButton: '#btnAddCounselor',
            refreshButton: '#btnRefreshCounselor',
            saveButton: '#btnSaveCounselor',
            cancelButton: '#btnCancelCounselor'
        },
        apiEndpoints: {
            summary: `${apiRoot}/crm-counselor-summary`,
            create: `${apiRoot}/crm-counselor`,
            update: function (id) { return `${apiRoot}/crm-counselor/${id}`; },
            delete: function (id) { return `${apiRoot}/crm-counselor/${id}`; },
            read: function (id) { return `${apiRoot}/crm-counselor/${id}`; },
            officesDdl: `${apiRoot}/crm-offices-ddl`
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
            columns: [{field: "counselorId", title: "#", width: 60, dataType: "number", filterable: false}, {field: "counselorName", title: "Counselor Name", width: 200}, {field: "counselorCode", title: "Code", width: 130}, {field: "email", title: "Email", width: 200}, {field: "phone", title: "Phone", width: 130}, {field: "isActive", title: "Status", width: 100, dataType: "boolean", kind: "boolean", trueText: "Active", falseText: "Inactive"}]
        },
        form: {
            fields: [{name: "counselorId", label: "Counselor Id", type: "hidden"}, {name: "counselorName", label: "Counselor Name", type: "text", required: true, maxLength: 150, placeholder: "Enter counselor name"}, {name: "counselorCode", label: "Counselor Code", type: "text", maxLength: 50, placeholder: "Enter counselor code"}, {name: "email", label: "Email", type: "text", maxLength: 150, placeholder: "Enter email"}, {name: "phone", label: "Phone", type: "text", maxLength: 50, placeholder: "Enter phone"}, {name: "officeId", label: "Office", type: "select", required: true, dataTextField: "officeName", dataValueField: "officeId", dataSourceEndpoint: `${apiRoot}/crm-offices-ddl`}, {name: "userId", label: "User Id", type: "number", placeholder: "Enter user id", min: 0}, {name: "isActive", label: "Active", type: "checkbox", defaultValue: true}],
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
                    counselorId: toInteger(values.counselorId),
                    counselorName: (values.counselorName || '').trim(),
                    counselorCode: normalizeString(values.counselorCode),
                    email: normalizeString(values.email),
                    phone: normalizeString(values.phone),
                    officeId: toInteger(values.officeId),
                    userId: toInteger(values.userId),
                    isActive: values.isActive !== false,
                    createdDate: state.currentRecord?.createdDate || now,
                    createdBy: state.currentRecord?.createdBy || 1,
                    updatedDate: state.isEditMode ? now : null,
                    updatedBy: state.isEditMode ? 1 : null
                };
            }
        }
    };

    window.CounselorModule.config.moduleRef = window.CounselorModule;

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        window.CounselorModule.Summary?.init?.();
        window.CounselorModule.Details?.init?.();

        $(window.CounselorModule.config.dom.addButton).on('click', function () {
            window.CounselorModule.Details?.openAddForm?.();
        });

        $(window.CounselorModule.config.dom.refreshButton).on('click', function () {
            window.CounselorModule.Summary?.refreshGrid?.();
        });
    });
})();
