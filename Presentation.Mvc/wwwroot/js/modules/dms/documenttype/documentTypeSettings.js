/**
 * Document Type Settings - Initialization Module
 * This file initializes the Document Type module when the page loads
 */

(function () {
    'use strict';

    // Module configuration
    window.DocumentTypeModule = window.DocumentTypeModule || {};

    window.DocumentTypeModule.config = {
        apiEndpoints: {
            summary: window.AppConfig.apiBaseUrl + '/dms/document-type-summary',
            create: window.AppConfig.apiBaseUrl + '/dms/document-type',
            update: (id) => window.AppConfig.apiBaseUrl + `/dms/document-type/${id}`,
            delete: (id) => window.AppConfig.apiBaseUrl + `/dms/document-type/${id}`,
            read: (id) => window.AppConfig.apiBaseUrl + `/dms/document-type/${id}`
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
            title: 'Document Type Details',
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
        if (window.DocumentTypeModule.Summary && typeof window.DocumentTypeModule.Summary.init === 'function') {
            window.DocumentTypeModule.Summary.init();
        }

        // Initialize form handlers
        if (window.DocumentTypeModule.Details && typeof window.DocumentTypeModule.Details.init === 'function') {
            window.DocumentTypeModule.Details.init();
        }

        // Attach event handlers
        attachEventHandlers();

        console.log('Document Type module initialized');
    });

    // Attach event handlers
    function attachEventHandlers() {
        // Add New Document Type button
        $('#btnAddDocumentType').on('click', function () {
            if (window.DocumentTypeModule.Details && typeof window.DocumentTypeModule.Details.openAddForm === 'function') {
                window.DocumentTypeModule.Details.openAddForm();
            }
        });

        // Refresh button
        $('#btnRefresh').on('click', function () {
            if (window.DocumentTypeModule.Summary && typeof window.DocumentTypeModule.Summary.refreshGrid === 'function') {
                window.DocumentTypeModule.Summary.refreshGrid();
                window.AppToast?.success('Grid refreshed successfully');
            }
        });
    }

})();
