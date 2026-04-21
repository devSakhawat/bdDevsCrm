/**
 * Module Details - Form CRUD Module
 * This file handles Create, Read, Update operations for Module
 */

(function () {
    'use strict';

    window.ModuleModule = window.ModuleModule || {};

    // Form window instance
    let formWindow = null;
    let formValidator = null;
    let isEditMode = false;

    window.ModuleModule.Details = {
        init: initializeForm,
        openAddForm: openAddForm,
        openEditForm: openEditForm,
        saveModule: saveModule,
        clearForm: clearForm
    };

    /**
     * Initialize form and window
     */
    function initializeForm() {
        // Initialize Kendo Window
        formWindow = $('#moduleFormWindow').kendoWindow({
            width: window.ModuleModule.config.windowOptions.width,
            title: window.ModuleModule.config.windowOptions.title,
            modal: window.ModuleModule.config.windowOptions.modal,
            visible: window.ModuleModule.config.windowOptions.visible,
            actions: window.ModuleModule.config.windowOptions.actions,
            close: onWindowClose
        }).data('kendoWindow');

        // Initialize Kendo Validator
        formValidator = $('#moduleForm').kendoValidator({
            rules: {
                required: function (input) {
                    if (input.is('[required]')) {
                        return $.trim(input.val()) !== '';
                    }
                    return true;
                },
                moduleNameLength: function (input) {
                    if (input.is('[name="moduleName"]')) {
                        const val = $.trim(input.val());
                        return val.length >= 2 && val.length <= 100;
                    }
                    return true;
                }
            },
            messages: {
                required: 'This field is required',
                moduleNameLength: 'Module name must be between 2 and 100 characters'
            }
        }).data('kendoValidator');

        // Attach form submit handler
        $('#moduleForm').on('submit', function (e) {
            e.preventDefault();
            saveModule();
        });

        // Attach cancel button handler
        $('#btnCancel').on('click', function () {
            formWindow.close();
        });

        console.log('Module form initialized');
    }

    /**
     * Open form for adding new module
     */
    function openAddForm() {
        isEditMode = false;
        clearForm();
        formWindow.title('Add New Module');
        formWindow.center().open();
        $('#moduleName').focus();
    }

    /**
     * Open form for editing existing module
     * @param {number} moduleId - Module ID to edit
     */
    async function openEditForm(moduleId) {
        if (!moduleId || moduleId <= 0) {
            window.AppToast?.error('Invalid module ID');
            return;
        }

        isEditMode = true;
        formWindow.title('Edit Module');
        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.get(
                window.ModuleModule.config.apiEndpoints.read(moduleId)
            );

            if (response.success && response.data) {
                populateForm(response.data);
                formWindow.center().open();
                $('#moduleName').focus();
            } else {
                window.AppToast?.error(response.message || 'Failed to load module details');
            }
        } catch (error) {
            console.error('Error loading module:', error);
            window.AppToast?.error('Error loading module details');
        } finally {
            window.AppLoader?.hide();
        }
    }

    /**
     * Save module (Create or Update)
     */
    async function saveModule() {
        // Validate form
        if (!formValidator.validate()) {
            window.AppToast?.warning('Please fix validation errors');
            return;
        }

        const formData = {
            moduleId: parseInt($('#moduleId').val()) || 0,
            moduleName: $.trim($('#moduleName').val()),
            description: $.trim($('#description').val()) || null,
            isActive: $('#isActive').is(':checked')
        };

        window.AppLoader?.show();

        try {
            let response;

            if (isEditMode) {
                // Update existing module
                response = await window.ApiClient.put(
                    window.ModuleModule.config.apiEndpoints.update(formData.moduleId),
                    formData
                );
            } else {
                // Create new module
                response = await window.ApiClient.post(
                    window.ModuleModule.config.apiEndpoints.create,
                    formData
                );
            }

            if (response.success) {
                window.AppToast?.success(response.message || 'Module saved successfully');
                formWindow.close();

                // Refresh grid
                if (window.ModuleModule.Summary && typeof window.ModuleModule.Summary.refreshGrid === 'function') {
                    window.ModuleModule.Summary.refreshGrid();
                }
            } else {
                window.AppToast?.error(response.message || 'Failed to save module');
            }
        } catch (error) {
            console.error('Error saving module:', error);
            window.AppToast?.error(error.message || 'Error saving module');
        } finally {
            window.AppLoader?.hide();
        }
    }

    /**
     * Populate form with module data
     * @param {object} data - Module data
     */
    function populateForm(data) {
        $('#moduleId').val(data.moduleId || 0);
        $('#moduleName').val(data.moduleName || '');
        $('#description').val(data.description || '');
        $('#isActive').prop('checked', data.isActive !== false);
    }

    /**
     * Clear form fields
     */
    function clearForm() {
        $('#moduleId').val(0);
        $('#moduleName').val('');
        $('#description').val('');
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
