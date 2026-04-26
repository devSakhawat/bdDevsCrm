/**
 * Thana Details - Form CRUD Module
 * This file handles Create, Read, Update operations for Thana with District dependency
 */

(function () {
    'use strict';

    window.ThanaModule = window.ThanaModule || {};

    // Form window instance
    let formWindow = null;
    let formValidator = null;
    let isEditMode = false;

    // Dropdown instances
    let districtDropDown = null;
    let statusDropDown = null;

    window.ThanaModule.Details = {
        init: initializeForm,
        openAddForm: openAddForm,
        openEditForm: openEditForm,
        saveThana: saveThana,
        clearForm: clearForm
    };

    /**
     * Initialize form and window
     */
    function initializeForm() {
        // Initialize Kendo Window
        formWindow = $('#thanaFormWindow').kendoWindow({
            width: window.ThanaModule.config.windowOptions.width,
            title: window.ThanaModule.config.windowOptions.title,
            modal: window.ThanaModule.config.windowOptions.modal,
            visible: window.ThanaModule.config.windowOptions.visible,
            actions: window.ThanaModule.config.windowOptions.actions,
            close: onWindowClose
        }).data('kendoWindow');

        // Initialize District DropDown
        districtDropDown = $('#districtId').kendoDropDownList({
            dataTextField: 'districtName',
            dataValueField: 'districtId',
            optionLabel: '-- Select District --',
            dataSource: {
                transport: {
                    read: {
                        url: window.ThanaModule.config.apiEndpoints.districtsDDL,
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

        // Initialize Status DropDown
        statusDropDown = $('#status').kendoDropDownList({
            dataSource: [
                { text: 'Active', value: 1 },
                { text: 'Inactive', value: 0 }
            ],
            dataTextField: 'text',
            dataValueField: 'value',
            value: 1
        }).data('kendoDropDownList');

        // Initialize Kendo Validator
        formValidator = $('#thanaForm').kendoValidator({
            rules: {
                required: function (input) {
                    if (input.is('[required]')) {
                        return $.trim(input.val()) !== '';
                    }
                    return true;
                },
                thanaNameLength: function (input) {
                    if (input.is('[name="thanaName"]')) {
                        const val = $.trim(input.val());
                        return val.length >= 2 && val.length <= 100;
                    }
                    return true;
                }
            },
            messages: {
                required: 'This field is required',
                thanaNameLength: 'Thana name must be between 2 and 100 characters'
            }
        }).data('kendoValidator');

        // Attach form submit handler
        $('#thanaForm').on('submit', function (e) {
            e.preventDefault();
            saveThana();
        });

        // Attach cancel button handler
        $('#btnCancel').on('click', function () {
            formWindow.close();
        });

        console.log('Thana form initialized');
    }

    /**
     * Open form for adding new thana
     */
    function openAddForm() {
        isEditMode = false;
        clearForm();
        formWindow.title('Add New Thana');
        formWindow.center().open();

        // Set default status to Active
        if (statusDropDown) {
            statusDropDown.value(1);
        }

        $('#thanaName').focus();
    }

    /**
     * Open form for editing existing thana
     * @param {number} thanaId - Thana ID to edit
     */
    async function openEditForm(thanaId) {
        if (!thanaId || thanaId <= 0) {
            window.AppToast?.error('Invalid thana ID');
            return;
        }

        isEditMode = true;
        formWindow.title('Edit Thana');
        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.get(
                window.ThanaModule.config.apiEndpoints.read(thanaId)
            );

            if (response.success && response.data) {
                populateForm(response.data);
                formWindow.center().open();
                $('#thanaName').focus();
            } else {
                window.AppToast?.error(response.message || 'Failed to load thana details');
            }
        } catch (error) {
            console.error('Error loading thana:', error);
            window.AppToast?.error('Error loading thana details');
        } finally {
            window.AppLoader?.hide();
        }
    }

    /**
     * Save thana (Create or Update)
     */
    async function saveThana() {
        // Validate form
        if (!formValidator.validate()) {
            window.AppToast?.warning('Please fix validation errors');
            return;
        }

        const districtId = districtDropDown.value();

        if (!districtId) {
            window.AppToast?.warning('Please select a district');
            return;
        }

        const thanaId = parseInt($('#thanaId').val()) || 0;

        const formData = {
            thanaId: thanaId,
            districtId: parseInt(districtId),
            thanaName: $.trim($('#thanaName').val()),
            thanaNameBn: $.trim($('#thanaNameBn').val()) || null,
            thanaCode: $.trim($('#thanaCode').val()) || null,
            status: parseInt(statusDropDown.value())
        };

        window.AppLoader?.show();

        try {
            let response;

            if (isEditMode) {
                // Update existing thana
                response = await window.ApiClient.put(
                    window.ThanaModule.config.apiEndpoints.update(formData.thanaId),
                    formData
                );
            } else {
                // Create new thana
                response = await window.ApiClient.post(
                    window.ThanaModule.config.apiEndpoints.create,
                    formData
                );
            }

            if (response.success) {
                window.AppToast?.success(response.message || 'Thana saved successfully');
                formWindow.close();

                // Refresh grid
                if (window.ThanaModule.Summary && typeof window.ThanaModule.Summary.refreshGrid === 'function') {
                    window.ThanaModule.Summary.refreshGrid();
                }
            } else {
                window.AppToast?.error(response.message || 'Failed to save thana');
            }
        } catch (error) {
            console.error('Error saving thana:', error);
            window.AppToast?.error(error.message || 'Error saving thana');
        } finally {
            window.AppLoader?.hide();
        }
    }

    /**
     * Populate form with thana data
     * @param {object} data - Thana data
     */
    function populateForm(data) {
        $('#thanaId').val(data.thanaId || 0);
        $('#thanaName').val(data.thanaName || '');
        $('#thanaNameBn').val(data.thanaNameBn || '');
        $('#thanaCode').val(data.thanaCode || '');

        // Set dropdown values
        if (data.districtId) {
            districtDropDown.value(data.districtId);
        }
        if (data.status !== undefined) {
            statusDropDown.value(data.status);
        }
    }

    /**
     * Clear form fields
     */
    function clearForm() {
        $('#thanaId').val(0);
        $('#thanaName').val('');
        $('#thanaNameBn').val('');
        $('#thanaCode').val('');

        // Reset dropdowns
        if (districtDropDown) {
            districtDropDown.value('');
        }
        if (statusDropDown) {
            statusDropDown.value(1);
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

})();
