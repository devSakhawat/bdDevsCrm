/**
 * Applicant Info Summary - Grid Module
 * This file handles grid operations (list, search, delete) for Applicant Information
 */

(function () {
    'use strict';

    window.ApplicantInfoModule = window.ApplicantInfoModule || {};

    // Grid instance
    let grid = null;

    window.ApplicantInfoModule.Summary = {
        init: initializeGrid,
        refreshGrid: refreshGrid,
        deleteApplicant: deleteApplicant
    };

    /**
     * Initialize Kendo Grid
     */
    function initializeGrid() {
        const dataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: window.ApplicantInfoModule.config.apiEndpoints.summary,
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
                    id: 'applicantId',
                    fields: {
                        applicantId: { type: 'number' },
                        firstName: { type: 'string' },
                        lastName: { type: 'string' },
                        genderName: { type: 'string' },
                        dateOfBirth: { type: 'date' },
                        emailAddress: { type: 'string' },
                        mobile: { type: 'string' },
                        nationality: { type: 'string' },
                        passportNumber: { type: 'string' },
                        hasValidPassport: { type: 'boolean' }
                    }
                }
            },
            pageSize: window.ApplicantInfoModule.config.gridOptions.pageSize,
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            error: function (e) {
                console.error('Grid data source error:', e);
                window.AppToast?.error('Error loading grid data');
            }
        });

        grid = $('#applicantGrid').kendoGrid({
            dataSource: dataSource,
            height: 550,
            sortable: window.ApplicantInfoModule.config.gridOptions.sortable,
            filterable: window.ApplicantInfoModule.config.gridOptions.filterable,
            pageable: window.ApplicantInfoModule.config.gridOptions.pageable,
            columns: [
                {
                    field: 'applicantId',
                    title: 'ID',
                    width: 80,
                    filterable: false
                },
                {
                    field: 'firstName',
                    title: 'First Name',
                    width: 150
                },
                {
                    field: 'lastName',
                    title: 'Last Name',
                    width: 150
                },
                {
                    field: 'genderName',
                    title: 'Gender',
                    width: 100,
                    template: function (dataItem) {
                        return dataItem.genderName || '<span style="color: #999;">-</span>';
                    }
                },
                {
                    field: 'dateOfBirth',
                    title: 'Date of Birth',
                    width: 130,
                    template: function (dataItem) {
                        if (dataItem.dateOfBirth) {
                            const date = new Date(dataItem.dateOfBirth);
                            return date.toLocaleDateString('en-GB');
                        }
                        return '-';
                    },
                    filterable: false
                },
                {
                    field: 'emailAddress',
                    title: 'Email',
                    width: 200
                },
                {
                    field: 'mobile',
                    title: 'Mobile',
                    width: 140,
                    template: function (dataItem) {
                        return dataItem.mobile || '<span style="color: #999;">-</span>';
                    },
                    filterable: false
                },
                {
                    field: 'nationality',
                    title: 'Nationality',
                    width: 130,
                    template: function (dataItem) {
                        return dataItem.nationality || '<span style="color: #999;">-</span>';
                    }
                },
                {
                    field: 'passportNumber',
                    title: 'Passport No',
                    width: 130,
                    template: function (dataItem) {
                        return dataItem.passportNumber || '<span style="color: #999;">-</span>';
                    }
                },
                {
                    field: 'hasValidPassport',
                    title: 'Valid Passport',
                    width: 130,
                    template: function (dataItem) {
                        if (dataItem.hasValidPassport === true || dataItem.hasValidPassport === 1) {
                            return '<span class="badge badge-success">Yes</span>';
                        } else {
                            return '<span class="badge badge-inactive">No</span>';
                        }
                    },
                    filterable: {
                        ui: function (element) {
                            element.kendoDropDownList({
                                dataSource: [
                                    { text: 'All', value: '' },
                                    { text: 'Yes', value: 'true' },
                                    { text: 'No', value: 'false' }
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
                            <button class="btn-grid-action btn-edit" data-id="${dataItem.applicantId}" title="Edit">
                                <span>✏️</span> Edit
                            </button>
                            <button class="btn-grid-action btn-delete" data-id="${dataItem.applicantId}" title="Delete">
                                <span>🗑️</span> Delete
                            </button>
                        `;
                    }
                }
            ],
            dataBound: onDataBound
        }).data('kendoGrid');

        console.log('Applicant Info grid initialized');
    }

    /**
     * Handle grid data bound event
     */
    function onDataBound() {
        $('.btn-edit').off('click').on('click', function () {
            const applicantId = parseInt($(this).data('id'));
            if (window.ApplicantInfoModule.Details && typeof window.ApplicantInfoModule.Details.openEditForm === 'function') {
                window.ApplicantInfoModule.Details.openEditForm(applicantId);
            }
        });

        $('.btn-delete').off('click').on('click', function () {
            const applicantId = parseInt($(this).data('id'));
            deleteApplicant(applicantId);
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
     * Delete applicant
     * @param {number} applicantId - Applicant ID to delete
     */
    async function deleteApplicant(applicantId) {
        if (!applicantId || applicantId <= 0) {
            window.AppToast?.error('Invalid applicant ID');
            return;
        }

        if (!confirm('Are you sure you want to delete this applicant?\n\nThis action cannot be undone.')) {
            return;
        }

        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.delete(
                window.ApplicantInfoModule.config.apiEndpoints.delete(applicantId)
            );

            if (response.success) {
                window.AppToast?.success(response.message || 'Applicant deleted successfully');
                refreshGrid();
            } else {
                window.AppToast?.error(response.message || 'Failed to delete applicant');
            }
        } catch (error) {
            console.error('Error deleting applicant:', error);
            window.AppToast?.error(error.message || 'Error deleting applicant');
        } finally {
            window.AppLoader?.hide();
        }
    }

})();
