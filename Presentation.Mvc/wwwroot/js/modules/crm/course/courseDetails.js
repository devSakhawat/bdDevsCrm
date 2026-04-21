/**
 * Course Details - Form CRUD Module
 * This file handles form operations (create, read, update) for Courses
 */

(function () {
    'use strict';

    window.CourseModule = window.CourseModule || {};

    // Form state
    let formWindow = null;
    let validator = null;
    let instituteDropdown = null;
    let isEditMode = false;
    let currentCourseId = null;

    window.CourseModule.Details = {
        init: initializeFormComponents,
        openAddForm: openAddForm,
        openEditForm: openEditForm
    };

    /**
     * Initialize form components
     */
    function initializeFormComponents() {
        // Initialize Kendo Window
        formWindow = $('#courseFormWindow').kendoWindow(
            window.CourseModule.config.windowOptions
        ).data('kendoWindow');

        // Initialize Institute DropDownList
        instituteDropdown = $('#instituteId').kendoDropDownList({
            dataTextField: 'instituteName',
            dataValueField: 'instituteId',
            optionLabel: 'Select Institute...',
            dataSource: {
                transport: {
                    read: {
                        url: window.CourseModule.config.apiEndpoints.institutesDDL,
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
        validator = $('#courseForm').kendoValidator({
            rules: {
                requiredDropdown: function (input) {
                    if (input.is('[name="instituteId"]')) {
                        const value = instituteDropdown?.value();
                        return value && value > 0;
                    }
                    return true;
                },
                emailFormat: function (input) {
                    if (input.is('[name="courseEmail"]') && input.val()) {
                        return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(input.val());
                    }
                    return true;
                },
                positiveNumber: function (input) {
                    if (input.is('[type="number"]') && input.val()) {
                        return parseFloat(input.val()) >= 0;
                    }
                    return true;
                }
            },
            messages: {
                requiredDropdown: 'Please select an institute',
                emailFormat: 'Please enter a valid email address',
                positiveNumber: 'Value must be 0 or greater'
            }
        }).data('kendoValidator');

        // Attach form submit handler
        $('#btnSaveCourse').off('click').on('click', handleFormSubmit);
        $('#btnCancelCourse').off('click').on('click', closeForm);

        console.log('Course form components initialized');
    }

    /**
     * Open form in Add mode
     */
    function openAddForm() {
        isEditMode = false;
        currentCourseId = null;
        resetForm();
        formWindow.title('Add New Course');
        formWindow.center().open();
    }

    /**
     * Open form in Edit mode
     * @param {number} courseId - Course ID to edit
     */
    async function openEditForm(courseId) {
        if (!courseId || courseId <= 0) {
            window.AppToast?.error('Invalid course ID');
            return;
        }

        isEditMode = true;
        currentCourseId = courseId;
        resetForm();
        formWindow.title('Edit Course');
        formWindow.center().open();

        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.get(
                window.CourseModule.config.apiEndpoints.read(courseId)
            );

            if (response.success && response.data) {
                populateForm(response.data);
            } else {
                window.AppToast?.error(response.message || 'Failed to load course data');
                formWindow.close();
            }
        } catch (error) {
            console.error('Error loading course:', error);
            window.AppToast?.error(error.message || 'Error loading course data');
            formWindow.close();
        } finally {
            window.AppLoader?.hide();
        }
    }

    /**
     * Populate form with course data
     * @param {object} data - Course data
     */
    function populateForm(data) {
        $('#courseId').val(data.courseId || 0);
        instituteDropdown?.value(data.instituteId || 0);
        $('#courseTitle').val(data.courseTitle || '');
        $('#courseLevel').val(data.courseLevel || '');
        $('#courseCategory').val(data.courseCategory || '');
        $('#courseDuration').val(data.courseDuration || '');
        $('#awardingBody').val(data.awardingBody || '');
        $('#courseFee').val(data.courseFee || '');
        $('#applicationFee').val(data.applicationFee || '');

        // Format dates for input[type="date"]
        if (data.startDate) {
            const startDate = new Date(data.startDate);
            $('#startDate').val(startDate.toISOString().split('T')[0]);
        }
        if (data.endDate) {
            const endDate = new Date(data.endDate);
            $('#endDate').val(endDate.toISOString().split('T')[0]);
        }

        $('#generalEligibility').val(data.generalEligibility || '');
        $('#languagesRequirement').val(data.languagesRequirement || '');
        $('#courseDescription').val(data.courseDescription || '');
        $('#status').prop('checked', data.status === true || data.status === 1);
    }

    /**
     * Reset form to initial state
     */
    function resetForm() {
        $('#courseForm')[0].reset();
        $('#courseId').val(0);
        instituteDropdown?.value('');
        $('#status').prop('checked', true);
        validator?.hideMessages();
    }

    /**
     * Close form window
     */
    function closeForm() {
        formWindow.close();
        resetForm();
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
            if (isEditMode) {
                response = await window.ApiClient.put(
                    window.CourseModule.config.apiEndpoints.update(currentCourseId),
                    formData
                );
            } else {
                response = await window.ApiClient.post(
                    window.CourseModule.config.apiEndpoints.create,
                    formData
                );
            }

            if (response.success) {
                window.AppToast?.success(response.message || `Course ${isEditMode ? 'updated' : 'created'} successfully`);
                closeForm();
                if (window.CourseModule.Summary && typeof window.CourseModule.Summary.refreshGrid === 'function') {
                    window.CourseModule.Summary.refreshGrid();
                }
            } else {
                window.AppToast?.error(response.message || `Failed to ${isEditMode ? 'update' : 'create'} course`);
            }
        } catch (error) {
            console.error('Error saving course:', error);
            window.AppToast?.error(error.message || 'Error saving course');
        } finally {
            window.AppLoader?.hide();
        }
    }

    /**
     * Collect form data
     * @returns {object|null} - Form data object or null if validation fails
     */
    function collectFormData() {
        const courseId = parseInt($('#courseId').val()) || 0;
        const instituteId = instituteDropdown?.value();
        const courseTitle = $('#courseTitle').val()?.trim();
        const courseLevel = $('#courseLevel').val()?.trim();
        const courseCategory = $('#courseCategory').val()?.trim();
        const courseDuration = $('#courseDuration').val()?.trim();
        const awardingBody = $('#awardingBody').val()?.trim();
        const courseFee = parseFloat($('#courseFee').val()) || null;
        const applicationFee = parseFloat($('#applicationFee').val()) || null;
        const startDate = $('#startDate').val() || null;
        const endDate = $('#endDate').val() || null;
        const generalEligibility = $('#generalEligibility').val()?.trim();
        const languagesRequirement = $('#languagesRequirement').val()?.trim();
        const courseDescription = $('#courseDescription').val()?.trim();
        const status = $('#status').is(':checked');

        // Required field validation
        if (!courseTitle) {
            window.AppToast?.error('Course title is required');
            return null;
        }

        if (!instituteId || instituteId <= 0) {
            window.AppToast?.error('Please select an institute');
            return null;
        }

        const formData = {
            courseId: courseId,
            instituteId: instituteId,
            courseTitle: courseTitle,
            courseLevel: courseLevel || null,
            courseCategory: courseCategory || null,
            courseDuration: courseDuration || null,
            awardingBody: awardingBody || null,
            courseFee: courseFee,
            applicationFee: applicationFee,
            startDate: startDate,
            endDate: endDate,
            generalEligibility: generalEligibility || null,
            languagesRequirement: languagesRequirement || null,
            courseDescription: courseDescription || null,
            status: status
        };

        return formData;
    }

})();
