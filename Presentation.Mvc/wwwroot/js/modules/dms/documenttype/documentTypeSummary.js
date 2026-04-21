/**
 * Document Type Summary - Grid Module
 * This file handles grid operations (list, search, delete)
 */

(function () {
    'use strict';

    window.DocumentTypeModule = window.DocumentTypeModule || {};

    // Grid instance
    let grid = null;

    window.DocumentTypeModule.Summary = {
        init: initializeGrid,
        refreshGrid: refreshGrid,
        deleteDocumentType: deleteDocumentType
    };

    /**
     * Initialize Kendo Grid
     */
    function initializeGrid() {
        const dataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: window.DocumentTypeModule.config.apiEndpoints.summary,
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
                    id: 'documentTypeId',
                    fields: {
                        documentTypeId: { type: 'number' },
                        typeName: { type: 'string' },
                        typeCode: { type: 'string' },
                        description: { type: 'string' },
                        allowedExtensions: { type: 'string' },
                        maxFileSize: { type: 'number' },
                        icon: { type: 'string' },
                        sortOrder: { type: 'number' },
                        requiresApproval: { type: 'boolean' },
                        isActive: { type: 'boolean' }
                    }
                }
            },
            pageSize: window.DocumentTypeModule.config.gridOptions.pageSize,
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            error: function (e) {
                console.error('Grid data source error:', e);
                window.AppToast?.error('Error loading grid data');
            }
        });

        grid = $('#documentTypeGrid').kendoGrid({
            dataSource: dataSource,
            height: 550,
            sortable: window.DocumentTypeModule.config.gridOptions.sortable,
            filterable: window.DocumentTypeModule.config.gridOptions.filterable,
            pageable: window.DocumentTypeModule.config.gridOptions.pageable,
            columns: [
                {
                    field: 'documentTypeId',
                    title: 'ID',
                    width: 80,
                    filterable: false
                },
                {
                    field: 'typeName',
                    title: 'Type Name',
                    width: 200,
                    template: function (dataItem) {
                        const iconHtml = dataItem.icon
                            ? `<i class="${dataItem.icon}" style="margin-right: 8px;"></i>`
                            : '<i class="k-icon k-i-file" style="margin-right: 8px;"></i>';
                        return `${iconHtml}<strong>${dataItem.typeName}</strong>`;
                    }
                },
                {
                    field: 'typeCode',
                    title: 'Code',
                    width: 100,
                    template: function (dataItem) {
                        if (dataItem.typeCode) {
                            return `<span class="badge badge-info">${dataItem.typeCode}</span>`;
                        }
                        return '-';
                    }
                },
                {
                    field: 'description',
                    title: 'Description',
                    width: 250,
                    template: function (dataItem) {
                        if (dataItem.description) {
                            return `<span title="${dataItem.description}">${truncateText(dataItem.description, 50)}</span>`;
                        }
                        return '-';
                    }
                },
                {
                    field: 'allowedExtensions',
                    title: 'Allowed Extensions',
                    width: 200,
                    filterable: false,
                    template: function (dataItem) {
                        if (dataItem.allowedExtensions) {
                            const extensions = dataItem.allowedExtensions.split(',').slice(0, 5);
                            let html = '';
                            extensions.forEach(ext => {
                                html += `<span class="extension-tag">${ext.trim()}</span>`;
                            });
                            if (dataItem.allowedExtensions.split(',').length > 5) {
                                html += '<span class="extension-tag">...</span>';
                            }
                            return html;
                        }
                        return '<span style="color: #999;">All</span>';
                    }
                },
                {
                    field: 'maxFileSize',
                    title: 'Max Size (MB)',
                    width: 120,
                    filterable: false,
                    template: function (dataItem) {
                        if (dataItem.maxFileSize && dataItem.maxFileSize > 0) {
                            return `${dataItem.maxFileSize} MB`;
                        }
                        return '<span style="color: #999;">No limit</span>';
                    }
                },
                {
                    field: 'sortOrder',
                    title: 'Sort Order',
                    width: 100,
                    filterable: false
                },
                {
                    field: 'requiresApproval',
                    title: 'Approval',
                    width: 100,
                    template: function (dataItem) {
                        if (dataItem.requiresApproval) {
                            return '<span class="badge badge-warning">Required</span>';
                        }
                        return '<span style="color: #999;">-</span>';
                    },
                    filterable: {
                        ui: function (element) {
                            element.kendoDropDownList({
                                dataSource: [
                                    { text: 'All', value: '' },
                                    { text: 'Required', value: 'true' },
                                    { text: 'Not Required', value: 'false' }
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
                            <button class="btn-grid-action btn-edit" data-id="${dataItem.documentTypeId}" title="Edit">
                                <i class="k-icon k-i-edit"></i> Edit
                            </button>
                            <button class="btn-grid-action btn-delete" data-id="${dataItem.documentTypeId}" title="Delete">
                                <i class="k-icon k-i-delete"></i> Delete
                            </button>
                        `;
                    }
                }
            ],
            dataBound: onDataBound
        }).data('kendoGrid');

        console.log('Document Type grid initialized');
    }

    /**
     * Handle grid data bound event
     */
    function onDataBound() {
        // Attach event handlers to action buttons
        $('.btn-edit').off('click').on('click', function () {
            const documentTypeId = parseInt($(this).data('id'));
            if (window.DocumentTypeModule.Details && typeof window.DocumentTypeModule.Details.openEditForm === 'function') {
                window.DocumentTypeModule.Details.openEditForm(documentTypeId);
            }
        });

        $('.btn-delete').off('click').on('click', function () {
            const documentTypeId = parseInt($(this).data('id'));
            deleteDocumentType(documentTypeId);
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
     * Delete document type
     * @param {number} documentTypeId - Document Type ID to delete
     */
    async function deleteDocumentType(documentTypeId) {
        if (!documentTypeId || documentTypeId <= 0) {
            window.AppToast?.error('Invalid document type ID');
            return;
        }

        // Confirm deletion
        if (!confirm('Are you sure you want to delete this document type?\n\nThis action cannot be undone.')) {
            return;
        }

        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.delete(
                window.DocumentTypeModule.config.apiEndpoints.delete(documentTypeId)
            );

            if (response.success) {
                window.AppToast?.success(response.message || 'Document type deleted successfully');
                refreshGrid();
            } else {
                window.AppToast?.error(response.message || 'Failed to delete document type');
            }
        } catch (error) {
            console.error('Error deleting document type:', error);
            window.AppToast?.error(error.message || 'Error deleting document type');
        } finally {
            window.AppLoader?.hide();
        }
    }

    /**
     * Truncate text
     * @param {string} text - Text to truncate
     * @param {number} length - Maximum length
     * @returns {string} - Truncated text
     */
    function truncateText(text, length) {
        if (!text) return '';
        if (text.length <= length) return text;
        return text.substring(0, length) + '...';
    }

})();
