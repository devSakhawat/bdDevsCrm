/**
 * Document Details - Form CRUD Module
 * This file handles Create, Read, Update operations for Documents
 * Features: File upload with progress, version management, tag management, access control
 */

(function () {
    'use strict';

    window.DocumentModule = window.DocumentModule || {};

    // Form state
    let formWindow = null;
    let formValidator = null;
    let tabStrip = null;
    let documentTypeDropdown = null;
    let folderDropdown = null;
    let restrictedUsersDropdown = null;
    let isEditMode = false;
    let currentDocumentId = null;
    let uploadedFile = null;
    let tagsList = [];
    let versionHistory = [];

    window.DocumentModule.Details = {
        init: initializeForm,
        openAddForm: openAddForm,
        openEditForm: openEditForm,
        saveDocument: saveDocument,
        clearForm: clearForm
    };

    /**
     * Initialize form and window
     */
    function initializeForm() {
        // Initialize Kendo Window
        formWindow = $('#documentFormWindow').kendoWindow({
            width: window.DocumentModule.config.windowOptions.width,
            height: window.DocumentModule.config.windowOptions.height,
            title: window.DocumentModule.config.windowOptions.title,
            modal: window.DocumentModule.config.windowOptions.modal,
            visible: window.DocumentModule.config.windowOptions.visible,
            actions: window.DocumentModule.config.windowOptions.actions,
            close: onWindowClose
        }).data('kendoWindow');

        // Initialize TabStrip
        tabStrip = $('#documentTabStrip').kendoTabStrip({
            animation: {
                open: { effects: 'fadeIn' }
            }
        }).data('kendoTabStrip');

        // Initialize Document Type DropDownList
        documentTypeDropdown = $('#documentTypeId').kendoDropDownList({
            dataTextField: 'typeName',
            dataValueField: 'documentTypeId',
            optionLabel: 'Select Document Type...',
            dataSource: {
                transport: {
                    read: {
                        url: window.DocumentModule.config.apiEndpoints.documentTypesDDL,
                        dataType: 'json',
                        beforeSend: function (xhr) {
                            const token = window.ApiClient?.getToken();
                            if (token) {
                                xhr.setRequestHeader('Authorization', `Bearer ${token}`);
                            }
                        }
                    }
                },
                schema: {
                    data: function (response) {
                        if (response.success && response.data) {
                            return response.data;
                        }
                        return [];
                    }
                }
            }
        }).data('kendoDropDownList');

        // Initialize Folder DropDownList
        folderDropdown = $('#folderId').kendoDropDownList({
            dataTextField: 'folderName',
            dataValueField: 'folderId',
            optionLabel: 'Select Folder (Optional)...',
            dataSource: {
                transport: {
                    read: {
                        url: window.DocumentModule.config.apiEndpoints.documentFoldersDDL,
                        dataType: 'json',
                        beforeSend: function (xhr) {
                            const token = window.ApiClient?.getToken();
                            if (token) {
                                xhr.setRequestHeader('Authorization', `Bearer ${token}`);
                            }
                        }
                    }
                },
                schema: {
                    data: function (response) {
                        if (response.success && response.data) {
                            return response.data;
                        }
                        return [];
                    }
                }
            }
        }).data('kendoDropDownList');

        // Initialize Restricted Users MultiSelect
        restrictedUsersDropdown = $('#restrictedUsers').kendoMultiSelect({
            dataTextField: 'userName',
            dataValueField: 'userId',
            placeholder: 'Select users...',
            dataSource: {
                transport: {
                    read: {
                        url: window.DocumentModule.config.apiEndpoints.restrictedUsersDDL,
                        dataType: 'json',
                        beforeSend: function (xhr) {
                            const token = window.ApiClient?.getToken();
                            if (token) {
                                xhr.setRequestHeader('Authorization', `Bearer ${token}`);
                            }
                        }
                    }
                },
                schema: {
                    data: function (response) {
                        if (response.success && response.data) {
                            return response.data;
                        }
                        return [];
                    }
                }
            }
        }).data('kendoMultiSelect');

        // Initialize Kendo Validator
        formValidator = $('#documentForm').kendoValidator({
            rules: {
                required: function (input) {
                    if (input.is('[required]')) {
                        return $.trim(input.val()) !== '';
                    }
                    return true;
                },
                requiredDropdown: function (input) {
                    if (input.is('[name="documentTypeId"]')) {
                        const value = documentTypeDropdown?.value();
                        return value && value > 0;
                    }
                    return true;
                },
                documentNameLength: function (input) {
                    if (input.is('[name="documentName"]')) {
                        const val = $.trim(input.val());
                        return val.length >= 2 && val.length <= 200;
                    }
                    return true;
                }
            },
            messages: {
                required: 'This field is required',
                requiredDropdown: 'Please select a document type',
                documentNameLength: 'Document name must be between 2 and 200 characters'
            }
        }).data('kendoValidator');

        // Attach event handlers
        attachEventHandlers();

        console.log('Document form initialized');
    }

    /**
     * Attach event handlers
     */
    function attachEventHandlers() {
        // Form submit handlers
        $('#btnSaveDocument').on('click', saveDocument);
        $('#btnCancelDocument').on('click', closeForm);

        // File upload handlers
        $('#btnSelectFile').on('click', function () {
            $('#documentFile').click();
        });

        $('#documentFile').on('change', handleFileSelect);

        // Drag and drop handlers
        const uploadZone = document.getElementById('fileUploadZone');

        uploadZone.addEventListener('dragover', function (e) {
            e.preventDefault();
            e.stopPropagation();
            $(this).addClass('drag-over');
        });

        uploadZone.addEventListener('dragleave', function (e) {
            e.preventDefault();
            e.stopPropagation();
            $(this).removeClass('drag-over');
        });

        uploadZone.addEventListener('drop', function (e) {
            e.preventDefault();
            e.stopPropagation();
            $(this).removeClass('drag-over');

            const files = e.dataTransfer.files;
            if (files && files.length > 0) {
                handleFileUpload(files[0]);
            }
        });

        // Tag management handlers
        $('#btnAddTag').on('click', addTag);
        $('#tagInput').on('keypress', function (e) {
            if (e.which === 13) { // Enter key
                e.preventDefault();
                addTag();
            }
        });

        // Access level change handler
        $('#accessLevel').on('change', function () {
            const accessLevel = $(this).val();
            if (accessLevel === 'Restricted') {
                $('#restrictedUsersGroup').show();
            } else {
                $('#restrictedUsersGroup').hide();
            }
        });
    }

    /**
     * Open form for adding new document
     */
    function openAddForm() {
        isEditMode = false;
        currentDocumentId = null;
        uploadedFile = null;
        tagsList = [];
        versionHistory = [];
        clearForm();
        formWindow.title('Upload New Document');
        formWindow.center().open();
        tabStrip.select(0); // Select first tab
        $('#documentName').focus();
    }

    /**
     * Open form for editing existing document
     * @param {number} documentId - Document ID to edit
     */
    async function openEditForm(documentId) {
        if (!documentId || documentId <= 0) {
            window.AppToast?.error('Invalid document ID');
            return;
        }

        isEditMode = true;
        currentDocumentId = documentId;
        uploadedFile = null;
        tagsList = [];
        versionHistory = [];
        clearForm();
        formWindow.title('Edit Document');
        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.get(
                window.DocumentModule.config.apiEndpoints.read(documentId)
            );

            if (response.success && response.data) {
                populateForm(response.data);
                formWindow.center().open();
                tabStrip.select(0);
                $('#documentName').focus();
            } else {
                window.AppToast?.error(response.message || 'Failed to load document details');
            }
        } catch (error) {
            console.error('Error loading document:', error);
            window.AppToast?.error('Error loading document details');
        } finally {
            window.AppLoader?.hide();
        }
    }

    /**
     * Save document (Create or Update)
     */
    async function saveDocument() {
        // Validate form
        if (!formValidator.validate()) {
            window.AppToast?.warning('Please fix validation errors');
            tabStrip.select(0); // Go to Basic Info tab
            return;
        }

        // Validate file upload for new documents
        if (!isEditMode && !uploadedFile) {
            window.AppToast?.warning('Please upload a file');
            tabStrip.select(1); // Go to File Upload tab
            return;
        }

        const formData = new FormData();

        // Basic info
        formData.append('documentId', currentDocumentId || 0);
        formData.append('documentName', $.trim($('#documentName').val()));
        formData.append('documentTypeId', documentTypeDropdown?.value() || 0);
        formData.append('folderId', folderDropdown?.value() || 0);
        formData.append('description', $.trim($('#description').val()) || '');
        formData.append('tags', tagsList.join(','));
        formData.append('isActive', $('#isActive').is(':checked'));

        // File upload (if new file selected)
        if (uploadedFile) {
            formData.append('file', uploadedFile);
        }

        // Access control
        formData.append('accessLevel', $('#accessLevel').val() || 'Private');
        formData.append('allowDownload', $('#allowDownload').is(':checked'));
        formData.append('allowPrint', $('#allowPrint').is(':checked'));
        formData.append('allowShare', $('#allowShare').is(':checked'));
        formData.append('expiryDate', $('#expiryDate').val() || '');

        // Restricted users
        const restrictedUsers = restrictedUsersDropdown?.value() || [];
        restrictedUsers.forEach((userId, index) => {
            formData.append(`restrictedUsers[${index}]`, userId);
        });

        window.AppLoader?.show();

        try {
            let response;
            const config = {
                headers: {
                    'Authorization': `Bearer ${window.ApiClient?.getToken()}`
                },
                onUploadProgress: function (progressEvent) {
                    if (progressEvent.lengthComputable) {
                        const percentComplete = Math.round((progressEvent.loaded * 100) / progressEvent.total);
                        updateUploadProgress(percentComplete);
                    }
                }
            };

            if (isEditMode) {
                // Update existing document using fetch with progress
                response = await uploadWithProgress(
                    window.DocumentModule.config.apiEndpoints.update(currentDocumentId),
                    'PUT',
                    formData
                );
            } else {
                // Create new document
                response = await uploadWithProgress(
                    window.DocumentModule.config.apiEndpoints.create,
                    'POST',
                    formData
                );
            }

            if (response.success) {
                window.AppToast?.success(response.message || 'Document saved successfully');
                closeForm();

                // Refresh grid
                if (window.DocumentModule.Summary && typeof window.DocumentModule.Summary.refreshGrid === 'function') {
                    window.DocumentModule.Summary.refreshGrid();
                }
            } else {
                window.AppToast?.error(response.message || 'Failed to save document');
            }
        } catch (error) {
            console.error('Error saving document:', error);
            window.AppToast?.error(error.message || 'Error saving document');
        } finally {
            hideUploadProgress();
            window.AppLoader?.hide();
        }
    }

    /**
     * Upload with progress tracking
     * @param {string} url - API endpoint
     * @param {string} method - HTTP method
     * @param {FormData} formData - Form data
     * @returns {Promise} - Response promise
     */
    function uploadWithProgress(url, method, formData) {
        return new Promise((resolve, reject) => {
            const xhr = new XMLHttpRequest();

            xhr.upload.addEventListener('progress', function (e) {
                if (e.lengthComputable) {
                    const percentComplete = Math.round((e.loaded * 100) / e.total);
                    updateUploadProgress(percentComplete);
                }
            });

            xhr.addEventListener('load', function () {
                if (xhr.status >= 200 && xhr.status < 300) {
                    try {
                        const response = JSON.parse(xhr.responseText);
                        resolve(response);
                    } catch (error) {
                        reject(new Error('Failed to parse response'));
                    }
                } else {
                    reject(new Error('Upload failed with status: ' + xhr.status));
                }
            });

            xhr.addEventListener('error', function () {
                reject(new Error('Upload failed'));
            });

            xhr.open(method, url);
            xhr.setRequestHeader('Authorization', `Bearer ${window.ApiClient?.getToken()}`);
            xhr.send(formData);
        });
    }

    /**
     * Update upload progress
     * @param {number} percent - Progress percentage
     */
    function updateUploadProgress(percent) {
        $('#uploadProgress').show();
        $('#uploadProgressBar').css('width', percent + '%').text(percent + '%');
    }

    /**
     * Hide upload progress
     */
    function hideUploadProgress() {
        $('#uploadProgress').hide();
        $('#uploadProgressBar').css('width', '0%').text('0%');
    }

    /**
     * Handle file select
     * @param {Event} e - Change event
     */
    function handleFileSelect(e) {
        const files = e.target.files;
        if (files && files.length > 0) {
            handleFileUpload(files[0]);
        }
    }

    /**
     * Handle file upload
     * @param {File} file - File object
     */
    function handleFileUpload(file) {
        // Validate file size
        if (file.size > window.DocumentModule.config.upload.maxFileSize) {
            window.AppToast?.error(`File size exceeds maximum limit of ${formatFileSize(window.DocumentModule.config.upload.maxFileSize)}`);
            return;
        }

        // Validate file extension
        const fileName = file.name;
        const fileExt = '.' + fileName.split('.').pop().toLowerCase();

        if (!window.DocumentModule.config.upload.allowedExtensions.includes(fileExt)) {
            window.AppToast?.error('File type not allowed. Allowed types: ' + window.DocumentModule.config.upload.allowedExtensions.join(', '));
            return;
        }

        uploadedFile = file;

        // Display file info
        displayUploadedFileInfo(file);

        // Auto-fill document name if empty
        if (!$('#documentName').val()) {
            const nameWithoutExt = fileName.substring(0, fileName.lastIndexOf('.')) || fileName;
            $('#documentName').val(nameWithoutExt);
        }

        window.AppToast?.success('File selected: ' + fileName);
    }

    /**
     * Display uploaded file info
     * @param {File} file - File object
     */
    function displayUploadedFileInfo(file) {
        const fileInfo = `
            <div class="uploaded-file-item">
                <div class="file-info">
                    <i class="k-icon k-i-file"></i>
                    <div>
                        <strong>${file.name}</strong>
                        <div class="file-size">${formatFileSize(file.size)}</div>
                    </div>
                </div>
                <button type="button" class="btn-secondary" onclick="window.DocumentModule.Details.clearUploadedFile()">
                    <i class="k-icon k-i-close"></i> Remove
                </button>
            </div>
        `;
        $('#uploadedFileInfo').html(fileInfo);
    }

    /**
     * Clear uploaded file
     */
    window.DocumentModule.Details.clearUploadedFile = function () {
        uploadedFile = null;
        $('#documentFile').val('');
        $('#uploadedFileInfo').html('');
        window.AppToast?.info('File removed');
    };

    /**
     * Add tag
     */
    function addTag() {
        const tagInput = $('#tagInput');
        const tagValue = $.trim(tagInput.val());

        if (!tagValue) {
            return;
        }

        // Check if tag already exists
        if (tagsList.includes(tagValue)) {
            window.AppToast?.warning('Tag already exists');
            return;
        }

        tagsList.push(tagValue);
        tagInput.val('');
        displayTags();
        updateTagsField();
    }

    /**
     * Remove tag
     * @param {number} index - Tag index
     */
    function removeTag(index) {
        if (index >= 0 && index < tagsList.length) {
            tagsList.splice(index, 1);
            displayTags();
            updateTagsField();
        }
    }

    /**
     * Display tags
     */
    function displayTags() {
        const container = $('#tagContainer');

        if (tagsList.length === 0) {
            container.html('');
            return;
        }

        let html = '';
        tagsList.forEach((tag, index) => {
            html += `
                <div class="tag-chip">
                    ${tag}
                    <span class="remove" onclick="window.DocumentModule.Details.removeTagByIndex(${index})">&times;</span>
                </div>
            `;
        });

        container.html(html);
    }

    /**
     * Remove tag by index (exposed globally)
     */
    window.DocumentModule.Details.removeTagByIndex = function (index) {
        removeTag(index);
    };

    /**
     * Update tags hidden field
     */
    function updateTagsField() {
        $('#tags').val(tagsList.join(','));
    }

    /**
     * Populate form with document data
     * @param {object} data - Document data
     */
    function populateForm(data) {
        // Basic Info
        $('#documentId').val(data.documentId || 0);
        $('#documentName').val(data.documentName || '');
        documentTypeDropdown?.value(data.documentTypeId || 0);
        folderDropdown?.value(data.folderId || 0);
        $('#description').val(data.description || '');
        $('#isActive').prop('checked', data.isActive !== false);

        // Tags
        if (data.tags) {
            tagsList = data.tags.split(',').map(tag => tag.trim()).filter(tag => tag);
            displayTags();
            updateTagsField();
        }

        // File info (display existing file info)
        if (data.fileName) {
            const fileInfo = `
                <div class="uploaded-file-item">
                    <div class="file-info">
                        <i class="k-icon k-i-file"></i>
                        <div>
                            <strong>${data.fileName}</strong>
                            <div class="file-size">${data.fileSizeFormatted || formatFileSize(data.fileSize)}</div>
                            <div class="file-size">Current version: v${data.version || 1}</div>
                        </div>
                    </div>
                </div>
                <p style="margin-top: 10px; color: #666;">
                    <i class="k-icon k-i-information"></i> Upload a new file to create a new version
                </p>
            `;
            $('#uploadedFileInfo').html(fileInfo);
        }

        // Access Control
        $('#accessLevel').val(data.accessLevel || 'Private').trigger('change');
        $('#allowDownload').prop('checked', data.allowDownload !== false);
        $('#allowPrint').prop('checked', data.allowPrint !== false);
        $('#allowShare').prop('checked', data.allowShare === true);

        if (data.expiryDate) {
            const expiryDate = new Date(data.expiryDate);
            $('#expiryDate').val(expiryDate.toISOString().split('T')[0]);
        }

        // Restricted users
        if (data.restrictedUserIds && Array.isArray(data.restrictedUserIds)) {
            restrictedUsersDropdown?.value(data.restrictedUserIds);
        }

        // Version History
        if (data.versionHistory && Array.isArray(data.versionHistory)) {
            versionHistory = data.versionHistory;
            displayVersionHistory();
        }
    }

    /**
     * Display version history
     */
    function displayVersionHistory() {
        const container = $('#versionHistoryContainer');

        if (!versionHistory || versionHistory.length === 0) {
            container.html('<p style="color: #999;">No version history available.</p>');
            return;
        }

        let html = '';
        versionHistory.forEach(version => {
            const uploadDate = new Date(version.uploadDate);
            html += `
                <div class="version-history-item">
                    <div>
                        <span class="version-number">Version ${version.version}</span>
                        <span style="margin-left: 15px; color: #666;">
                            ${uploadDate.toLocaleDateString('en-GB')} ${uploadDate.toLocaleTimeString('en-GB', { hour: '2-digit', minute: '2-digit' })}
                        </span>
                    </div>
                    <div style="margin-top: 5px;">
                        <strong>File:</strong> ${version.fileName} (${formatFileSize(version.fileSize)})
                    </div>
                    <div style="margin-top: 5px;">
                        <strong>Uploaded by:</strong> ${version.uploadedBy || 'Unknown'}
                    </div>
                    ${version.versionNotes ? `<div style="margin-top: 5px;"><strong>Notes:</strong> ${version.versionNotes}</div>` : ''}
                </div>
            `;
        });

        container.html(html);
    }

    /**
     * Clear form fields
     */
    function clearForm() {
        $('#documentForm')[0].reset();
        $('#documentId').val(0);
        documentTypeDropdown?.value('');
        folderDropdown?.value('');
        restrictedUsersDropdown?.value([]);
        uploadedFile = null;
        tagsList = [];
        versionHistory = [];
        $('#documentFile').val('');
        $('#uploadedFileInfo').html('');
        $('#tagContainer').html('');
        $('#tags').val('');
        $('#versionHistoryContainer').html('<p style="color: #999;">No version history available. Upload a document to create the first version.</p>');
        $('#restrictedUsersGroup').hide();
        hideUploadProgress();

        // Clear validation messages
        if (formValidator) {
            formValidator.hideMessages();
        }
    }

    /**
     * Close form window
     */
    function closeForm() {
        formWindow.close();
    }

    /**
     * Handle window close event
     */
    function onWindowClose() {
        clearForm();
        isEditMode = false;
        currentDocumentId = null;
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

})();
