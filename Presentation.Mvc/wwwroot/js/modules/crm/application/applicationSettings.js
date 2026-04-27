/**
 * CRM Application Settings - Initialization Module
 * Handles 7-tab application form configuration
 */

(function () {
    'use strict';

    window.ApplicationModule = window.ApplicationModule || {};
    const apiRouteBaseUrl = window.AppConfig.apiRouteBaseUrl || `${window.location.origin}/bdDevs-crm`;

    window.ApplicationModule.config = {
        apiEndpoints: {
            summary: `${apiRouteBaseUrl}/crm-application-summary`,
            create: `${apiRouteBaseUrl}/crm-application`,
            update: (id) => `${apiRouteBaseUrl}/crm-application/${id}`,
            delete: (id) => `${apiRouteBaseUrl}/crm-application/${id}`,
            read: (id) => `${apiRouteBaseUrl}/crm-application/${id}`,
            applicantsDDL: `${apiRouteBaseUrl}/crm-applicant-infos-ddl`,
            statusesDDL: `${apiRouteBaseUrl}/crm-application-statuses-ddl`,
            countriesDDL: `${apiRouteBaseUrl}/countryddl`,
            institutesDDL: (countryId) => `${apiRouteBaseUrl}/crm-institut-by-countryid-ddl/${countryId}`,
            coursesDDL: (instituteId) => `${apiRouteBaseUrl}/crm-course-by-instituteid-ddl/${instituteId}`,
            intakeMonthsDDL: `${apiRouteBaseUrl}/intake-month-ddl`,
            uploadDocument: `${apiRouteBaseUrl}/crm-application/upload-document`
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
