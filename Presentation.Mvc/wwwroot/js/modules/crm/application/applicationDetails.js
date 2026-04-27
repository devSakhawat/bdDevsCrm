/**
 * CRM Application Details - Form CRUD Module
 * This file handles form operations (create, read, update) for CRM Applications
 * Features: 7-tab form, cascading dropdowns, document upload, draft/submit
 */

(function () {
    'use strict';

    window.ApplicationModule = window.ApplicationModule || {};

    // Form state
    let formWindow = null;
    let validator = null;
    let tabStrip = null;
    let applicantDropdown = null;
    let statusDropdown = null;
    let countryDropdown = null;
    let instituteDropdown = null;
    let courseDropdown = null;
    let intakeMonthDropdown = null;
    let isEditMode = false;
    let currentApplicationId = null;
    let isDraftMode = false;
    let uploadedDocuments = [];

    window.ApplicationModule.Details = {
        init: initializeFormComponents,
        openAddForm: openAddForm,
        openEditForm: openEditForm
    };

    /**
     * Initialize form components
     */
    function initializeFormComponents() {
        // Initialize Kendo Window
        formWindow = $('#applicationFormWindow').kendoWindow(
            window.ApplicationModule.config.windowOptions
        ).data('kendoWindow');

        // Initialize TabStrip
        tabStrip = $('#applicationTabStrip').kendoTabStrip({
            animation: {
                open: { effects: 'fadeIn' }
            }
        }).data('kendoTabStrip');

        // Initialize Applicant DropDownList
        applicantDropdown = $('#applicantId').kendoDropDownList({
            dataTextField: 'applicantName',
            dataValueField: 'applicantId',
            optionLabel: 'Select Applicant...',
            dataSource: {
                transport: {
                    read: {
                        url: window.ApplicationModule.config.apiEndpoints.applicantsDDL,
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

        // Initialize Status DropDownList
        statusDropdown = $('#statusId').kendoDropDownList({
            dataTextField: 'statusName',
            dataValueField: 'statusId',
            optionLabel: 'Select Status...',
            dataSource: {
                transport: {
                    read: {
                        url: window.ApplicationModule.config.apiEndpoints.statusesDDL,
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

        // Initialize Country DropDownList (cascading parent)
        countryDropdown = $('#countryId').kendoDropDownList({
            dataTextField: 'countryName',
            dataValueField: 'countryId',
            optionLabel: 'Select Country...',
            dataSource: {
                transport: {
                    read: {
                        url: window.ApplicationModule.config.apiEndpoints.countriesDDL,
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
            },
            change: onCountryChange
        }).data('kendoDropDownList');

        // Initialize Institute DropDownList (cascading child of Country)
        instituteDropdown = $('#instituteId').kendoDropDownList({
            dataTextField: 'instituteName',
            dataValueField: 'instituteId',
            optionLabel: 'Select Institute...',
            autoBind: false,
            enable: false,
            change: onInstituteChange
        }).data('kendoDropDownList');

        // Initialize Course DropDownList (cascading child of Institute)
        courseDropdown = $('#courseId').kendoDropDownList({
            dataTextField: 'courseName',
            dataValueField: 'courseId',
            optionLabel: 'Select Course...',
            autoBind: false,
            enable: false
        }).data('kendoDropDownList');

        // Initialize Intake Month DropDownList
        intakeMonthDropdown = $('#intakeMonthId').kendoDropDownList({
            dataTextField: 'monthName',
            dataValueField: 'monthId',
            optionLabel: 'Select Intake Month...',
            dataSource: {
                transport: {
                    read: {
                        url: window.ApplicationModule.config.apiEndpoints.intakeMonthsDDL,
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
        validator = $('#applicationForm').kendoValidator({
            rules: {
                requiredDropdown: function (input) {
                    if (input.is('[name="applicantId"]')) {
                        const value = applicantDropdown?.value();
                        return value && value > 0;
                    }
                    if (input.is('[name="statusId"]')) {
                        const value = statusDropdown?.value();
                        return value && value > 0;
                    }
                    if (input.is('[name="countryId"]')) {
                        const value = countryDropdown?.value();
                        return value && value > 0;
                    }
                    if (input.is('[name="instituteId"]')) {
                        const value = instituteDropdown?.value();
                        return value && value > 0;
                    }
                    if (input.is('[name="courseId"]')) {
                        const value = courseDropdown?.value();
                        return value && value > 0;
                    }
                    if (input.is('[name="intakeMonthId"]')) {
                        const value = intakeMonthDropdown?.value();
                        return value && value > 0;
                    }
                    return true;
                },
                applicationDateValid: function (input) {
                    if (input.is('[name="applicationDate"]') && input.val()) {
                        const appDate = new Date(input.val());
                        const today = new Date();
                        return appDate <= today;
                    }
                    return true;
                },
                intakeYearValid: function (input) {
                    if (input.is('[name="intakeYear"]') && input.val()) {
                        const year = parseInt(input.val());
                        const currentYear = new Date().getFullYear();
                        return year >= currentYear && year <= currentYear + 5;
                    }
                    return true;
                }
            },
            messages: {
                requiredDropdown: 'This field is required',
                applicationDateValid: 'Application date cannot be in the future',
                intakeYearValid: 'Intake year must be between current year and 5 years from now'
            }
        }).data('kendoValidator');

        // Attach event handlers
        $('#btnSaveAsDraft').off('click').on('click', () => handleFormSubmit(true));
        $('#btnSaveApplication').off('click').on('click', () => handleFormSubmit(false));
        $('#btnCancelApplication').off('click').on('click', closeForm);
        $('#btnUploadDocument').off('click').on('click', () => $('#documentFile').click());
        $('#documentFile').off('change').on('change', handleDocumentUpload);
        $('#btnNextTab').off('click').on('click', navigateToNextTab);
        $('#btnPreviousTab').off('click').on('click', navigateToPreviousTab);
        $('#btnReviewApplication').off('click').on('click', showReviewTab);

        console.log('CRM Application form components initialized');
    }

    /**
     * Handle Country dropdown change (cascading)
     */
    function onCountryChange() {
        const countryId = countryDropdown.value();

        // Reset Institute and Course dropdowns
        instituteDropdown.value('');
        instituteDropdown.enable(false);
        courseDropdown.value('');
        courseDropdown.enable(false);

        if (!countryId || countryId <= 0) {
            return;
        }

        // Load institutes for selected country
        instituteDropdown.setDataSource({
            transport: {
                read: {
                    url: window.ApplicationModule.config.apiEndpoints.institutesDDL(countryId),
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
        });

        instituteDropdown.enable(true);
        instituteDropdown.dataSource.read();
    }

    /**
     * Handle Institute dropdown change (cascading)
     */
    function onInstituteChange() {
        const instituteId = instituteDropdown.value();

        // Reset Course dropdown
        courseDropdown.value('');
        courseDropdown.enable(false);

        if (!instituteId || instituteId <= 0) {
            return;
        }

        // Load courses for selected institute
        courseDropdown.setDataSource({
            transport: {
                read: {
                    url: window.ApplicationModule.config.apiEndpoints.coursesDDL(instituteId),
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
        });

        courseDropdown.enable(true);
        courseDropdown.dataSource.read();
    }

    /**
     * Navigate to next tab
     */
    function navigateToNextTab() {
        const currentIndex = tabStrip.select().index();
        const totalTabs = tabStrip.tabGroup.children().length;

        if (currentIndex < totalTabs - 1) {
            tabStrip.select(currentIndex + 1);
        }
    }

    /**
     * Navigate to previous tab
     */
    function navigateToPreviousTab() {
        const currentIndex = tabStrip.select().index();

        if (currentIndex > 0) {
            tabStrip.select(currentIndex - 1);
        }
    }

    /**
     * Show review tab with populated data
     */
    function showReviewTab() {
        updateReviewTab();
        tabStrip.select(6); // Select Review & Submit tab (index 6)
    }

    /**
     * Update Review tab with form data
     */
    function updateReviewTab() {
        const formData = collectFormData();

        if (!formData) {
            return;
        }

        // Build review HTML
        let reviewHtml = '<div class="review-content">';

        // Basic Information
        reviewHtml += '<div class="review-section">';
        reviewHtml += '<h4>Basic Information</h4>';
        reviewHtml += '<table class="review-table">';
        reviewHtml += `<tr><td><strong>Applicant:</strong></td><td>${applicantDropdown.text() || '-'}</td></tr>`;
        reviewHtml += `<tr><td><strong>Application Date:</strong></td><td>${formData.applicationDate ? new Date(formData.applicationDate).toLocaleDateString('en-GB') : '-'}</td></tr>`;
        reviewHtml += `<tr><td><strong>Status:</strong></td><td>${statusDropdown.text() || '-'}</td></tr>`;
        reviewHtml += '</table>';
        reviewHtml += '</div>';

        // Personal Information
        reviewHtml += '<div class="review-section">';
        reviewHtml += '<h4>Personal Information</h4>';
        reviewHtml += '<table class="review-table">';
        reviewHtml += `<tr><td><strong>Emergency Contact:</strong></td><td>${formData.emergencyContactName || '-'}</td></tr>`;
        reviewHtml += `<tr><td><strong>Emergency Phone:</strong></td><td>${formData.emergencyContactPhone || '-'}</td></tr>`;
        reviewHtml += `<tr><td><strong>Current Address:</strong></td><td>${formData.currentAddress || '-'}</td></tr>`;
        reviewHtml += '</table>';
        reviewHtml += '</div>';

        // Education History
        reviewHtml += '<div class="review-section">';
        reviewHtml += '<h4>Education History</h4>';
        reviewHtml += '<table class="review-table">';
        reviewHtml += `<tr><td><strong>Highest Qualification:</strong></td><td>${formData.highestQualification || '-'}</td></tr>`;
        reviewHtml += `<tr><td><strong>Institution:</strong></td><td>${formData.lastInstitutionName || '-'}</td></tr>`;
        reviewHtml += `<tr><td><strong>Year of Completion:</strong></td><td>${formData.yearOfCompletion || '-'}</td></tr>`;
        reviewHtml += `<tr><td><strong>GPA/Percentage:</strong></td><td>${formData.gpaOrPercentage || '-'}</td></tr>`;
        reviewHtml += '</table>';
        reviewHtml += '</div>';

        // Work Experience
        reviewHtml += '<div class="review-section">';
        reviewHtml += '<h4>Work Experience</h4>';
        reviewHtml += '<table class="review-table">';
        reviewHtml += `<tr><td><strong>Current/Last Employer:</strong></td><td>${formData.currentEmployer || '-'}</td></tr>`;
        reviewHtml += `<tr><td><strong>Job Title:</strong></td><td>${formData.currentJobTitle || '-'}</td></tr>`;
        reviewHtml += `<tr><td><strong>Years of Experience:</strong></td><td>${formData.yearsOfExperience || '-'}</td></tr>`;
        reviewHtml += '</table>';
        reviewHtml += '</div>';

        // Documents
        reviewHtml += '<div class="review-section">';
        reviewHtml += '<h4>Uploaded Documents</h4>';
        if (uploadedDocuments.length > 0) {
            reviewHtml += '<ul class="document-list">';
            uploadedDocuments.forEach(doc => {
                reviewHtml += `<li>${doc.fileName} (${doc.fileSize})</li>`;
            });
            reviewHtml += '</ul>';
        } else {
            reviewHtml += '<p style="color: #999;">No documents uploaded</p>';
        }
        reviewHtml += '</div>';

        // Course Selection
        reviewHtml += '<div class="review-section">';
        reviewHtml += '<h4>Course Selection</h4>';
        reviewHtml += '<table class="review-table">';
        reviewHtml += `<tr><td><strong>Country:</strong></td><td>${countryDropdown.text() || '-'}</td></tr>`;
        reviewHtml += `<tr><td><strong>Institute:</strong></td><td>${instituteDropdown.text() || '-'}</td></tr>`;
        reviewHtml += `<tr><td><strong>Course:</strong></td><td>${courseDropdown.text() || '-'}</td></tr>`;
        reviewHtml += `<tr><td><strong>Intake:</strong></td><td>${intakeMonthDropdown.text() || '-'} ${formData.intakeYear || ''}</td></tr>`;
        reviewHtml += `<tr><td><strong>Course Duration:</strong></td><td>${formData.courseDuration || '-'} months</td></tr>`;
        reviewHtml += `<tr><td><strong>Tuition Fee:</strong></td><td>${formData.tuitionFee || '-'}</td></tr>`;
        reviewHtml += '</table>';
        reviewHtml += '</div>';

        reviewHtml += '</div>';

        $('#reviewContent').html(reviewHtml);
    }

    /**
     * Open form in Add mode
     */
    function openAddForm() {
        isEditMode = false;
        currentApplicationId = null;
        isDraftMode = false;
        uploadedDocuments = [];
        resetForm();
        formWindow.title('Add New Application');
        formWindow.center().open();
        tabStrip.select(0); // Select first tab

        // Set default application date to today
        const today = new Date().toISOString().split('T')[0];
        $('#applicationDate').val(today);
    }

    /**
     * Open form in Edit mode
     * @param {number} applicationId - Application ID to edit
     */
    async function openEditForm(applicationId) {
        if (!applicationId || applicationId <= 0) {
            window.AppToast?.error('Invalid application ID');
            return;
        }

        isEditMode = true;
        currentApplicationId = applicationId;
        isDraftMode = false;
        uploadedDocuments = [];
        resetForm();
        formWindow.title('Edit Application');
        formWindow.center().open();
        tabStrip.select(0);

        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.get(
                window.ApplicationModule.config.apiEndpoints.read(applicationId)
            );

            if (response.success && response.data) {
                await populateForm(response.data);
            } else {
                window.AppToast?.error(response.message || 'Failed to load application data');
                formWindow.close();
            }
        } catch (error) {
            console.error('Error loading application:', error);
            window.AppToast?.error(error.message || 'Error loading application data');
            formWindow.close();
        } finally {
            window.AppLoader?.hide();
        }
    }

    /**
     * Populate form with application data
     * @param {object} data - Application data
     */
    async function populateForm(data) {
        // Basic Information (Tab 1)
        $('#applicationId').val(data.applicationId || 0);
        applicantDropdown?.value(data.applicantId || 0);
        statusDropdown?.value(data.statusId || 0);

        if (data.applicationDate) {
            const appDate = new Date(data.applicationDate);
            $('#applicationDate').val(appDate.toISOString().split('T')[0]);
        }

        // Personal Information (Tab 2)
        $('#emergencyContactName').val(data.emergencyContactName || '');
        $('#emergencyContactPhone').val(data.emergencyContactPhone || '');
        $('#emergencyContactRelation').val(data.emergencyContactRelation || '');
        $('#currentAddress').val(data.currentAddress || '');
        $('#permanentAddress').val(data.permanentAddress || '');

        // Education History (Tab 3)
        $('#highestQualification').val(data.highestQualification || '');
        $('#lastInstitutionName').val(data.lastInstitutionName || '');
        $('#yearOfCompletion').val(data.yearOfCompletion || '');
        $('#gpaOrPercentage').val(data.gpaOrPercentage || '');
        $('#fieldOfStudy').val(data.fieldOfStudy || '');

        // Work Experience (Tab 4)
        $('#currentEmployer').val(data.currentEmployer || '');
        $('#currentJobTitle').val(data.currentJobTitle || '');
        $('#yearsOfExperience').val(data.yearsOfExperience || '');
        $('#employmentStartDate').val(data.employmentStartDate ? new Date(data.employmentStartDate).toISOString().split('T')[0] : '');
        $('#employmentEndDate').val(data.employmentEndDate ? new Date(data.employmentEndDate).toISOString().split('T')[0] : '');

        // Course Selection (Tab 6) - with cascading
        if (data.countryId) {
            countryDropdown?.value(data.countryId);

            // Wait for institutes to load
            await new Promise(resolve => {
                const checkInstitutes = setInterval(() => {
                    if (instituteDropdown.dataSource.data().length > 0) {
                        clearInterval(checkInstitutes);
                        resolve();
                    }
                }, 100);

                // Timeout after 3 seconds
                setTimeout(() => {
                    clearInterval(checkInstitutes);
                    resolve();
                }, 3000);
            });

            if (data.instituteId) {
                instituteDropdown?.value(data.instituteId);

                // Wait for courses to load
                await new Promise(resolve => {
                    const checkCourses = setInterval(() => {
                        if (courseDropdown.dataSource.data().length > 0) {
                            clearInterval(checkCourses);
                            resolve();
                        }
                    }, 100);

                    // Timeout after 3 seconds
                    setTimeout(() => {
                        clearInterval(checkCourses);
                        resolve();
                    }, 3000);
                });

                if (data.courseId) {
                    courseDropdown?.value(data.courseId);
                }
            }
        }

        intakeMonthDropdown?.value(data.intakeMonthId || 0);
        $('#intakeYear').val(data.intakeYear || '');
        $('#courseDuration').val(data.courseDuration || '');
        $('#tuitionFee').val(data.tuitionFee || '');
        $('#applicationFee').val(data.applicationFee || '');

        // Additional fields
        $('#remarks').val(data.remarks || '');
        isDraftMode = data.isDraft === true || data.isDraft === 1;

        // Documents (Tab 5)
        let persistedDocuments = [];
        if (Array.isArray(data.documents)) {
            persistedDocuments = data.documents;
        } else if (Array.isArray(data.additionalDocuments)) {
            persistedDocuments = data.additionalDocuments;
        }

        if (persistedDocuments.length > 0) {
            uploadedDocuments = persistedDocuments.map(doc => ({
                documentId: doc.documentId || doc.additionalDocumentId || 0,
                fileName: doc.fileName || doc.documentName || '',
                filePath: doc.filePath || doc.documentPath || '',
                fileSize: doc.fileSize || '',
                documentType: doc.documentType || doc.recordType || doc.documentTitle || 'General'
            }));
            displayUploadedDocuments();
        }
    }

    /**
     * Reset form to initial state
     */
    function resetForm() {
        $('#applicationForm')[0].reset();
        $('#applicationId').val(0);
        applicantDropdown?.value('');
        statusDropdown?.value('');
        countryDropdown?.value('');
        instituteDropdown?.value('');
        instituteDropdown?.enable(false);
        courseDropdown?.value('');
        courseDropdown?.enable(false);
        intakeMonthDropdown?.value('');
        validator?.hideMessages();
        uploadedDocuments = [];
        displayUploadedDocuments();
        $('#reviewContent').html('');
    }

    /**
     * Close form window
     */
    function closeForm() {
        formWindow.close();
        resetForm();
    }

    /**
     * Handle document upload
     */
    async function handleDocumentUpload(e) {
        const files = e.target.files;
        if (!files || files.length === 0) return;

        const applicantId = applicantDropdown?.value() || null;
        const applicationId = parseInt($('#applicationId').val()) || 0;

        if ((!applicantId || applicantId <= 0) && applicationId <= 0) {
            window.AppToast?.warning('Please select an applicant before uploading documents');
            $('#documentFile').val('');
            return;
        }

        window.AppLoader?.show();

        try {
            for (let i = 0; i < files.length; i++) {
                const file = files[i];

                // Validate file size (max 10MB)
                if (file.size > 10 * 1024 * 1024) {
                    window.AppToast?.error(`File ${file.name} is too large. Maximum size is 10MB`);
                    continue;
                }

                const formData = new FormData();
                formData.append('file', file);
                formData.append('documentType', $('#documentType').val() || 'General');
                if (applicantId && applicantId > 0) {
                    formData.append('applicantId', applicantId);
                }
                if (applicationId > 0) {
                    formData.append('applicationId', applicationId);
                }

                const response = await fetch(
                    window.ApplicationModule.config.apiEndpoints.uploadDocument,
                    {
                        method: 'POST',
                        headers: {
                            'Authorization': `Bearer ${window.ApiClient?.getToken()}`
                        },
                        body: formData
                    }
                );

                const result = await response.json();

                if (result.success && result.data) {
                    uploadedDocuments.push({
                        documentId: result.data.documentId || 0,
                        fileName: result.data.fileName || file.name,
                        filePath: result.data.filePath,
                        fileSize: formatFileSize(file.size),
                        documentType: result.data.documentType || $('#documentType').val() || 'General'
                    });

                    window.AppToast?.success(`File ${file.name} uploaded successfully`);
                } else {
                    window.AppToast?.error(`Failed to upload ${file.name}`);
                }
            }

            displayUploadedDocuments();
            $('#documentFile').val(''); // Clear file input

        } catch (error) {
            console.error('Error uploading document:', error);
            window.AppToast?.error('Error uploading documents');
        } finally {
            window.AppLoader?.hide();
        }
    }

    /**
     * Display uploaded documents
     */
    function displayUploadedDocuments() {
        const container = $('#uploadedDocumentsList');

        if (!uploadedDocuments || uploadedDocuments.length === 0) {
            container.html('<p style="color: #999;">No documents uploaded yet</p>');
            return;
        }

        let html = '<table class="table table-sm">';
        html += '<thead><tr><th>File Name</th><th>Type</th><th>Size</th><th>Action</th></tr></thead>';
        html += '<tbody>';

        uploadedDocuments.forEach((doc, index) => {
            html += '<tr>';
            html += `<td>${escapeHtml(doc.fileName || '')}</td>`;
            html += `<td>${escapeHtml(doc.documentType || 'General')}</td>`;
            html += `<td>${escapeHtml(doc.fileSize || '')}</td>`;
            html += `<td><button type="button" class="btn btn-sm btn-danger btn-remove-document" data-index="${index}">Remove</button></td>`;
            html += '</tr>';
        });

        html += '</tbody></table>';
        container.html(html);

        // Attach remove handlers
        $('.btn-remove-document').off('click').on('click', function () {
            const index = parseInt($(this).data('index'));
            removeDocument(index);
        });
    }

    /**
     * Remove document from list
     * @param {number} index - Document index
     */
    function removeDocument(index) {
        if (index >= 0 && index < uploadedDocuments.length) {
            const doc = uploadedDocuments[index];
            if (confirm(`Remove ${doc.fileName}?`)) {
                uploadedDocuments.splice(index, 1);
                displayUploadedDocuments();
                window.AppToast?.success('Document removed');
            }
        }
    }

    /**
     * Format file size
     * @param {number} bytes - File size in bytes
     * @returns {string} - Formatted file size
     */
    function formatFileSize(bytes) {
        if (bytes === 0) return '0 Bytes';
        const k = 1024;
        const sizes = ['Bytes', 'KB', 'MB', 'GB'];
        const i = Math.floor(Math.log(bytes) / Math.log(k));
        return Math.round(bytes / Math.pow(k, i) * 100) / 100 + ' ' + sizes[i];
    }

    /**
     * Escape HTML special characters before injecting text into markup.
     * @param {string} value - Text value to escape
     * @returns {string} - Escaped text
     */
    function escapeHtml(value) {
        return String(value)
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#39;');
    }

    /**
     * Handle form submit
     * @param {boolean} saveAsDraft - Whether to save as draft
     */
    async function handleFormSubmit(saveAsDraft) {
        isDraftMode = saveAsDraft;

        // For draft, skip validation
        if (!saveAsDraft && !validator.validate()) {
            window.AppToast?.warning('Please fix validation errors');
            return;
        }

        const formData = collectFormData();

        if (!formData) {
            return;
        }

        formData.isDraft = saveAsDraft;

        window.AppLoader?.show();

        try {
            let response;

            if (isEditMode) {
                response = await window.ApiClient.put(
                    window.ApplicationModule.config.apiEndpoints.update(currentApplicationId),
                    formData
                );
            } else {
                response = await window.ApiClient.post(
                    window.ApplicationModule.config.apiEndpoints.create,
                    formData
                );
            }

            if (response.success) {
                const action = saveAsDraft ? 'saved as draft' : (isEditMode ? 'updated' : 'created');
                window.AppToast?.success(response.message || `Application ${action} successfully`);
                closeForm();
                if (window.ApplicationModule.Summary && typeof window.ApplicationModule.Summary.refreshGrid === 'function') {
                    window.ApplicationModule.Summary.refreshGrid();
                }
            } else {
                window.AppToast?.error(response.message || `Failed to ${isEditMode ? 'update' : 'create'} application`);
            }
        } catch (error) {
            console.error('Error saving application:', error);
            window.AppToast?.error(error.message || 'Error saving application');
        } finally {
            window.AppLoader?.hide();
        }
    }

    /**
     * Collect form data
     * @returns {object|null} - Form data object or null if validation fails
     */
    function collectFormData() {
        const applicationId = parseInt($('#applicationId').val()) || 0;
        const applicantId = applicantDropdown?.value();
        const statusId = statusDropdown?.value();
        const countryId = countryDropdown?.value();
        const instituteId = instituteDropdown?.value();
        const courseId = courseDropdown?.value();
        const intakeMonthId = intakeMonthDropdown?.value();
        const applicationDate = $('#applicationDate').val() || null;
        const intakeYear = $('#intakeYear').val() || null;

        // Required field validation (only if not draft)
        if (!isDraftMode) {
            if (!applicantId || applicantId <= 0) {
                window.AppToast?.error('Please select an applicant');
                tabStrip.select(0);
                return null;
            }

            if (!applicationDate) {
                window.AppToast?.error('Application date is required');
                tabStrip.select(0);
                return null;
            }

            if (!countryId || countryId <= 0) {
                window.AppToast?.error('Please select a country');
                tabStrip.select(5); // Course Selection tab
                return null;
            }

            if (!instituteId || instituteId <= 0) {
                window.AppToast?.error('Please select an institute');
                tabStrip.select(5);
                return null;
            }

            if (!courseId || courseId <= 0) {
                window.AppToast?.error('Please select a course');
                tabStrip.select(5);
                return null;
            }
        }

        const formData = {
            applicationId: applicationId,
            applicantId: applicantId || null,
            statusId: statusId || null,
            applicationDate: applicationDate,
            emergencyContactName: $('#emergencyContactName').val()?.trim() || null,
            emergencyContactPhone: $('#emergencyContactPhone').val()?.trim() || null,
            emergencyContactRelation: $('#emergencyContactRelation').val()?.trim() || null,
            currentAddress: $('#currentAddress').val()?.trim() || null,
            permanentAddress: $('#permanentAddress').val()?.trim() || null,
            highestQualification: $('#highestQualification').val()?.trim() || null,
            lastInstitutionName: $('#lastInstitutionName').val()?.trim() || null,
            yearOfCompletion: $('#yearOfCompletion').val() || null,
            gpaOrPercentage: $('#gpaOrPercentage').val()?.trim() || null,
            fieldOfStudy: $('#fieldOfStudy').val()?.trim() || null,
            currentEmployer: $('#currentEmployer').val()?.trim() || null,
            currentJobTitle: $('#currentJobTitle').val()?.trim() || null,
            yearsOfExperience: $('#yearsOfExperience').val() || null,
            employmentStartDate: $('#employmentStartDate').val() || null,
            employmentEndDate: $('#employmentEndDate').val() || null,
            countryId: countryId || null,
            instituteId: instituteId || null,
            courseId: courseId || null,
            intakeMonthId: intakeMonthId || null,
            intakeYear: intakeYear,
            courseDuration: $('#courseDuration').val() || null,
            tuitionFee: $('#tuitionFee').val()?.trim() || null,
            applicationFee: $('#applicationFee').val()?.trim() || null,
            remarks: $('#remarks').val()?.trim() || null,
            documents: uploadedDocuments,
            isDraft: isDraftMode
        };

        return formData;
    }

})();
