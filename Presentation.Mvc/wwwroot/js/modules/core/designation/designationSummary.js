/**
 * Designation Summary - Grid Module
 * This file handles grid operations (list, search, delete) for Designations
 */

(function () {
    'use strict';

    window.DesignationModule = window.DesignationModule || {};

    // Grid instance
    let grid = null;

    window.DesignationModule.Summary = {
        init: initializeGrid,
        refreshGrid: refreshGrid,
        deleteDesignation: deleteDesignation
    };

    /**
     * Initialize Kendo Grid
     */
    function initializeGrid() {
        const dataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: window.DesignationModule.config.apiEndpoints.summary,
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
                    id: 'designationId',
                    fields: {
                        designationId: { type: 'number' },
                        designationName: { type: 'string' },
                        designationCode: { type: 'string' },
                        sortOrder: { type: 'number' },
                        isActive: { type: 'number' }
                    }
                }
            },
            pageSize: window.DesignationModule.config.gridOptions.pageSize,
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            error: function (e) {
                console.error('Grid data source error:', e);
                window.AppToast?.error('Error loading grid data');
            }
        });

        grid = $('#designationGrid').kendoGrid({
            dataSource: dataSource,
            height: 550,
            sortable: window.DesignationModule.config.gridOptions.sortable,
            filterable: window.DesignationModule.config.gridOptions.filterable,
            pageable: window.DesignationModule.config.gridOptions.pageable,
            columns: [
                {
                    field: 'designationId',
                    title: 'ID',
                    width: 80,
                    filterable: false
                },
                {
                    field: 'designationName',
                    title: 'Designation Name',
                    width: 300
                },
                {
                    field: 'designationCode',
                    title: 'Code',
                    width: 150,
                    template: function (dataItem) {
                        return dataItem.designationCode || '<span style="color: #999;">-</span>';
                    }
                },
                {
                    field: 'sortOrder',
                    title: 'Sort Order',
                    width: 120,
                    filterable: false,
                    template: function (dataItem) {
                        return dataItem.sortOrder || 0;
                    }
                },
                {
                    field: 'isActive',
                    title: 'Status',
                    width: 120,
                    template: function (dataItem) {
                        if (dataItem.isActive === 1) {
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
                            <button class="btn-grid-action btn-edit" data-id="${dataItem.designationId}" title="Edit">
                                <span>✏️</span> Edit
                            </button>
                            <button class="btn-grid-action btn-delete" data-id="${dataItem.designationId}" title="Delete">
                                <span>🗑️</span> Delete
                            </button>
                        `;
                    }
                }
            ],
            dataBound: onDataBound
        }).data('kendoGrid');

        console.log('Designation grid initialized');
    }

    /**
     * Handle grid data bound event
     */
    function onDataBound() {
        $('.btn-edit').off('click').on('click', function () {
            const designationId = parseInt($(this).data('id'));
            if (window.DesignationModule.Details && typeof window.DesignationModule.Details.openEditForm === 'function') {
                window.DesignationModule.Details.openEditForm(designationId);
            }
        });

        $('.btn-delete').off('click').on('click', function () {
            const designationId = parseInt($(this).data('id'));
            deleteDesignation(designationId);
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
     * Delete designation
     * @param {number} designationId - Designation ID to delete
     */
    async function deleteDesignation(designationId) {
        if (!designationId || designationId <= 0) {
            window.AppToast?.error('Invalid designation ID');
            return;
        }

        if (!confirm('Are you sure you want to delete this designation?\\n\\nThis action cannot be undone.')) {
            return;
        }

        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.delete(
                window.DesignationModule.config.apiEndpoints.delete(designationId)
            );

            if (response.success) {
                window.AppToast?.success(response.message || 'Designation deleted successfully');
                refreshGrid();
            } else {
                window.AppToast?.error(response.message || 'Failed to delete designation');
            }
        } catch (error) {
            console.error('Error deleting designation:', error);
            window.AppToast?.error(error.message || 'Error deleting designation');
        } finally {
            window.AppLoader?.hide();
        }
    }

})();
