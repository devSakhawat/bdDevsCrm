/**
 * Department Summary - Grid Module
 * This file handles grid operations (list, search, delete) for Departments
 */

(function () {
    'use strict';

    window.DepartmentModule = window.DepartmentModule || {};

    // Grid instance
    let grid = null;

    window.DepartmentModule.Summary = {
        init: initializeGrid,
        refreshGrid: refreshGrid,
        deleteDepartment: deleteDepartment
    };

    /**
     * Initialize Kendo Grid
     */
    function initializeGrid() {
        const dataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: window.DepartmentModule.config.apiEndpoints.summary,
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
                    id: 'departmentId',
                    fields: {
                        departmentId: { type: 'number' },
                        departmentName: { type: 'string' },
                        departmentCode: { type: 'string' },
                        isCostCentre: { type: 'number' },
                        isActive: { type: 'number' }
                    }
                }
            },
            pageSize: window.DepartmentModule.config.gridOptions.pageSize,
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            error: function (e) {
                console.error('Grid data source error:', e);
                window.AppToast?.error('Error loading grid data');
            }
        });

        grid = $('#departmentGrid').kendoGrid({
            dataSource: dataSource,
            height: 550,
            sortable: window.DepartmentModule.config.gridOptions.sortable,
            filterable: window.DepartmentModule.config.gridOptions.filterable,
            pageable: window.DepartmentModule.config.gridOptions.pageable,
            columns: [
                {
                    field: 'departmentId',
                    title: 'ID',
                    width: 80,
                    filterable: false
                },
                {
                    field: 'departmentName',
                    title: 'Department Name',
                    width: 250
                },
                {
                    field: 'departmentCode',
                    title: 'Code',
                    width: 150,
                    template: function (dataItem) {
                        return dataItem.departmentCode || '<span style="color: #999;">-</span>';
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
                            <button class="btn-grid-action btn-edit" data-id="${dataItem.departmentId}" title="Edit">
                                <span>✏️</span> Edit
                            </button>
                            <button class="btn-grid-action btn-delete" data-id="${dataItem.departmentId}" title="Delete">
                                <span>🗑️</span> Delete
                            </button>
                        `;
                    }
                }
            ],
            dataBound: onDataBound
        }).data('kendoGrid');

        console.log('Department grid initialized');
    }

    /**
     * Handle grid data bound event
     */
    function onDataBound() {
        $('.btn-edit').off('click').on('click', function () {
            const departmentId = parseInt($(this).data('id'));
            if (window.DepartmentModule.Details && typeof window.DepartmentModule.Details.openEditForm === 'function') {
                window.DepartmentModule.Details.openEditForm(departmentId);
            }
        });

        $('.btn-delete').off('click').on('click', function () {
            const departmentId = parseInt($(this).data('id'));
            deleteDepartment(departmentId);
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
     * Delete department
     * @param {number} departmentId - Department ID to delete
     */
    async function deleteDepartment(departmentId) {
        if (!departmentId || departmentId <= 0) {
            window.AppToast?.error('Invalid department ID');
            return;
        }

        if (!confirm('Are you sure you want to delete this department?\\n\\nThis action cannot be undone.')) {
            return;
        }

        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.delete(
                window.DepartmentModule.config.apiEndpoints.delete(departmentId)
            );

            if (response.success) {
                window.AppToast?.success(response.message || 'Department deleted successfully');
                refreshGrid();
            } else {
                window.AppToast?.error(response.message || 'Failed to delete department');
            }
        } catch (error) {
            console.error('Error deleting department:', error);
            window.AppToast?.error(error.message || 'Error deleting department');
        } finally {
            window.AppLoader?.hide();
        }
    }

})();
