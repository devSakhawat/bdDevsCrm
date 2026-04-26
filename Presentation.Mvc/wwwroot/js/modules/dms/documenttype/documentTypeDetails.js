/**
 * Document Type Details - Form CRUD Module
 * This file handles Create, Read, Update operations for Document Type
 */

(function () {
    'use strict';

    window.DocumentTypeModule = window.DocumentTypeModule || {};

    // Form window instance
    let formWindow = null;
    let formValidator = null;
    let isEditMode = false;

    window.DocumentTypeModule.Details = {
        init: initializeForm,
        openAddForm: openAddForm,
        openEditForm: openEditForm,
        saveDocumentType: saveDocumentType,
        clearForm: clearForm
    };

    /**
     * Initialize form and window
     */
    function initializeForm() {
        // Initialize Kendo Window
        formWindow = $('#documentTypeFormWindow').kendoWindow({
            width: window.DocumentTypeModule.config.windowOptions.width,
            title: window.DocumentTypeModule.config.windowOptions.title,
            modal: window.DocumentTypeModule.config.windowOptions.modal,
            visible: window.DocumentTypeModule.config.windowOptions.visible,
            actions: window.DocumentTypeModule.config.windowOptions.actions,
            close: onWindowClose
        }).data('kendoWindow');

        // Initialize Kendo Validator
        formValidator = $('#documentTypeForm').kendoValidator({
            rules: {
                required: function (input) {
                    if (input.is('[required]')) {
                        return $.trim(input.val()) !== '';
                    }
                    return true;
                },
                typeNameLength: function (input) {
                    if (input.is('[name="typeName"]')) {
                        const val = $.trim(input.val());
                        return val.length >= 2 && val.length <= 100;
                    }
                    return true;
                },
                typeCodeLength: function (input) {
                    if (input.is('[name="typeCode"]') && input.val()) {
                        const val = $.trim(input.val());
                        return val.length <= 20;
                    }
                    return true;
                },
                maxFileSizeValid: function (input) {
                    if (input.is('[name="maxFileSize"]') && input.val()) {
                        const size = parseInt(input.val());
                        return size >= 1 && size <= 500;
                    }
                    return true;
                },
                extensionsFormat: function (input) {
                    if (input.is('[name="allowedExtensions"]') && input.val()) {
                        const val = $.trim(input.val());
                        // Check if format is correct (comma-separated, starts with dot)
                        const extensions = val.split(',').map(ext => ext.trim());
                        for (let ext of extensions) {
                            if (ext && !ext.startsWith('.')) {
                                return false;
                            }
                        }
                    }
                    return true;
                }
            },
            messages: {
                required: 'This field is required',
                typeNameLength: 'Type name must be between 2 and 100 characters',
                typeCodeLength: 'Type code must not exceed 20 characters',
                maxFileSizeValid: 'Max file size must be between 1 and 500 MB',
                extensionsFormat: 'Extensions must start with a dot (e.g., .pdf,.doc)'
            }
        }).data('kendoValidator');

        // Attach form submit handler
        $('#documentTypeForm').on('submit', function (e) {
            e.preventDefault();
            saveDocumentType();
        });

        // Attach button handlers
        $('#btnSaveDocumentType').on('click', saveDocumentType);
        $('#btnCancelDocumentType').on('click', function () {
            formWindow.close();
        });

        console.log('Document Type form initialized');
    }

    /**
     * Open form for adding new document type
     */
    function openAddForm() {
        isEditMode = false;
        clearForm();
        formWindow.title('Add New Document Type');
        formWindow.center().open();
        $('#typeName').focus();
    }

    /**
     * Open form for editing existing document type
     * @param {number} documentTypeId - Document Type ID to edit
     */
    async function openEditForm(documentTypeId) {
        if (!documentTypeId || documentTypeId <= 0) {
            window.AppToast?.error('Invalid document type ID');
            return;
        }

        isEditMode = true;
        formWindow.title('Edit Document Type');
        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.get(
                window.DocumentTypeModule.config.apiEndpoints.read(documentTypeId)
            );

            if (response.success && response.data) {
                populateForm(response.data);
                formWindow.center().open();
                $('#typeName').focus();
            } else {
                window.AppToast?.error(response.message || 'Failed to load document type details');
            }
        } catch (error) {
            console.error('Error loading document type:', error);
            window.AppToast?.error('Error loading document type details');
        } finally {
            window.AppLoader?.hide();
        }
    }

    /**
     * Save document type (Create or Update)
     */
    async function saveDocumentType() {
        // Validate form
        if (!formValidator.validate()) {
            window.AppToast?.warning('Please fix validation errors');
            return;
        }

        const formData = {
            documentTypeId: parseInt($('#documentTypeId').val()) || 0,
            typeName: $.trim($('#typeName').val()),
            typeCode: $.trim($('#typeCode').val()) || null,
            description: $.trim($('#description').val()) || null,
            allowedExtensions: $.trim($('#allowedExtensions').val()) || null,
            maxFileSize: parseInt($('#maxFileSize').val()) || null,
            icon: $.trim($('#icon').val()) || null,
            sortOrder: parseInt($('#sortOrder').val()) || 0,
            requiresApproval: $('#requiresApproval').is(':checked'),
            isActive: $('#isActive').is(':checked')
        };

        window.AppLoader?.show();

        try {
            let response;

            if (isEditMode) {
                // Update existing document type
                response = await window.ApiClient.put(
                    window.DocumentTypeModule.config.apiEndpoints.update(formData.documentTypeId),
                    formData
                );
            } else {
                // Create new document type
                response = await window.ApiClient.post(
                    window.DocumentTypeModule.config.apiEndpoints.create,
                    formData
                );
            }

            if (response.success) {
                window.AppToast?.success(response.message || 'Document type saved successfully');
                formWindow.close();

                // Refresh grid
                if (window.DocumentTypeModule.Summary && typeof window.DocumentTypeModule.Summary.refreshGrid === 'function') {
                    window.DocumentTypeModule.Summary.refreshGrid();
                }
            } else {
                window.AppToast?.error(response.message || 'Failed to save document type');
            }
        } catch (error) {
            console.error('Error saving document type:', error);
            window.AppToast?.error(error.message || 'Error saving document type');
        } finally {
            window.AppLoader?.hide();
        }
    }

    /**
     * Populate form with document type data
     * @param {object} data - Document type data
     */
    function populateForm(data) {
        $('#documentTypeId').val(data.documentTypeId || 0);
        $('#typeName').val(data.typeName || '');
        $('#typeCode').val(data.typeCode || '');
        $('#description').val(data.description || '');
        $('#allowedExtensions').val(data.allowedExtensions || '');
        $('#maxFileSize').val(data.maxFileSize || '');
        $('#icon').val(data.icon || '');
        $('#sortOrder').val(data.sortOrder || 0);
        $('#requiresApproval').prop('checked', data.requiresApproval === true);
        $('#isActive').prop('checked', data.isActive !== false);
    }

    /**
     * Clear form fields
     */
    function clearForm() {
        $('#documentTypeId').val(0);
        $('#typeName').val('');
        $('#typeCode').val('');
        $('#description').val('');
        $('#allowedExtensions').val('');
        $('#maxFileSize').val('');
        $('#icon').val('');
        $('#sortOrder').val(0);
        $('#requiresApproval').prop('checked', false);
        $('#isActive').prop('checked', true);

        // Clear validation messages
        if (formValidator) {
            formValidator.hideMessages();
        }
    }

    /**
     * Handle window close event
     */
    function onWindowClose() {
        clearForm();
        isEditMode = false;
    }

})();
