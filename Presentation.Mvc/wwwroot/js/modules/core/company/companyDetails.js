/**
 * Company Details - Form CRUD Module
 * This file handles Create, Read, Update operations for Company
 */

(function () {
    'use strict';

    window.CompanyModule = window.CompanyModule || {};

    // Form window instance
    let formWindow = null;
    let formValidator = null;
    let isEditMode = false;

    window.CompanyModule.Details = {
        init: initializeForm,
        openAddForm: openAddForm,
        openEditForm: openEditForm,
        saveCompany: saveCompany,
        clearForm: clearForm
    };

    /**
     * Initialize form and window
     */
    function initializeForm() {
        // Initialize Kendo Window
        formWindow = $('#companyFormWindow').kendoWindow({
            width: window.CompanyModule.config.windowOptions.width,
            title: window.CompanyModule.config.windowOptions.title,
            modal: window.CompanyModule.config.windowOptions.modal,
            visible: window.CompanyModule.config.windowOptions.visible,
            actions: window.CompanyModule.config.windowOptions.actions,
            close: onWindowClose
        }).data('kendoWindow');

        // Initialize Kendo Validator
        formValidator = $('#companyForm').kendoValidator({
            rules: {
                required: function (input) {
                    if (input.is('[required]')) {
                        return $.trim(input.val()) !== '';
                    }
                    return true;
                },
                companyNameLength: function (input) {
                    if (input.is('[name="companyName"]')) {
                        const val = $.trim(input.val());
                        return val.length >= 2 && val.length <= 200;
                    }
                    return true;
                },
                shortNameLength: function (input) {
                    if (input.is('[name="shortName"]')) {
                        const val = $.trim(input.val());
                        return val.length >= 1 && val.length <= 50;
                    }
                    return true;
                },
                emailFormat: function (input) {
                    if (input.is('[name="email"]') && input.val()) {
                        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
                        return emailRegex.test(input.val());
                    }
                    return true;
                },
                phoneFormat: function (input) {
                    if (input.is('[name="phone"]') && input.val()) {
                        // Allow digits, spaces, dashes, parentheses, and plus sign
                        const phoneRegex = /^[\d\s\-\(\)\+]+$/;
                        return phoneRegex.test(input.val());
                    }
                    return true;
                },
                websiteFormat: function (input) {
                    if (input.is('[name="website"]') && input.val()) {
                        const urlRegex = /^https?:\/\/.+\..+/i;
                        return urlRegex.test(input.val());
                    }
                    return true;
                }
            },
            messages: {
                required: 'This field is required',
                companyNameLength: 'Company name must be between 2 and 200 characters',
                shortNameLength: 'Short name must be between 1 and 50 characters',
                emailFormat: 'Please enter a valid email address',
                phoneFormat: 'Please enter a valid phone number',
                websiteFormat: 'Website must start with http:// or https://'
            }
        }).data('kendoValidator');

        // Attach form submit handler
        $('#companyForm').on('submit', function (e) {
            e.preventDefault();
            saveCompany();
        });

        // Attach cancel button handler
        $('#btnCancel').on('click', function () {
            formWindow.close();
        });

        console.log('Company form initialized');
    }

    /**
     * Open form for adding new company
     */
    function openAddForm() {
        isEditMode = false;
        clearForm();
        formWindow.title('Add New Company');
        formWindow.center().open();
        $('#companyName').focus();
    }

    /**
     * Open form for editing existing company
     * @param {number} companyId - Company ID to edit
     */
    async function openEditForm(companyId) {
        if (!companyId || companyId <= 0) {
            window.AppToast?.error('Invalid company ID');
            return;
        }

        isEditMode = true;
        formWindow.title('Edit Company');
        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.get(
                window.CompanyModule.config.apiEndpoints.read(companyId)
            );

            if (response.success && response.data) {
                populateForm(response.data);
                formWindow.center().open();
                $('#companyName').focus();
            } else {
                window.AppToast?.error(response.message || 'Failed to load company details');
            }
        } catch (error) {
            console.error('Error loading company:', error);
            window.AppToast?.error('Error loading company details');
        } finally {
            window.AppLoader?.hide();
        }
    }

    /**
     * Save company (Create or Update)
     */
    async function saveCompany() {
        // Validate form
        if (!formValidator.validate()) {
            window.AppToast?.warning('Please fix validation errors');
            return;
        }

        const formData = {
            companyId: parseInt($('#companyId').val()) || 0,
            companyName: $.trim($('#companyName').val()),
            shortName: $.trim($('#shortName').val()),
            address: $.trim($('#address').val()) || null,
            phone: $.trim($('#phone').val()) || null,
            email: $.trim($('#email').val()) || null,
            website: $.trim($('#website').val()) || null,
            isActive: $('#isActive').is(':checked')
        };

        window.AppLoader?.show();

        try {
            let response;

            if (isEditMode) {
                // Update existing company
                response = await window.ApiClient.put(
                    window.CompanyModule.config.apiEndpoints.update(formData.companyId),
                    formData
                );
            } else {
                // Create new company
                response = await window.ApiClient.post(
                    window.CompanyModule.config.apiEndpoints.create,
                    formData
                );
            }

            if (response.success) {
                window.AppToast?.success(response.message || 'Company saved successfully');
                formWindow.close();

                // Refresh grid
                if (window.CompanyModule.Summary && typeof window.CompanyModule.Summary.refreshGrid === 'function') {
                    window.CompanyModule.Summary.refreshGrid();
                }
            } else {
                window.AppToast?.error(response.message || 'Failed to save company');
            }
        } catch (error) {
            console.error('Error saving company:', error);
            window.AppToast?.error(error.message || 'Error saving company');
        } finally {
            window.AppLoader?.hide();
        }
    }

    /**
     * Populate form with company data
     * @param {object} data - Company data
     */
    function populateForm(data) {
        $('#companyId').val(data.companyId || 0);
        $('#companyName').val(data.companyName || '');
        $('#shortName').val(data.shortName || '');
        $('#address').val(data.address || '');
        $('#phone').val(data.phone || '');
        $('#email').val(data.email || '');
        $('#website').val(data.website || '');
        $('#isActive').prop('checked', data.isActive !== false);
    }

    /**
     * Clear form fields
     */
    function clearForm() {
        $('#companyId').val(0);
        $('#companyName').val('');
        $('#shortName').val('');
        $('#address').val('');
        $('#phone').val('');
        $('#email').val('');
        $('#website').val('');
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
