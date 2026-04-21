/**
 * Group Summary - Grid Module
 * This file handles grid operations (list, search, delete)
 */

(function () {
    'use strict';

    window.GroupModule = window.GroupModule || {};

    // Grid instance
    let grid = null;

    window.GroupModule.Summary = {
        init: initializeGrid,
        refreshGrid: refreshGrid,
        deleteGroup: deleteGroup
    };

    /**
     * Initialize Kendo Grid
     */
    function initializeGrid() {
        const dataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: window.GroupModule.config.apiEndpoints.summary,
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
                    id: 'groupId',
                    fields: {
                        groupId: { type: 'number' },
                        groupName: { type: 'string' },
                        isDefault: { type: 'number' }
                    }
                }
            },
            pageSize: window.GroupModule.config.gridOptions.pageSize,
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            error: function (e) {
                console.error('Grid data source error:', e);
                window.AppToast?.error('Error loading grid data');
            }
        });

        grid = $('#groupGrid').kendoGrid({
            dataSource: dataSource,
            height: 550,
            sortable: window.GroupModule.config.gridOptions.sortable,
            filterable: window.GroupModule.config.gridOptions.filterable,
            pageable: window.GroupModule.config.gridOptions.pageable,
            columns: [
                {
                    field: 'groupId',
                    title: 'ID',
                    width: 80,
                    filterable: false
                },
                {
                    field: 'groupName',
                    title: 'Group Name',
                    width: 300
                },
                {
                    field: 'isDefault',
                    title: 'Default Group',
                    width: 150,
                    template: function (dataItem) {
                        if (dataItem.isDefault === 1) {
                            return '<span class="badge badge-default">Yes</span>';
                        } else {
                            return '<span class="badge badge-inactive">No</span>';
                        }
                    },
                    filterable: {
                        ui: function (element) {
                            element.kendoDropDownList({
                                dataSource: [
                                    { text: 'All', value: '' },
                                    { text: 'Default', value: '1' },
                                    { text: 'Not Default', value: '0' }
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
                            <button class="btn-grid-action btn-edit" data-id="${dataItem.groupId}" title="Edit">
                                <span>✏️</span> Edit
                            </button>
                            <button class="btn-grid-action btn-delete" data-id="${dataItem.groupId}" title="Delete">
                                <span>🗑️</span> Delete
                            </button>
                        `;
                    }
                }
            ],
            dataBound: onDataBound
        }).data('kendoGrid');

        console.log('Group grid initialized');
    }

    /**
     * Handle grid data bound event
     */
    function onDataBound() {
        // Attach event handlers to action buttons
        $('.btn-edit').off('click').on('click', function () {
            const groupId = parseInt($(this).data('id'));
            if (window.GroupModule.Details && typeof window.GroupModule.Details.openEditForm === 'function') {
                window.GroupModule.Details.openEditForm(groupId);
            }
        });

        $('.btn-delete').off('click').on('click', function () {
            const groupId = parseInt($(this).data('id'));
            deleteGroup(groupId);
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
     * Delete group
     * @param {number} groupId - Group ID to delete
     */
    async function deleteGroup(groupId) {
        if (!groupId || groupId <= 0) {
            window.AppToast?.error('Invalid group ID');
            return;
        }

        // Confirm deletion
        if (!confirm('Are you sure you want to delete this group?\n\nThis action cannot be undone.')) {
            return;
        }

        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.delete(
                window.GroupModule.config.apiEndpoints.delete(groupId)
            );

            if (response.success) {
                window.AppToast?.success(response.message || 'Group deleted successfully');
                refreshGrid();
            } else {
                window.AppToast?.error(response.message || 'Failed to delete group');
            }
        } catch (error) {
            console.error('Error deleting group:', error);
            window.AppToast?.error(error.message || 'Error deleting group');
        } finally {
            window.AppLoader?.hide();
        }
    }

})();
