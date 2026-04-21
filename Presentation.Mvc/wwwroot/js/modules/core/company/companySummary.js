/**
 * Company Summary - Grid Module
 * This file handles grid operations (list, search, delete)
 */

(function () {
    'use strict';

    window.CompanyModule = window.CompanyModule || {};

    // Grid instance
    let grid = null;

    window.CompanyModule.Summary = {
        init: initializeGrid,
        refreshGrid: refreshGrid,
        deleteCompany: deleteCompany
    };

    /**
     * Initialize Kendo Grid
     */
    function initializeGrid() {
        const dataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: window.CompanyModule.config.apiEndpoints.summary,
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
                    id: 'companyId',
                    fields: {
                        companyId: { type: 'number' },
                        companyName: { type: 'string' },
                        shortName: { type: 'string' },
                        address: { type: 'string' },
                        phone: { type: 'string' },
                        email: { type: 'string' },
                        website: { type: 'string' },
                        isActive: { type: 'boolean' }
                    }
                }
            },
            pageSize: window.CompanyModule.config.gridOptions.pageSize,
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            error: function (e) {
                console.error('Grid data source error:', e);
                window.AppToast?.error('Error loading grid data');
            }
        });

        grid = $('#companyGrid').kendoGrid({
            dataSource: dataSource,
            height: 550,
            sortable: window.CompanyModule.config.gridOptions.sortable,
            filterable: window.CompanyModule.config.gridOptions.filterable,
            pageable: window.CompanyModule.config.gridOptions.pageable,
            columns: [
                {
                    field: 'companyId',
                    title: 'ID',
                    width: 80,
                    filterable: false
                },
                {
                    field: 'companyName',
                    title: 'Company Name',
                    width: 200
                },
                {
                    field: 'shortName',
                    title: 'Short Name',
                    width: 120
                },
                {
                    field: 'address',
                    title: 'Address',
                    width: 200
                },
                {
                    field: 'phone',
                    title: 'Phone',
                    width: 120
                },
                {
                    field: 'email',
                    title: 'Email',
                    width: 180
                },
                {
                    field: 'website',
                    title: 'Website',
                    width: 150,
                    template: function (dataItem) {
                        if (dataItem.website) {
                            return `<a href="${dataItem.website}" target="_blank" rel="noopener noreferrer">${dataItem.website}</a>`;
                        }
                        return '';
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
                            <button class="btn-grid-action btn-edit" data-id="${dataItem.companyId}" title="Edit">
                                <span>✏️</span> Edit
                            </button>
                            <button class="btn-grid-action btn-delete" data-id="${dataItem.companyId}" title="Delete">
                                <span>🗑️</span> Delete
                            </button>
                        `;
                    }
                }
            ],
            dataBound: onDataBound
        }).data('kendoGrid');

        console.log('Company grid initialized');
    }

    /**
     * Handle grid data bound event
     */
    function onDataBound() {
        // Attach event handlers to action buttons
        $('.btn-edit').off('click').on('click', function () {
            const companyId = parseInt($(this).data('id'));
            if (window.CompanyModule.Details && typeof window.CompanyModule.Details.openEditForm === 'function') {
                window.CompanyModule.Details.openEditForm(companyId);
            }
        });

        $('.btn-delete').off('click').on('click', function () {
            const companyId = parseInt($(this).data('id'));
            deleteCompany(companyId);
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
     * Delete company
     * @param {number} companyId - Company ID to delete
     */
    async function deleteCompany(companyId) {
        if (!companyId || companyId <= 0) {
            window.AppToast?.error('Invalid company ID');
            return;
        }

        // Confirm deletion
        if (!confirm('Are you sure you want to delete this company?\n\nThis action cannot be undone.')) {
            return;
        }

        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.delete(
                window.CompanyModule.config.apiEndpoints.delete(companyId)
            );

            if (response.success) {
                window.AppToast?.success(response.message || 'Company deleted successfully');
                refreshGrid();
            } else {
                window.AppToast?.error(response.message || 'Failed to delete company');
            }
        } catch (error) {
            console.error('Error deleting company:', error);
            window.AppToast?.error(error.message || 'Error deleting company');
        } finally {
            window.AppLoader?.hide();
        }
    }

})();
