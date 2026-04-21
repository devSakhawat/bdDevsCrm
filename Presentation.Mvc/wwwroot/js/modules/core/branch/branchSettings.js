/**
 * Branch Settings - Initialization Module
 * This file initializes the Branch module when the page loads
 */

(function () {
    'use strict';

    // Module configuration
    window.BranchModule = window.BranchModule || {};

    window.BranchModule.config = {
        apiEndpoints: {
            summary: window.AppConfig.apiBaseUrl + '/core/hr/branch-summary',
            create: window.AppConfig.apiBaseUrl + '/core/hr/branch',
            update: (id) => window.AppConfig.apiBaseUrl + `/core/hr/branch/${id}`,
            delete: (id) => window.AppConfig.apiBaseUrl + `/core/hr/branch/${id}`,
            read: (id) => window.AppConfig.apiBaseUrl + `/core/hr/branch/${id}`,
            companiesDDL: window.AppConfig.apiBaseUrl + '/core/systemadmin/companies-ddl',
            branchesDDL: (companyId) => window.AppConfig.apiBaseUrl + `/core/hr/branches-ddl?companyId=${companyId}`
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
            title: 'Branch Details',
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
        if (window.BranchModule.Summary && typeof window.BranchModule.Summary.init === 'function') {
            window.BranchModule.Summary.init();
        }

        // Initialize form handlers
        if (window.BranchModule.Details && typeof window.BranchModule.Details.init === 'function') {
            window.BranchModule.Details.init();
        }

        // Attach event handlers
        attachEventHandlers();

        console.log('Branch module initialized');
    });

    // Attach event handlers
    function attachEventHandlers() {
        // Add New Branch button
        $('#btnAddBranch').on('click', function () {
            if (window.BranchModule.Details && typeof window.BranchModule.Details.openAddForm === 'function') {
                window.BranchModule.Details.openAddForm();
            }
        });

        // Refresh button
        $('#btnRefresh').on('click', function () {
            if (window.BranchModule.Summary && typeof window.BranchModule.Summary.refreshGrid === 'function') {
                window.BranchModule.Summary.refreshGrid();
                window.AppToast?.success('Grid refreshed successfully');
            }
        });
    }

})();
