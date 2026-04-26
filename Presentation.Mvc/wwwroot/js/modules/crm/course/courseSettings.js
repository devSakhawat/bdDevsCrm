/**
 * Course Settings - Initialization Module
 */

(function () {
    'use strict';

    window.CourseModule = window.CourseModule || {};

    window.CourseModule.config = {
        apiEndpoints: {
            summary: window.AppConfig.apiBaseUrl + '/crm/crm-course-summary',
            create: window.AppConfig.apiBaseUrl + '/crm/crm-course',
            update: (id) => window.AppConfig.apiBaseUrl + `/crm/crm-course/${id}`,
            delete: (id) => window.AppConfig.apiBaseUrl + `/crm/crm-course/${id}`,
            read: (id) => window.AppConfig.apiBaseUrl + `/crm/crm-course/${id}`,
            institutesDDL: window.AppConfig.apiBaseUrl + '/crm/crm-institutes-ddl',
            coursesByInstitute: (instituteId) => window.AppConfig.apiBaseUrl + `/crm/crm-courses-by-institute/${instituteId}`
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
            title: 'Course Details',
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

        if (window.CourseModule.Summary) window.CourseModule.Summary.init();
        if (window.CourseModule.Details) window.CourseModule.Details.init();

        $('#btnAddCourse').on('click', () => window.CourseModule.Details?.openAddForm());
        $('#btnRefresh').on('click', () => {
            window.CourseModule.Summary?.refreshGrid();
            window.AppToast?.success('Grid refreshed');
        });

        console.log('Course module initialized');
    });

})();
