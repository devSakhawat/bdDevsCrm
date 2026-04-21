/**
 * Document Summary - Grid Module
 * This file handles grid operations (list, search, delete, download)
 */

(function () {
    'use strict';

    window.DocumentModule = window.DocumentModule || {};

    // Grid instance
    let grid = null;

    window.DocumentModule.Summary = {
        init: initializeGrid,
        refreshGrid: refreshGrid,
        deleteDocument: deleteDocument,
        downloadDocument: downloadDocument
    };

    /**
     * Initialize Kendo Grid
     */
    function initializeGrid() {
        const dataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: window.DocumentModule.config.apiEndpoints.summary,
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
                    id: 'documentId',
                    fields: {
                        documentId: { type: 'number' },
                        documentName: { type: 'string' },
                        documentType: { type: 'string' },
                        fileName: { type: 'string' },
                        fileSize: { type: 'number' },
                        fileSizeFormatted: { type: 'string' },
                        uploadDate: { type: 'date' },
                        uploadedBy: { type: 'string' },
                        accessLevel: { type: 'string' },
                        tags: { type: 'string' },
                        downloadCount: { type: 'number' },
                        version: { type: 'number' },
                        isActive: { type: 'boolean' }
                    }
                }
            },
            pageSize: window.DocumentModule.config.gridOptions.pageSize,
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            error: function (e) {
                console.error('Grid data source error:', e);
                window.AppToast?.error('Error loading grid data');
            }
        });

        grid = $('#documentGrid').kendoGrid({
            dataSource: dataSource,
            height: 600,
            sortable: window.DocumentModule.config.gridOptions.sortable,
            filterable: window.DocumentModule.config.gridOptions.filterable,
            pageable: window.DocumentModule.config.gridOptions.pageable,
            columns: [
                {
                    field: 'documentId',
                    title: 'ID',
                    width: 80,
                    filterable: false
                },
                {
                    field: 'documentName',
                    title: 'Document Name',
                    width: 250,
                    template: function (dataItem) {
                        const fileExt = getFileExtension(dataItem.fileName);
                        const badge = getFileBadge(fileExt);
                        return `${badge} <strong>${dataItem.documentName}</strong>`;
                    }
                },
                {
                    field: 'documentType',
                    title: 'Type',
                    width: 120
                },
                {
                    field: 'fileName',
                    title: 'File Name',
                    width: 200,
                    template: function (dataItem) {
                        return `<span title="${dataItem.fileName}">${truncateText(dataItem.fileName, 30)}</span>`;
                    }
                },
                {
                    field: 'fileSizeFormatted',
                    title: 'Size',
                    width: 100,
                    filterable: false,
                    template: function (dataItem) {
                        return dataItem.fileSizeFormatted || formatFileSize(dataItem.fileSize);
                    }
                },
                {
                    field: 'uploadDate',
                    title: 'Upload Date',
                    width: 150,
                    template: function (dataItem) {
                        if (dataItem.uploadDate) {
                            const date = new Date(dataItem.uploadDate);
                            return date.toLocaleDateString('en-GB') + ' ' + date.toLocaleTimeString('en-GB', { hour: '2-digit', minute: '2-digit' });
                        }
                        return '-';
                    },
                    filterable: {
                        ui: function (element) {
                            element.kendoDatePicker({
                                format: 'dd/MM/yyyy'
                            });
                        }
                    }
                },
                {
                    field: 'uploadedBy',
                    title: 'Uploaded By',
                    width: 150
                },
                {
                    field: 'accessLevel',
                    title: 'Access',
                    width: 120,
                    template: function (dataItem) {
                        const level = dataItem.accessLevel || 'Private';
                        const cssClass = 'access-level-' + level.toLowerCase();
                        return `<span class="${cssClass}"><strong>${level}</strong></span>`;
                    },
                    filterable: {
                        ui: function (element) {
                            element.kendoDropDownList({
                                dataSource: [
                                    { text: 'All', value: '' },
                                    { text: 'Public', value: 'Public' },
                                    { text: 'Private', value: 'Private' },
                                    { text: 'Restricted', value: 'Restricted' }
                                ],
                                dataTextField: 'text',
                                dataValueField: 'value',
                                optionLabel: 'All'
                            });
                        }
                    }
                },
                {
                    field: 'version',
                    title: 'Ver.',
                    width: 80,
                    filterable: false,
                    template: function (dataItem) {
                        return `v${dataItem.version || 1}`;
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
                    width: 220,
                    filterable: false,
                    sortable: false,
                    template: function (dataItem) {
                        return `
                            <button class="btn-grid-action btn-download" data-id="${dataItem.documentId}" title="Download">
                                <i class="k-icon k-i-download"></i> Download
                            </button>
                            <button class="btn-grid-action btn-edit" data-id="${dataItem.documentId}" title="Edit">
                                <i class="k-icon k-i-edit"></i> Edit
                            </button>
                            <button class="btn-grid-action btn-delete" data-id="${dataItem.documentId}" title="Delete">
                                <i class="k-icon k-i-delete"></i> Delete
                            </button>
                        `;
                    }
                }
            ],
            dataBound: onDataBound
        }).data('kendoGrid');

        console.log('Document grid initialized');
    }

    /**
     * Handle grid data bound event
     */
    function onDataBound() {
        // Attach event handlers to action buttons
        $('.btn-download').off('click').on('click', function () {
            const documentId = parseInt($(this).data('id'));
            downloadDocument(documentId);
        });

        $('.btn-edit').off('click').on('click', function () {
            const documentId = parseInt($(this).data('id'));
            if (window.DocumentModule.Details && typeof window.DocumentModule.Details.openEditForm === 'function') {
                window.DocumentModule.Details.openEditForm(documentId);
            }
        });

        $('.btn-delete').off('click').on('click', function () {
            const documentId = parseInt($(this).data('id'));
            deleteDocument(documentId);
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
     * Delete document
     * @param {number} documentId - Document ID to delete
     */
    async function deleteDocument(documentId) {
        if (!documentId || documentId <= 0) {
            window.AppToast?.error('Invalid document ID');
            return;
        }

        // Confirm deletion
        if (!confirm('Are you sure you want to delete this document?\n\nThis action cannot be undone and will delete the file permanently.')) {
            return;
        }

        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.delete(
                window.DocumentModule.config.apiEndpoints.delete(documentId)
            );

            if (response.success) {
                window.AppToast?.success(response.message || 'Document deleted successfully');
                refreshGrid();
            } else {
                window.AppToast?.error(response.message || 'Failed to delete document');
            }
        } catch (error) {
            console.error('Error deleting document:', error);
            window.AppToast?.error(error.message || 'Error deleting document');
        } finally {
            window.AppLoader?.hide();
        }
    }

    /**
     * Download document
     * @param {number} documentId - Document ID to download
     */
    async function downloadDocument(documentId) {
        if (!documentId || documentId <= 0) {
            window.AppToast?.error('Invalid document ID');
            return;
        }

        window.AppLoader?.show();

        try {
            const downloadUrl = window.DocumentModule.config.apiEndpoints.download(documentId);
            const token = window.ApiClient?.getToken();

            // Create a hidden link and trigger download
            const link = document.createElement('a');
            link.href = downloadUrl;
            link.target = '_blank';

            // If token is required, add it to the request
            if (token) {
                const response = await fetch(downloadUrl, {
                    method: 'GET',
                    headers: {
                        'Authorization': `Bearer ${token}`
                    }
                });

                if (!response.ok) {
                    throw new Error('Download failed');
                }

                const blob = await response.blob();
                const blobUrl = window.URL.createObjectURL(blob);

                // Get filename from Content-Disposition header or use default
                const contentDisposition = response.headers.get('Content-Disposition');
                let fileName = 'document';
                if (contentDisposition) {
                    const matches = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/.exec(contentDisposition);
                    if (matches != null && matches[1]) {
                        fileName = matches[1].replace(/['"]/g, '');
                    }
                }

                link.href = blobUrl;
                link.download = fileName;
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);
                window.URL.revokeObjectURL(blobUrl);

                window.AppToast?.success('Document download started');
            } else {
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);
            }

        } catch (error) {
            console.error('Error downloading document:', error);
            window.AppToast?.error(error.message || 'Error downloading document');
        } finally {
            window.AppLoader?.hide();
        }
    }

    /**
     * Get file extension from filename
     * @param {string} fileName - File name
     * @returns {string} - File extension
     */
    function getFileExtension(fileName) {
        if (!fileName) return '';
        const parts = fileName.split('.');
        return parts.length > 1 ? parts[parts.length - 1].toLowerCase() : '';
    }

    /**
     * Get file badge HTML based on extension
     * @param {string} extension - File extension
     * @returns {string} - Badge HTML
     */
    function getFileBadge(extension) {
        const ext = extension.toLowerCase();

        if (['pdf'].includes(ext)) {
            return '<span class="badge-file badge-pdf">PDF</span>';
        } else if (['doc', 'docx'].includes(ext)) {
            return '<span class="badge-file badge-doc">DOC</span>';
        } else if (['xls', 'xlsx'].includes(ext)) {
            return '<span class="badge-file badge-doc">XLS</span>';
        } else if (['jpg', 'jpeg', 'png', 'gif'].includes(ext)) {
            return '<span class="badge-file badge-img">IMG</span>';
        } else {
            return '<span class="badge-file badge-other">' + ext.toUpperCase() + '</span>';
        }
    }

    /**
     * Format file size
     * @param {number} bytes - File size in bytes
     * @returns {string} - Formatted file size
     */
    function formatFileSize(bytes) {
        if (!bytes || bytes === 0) return '0 Bytes';
        const k = 1024;
        const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
        const i = Math.floor(Math.log(bytes) / Math.log(k));
        return Math.round((bytes / Math.pow(k, i)) * 100) / 100 + ' ' + sizes[i];
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
