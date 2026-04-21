/**
 * Menu Settings - Initialization Module
 * This file initializes the Menu module when the page loads
 */

(function () {
    'use strict';

    // Module configuration
    window.MenuModule = window.MenuModule || {};

    window.MenuModule.config = {
        apiEndpoints: {
            summary: window.AppConfig.apiBaseUrl + '/core/systemadmin/menu-summary',
            create: window.AppConfig.apiBaseUrl + '/core/systemadmin/menu',
            update: (id) => window.AppConfig.apiBaseUrl + `/core/systemadmin/menu/${id}`,
            delete: (id) => window.AppConfig.apiBaseUrl + `/core/systemadmin/menu/${id}`,
            read: (id) => window.AppConfig.apiBaseUrl + `/core/systemadmin/menu/${id}`,
            menusDDL: window.AppConfig.apiBaseUrl + '/core/systemadmin/menus-ddl',
            modulesDDL: window.AppConfig.apiBaseUrl + '/core/systemadmin/modules-ddl'
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
            title: 'Menu Details',
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
        if (window.MenuModule.Summary && typeof window.MenuModule.Summary.init === 'function') {
            window.MenuModule.Summary.init();
        }

        // Initialize form handlers
        if (window.MenuModule.Details && typeof window.MenuModule.Details.init === 'function') {
            window.MenuModule.Details.init();
        }

        // Attach event handlers
        attachEventHandlers();

        console.log('Menu module initialized');
    });

    // Attach event handlers
    function attachEventHandlers() {
        // Add New Menu button
        $('#btnAddMenu').on('click', function () {
            if (window.MenuModule.Details && typeof window.MenuModule.Details.openAddForm === 'function') {
                window.MenuModule.Details.openAddForm();
            }
        });

        // Refresh button
        $('#btnRefresh').on('click', function () {
            if (window.MenuModule.Summary && typeof window.MenuModule.Summary.refreshGrid === 'function') {
                window.MenuModule.Summary.refreshGrid();
                window.AppToast?.success('Grid refreshed successfully');
            }
        });
    }

})();
