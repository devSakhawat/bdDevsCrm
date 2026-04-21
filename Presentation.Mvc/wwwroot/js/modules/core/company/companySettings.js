/**
 * Company Settings - Initialization Module
 * This file initializes the Company module when the page loads
 */

(function () {
    'use strict';

    // Module configuration
    window.CompanyModule = window.CompanyModule || {};

    window.CompanyModule.config = {
        apiEndpoints: {
            summary: window.AppConfig.apiBaseUrl + '/core/systemadmin/company-summary',
            create: window.AppConfig.apiBaseUrl + '/core/systemadmin/company',
            update: (id) => window.AppConfig.apiBaseUrl + `/core/systemadmin/company/${id}`,
            delete: (id) => window.AppConfig.apiBaseUrl + `/core/systemadmin/company/${id}`,
            read: (id) => window.AppConfig.apiBaseUrl + `/core/systemadmin/company/${id}`
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
            title: 'Company Details',
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
        if (window.CompanyModule.Summary && typeof window.CompanyModule.Summary.init === 'function') {
            window.CompanyModule.Summary.init();
        }

        // Initialize form handlers
        if (window.CompanyModule.Details && typeof window.CompanyModule.Details.init === 'function') {
            window.CompanyModule.Details.init();
        }

        // Attach event handlers
        attachEventHandlers();

        console.log('Company module initialized');
    });

    // Attach event handlers
    function attachEventHandlers() {
        // Add New Company button
        $('#btnAddCompany').on('click', function () {
            if (window.CompanyModule.Details && typeof window.CompanyModule.Details.openAddForm === 'function') {
                window.CompanyModule.Details.openAddForm();
            }
        });

        // Refresh button
        $('#btnRefresh').on('click', function () {
            if (window.CompanyModule.Summary && typeof window.CompanyModule.Summary.refreshGrid === 'function') {
                window.CompanyModule.Summary.refreshGrid();
                window.AppToast?.success('Grid refreshed successfully');
            }
        });
    }

})();
