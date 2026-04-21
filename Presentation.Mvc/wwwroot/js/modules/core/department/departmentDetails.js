/**
 * Department Details - Form Module
 * This file handles form operations (create, update, read) for Department
 */

(function () {
    'use strict';

    window.DepartmentModule = window.DepartmentModule || {};

    // Window and validator instances
    let detailsWindow = null;
    let validator = null;
    let isEditMode = false;

    window.DepartmentModule.Details = {
        init: initializeForm,
        openAddForm: openAddForm,
        openEditForm: openEditForm
    };

    /**
     * Initialize form window and validator
     */
    function initializeForm() {
        // Initialize Kendo Window
        detailsWindow = $('#departmentWindow').kendoWindow({
            width: window.DepartmentModule.config.windowOptions.width,
            title: window.DepartmentModule.config.windowOptions.title,
            visible: window.DepartmentModule.config.windowOptions.visible,
            modal: window.DepartmentModule.config.windowOptions.modal,
            actions: window.DepartmentModule.config.windowOptions.actions,
            close: onWindowClose
        }).data('kendoWindow');

        // Initialize Kendo Validator
        validator = $('#departmentForm').kendoValidator({
            rules: {
                departmentNameRequired: function (input) {
                    if (input.is('[name="departmentName"]')) {
                        return $.trim(input.val()).length > 0;
                    }
                    return true;
                }
            },
            messages: {
                departmentNameRequired: 'Department name is required'
            }
        }).data('kendoValidator');

        // Attach form button handlers
        $('#btnSaveDepartment').on('click', saveDepartment);
        $('#btnCancelDepartment').on('click', function () {
            detailsWindow.close();
        });

        console.log('Department form initialized');
    }

    /**
     * Open form for adding new department
     */
    function openAddForm() {
        isEditMode = false;
        resetForm();
        detailsWindow.title('Add New Department');
        detailsWindow.center().open();
    }

    /**
     * Open form for editing existing department
     * @param {number} departmentId - Department ID to edit
     */
    async function openEditForm(departmentId) {
        if (!departmentId || departmentId <= 0) {
            window.AppToast?.error('Invalid department ID');
            return;
        }

        isEditMode = true;
        resetForm();
        detailsWindow.title('Edit Department');
        detailsWindow.center().open();

        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.get(
                window.DepartmentModule.config.apiEndpoints.read(departmentId)
            );

            if (response.success && response.data) {
                populateForm(response.data);
            } else {
                window.AppToast?.error(response.message || 'Failed to load department details');
                detailsWindow.close();
            }
        } catch (error) {
            console.error('Error loading department:', error);
            window.AppToast?.error(error.message || 'Error loading department details');
            detailsWindow.close();
        } finally {
            window.AppLoader?.hide();
        }
    }

    /**
     * Populate form with department data
     * @param {object} data - Department data
     */
    function populateForm(data) {
        $('#departmentId').val(data.departmentId || 0);
        $('#departmentName').val(data.departmentName || '');
        $('#departmentCode').val(data.departmentCode || '');
        $('#isCostCentre').prop('checked', data.isCostCentre === 1);
        $('#isActive').prop('checked', data.isActive === 1);
    }

    /**
     * Reset form to default state
     */
    function resetForm() {
        $('#departmentForm')[0].reset();
        $('#departmentId').val(0);
        $('#isActive').prop('checked', true);
        $('#isCostCentre').prop('checked', false);

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
     * Save department (create or update)
     */
    async function saveDepartment() {
        // Validate form
        if (!validator.validate()) {
            window.AppToast?.warning('Please correct the validation errors');
            return;
        }

        // Collect form data
        const departmentId = parseInt($('#departmentId').val()) || 0;
        const departmentName = $.trim($('#departmentName').val());
        const departmentCode = $.trim($('#departmentCode').val()) || null;
        const isCostCentre = $('#isCostCentre').is(':checked') ? 1 : 0;
        const isActive = $('#isActive').is(':checked') ? 1 : 0;

        // Basic validation
        if (!departmentName) {
            window.AppToast?.warning('Department name is required');
            return;
        }

        const formData = {
            departmentId: departmentId,
            departmentName: departmentName,
            departmentCode: departmentCode,
            isCostCentre: isCostCentre,
            isActive: isActive
        };

        window.AppLoader?.show();

        try {
            let response;
            if (isEditMode && departmentId > 0) {
                // Update existing department
                response = await window.ApiClient.put(
                    window.DepartmentModule.config.apiEndpoints.update(departmentId),
                    formData
                );
            } else {
                // Create new department
                response = await window.ApiClient.post(
                    window.DepartmentModule.config.apiEndpoints.create,
                    formData
                );
            }

            if (response.success) {
                window.AppToast?.success(response.message || `Department ${isEditMode ? 'updated' : 'created'} successfully`);
                detailsWindow.close();
                window.DepartmentModule.Summary.refreshGrid();
            } else {
                window.AppToast?.error(response.message || `Failed to ${isEditMode ? 'update' : 'create'} department`);
            }
        } catch (error) {
            console.error(`Error ${isEditMode ? 'updating' : 'creating'} department:`, error);
            window.AppToast?.error(error.message || `Error ${isEditMode ? 'updating' : 'creating'} department`);
        } finally {
            window.AppLoader?.hide();
        }
    }

})();
