/**
 * Access Control Details - Form Management
 */

(function () {
    'use strict';

    window.AccessControlModule = window.AccessControlModule || {};

    window.AccessControlModule.Details = {
        window: null,
        validator: null,
        isEditMode: false,
        currentId: 0,

        init: function () {
            this.initWindow();
            this.initForm();
        },

        initWindow: function () {
            const config = window.AccessControlModule.config;
            this.window = $('#accessControlFormWindow').kendoWindow(config.windowOptions).data('kendoWindow');
        },

        initForm: function () {
            // Validator
            this.validator = $('#accessControlForm').kendoValidator({
                rules: {
                    required: (input) => {
                        if (input.is('[required]')) {
                            return $.trim(input.val()) !== '';
                        }
                        return true;
                    }
                },
                messages: {
                    required: 'This field is required'
                }
            }).data('kendoValidator');

            // Event handlers
            $('#btnSave').on('click', () => this.save());
            $('#btnCancel').on('click', () => this.window.close());
        },

        openAddForm: function () {
            this.isEditMode = false;
            this.currentId = 0;
            this.clearForm();
            this.window.title('Add Access Control');
            this.window.center().open();
        },

        edit: async function (accessId) {
            this.isEditMode = true;
            this.currentId = accessId;

            try {
                const response = await window.ApiClient.get(
                    window.AccessControlModule.config.apiEndpoints.read(accessId)
                );

                if (response.success && response.data) {
                    const data = response.data;
                    $('#accessId').val(data.accessId);
                    $('#accessName').val(data.accessName);
                    $('#accessDescription').val(data.accessDescription || '');

                    this.window.title('Edit Access Control');
                    this.window.center().open();
                }
            } catch (error) {
                console.error('Load error:', error);
                window.AppToast?.error('Failed to load access control details');
            }
        },

        save: async function () {
            if (!this.validator.validate()) {
                window.AppToast?.warning('Please fill all required fields');
                return;
            }

            const formData = {
                accessId: parseInt($('#accessId').val()) || 0,
                accessName: $('#accessName').val(),
                accessDescription: $('#accessDescription').val() || null,
                isActive: true
            };

            try {
                const config = window.AccessControlModule.config;
                let response;

                if (this.isEditMode) {
                    response = await window.ApiClient.put(
                        config.apiEndpoints.update(formData.accessId),
                        formData
                    );
                } else {
                    response = await window.ApiClient.post(
                        config.apiEndpoints.create,
                        formData
                    );
                }

                if (response.success) {
                    window.AppToast?.success(response.message || 'Access control saved successfully');
                    this.window.close();
                    window.AccessControlModule.Summary?.refreshGrid();
                } else {
                    window.AppToast?.error(response.message || 'Failed to save access control');
                }
            } catch (error) {
                console.error('Save error:', error);
                window.AppToast?.error('Failed to save access control');
            }
        },

        delete: async function (accessId) {
            if (!confirm('Are you sure you want to delete this access control?')) {
                return;
            }

            try {
                const response = await window.ApiClient.delete(
                    window.AccessControlModule.config.apiEndpoints.delete(accessId)
                );

                if (response.success) {
                    window.AppToast?.success(response.message || 'Access control deleted successfully');
                    window.AccessControlModule.Summary?.refreshGrid();
                } else {
                    window.AppToast?.error(response.message || 'Failed to delete access control');
                }
            } catch (error) {
                console.error('Delete error:', error);
                window.AppToast?.error('Failed to delete access control');
            }
        },

        clearForm: function () {
            $('#accessControlForm')[0].reset();
            $('#accessId').val(0);
            this.validator?.hideMessages();
        }
    };

})();
