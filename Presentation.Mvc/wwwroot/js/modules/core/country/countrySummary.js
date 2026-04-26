/**
 * Country Summary - Grid Module
 * This file handles grid operations (list, search, delete)
 */

(function () {
    'use strict';

    window.CountryModule = window.CountryModule || {};

    // Grid instance
    let grid = null;

    window.CountryModule.Summary = {
        init: initializeGrid,
        refreshGrid: refreshGrid,
        deleteCountry: deleteCountry
    };

    /**
     * Initialize Kendo Grid
     */
    function initializeGrid() {
        const dataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: window.CountryModule.config.apiEndpoints.summary,
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
                    id: 'countryId',
                    fields: {
                        countryId: { type: 'number' },
                        countryName: { type: 'string' },
                        countryCode: { type: 'string' },
                        sortOrder: { type: 'number' },
                        isActive: { type: 'boolean' },
                        remarks: { type: 'string' }
                    }
                }
            },
            pageSize: window.CountryModule.config.gridOptions.pageSize,
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            error: function (e) {
                console.error('Grid data source error:', e);
                window.AppToast?.error('Error loading grid data');
            }
        });

        grid = $('#countryGrid').kendoGrid({
            dataSource: dataSource,
            height: 550,
            sortable: window.CountryModule.config.gridOptions.sortable,
            filterable: window.CountryModule.config.gridOptions.filterable,
            pageable: window.CountryModule.config.gridOptions.pageable,
            columns: [
                {
                    field: 'countryId',
                    title: 'ID',
                    width: 80,
                    filterable: false
                },
                {
                    field: 'countryName',
                    title: 'Country Name',
                    width: 200
                },
                {
                    field: 'countryCode',
                    title: 'Code',
                    width: 100
                },
                {
                    field: 'sortOrder',
                    title: 'Sort Order',
                    width: 120,
                    filterable: false
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
                    field: 'remarks',
                    title: 'Remarks',
                    width: 200
                },
                {
                    title: 'Actions',
                    width: 180,
                    filterable: false,
                    sortable: false,
                    template: function (dataItem) {
                        return `
                            <button class="btn-grid-action btn-edit" data-id="${dataItem.countryId}" title="Edit">
                                <span>✏️</span> Edit
                            </button>
                            <button class="btn-grid-action btn-delete" data-id="${dataItem.countryId}" title="Delete">
                                <span>🗑️</span> Delete
                            </button>
                        `;
                    }
                }
            ],
            dataBound: onDataBound
        }).data('kendoGrid');

        console.log('Country grid initialized');
    }

    /**
     * Handle grid data bound event
     */
    function onDataBound() {
        // Attach event handlers to action buttons
        $('.btn-edit').off('click').on('click', function () {
            const countryId = parseInt($(this).data('id'));
            if (window.CountryModule.Details && typeof window.CountryModule.Details.openEditForm === 'function') {
                window.CountryModule.Details.openEditForm(countryId);
            }
        });

        $('.btn-delete').off('click').on('click', function () {
            const countryId = parseInt($(this).data('id'));
            deleteCountry(countryId);
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
     * Delete country
     * @param {number} countryId - Country ID to delete
     */
    async function deleteCountry(countryId) {
        if (!countryId || countryId <= 0) {
            window.AppToast?.error('Invalid country ID');
            return;
        }

        // Confirm deletion
        if (!confirm('Are you sure you want to delete this country?\n\nThis action cannot be undone.')) {
            return;
        }

        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.delete(
                window.CountryModule.config.apiEndpoints.delete(countryId)
            );

            if (response.success) {
                window.AppToast?.success(response.message || 'Country deleted successfully');
                refreshGrid();
            } else {
                window.AppToast?.error(response.message || 'Failed to delete country');
            }
        } catch (error) {
            console.error('Error deleting country:', error);
            window.AppToast?.error(error.message || 'Error deleting country');
        } finally {
            window.AppLoader?.hide();
        }
    }

})();
