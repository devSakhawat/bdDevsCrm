/**
 * Employee Summary - Grid Module
 * This file handles grid operations (list, search, delete) for Employees
 */

(function () {
    'use strict';

    window.EmployeeModule = window.EmployeeModule || {};

    // Grid instance
    let grid = null;

    window.EmployeeModule.Summary = {
        init: initializeGrid,
        refreshGrid: refreshGrid,
        deleteEmployee: deleteEmployee
    };

    /**
     * Initialize Kendo Grid
     */
    function initializeGrid() {
        const dataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: window.EmployeeModule.config.apiEndpoints.summary,
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
                    id: 'employeeId',
                    fields: {
                        employeeId: { type: 'number' },
                        employeeCode: { type: 'string' },
                        fullName: { type: 'string' },
                        email: { type: 'string' },
                        mobileNumber: { type: 'string' },
                        departmentName: { type: 'string' },
                        designationName: { type: 'string' },
                        joinDate: { type: 'date' },
                        isActive: { type: 'number' }
                    }
                }
            },
            pageSize: window.EmployeeModule.config.gridOptions.pageSize,
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            error: function (e) {
                console.error('Grid data source error:', e);
                window.AppToast?.error('Error loading grid data');
            }
        });

        grid = $('#employeeGrid').kendoGrid({
            dataSource: dataSource,
            height: 550,
            sortable: window.EmployeeModule.config.gridOptions.sortable,
            filterable: window.EmployeeModule.config.gridOptions.filterable,
            pageable: window.EmployeeModule.config.gridOptions.pageable,
            columns: [
                {
                    field: 'employeeId',
                    title: 'ID',
                    width: 70,
                    filterable: false
                },
                {
                    field: 'employeeCode',
                    title: 'Employee Code',
                    width: 130,
                    template: function (dataItem) {
                        return dataItem.employeeCode || '<span style="color: #999;">-</span>';
                    }
                },
                {
                    field: 'fullName',
                    title: 'Full Name',
                    width: 200,
                    template: function (dataItem) {
                        return dataItem.fullName || '<span style="color: #999;">-</span>';
                    }
                },
                {
                    field: 'email',
                    title: 'Email',
                    width: 220,
                    template: function (dataItem) {
                        if (dataItem.email) {
                            return `<a href="mailto:${dataItem.email}" style="color: #007bff;">${dataItem.email}</a>`;
                        }
                        return '<span style="color: #999;">-</span>';
                    }
                },
                {
                    field: 'mobileNumber',
                    title: 'Mobile',
                    width: 130,
                    template: function (dataItem) {
                        return dataItem.mobileNumber || '<span style="color: #999;">-</span>';
                    }
                },
                {
                    field: 'departmentName',
                    title: 'Department',
                    width: 150,
                    template: function (dataItem) {
                        return dataItem.departmentName || '<span style="color: #999;">-</span>';
                    }
                },
                {
                    field: 'designationName',
                    title: 'Designation',
                    width: 150,
                    template: function (dataItem) {
                        return dataItem.designationName || '<span style="color: #999;">-</span>';
                    }
                },
                {
                    field: 'joinDate',
                    title: 'Join Date',
                    width: 120,
                    template: function (dataItem) {
                        if (dataItem.joinDate) {
                            const date = new Date(dataItem.joinDate);
                            return date.toLocaleDateString('en-GB');
                        }
                        return '<span style="color: #999;">-</span>';
                    },
                    filterable: false
                },
                {
                    field: 'isActive',
                    title: 'Status',
                    width: 100,
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
                            <button class="btn-grid-action btn-edit" data-id="${dataItem.employeeId}" title="Edit">
                                <span>✏️</span> Edit
                            </button>
                            <button class="btn-grid-action btn-delete" data-id="${dataItem.employeeId}" title="Delete">
                                <span>🗑️</span> Delete
                            </button>
                        `;
                    }
                }
            ],
            dataBound: onDataBound
        }).data('kendoGrid');

        console.log('Employee grid initialized');
    }

    /**
     * Handle grid data bound event
     */
    function onDataBound() {
        $('.btn-edit').off('click').on('click', function () {
            const employeeId = parseInt($(this).data('id'));
            if (window.EmployeeModule.Details && typeof window.EmployeeModule.Details.openEditForm === 'function') {
                window.EmployeeModule.Details.openEditForm(employeeId);
            }
        });

        $('.btn-delete').off('click').on('click', function () {
            const employeeId = parseInt($(this).data('id'));
            deleteEmployee(employeeId);
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
     * Delete employee
     * @param {number} employeeId - Employee ID to delete
     */
    async function deleteEmployee(employeeId) {
        if (!employeeId || employeeId <= 0) {
            window.AppToast?.error('Invalid employee ID');
            return;
        }

        if (!confirm('Are you sure you want to delete this employee?\n\nThis action cannot be undone.')) {
            return;
        }

        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.delete(
                window.EmployeeModule.config.apiEndpoints.delete(employeeId)
            );

            if (response.success) {
                window.AppToast?.success(response.message || 'Employee deleted successfully');
                refreshGrid();
            } else {
                window.AppToast?.error(response.message || 'Failed to delete employee');
            }
        } catch (error) {
            console.error('Error deleting employee:', error);
            window.AppToast?.error(error.message || 'Error deleting employee');
        } finally {
            window.AppLoader?.hide();
        }
    }

})();
