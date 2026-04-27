/**
 * Course Settings - Initialization Module
 */

(function () {
    'use strict';

    window.CourseModule = window.CourseModule || {};
    const apiRouteBaseUrl = window.AppConfig.apiRouteBaseUrl || `${window.location.origin}/bdDevs-crm`;

    window.CourseModule.config = {
        apiEndpoints: {
            summary: `${apiRouteBaseUrl}/crm-course-summary`,
            create: `${apiRouteBaseUrl}/crm-course`,
            update: (id) => `${apiRouteBaseUrl}/crm-course/${id}`,
            delete: (id) => `${apiRouteBaseUrl}/crm-course/${id}`,
            read: (id) => `${apiRouteBaseUrl}/crm-course/${id}`,
            institutesDDL: `${apiRouteBaseUrl}/crm-institutes-ddl`,
            coursesByInstitute: (instituteId) => `${apiRouteBaseUrl}/crm-course-by-instituteid-ddl/${instituteId}`
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
