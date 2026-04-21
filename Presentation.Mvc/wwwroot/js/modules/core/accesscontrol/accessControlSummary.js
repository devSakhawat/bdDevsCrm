/**
 * Access Control Summary - Grid Management
 */

(function () {
    'use strict';

    window.AccessControlModule = window.AccessControlModule || {};

    window.AccessControlModule.Summary = {
        grid: null,

        init: function () {
            this.initGrid();
        },

        initGrid: function () {
            const config = window.AccessControlModule.config;

            this.grid = $('#accessControlGrid').kendoGrid({
                dataSource: {
                    transport: {
                        read: async (options) => {
                            try {
                                const response = await window.ApiClient.post(
                                    config.apiEndpoints.summary,
                                    {
                                        pageNumber: 1,
                                        pageSize: 1000,
                                        searchTerm: '',
                                        sortColumn: 'accessId',
                                        sortDirection: 'asc'
                                    }
                                );
                                options.success(response.data?.items || []);
                            } catch (error) {
                                console.error('Grid load error:', error);
                                window.AppToast?.error('Failed to load access controls');
                                options.error(error);
                            }
                        }
                    },
                    schema: {
                        model: {
                            id: 'accessId',
                            fields: {
                                accessId: { type: 'number' },
                                accessName: { type: 'string' },
                                accessDescription: { type: 'string' },
                                isActive: { type: 'boolean' }
                            }
                        }
                    },
                    pageSize: config.gridOptions.pageSize
                },
                columns: [
                    {
                        field: 'accessId',
                        title: 'ID',
                        width: 100
                    },
                    {
                        field: 'accessName',
                        title: 'Access Name',
                        width: 250
                    },
                    {
                        field: 'accessDescription',
                        title: 'Description',
                        width: 400
                    },
                    {
                        field: 'isActive',
                        title: 'Status',
                        width: 120,
                        template: (dataItem) => dataItem.isActive
                            ? '<span class="badge badge-success">Active</span>'
                            : '<span class="badge badge-inactive">Inactive</span>'
                    },
                    {
                        title: 'Actions',
                        width: 150,
                        template: (dataItem) => `
                            <button class="btn-grid-action btn-edit" data-id="${dataItem.accessId}" onclick="window.AccessControlModule.Details?.edit(${dataItem.accessId})">Edit</button>
                            <button class="btn-grid-action btn-delete" data-id="${dataItem.accessId}" onclick="window.AccessControlModule.Details?.delete(${dataItem.accessId})">Delete</button>
                        `
                    }
                ],
                sortable: config.gridOptions.sortable,
                filterable: config.gridOptions.filterable,
                pageable: config.gridOptions.pageable,
                height: 550
            }).data('kendoGrid');
        },

        refreshGrid: function () {
            this.grid?.dataSource.read();
        }
    };

})();
