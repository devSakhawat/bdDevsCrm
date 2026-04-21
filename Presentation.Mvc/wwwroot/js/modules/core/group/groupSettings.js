/**
 * Group Settings - Initialization Module
 * This file initializes the Group module when the page loads
 */

(function () {
    'use strict';

    // Module configuration
    window.GroupModule = window.GroupModule || {};

    window.GroupModule.config = {
        apiEndpoints: {
            summary: window.AppConfig.apiBaseUrl + '/core/systemadmin/group-summary',
            create: window.AppConfig.apiBaseUrl + '/core/systemadmin/group',
            update: (id) => window.AppConfig.apiBaseUrl + `/core/systemadmin/group/${id}`,
            delete: (id) => window.AppConfig.apiBaseUrl + `/core/systemadmin/group/${id}`,
            read: (id) => window.AppConfig.apiBaseUrl + `/core/systemadmin/group/${id}`,
            companiesDDL: window.AppConfig.apiBaseUrl + '/core/systemadmin/companies-ddl',
            accessControls: window.AppConfig.apiBaseUrl + '/core/systemadmin/access-controls',
            groupPermissions: (groupId) => window.AppConfig.apiBaseUrl + `/core/systemadmin/group-permissions/${groupId}`
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
            width: '900px',
            title: 'Group Details',
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
        if (window.GroupModule.Summary && typeof window.GroupModule.Summary.init === 'function') {
            window.GroupModule.Summary.init();
        }

        // Initialize form handlers
        if (window.GroupModule.Details && typeof window.GroupModule.Details.init === 'function') {
            window.GroupModule.Details.init();
        }

        // Attach event handlers
        attachEventHandlers();

        console.log('Group module initialized');
    });

    // Attach event handlers
    function attachEventHandlers() {
        // Add New Group button
        $('#btnAddGroup').on('click', function () {
            if (window.GroupModule.Details && typeof window.GroupModule.Details.openAddForm === 'function') {
                window.GroupModule.Details.openAddForm();
            }
        });

        // Refresh button
        $('#btnRefresh').on('click', function () {
            if (window.GroupModule.Summary && typeof window.GroupModule.Summary.refreshGrid === 'function') {
                window.GroupModule.Summary.refreshGrid();
                window.AppToast?.success('Grid refreshed successfully');
            }
        });
    }

})();
