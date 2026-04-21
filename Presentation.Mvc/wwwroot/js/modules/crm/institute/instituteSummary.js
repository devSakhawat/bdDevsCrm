/**
 * Institute Summary - Grid Module
 * This file handles grid operations (list, search, delete) for Institutes
 */

(function () {
    'use strict';

    window.InstituteModule = window.InstituteModule || {};

    // Grid instance
    let grid = null;

    window.InstituteModule.Summary = {
        init: initializeGrid,
        refreshGrid: refreshGrid,
        deleteInstitute: deleteInstitute
    };

    /**
     * Initialize Kendo Grid
     */
    function initializeGrid() {
        const dataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: window.InstituteModule.config.apiEndpoints.summary,
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
                    id: 'instituteId',
                    fields: {
                        instituteId: { type: 'number' },
                        instituteName: { type: 'string' },
                        instituteCode: { type: 'string' },
                        countryName: { type: 'string' },
                        instituteEmail: { type: 'string' },
                        institutePhoneNo: { type: 'string' },
                        campus: { type: 'string' },
                        status: { type: 'boolean' }
                    }
                }
            },
            pageSize: window.InstituteModule.config.gridOptions.pageSize,
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            error: function (e) {
                console.error('Grid data source error:', e);
                window.AppToast?.error('Error loading grid data');
            }
        });

        grid = $('#instituteGrid').kendoGrid({
            dataSource: dataSource,
            height: 550,
            sortable: window.InstituteModule.config.gridOptions.sortable,
            filterable: window.InstituteModule.config.gridOptions.filterable,
            pageable: window.InstituteModule.config.gridOptions.pageable,
            columns: [
                {
                    field: 'instituteId',
                    title: 'ID',
                    width: 80,
                    filterable: false
                },
                {
                    field: 'instituteName',
                    title: 'Institute Name',
                    width: 250
                },
                {
                    field: 'instituteCode',
                    title: 'Code',
                    width: 120,
                    template: function (dataItem) {
                        return dataItem.instituteCode || '<span style="color: #999;">-</span>';
                    }
                },
                {
                    field: 'countryName',
                    title: 'Country',
                    width: 150
                },
                {
                    field: 'campus',
                    title: 'Campus',
                    width: 150,
                    template: function (dataItem) {
                        return dataItem.campus || '<span style="color: #999;">-</span>';
                    }
                },
                {
                    field: 'instituteEmail',
                    title: 'Email',
                    width: 200,
                    template: function (dataItem) {
                        return dataItem.instituteEmail || '<span style="color: #999;">-</span>';
                    }
                },
                {
                    field: 'institutePhoneNo',
                    title: 'Phone',
                    width: 150,
                    template: function (dataItem) {
                        return dataItem.institutePhoneNo || '<span style="color: #999;">-</span>';
                    }
                },
                {
                    field: 'status',
                    title: 'Status',
                    width: 120,
                    template: function (dataItem) {
                        if (dataItem.status === true || dataItem.status === 1) {
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
                                    { text: 'Active', value: 'true' },
                                    { text: 'Inactive', value: 'false' }
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
                            <button class="btn-grid-action btn-edit" data-id="${dataItem.instituteId}" title="Edit">
                                <span>✏️</span> Edit
                            </button>
                            <button class="btn-grid-action btn-delete" data-id="${dataItem.instituteId}" title="Delete">
                                <span>🗑️</span> Delete
                            </button>
                        `;
                    }
                }
            ],
            dataBound: onDataBound
        }).data('kendoGrid');

        console.log('Institute grid initialized');
    }

    /**
     * Handle grid data bound event
     */
    function onDataBound() {
        $('.btn-edit').off('click').on('click', function () {
            const instituteId = parseInt($(this).data('id'));
            if (window.InstituteModule.Details && typeof window.InstituteModule.Details.openEditForm === 'function') {
                window.InstituteModule.Details.openEditForm(instituteId);
            }
        });

        $('.btn-delete').off('click').on('click', function () {
            const instituteId = parseInt($(this).data('id'));
            deleteInstitute(instituteId);
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
     * Delete institute
     * @param {number} instituteId - Institute ID to delete
     */
    async function deleteInstitute(instituteId) {
        if (!instituteId || instituteId <= 0) {
            window.AppToast?.error('Invalid institute ID');
            return;
        }

        if (!confirm('Are you sure you want to delete this institute?\\n\\nThis action cannot be undone.')) {
            return;
        }

        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.delete(
                window.InstituteModule.config.apiEndpoints.delete(instituteId)
            );

            if (response.success) {
                window.AppToast?.success(response.message || 'Institute deleted successfully');
                refreshGrid();
            } else {
                window.AppToast?.error(response.message || 'Failed to delete institute');
            }
        } catch (error) {
            console.error('Error deleting institute:', error);
            window.AppToast?.error(error.message || 'Error deleting institute');
        } finally {
            window.AppLoader?.hide();
        }
    }

})();
