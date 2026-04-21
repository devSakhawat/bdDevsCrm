/**
 * Workflow Summary - Kendo TabStrip: States tab + Actions tab
 */

(function () {
    'use strict';

    window.WorkflowModule = window.WorkflowModule || {};

    window.WorkflowModule.Summary = {
        stateGrid: null,
        actionGrid: null,
        tabStrip: null,

        init: function () {
            this.initTabStrip();
            this.initStateGrid();
            this.initActionGrid();
        },

        initTabStrip: function () {
            this.tabStrip = $('#workflowTabStrip').kendoTabStrip({
                animation: {
                    open: { effects: 'fadeIn' }
                }
            }).data('kendoTabStrip');
        },

        initStateGrid: function () {
            const config = window.WorkflowModule.config;

            this.stateGrid = $('#stateGrid').kendoGrid({
                dataSource: {
                    transport: {
                        read: async (options) => {
                            try {
                                const response = await window.ApiClient.post(
                                    config.apiEndpoints.stateSummary,
                                    {
                                        pageNumber: 1,
                                        pageSize: 1000,
                                        searchTerm: '',
                                        sortColumn: 'sequence',
                                        sortDirection: 'asc'
                                    }
                                );
                                options.success(response.data?.items || []);
                            } catch (error) {
                                console.error('State grid load error:', error);
                                window.AppToast?.error('Failed to load states');
                                options.error(error);
                            }
                        }
                    },
                    schema: {
                        model: {
                            id: 'wfStateId',
                            fields: {
                                wfStateId: { type: 'number' },
                                stateName: { type: 'string' },
                                menuName: { type: 'string' },
                                sequence: { type: 'number' },
                                isDefaultStart: { type: 'boolean' },
                                isClosed: { type: 'boolean' },
                                isActive: { type: 'boolean' }
                            }
                        }
                    },
                    pageSize: config.gridOptions.pageSize
                },
                columns: [
                    {
                        field: 'wfStateId',
                        title: 'ID',
                        width: 80
                    },
                    {
                        field: 'stateName',
                        title: 'State Name',
                        width: 200
                    },
                    {
                        field: 'menuName',
                        title: 'Menu',
                        width: 150
                    },
                    {
                        field: 'sequence',
                        title: 'Sequence',
                        width: 100,
                        attributes: { style: 'text-align: center;' }
                    },
                    {
                        field: 'isDefaultStart',
                        title: 'Default Start',
                        width: 120,
                        template: (dataItem) => dataItem.isDefaultStart
                            ? '<span class="badge badge-success">Yes</span>'
                            : '<span class="badge badge-inactive">No</span>'
                    },
                    {
                        field: 'isClosed',
                        title: 'Closed',
                        width: 100,
                        template: (dataItem) => dataItem.isClosed
                            ? '<span class="badge badge-info">Yes</span>'
                            : '<span class="badge badge-inactive">No</span>'
                    },
                    {
                        field: 'isActive',
                        title: 'Status',
                        width: 100,
                        template: (dataItem) => dataItem.isActive
                            ? '<span class="badge badge-success">Active</span>'
                            : '<span class="badge badge-inactive">Inactive</span>'
                    },
                    {
                        title: 'Actions',
                        width: 120,
                        template: (dataItem) => `
                            <button class="btn-grid-action btn-edit" data-id="${dataItem.wfStateId}" onclick="window.WorkflowModule.Details?.editState(${dataItem.wfStateId})">Edit</button>
                            <button class="btn-grid-action btn-delete" data-id="${dataItem.wfStateId}" onclick="window.WorkflowModule.Details?.deleteState(${dataItem.wfStateId})">Delete</button>
                        `
                    }
                ],
                sortable: config.gridOptions.sortable,
                filterable: config.gridOptions.filterable,
                pageable: config.gridOptions.pageable,
                height: 450
            }).data('kendoGrid');

            $('#btnAddState').on('click', () => window.WorkflowModule.Details?.openAddStateForm());
        },

        initActionGrid: function () {
            const config = window.WorkflowModule.config;

            this.actionGrid = $('#actionGrid').kendoGrid({
                dataSource: {
                    transport: {
                        read: async (options) => {
                            try {
                                const response = await window.ApiClient.post(
                                    config.apiEndpoints.actionSummary,
                                    {
                                        pageNumber: 1,
                                        pageSize: 1000,
                                        searchTerm: '',
                                        sortColumn: 'wfActionId',
                                        sortDirection: 'asc'
                                    }
                                );
                                options.success(response.data?.items || []);
                            } catch (error) {
                                console.error('Action grid load error:', error);
                                window.AppToast?.error('Failed to load actions');
                                options.error(error);
                            }
                        }
                    },
                    schema: {
                        model: {
                            id: 'wfActionId',
                            fields: {
                                wfActionId: { type: 'number' },
                                wfStateId: { type: 'number' },
                                stateName: { type: 'string' },
                                actionName: { type: 'string' },
                                nextStateId: { type: 'number' },
                                nextStateName: { type: 'string' },
                                emailAlert: { type: 'boolean' },
                                smsAlert: { type: 'boolean' },
                                isActive: { type: 'boolean' }
                            }
                        }
                    },
                    pageSize: config.gridOptions.pageSize
                },
                columns: [
                    {
                        field: 'wfActionId',
                        title: 'ID',
                        width: 80
                    },
                    {
                        field: 'stateName',
                        title: 'From State',
                        width: 150
                    },
                    {
                        field: 'actionName',
                        title: 'Action Name',
                        width: 200
                    },
                    {
                        field: 'nextStateName',
                        title: 'Next State',
                        width: 150
                    },
                    {
                        field: 'emailAlert',
                        title: 'Email Alert',
                        width: 100,
                        template: (dataItem) => dataItem.emailAlert
                            ? '<span class="badge badge-success">Yes</span>'
                            : '<span class="badge badge-inactive">No</span>'
                    },
                    {
                        field: 'smsAlert',
                        title: 'SMS Alert',
                        width: 100,
                        template: (dataItem) => dataItem.smsAlert
                            ? '<span class="badge badge-success">Yes</span>'
                            : '<span class="badge badge-inactive">No</span>'
                    },
                    {
                        field: 'isActive',
                        title: 'Status',
                        width: 100,
                        template: (dataItem) => dataItem.isActive
                            ? '<span class="badge badge-success">Active</span>'
                            : '<span class="badge badge-inactive">Inactive</span>'
                    },
                    {
                        title: 'Actions',
                        width: 120,
                        template: (dataItem) => `
                            <button class="btn-grid-action btn-edit" data-id="${dataItem.wfActionId}" onclick="window.WorkflowModule.Details?.editAction(${dataItem.wfActionId})">Edit</button>
                            <button class="btn-grid-action btn-delete" data-id="${dataItem.wfActionId}" onclick="window.WorkflowModule.Details?.deleteAction(${dataItem.wfActionId})">Delete</button>
                        `
                    }
                ],
                sortable: config.gridOptions.sortable,
                filterable: config.gridOptions.filterable,
                pageable: config.gridOptions.pageable,
                height: 450
            }).data('kendoGrid');

            $('#btnAddAction').on('click', () => window.WorkflowModule.Details?.openAddActionForm());
        },

        refreshStateGrid: function () {
            this.stateGrid?.dataSource.read();
        },

        refreshActionGrid: function () {
            this.actionGrid?.dataSource.read();
        }
    };

})();
