/**
 * Course Summary - Grid Module
 * This file handles grid operations (list, search, delete) for Courses
 */

(function () {
    'use strict';

    window.CourseModule = window.CourseModule || {};

    // Grid instance
    let grid = null;

    window.CourseModule.Summary = {
        init: initializeGrid,
        refreshGrid: refreshGrid,
        deleteCourse: deleteCourse
    };

    /**
     * Initialize Kendo Grid
     */
    function initializeGrid() {
        const dataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: window.CourseModule.config.apiEndpoints.summary,
                    type: 'POST',
                    dataType: 'json',
                    contentType: 'application/json',
                    beforeSend: function (xhr) {
                        const token = window.ApiClient?.getToken();
                        if (token) {
                            xhr.setRequestHeader('Authorization', `Bearer ${token}`);
                        }
                    }
                },
                parameterMap: function (options, operation) {
                    if (operation === 'read') {
                        const gridOptions = {
                            page: options.page || 1,
                            pageSize: options.pageSize || 20,
                            sort: options.sort || [],
                            filter: options.filter || null
                        };
                        return JSON.stringify(gridOptions);
                    }
                    return kendo.stringify(options);
                }
            },
            schema: {
                data: function (response) {
                    if (response.success && response.data) {
                        return response.data.items || [];
                    }
                    return [];
                },
                total: function (response) {
                    if (response.success && response.data) {
                        return response.data.total || 0;
                    }
                    return 0;
                },
                model: {
                    id: 'courseId',
                    fields: {
                        courseId: { type: 'number' },
                        instituteId: { type: 'number' },
                        instituteName: { type: 'string' },
                        courseTitle: { type: 'string' },
                        courseLevel: { type: 'string' },
                        courseCategory: { type: 'string' },
                        courseDuration: { type: 'string' },
                        courseFee: { type: 'number' },
                        applicationFee: { type: 'number' },
                        startDate: { type: 'string' },
                        endDate: { type: 'string' },
                        status: { type: 'number' }
                    }
                }
            },
            pageSize: window.CourseModule.config.gridOptions.pageSize,
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            error: function (e) {
                console.error('Grid data source error:', e);
                window.AppToast?.error('Error loading grid data');
            }
        });

        grid = $('#courseGrid').kendoGrid({
            dataSource: dataSource,
            height: 550,
            sortable: window.CourseModule.config.gridOptions.sortable,
            filterable: window.CourseModule.config.gridOptions.filterable,
            pageable: window.CourseModule.config.gridOptions.pageable,
            columns: [
                {
                    field: 'courseId',
                    title: 'ID',
                    width: 80,
                    filterable: false
                },
                {
                    field: 'courseTitle',
                    title: 'Course Title',
                    width: 250
                },
                {
                    field: 'instituteName',
                    title: 'Institute',
                    width: 200
                },
                {
                    field: 'courseLevel',
                    title: 'Level',
                    width: 130,
                    template: function (dataItem) {
                        return dataItem.courseLevel || '<span style="color: #999;">-</span>';
                    }
                },
                {
                    field: 'courseCategory',
                    title: 'Category',
                    width: 150,
                    template: function (dataItem) {
                        return dataItem.courseCategory || '<span style="color: #999;">-</span>';
                    }
                },
                {
                    field: 'courseDuration',
                    title: 'Duration',
                    width: 120,
                    template: function (dataItem) {
                        return dataItem.courseDuration || '-';
                    },
                    filterable: false
                },
                {
                    field: 'courseFee',
                    title: 'Course Fee',
                    width: 130,
                    template: function (dataItem) {
                        if (dataItem.courseFee && dataItem.courseFee > 0) {
                            return dataItem.courseFee.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
                        }
                        return '<span style="color: #999;">-</span>';
                    },
                    filterable: false
                },
                {
                    field: 'applicationFee',
                    title: 'App Fee',
                    width: 120,
                    template: function (dataItem) {
                        if (dataItem.applicationFee && dataItem.applicationFee > 0) {
                            return dataItem.applicationFee.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
                        }
                        return '<span style="color: #999;">-</span>';
                    },
                    filterable: false
                },
                {
                    field: 'startDate',
                    title: 'Start Date',
                    width: 120,
                    template: function (dataItem) {
                        if (dataItem.startDate) {
                            const date = new Date(dataItem.startDate);
                            return date.toLocaleDateString('en-GB');
                        }
                        return '-';
                    },
                    filterable: false
                },
                {
                    field: 'status',
                    title: 'Status',
                    width: 120,
                    template: function (dataItem) {
                        if (dataItem.status === 1 || dataItem.status === true) {
                            return '<span class="badge badge-success">Active</span>';
                        } else {
                            return '<span class="badge badge-inactive">Inactive</span>';
                        }
                    },
                    filterable: {
                        ui: function (element) {
                            element.kendoDropDownList({
                                dataSource: [
                                    { text: 'All', value: '' },
                                    { text: 'Active', value: '1' },
                                    { text: 'Inactive', value: '0' }
                                ],
                                dataTextField: 'text',
                                dataValueField: 'value',
                                optionLabel: 'All'
                            });
                        }
                    }
                },
                {
                    title: 'Actions',
                    width: 180,
                    filterable: false,
                    sortable: false,
                    template: function (dataItem) {
                        return `
                            <button class="btn-grid-action btn-edit" data-id="${dataItem.courseId}" title="Edit">
                                <span>✏️</span> Edit
                            </button>
                            <button class="btn-grid-action btn-delete" data-id="${dataItem.courseId}" title="Delete">
                                <span>🗑️</span> Delete
                            </button>
                        `;
                    }
                }
            ],
            dataBound: onDataBound
        }).data('kendoGrid');

        console.log('Course grid initialized');
    }

    /**
     * Handle grid data bound event
     */
    function onDataBound() {
        $('.btn-edit').off('click').on('click', function () {
            const courseId = parseInt($(this).data('id'));
            if (window.CourseModule.Details && typeof window.CourseModule.Details.openEditForm === 'function') {
                window.CourseModule.Details.openEditForm(courseId);
            }
        });

        $('.btn-delete').off('click').on('click', function () {
            const courseId = parseInt($(this).data('id'));
            deleteCourse(courseId);
        });
    }

    /**
     * Refresh grid
     */
    function refreshGrid() {
        if (grid) {
            grid.dataSource.read();
        }
    }

    /**
     * Delete course
     * @param {number} courseId - Course ID to delete
     */
    async function deleteCourse(courseId) {
        if (!courseId || courseId <= 0) {
            window.AppToast?.error('Invalid course ID');
            return;
        }

        if (!confirm('Are you sure you want to delete this course?\n\nThis action cannot be undone.')) {
            return;
        }

        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.delete(
                window.CourseModule.config.apiEndpoints.delete(courseId)
            );

            if (response.success) {
                window.AppToast?.success(response.message || 'Course deleted successfully');
                refreshGrid();
            } else {
                window.AppToast?.error(response.message || 'Failed to delete course');
            }
        } catch (error) {
            console.error('Error deleting course:', error);
            window.AppToast?.error(error.message || 'Error deleting course');
        } finally {
            window.AppLoader?.hide();
        }
    }

})();
