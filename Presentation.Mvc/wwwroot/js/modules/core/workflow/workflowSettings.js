/**
 * Workflow Settings - Initialization Module
 * Handles States + Actions configuration
 */

(function () {
    'use strict';

    window.WorkflowModule = window.WorkflowModule || {};

    window.WorkflowModule.config = {
        apiEndpoints: {
            // State endpoints
            stateSummary: window.AppConfig.apiBaseUrl + '/bdDevs-crm/workflow-summary',
            stateCreate: window.AppConfig.apiBaseUrl + '/bdDevs-crm/workflow-state',
            stateUpdate: (id) => window.AppConfig.apiBaseUrl + `/bdDevs-crm/workflow-state/${id}`,
            stateDelete: (id) => window.AppConfig.apiBaseUrl + `/bdDevs-crm/workflow-state/${id}`,
            stateRead: (id) => window.AppConfig.apiBaseUrl + `/bdDevs-crm/workflow-state/${id}`,

            // Action endpoints
            actionSummary: window.AppConfig.apiBaseUrl + '/bdDevs-crm/workflow-action-summary',
            actionCreate: window.AppConfig.apiBaseUrl + '/bdDevs-crm/workflow-action',
            actionUpdate: (id) => window.AppConfig.apiBaseUrl + `/bdDevs-crm/workflow-action/${id}`,
            actionDelete: (id) => window.AppConfig.apiBaseUrl + `/bdDevs-crm/workflow-action/${id}`,
            actionRead: (id) => window.AppConfig.apiBaseUrl + `/bdDevs-crm/workflow-action/${id}`,

            // Dropdown endpoints
            menusDDL: window.AppConfig.apiBaseUrl + '/bdDevs-crm/menus-ddl',
            statesDDL: window.AppConfig.apiBaseUrl + '/bdDevs-crm/workflow-states-ddl'
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
        stateWindowOptions: {
            width: '600px',
            title: 'Workflow State',
            modal: true,
            visible: false,
            actions: ['Close']
        },
        actionWindowOptions: {
            width: '600px',
            title: 'Workflow Action',
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

        if (window.WorkflowModule.Summary) window.WorkflowModule.Summary.init();
        if (window.WorkflowModule.Details) window.WorkflowModule.Details.init();

        $('#btnRefresh').on('click', () => {
            const activeTab = $('#workflowTabStrip').data('kendoTabStrip').select().index();
            if (activeTab === 0) {
                window.WorkflowModule.Summary?.refreshStateGrid();
            } else {
                window.WorkflowModule.Summary?.refreshActionGrid();
            }
            window.AppToast?.success('Grid refreshed');
        });

        console.log('Workflow module initialized with States + Actions configuration');
    });

})();
