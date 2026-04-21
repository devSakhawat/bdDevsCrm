/**
 * Employee Settings - Initialization Module
 * Handles 12-tab employee form configuration
 */

(function () {
    'use strict';

    window.EmployeeModule = window.EmployeeModule || {};

    window.EmployeeModule.config = {
        apiEndpoints: {
            summary: window.AppConfig.apiBaseUrl + '/core/hr/employee-summary',
            create: window.AppConfig.apiBaseUrl + '/core/hr/employee',
            update: (id) => window.AppConfig.apiBaseUrl + `/core/hr/employee/${id}`,
            delete: (id) => window.AppConfig.apiBaseUrl + `/core/hr/employee/${id}`,
            read: (id) => window.AppConfig.apiBaseUrl + `/core/hr/employee/${id}`,
            companiesDDL: window.AppConfig.apiBaseUrl + '/core/hr/companies-ddl',
            branchesByCompany: (companyId) => window.AppConfig.apiBaseUrl + `/core/hr/branches-by-company/${companyId}`,
            departmentsByBranch: (branchId) => window.AppConfig.apiBaseUrl + `/core/hr/departments-by-branch/${branchId}`,
            designationsDDL: window.AppConfig.apiBaseUrl + '/core/hr/designations-ddl',
            shiftsDDL: window.AppConfig.apiBaseUrl + '/core/hr/shifts-ddl',
            employeesDDL: window.AppConfig.apiBaseUrl + '/core/hr/employees-ddl',
            gradesDDL: window.AppConfig.apiBaseUrl + '/core/hr/grades-ddl',
            locationsDDL: window.AppConfig.apiBaseUrl + '/core/hr/locations-ddl',
            uploadDocument: window.AppConfig.apiBaseUrl + '/core/hr/employee/upload-document'
        },
        gridOptions: {
            pageSize: 20,
            sortable: true,
            filterable: true,
            pageable: {
                refresh: true,
                pageSizes: [10, 20, 50, 100],
                buttonCount: 5
            }
        },
        windowOptions: {
            width: '1100px',
            height: '650px',
            title: 'Employee Details',
            modal: true,
            visible: false,
            actions: ['Close']
        },
        tabs: [
            { id: 'tab-basic', title: 'Basic Information' },
            { id: 'tab-personal', title: 'Personal Details' },
            { id: 'tab-contact', title: 'Contact Information' },
            { id: 'tab-employment', title: 'Employment Details' },
            { id: 'tab-department', title: 'Department & Designation' },
            { id: 'tab-salary', title: 'Salary Information' },
            { id: 'tab-bank', title: 'Bank Details' },
            { id: 'tab-emergency', title: 'Emergency Contact' },
            { id: 'tab-education', title: 'Education' },
            { id: 'tab-experience', title: 'Experience' },
            { id: 'tab-documents', title: 'Documents' },
            { id: 'tab-additional', title: 'Additional Info' }
        ],
        dropdownData: {
            gender: [
                { text: 'Select Gender...', value: '' },
                { text: 'Male', value: 'Male' },
                { text: 'Female', value: 'Female' },
                { text: 'Other', value: 'Other' }
            ],
            maritalStatus: [
                { text: 'Select Status...', value: '' },
                { text: 'Single', value: 'Single' },
                { text: 'Married', value: 'Married' },
                { text: 'Divorced', value: 'Divorced' },
                { text: 'Widowed', value: 'Widowed' }
            ],
            religion: [
                { text: 'Select Religion...', value: '' },
                { text: 'Islam', value: 'Islam' },
                { text: 'Hinduism', value: 'Hinduism' },
                { text: 'Buddhism', value: 'Buddhism' },
                { text: 'Christianity', value: 'Christianity' },
                { text: 'Other', value: 'Other' }
            ],
            bloodGroup: [
                { text: 'Select Blood Group...', value: '' },
                { text: 'A+', value: 'A+' },
                { text: 'A-', value: 'A-' },
                { text: 'B+', value: 'B+' },
                { text: 'B-', value: 'B-' },
                { text: 'AB+', value: 'AB+' },
                { text: 'AB-', value: 'AB-' },
                { text: 'O+', value: 'O+' },
                { text: 'O-', value: 'O-' }
            ]
        }
    };

    $(document).ready(function () {
        // Check authentication
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning('Session expired');
            setTimeout(() => window.location.href = '/Account/Login', 1500);
            return;
        }

        // Initialize grid
        if (window.EmployeeModule.Summary) window.EmployeeModule.Summary.init();

        // Initialize form handlers
        if (window.EmployeeModule.Details) window.EmployeeModule.Details.init();

        // Attach event handlers
        $('#btnAddEmployee').on('click', () => window.EmployeeModule.Details?.openAddForm());
        $('#btnRefresh').on('click', () => {
            window.EmployeeModule.Summary?.refreshGrid();
            window.AppToast?.success('Grid refreshed');
        });

        console.log('Employee module initialized with 12-tab configuration');
    });

})();
