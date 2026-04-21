/**
 * Designation Details - Form Module
 * This file handles form operations (create, update, read) for Designation
 */

(function () {
    'use strict';

    window.DesignationModule = window.DesignationModule || {};

    // Window and validator instances
    let detailsWindow = null;
    let validator = null;
    let isEditMode = false;

    window.DesignationModule.Details = {
        init: initializeForm,
        openAddForm: openAddForm,
        openEditForm: openEditForm
    };

    /**
     * Initialize form window and validator
     */
    function initializeForm() {
        // Initialize Kendo Window
        detailsWindow = $('#designationWindow').kendoWindow({
            width: window.DesignationModule.config.windowOptions.width,
            title: window.DesignationModule.config.windowOptions.title,
            visible: window.DesignationModule.config.windowOptions.visible,
            modal: window.DesignationModule.config.windowOptions.modal,
            actions: window.DesignationModule.config.windowOptions.actions,
            close: onWindowClose
        }).data('kendoWindow');

        // Initialize Kendo Validator
        validator = $('#designationForm').kendoValidator({
            rules: {
                designationNameRequired: function (input) {
                    if (input.is('[name="designationName"]')) {
                        return $.trim(input.val()).length > 0;
                    }
                    return true;
                }
            },
            messages: {
                designationNameRequired: 'Designation name is required'
            }
        }).data('kendoValidator');

        // Attach form button handlers
        $('#btnSaveDesignation').on('click', saveDesignation);
        $('#btnCancelDesignation').on('click', function () {
            detailsWindow.close();
        });

        console.log('Designation form initialized');
    }

    /**
     * Open form for adding new designation
     */
    function openAddForm() {
        isEditMode = false;
        resetForm();
        detailsWindow.title('Add New Designation');
        detailsWindow.center().open();
    }

    /**
     * Open form for editing existing designation
     * @param {number} designationId - Designation ID to edit
     */
    async function openEditForm(designationId) {
        if (!designationId || designationId <= 0) {
            window.AppToast?.error('Invalid designation ID');
            return;
        }

        isEditMode = true;
        resetForm();
        detailsWindow.title('Edit Designation');
        detailsWindow.center().open();

        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.get(
                window.DesignationModule.config.apiEndpoints.read(designationId)
            );

            if (response.success && response.data) {
                populateForm(response.data);
            } else {
                window.AppToast?.error(response.message || 'Failed to load designation details');
                detailsWindow.close();
            }
        } catch (error) {
            console.error('Error loading designation:', error);
            window.AppToast?.error(error.message || 'Error loading designation details');
            detailsWindow.close();
        } finally {
            window.AppLoader?.hide();
        }
    }

    /**
     * Populate form with designation data
     * @param {object} data - Designation data
     */
    function populateForm(data) {
        $('#designationId').val(data.designationId || 0);
        $('#designationName').val(data.designationName || '');
        $('#designationCode').val(data.designationCode || '');
        $('#sortOrder').val(data.sortOrder || 0);
        $('#isActive').prop('checked', data.isActive === 1);
    }

    /**
     * Reset form to default state
     */
    function resetForm() {
        $('#designationForm')[0].reset();
        $('#designationId').val(0);
        $('#sortOrder').val(0);
        $('#isActive').prop('checked', true);

        if (validator) {
            validator.hideMessages();
        }
    }

    /**
     * Handle window close event
     */
    function onWindowClose() {
        resetForm();
    }

    /**
     * Save designation (create or update)
     */
    async function saveDesignation() {
        // Validate form
        if (!validator.validate()) {
            window.AppToast?.warning('Please correct the validation errors');
            return;
        }

        // Collect form data
        const designationId = parseInt($('#designationId').val()) || 0;
        const designationName = $.trim($('#designationName').val());
        const designationCode = $.trim($('#designationCode').val()) || null;
        const sortOrder = parseInt($('#sortOrder').val()) || 0;
        const isActive = $('#isActive').is(':checked') ? 1 : 0;

        // Basic validation
        if (!designationName) {
            window.AppToast?.warning('Designation name is required');
            return;
        }

        const formData = {
            designationId: designationId,
            designationName: designationName,
            designationCode: designationCode,
            sortOrder: sortOrder,
            isActive: isActive
        };

        window.AppLoader?.show();

        try {
            let response;
            if (isEditMode && designationId > 0) {
                // Update existing designation
                response = await window.ApiClient.put(
                    window.DesignationModule.config.apiEndpoints.update(designationId),
                    formData
                );
            } else {
                // Create new designation
                response = await window.ApiClient.post(
                    window.DesignationModule.config.apiEndpoints.create,
                    formData
                );
            }

            if (response.success) {
                window.AppToast?.success(response.message || `Designation ${isEditMode ? 'updated' : 'created'} successfully`);
                detailsWindow.close();
                window.DesignationModule.Summary.refreshGrid();
            } else {
                window.AppToast?.error(response.message || `Failed to ${isEditMode ? 'update' : 'create'} designation`);
            }
        } catch (error) {
            console.error(`Error ${isEditMode ? 'updating' : 'creating'} designation:`, error);
            window.AppToast?.error(error.message || `Error ${isEditMode ? 'updating' : 'creating'} designation`);
        } finally {
            window.AppLoader?.hide();
        }
    }

})();
