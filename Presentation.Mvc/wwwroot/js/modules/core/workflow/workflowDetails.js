/**
 * Workflow Details - Two forms: State form + Action form
 */

(function () {
    'use strict';

    window.WorkflowModule = window.WorkflowModule || {};

    window.WorkflowModule.Details = {
        stateWindow: null,
        actionWindow: null,
        stateValidator: null,
        actionValidator: null,
        menuDropdown: null,
        stateDropdown: null,
        nextStateDropdown: null,
        isEditMode: false,
        currentStateId: 0,
        currentActionId: 0,

        init: function () {
            this.initStateWindow();
            this.initActionWindow();
            this.initStateForm();
            this.initActionForm();
        },

        initStateWindow: function () {
            const config = window.WorkflowModule.config;
            this.stateWindow = $('#stateFormWindow').kendoWindow(config.stateWindowOptions).data('kendoWindow');
        },

        initActionWindow: function () {
            const config = window.WorkflowModule.config;
            this.actionWindow = $('#actionFormWindow').kendoWindow(config.actionWindowOptions).data('kendoWindow');
        },

        initStateForm: function () {
            // Menu dropdown
            this.menuDropdown = $('#menuId').kendoDropDownList({
                dataTextField: 'text',
                dataValueField: 'value',
                optionLabel: 'Select Menu...',
                dataSource: {
                    transport: {
                        read: async (options) => {
                            try {
                                const response = await window.ApiClient.get(
                                    window.WorkflowModule.config.apiEndpoints.menusDDL
                                );
                                options.success(response.data || []);
                            } catch (error) {
                                console.error('Menu dropdown error:', error);
                                options.error(error);
                            }
                        }
                    }
                }
            }).data('kendoDropDownList');

            // Validator
            this.stateValidator = $('#stateForm').kendoValidator({
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
            $('#btnSaveState').on('click', () => this.saveState());
            $('#btnCancelState').on('click', () => this.stateWindow.close());
        },

        initActionForm: function () {
            const config = window.WorkflowModule.config;

            // From State dropdown
            this.stateDropdown = $('#wfStateId').kendoDropDownList({
                dataTextField: 'text',
                dataValueField: 'value',
                optionLabel: 'Select From State...',
                dataSource: {
                    transport: {
                        read: async (options) => {
                            try {
                                const response = await window.ApiClient.get(config.apiEndpoints.statesDDL);
                                options.success(response.data || []);
                            } catch (error) {
                                console.error('State dropdown error:', error);
                                options.error(error);
                            }
                        }
                    }
                }
            }).data('kendoDropDownList');

            // Next State dropdown
            this.nextStateDropdown = $('#nextStateId').kendoDropDownList({
                dataTextField: 'text',
                dataValueField: 'value',
                optionLabel: 'Select Next State...',
                dataSource: {
                    transport: {
                        read: async (options) => {
                            try {
                                const response = await window.ApiClient.get(config.apiEndpoints.statesDDL);
                                options.success(response.data || []);
                            } catch (error) {
                                console.error('Next state dropdown error:', error);
                                options.error(error);
                            }
                        }
                    }
                }
            }).data('kendoDropDownList');

            // Validator
            this.actionValidator = $('#actionForm').kendoValidator({
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
            $('#btnSaveAction').on('click', () => this.saveAction());
            $('#btnCancelAction').on('click', () => this.actionWindow.close());
        },

        // ===== STATE METHODS =====

        openAddStateForm: function () {
            this.isEditMode = false;
            this.currentStateId = 0;
            this.clearStateForm();
            this.stateWindow.title('Add Workflow State');
            this.stateWindow.center().open();
            this.menuDropdown.dataSource.read();
        },

        editState: async function (stateId) {
            this.isEditMode = true;
            this.currentStateId = stateId;

            try {
                const response = await window.ApiClient.get(
                    window.WorkflowModule.config.apiEndpoints.stateRead(stateId)
                );

                if (response.success && response.data) {
                    const state = response.data;
                    $('#stateId').val(state.wfStateId);
                    $('#stateName').val(state.stateName);
                    $('#stateSequence').val(state.sequence);
                    $('#isDefaultStart').prop('checked', state.isDefaultStart);
                    $('#isClosed').prop('checked', state.isClosed);

                    await this.menuDropdown.dataSource.read();
                    this.menuDropdown.value(state.menuId);

                    this.stateWindow.title('Edit Workflow State');
                    this.stateWindow.center().open();
                }
            } catch (error) {
                console.error('Load state error:', error);
                window.AppToast?.error('Failed to load state details');
            }
        },

        saveState: async function () {
            if (!this.stateValidator.validate()) {
                window.AppToast?.warning('Please fill all required fields');
                return;
            }

            const formData = {
                wfStateId: parseInt($('#stateId').val()) || 0,
                stateName: $('#stateName').val(),
                menuId: this.menuDropdown.value(),
                sequence: parseInt($('#stateSequence').val()),
                isDefaultStart: $('#isDefaultStart').is(':checked'),
                isClosed: $('#isClosed').is(':checked'),
                isActive: true
            };

            try {
                const config = window.WorkflowModule.config;
                let response;

                if (this.isEditMode) {
                    response = await window.ApiClient.put(
                        config.apiEndpoints.stateUpdate(formData.wfStateId),
                        formData
                    );
                } else {
                    response = await window.ApiClient.post(
                        config.apiEndpoints.stateCreate,
                        formData
                    );
                }

                if (response.success) {
                    window.AppToast?.success(response.message || 'State saved successfully');
                    this.stateWindow.close();
                    window.WorkflowModule.Summary?.refreshStateGrid();
                } else {
                    window.AppToast?.error(response.message || 'Failed to save state');
                }
            } catch (error) {
                console.error('Save state error:', error);
                window.AppToast?.error('Failed to save state');
            }
        },

        deleteState: async function (stateId) {
            if (!confirm('Are you sure you want to delete this workflow state?')) {
                return;
            }

            try {
                const response = await window.ApiClient.delete(
                    window.WorkflowModule.config.apiEndpoints.stateDelete(stateId)
                );

                if (response.success) {
                    window.AppToast?.success(response.message || 'State deleted successfully');
                    window.WorkflowModule.Summary?.refreshStateGrid();
                } else {
                    window.AppToast?.error(response.message || 'Failed to delete state');
                }
            } catch (error) {
                console.error('Delete state error:', error);
                window.AppToast?.error('Failed to delete state');
            }
        },

        clearStateForm: function () {
            $('#stateForm')[0].reset();
            $('#stateId').val(0);
            this.menuDropdown?.select(0);
            this.stateValidator?.hideMessages();
        },

        // ===== ACTION METHODS =====

        openAddActionForm: function () {
            this.isEditMode = false;
            this.currentActionId = 0;
            this.clearActionForm();
            this.actionWindow.title('Add Workflow Action');
            this.actionWindow.center().open();
            this.stateDropdown.dataSource.read();
            this.nextStateDropdown.dataSource.read();
        },

        editAction: async function (actionId) {
            this.isEditMode = true;
            this.currentActionId = actionId;

            try {
                const response = await window.ApiClient.get(
                    window.WorkflowModule.config.apiEndpoints.actionRead(actionId)
                );

                if (response.success && response.data) {
                    const action = response.data;
                    $('#actionId').val(action.wfActionId);
                    $('#actionName').val(action.actionName);
                    $('#emailAlert').prop('checked', action.emailAlert);
                    $('#smsAlert').prop('checked', action.smsAlert);

                    await this.stateDropdown.dataSource.read();
                    await this.nextStateDropdown.dataSource.read();
                    this.stateDropdown.value(action.wfStateId);
                    this.nextStateDropdown.value(action.nextStateId);

                    this.actionWindow.title('Edit Workflow Action');
                    this.actionWindow.center().open();
                }
            } catch (error) {
                console.error('Load action error:', error);
                window.AppToast?.error('Failed to load action details');
            }
        },

        saveAction: async function () {
            if (!this.actionValidator.validate()) {
                window.AppToast?.warning('Please fill all required fields');
                return;
            }

            const formData = {
                wfActionId: parseInt($('#actionId').val()) || 0,
                wfStateId: this.stateDropdown.value(),
                actionName: $('#actionName').val(),
                nextStateId: this.nextStateDropdown.value(),
                emailAlert: $('#emailAlert').is(':checked'),
                smsAlert: $('#smsAlert').is(':checked'),
                isActive: true
            };

            try {
                const config = window.WorkflowModule.config;
                let response;

                if (this.isEditMode) {
                    response = await window.ApiClient.put(
                        config.apiEndpoints.actionUpdate(formData.wfActionId),
                        formData
                    );
                } else {
                    response = await window.ApiClient.post(
                        config.apiEndpoints.actionCreate,
                        formData
                    );
                }

                if (response.success) {
                    window.AppToast?.success(response.message || 'Action saved successfully');
                    this.actionWindow.close();
                    window.WorkflowModule.Summary?.refreshActionGrid();
                } else {
                    window.AppToast?.error(response.message || 'Failed to save action');
                }
            } catch (error) {
                console.error('Save action error:', error);
                window.AppToast?.error('Failed to save action');
            }
        },

        deleteAction: async function (actionId) {
            if (!confirm('Are you sure you want to delete this workflow action?')) {
                return;
            }

            try {
                const response = await window.ApiClient.delete(
                    window.WorkflowModule.config.apiEndpoints.actionDelete(actionId)
                );

                if (response.success) {
                    window.AppToast?.success(response.message || 'Action deleted successfully');
                    window.WorkflowModule.Summary?.refreshActionGrid();
                } else {
                    window.AppToast?.error(response.message || 'Failed to delete action');
                }
            } catch (error) {
                console.error('Delete action error:', error);
                window.AppToast?.error('Failed to delete action');
            }
        },

        clearActionForm: function () {
            $('#actionForm')[0].reset();
            $('#actionId').val(0);
            this.stateDropdown?.select(0);
            this.nextStateDropdown?.select(0);
            this.actionValidator?.hideMessages();
        }
    };

})();
