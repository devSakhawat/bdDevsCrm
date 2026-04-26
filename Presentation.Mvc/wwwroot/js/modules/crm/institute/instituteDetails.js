/**
 * Institute Details - Form Module
 * This file handles form operations (create, update, read) for Institute
 */

(function () {
    'use strict';

    window.InstituteModule = window.InstituteModule || {};

    // Window and validator instances
    let detailsWindow = null;
    let validator = null;
    let countryDropdown = null;
    let isEditMode = false;

    window.InstituteModule.Details = {
        init: initializeForm,
        openAddForm: openAddForm,
        openEditForm: openEditForm
    };

    /**
     * Initialize form window and validator
     */
    function initializeForm() {
        // Initialize Kendo Window
        detailsWindow = $('#instituteWindow').kendoWindow({
            width: window.InstituteModule.config.windowOptions.width,
            title: window.InstituteModule.config.windowOptions.title,
            visible: window.InstituteModule.config.windowOptions.visible,
            modal: window.InstituteModule.config.windowOptions.modal,
            actions: window.InstituteModule.config.windowOptions.actions,
            close: onWindowClose
        }).data('kendoWindow');

        // Initialize Country Dropdown
        countryDropdown = $('#countryId').kendoDropDownList({
            dataTextField: 'countryName',
            dataValueField: 'countryId',
            optionLabel: '-- Select Country --',
            dataSource: {
                transport: {
                    read: {
                        url: window.InstituteModule.config.apiEndpoints.countriesDDL,
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

        // Initialize Kendo Validator
        validator = $('#instituteForm').kendoValidator({
            rules: {
                instituteNameRequired: function (input) {
                    if (input.is('[name="instituteName"]')) {
                        return $.trim(input.val()).length > 0;
                    }
                    return true;
                },
                countryRequired: function (input) {
                    if (input.is('[name="countryId"]')) {
                        const countryId = parseInt(input.val());
                        return countryId > 0;
                    }
                    return true;
                }
            },
            messages: {
                instituteNameRequired: 'Institute name is required',
                countryRequired: 'Country is required'
            }
        }).data('kendoValidator');

        // Attach form button handlers
        $('#btnSaveInstitute').on('click', saveInstitute);
        $('#btnCancelInstitute').on('click', function () {
            detailsWindow.close();
        });

        console.log('Institute form initialized');
    }

    /**
     * Open form for adding new institute
     */
    function openAddForm() {
        isEditMode = false;
        resetForm();
        detailsWindow.title('Add New Institute');
        detailsWindow.center().open();
    }

    /**
     * Open form for editing existing institute
     * @param {number} instituteId - Institute ID to edit
     */
    async function openEditForm(instituteId) {
        if (!instituteId || instituteId <= 0) {
            window.AppToast?.error('Invalid institute ID');
            return;
        }

        isEditMode = true;
        resetForm();
        detailsWindow.title('Edit Institute');
        detailsWindow.center().open();

        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.get(
                window.InstituteModule.config.apiEndpoints.read(instituteId)
            );

            if (response.success && response.data) {
                populateForm(response.data);
            } else {
                window.AppToast?.error(response.message || 'Failed to load institute details');
                detailsWindow.close();
            }
        } catch (error) {
            console.error('Error loading institute:', error);
            window.AppToast?.error(error.message || 'Error loading institute details');
            detailsWindow.close();
        } finally {
            window.AppLoader?.hide();
        }
    }

    /**
     * Populate form with institute data
     * @param {object} data - Institute data
     */
    function populateForm(data) {
        $('#instituteId').val(data.instituteId || 0);
        $('#instituteName').val(data.instituteName || '');
        $('#instituteCode').val(data.instituteCode || '');
        $('#campus').val(data.campus || '');
        $('#instituteEmail').val(data.instituteEmail || '');
        $('#institutePhoneNo').val(data.institutePhoneNo || '');
        $('#instituteMobileNo').val(data.instituteMobileNo || '');
        $('#website').val(data.website || '');
        $('#instituteAddress').val(data.instituteAddress || '');
        $('#applicationFee').val(data.applicationFee || '');
        $('#monthlyLivingCost').val(data.monthlyLivingCost || '');
        $('#status').prop('checked', data.status === true || data.status === 1);

        // Set country dropdown value
        if (data.countryId && countryDropdown) {
            countryDropdown.value(data.countryId);
        }
    }

    /**
     * Reset form to default state
     */
    function resetForm() {
        $('#instituteForm')[0].reset();
        $('#instituteId').val(0);
        $('#status').prop('checked', true);

        if (countryDropdown) {
            countryDropdown.value('');
        }

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
     * Save institute (create or update)
     */
    async function saveInstitute() {
        // Validate form
        if (!validator.validate()) {
            window.AppToast?.warning('Please correct the validation errors');
            return;
        }

        // Collect form data
        const instituteId = parseInt($('#instituteId').val()) || 0;
        const countryId = countryDropdown ? parseInt(countryDropdown.value()) : 0;
        const instituteName = $.trim($('#instituteName').val());
        const instituteCode = $.trim($('#instituteCode').val()) || null;
        const campus = $.trim($('#campus').val()) || null;
        const instituteEmail = $.trim($('#instituteEmail').val()) || null;
        const institutePhoneNo = $.trim($('#institutePhoneNo').val()) || null;
        const instituteMobileNo = $.trim($('#instituteMobileNo').val()) || null;
        const website = $.trim($('#website').val()) || null;
        const instituteAddress = $.trim($('#instituteAddress').val()) || null;
        const applicationFee = parseFloat($('#applicationFee').val()) || null;
        const monthlyLivingCost = parseFloat($('#monthlyLivingCost').val()) || null;
        const status = $('#status').is(':checked');

        // Basic validation
        if (countryId <= 0) {
            window.AppToast?.warning('Please select a country');
            return;
        }

        if (!instituteName) {
            window.AppToast?.warning('Institute name is required');
            return;
        }

        const formData = {
            instituteId: instituteId,
            countryId: countryId,
            instituteName: instituteName,
            instituteCode: instituteCode,
            campus: campus,
            instituteEmail: instituteEmail,
            institutePhoneNo: institutePhoneNo,
            instituteMobileNo: instituteMobileNo,
            website: website,
            instituteAddress: instituteAddress,
            applicationFee: applicationFee,
            monthlyLivingCost: monthlyLivingCost,
            status: status
        };

        window.AppLoader?.show();

        try {
            let response;
            if (isEditMode && instituteId > 0) {
                // Update existing institute
                response = await window.ApiClient.put(
                    window.InstituteModule.config.apiEndpoints.update(instituteId),
                    formData
                );
            } else {
                // Create new institute
                response = await window.ApiClient.post(
                    window.InstituteModule.config.apiEndpoints.create,
                    formData
                );
            }

            if (response.success) {
                window.AppToast?.success(response.message || `Institute ${isEditMode ? 'updated' : 'created'} successfully`);
                detailsWindow.close();
                window.InstituteModule.Summary.refreshGrid();
            } else {
                window.AppToast?.error(response.message || `Failed to ${isEditMode ? 'update' : 'create'} institute`);
            }
        } catch (error) {
            console.error(`Error ${isEditMode ? 'updating' : 'creating'} institute:`, error);
            window.AppToast?.error(error.message || `Error ${isEditMode ? 'updating' : 'creating'} institute`);
        } finally {
            window.AppLoader?.hide();
        }
    }

})();
