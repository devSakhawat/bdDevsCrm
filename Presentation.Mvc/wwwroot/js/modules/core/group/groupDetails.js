/**
 * Group Details - Form CRUD Module
 * This file handles Create, Read, Update operations for Group with Permissions
 */

(function () {
    'use strict';

    window.GroupModule = window.GroupModule || {};

    // Form window instance
    let formWindow = null;
    let formValidator = null;
    let isEditMode = false;

    // Dropdown instances
    let companyDropDown = null;

    // Permissions data
    let allAccessControls = [];
    let selectedPermissions = {
        modules: [],
        menus: [],
        access: []
    };

    window.GroupModule.Details = {
        init: initializeForm,
        openAddForm: openAddForm,
        openEditForm: openEditForm,
        saveGroup: saveGroup,
        clearForm: clearForm
    };

    /**
     * Initialize form and window
     */
    function initializeForm() {
        // Initialize Kendo Window
        formWindow = $('#groupFormWindow').kendoWindow({
            width: window.GroupModule.config.windowOptions.width,
            title: window.GroupModule.config.windowOptions.title,
            modal: window.GroupModule.config.windowOptions.modal,
            visible: window.GroupModule.config.windowOptions.visible,
            actions: window.GroupModule.config.windowOptions.actions,
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
                        url: window.GroupModule.config.apiEndpoints.companiesDDL,
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
        formValidator = $('#groupForm').kendoValidator({
            rules: {
                required: function (input) {
                    if (input.is('[required]')) {
                        return $.trim(input.val()) !== '';
                    }
                    return true;
                },
                groupNameLength: function (input) {
                    if (input.is('[name="groupName"]')) {
                        const val = $.trim(input.val());
                        return val.length >= 2 && val.length <= 100;
                    }
                    return true;
                }
            },
            messages: {
                required: 'This field is required',
                groupNameLength: 'Group name must be between 2 and 100 characters'
            }
        }).data('kendoValidator');

        // Attach form submit handler
        $('#groupForm').on('submit', function (e) {
            e.preventDefault();
            saveGroup();
        });

        // Attach cancel button handler
        $('#btnCancel').on('click', function () {
            formWindow.close();
        });

        // Load access controls (permissions data)
        loadAccessControls();

        console.log('Group form initialized');
    }

    /**
     * Load access controls for permissions
     */
    async function loadAccessControls() {
        try {
            const response = await window.ApiClient.get(
                window.GroupModule.config.apiEndpoints.accessControls
            );

            if (response.success && response.data) {
                allAccessControls = response.data;
                console.log('Access controls loaded:', allAccessControls.length);
            }
        } catch (error) {
            console.error('Error loading access controls:', error);
        }
    }

    /**
     * Open form for adding new group
     */
    function openAddForm() {
        isEditMode = false;
        clearForm();
        formWindow.title('Add New Group');
        formWindow.center().open();
        renderPermissions();
        $('#groupName').focus();
    }

    /**
     * Open form for editing existing group
     * @param {number} groupId - Group ID to edit
     */
    async function openEditForm(groupId) {
        if (!groupId || groupId <= 0) {
            window.AppToast?.error('Invalid group ID');
            return;
        }

        isEditMode = true;
        formWindow.title('Edit Group');
        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.get(
                window.GroupModule.config.apiEndpoints.read(groupId)
            );

            if (response.success && response.data) {
                await populateForm(response.data);
                formWindow.center().open();
                $('#groupName').focus();
            } else {
                window.AppToast?.error(response.message || 'Failed to load group details');
            }
        } catch (error) {
            console.error('Error loading group:', error);
            window.AppToast?.error('Error loading group details');
        } finally {
            window.AppLoader?.hide();
        }
    }

    /**
     * Save group (Create or Update)
     */
    async function saveGroup() {
        // Validate form
        if (!formValidator.validate()) {
            window.AppToast?.warning('Please fix validation errors');
            return;
        }

        const companyId = companyDropDown.value();
        if (!companyId) {
            window.AppToast?.warning('Please select a company');
            return;
        }

        // Collect selected permissions
        collectSelectedPermissions();

        const formData = {
            groupId: parseInt($('#groupId').val()) || 0,
            companyId: parseInt(companyId),
            groupName: $.trim($('#groupName').val()),
            isDefault: $('#isDefault').is(':checked') ? 1 : 0,
            moduleList: selectedPermissions.modules,
            menuList: selectedPermissions.menus,
            accessList: selectedPermissions.access
        };

        window.AppLoader?.show();

        try {
            let response;

            if (isEditMode) {
                // Update existing group
                response = await window.ApiClient.put(
                    window.GroupModule.config.apiEndpoints.update(formData.groupId),
                    formData
                );
            } else {
                // Create new group
                response = await window.ApiClient.post(
                    window.GroupModule.config.apiEndpoints.create,
                    formData
                );
            }

            if (response.success) {
                window.AppToast?.success(response.message || 'Group saved successfully');
                formWindow.close();

                // Refresh grid
                if (window.GroupModule.Summary && typeof window.GroupModule.Summary.refreshGrid === 'function') {
                    window.GroupModule.Summary.refreshGrid();
                }
            } else {
                window.AppToast?.error(response.message || 'Failed to save group');
            }
        } catch (error) {
            console.error('Error saving group:', error);
            window.AppToast?.error(error.message || 'Error saving group');
        } finally {
            window.AppLoader?.hide();
        }
    }

    /**
     * Populate form with group data
     * @param {object} data - Group data
     */
    async function populateForm(data) {
        $('#groupId').val(data.groupId || 0);
        $('#groupName').val(data.groupName || '');
        $('#isDefault').prop('checked', data.isDefault === 1);

        // Set company dropdown
        if (data.companyId) {
            companyDropDown.value(data.companyId);
        }

        // Load and render permissions with selected state
        await loadGroupPermissions(data.groupId);
        renderPermissions();
    }

    /**
     * Load group permissions
     */
    async function loadGroupPermissions(groupId) {
        try {
            const response = await window.ApiClient.get(
                window.GroupModule.config.apiEndpoints.groupPermissions(groupId)
            );

            if (response.success && response.data) {
                const permissions = response.data;
                // Store selected permission IDs
                selectedPermissions.modules = permissions.filter(p => p.type === 'module').map(p => ({ id: p.id, selected: true }));
                selectedPermissions.menus = permissions.filter(p => p.type === 'menu').map(p => ({ id: p.id, selected: true }));
                selectedPermissions.access = permissions.filter(p => p.type === 'access').map(p => ({ id: p.id, selected: true }));
            }
        } catch (error) {
            console.error('Error loading group permissions:', error);
        }
    }

    /**
     * Render permissions checkboxes
     */
    function renderPermissions() {
        // Simple permission rendering (can be enhanced with actual API data)
        const modules = allAccessControls.filter(ac => ac.type === 'module') || [];
        const menus = allAccessControls.filter(ac => ac.type === 'menu') || [];
        const access = allAccessControls.filter(ac => ac.type === 'access') || [];

        $('#modulePermissions').html(renderPermissionList(modules, 'module'));
        $('#menuPermissions').html(renderPermissionList(menus, 'menu'));
        $('#accessPermissions').html(renderPermissionList(access, 'access'));
    }

    /**
     * Render permission list HTML
     */
    function renderPermissionList(items, type) {
        if (!items || items.length === 0) {
            return '<p style="color: #999;">No permissions available</p>';
        }

        return items.map(item => {
            const isChecked = isPermissionSelected(item.id, type);
            return `
                <div class="permission-item">
                    <input type="checkbox" id="${type}_${item.id}"
                           data-type="${type}" data-id="${item.id}"
                           ${isChecked ? 'checked' : ''} />
                    <label for="${type}_${item.id}">${item.name}</label>
                </div>
            `;
        }).join('');
    }

    /**
     * Check if permission is selected
     */
    function isPermissionSelected(id, type) {
        const list = selectedPermissions[type + 's'] || [];
        return list.some(p => p.id === id);
    }

    /**
     * Collect selected permissions from checkboxes
     */
    function collectSelectedPermissions() {
        selectedPermissions = {
            modules: [],
            menus: [],
            access: []
        };

        $('input[type="checkbox"][data-type]').each(function () {
            if ($(this).is(':checked')) {
                const type = $(this).data('type');
                const id = $(this).data('id');
                selectedPermissions[type + 's'].push({ id: id, selected: true });
            }
        });
    }

    /**
     * Clear form fields
     */
    function clearForm() {
        $('#groupId').val(0);
        $('#groupName').val('');
        $('#isDefault').prop('checked', false);

        // Reset dropdown
        if (companyDropDown) {
            companyDropDown.value('');
        }

        // Clear permissions
        selectedPermissions = {
            modules: [],
            menus: [],
            access: []
        };

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
