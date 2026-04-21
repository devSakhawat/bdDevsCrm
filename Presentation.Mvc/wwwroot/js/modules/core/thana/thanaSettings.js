/**
 * Thana Settings - Initialization Module
 * This file initializes the Thana module when the page loads
 */

(function () {
    'use strict';

    // Module configuration
    window.ThanaModule = window.ThanaModule || {};

    window.ThanaModule.config = {
        apiEndpoints: {
            summary: window.AppConfig.apiBaseUrl + '/core/systemadmin/thana-summary',
            create: window.AppConfig.apiBaseUrl + '/core/systemadmin/thana',
            update: (id) => window.AppConfig.apiBaseUrl + `/core/systemadmin/thana/${id}`,
            delete: (id) => window.AppConfig.apiBaseUrl + `/core/systemadmin/thana/${id}`,
            read: (id) => window.AppConfig.apiBaseUrl + `/core/systemadmin/thana/${id}`,
            districtsDDL: window.AppConfig.apiBaseUrl + '/core/systemadmin/districts-ddl'
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
            title: 'Thana Details',
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
        if (window.ThanaModule.Summary && typeof window.ThanaModule.Summary.init === 'function') {
            window.ThanaModule.Summary.init();
        }

        // Initialize form handlers
        if (window.ThanaModule.Details && typeof window.ThanaModule.Details.init === 'function') {
            window.ThanaModule.Details.init();
        }

        // Attach event handlers
        attachEventHandlers();

        console.log('Thana module initialized');
    });

    // Attach event handlers
    function attachEventHandlers() {
        // Add New Thana button
        $('#btnAddThana').on('click', function () {
            if (window.ThanaModule.Details && typeof window.ThanaModule.Details.openAddForm === 'function') {
                window.ThanaModule.Details.openAddForm();
            }
        });

        // Refresh button
        $('#btnRefresh').on('click', function () {
            if (window.ThanaModule.Summary && typeof window.ThanaModule.Summary.refreshGrid === 'function') {
                window.ThanaModule.Summary.refreshGrid();
                window.AppToast?.success('Grid refreshed successfully');
            }
        });
    }

})();
