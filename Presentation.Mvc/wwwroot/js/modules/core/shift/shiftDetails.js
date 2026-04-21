/**
 * Shift Details - Form Module
 * This file handles form operations (create, update, read) for Shift
 */

(function () {
    'use strict';

    window.ShiftModule = window.ShiftModule || {};

    // Window and validator instances
    let detailsWindow = null;
    let validator = null;
    let isEditMode = false;

    window.ShiftModule.Details = {
        init: initializeForm,
        openAddForm: openAddForm,
        openEditForm: openEditForm
    };

    /**
     * Initialize form window and validator
     */
    function initializeForm() {
        // Initialize Kendo Window
        detailsWindow = $('#shiftWindow').kendoWindow({
            width: window.ShiftModule.config.windowOptions.width,
            title: window.ShiftModule.config.windowOptions.title,
            visible: window.ShiftModule.config.windowOptions.visible,
            modal: window.ShiftModule.config.windowOptions.modal,
            actions: window.ShiftModule.config.windowOptions.actions,
            close: onWindowClose
        }).data('kendoWindow');

        // Initialize Kendo Validator
        validator = $('#shiftForm').kendoValidator({
            rules: {
                shiftNameRequired: function (input) {
                    if (input.is('[name="shiftName"]')) {
                        return $.trim(input.val()).length > 0;
                    }
                    return true;
                },
                startTimeRequired: function (input) {
                    if (input.is('[name="startTime"]')) {
                        return $.trim(input.val()).length > 0;
                    }
                    return true;
                },
                endTimeRequired: function (input) {
                    if (input.is('[name="endTime"]')) {
                        return $.trim(input.val()).length > 0;
                    }
                    return true;
                }
            },
            messages: {
                shiftNameRequired: 'Shift name is required',
                startTimeRequired: 'Start time is required',
                endTimeRequired: 'End time is required'
            }
        }).data('kendoValidator');

        // Attach form button handlers
        $('#btnSaveShift').on('click', saveShift);
        $('#btnCancelShift').on('click', function () {
            detailsWindow.close();
        });

        console.log('Shift form initialized');
    }

    /**
     * Open form for adding new shift
     */
    function openAddForm() {
        isEditMode = false;
        resetForm();
        detailsWindow.title('Add New Shift');
        detailsWindow.center().open();
    }

    /**
     * Open form for editing existing shift
     * @param {number} shiftId - Shift ID to edit
     */
    async function openEditForm(shiftId) {
        if (!shiftId || shiftId <= 0) {
            window.AppToast?.error('Invalid shift ID');
            return;
        }

        isEditMode = true;
        resetForm();
        detailsWindow.title('Edit Shift');
        detailsWindow.center().open();

        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.get(
                window.ShiftModule.config.apiEndpoints.read(shiftId)
            );

            if (response.success && response.data) {
                populateForm(response.data);
            } else {
                window.AppToast?.error(response.message || 'Failed to load shift details');
                detailsWindow.close();
            }
        } catch (error) {
            console.error('Error loading shift:', error);
            window.AppToast?.error(error.message || 'Error loading shift details');
            detailsWindow.close();
        } finally {
            window.AppLoader?.hide();
        }
    }

    /**
     * Populate form with shift data
     * @param {object} data - Shift data
     */
    function populateForm(data) {
        $('#shiftId').val(data.shiftId || 0);
        $('#shiftName').val(data.shiftName || '');
        $('#shiftCode').val(data.shiftCode || '');
        $('#startTime').val(data.startTime || '');
        $('#endTime').val(data.endTime || '');
        $('#graceTimeMinutes').val(data.graceTimeMinutes || 0);
        $('#workHours').val(data.workHours || 8);
        $('#isNightShift').prop('checked', data.isNightShift === 1);
        $('#isFlexible').prop('checked', data.isFlexible === 1);
        $('#isActive').prop('checked', data.isActive === 1);
    }

    /**
     * Reset form to default state
     */
    function resetForm() {
        $('#shiftForm')[0].reset();
        $('#shiftId').val(0);
        $('#graceTimeMinutes').val(0);
        $('#workHours').val(8);
        $('#isActive').prop('checked', true);
        $('#isNightShift').prop('checked', false);
        $('#isFlexible').prop('checked', false);

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
     * Save shift (create or update)
     */
    async function saveShift() {
        // Validate form
        if (!validator.validate()) {
            window.AppToast?.warning('Please correct the validation errors');
            return;
        }

        // Collect form data
        const shiftId = parseInt($('#shiftId').val()) || 0;
        const shiftName = $.trim($('#shiftName').val());
        const shiftCode = $.trim($('#shiftCode').val()) || null;
        const startTime = $.trim($('#startTime').val());
        const endTime = $.trim($('#endTime').val());
        const graceTimeMinutes = parseInt($('#graceTimeMinutes').val()) || 0;
        const workHours = parseFloat($('#workHours').val()) || 8;
        const isNightShift = $('#isNightShift').is(':checked') ? 1 : 0;
        const isFlexible = $('#isFlexible').is(':checked') ? 1 : 0;
        const isActive = $('#isActive').is(':checked') ? 1 : 0;

        // Basic validation
        if (!shiftName) {
            window.AppToast?.warning('Shift name is required');
            return;
        }

        if (!startTime || !endTime) {
            window.AppToast?.warning('Start time and end time are required');
            return;
        }

        const formData = {
            shiftId: shiftId,
            shiftName: shiftName,
            shiftCode: shiftCode,
            startTime: startTime,
            endTime: endTime,
            graceTimeMinutes: graceTimeMinutes,
            workHours: workHours,
            isNightShift: isNightShift,
            isFlexible: isFlexible,
            isActive: isActive
        };

        window.AppLoader?.show();

        try {
            let response;
            if (isEditMode && shiftId > 0) {
                // Update existing shift
                response = await window.ApiClient.put(
                    window.ShiftModule.config.apiEndpoints.update(shiftId),
                    formData
                );
            } else {
                // Create new shift
                response = await window.ApiClient.post(
                    window.ShiftModule.config.apiEndpoints.create,
                    formData
                );
            }

            if (response.success) {
                window.AppToast?.success(response.message || `Shift ${isEditMode ? 'updated' : 'created'} successfully`);
                detailsWindow.close();
                window.ShiftModule.Summary.refreshGrid();
            } else {
                window.AppToast?.error(response.message || `Failed to ${isEditMode ? 'update' : 'create'} shift`);
            }
        } catch (error) {
            console.error(`Error ${isEditMode ? 'updating' : 'creating'} shift:`, error);
            window.AppToast?.error(error.message || `Error ${isEditMode ? 'updating' : 'creating'} shift`);
        } finally {
            window.AppLoader?.hide();
        }
    }

})();
