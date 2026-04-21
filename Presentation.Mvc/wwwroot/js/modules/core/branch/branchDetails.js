/**
 * Branch Details - Form Module
 * This file handles form operations (create, update, read) for Branch
 */

(function () {
    'use strict';

    window.BranchModule = window.BranchModule || {};

    // Window and validator instances
    let detailsWindow = null;
    let validator = null;
    let companyDropdown = null;
    let isEditMode = false;

    window.BranchModule.Details = {
        init: initializeForm,
        openAddForm: openAddForm,
        openEditForm: openEditForm
    };

    /**
     * Initialize form window and validator
     */
    function initializeForm() {
        // Initialize Kendo Window
        detailsWindow = $('#branchWindow').kendoWindow({
            width: window.BranchModule.config.windowOptions.width,
            title: window.BranchModule.config.windowOptions.title,
            visible: window.BranchModule.config.windowOptions.visible,
            modal: window.BranchModule.config.windowOptions.modal,
            actions: window.BranchModule.config.windowOptions.actions,
            close: onWindowClose
        }).data('kendoWindow');

        // Initialize Company Dropdown
        companyDropdown = $('#companyId').kendoDropDownList({
            dataTextField: 'companyName',
            dataValueField: 'companyId',
            optionLabel: '-- Select Company --',
            dataSource: {
                transport: {
                    read: {
                        url: window.BranchModule.config.apiEndpoints.companiesDDL,
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
        validator = $('#branchForm').kendoValidator({
            rules: {
                companyRequired: function (input) {
                    if (input.is('[name="companyId"]')) {
                        const companyId = parseInt(input.val());
                        return companyId > 0;
                    }
                    return true;
                },
                branchNameRequired: function (input) {
                    if (input.is('[name="branchName"]')) {
                        return $.trim(input.val()).length > 0;
                    }
                    return true;
                }
            },
            messages: {
                companyRequired: 'Company is required',
                branchNameRequired: 'Branch name is required'
            }
        }).data('kendoValidator');

        // Attach form button handlers
        $('#btnSaveBranch').on('click', saveBranch);
        $('#btnCancelBranch').on('click', function () {
            detailsWindow.close();
        });

        console.log('Branch form initialized');
    }

    /**
     * Open form for adding new branch
     */
    function openAddForm() {
        isEditMode = false;
        resetForm();
        detailsWindow.title('Add New Branch');
        detailsWindow.center().open();
    }

    /**
     * Open form for editing existing branch
     * @param {number} branchId - Branch ID to edit
     */
    async function openEditForm(branchId) {
        if (!branchId || branchId <= 0) {
            window.AppToast?.error('Invalid branch ID');
            return;
        }

        isEditMode = true;
        resetForm();
        detailsWindow.title('Edit Branch');
        detailsWindow.center().open();

        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.get(
                window.BranchModule.config.apiEndpoints.read(branchId)
            );

            if (response.success && response.data) {
                populateForm(response.data);
            } else {
                window.AppToast?.error(response.message || 'Failed to load branch details');
                detailsWindow.close();
            }
        } catch (error) {
            console.error('Error loading branch:', error);
            window.AppToast?.error(error.message || 'Error loading branch details');
            detailsWindow.close();
        } finally {
            window.AppLoader?.hide();
        }
    }

    /**
     * Populate form with branch data
     * @param {object} data - Branch data
     */
    function populateForm(data) {
        $('#branchId').val(data.branchId || 0);
        $('#branchName').val(data.branchName || '');
        $('#branchCode').val(data.branchCode || '');
        $('#branchDescription').val(data.branchDescription || '');
        $('#branchAddress').val(data.branchAddress || '');
        $('#isCostCentre').prop('checked', data.isCostCentre === 1);
        $('#isActive').prop('checked', data.isActive === 1);

        // Set company dropdown value
        if (data.companyId && companyDropdown) {
            companyDropdown.value(data.companyId);
        }
    }

    /**
     * Reset form to default state
     */
    function resetForm() {
        $('#branchForm')[0].reset();
        $('#branchId').val(0);
        $('#isActive').prop('checked', true);
        $('#isCostCentre').prop('checked', false);

        if (companyDropdown) {
            companyDropdown.value('');
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
     * Save branch (create or update)
     */
    async function saveBranch() {
        // Validate form
        if (!validator.validate()) {
            window.AppToast?.warning('Please correct the validation errors');
            return;
        }

        // Collect form data
        const branchId = parseInt($('#branchId').val()) || 0;
        const companyId = companyDropdown ? parseInt(companyDropdown.value()) : 0;
        const branchName = $.trim($('#branchName').val());
        const branchCode = $.trim($('#branchCode').val()) || null;
        const branchDescription = $.trim($('#branchDescription').val()) || null;
        const branchAddress = $.trim($('#branchAddress').val()) || null;
        const isCostCentre = $('#isCostCentre').is(':checked') ? 1 : 0;
        const isActive = $('#isActive').is(':checked') ? 1 : 0;

        // Basic validation
        if (companyId <= 0) {
            window.AppToast?.warning('Please select a company');
            return;
        }

        if (!branchName) {
            window.AppToast?.warning('Branch name is required');
            return;
        }

        const formData = {
            branchId: branchId,
            companyId: companyId,
            branchName: branchName,
            branchCode: branchCode,
            branchDescription: branchDescription,
            branchAddress: branchAddress,
            isCostCentre: isCostCentre,
            isActive: isActive
        };

        window.AppLoader?.show();

        try {
            let response;
            if (isEditMode && branchId > 0) {
                // Update existing branch
                response = await window.ApiClient.put(
                    window.BranchModule.config.apiEndpoints.update(branchId),
                    formData
                );
            } else {
                // Create new branch
                response = await window.ApiClient.post(
                    window.BranchModule.config.apiEndpoints.create,
                    formData
                );
            }

            if (response.success) {
                window.AppToast?.success(response.message || `Branch ${isEditMode ? 'updated' : 'created'} successfully`);
                detailsWindow.close();
                window.BranchModule.Summary.refreshGrid();
            } else {
                window.AppToast?.error(response.message || `Failed to ${isEditMode ? 'update' : 'create'} branch`);
            }
        } catch (error) {
            console.error(`Error ${isEditMode ? 'updating' : 'creating'} branch:`, error);
            window.AppToast?.error(error.message || `Error ${isEditMode ? 'updating' : 'creating'} branch`);
        } finally {
            window.AppLoader?.hide();
        }
    }

})();
