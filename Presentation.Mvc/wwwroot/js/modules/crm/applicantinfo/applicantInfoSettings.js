/**
 * Applicant Info Settings - Initialization Module
 */

(function () {
    'use strict';

    window.ApplicantInfoModule = window.ApplicantInfoModule || {};
    const apiRouteBaseUrl = window.AppConfig.apiRouteBaseUrl || `${window.location.origin}/bdDevs-crm`;

    window.ApplicantInfoModule.config = {
        apiEndpoints: {
            summary: `${apiRouteBaseUrl}/crm-applicant-info-summary`,
            create: `${apiRouteBaseUrl}/crm-applicant-info`,
            update: (id) => `${apiRouteBaseUrl}/crm-applicant-info/${id}`,
            delete: (id) => `${apiRouteBaseUrl}/crm-applicant-info/${id}`,
            read: (id) => `${apiRouteBaseUrl}/crm-applicant-info/${id}`,
            gendersDDL: `${apiRouteBaseUrl}/genders-ddl`,
            maritalStatusesDDL: `${apiRouteBaseUrl}/marital-statuses-ddl`,
            uploadPhoto: `${apiRouteBaseUrl}/crm-applicant-info/upload-photo`
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
            title: 'Applicant Details',
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

        if (window.ApplicantInfoModule.Summary) window.ApplicantInfoModule.Summary.init();
        if (window.ApplicantInfoModule.Details) window.ApplicantInfoModule.Details.init();

        $('#btnAddApplicant').on('click', () => window.ApplicantInfoModule.Details?.openAddForm());
        $('#btnRefresh').on('click', () => {
            window.ApplicantInfoModule.Summary?.refreshGrid();
            window.AppToast?.success('Grid refreshed');
        });

        console.log('Applicant Info module initialized');
    });

})();
