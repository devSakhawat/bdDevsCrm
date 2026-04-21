/**
 * Shift Summary - Grid Module
 * This file handles grid operations (list, search, delete) for Shifts
 */

(function () {
    'use strict';

    window.ShiftModule = window.ShiftModule || {};

    // Grid instance
    let grid = null;

    window.ShiftModule.Summary = {
        init: initializeGrid,
        refreshGrid: refreshGrid,
        deleteShift: deleteShift
    };

    /**
     * Initialize Kendo Grid
     */
    function initializeGrid() {
        const dataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: window.ShiftModule.config.apiEndpoints.summary,
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
                    id: 'shiftId',
                    fields: {
                        shiftId: { type: 'number' },
                        shiftName: { type: 'string' },
                        shiftCode: { type: 'string' },
                        startTime: { type: 'string' },
                        endTime: { type: 'string' },
                        workHours: { type: 'number' },
                        isNightShift: { type: 'number' },
                        isFlexible: { type: 'number' },
                        isActive: { type: 'number' }
                    }
                }
            },
            pageSize: window.ShiftModule.config.gridOptions.pageSize,
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            error: function (e) {
                console.error('Grid data source error:', e);
                window.AppToast?.error('Error loading grid data');
            }
        });

        grid = $('#shiftGrid').kendoGrid({
            dataSource: dataSource,
            height: 550,
            sortable: window.ShiftModule.config.gridOptions.sortable,
            filterable: window.ShiftModule.config.gridOptions.filterable,
            pageable: window.ShiftModule.config.gridOptions.pageable,
            columns: [
                {
                    field: 'shiftId',
                    title: 'ID',
                    width: 80,
                    filterable: false
                },
                {
                    field: 'shiftName',
                    title: 'Shift Name',
                    width: 200
                },
                {
                    field: 'shiftCode',
                    title: 'Code',
                    width: 120,
                    template: function (dataItem) {
                        return dataItem.shiftCode || '<span style="color: #999;">-</span>';
                    }
                },
                {
                    field: 'startTime',
                    title: 'Start Time',
                    width: 120,
                    template: function (dataItem) {
                        return dataItem.startTime || '-';
                    },
                    filterable: false
                },
                {
                    field: 'endTime',
                    title: 'End Time',
                    width: 120,
                    template: function (dataItem) {
                        return dataItem.endTime || '-';
                    },
                    filterable: false
                },
                {
                    field: 'workHours',
                    title: 'Work Hours',
                    width: 120,
                    template: function (dataItem) {
                        return (dataItem.workHours || 8) + ' hrs';
                    },
                    filterable: false
                },
                {
                    field: 'isNightShift',
                    title: 'Night Shift',
                    width: 120,
                    template: function (dataItem) {
                        if (dataItem.isNightShift === 1) {
                            return '<span class="badge badge-warning">Yes</span>';
                        } else {
                            return '<span class="badge badge-secondary">No</span>';
                        }
                    },
                    filterable: {
                        ui: function (element) {
                            element.kendoDropDownList({
                                dataSource: [
                                    { text: 'All', value: '' },
                                    { text: 'Yes', value: '1' },
                                    { text: 'No', value: '0' }
                                ],
                                dataTextField: 'text',
                                dataValueField: 'value',
                                optionLabel: 'All'
                            });
                        }
                    }
                },
                {
                    field: 'isFlexible',
                    title: 'Flexible',
                    width: 110,
                    template: function (dataItem) {
                        if (dataItem.isFlexible === 1) {
                            return '<span class="badge badge-info">Yes</span>';
                        } else {
                            return '<span class="badge badge-secondary">No</span>';
                        }
                    },
                    filterable: {
                        ui: function (element) {
                            element.kendoDropDownList({
                                dataSource: [
                                    { text: 'All', value: '' },
                                    { text: 'Yes', value: '1' },
                                    { text: 'No', value: '0' }
                                ],
                                dataTextField: 'text',
                                dataValueField: 'value',
                                optionLabel: 'All'
                            });
                        }
                    }
                },
                {
                    field: 'isActive',
                    title: 'Status',
                    width: 120,
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
                            <button class="btn-grid-action btn-edit" data-id="${dataItem.shiftId}" title="Edit">
                                <span>✏️</span> Edit
                            </button>
                            <button class="btn-grid-action btn-delete" data-id="${dataItem.shiftId}" title="Delete">
                                <span>🗑️</span> Delete
                            </button>
                        `;
                    }
                }
            ],
            dataBound: onDataBound
        }).data('kendoGrid');

        console.log('Shift grid initialized');
    }

    /**
     * Handle grid data bound event
     */
    function onDataBound() {
        $('.btn-edit').off('click').on('click', function () {
            const shiftId = parseInt($(this).data('id'));
            if (window.ShiftModule.Details && typeof window.ShiftModule.Details.openEditForm === 'function') {
                window.ShiftModule.Details.openEditForm(shiftId);
            }
        });

        $('.btn-delete').off('click').on('click', function () {
            const shiftId = parseInt($(this).data('id'));
            deleteShift(shiftId);
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
     * Delete shift
     * @param {number} shiftId - Shift ID to delete
     */
    async function deleteShift(shiftId) {
        if (!shiftId || shiftId <= 0) {
            window.AppToast?.error('Invalid shift ID');
            return;
        }

        if (!confirm('Are you sure you want to delete this shift?\\n\\nThis action cannot be undone.')) {
            return;
        }

        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.delete(
                window.ShiftModule.config.apiEndpoints.delete(shiftId)
            );

            if (response.success) {
                window.AppToast?.success(response.message || 'Shift deleted successfully');
                refreshGrid();
            } else {
                window.AppToast?.error(response.message || 'Failed to delete shift');
            }
        } catch (error) {
            console.error('Error deleting shift:', error);
            window.AppToast?.error(error.message || 'Error deleting shift');
        } finally {
            window.AppLoader?.hide();
        }
    }

})();
