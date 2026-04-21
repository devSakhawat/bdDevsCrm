/**
 * Access Control Settings - Initialization Module
 */

(function () {
    'use strict';

    window.AccessControlModule = window.AccessControlModule || {};

    window.AccessControlModule.config = {
        apiEndpoints: {
            summary: window.AppConfig.apiBaseUrl + '/bdDevs-crm/access-control-summary',
            create: window.AppConfig.apiBaseUrl + '/bdDevs-crm/access-control',
            update: (id) => window.AppConfig.apiBaseUrl + `/bdDevs-crm/access-control/${id}`,
            delete: (id) => window.AppConfig.apiBaseUrl + `/bdDevs-crm/access-control/${id}`,
            read: (id) => window.AppConfig.apiBaseUrl + `/bdDevs-crm/access-control/${id}`
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
            title: 'Access Control',
            modal: true,
            visible: false,
            actions: ['Close']
        }
    };

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning('Session expired');
            setTimeout(() => window.location.href = '/Account/Login', 1500);
            return;
        }

        if (window.AccessControlModule.Summary) window.AccessControlModule.Summary.init();
        if (window.AccessControlModule.Details) window.AccessControlModule.Details.init();

        $('#btnAddAccessControl').on('click', () => window.AccessControlModule.Details?.openAddForm());
        $('#btnRefresh').on('click', () => {
            window.AccessControlModule.Summary?.refreshGrid();
            window.AppToast?.success('Grid refreshed');
        });

        console.log('Access Control module initialized');
    });

})();
