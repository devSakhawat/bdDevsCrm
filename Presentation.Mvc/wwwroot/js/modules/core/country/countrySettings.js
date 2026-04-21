/**
 * Country Settings - Initialization Module
 * This file initializes the Country module when the page loads
 */

(function () {
    'use strict';

    // Module configuration
    window.CountryModule = window.CountryModule || {};

    window.CountryModule.config = {
        apiEndpoints: {
            summary: window.AppConfig.apiBaseUrl + '/core/systemadmin/country-summary',
            create: window.AppConfig.apiBaseUrl + '/core/systemadmin/country',
            update: (id) => window.AppConfig.apiBaseUrl + `/core/systemadmin/country/${id}`,
            delete: (id) => window.AppConfig.apiBaseUrl + `/core/systemadmin/country/${id}`,
            read: (id) => window.AppConfig.apiBaseUrl + `/core/systemadmin/country/${id}`
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
            title: 'Country Details',
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
        if (window.CountryModule.Summary && typeof window.CountryModule.Summary.init === 'function') {
            window.CountryModule.Summary.init();
        }

        // Initialize form handlers
        if (window.CountryModule.Details && typeof window.CountryModule.Details.init === 'function') {
            window.CountryModule.Details.init();
        }

        // Attach event handlers
        attachEventHandlers();

        console.log('Country module initialized');
    });

    // Attach event handlers
    function attachEventHandlers() {
        // Add New Country button
        $('#btnAddCountry').on('click', function () {
            if (window.CountryModule.Details && typeof window.CountryModule.Details.openAddForm === 'function') {
                window.CountryModule.Details.openAddForm();
            }
        });

        // Refresh button
        $('#btnRefresh').on('click', function () {
            if (window.CountryModule.Summary && typeof window.CountryModule.Summary.refreshGrid === 'function') {
                window.CountryModule.Summary.refreshGrid();
                window.AppToast?.success('Grid refreshed successfully');
            }
        });
    }

})();
