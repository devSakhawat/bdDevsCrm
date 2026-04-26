/**
 * Menu Summary - Grid Module
 * This file handles grid operations (list, search, delete) with hierarchical display
 */

(function () {
    'use strict';

    window.MenuModule = window.MenuModule || {};

    // Grid instance
    let grid = null;

    window.MenuModule.Summary = {
        init: initializeGrid,
        refreshGrid: refreshGrid,
        deleteMenu: deleteMenu
    };

    /**
     * Initialize Kendo Grid
     */
    function initializeGrid() {
        const dataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: window.MenuModule.config.apiEndpoints.summary,
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
                        // Map Kendo Grid options to API GridOptions format
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
                    id: 'menuId',
                    fields: {
                        menuId: { type: 'number' },
                        menuName: { type: 'string' },
                        moduleName: { type: 'string' },
                        parentMenuName: { type: 'string' },
                        menuPath: { type: 'string' },
                        menuCode: { type: 'string' },
                        sortOrder: { type: 'number' },
                        menuType: { type: 'number' },
                        isQuickLink: { type: 'number' },
                        isActive: { type: 'number' }
                    }
                }
            },
            pageSize: window.MenuModule.config.gridOptions.pageSize,
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            error: function (e) {
                console.error('Grid data source error:', e);
                window.AppToast?.error('Error loading grid data');
            }
        });

        grid = $('#menuGrid').kendoGrid({
            dataSource: dataSource,
            height: 550,
            sortable: window.MenuModule.config.gridOptions.sortable,
            filterable: window.MenuModule.config.gridOptions.filterable,
            pageable: window.MenuModule.config.gridOptions.pageable,
            columns: [
                {
                    field: 'menuId',
                    title: 'ID',
                    width: 80,
                    filterable: false
                },
                {
                    field: 'menuName',
                    title: 'Menu Name',
                    width: 200
                },
                {
                    field: 'moduleName',
                    title: 'Module',
                    width: 150
                },
                {
                    field: 'parentMenuName',
                    title: 'Parent Menu',
                    width: 150,
                    template: function (dataItem) {
                        return dataItem.parentMenuName || '<span style="color: #999;">Root</span>';
                    }
                },
                {
                    field: 'menuPath',
                    title: 'Path/URL',
                    width: 200,
                    template: function (dataItem) {
                        return dataItem.menuPath || '<span style="color: #999;">-</span>';
                    }
                },
                {
                    field: 'menuCode',
                    title: 'Code',
                    width: 120
                },
                {
                    field: 'sortOrder',
                    title: 'Order',
                    width: 80,
                    filterable: false
                },
                {
                    field: 'menuType',
                    title: 'Type',
                    width: 100,
                    template: function (dataItem) {
                        const types = ['Standard', 'Module', 'Group'];
                        return types[dataItem.menuType] || 'Standard';
                    },
                    filterable: false
                },
                {
                    field: 'isQuickLink',
                    title: 'Quick Link',
                    width: 110,
                    template: function (dataItem) {
                        if (dataItem.isQuickLink === 1) {
                            return '<span class="badge badge-success">Yes</span>';
                        } else {
                            return '<span class="badge badge-inactive">No</span>';
                        }
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
                            <button class="btn-grid-action btn-edit" data-id="${dataItem.menuId}" title="Edit">
                                <span>✏️</span> Edit
                            </button>
                            <button class="btn-grid-action btn-delete" data-id="${dataItem.menuId}" title="Delete">
                                <span>🗑️</span> Delete
                            </button>
                        `;
                    }
                }
            ],
            dataBound: onDataBound
        }).data('kendoGrid');

        console.log('Menu grid initialized');
    }

    /**
     * Handle grid data bound event
     */
    function onDataBound() {
        // Attach event handlers to action buttons
        $('.btn-edit').off('click').on('click', function () {
            const menuId = parseInt($(this).data('id'));
            if (window.MenuModule.Details && typeof window.MenuModule.Details.openEditForm === 'function') {
                window.MenuModule.Details.openEditForm(menuId);
            }
        });

        $('.btn-delete').off('click').on('click', function () {
            const menuId = parseInt($(this).data('id'));
            deleteMenu(menuId);
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
     * Delete menu
     * @param {number} menuId - Menu ID to delete
     */
    async function deleteMenu(menuId) {
        if (!menuId || menuId <= 0) {
            window.AppToast?.error('Invalid menu ID');
            return;
        }

        // Confirm deletion
        if (!confirm('Are you sure you want to delete this menu?\n\nThis action cannot be undone.')) {
            return;
        }

        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.delete(
                window.MenuModule.config.apiEndpoints.delete(menuId)
            );

            if (response.success) {
                window.AppToast?.success(response.message || 'Menu deleted successfully');
                refreshGrid();
            } else {
                window.AppToast?.error(response.message || 'Failed to delete menu');
            }
        } catch (error) {
            console.error('Error deleting menu:', error);
            window.AppToast?.error(error.message || 'Error deleting menu');
        } finally {
            window.AppLoader?.hide();
        }
    }

})();
