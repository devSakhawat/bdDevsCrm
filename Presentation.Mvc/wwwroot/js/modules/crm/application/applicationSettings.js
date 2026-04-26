/**
 * CRM Application Settings - Initialization Module
 * Handles 7-tab application form configuration
 */

(function () {
    'use strict';

    window.ApplicationModule = window.ApplicationModule || {};

    window.ApplicationModule.config = {
        apiEndpoints: {
            summary: window.AppConfig.apiBaseUrl + '/crm/application-summary',
            create: window.AppConfig.apiBaseUrl + '/crm/application',
            update: (id) => window.AppConfig.apiBaseUrl + `/crm/application/${id}`,
            delete: (id) => window.AppConfig.apiBaseUrl + `/crm/application/${id}`,
            read: (id) => window.AppConfig.apiBaseUrl + `/crm/application/${id}`,
            applicantsDDL: window.AppConfig.apiBaseUrl + '/crm/applicants-ddl',
            statusesDDL: window.AppConfig.apiBaseUrl + '/core/systemadmin/statuses-ddl',
            countriesDDL: window.AppConfig.apiBaseUrl + '/core/systemadmin/countries-ddl',
            institutesDDL: (countryId) => window.AppConfig.apiBaseUrl + `/crm/institutes-by-country/${countryId}`,
            coursesDDL: (instituteId) => window.AppConfig.apiBaseUrl + `/crm/courses-by-institute/${instituteId}`,
            intakeMonthsDDL: window.AppConfig.apiBaseUrl + '/crm/intake-months-ddl',
            uploadDocument: window.AppConfig.apiBaseUrl + '/crm/application/upload-document'
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
            width: '1000px',
            height: '700px',
            title: 'CRM Application Form',
            modal: true,
            visible: false,
            actions: ['Close']
        },
        tabs: [
            { id: 'tab-basic', title: 'Basic Information' },
            { id: 'tab-personal', title: 'Personal Information' },
            { id: 'tab-education', title: 'Education History' },
            { id: 'tab-work', title: 'Work Experience' },
            { id: 'tab-documents', title: 'Documents' },
            { id: 'tab-course', title: 'Course Selection' },
            { id: 'tab-review', title: 'Review & Submit' }
        ]
    };

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning('Session expired');
            setTimeout(() => window.location.href = '/Account/Login', 1500);
            return;
        }

        if (window.ApplicationModule.Summary) window.ApplicationModule.Summary.init();
        if (window.ApplicationModule.Details) window.ApplicationModule.Details.init();

        $('#btnAddApplication').on('click', () => window.ApplicationModule.Details?.openAddForm());
        $('#btnRefresh').on('click', () => {
            window.ApplicationModule.Summary?.refreshGrid();
            window.AppToast?.success('Grid refreshed');
        });

        console.log('CRM Application module initialized with 7-tab configuration');
    });

})();
