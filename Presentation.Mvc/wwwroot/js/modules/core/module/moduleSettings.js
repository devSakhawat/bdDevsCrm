/**
 * Module Settings - Initialization Module
 * This file initializes the Module module when the page loads
 */

(function () {
    'use strict';

    // Module configuration
    window.ModuleModule = window.ModuleModule || {};

    window.ModuleModule.config = {
        apiEndpoints: {
            summary: window.AppConfig.apiBaseUrl + '/core/systemadmin/module-summary',
            create: window.AppConfig.apiBaseUrl + '/core/systemadmin/module',
            update: (id) => window.AppConfig.apiBaseUrl + `/core/systemadmin/module/${id}`,
            delete: (id) => window.AppConfig.apiBaseUrl + `/core/systemadmin/module/${id}`,
            read: (id) => window.AppConfig.apiBaseUrl + `/core/systemadmin/module/${id}`
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
            title: 'Module Details',
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
        if (window.ModuleModule.Summary && typeof window.ModuleModule.Summary.init === 'function') {
            window.ModuleModule.Summary.init();
        }

        // Initialize form handlers
        if (window.ModuleModule.Details && typeof window.ModuleModule.Details.init === 'function') {
            window.ModuleModule.Details.init();
        }

        // Attach event handlers
        attachEventHandlers();

        console.log('Module module initialized');
    });

    // Attach event handlers
    function attachEventHandlers() {
        // Add New Module button
        $('#btnAddModule').on('click', function () {
            if (window.ModuleModule.Details && typeof window.ModuleModule.Details.openAddForm === 'function') {
                window.ModuleModule.Details.openAddForm();
            }
        });

        // Refresh button
        $('#btnRefresh').on('click', function () {
            if (window.ModuleModule.Summary && typeof window.ModuleModule.Summary.refreshGrid === 'function') {
                window.ModuleModule.Summary.refreshGrid();
                window.AppToast?.success('Grid refreshed successfully');
            }
        });
    }

})();
