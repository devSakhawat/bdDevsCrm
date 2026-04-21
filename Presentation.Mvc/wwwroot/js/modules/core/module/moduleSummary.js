/**
 * Module Summary - Grid Module
 * This file handles grid operations (list, search, delete)
 */

(function () {
    'use strict';

    window.ModuleModule = window.ModuleModule || {};

    // Grid instance
    let grid = null;

    window.ModuleModule.Summary = {
        init: initializeGrid,
        refreshGrid: refreshGrid,
        deleteModule: deleteModule
    };

    /**
     * Initialize Kendo Grid
     */
    function initializeGrid() {
        const dataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: window.ModuleModule.config.apiEndpoints.summary,
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
                    id: 'moduleId',
                    fields: {
                        moduleId: { type: 'number' },
                        moduleName: { type: 'string' },
                        description: { type: 'string' },
                        isActive: { type: 'boolean' }
                    }
                }
            },
            pageSize: window.ModuleModule.config.gridOptions.pageSize,
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            error: function (e) {
                console.error('Grid data source error:', e);
                window.AppToast?.error('Error loading grid data');
            }
        });

        grid = $('#moduleGrid').kendoGrid({
            dataSource: dataSource,
            height: 550,
            sortable: window.ModuleModule.config.gridOptions.sortable,
            filterable: window.ModuleModule.config.gridOptions.filterable,
            pageable: window.ModuleModule.config.gridOptions.pageable,
            columns: [
                {
                    field: 'moduleId',
                    title: 'ID',
                    width: 80,
                    filterable: false
                },
                {
                    field: 'moduleName',
                    title: 'Module Name',
                    width: 250
                },
                {
                    field: 'description',
                    title: 'Description',
                    width: 300
                },
                {
                    field: 'isActive',
                    title: 'Status',
                    width: 120,
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
                            <button class="btn-grid-action btn-edit" data-id="${dataItem.moduleId}" title="Edit">
                                <span>✏️</span> Edit
                            </button>
                            <button class="btn-grid-action btn-delete" data-id="${dataItem.moduleId}" title="Delete">
                                <span>🗑️</span> Delete
                            </button>
                        `;
                    }
                }
            ],
            dataBound: onDataBound
        }).data('kendoGrid');

        console.log('Module grid initialized');
    }

    /**
     * Handle grid data bound event
     */
    function onDataBound() {
        // Attach event handlers to action buttons
        $('.btn-edit').off('click').on('click', function () {
            const moduleId = parseInt($(this).data('id'));
            if (window.ModuleModule.Details && typeof window.ModuleModule.Details.openEditForm === 'function') {
                window.ModuleModule.Details.openEditForm(moduleId);
            }
        });

        $('.btn-delete').off('click').on('click', function () {
            const moduleId = parseInt($(this).data('id'));
            deleteModule(moduleId);
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
     * Delete module
     * @param {number} moduleId - Module ID to delete
     */
    async function deleteModule(moduleId) {
        if (!moduleId || moduleId <= 0) {
            window.AppToast?.error('Invalid module ID');
            return;
        }

        // Confirm deletion
        if (!confirm('Are you sure you want to delete this module?\n\nThis action cannot be undone.')) {
            return;
        }

        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.delete(
                window.ModuleModule.config.apiEndpoints.delete(moduleId)
            );

            if (response.success) {
                window.AppToast?.success(response.message || 'Module deleted successfully');
                refreshGrid();
            } else {
                window.AppToast?.error(response.message || 'Failed to delete module');
            }
        } catch (error) {
            console.error('Error deleting module:', error);
            window.AppToast?.error(error.message || 'Error deleting module');
        } finally {
            window.AppLoader?.hide();
        }
    }

})();
