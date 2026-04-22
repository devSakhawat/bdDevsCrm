(function () {
    'use strict';

    window.ApproverDetailsModule = window.ApproverDetailsModule || {};

    window.ApproverDetailsModule.config = {
        moduleTitle: 'Approver Details',
        pluralTitle: 'Approver Details',
        idField: 'remarksId',
        displayField: 'comments',
        dom: {
            grid: '#approverDetailsGrid',
            window: '#approverDetailsWindow',
            form: '#approverDetailsForm',
            addButton: '#btnAddApproverDetails',
            refreshButton: '#btnRefreshApproverDetails',
            saveButton: '#btnSaveApproverDetails',
            cancelButton: '#btnCancelApproverDetails'
        },
        apiEndpoints: {
            summary: `${window.AppConfig.apiBaseUrl}/core/systemadmin/approver-details-summary`,
            create: `${window.AppConfig.apiBaseUrl}/core/systemadmin/approver-details`,
            update: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/approver-details/${id}`; },
            delete: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/approver-details/${id}`; },
            read: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/approver-details/${id}`; },
            modules: `${window.AppConfig.apiBaseUrl}/core/systemadmin/modules-ddl`
        },
        gridOptions: {
            pageSize: 20,
            sortable: true,
            filterable: true,
            height: 560,
            pageable: { refresh: true, pageSizes: [10, 20, 50, 100], buttonCount: 5 }
        },
        windowOptions: { width: '820px' },
        grid: { columns: [{field: 'remarksId', title: 'ID', width: 90, dataType: 'number', filterable: false}, {field: 'comments', title: 'Comments', width: 260}, {field: 'applicationId', title: 'Application Id', width: 130, dataType: 'number'}, {field: 'moduleId', title: 'Module', width: 120, dataType: 'number'}, {field: 'approvedBy', title: 'Approved By', width: 120, dataType: 'number'}, {field: 'approvedDate', title: 'Approved Date', width: 170}] },
        form: { fields: [{name: 'remarksId', label: 'Remarks Id', type: 'hidden'}, {name: 'menuId', label: 'Menu Id', type: 'number', min: 0, placeholder: 'Enter menu id'}, {name: 'applicationId', label: 'Application Id', type: 'number', required: true, min: 0, placeholder: 'Enter application id'}, {name: 'isOpen', label: 'Is Open', type: 'checkbox', defaultValue: false}, {name: 'comments', label: 'Comments', type: 'textarea', wide: true, maxLength: 2000, placeholder: 'Enter comments'}, {name: 'approvedDate', label: 'Approved Date', type: 'date'}, {name: 'applicantHrRecordId', label: 'Applicant HR Record Id', type: 'number', min: 0, placeholder: 'Enter applicant HR record id'}, {name: 'approvedBy', label: 'Approved By', type: 'number', min: 0, placeholder: 'Enter approver id'}, {name: 'approverType', label: 'Approver Type', type: 'number', min: 0, placeholder: 'Enter approver type'}, {name: 'sequence', label: 'Sequence', type: 'number', min: 0, placeholder: 'Enter sequence'}, {name: 'moduleId', label: 'Module', type: 'select', dataSourceEndpoint: function () { return window.ApproverDetailsModule.config.apiEndpoints.modules; }, dataTextField: 'moduleName', dataValueField: 'moduleId', optionLabel: 'Select Module...'}, {name: 'companyId', label: 'Company Id', type: 'number', min: 0, placeholder: 'Enter company id'}, {name: 'branchId', label: 'Branch Id', type: 'number', min: 0, placeholder: 'Enter branch id'}, {name: 'divisionId', label: 'Division Id', type: 'number', min: 0, placeholder: 'Enter division id'}, {name: 'departmentId', label: 'Department Id', type: 'number', min: 0, placeholder: 'Enter department id'}, {name: 'facilityId', label: 'Facility Id', type: 'number', min: 0, placeholder: 'Enter facility id'}, {name: 'sectionId', label: 'Section Id', type: 'number', min: 0, placeholder: 'Enter section id'}, {name: 'designationId', label: 'Designation Id', type: 'number', min: 0, placeholder: 'Enter designation id'}, {name: 'gradeId', label: 'Grade Id', type: 'number', min: 0, placeholder: 'Enter grade id'}, {name: 'employeeTypeId', label: 'Employee Type Id', type: 'number', min: 0, placeholder: 'Enter employee type id'}, {name: 'funcId', label: 'Function Id', type: 'number', min: 0, placeholder: 'Enter function id'}, {name: 'assignApproverId', label: 'Assign Approver Id', type: 'number', min: 0, placeholder: 'Enter assign approver id'}], buildPayload: function (values) {
                function toInteger(value) { const parsed = parseInt(value || 0, 10); return Number.isNaN(parsed) ? 0 : parsed; }
                function toNullableInteger(value) { if (value === null || value === undefined || value === '') { return null; } const parsed = parseInt(value, 10); return Number.isNaN(parsed) ? null : parsed; }
                function normalizeString(value) { return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim(); }
                return { remarksId: toInteger(values.remarksId), menuId: toNullableInteger(values.menuId), applicationId: toInteger(values.applicationId), isOpen: values.isOpen ? 1 : 0, comments: normalizeString(values.comments), approvedDate: values.approvedDate || null, applicantHrRecordId: toNullableInteger(values.applicantHrRecordId), approvedBy: toNullableInteger(values.approvedBy), approverType: toNullableInteger(values.approverType), sequence: toNullableInteger(values.sequence), moduleId: toNullableInteger(values.moduleId), companyId: toNullableInteger(values.companyId), branchId: toNullableInteger(values.branchId), divisionId: toNullableInteger(values.divisionId), departmentId: toNullableInteger(values.departmentId), facilityId: toNullableInteger(values.facilityId), sectionId: toNullableInteger(values.sectionId), designationId: toNullableInteger(values.designationId), gradeId: toNullableInteger(values.gradeId), employeeTypeId: toNullableInteger(values.employeeTypeId), funcId: toNullableInteger(values.funcId), assignApproverId: toNullableInteger(values.assignApproverId) };
            } }
    };

    window.ApproverDetailsModule.config.moduleRef = window.ApproverDetailsModule;
    window.ApproverDetailsModule.config.form.fields.forEach(function (field) { field.moduleRef = window.ApproverDetailsModule; });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () { window.location.href = '/Account/Login'; }, 1500);
            return;
        }

        window.ApproverDetailsModule.Summary?.init?.();
        window.ApproverDetailsModule.Details?.init?.();

        $(window.ApproverDetailsModule.config.dom.addButton).on('click', function () { window.ApproverDetailsModule.Details?.openAddForm?.(); });
        $(window.ApproverDetailsModule.config.dom.refreshButton).on('click', function () { window.ApproverDetailsModule.Summary?.refreshGrid?.(); });
    });
})();
