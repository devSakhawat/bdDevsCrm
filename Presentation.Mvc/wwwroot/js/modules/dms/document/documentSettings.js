/**
 * Document Settings - Initialization Module
 * This file initializes the Document Management module when the page loads
 */

(function () {
    'use strict';

    // Module configuration
    window.DocumentModule = window.DocumentModule || {};

    window.DocumentModule.config = {
        apiEndpoints: {
            summary: window.AppConfig.apiBaseUrl + '/dms/document-summary',
            create: window.AppConfig.apiBaseUrl + '/dms/document',
            update: (id) => window.AppConfig.apiBaseUrl + `/dms/document/${id}`,
            delete: (id) => window.AppConfig.apiBaseUrl + `/dms/document/${id}`,
            read: (id) => window.AppConfig.apiBaseUrl + `/dms/document/${id}`,
            download: (id) => window.AppConfig.apiBaseUrl + `/dms/document/${id}/download`,
            upload: window.AppConfig.apiBaseUrl + '/dms/document/upload',
            documentTypesDDL: window.AppConfig.apiBaseUrl + '/dms/document-types-ddl',
            documentFoldersDDL: window.AppConfig.apiBaseUrl + '/dms/document-folders-ddl',
            restrictedUsersDDL: window.AppConfig.apiBaseUrl + '/core/systemadmin/users-ddl'
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
            height: '700px',
            title: 'Document Details',
            modal: true,
            visible: false,
            actions: ['Close']
        },
        upload: {
            maxFileSize: 50 * 1024 * 1024, // 50 MB
            allowedExtensions: [
                '.pdf', '.doc', '.docx', '.xls', '.xlsx', '.ppt', '.pptx',
                '.txt', '.csv', '.jpg', '.jpeg', '.png', '.gif', '.zip', '.rar'
            ]
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
        if (window.DocumentModule.Summary && typeof window.DocumentModule.Summary.init === 'function') {
            window.DocumentModule.Summary.init();
        }

        // Initialize form handlers
        if (window.DocumentModule.Details && typeof window.DocumentModule.Details.init === 'function') {
            window.DocumentModule.Details.init();
        }

        // Attach event handlers
        attachEventHandlers();

        console.log('Document Management module initialized');
    });

    // Attach event handlers
    function attachEventHandlers() {
        // Add New Document button
        $('#btnAddDocument').on('click', function () {
            if (window.DocumentModule.Details && typeof window.DocumentModule.Details.openAddForm === 'function') {
                window.DocumentModule.Details.openAddForm();
            }
        });

        // Refresh button
        $('#btnRefresh').on('click', function () {
            if (window.DocumentModule.Summary && typeof window.DocumentModule.Summary.refreshGrid === 'function') {
                window.DocumentModule.Summary.refreshGrid();
                window.AppToast?.success('Grid refreshed successfully');
            }
        });
    }

})();
