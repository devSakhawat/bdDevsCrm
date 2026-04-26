/**
 * Users Summary - Grid Module
 * This file handles grid operations (list, search, delete) for Users
 */

(function () {
    'use strict';

    window.UsersModule = window.UsersModule || {};

    // Grid instance
    let grid = null;

    window.UsersModule.Summary = {
        init: initializeGrid,
        refreshGrid: refreshGrid,
        deleteUser: deleteUser
    };

    /**
     * Initialize Kendo Grid
     */
    function initializeGrid() {
        const dataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: window.UsersModule.config.apiEndpoints.summary + '?companyId=0',
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
                    id: 'userId',
                    fields: {
                        userId: { type: 'number' },
                        loginId: { type: 'string' },
                        userName: { type: 'string' },
                        emailAddress: { type: 'string' },
                        companyName: { type: 'string' },
                        employeeName: { type: 'string' },
                        isActive: { type: 'boolean' }
                    }
                }
            },
            pageSize: window.UsersModule.config.gridOptions.pageSize,
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            error: function (e) {
                console.error('Grid data source error:', e);
                window.AppToast?.error('Error loading grid data');
            }
        });

        grid = $('#usersGrid').kendoGrid({
            dataSource: dataSource,
            height: 550,
            sortable: window.UsersModule.config.gridOptions.sortable,
            filterable: window.UsersModule.config.gridOptions.filterable,
            pageable: window.UsersModule.config.gridOptions.pageable,
            columns: [
                {
                    field: 'userId',
                    title: 'ID',
                    width: 80,
                    filterable: false
                },
                {
                    field: 'loginId',
                    title: 'Login ID',
                    width: 150
                },
                {
                    field: 'userName',
                    title: 'Full Name',
                    width: 200
                },
                {
                    field: 'emailAddress',
                    title: 'Email',
                    width: 200,
                    template: function (dataItem) {
                        return dataItem.emailAddress || '<span style="color: #999;">-</span>';
                    }
                },
                {
                    field: 'companyName',
                    title: 'Company',
                    width: 180
                },
                {
                    field: 'employeeName',
                    title: 'Employee',
                    width: 180,
                    template: function (dataItem) {
                        return dataItem.employeeName || '<span style="color: #999;">Not Linked</span>';
                    }
                },
                {
                    field: 'isActive',
                    title: 'Status',
                    width: 100,
                    template: function (dataItem) {
                        if (dataItem.isActive) {
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
                            <button class="btn-grid-action btn-edit" data-id="${dataItem.userId}" title="Edit">
                                <span>✏️</span> Edit
                            </button>
                            <button class="btn-grid-action btn-delete" data-id="${dataItem.userId}" title="Delete">
                                <span>🗑️</span> Delete
                            </button>
                        `;
                    }
                }
            ],
            dataBound: onDataBound
        }).data('kendoGrid');

        console.log('Users grid initialized');
    }

    /**
     * Handle grid data bound event
     */
    function onDataBound() {
        $('.btn-edit').off('click').on('click', function () {
            const userId = parseInt($(this).data('id'));
            if (window.UsersModule.Details && typeof window.UsersModule.Details.openEditForm === 'function') {
                window.UsersModule.Details.openEditForm(userId);
            }
        });

        $('.btn-delete').off('click').on('click', function () {
            const userId = parseInt($(this).data('id'));
            deleteUser(userId);
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
     * Delete user
     * @param {number} userId - User ID to delete
     */
    async function deleteUser(userId) {
        if (!userId || userId <= 0) {
            window.AppToast?.error('Invalid user ID');
            return;
        }

        if (!confirm('Are you sure you want to delete this user?\n\nThis action cannot be undone.')) {
            return;
        }

        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.delete(
                window.UsersModule.config.apiEndpoints.delete(userId)
            );

            if (response.success) {
                window.AppToast?.success(response.message || 'User deleted successfully');
                refreshGrid();
            } else {
                window.AppToast?.error(response.message || 'Failed to delete user');
            }
        } catch (error) {
            console.error('Error deleting user:', error);
            window.AppToast?.error(error.message || 'Error deleting user');
        } finally {
            window.AppLoader?.hide();
        }
    }

})();
