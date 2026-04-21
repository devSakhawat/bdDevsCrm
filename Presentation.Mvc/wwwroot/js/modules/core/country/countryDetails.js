/**
 * Country Details - Form CRUD Module
 * This file handles Create, Read, Update operations for Country
 */

(function () {
    'use strict';

    window.CountryModule = window.CountryModule || {};

    // Form window instance
    let formWindow = null;
    let formValidator = null;
    let isEditMode = false;

    window.CountryModule.Details = {
        init: initializeForm,
        openAddForm: openAddForm,
        openEditForm: openEditForm,
        saveCountry: saveCountry,
        clearForm: clearForm
    };

    /**
     * Initialize form and window
     */
    function initializeForm() {
        // Initialize Kendo Window
        formWindow = $('#countryFormWindow').kendoWindow({
            width: window.CountryModule.config.windowOptions.width,
            title: window.CountryModule.config.windowOptions.title,
            modal: window.CountryModule.config.windowOptions.modal,
            visible: window.CountryModule.config.windowOptions.visible,
            actions: window.CountryModule.config.windowOptions.actions,
            close: onWindowClose
        }).data('kendoWindow');

        // Initialize Kendo Validator
        formValidator = $('#countryForm').kendoValidator({
            rules: {
                required: function (input) {
                    if (input.is('[required]')) {
                        return $.trim(input.val()) !== '';
                    }
                    return true;
                },
                countryNameLength: function (input) {
                    if (input.is('[name="countryName"]')) {
                        const val = $.trim(input.val());
                        return val.length >= 2 && val.length <= 100;
                    }
                    return true;
                }
            },
            messages: {
                required: 'This field is required',
                countryNameLength: 'Country name must be between 2 and 100 characters'
            }
        }).data('kendoValidator');

        // Attach form submit handler
        $('#countryForm').on('submit', function (e) {
            e.preventDefault();
            saveCountry();
        });

        // Attach cancel button handler
        $('#btnCancel').on('click', function () {
            formWindow.close();
        });

        console.log('Country form initialized');
    }

    /**
     * Open form for adding new country
     */
    function openAddForm() {
        isEditMode = false;
        clearForm();
        formWindow.title('Add New Country');
        formWindow.center().open();
        $('#countryName').focus();
    }

    /**
     * Open form for editing existing country
     * @param {number} countryId - Country ID to edit
     */
    async function openEditForm(countryId) {
        if (!countryId || countryId <= 0) {
            window.AppToast?.error('Invalid country ID');
            return;
        }

        isEditMode = true;
        formWindow.title('Edit Country');
        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.get(
                window.CountryModule.config.apiEndpoints.read(countryId)
            );

            if (response.success && response.data) {
                populateForm(response.data);
                formWindow.center().open();
                $('#countryName').focus();
            } else {
                window.AppToast?.error(response.message || 'Failed to load country details');
            }
        } catch (error) {
            console.error('Error loading country:', error);
            window.AppToast?.error('Error loading country details');
        } finally {
            window.AppLoader?.hide();
        }
    }

    /**
     * Save country (Create or Update)
     */
    async function saveCountry() {
        // Validate form
        if (!formValidator.validate()) {
            window.AppToast?.warning('Please fix validation errors');
            return;
        }

        const formData = {
            countryId: parseInt($('#countryId').val()) || 0,
            countryName: $.trim($('#countryName').val()),
            countryCode: $.trim($('#countryCode').val()) || null,
            sortOrder: parseInt($('#sortOrder').val()) || 0,
            remarks: $.trim($('#remarks').val()) || null,
            isActive: $('#isActive').is(':checked')
        };

        window.AppLoader?.show();

        try {
            let response;

            if (isEditMode) {
                // Update existing country
                response = await window.ApiClient.put(
                    window.CountryModule.config.apiEndpoints.update(formData.countryId),
                    formData
                );
            } else {
                // Create new country
                response = await window.ApiClient.post(
                    window.CountryModule.config.apiEndpoints.create,
                    formData
                );
            }

            if (response.success) {
                window.AppToast?.success(response.message || 'Country saved successfully');
                formWindow.close();

                // Refresh grid
                if (window.CountryModule.Summary && typeof window.CountryModule.Summary.refreshGrid === 'function') {
                    window.CountryModule.Summary.refreshGrid();
                }
            } else {
                window.AppToast?.error(response.message || 'Failed to save country');
            }
        } catch (error) {
            console.error('Error saving country:', error);
            window.AppToast?.error(error.message || 'Error saving country');
        } finally {
            window.AppLoader?.hide();
        }
    }

    /**
     * Populate form with country data
     * @param {object} data - Country data
     */
    function populateForm(data) {
        $('#countryId').val(data.countryId || 0);
        $('#countryName').val(data.countryName || '');
        $('#countryCode').val(data.countryCode || '');
        $('#sortOrder').val(data.sortOrder || 0);
        $('#remarks').val(data.remarks || '');
        $('#isActive').prop('checked', data.isActive !== false);
    }

    /**
     * Clear form fields
     */
    function clearForm() {
        $('#countryId').val(0);
        $('#countryName').val('');
        $('#countryCode').val('');
        $('#sortOrder').val(0);
        $('#remarks').val('');
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
