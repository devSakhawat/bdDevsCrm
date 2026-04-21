/**
 * Users Details - Form CRUD Module
 * This file handles Create, Read, Update operations for Users
 */

(function () {
    'use strict';

    window.UsersModule = window.UsersModule || {};

    // Form window instance
    let formWindow = null;
    let formValidator = null;
    let isEditMode = false;

    // Dropdown instances
    let companyDropDown = null;
    let employeeDropDown = null;
    let groupDropDown = null;

    window.UsersModule.Details = {
        init: initializeForm,
        openAddForm: openAddForm,
        openEditForm: openEditForm,
        saveUser: saveUser,
        clearForm: clearForm
    };

    /**
     * Initialize form and window
     */
    function initializeForm() {
        // Initialize Kendo Window
        formWindow = $('#userFormWindow').kendoWindow({
            width: window.UsersModule.config.windowOptions.width,
            title: window.UsersModule.config.windowOptions.title,
            modal: window.UsersModule.config.windowOptions.modal,
            visible: window.UsersModule.config.windowOptions.visible,
            actions: window.UsersModule.config.windowOptions.actions,
            close: onWindowClose
        }).data('kendoWindow');

        // Initialize Company DropDown
        companyDropDown = $('#companyId').kendoDropDownList({
            dataTextField: 'companyName',
            dataValueField: 'companyId',
            optionLabel: '-- Select Company --',
            dataSource: {
                transport: {
                    read: {
                        url: window.UsersModule.config.apiEndpoints.companiesDDL,
                        type: 'GET',
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

        // Initialize Employee DropDown
        employeeDropDown = $('#employeeId').kendoDropDownList({
            dataTextField: 'employeeName',
            dataValueField: 'employeeId',
            optionLabel: '-- Select Employee (Optional) --',
            dataSource: {
                transport: {
                    read: {
                        url: window.UsersModule.config.apiEndpoints.employeesDDL,
                        type: 'GET',
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

        // Initialize Group DropDown
        groupDropDown = $('#groupId').kendoDropDownList({
            dataTextField: 'groupName',
            dataValueField: 'groupId',
            optionLabel: '-- Select Group --',
            dataSource: {
                transport: {
                    read: {
                        url: window.UsersModule.config.apiEndpoints.groupsDDL,
                        type: 'GET',
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
        formValidator = $('#userForm').kendoValidator({
            rules: {
                required: function (input) {
                    if (input.is('[required]')) {
                        return $.trim(input.val()) !== '';
                    }
                    return true;
                },
                loginIdLength: function (input) {
                    if (input.is('[name="loginId"]')) {
                        const val = $.trim(input.val());
                        return val.length >= 3 && val.length <= 50;
                    }
                    return true;
                },
                email: function (input) {
                    if (input.is('[type="email"]') && input.val()) {
                        return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(input.val());
                    }
                    return true;
                }
            },
            messages: {
                required: 'This field is required',
                loginIdLength: 'Login ID must be between 3 and 50 characters',
                email: 'Please enter a valid email address'
            }
        }).data('kendoValidator');

        // Attach form submit handler
        $('#userForm').on('submit', function (e) {
            e.preventDefault();
            saveUser();
        });

        // Attach cancel button handler
        $('#btnCancel').on('click', function () {
            formWindow.close();
        });

        console.log('Users form initialized');
    }

    /**
     * Open form for adding new user
     */
    function openAddForm() {
        isEditMode = false;
        clearForm();
        formWindow.title('Add New User');
        formWindow.center().open();
        $('#loginId').focus();
    }

    /**
     * Open form for editing existing user
     * @param {number} userId - User ID to edit
     */
    async function openEditForm(userId) {
        if (!userId || userId <= 0) {
            window.AppToast?.error('Invalid user ID');
            return;
        }

        isEditMode = true;
        formWindow.title('Edit User');
        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.get(
                window.UsersModule.config.apiEndpoints.read(userId)
            );

            if (response.success && response.data) {
                populateForm(response.data);
                formWindow.center().open();
                $('#loginId').focus();
            } else {
                window.AppToast?.error(response.message || 'Failed to load user details');
            }
        } catch (error) {
            console.error('Error loading user:', error);
            window.AppToast?.error('Error loading user details');
        } finally {
            window.AppLoader?.hide();
        }
    }

    /**
     * Save user (Create or Update)
     */
    async function saveUser() {
        // Validate form
        if (!formValidator.validate()) {
            window.AppToast?.warning('Please fix validation errors');
            return;
        }

        const companyId = companyDropDown.value();
        const groupId = groupDropDown.value();

        if (!companyId) {
            window.AppToast?.warning('Please select a company');
            return;
        }

        if (!groupId) {
            window.AppToast?.warning('Please select a user group');
            return;
        }

        const employeeId = employeeDropDown.value();
        const password = $('#password').val();

        // Password validation for new users
        if (!isEditMode && !password) {
            window.AppToast?.warning('Password is required for new users');
            return;
        }

        const formData = {
            userId: parseInt($('#userId').val()) || 0,
            companyId: parseInt(companyId),
            employeeId: employeeId ? parseInt(employeeId) : null,
            loginId: $.trim($('#loginId').val()),
            userName: $.trim($('#userName').val()),
            emailAddress: $.trim($('#emailAddress').val()) || null,
            password: password || null,
            groupId: parseInt(groupId),
            isActive: $('#isActive').is(':checked')
        };

        window.AppLoader?.show();

        try {
            let response;

            if (isEditMode) {
                // Update existing user
                response = await window.ApiClient.put(
                    window.UsersModule.config.apiEndpoints.update(formData.userId),
                    formData
                );
            } else {
                // Create new user
                response = await window.ApiClient.post(
                    window.UsersModule.config.apiEndpoints.create,
                    formData
                );
            }

            if (response.success) {
                window.AppToast?.success(response.message || 'User saved successfully');
                formWindow.close();

                // Refresh grid
                if (window.UsersModule.Summary && typeof window.UsersModule.Summary.refreshGrid === 'function') {
                    window.UsersModule.Summary.refreshGrid();
                }
            } else {
                window.AppToast?.error(response.message || 'Failed to save user');
            }
        } catch (error) {
            console.error('Error saving user:', error);
            window.AppToast?.error(error.message || 'Error saving user');
        } finally {
            window.AppLoader?.hide();
        }
    }

    /**
     * Populate form with user data
     * @param {object} data - User data
     */
    function populateForm(data) {
        $('#userId').val(data.userId || 0);
        $('#loginId').val(data.loginId || '');
        $('#userName').val(data.userName || '');
        $('#emailAddress').val(data.emailAddress || '');
        $('#isActive').prop('checked', data.isActive === true);

        // Password is not populated for security reasons
        $('#password').val('');

        // Set dropdown values
        if (data.companyId) {
            companyDropDown.value(data.companyId);
        }
        if (data.employeeId) {
            employeeDropDown.value(data.employeeId);
        }
        if (data.groupId) {
            groupDropDown.value(data.groupId);
        }
    }

    /**
     * Clear form fields
     */
    function clearForm() {
        $('#userId').val(0);
        $('#loginId').val('');
        $('#userName').val('');
        $('#emailAddress').val('');
        $('#password').val('');
        $('#isActive').prop('checked', true);

        // Reset dropdowns
        if (companyDropDown) {
            companyDropDown.value('');
        }
        if (employeeDropDown) {
            employeeDropDown.value('');
        }
        if (groupDropDown) {
            groupDropDown.value('');
        }

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

    /**
     * Handle company dropdown change
     */
    function onCompanyChange() {
        // Could filter employees by company if needed
        console.log('Company changed:', companyDropDown.value());
    }

})();
