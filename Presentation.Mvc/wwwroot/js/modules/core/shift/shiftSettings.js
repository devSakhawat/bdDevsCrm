/**
 * Shift Settings - Initialization Module
 * This file initializes the Shift module when the page loads
 */

(function () {
    'use strict';

    // Module configuration
    window.ShiftModule = window.ShiftModule || {};

    window.ShiftModule.config = {
        apiEndpoints: {
            summary: window.AppConfig.apiBaseUrl + '/core/hr/shift-summary',
            create: window.AppConfig.apiBaseUrl + '/core/hr/shift',
            update: (id) => window.AppConfig.apiBaseUrl + `/core/hr/shift/${id}`,
            delete: (id) => window.AppConfig.apiBaseUrl + `/core/hr/shift/${id}`,
            read: (id) => window.AppConfig.apiBaseUrl + `/core/hr/shift/${id}`,
            shiftsDDL: window.AppConfig.apiBaseUrl + '/core/hr/shifts-ddl'
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
            title: 'Shift Details',
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
        if (window.ShiftModule.Summary && typeof window.ShiftModule.Summary.init === 'function') {
            window.ShiftModule.Summary.init();
        }

        // Initialize form handlers
        if (window.ShiftModule.Details && typeof window.ShiftModule.Details.init === 'function') {
            window.ShiftModule.Details.init();
        }

        // Attach event handlers
        attachEventHandlers();

        console.log('Shift module initialized');
    });

    // Attach event handlers
    function attachEventHandlers() {
        // Add New Shift button
        $('#btnAddShift').on('click', function () {
            if (window.ShiftModule.Details && typeof window.ShiftModule.Details.openAddForm === 'function') {
                window.ShiftModule.Details.openAddForm();
            }
        });

        // Refresh button
        $('#btnRefresh').on('click', function () {
            if (window.ShiftModule.Summary && typeof window.ShiftModule.Summary.refreshGrid === 'function') {
                window.ShiftModule.Summary.refreshGrid();
                window.AppToast?.success('Grid refreshed successfully');
            }
        });
    }

})();
