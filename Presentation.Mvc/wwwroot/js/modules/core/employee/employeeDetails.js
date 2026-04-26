/**
 * Employee Details - Form CRUD Module
 * This file handles form operations (create, read, update) for Employees
 * Features: 12-tab form, cascading dropdowns, salary calculations, document upload
 */

(function () {
    'use strict';

    window.EmployeeModule = window.EmployeeModule || {};

    // Form state
    let formWindow = null;
    let validator = null;
    let tabStrip = null;
    let isEditMode = false;
    let currentEmployeeId = null;
    let uploadedDocuments = [];

    // Dropdown instances
    let companyDropdown = null;
    let branchDropdown = null;
    let departmentDropdown = null;
    let designationDropdown = null;
    let shiftDropdown = null;
    let reportingManagerDropdown = null;
    let gradeDropdown = null;
    let locationDropdown = null;
    let genderDropdown = null;
    let maritalStatusDropdown = null;
    let religionDropdown = null;
    let bloodGroupDropdown = null;

    window.EmployeeModule.Details = {
        init: initializeFormComponents,
        openAddForm: openAddForm,
        openEditForm: openEditForm
    };

    /**
     * Initialize form components
     */
    function initializeFormComponents() {
        // Initialize Kendo Window
        formWindow = $('#employeeFormWindow').kendoWindow(
            window.EmployeeModule.config.windowOptions
        ).data('kendoWindow');

        // Initialize TabStrip
        tabStrip = $('#employeeTabStrip').kendoTabStrip({
            animation: {
                open: { effects: 'fadeIn' }
            }
        }).data('kendoTabStrip');

        // Initialize static dropdowns (Gender, Marital Status, etc.)
        initializeStaticDropdowns();

        // Initialize cascading dropdowns (Company -> Branch -> Department)
        initializeCascadingDropdowns();

        // Initialize other dropdowns
        initializeOtherDropdowns();

        // Initialize form validator
        initializeValidator();

        // Attach event handlers
        attachEventHandlers();

        console.log('Employee form components initialized');
    }

    /**
     * Initialize static dropdowns
     */
    function initializeStaticDropdowns() {
        genderDropdown = $('#gender').kendoDropDownList({
            dataTextField: 'text',
            dataValueField: 'value',
            dataSource: window.EmployeeModule.config.dropdownData.gender
        }).data('kendoDropDownList');

        maritalStatusDropdown = $('#maritalStatus').kendoDropDownList({
            dataTextField: 'text',
            dataValueField: 'value',
            dataSource: window.EmployeeModule.config.dropdownData.maritalStatus
        }).data('kendoDropDownList');

        religionDropdown = $('#religion').kendoDropDownList({
            dataTextField: 'text',
            dataValueField: 'value',
            dataSource: window.EmployeeModule.config.dropdownData.religion
        }).data('kendoDropDownList');

        bloodGroupDropdown = $('#bloodGroup').kendoDropDownList({
            dataTextField: 'text',
            dataValueField: 'value',
            dataSource: window.EmployeeModule.config.dropdownData.bloodGroup
        }).data('kendoDropDownList');
    }

    /**
     * Initialize cascading dropdowns
     */
    function initializeCascadingDropdowns() {
        // Company DropDownList (parent)
        companyDropdown = $('#companyId').kendoDropDownList({
            dataTextField: 'companyName',
            dataValueField: 'companyId',
            optionLabel: 'Select Company...',
            dataSource: {
                transport: {
                    read: {
                        url: window.EmployeeModule.config.apiEndpoints.companiesDDL,
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
            change: onCompanyChange
        }).data('kendoDropDownList');

        // Branch DropDownList (cascading child of Company)
        branchDropdown = $('#branchId').kendoDropDownList({
            dataTextField: 'branchName',
            dataValueField: 'branchId',
            optionLabel: 'Select Branch...',
            autoBind: false,
            enable: false,
            change: onBranchChange
        }).data('kendoDropDownList');

        // Department DropDownList (cascading child of Branch)
        departmentDropdown = $('#departmentId').kendoDropDownList({
            dataTextField: 'departmentName',
            dataValueField: 'departmentId',
            optionLabel: 'Select Department...',
            autoBind: false,
            enable: false
        }).data('kendoDropDownList');
    }

    /**
     * Initialize other dropdowns
     */
    function initializeOtherDropdowns() {
        // Designation DropDownList
        designationDropdown = $('#designationId').kendoDropDownList({
            dataTextField: 'designationName',
            dataValueField: 'designationId',
            optionLabel: 'Select Designation...',
            dataSource: {
                transport: {
                    read: {
                        url: window.EmployeeModule.config.apiEndpoints.designationsDDL,
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

        // Shift DropDownList
        shiftDropdown = $('#shiftId').kendoDropDownList({
            dataTextField: 'shiftName',
            dataValueField: 'shiftId',
            optionLabel: 'Select Shift...',
            dataSource: {
                transport: {
                    read: {
                        url: window.EmployeeModule.config.apiEndpoints.shiftsDDL,
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

        // Reporting Manager DropDownList
        reportingManagerDropdown = $('#reportingManagerId').kendoDropDownList({
            dataTextField: 'employeeName',
            dataValueField: 'employeeId',
            optionLabel: 'Select Manager...',
            dataSource: {
                transport: {
                    read: {
                        url: window.EmployeeModule.config.apiEndpoints.employeesDDL,
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

        // Grade DropDownList
        gradeDropdown = $('#gradeId').kendoDropDownList({
            dataTextField: 'gradeName',
            dataValueField: 'gradeId',
            optionLabel: 'Select Grade...',
            dataSource: {
                transport: {
                    read: {
                        url: window.EmployeeModule.config.apiEndpoints.gradesDDL,
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

        // Location DropDownList
        locationDropdown = $('#locationId').kendoDropDownList({
            dataTextField: 'locationName',
            dataValueField: 'locationId',
            optionLabel: 'Select Location...',
            dataSource: {
                transport: {
                    read: {
                        url: window.EmployeeModule.config.apiEndpoints.locationsDDL,
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
    }

    /**
     * Initialize form validator
     */
    function initializeValidator() {
        validator = $('#employeeForm').kendoValidator({
            rules: {
                emailValid: function (input) {
                    if (input.is('[type="email"]') && input.val()) {
                        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
                        return emailRegex.test(input.val());
                    }
                    return true;
                },
                dateValid: function (input) {
                    if (input.is('[type="date"]') && input.val()) {
                        const date = new Date(input.val());
                        return !isNaN(date.getTime());
                    }
                    return true;
                },
                joinDateValid: function (input) {
                    if (input.is('[name="joinDate"]') && input.val()) {
                        const joinDate = new Date(input.val());
                        const today = new Date();
                        return joinDate <= today;
                    }
                    return true;
                }
            },
            messages: {
                emailValid: 'Please enter a valid email address',
                dateValid: 'Please enter a valid date',
                joinDateValid: 'Join date cannot be in the future'
            }
        }).data('kendoValidator');
    }

    /**
     * Attach event handlers
     */
    function attachEventHandlers() {
        $('#btnSaveEmployee').off('click').on('click', saveEmployee);
        $('#btnCancelEmployee').off('click').on('click', closeForm);

        // Salary auto-calculation
        $('#basicSalary, #houseRent, #medicalAllowance, #transportAllowance, #otherAllowances')
            .on('input change', calculateGrossSalary);

        // Document upload
        $('#documentUpload').on('change', handleDocumentUpload);
    }

    /**
     * Handle Company dropdown change (cascading)
     */
    function onCompanyChange() {
        const companyId = companyDropdown.value();

        // Reset Branch and Department dropdowns
        branchDropdown.value('');
        branchDropdown.enable(false);
        departmentDropdown.value('');
        departmentDropdown.enable(false);

        if (!companyId || companyId <= 0) {
            return;
        }

        // Load branches for selected company
        branchDropdown.setDataSource({
            transport: {
                read: {
                    url: window.EmployeeModule.config.apiEndpoints.branchesByCompany(companyId),
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

        branchDropdown.enable(true);
        branchDropdown.dataSource.read();
    }

    /**
     * Handle Branch dropdown change (cascading)
     */
    function onBranchChange() {
        const branchId = branchDropdown.value();

        // Reset Department dropdown
        departmentDropdown.value('');
        departmentDropdown.enable(false);

        if (!branchId || branchId <= 0) {
            return;
        }

        // Load departments for selected branch
        departmentDropdown.setDataSource({
            transport: {
                read: {
                    url: window.EmployeeModule.config.apiEndpoints.departmentsByBranch(branchId),
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

        departmentDropdown.enable(true);
        departmentDropdown.dataSource.read();
    }

    /**
     * Calculate gross salary
     */
    function calculateGrossSalary() {
        const basicSalary = parseFloat($('#basicSalary').val()) || 0;
        const houseRent = parseFloat($('#houseRent').val()) || 0;
        const medicalAllowance = parseFloat($('#medicalAllowance').val()) || 0;
        const transportAllowance = parseFloat($('#transportAllowance').val()) || 0;
        const otherAllowances = parseFloat($('#otherAllowances').val()) || 0;

        const grossSalary = basicSalary + houseRent + medicalAllowance + transportAllowance + otherAllowances;
        $('#grossSalary').val(grossSalary.toFixed(2));
    }

    /**
     * Handle document upload
     */
    async function handleDocumentUpload(e) {
        const files = e.target.files;
        if (!files || files.length === 0) return;

        window.AppLoader?.show();

        try {
            for (let i = 0; i < files.length; i++) {
                const file = files[i];

                // Validate file size (max 10MB)
                if (file.size > 10 * 1024 * 1024) {
                    window.AppToast?.error(`File ${file.name} is too large. Maximum size is 10MB`);
                    continue;
                }

                uploadedDocuments.push({
                    fileName: file.name,
                    fileSize: formatFileSize(file.size),
                    uploadDate: new Date().toLocaleDateString('en-GB')
                });
            }

            displayUploadedDocuments();
            window.AppToast?.success(`${files.length} file(s) uploaded successfully`);

        } catch (error) {
            console.error('Error uploading documents:', error);
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

        let html = '<table class="table table-sm" style="width: 100%; border-collapse: collapse;">';
        html += '<thead><tr style="background: #f5f5f5;"><th style="padding: 8px; border: 1px solid #ddd;">File Name</th><th style="padding: 8px; border: 1px solid #ddd;">Size</th><th style="padding: 8px; border: 1px solid #ddd;">Upload Date</th><th style="padding: 8px; border: 1px solid #ddd;">Action</th></tr></thead>';
        html += '<tbody>';

        uploadedDocuments.forEach((doc, index) => {
            html += '<tr>';
            html += `<td style="padding: 8px; border: 1px solid #ddd;">${doc.fileName}</td>`;
            html += `<td style="padding: 8px; border: 1px solid #ddd;">${doc.fileSize}</td>`;
            html += `<td style="padding: 8px; border: 1px solid #ddd;">${doc.uploadDate}</td>`;
            html += `<td style="padding: 8px; border: 1px solid #ddd;"><button type="button" class="btn btn-sm btn-danger btn-remove-document" data-index="${index}">Remove</button></td>`;
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
     * Open form in Add mode
     */
    function openAddForm() {
        isEditMode = false;
        currentEmployeeId = null;
        uploadedDocuments = [];
        resetForm();
        formWindow.title('Add New Employee');
        formWindow.center().open();
        tabStrip.select(0); // Select first tab
    }

    /**
     * Open form in Edit mode
     * @param {number} employeeId - Employee ID to edit
     */
    async function openEditForm(employeeId) {
        if (!employeeId || employeeId <= 0) {
            window.AppToast?.error('Invalid employee ID');
            return;
        }

        isEditMode = true;
        currentEmployeeId = employeeId;
        uploadedDocuments = [];
        resetForm();
        formWindow.title('Edit Employee');
        formWindow.center().open();
        tabStrip.select(0);

        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.get(
                window.EmployeeModule.config.apiEndpoints.read(employeeId)
            );

            if (response.success && response.data) {
                await populateForm(response.data);
            } else {
                window.AppToast?.error(response.message || 'Failed to load employee data');
                formWindow.close();
            }
        } catch (error) {
            console.error('Error loading employee:', error);
            window.AppToast?.error(error.message || 'Error loading employee data');
            formWindow.close();
        } finally {
            window.AppLoader?.hide();
        }
    }

    /**
     * Populate form with employee data
     * @param {object} data - Employee data
     */
    async function populateForm(data) {
        // Tab 1: Basic Information
        $('#employeeId').val(data.employeeId || 0);
        $('#employeeCode').val(data.employeeCode || '');
        $('#employeeType').val(data.employeeType || '');
        $('#firstName').val(data.firstName || '');
        $('#middleName').val(data.middleName || '');
        $('#lastName').val(data.lastName || '');
        $('#email').val(data.email || '');
        $('#mobileNumber').val(data.mobileNumber || '');
        $('#joinDate').val(data.joinDate ? new Date(data.joinDate).toISOString().split('T')[0] : '');
        $('#confirmationDate').val(data.confirmationDate ? new Date(data.confirmationDate).toISOString().split('T')[0] : '');

        // Tab 2: Personal Details
        $('#dateOfBirth').val(data.dateOfBirth ? new Date(data.dateOfBirth).toISOString().split('T')[0] : '');
        genderDropdown?.value(data.gender || '');
        maritalStatusDropdown?.value(data.maritalStatus || '');
        $('#nationality').val(data.nationality || '');
        religionDropdown?.value(data.religion || '');
        bloodGroupDropdown?.value(data.bloodGroup || '');
        $('#nidNumber').val(data.nidNumber || '');
        $('#passportNumber').val(data.passportNumber || '');
        $('#tinNumber').val(data.tinNumber || '');
        $('#drivingLicenseNumber').val(data.drivingLicenseNumber || '');

        // Tab 3: Contact Information
        $('#presentAddress').val(data.presentAddress || '');
        $('#permanentAddress').val(data.permanentAddress || '');
        $('#personalEmail').val(data.personalEmail || '');
        $('#alternatePhoneNumber').val(data.alternatePhoneNumber || '');

        // Tab 4: Employment Details
        $('#probationPeriodMonths').val(data.probationPeriodMonths || '');
        $('#noticePeriodDays').val(data.noticePeriodDays || '');
        $('#contractStartDate').val(data.contractStartDate ? new Date(data.contractStartDate).toISOString().split('T')[0] : '');
        $('#contractEndDate').val(data.contractEndDate ? new Date(data.contractEndDate).toISOString().split('T')[0] : '');
        $('#employmentStatus').val(data.employmentStatus || '');
        $('#resignationDate').val(data.resignationDate ? new Date(data.resignationDate).toISOString().split('T')[0] : '');
        $('#employmentRemarks').val(data.employmentRemarks || '');

        // Tab 5: Department & Designation (with cascading)
        if (data.companyId) {
            companyDropdown?.value(data.companyId);

            // Wait for branches to load
            await waitForDataLoad(() => branchDropdown.dataSource.data().length > 0);

            if (data.branchId) {
                branchDropdown?.value(data.branchId);

                // Wait for departments to load
                await waitForDataLoad(() => departmentDropdown.dataSource.data().length > 0);

                if (data.departmentId) {
                    departmentDropdown?.value(data.departmentId);
                }
            }
        }

        designationDropdown?.value(data.designationId || null);
        shiftDropdown?.value(data.shiftId || null);
        reportingManagerDropdown?.value(data.reportingManagerId || null);
        gradeDropdown?.value(data.gradeId || null);
        locationDropdown?.value(data.locationId || null);

        // Tab 6: Salary Information
        $('#basicSalary').val(data.basicSalary || '');
        $('#houseRent').val(data.houseRent || '');
        $('#medicalAllowance').val(data.medicalAllowance || '');
        $('#transportAllowance').val(data.transportAllowance || '');
        $('#otherAllowances').val(data.otherAllowances || '');
        $('#grossSalary').val(data.grossSalary || '');
        $('#paymentMode').val(data.paymentMode || '');
        $('#salaryEffectiveDate').val(data.salaryEffectiveDate ? new Date(data.salaryEffectiveDate).toISOString().split('T')[0] : '');

        // Tab 7: Bank Details
        $('#bankName').val(data.bankName || '');
        $('#branchName').val(data.branchName || '');
        $('#accountNumber').val(data.accountNumber || '');
        $('#accountHolderName').val(data.accountHolderName || '');
        $('#ifscCode').val(data.ifscCode || '');
        $('#accountType').val(data.accountType || '');

        // Tab 8: Emergency Contact
        $('#emergencyContactName').val(data.emergencyContactName || '');
        $('#emergencyContactRelation').val(data.emergencyContactRelation || '');
        $('#emergencyContactPhone').val(data.emergencyContactPhone || '');
        $('#emergencyContactEmail').val(data.emergencyContactEmail || '');
        $('#emergencyContactAddress').val(data.emergencyContactAddress || '');

        // Tab 9: Education
        $('#highestQualification').val(data.highestQualification || '');
        $('#institutionName').val(data.institutionName || '');
        $('#fieldOfStudy').val(data.fieldOfStudy || '');
        $('#yearOfPassing').val(data.yearOfPassing || '');
        $('#gpaOrPercentage').val(data.gpaOrPercentage || '');
        $('#certificationsCourses').val(data.certificationsCourses || '');
        $('#educationRemarks').val(data.educationRemarks || '');

        // Tab 10: Experience
        $('#totalExperienceYears').val(data.totalExperienceYears || '');
        $('#previousEmployer').val(data.previousEmployer || '');
        $('#previousDesignation').val(data.previousDesignation || '');
        $('#previousSalary').val(data.previousSalary || '');
        $('#previousEmploymentStartDate').val(data.previousEmploymentStartDate ? new Date(data.previousEmploymentStartDate).toISOString().split('T')[0] : '');
        $('#previousEmploymentEndDate').val(data.previousEmploymentEndDate ? new Date(data.previousEmploymentEndDate).toISOString().split('T')[0] : '');
        $('#experienceRemarks').val(data.experienceRemarks || '');

        // Tab 11: Documents
        if (data.documents && Array.isArray(data.documents)) {
            uploadedDocuments = data.documents;
            displayUploadedDocuments();
        }
        $('#documentRemarks').val(data.documentRemarks || '');

        // Tab 12: Additional Info
        $('#employeePhoto').val(data.employeePhoto || '');
        $('#employeeSignature').val(data.employeeSignature || '');
        $('#isPfApplicable').prop('checked', data.isPfApplicable === 1 || data.isPfApplicable === true);
        $('#isEsiApplicable').prop('checked', data.isEsiApplicable === 1 || data.isEsiApplicable === true);
        $('#pfNumber').val(data.pfNumber || '');
        $('#esiNumber').val(data.esiNumber || '');
        $('#isActive').prop('checked', data.isActive === 1 || data.isActive === true);
        $('#remarks').val(data.remarks || '');
    }

    /**
     * Wait for data to load with timeout
     * @param {function} condition - Condition to check
     * @param {number} timeout - Timeout in milliseconds
     */
    function waitForDataLoad(condition, timeout = 3000) {
        return new Promise(resolve => {
            const checkInterval = setInterval(() => {
                if (condition()) {
                    clearInterval(checkInterval);
                    resolve();
                }
            }, 100);

            setTimeout(() => {
                clearInterval(checkInterval);
                resolve();
            }, timeout);
        });
    }

    /**
     * Reset form to initial state
     */
    function resetForm() {
        $('#employeeForm')[0].reset();
        $('#employeeId').val(0);

        // Reset all dropdowns
        companyDropdown?.value('');
        branchDropdown?.value('');
        branchDropdown?.enable(false);
        departmentDropdown?.value('');
        departmentDropdown?.enable(false);
        designationDropdown?.value('');
        shiftDropdown?.value('');
        reportingManagerDropdown?.value('');
        gradeDropdown?.value('');
        locationDropdown?.value('');
        genderDropdown?.value('');
        maritalStatusDropdown?.value('');
        religionDropdown?.value('');
        bloodGroupDropdown?.value('');

        // Reset checkboxes
        $('#isActive').prop('checked', true);
        $('#isPfApplicable').prop('checked', false);
        $('#isEsiApplicable').prop('checked', false);

        validator?.hideMessages();
        uploadedDocuments = [];
        displayUploadedDocuments();
    }

    /**
     * Close form window
     */
    function closeForm() {
        formWindow.close();
        resetForm();
    }

    /**
     * Save employee (create or update)
     */
    async function saveEmployee() {
        // Validate form
        if (!validator.validate()) {
            window.AppToast?.warning('Please correct the validation errors');
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
                    window.EmployeeModule.config.apiEndpoints.update(currentEmployeeId),
                    formData
                );
            } else {
                response = await window.ApiClient.post(
                    window.EmployeeModule.config.apiEndpoints.create,
                    formData
                );
            }

            if (response.success) {
                window.AppToast?.success(response.message || `Employee ${isEditMode ? 'updated' : 'created'} successfully`);
                closeForm();
                if (window.EmployeeModule.Summary && typeof window.EmployeeModule.Summary.refreshGrid === 'function') {
                    window.EmployeeModule.Summary.refreshGrid();
                }
            } else {
                window.AppToast?.error(response.message || `Failed to ${isEditMode ? 'update' : 'create'} employee`);
            }
        } catch (error) {
            console.error('Error saving employee:', error);
            window.AppToast?.error(error.message || 'Error saving employee');
        } finally {
            window.AppLoader?.hide();
        }
    }

    /**
     * Collect form data
     * @returns {object|null} - Form data object or null if validation fails
     */
    function collectFormData() {
        const employeeId = parseInt($('#employeeId').val()) || 0;

        // Basic validation
        const employeeCode = $('#employeeCode').val()?.trim();
        const firstName = $('#firstName').val()?.trim();
        const lastName = $('#lastName').val()?.trim();
        const email = $('#email').val()?.trim();
        const mobileNumber = $('#mobileNumber').val()?.trim();
        const joinDate = $('#joinDate').val();
        const companyId = companyDropdown?.value();
        const branchId = branchDropdown?.value();
        const departmentId = departmentDropdown?.value();
        const designationId = designationDropdown?.value();

        if (!employeeCode) {
            window.AppToast?.error('Employee code is required');
            tabStrip.select(0);
            return null;
        }

        if (!firstName || !lastName) {
            window.AppToast?.error('First name and last name are required');
            tabStrip.select(0);
            return null;
        }

        if (!email) {
            window.AppToast?.error('Official email is required');
            tabStrip.select(0);
            return null;
        }

        if (!mobileNumber) {
            window.AppToast?.error('Mobile number is required');
            tabStrip.select(0);
            return null;
        }

        if (!joinDate) {
            window.AppToast?.error('Join date is required');
            tabStrip.select(0);
            return null;
        }

        if (!companyId || companyId <= 0) {
            window.AppToast?.error('Please select a company');
            tabStrip.select(4);
            return null;
        }

        if (!branchId || branchId <= 0) {
            window.AppToast?.error('Please select a branch');
            tabStrip.select(4);
            return null;
        }

        if (!departmentId || departmentId <= 0) {
            window.AppToast?.error('Please select a department');
            tabStrip.select(4);
            return null;
        }

        if (!designationId || designationId <= 0) {
            window.AppToast?.error('Please select a designation');
            tabStrip.select(4);
            return null;
        }

        const formData = {
            employeeId: employeeId,
            employeeCode: employeeCode,
            employeeType: $('#employeeType').val() || null,
            firstName: firstName,
            middleName: $('#middleName').val()?.trim() || null,
            lastName: lastName,
            email: email,
            mobileNumber: mobileNumber,
            joinDate: joinDate,
            confirmationDate: $('#confirmationDate').val() || null,
            dateOfBirth: $('#dateOfBirth').val() || null,
            gender: genderDropdown?.value() || null,
            maritalStatus: maritalStatusDropdown?.value() || null,
            nationality: $('#nationality').val()?.trim() || null,
            religion: religionDropdown?.value() || null,
            bloodGroup: bloodGroupDropdown?.value() || null,
            nidNumber: $('#nidNumber').val()?.trim() || null,
            passportNumber: $('#passportNumber').val()?.trim() || null,
            tinNumber: $('#tinNumber').val()?.trim() || null,
            drivingLicenseNumber: $('#drivingLicenseNumber').val()?.trim() || null,
            presentAddress: $('#presentAddress').val()?.trim() || null,
            permanentAddress: $('#permanentAddress').val()?.trim() || null,
            personalEmail: $('#personalEmail').val()?.trim() || null,
            alternatePhoneNumber: $('#alternatePhoneNumber').val()?.trim() || null,
            probationPeriodMonths: $('#probationPeriodMonths').val() || null,
            noticePeriodDays: $('#noticePeriodDays').val() || null,
            contractStartDate: $('#contractStartDate').val() || null,
            contractEndDate: $('#contractEndDate').val() || null,
            employmentStatus: $('#employmentStatus').val() || null,
            resignationDate: $('#resignationDate').val() || null,
            employmentRemarks: $('#employmentRemarks').val()?.trim() || null,
            companyId: companyId,
            branchId: branchId,
            departmentId: departmentId,
            designationId: designationId,
            shiftId: shiftDropdown?.value() || null,
            reportingManagerId: reportingManagerDropdown?.value() || null,
            gradeId: gradeDropdown?.value() || null,
            locationId: locationDropdown?.value() || null,
            basicSalary: $('#basicSalary').val() || null,
            houseRent: $('#houseRent').val() || null,
            medicalAllowance: $('#medicalAllowance').val() || null,
            transportAllowance: $('#transportAllowance').val() || null,
            otherAllowances: $('#otherAllowances').val() || null,
            grossSalary: $('#grossSalary').val() || null,
            paymentMode: $('#paymentMode').val() || null,
            salaryEffectiveDate: $('#salaryEffectiveDate').val() || null,
            bankName: $('#bankName').val()?.trim() || null,
            branchName: $('#branchName').val()?.trim() || null,
            accountNumber: $('#accountNumber').val()?.trim() || null,
            accountHolderName: $('#accountHolderName').val()?.trim() || null,
            ifscCode: $('#ifscCode').val()?.trim() || null,
            accountType: $('#accountType').val() || null,
            emergencyContactName: $('#emergencyContactName').val()?.trim() || null,
            emergencyContactRelation: $('#emergencyContactRelation').val()?.trim() || null,
            emergencyContactPhone: $('#emergencyContactPhone').val()?.trim() || null,
            emergencyContactEmail: $('#emergencyContactEmail').val()?.trim() || null,
            emergencyContactAddress: $('#emergencyContactAddress').val()?.trim() || null,
            highestQualification: $('#highestQualification').val()?.trim() || null,
            institutionName: $('#institutionName').val()?.trim() || null,
            fieldOfStudy: $('#fieldOfStudy').val()?.trim() || null,
            yearOfPassing: $('#yearOfPassing').val() || null,
            gpaOrPercentage: $('#gpaOrPercentage').val()?.trim() || null,
            certificationsCourses: $('#certificationsCourses').val()?.trim() || null,
            educationRemarks: $('#educationRemarks').val()?.trim() || null,
            totalExperienceYears: $('#totalExperienceYears').val() || null,
            previousEmployer: $('#previousEmployer').val()?.trim() || null,
            previousDesignation: $('#previousDesignation').val()?.trim() || null,
            previousSalary: $('#previousSalary').val() || null,
            previousEmploymentStartDate: $('#previousEmploymentStartDate').val() || null,
            previousEmploymentEndDate: $('#previousEmploymentEndDate').val() || null,
            experienceRemarks: $('#experienceRemarks').val()?.trim() || null,
            documents: uploadedDocuments,
            documentRemarks: $('#documentRemarks').val()?.trim() || null,
            employeePhoto: $('#employeePhoto').val()?.trim() || null,
            employeeSignature: $('#employeeSignature').val()?.trim() || null,
            isPfApplicable: $('#isPfApplicable').is(':checked') ? 1 : 0,
            isEsiApplicable: $('#isEsiApplicable').is(':checked') ? 1 : 0,
            pfNumber: $('#pfNumber').val()?.trim() || null,
            esiNumber: $('#esiNumber').val()?.trim() || null,
            isActive: $('#isActive').is(':checked') ? 1 : 0,
            remarks: $('#remarks').val()?.trim() || null
        };

        return formData;
    }

})();
