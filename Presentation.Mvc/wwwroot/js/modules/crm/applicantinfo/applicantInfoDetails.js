/**
 * Applicant Info Details - Form CRUD Module
 * This file handles form operations (create, read, update) for Applicant Information
 */

(function () {
    'use strict';

    window.ApplicantInfoModule = window.ApplicantInfoModule || {};

    // Form state
    let formWindow = null;
    let validator = null;
    let tabStrip = null;
    let genderDropdown = null;
    let maritalStatusDropdown = null;
    let isEditMode = false;
    let currentApplicantId = null;
    let selectedPhotoFile = null;

    window.ApplicantInfoModule.Details = {
        init: initializeFormComponents,
        openAddForm: openAddForm,
        openEditForm: openEditForm
    };

    /**
     * Initialize form components
     */
    function initializeFormComponents() {
        // Initialize Kendo Window
        formWindow = $('#applicantFormWindow').kendoWindow(
            window.ApplicantInfoModule.config.windowOptions
        ).data('kendoWindow');

        // Initialize TabStrip
        tabStrip = $('#applicantTabStrip').kendoTabStrip({
            animation: {
                open: { effects: 'fadeIn' }
            }
        }).data('kendoTabStrip');

        // Initialize Gender DropDownList
        genderDropdown = $('#genderId').kendoDropDownList({
            dataTextField: 'genderName',
            dataValueField: 'genderId',
            optionLabel: 'Select Gender...',
            dataSource: {
                transport: {
                    read: {
                        url: window.ApplicantInfoModule.config.apiEndpoints.gendersDDL,
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

        // Initialize Marital Status DropDownList
        maritalStatusDropdown = $('#maritalStatusId').kendoDropDownList({
            dataTextField: 'maritalStatusName',
            dataValueField: 'maritalStatusId',
            optionLabel: 'Select Marital Status...',
            dataSource: {
                transport: {
                    read: {
                        url: window.ApplicantInfoModule.config.apiEndpoints.maritalStatusesDDL,
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

        // Initialize form validator
        validator = $('#applicantForm').kendoValidator({
            rules: {
                requiredDropdown: function (input) {
                    if (input.is('[name="genderId"]')) {
                        const value = genderDropdown?.value();
                        return value && value > 0;
                    }
                    if (input.is('[name="maritalStatusId"]')) {
                        const value = maritalStatusDropdown?.value();
                        return value && value > 0;
                    }
                    return true;
                },
                emailFormat: function (input) {
                    if (input.is('[name="emailAddress"]') && input.val()) {
                        return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(input.val());
                    }
                    return true;
                },
                dateOfBirthValid: function (input) {
                    if (input.is('[name="dateOfBirth"]') && input.val()) {
                        const dob = new Date(input.val());
                        const today = new Date();
                        const age = today.getFullYear() - dob.getFullYear();
                        return age >= 18 && age <= 100;
                    }
                    return true;
                },
                passportExpiryValid: function (input) {
                    if (input.is('[name="passportExpiryDate"]') && input.val()) {
                        const expiry = new Date(input.val());
                        const today = new Date();
                        return expiry > today;
                    }
                    return true;
                }
            },
            messages: {
                requiredDropdown: 'This field is required',
                emailFormat: 'Please enter a valid email address',
                dateOfBirthValid: 'Applicant must be between 18 and 100 years old',
                passportExpiryValid: 'Passport expiry date must be in the future'
            }
        }).data('kendoValidator');

        // Attach event handlers
        $('#btnSaveApplicant').off('click').on('click', handleFormSubmit);
        $('#btnCancelApplicant').off('click').on('click', closeForm);
        $('#btnUploadPhoto').off('click').on('click', () => $('#applicantImageFile').click());
        $('#applicantImageFile').off('change').on('change', handlePhotoSelection);

        console.log('Applicant Info form components initialized');
    }

    /**
     * Open form in Add mode
     */
    function openAddForm() {
        isEditMode = false;
        currentApplicantId = null;
        selectedPhotoFile = null;
        resetForm();
        formWindow.title('Add New Applicant');
        formWindow.center().open();
        tabStrip.select(0); // Select first tab
    }

    /**
     * Open form in Edit mode
     * @param {number} applicantId - Applicant ID to edit
     */
    async function openEditForm(applicantId) {
        if (!applicantId || applicantId <= 0) {
            window.AppToast?.error('Invalid applicant ID');
            return;
        }

        isEditMode = true;
        currentApplicantId = applicantId;
        selectedPhotoFile = null;
        resetForm();
        formWindow.title('Edit Applicant');
        formWindow.center().open();
        tabStrip.select(0);

        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.get(
                window.ApplicantInfoModule.config.apiEndpoints.read(applicantId)
            );

            if (response.success && response.data) {
                populateForm(response.data);
            } else {
                window.AppToast?.error(response.message || 'Failed to load applicant data');
                formWindow.close();
            }
        } catch (error) {
            console.error('Error loading applicant:', error);
            window.AppToast?.error(error.message || 'Error loading applicant data');
            formWindow.close();
        } finally {
            window.AppLoader?.hide();
        }
    }

    /**
     * Populate form with applicant data
     * @param {object} data - Applicant data
     */
    function populateForm(data) {
        // Personal Details
        $('#applicantId').val(data.applicantId || 0);
        $('#firstName').val(data.firstName || '');
        $('#lastName').val(data.lastName || '');
        genderDropdown?.value(data.genderId || 0);
        maritalStatusDropdown?.value(data.maritalStatusId || 0);

        if (data.dateOfBirth) {
            const dob = new Date(data.dateOfBirth);
            $('#dateOfBirth').val(dob.toISOString().split('T')[0]);
        }
        $('#nationality').val(data.nationality || '');

        // Contact Information
        $('#emailAddress').val(data.emailAddress || '');
        $('#phoneCountryCode').val(data.phoneCountryCode || '');
        $('#phoneAreaCode').val(data.phoneAreaCode || '');
        $('#phoneNumber').val(data.phoneNumber || '');
        $('#mobile').val(data.mobile || '');
        $('#skypeId').val(data.skypeId || '');

        // Passport Information
        $('#hasValidPassport').prop('checked', data.hasValidPassport === true || data.hasValidPassport === 1);
        $('#passportNumber').val(data.passportNumber || '');

        if (data.passportIssueDate) {
            const issueDate = new Date(data.passportIssueDate);
            $('#passportIssueDate').val(issueDate.toISOString().split('T')[0]);
        }
        if (data.passportExpiryDate) {
            const expiryDate = new Date(data.passportExpiryDate);
            $('#passportExpiryDate').val(expiryDate.toISOString().split('T')[0]);
        }

        // Photo preview
        if (data.applicantImagePath) {
            displayPhotoPreview(data.applicantImagePath);
        }
    }

    /**
     * Reset form to initial state
     */
    function resetForm() {
        $('#applicantForm')[0].reset();
        $('#applicantId').val(0);
        genderDropdown?.value('');
        maritalStatusDropdown?.value('');
        validator?.hideMessages();
        resetPhotoPreview();
    }

    /**
     * Close form window
     */
    function closeForm() {
        formWindow.close();
        resetForm();
    }

    /**
     * Handle photo file selection
     */
    function handlePhotoSelection(e) {
        const file = e.target.files[0];
        if (!file) return;

        // Validate file type
        if (!file.type.startsWith('image/')) {
            window.AppToast?.error('Please select a valid image file');
            return;
        }

        // Validate file size (max 5MB)
        if (file.size > 5 * 1024 * 1024) {
            window.AppToast?.error('Image size must be less than 5MB');
            return;
        }

        selectedPhotoFile = file;

        // Preview image
        const reader = new FileReader();
        reader.onload = function (e) {
            displayPhotoPreview(e.target.result);
        };
        reader.readAsDataURL(file);
    }

    /**
     * Display photo preview
     * @param {string} imageSrc - Image source (base64 or URL)
     */
    function displayPhotoPreview(imageSrc) {
        const preview = $('#imagePreview');
        preview.html(`<img src="${imageSrc}" alt="Applicant Photo" />`);
    }

    /**
     * Reset photo preview
     */
    function resetPhotoPreview() {
        const preview = $('#imagePreview');
        preview.html('<div class="image-preview-placeholder">No photo uploaded</div>');
        selectedPhotoFile = null;
        $('#applicantImageFile').val('');
    }

    /**
     * Handle form submit
     */
    async function handleFormSubmit(e) {
        e.preventDefault();

        if (!validator.validate()) {
            window.AppToast?.warning('Please fix validation errors');
            return;
        }

        const formData = collectFormData();

        if (!formData) {
            return;
        }

        window.AppLoader?.show();

        try {
            let response;

            // If photo file is selected, upload it first
            if (selectedPhotoFile) {
                const photoUploadData = new FormData();
                photoUploadData.append('file', selectedPhotoFile);

                const uploadResponse = await fetch(
                    window.ApplicantInfoModule.config.apiEndpoints.uploadPhoto,
                    {
                        method: 'POST',
                        headers: {
                            'Authorization': `Bearer ${window.ApiClient?.getToken()}`
                        },
                        body: photoUploadData
                    }
                );

                const uploadResult = await uploadResponse.json();
                if (uploadResult.success && uploadResult.data) {
                    formData.applicantImagePath = uploadResult.data.filePath;
                }
            }

            if (isEditMode) {
                response = await window.ApiClient.put(
                    window.ApplicantInfoModule.config.apiEndpoints.update(currentApplicantId),
                    formData
                );
            } else {
                response = await window.ApiClient.post(
                    window.ApplicantInfoModule.config.apiEndpoints.create,
                    formData
                );
            }

            if (response.success) {
                window.AppToast?.success(response.message || `Applicant ${isEditMode ? 'updated' : 'created'} successfully`);
                closeForm();
                if (window.ApplicantInfoModule.Summary && typeof window.ApplicantInfoModule.Summary.refreshGrid === 'function') {
                    window.ApplicantInfoModule.Summary.refreshGrid();
                }
            } else {
                window.AppToast?.error(response.message || `Failed to ${isEditMode ? 'update' : 'create'} applicant`);
            }
        } catch (error) {
            console.error('Error saving applicant:', error);
            window.AppToast?.error(error.message || 'Error saving applicant');
        } finally {
            window.AppLoader?.hide();
        }
    }

    /**
     * Collect form data
     * @returns {object|null} - Form data object or null if validation fails
     */
    function collectFormData() {
        const applicantId = parseInt($('#applicantId').val()) || 0;
        const genderId = genderDropdown?.value();
        const maritalStatusId = maritalStatusDropdown?.value();
        const firstName = $('#firstName').val()?.trim();
        const lastName = $('#lastName').val()?.trim();
        const dateOfBirth = $('#dateOfBirth').val() || null;
        const nationality = $('#nationality').val()?.trim();
        const emailAddress = $('#emailAddress').val()?.trim();
        const phoneCountryCode = $('#phoneCountryCode').val()?.trim();
        const phoneAreaCode = $('#phoneAreaCode').val()?.trim();
        const phoneNumber = $('#phoneNumber').val()?.trim();
        const mobile = $('#mobile').val()?.trim();
        const skypeId = $('#skypeId').val()?.trim();
        const hasValidPassport = $('#hasValidPassport').is(':checked');
        const passportNumber = $('#passportNumber').val()?.trim();
        const passportIssueDate = $('#passportIssueDate').val() || null;
        const passportExpiryDate = $('#passportExpiryDate').val() || null;

        // Required field validation
        if (!firstName) {
            window.AppToast?.error('First name is required');
            tabStrip.select(0);
            return null;
        }

        if (!lastName) {
            window.AppToast?.error('Last name is required');
            tabStrip.select(0);
            return null;
        }

        if (!genderId || genderId <= 0) {
            window.AppToast?.error('Please select a gender');
            tabStrip.select(0);
            return null;
        }

        if (!maritalStatusId || maritalStatusId <= 0) {
            window.AppToast?.error('Please select marital status');
            tabStrip.select(0);
            return null;
        }

        if (!dateOfBirth) {
            window.AppToast?.error('Date of birth is required');
            tabStrip.select(0);
            return null;
        }

        if (!emailAddress) {
            window.AppToast?.error('Email address is required');
            tabStrip.select(1);
            return null;
        }

        const formData = {
            applicantId: applicantId,
            applicationId: 0, // Default, will be set when linking to application
            genderId: genderId,
            maritalStatusId: maritalStatusId,
            firstName: firstName,
            lastName: lastName,
            dateOfBirth: dateOfBirth,
            nationality: nationality || null,
            emailAddress: emailAddress,
            phoneCountryCode: phoneCountryCode || null,
            phoneAreaCode: phoneAreaCode || null,
            phoneNumber: phoneNumber || null,
            mobile: mobile || null,
            skypeId: skypeId || null,
            hasValidPassport: hasValidPassport,
            passportNumber: passportNumber || null,
            passportIssueDate: passportIssueDate,
            passportExpiryDate: passportExpiryDate
        };

        return formData;
    }

})();
