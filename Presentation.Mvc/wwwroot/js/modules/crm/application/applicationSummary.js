/**
 * CRM Application Summary - Grid Module
 * This file handles grid operations (list, search, delete) for CRM Applications
 */

(function () {
    'use strict';

    window.ApplicationModule = window.ApplicationModule || {};

    // Grid instance
    let grid = null;

    window.ApplicationModule.Summary = {
        init: initializeGrid,
        refreshGrid: refreshGrid,
        deleteApplication: deleteApplication
    };

    /**
     * Initialize Kendo Grid
     */
    function initializeGrid() {
        const dataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: window.ApplicationModule.config.apiEndpoints.summary,
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
                    id: 'applicationId',
                    fields: {
                        applicationId: { type: 'number' },
                        applicantName: { type: 'string' },
                        applicationDate: { type: 'date' },
                        statusName: { type: 'string' },
                        countryName: { type: 'string' },
                        instituteName: { type: 'string' },
                        courseName: { type: 'string' },
                        intakeMonth: { type: 'string' },
                        intakeYear: { type: 'number' },
                        isDraft: { type: 'boolean' }
                    }
                }
            },
            pageSize: window.ApplicationModule.config.gridOptions.pageSize,
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            error: function (e) {
                console.error('Grid data source error:', e);
                window.AppToast?.error('Error loading grid data');
            }
        });

        grid = $('#applicationGrid').kendoGrid({
            dataSource: dataSource,
            height: 550,
            sortable: window.ApplicationModule.config.gridOptions.sortable,
            filterable: window.ApplicationModule.config.gridOptions.filterable,
            pageable: window.ApplicationModule.config.gridOptions.pageable,
            columns: [
                {
                    field: 'applicationId',
                    title: 'App ID',
                    width: 90,
                    filterable: false
                },
                {
                    field: 'applicantName',
                    title: 'Applicant Name',
                    width: 200,
                    template: function (dataItem) {
                        return dataItem.applicantName || '<span style="color: #999;">-</span>';
                    }
                },
                {
                    field: 'applicationDate',
                    title: 'Application Date',
                    width: 140,
                    template: function (dataItem) {
                        if (dataItem.applicationDate) {
                            const date = new Date(dataItem.applicationDate);
                            return date.toLocaleDateString('en-GB');
                        }
                        return '-';
                    },
                    filterable: false
                },
                {
                    field: 'statusName',
                    title: 'Status',
                    width: 140,
                    template: function (dataItem) {
                        if (!dataItem.statusName) {
                            return '<span class="badge badge-secondary">Unknown</span>';
                        }

                        // Determine badge color based on status
                        let badgeClass = 'badge-secondary';
                        const status = dataItem.statusName.toLowerCase();

                        if (dataItem.isDraft === true || dataItem.isDraft === 1) {
                            badgeClass = 'badge-warning';
                        } else if (status.includes('approved') || status.includes('accepted')) {
                            badgeClass = 'badge-success';
                        } else if (status.includes('reject') || status.includes('cancelled')) {
                            badgeClass = 'badge-danger';
                        } else if (status.includes('pending') || status.includes('submitted')) {
                            badgeClass = 'badge-info';
                        } else if (status.includes('processing') || status.includes('review')) {
                            badgeClass = 'badge-primary';
                        }

                        return `<span class="badge ${badgeClass}">${dataItem.statusName}</span>`;
                    }
                },
                {
                    field: 'countryName',
                    title: 'Country',
                    width: 150,
                    template: function (dataItem) {
                        return dataItem.countryName || '<span style="color: #999;">-</span>';
                    }
                },
                {
                    field: 'instituteName',
                    title: 'Institute',
                    width: 200,
                    template: function (dataItem) {
                        return dataItem.instituteName || '<span style="color: #999;">-</span>';
                    }
                },
                {
                    field: 'courseName',
                    title: 'Course',
                    width: 200,
                    template: function (dataItem) {
                        return dataItem.courseName || '<span style="color: #999;">-</span>';
                    }
                },
                {
                    field: 'intakeMonth',
                    title: 'Intake',
                    width: 120,
                    template: function (dataItem) {
                        if (dataItem.intakeMonth && dataItem.intakeYear) {
                            return `${dataItem.intakeMonth} ${dataItem.intakeYear}`;
                        } else if (dataItem.intakeMonth) {
                            return dataItem.intakeMonth;
                        } else if (dataItem.intakeYear) {
                            return dataItem.intakeYear.toString();
                        }
                        return '<span style="color: #999;">-</span>';
                    },
                    filterable: false
                },
                {
                    title: 'Actions',
                    width: 180,
                    filterable: false,
                    sortable: false,
                    template: function (dataItem) {
                        return `
                            <button class="btn-grid-action btn-edit" data-id="${dataItem.applicationId}" title="Edit">
                                <span>✏️</span> Edit
                            </button>
                            <button class="btn-grid-action btn-delete" data-id="${dataItem.applicationId}" title="Delete">
                                <span>🗑️</span> Delete
                            </button>
                        `;
                    }
                }
            ],
            dataBound: onDataBound
        }).data('kendoGrid');

        console.log('CRM Application grid initialized');
    }

    /**
     * Handle grid data bound event
     */
    function onDataBound() {
        $('.btn-edit').off('click').on('click', function () {
            const applicationId = parseInt($(this).data('id'));
            if (window.ApplicationModule.Details && typeof window.ApplicationModule.Details.openEditForm === 'function') {
                window.ApplicationModule.Details.openEditForm(applicationId);
            }
        });

        $('.btn-delete').off('click').on('click', function () {
            const applicationId = parseInt($(this).data('id'));
            deleteApplication(applicationId);
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
     * Delete application
     * @param {number} applicationId - Application ID to delete
     */
    async function deleteApplication(applicationId) {
        if (!applicationId || applicationId <= 0) {
            window.AppToast?.error('Invalid application ID');
            return;
        }

        if (!confirm('Are you sure you want to delete this application?\n\nThis action cannot be undone.')) {
            return;
        }

        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.delete(
                window.ApplicationModule.config.apiEndpoints.delete(applicationId)
            );

            if (response.success) {
                window.AppToast?.success(response.message || 'Application deleted successfully');
                refreshGrid();
            } else {
                window.AppToast?.error(response.message || 'Failed to delete application');
            }
        } catch (error) {
            console.error('Error deleting application:', error);
            window.AppToast?.error(error.message || 'Error deleting application');
        } finally {
            window.AppLoader?.hide();
        }
    }

})();
