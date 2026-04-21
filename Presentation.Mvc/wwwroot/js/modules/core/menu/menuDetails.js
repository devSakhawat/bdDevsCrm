/**
 * Menu Details - Form CRUD Module
 * This file handles Create, Read, Update operations for Menu
 */

(function () {
    'use strict';

    window.MenuModule = window.MenuModule || {};

    // Form window instance
    let formWindow = null;
    let formValidator = null;
    let isEditMode = false;

    // Dropdown instances
    let moduleDropDown = null;
    let parentMenuDropDown = null;
    let menuTypeDropDown = null;

    window.MenuModule.Details = {
        init: initializeForm,
        openAddForm: openAddForm,
        openEditForm: openEditForm,
        saveMenu: saveMenu,
        clearForm: clearForm
    };

    /**
     * Initialize form and window
     */
    function initializeForm() {
        // Initialize Kendo Window
        formWindow = $('#menuFormWindow').kendoWindow({
            width: window.MenuModule.config.windowOptions.width,
            title: window.MenuModule.config.windowOptions.title,
            modal: window.MenuModule.config.windowOptions.modal,
            visible: window.MenuModule.config.windowOptions.visible,
            actions: window.MenuModule.config.windowOptions.actions,
            close: onWindowClose
        }).data('kendoWindow');

        // Initialize Kendo DropDowns
        moduleDropDown = $('#moduleId').kendoDropDownList({
            dataTextField: 'moduleName',
            dataValueField: 'moduleId',
            optionLabel: '-- Select Module --',
            dataSource: {
                transport: {
                    read: {
                        url: window.MenuModule.config.apiEndpoints.modulesDDL,
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
            change: onModuleChange
        }).data('kendoDropDownList');

        parentMenuDropDown = $('#parentMenu').kendoDropDownList({
            dataTextField: 'menuName',
            dataValueField: 'menuId',
            optionLabel: '-- Root Menu (No Parent) --',
            dataSource: {
                transport: {
                    read: {
                        url: window.MenuModule.config.apiEndpoints.menusDDL,
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

        menuTypeDropDown = $('#menuType').kendoDropDownList({
            dataSource: [
                { text: 'Standard', value: 0 },
                { text: 'Module', value: 1 },
                { text: 'Group', value: 2 }
            ],
            dataTextField: 'text',
            dataValueField: 'value'
        }).data('kendoDropDownList');

        // Initialize Kendo Validator
        formValidator = $('#menuForm').kendoValidator({
            rules: {
                required: function (input) {
                    if (input.is('[required]')) {
                        return $.trim(input.val()) !== '';
                    }
                    return true;
                },
                menuNameLength: function (input) {
                    if (input.is('[name="menuName"]')) {
                        const val = $.trim(input.val());
                        return val.length >= 2 && val.length <= 100;
                    }
                    return true;
                }
            },
            messages: {
                required: 'This field is required',
                menuNameLength: 'Menu name must be between 2 and 100 characters'
            }
        }).data('kendoValidator');

        // Attach form submit handler
        $('#menuForm').on('submit', function (e) {
            e.preventDefault();
            saveMenu();
        });

        // Attach cancel button handler
        $('#btnCancel').on('click', function () {
            formWindow.close();
        });

        console.log('Menu form initialized');
    }

    /**
     * Open form for adding new menu
     */
    function openAddForm() {
        isEditMode = false;
        clearForm();
        formWindow.title('Add New Menu');
        formWindow.center().open();
        $('#menuName').focus();
    }

    /**
     * Open form for editing existing menu
     * @param {number} menuId - Menu ID to edit
     */
    async function openEditForm(menuId) {
        if (!menuId || menuId <= 0) {
            window.AppToast?.error('Invalid menu ID');
            return;
        }

        isEditMode = true;
        formWindow.title('Edit Menu');
        window.AppLoader?.show();

        try {
            const response = await window.ApiClient.get(
                window.MenuModule.config.apiEndpoints.read(menuId)
            );

            if (response.success && response.data) {
                populateForm(response.data);
                formWindow.center().open();
                $('#menuName').focus();
            } else {
                window.AppToast?.error(response.message || 'Failed to load menu details');
            }
        } catch (error) {
            console.error('Error loading menu:', error);
            window.AppToast?.error('Error loading menu details');
        } finally {
            window.AppLoader?.hide();
        }
    }

    /**
     * Save menu (Create or Update)
     */
    async function saveMenu() {
        // Validate form
        if (!formValidator.validate()) {
            window.AppToast?.warning('Please fix validation errors');
            return;
        }

        const menuId = parseInt($('#menuId').val()) || 0;
        const moduleId = moduleDropDown.value();
        const parentMenuId = parentMenuDropDown.value();

        if (!moduleId) {
            window.AppToast?.warning('Please select a module');
            return;
        }

        const formData = {
            menuId: menuId,
            menuName: $.trim($('#menuName').val()),
            moduleId: parseInt(moduleId),
            parentMenu: parentMenuId ? parseInt(parentMenuId) : null,
            menuPath: $.trim($('#menuPath').val()) || null,
            menuCode: $.trim($('#menuCode').val()) || null,
            sortOrder: parseInt($('#sortOrder').val()) || 0,
            menuType: parseInt(menuTypeDropDown.value()) || 0,
            isQuickLink: $('#isQuickLink').is(':checked') ? 1 : 0,
            isActive: $('#isActive').is(':checked') ? 1 : 0
        };

        window.AppLoader?.show();

        try {
            let response;

            if (isEditMode) {
                // Update existing menu
                response = await window.ApiClient.put(
                    window.MenuModule.config.apiEndpoints.update(formData.menuId),
                    formData
                );
            } else {
                // Create new menu
                response = await window.ApiClient.post(
                    window.MenuModule.config.apiEndpoints.create,
                    formData
                );
            }

            if (response.success) {
                window.AppToast?.success(response.message || 'Menu saved successfully');
                formWindow.close();

                // Refresh grid
                if (window.MenuModule.Summary && typeof window.MenuModule.Summary.refreshGrid === 'function') {
                    window.MenuModule.Summary.refreshGrid();
                }
            } else {
                window.AppToast?.error(response.message || 'Failed to save menu');
            }
        } catch (error) {
            console.error('Error saving menu:', error);
            window.AppToast?.error(error.message || 'Error saving menu');
        } finally {
            window.AppLoader?.hide();
        }
    }

    /**
     * Populate form with menu data
     * @param {object} data - Menu data
     */
    function populateForm(data) {
        $('#menuId').val(data.menuId || 0);
        $('#menuName').val(data.menuName || '');
        $('#menuPath').val(data.menuPath || '');
        $('#menuCode').val(data.menuCode || '');
        $('#sortOrder').val(data.sortOrder || 0);
        $('#isQuickLink').prop('checked', data.isQuickLink === 1);
        $('#isActive').prop('checked', data.isActive === 1);

        // Set dropdown values
        if (data.moduleId) {
            moduleDropDown.value(data.moduleId);
        }
        if (data.parentMenu) {
            parentMenuDropDown.value(data.parentMenu);
        }
        if (data.menuType !== undefined) {
            menuTypeDropDown.value(data.menuType);
        }
    }

    /**
     * Clear form fields
     */
    function clearForm() {
        $('#menuId').val(0);
        $('#menuName').val('');
        $('#menuPath').val('');
        $('#menuCode').val('');
        $('#sortOrder').val(0);
        $('#isQuickLink').prop('checked', false);
        $('#isActive').prop('checked', true);

        // Reset dropdowns
        if (moduleDropDown) {
            moduleDropDown.value('');
        }
        if (parentMenuDropDown) {
            parentMenuDropDown.value('');
        }
        if (menuTypeDropDown) {
            menuTypeDropDown.value(0);
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
     * Handle module dropdown change
     */
    function onModuleChange() {
        // Could filter parent menus by module if needed
        console.log('Module changed:', moduleDropDown.value());
    }

})();
