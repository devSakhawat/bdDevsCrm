/**
 * Branch Summary - Grid Module
 * This file handles grid operations (list, search, delete) for Branches
 */

(function () {
    'use strict';

    window.BranchModule = window.BranchModule || {};

    // Grid instance
    let grid = null;

    window.BranchModule.Summary = {
        init: initializeGrid,
        refreshGrid: refreshGrid,
        deleteBranch: deleteBranch
    };

    /**
     * Initialize Kendo Grid
     */
    function initializeGrid() {
        const dataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: window.BranchModule.config.apiEndpoints.summary,
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
                    id: 'branchId',
                    fields: {
                        branchId: { type: 'number' },
                        branchName: { type: 'string' },
                        branchCode: { type: 'string' },
                        companyName: { type: 'string' },
                        branchAddress: { type: 'string' },
                        isCostCentre: { type: 'number' },
                        isActive: { type: 'number' }
                    }
                }
            },
            pageSize: window.BranchModule.config.gridOptions.pageSize,
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            error: function (e) {
                console.error('Grid data source error:', e);
                window.AppToast?.error('Error loading grid data');
            }
        });

        grid = $('#branchGrid').kendoGrid({
            dataSource: dataSource,
            height: 550,
            sortable: window.BranchModule.config.gridOptions.sortable,
            filterable: window.BranchModule.config.gridOptions.filterable,
            pageable: window.BranchModule.config.gridOptions.pageable,
            columns: [
                {
                    field: 'branchId',
                    title: 'ID',
                    width: 80,
                    filterable: false
                },
                {
                    field: 'branchName',
                    title: 'Branch Name',
                    width: 220
                },
                {
                    field: 'branchCode',
                    title: 'Code',
                    width: 120,
                    template: function (dataItem) {
                        return dataItem.branchCode || '<span style="color: #999;">-</span>';
                    }
                },
                {
                    field: 'companyName',
                    title: 'Company',
                    width: 200
                },
                {
                    field: 'branchAddress',
                    title: 'Address',
                    width: 250,
                    template: function (dataItem) {
                        return dataItem.branchAddress || '<span style="color: #999;">-</span>';
                    }
                },
                {
                    field: 'isCostCentre',
                    title: 'Cost Centre',
                    width: 130,
                    template: function (dataItem) {
                        if (dataItem.isCostCentre === 1) {
                            return '<span class="badge badge-info">Yes</span>';
                        } else {
                            return '<span class="badge badge-secondary">No</span>';
                        }
                    },
                    filterable: {
                        ui: function (element) {
                            element.kendoDropDownList({
                                dataSource: [
                                    { text: 'All', value: '' },
                                    { text: 'Yes', value: '1' },
                                    { text: 'No', value: '0' }
                                ],
                                dataTextField: 'text',
                                dataValueField: 'value',
                                optionLabel: 'All'
                            });
                        }
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
                            <button class="btn-grid-action btn-edit" data-id="${dataItem.branchId}" title="Edit">
                                <span>✏️</span> Edit
                            </button>
                            <button class="btn-grid-action btn-delete" data-id="${dataItem.branchId}" title="Delete">
                                <span>🗑️</span> Delete
                            </button>
                        `;
                    }
                }
            ],
            dataBound: onDataBound
        }).data('kendoGrid');

        console.log('Branch grid initialized');
    }

    /**
     * Handle grid data bound event
     */
    function onDataBound() {
        $('.btn-edit').off('click').on('click', function () {
            const branchId = parseInt($(this).data('id'));
            if (window.BranchModule.Details && typeof window.BranchModule.Details.openEditForm === 'function') {
                window.BranchModule.Details.openEditForm(branchId);
            }
        });

        $('.btn-delete').off('click').on('click', function () {
            const branchId = parseInt($(this).data('id'));
            deleteBranch(branchId);
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
     * Delete branch
     * @param {number} branchId - Branch ID to delete
     */
    async function deleteBranch(branchId) {
        if (!branchId || branchId <= 0) {
            window.AppToast?.error('Invalid branch ID');
            return;
        }

        if (!confirm('Are you sure you want to delete this branch?\\n\\nThis action cannot be undone.')) {
            return;
        }

        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.delete(
                window.BranchModule.config.apiEndpoints.delete(branchId)
            );

            if (response.success) {
                window.AppToast?.success(response.message || 'Branch deleted successfully');
                refreshGrid();
            } else {
                window.AppToast?.error(response.message || 'Failed to delete branch');
            }
        } catch (error) {
            console.error('Error deleting branch:', error);
            window.AppToast?.error(error.message || 'Error deleting branch');
        } finally {
            window.AppLoader?.hide();
        }
    }

})();
