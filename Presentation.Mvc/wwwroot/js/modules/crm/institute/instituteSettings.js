/**
 * Institute Settings - Initialization Module
 * This file initializes the Institute module when the page loads
 */

(function () {
    'use strict';

    // Module configuration
    window.InstituteModule = window.InstituteModule || {};

    window.InstituteModule.config = {
        apiEndpoints: {
            summary: window.AppConfig.apiBaseUrl + '/crm/crm-institute-summary',
            create: window.AppConfig.apiBaseUrl + '/crm/crm-institute',
            update: (id) => window.AppConfig.apiBaseUrl + `/crm/crm-institute/${id}`,
            delete: (id) => window.AppConfig.apiBaseUrl + `/crm/crm-institute/${id}`,
            read: (id) => window.AppConfig.apiBaseUrl + `/crm/crm-institute/${id}`,
            countriesDDL: window.AppConfig.apiBaseUrl + '/core/systemadmin/countries-ddl',
            institutesByCountry: (countryId) => window.AppConfig.apiBaseUrl + `/crm/crm-institutes-by-country/${countryId}`
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
            width: '800px',
            title: 'Institute Details',
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
        if (window.InstituteModule.Summary && typeof window.InstituteModule.Summary.init === 'function') {
            window.InstituteModule.Summary.init();
        }

        // Initialize form handlers
        if (window.InstituteModule.Details && typeof window.InstituteModule.Details.init === 'function') {
            window.InstituteModule.Details.init();
        }

        // Attach event handlers
        attachEventHandlers();

        console.log('Institute module initialized');
    });

    // Attach event handlers
    function attachEventHandlers() {
        // Add New Institute button
        $('#btnAddInstitute').on('click', function () {
            if (window.InstituteModule.Details && typeof window.InstituteModule.Details.openAddForm === 'function') {
                window.InstituteModule.Details.openAddForm();
            }
        });

        // Refresh button
        $('#btnRefresh').on('click', function () {
            if (window.InstituteModule.Summary && typeof window.InstituteModule.Summary.refreshGrid === 'function') {
                window.InstituteModule.Summary.refreshGrid();
                window.AppToast?.success('Grid refreshed successfully');
            }
        });
    }

})();
