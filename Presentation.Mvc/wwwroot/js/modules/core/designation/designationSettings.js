/**
 * Designation Settings - Initialization Module
 * This file initializes the Designation module when the page loads
 */

(function () {
    'use strict';

    // Module configuration
    window.DesignationModule = window.DesignationModule || {};

    window.DesignationModule.config = {
        apiEndpoints: {
            summary: window.AppConfig.apiBaseUrl + '/core/hr/designation-summary',
            create: window.AppConfig.apiBaseUrl + '/core/hr/designation',
            update: (id) => window.AppConfig.apiBaseUrl + `/core/hr/designation/${id}`,
            delete: (id) => window.AppConfig.apiBaseUrl + `/core/hr/designation/${id}`,
            read: (id) => window.AppConfig.apiBaseUrl + `/core/hr/designation/${id}`,
            designationsDDL: window.AppConfig.apiBaseUrl + '/core/hr/designations-ddl'
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
            title: 'Designation Details',
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
        if (window.DesignationModule.Summary && typeof window.DesignationModule.Summary.init === 'function') {
            window.DesignationModule.Summary.init();
        }

        // Initialize form handlers
        if (window.DesignationModule.Details && typeof window.DesignationModule.Details.init === 'function') {
            window.DesignationModule.Details.init();
        }

        // Attach event handlers
        attachEventHandlers();

        console.log('Designation module initialized');
    });

    // Attach event handlers
    function attachEventHandlers() {
        // Add New Designation button
        $('#btnAddDesignation').on('click', function () {
            if (window.DesignationModule.Details && typeof window.DesignationModule.Details.openAddForm === 'function') {
                window.DesignationModule.Details.openAddForm();
            }
        });

        // Refresh button
        $('#btnRefresh').on('click', function () {
            if (window.DesignationModule.Summary && typeof window.DesignationModule.Summary.refreshGrid === 'function') {
                window.DesignationModule.Summary.refreshGrid();
                window.AppToast?.success('Grid refreshed successfully');
            }
        });
    }

})();
