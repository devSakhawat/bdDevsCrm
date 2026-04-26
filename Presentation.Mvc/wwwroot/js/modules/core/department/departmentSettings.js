/**
 * Department Settings - Initialization Module
 * This file initializes the Department module when the page loads
 */

(function () {
    'use strict';

    // Module configuration
    window.DepartmentModule = window.DepartmentModule || {};

    window.DepartmentModule.config = {
        apiEndpoints: {
            summary: window.AppConfig.apiBaseUrl + '/core/hr/department-summary',
            create: window.AppConfig.apiBaseUrl + '/core/hr/department',
            update: (id) => window.AppConfig.apiBaseUrl + `/core/hr/department/${id}`,
            delete: (id) => window.AppConfig.apiBaseUrl + `/core/hr/department/${id}`,
            read: (id) => window.AppConfig.apiBaseUrl + `/core/hr/department/${id}`,
            departmentsDDL: window.AppConfig.apiBaseUrl + '/core/hr/departments-ddl'
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
            width: '600px',
            title: 'Department Details',
            modal: true,
            visible: false,
            actions: ['Close']
        }
    };

    // Initialize on DOM ready
    $(document).ready(function () {
        // Check authentication
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages.warnings.sessionExpired || 'Session expired');
            setTimeout(() => {
                window.location.href = '/Account/Login';
            }, 1500);
            return;
        }

        // Initialize grid
        if (window.DepartmentModule.Summary && typeof window.DepartmentModule.Summary.init === 'function') {
            window.DepartmentModule.Summary.init();
        }

        // Initialize form handlers
        if (window.DepartmentModule.Details && typeof window.DepartmentModule.Details.init === 'function') {
            window.DepartmentModule.Details.init();
        }

        // Attach event handlers
        attachEventHandlers();

        console.log('Department module initialized');
    });

    // Attach event handlers
    function attachEventHandlers() {
        // Add New Department button
        $('#btnAddDepartment').on('click', function () {
            if (window.DepartmentModule.Details && typeof window.DepartmentModule.Details.openAddForm === 'function') {
                window.DepartmentModule.Details.openAddForm();
            }
        });

        // Refresh button
        $('#btnRefresh').on('click', function () {
            if (window.DepartmentModule.Summary && typeof window.DepartmentModule.Summary.refreshGrid === 'function') {
                window.DepartmentModule.Summary.refreshGrid();
                window.AppToast?.success('Grid refreshed successfully');
            }
        });
    }

})();
