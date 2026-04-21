/**
 * Thana Summary - Grid Module
 * This file handles grid operations (list, search, delete) with District relationship
 */

(function () {
    'use strict';

    window.ThanaModule = window.ThanaModule || {};

    // Grid instance
    let grid = null;

    window.ThanaModule.Summary = {
        init: initializeGrid,
        refreshGrid: refreshGrid,
        deleteThana: deleteThana
    };

    /**
     * Initialize Kendo Grid
     */
    function initializeGrid() {
        const dataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: window.ThanaModule.config.apiEndpoints.summary,
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
                    id: 'thanaId',
                    fields: {
                        thanaId: { type: 'number' },
                        thanaName: { type: 'string' },
                        thanaNameBn: { type: 'string' },
                        thanaCode: { type: 'string' },
                        districtId: { type: 'number' },
                        districtName: { type: 'string' },
                        status: { type: 'number' }
                    }
                }
            },
            pageSize: window.ThanaModule.config.gridOptions.pageSize,
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            error: function (e) {
                console.error('Grid data source error:', e);
                window.AppToast?.error('Error loading grid data');
            }
        });

        grid = $('#thanaGrid').kendoGrid({
            dataSource: dataSource,
            height: 550,
            sortable: window.ThanaModule.config.gridOptions.sortable,
            filterable: window.ThanaModule.config.gridOptions.filterable,
            pageable: window.ThanaModule.config.gridOptions.pageable,
            columns: [
                {
                    field: 'thanaId',
                    title: 'ID',
                    width: 80,
                    filterable: false
                },
                {
                    field: 'thanaName',
                    title: 'Thana Name',
                    width: 200
                },
                {
                    field: 'thanaNameBn',
                    title: 'Thana Name (Bangla)',
                    width: 200,
                    template: function (dataItem) {
                        return dataItem.thanaNameBn || '<span style="color: #999;">-</span>';
                    }
                },
                {
                    field: 'thanaCode',
                    title: 'Code',
                    width: 120,
                    template: function (dataItem) {
                        return dataItem.thanaCode || '<span style="color: #999;">-</span>';
                    }
                },
                {
                    field: 'districtName',
                    title: 'District',
                    width: 180,
                    template: function (dataItem) {
                        return dataItem.districtName || '<span style="color: #999;">N/A</span>';
                    }
                },
                {
                    field: 'status',
                    title: 'Status',
                    width: 100,
                    template: function (dataItem) {
                        if (dataItem.status === 1) {
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
                            <button class="btn-grid-action btn-edit" data-id="${dataItem.thanaId}" title="Edit">
                                <span>✏️</span> Edit
                            </button>
                            <button class="btn-grid-action btn-delete" data-id="${dataItem.thanaId}" title="Delete">
                                <span>🗑️</span> Delete
                            </button>
                        `;
                    }
                }
            ],
            dataBound: onDataBound
        }).data('kendoGrid');

        console.log('Thana grid initialized');
    }

    /**
     * Handle grid data bound event
     */
    function onDataBound() {
        // Attach event handlers to action buttons
        $('.btn-edit').off('click').on('click', function () {
            const thanaId = parseInt($(this).data('id'));
            if (window.ThanaModule.Details && typeof window.ThanaModule.Details.openEditForm === 'function') {
                window.ThanaModule.Details.openEditForm(thanaId);
            }
        });

        $('.btn-delete').off('click').on('click', function () {
            const thanaId = parseInt($(this).data('id'));
            deleteThana(thanaId);
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
     * Delete thana
     * @param {number} thanaId - Thana ID to delete
     */
    async function deleteThana(thanaId) {
        if (!thanaId || thanaId <= 0) {
            window.AppToast?.error('Invalid thana ID');
            return;
        }

        // Confirm deletion
        if (!confirm('Are you sure you want to delete this thana?\n\nThis action cannot be undone.')) {
            return;
        }

        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.delete(
                window.ThanaModule.config.apiEndpoints.delete(thanaId)
            );

            if (response.success) {
                window.AppToast?.success(response.message || 'Thana deleted successfully');
                refreshGrid();
            } else {
                window.AppToast?.error(response.message || 'Failed to delete thana');
            }
        } catch (error) {
            console.error('Error deleting thana:', error);
            window.AppToast?.error(error.message || 'Error deleting thana');
        } finally {
            window.AppLoader?.hide();
        }
    }

})();
