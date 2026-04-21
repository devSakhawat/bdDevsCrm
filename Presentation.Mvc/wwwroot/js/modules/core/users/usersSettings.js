/**
 * Users Settings - Initialization Module
 * This file initializes the Users module when the page loads
 */

(function () {
    'use strict';

    // Module configuration
    window.UsersModule = window.UsersModule || {};

    window.UsersModule.config = {
        apiEndpoints: {
            summary: window.AppConfig.apiBaseUrl + '/core/systemadmin/user-summary',
            create: window.AppConfig.apiBaseUrl + '/core/systemadmin/user',
            update: (id) => window.AppConfig.apiBaseUrl + `/core/systemadmin/user/${id}`,
            delete: (id) => window.AppConfig.apiBaseUrl + `/core/systemadmin/user/${id}`,
            read: (id) => window.AppConfig.apiBaseUrl + `/core/systemadmin/user/${id}`,
            companiesDDL: window.AppConfig.apiBaseUrl + '/core/systemadmin/companies-ddl',
            groupsDDL: window.AppConfig.apiBaseUrl + '/core/systemadmin/groups-ddl',
            employeesDDL: window.AppConfig.apiBaseUrl + '/core/hr/employees-ddl'
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
            width: '700px',
            title: 'User Details',
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
        if (window.UsersModule.Summary && typeof window.UsersModule.Summary.init === 'function') {
            window.UsersModule.Summary.init();
        }

        // Initialize form handlers
        if (window.UsersModule.Details && typeof window.UsersModule.Details.init === 'function') {
            window.UsersModule.Details.init();
        }

        // Attach event handlers
        attachEventHandlers();

        console.log('Users module initialized');
    });

    // Attach event handlers
    function attachEventHandlers() {
        // Add New User button
        $('#btnAddUser').on('click', function () {
            if (window.UsersModule.Details && typeof window.UsersModule.Details.openAddForm === 'function') {
                window.UsersModule.Details.openAddForm();
            }
        });

        // Refresh button
        $('#btnRefresh').on('click', function () {
            if (window.UsersModule.Summary && typeof window.UsersModule.Summary.refreshGrid === 'function') {
                window.UsersModule.Summary.refreshGrid();
                window.AppToast?.success('Grid refreshed successfully');
            }
        });
    }

})();
