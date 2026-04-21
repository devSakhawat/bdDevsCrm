(function () {
    'use strict';

    const factory = window.CrmSimpleCrudFactory = window.CrmSimpleCrudFactory || {};

    factory.getApiRoot = function () {
        const configured = window.AppConfig?.apiBaseUrl || '';
        try {
            const resolved = new URL(configured, window.location.origin);
            return `${resolved.origin}/bdDevs-crm`;
        } catch (_error) {
            return `${window.location.origin}/bdDevs-crm`;
        }
    };

    factory.createSummary = function (moduleRef) {
        let grid = null;

        function initializeGrid() {
            if (!ensureKendo()) {
                return;
            }

            const config = moduleRef.config;
            const dataSource = new kendo.data.DataSource({
                transport: {
                    read: function (options) {
                        window.AppLoader?.show('Loading records...');

                        window.ApiClient.post(config.apiEndpoints.summary, mapGridOptions(options.data))
                            .then(function (response) {
                                const items = response?.success ? (response.data?.items || []) : [];
                                const total = response?.success ? (response.data?.totalCount || response.data?.total || items.length || 0) : 0;
                                options.success({ data: items, total: total });

                                if (!response?.success) {
                                    window.AppToast?.error(response?.message || 'Failed to load records');
                                }
                            })
                            .catch(function (error) {
                                options.error(error);
                                window.AppToast?.error(error?.message || 'Failed to load records');
                            })
                            .finally(function () {
                                window.AppLoader?.hide();
                            });
                    }
                },
                schema: {
                    data: 'data',
                    total: 'total',
                    model: buildModel(config)
                },
                pageSize: config.gridOptions.pageSize,
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true
            });

            grid = $(config.dom.grid).kendoGrid({
                dataSource: dataSource,
                height: config.gridOptions.height || 560,
                sortable: config.gridOptions.sortable,
                filterable: config.gridOptions.filterable,
                pageable: config.gridOptions.pageable,
                resizable: true,
                reorderable: true,
                scrollable: true,
                noRecords: {
                    template: `<div class="grid-empty-state">No ${config.pluralTitle.toLowerCase()} found.</div>`
                },
                columns: buildColumns(config),
                dataBound: function () {
                    bindGridActions(config);
                }
            }).data('kendoGrid');
        }

        function refreshGrid() {
            if (grid) {
                grid.dataSource.read();
            }
        }

        function deleteRecord(id, name) {
            if (!id || id <= 0) {
                window.AppToast?.error(`Invalid ${moduleRef.config.moduleTitle} id`);
                return;
            }

            const targetName = name || moduleRef.config.moduleTitle;
            if (!window.confirm(`Are you sure you want to delete "${targetName}"?`)) {
                return;
            }

            window.AppLoader?.show('Deleting record...');
            window.ApiClient.delete(moduleRef.config.apiEndpoints.delete(id))
                .then(function (response) {
                    if (response?.success) {
                        window.AppToast?.success(response.message || 'Record deleted successfully');
                        refreshGrid();
                        return;
                    }

                    window.AppToast?.error(response?.message || 'Failed to delete record');
                })
                .catch(function (error) {
                    window.AppToast?.error(error?.message || 'Failed to delete record');
                })
                .finally(function () {
                    window.AppLoader?.hide();
                });
        }

        return {
            init: initializeGrid,
            refreshGrid: refreshGrid,
            deleteRecord: deleteRecord
        };
    };

    factory.createDetails = function (moduleRef) {
        let formWindow = null;
        let validator = null;
        let isEditMode = false;
        let currentRecord = null;

        function initializeForm() {
            if (!ensureKendo()) {
                return;
            }

            const config = moduleRef.config;
            formWindow = $(config.dom.window).kendoWindow({
                width: config.windowOptions.width,
                title: `${config.moduleTitle} Details`,
                modal: true,
                visible: false,
                actions: ['Close'],
                close: onWindowClose
            }).data('kendoWindow');

            $(config.dom.saveButton).off('click').on('click', saveRecord);
            $(config.dom.cancelButton).off('click').on('click', function () {
                formWindow.close();
            });

            renderForm();
        }

        function openAddForm() {
            if (!formWindow) {
                return;
            }

            isEditMode = false;
            currentRecord = null;
            renderForm();
            formWindow.title(`Add ${moduleRef.config.moduleTitle}`);
            formWindow.center().open();
        }

        async function openEditForm(id) {
            if (!id || id <= 0) {
                window.AppToast?.error(`Invalid ${moduleRef.config.moduleTitle} id`);
                return;
            }

            window.AppLoader?.show('Loading record...');

            try {
                const response = await window.ApiClient.get(moduleRef.config.apiEndpoints.read(id));
                const record = normalizeReadResponse(response?.data, id, moduleRef.config.idField);

                if (!response?.success || !record) {
                    window.AppToast?.error(response?.message || 'Failed to load record');
                    return;
                }

                isEditMode = true;
                currentRecord = record;
                renderForm(record);
                formWindow.title(`Edit ${moduleRef.config.moduleTitle}`);
                formWindow.center().open();
            } catch (error) {
                window.AppToast?.error(error?.message || 'Failed to load record');
            } finally {
                window.AppLoader?.hide();
            }
        }

        async function saveRecord() {
            const config = moduleRef.config;
            const values = collectFormValues(config.form.fields);
            const validationErrors = validateValues(config.form.fields, values);

            if (validationErrors.length) {
                window.AppToast?.warning(validationErrors[0]);
                return;
            }

            const payload = config.form.buildPayload(values, {
                isEditMode: isEditMode,
                currentRecord: currentRecord
            });

            window.AppLoader?.show(isEditMode ? 'Updating record...' : 'Creating record...');

            try {
                const response = isEditMode
                    ? await window.ApiClient.put(config.apiEndpoints.update(values[config.idField]), payload)
                    : await window.ApiClient.post(config.apiEndpoints.create, payload);

                if (response?.success) {
                    window.AppToast?.success(response.message || 'Record saved successfully');
                    formWindow.close();
                    moduleRef.Summary?.refreshGrid?.();
                    return;
                }

                window.AppToast?.error(response?.message || 'Failed to save record');
            } catch (error) {
                window.AppToast?.error(error?.message || 'Failed to save record');
            } finally {
                window.AppLoader?.hide();
            }
        }

        function onWindowClose() {
            isEditMode = false;
            currentRecord = null;
            renderForm();
        }

        function renderForm(data) {
            const config = moduleRef.config;
            const html = config.form.fields.map(function (field) {
                return renderField(field, data || {});
            }).join('');

            $(config.dom.form).html(html);
            initializeInputs(config.form.fields);
            validator = $(config.dom.form).kendoValidator().data('kendoValidator');
        }

        return {
            init: initializeForm,
            openAddForm: openAddForm,
            openEditForm: openEditForm,
            saveRecord: saveRecord
        };
    };

    function ensureKendo() {
        if (!window.kendo || !$.fn.kendoGrid || !$.fn.kendoWindow) {
            window.AppToast?.error('Kendo UI assets are required for this module.');
            return false;
        }

        return true;
    }

    function buildModel(config) {
        const fields = {};
        config.grid.columns.forEach(function (column) {
            fields[column.field] = { type: column.dataType || 'string' };
        });

        return {
            id: config.idField,
            fields: fields
        };
    }

    function buildColumns(config) {
        const columns = config.grid.columns.map(function (column) {
            const definition = {
                field: column.field,
                title: column.title,
                width: column.width,
                filterable: column.filterable !== false,
                sortable: column.sortable !== false
            };

            if (column.kind === 'boolean') {
                definition.template = function (dataItem) {
                    const value = !!dataItem[column.field];
                    return renderBadge(value, column.trueText || 'Active', column.falseText || 'Inactive');
                };
                definition.filterable = false;
            }

            if (column.kind === 'currency') {
                definition.template = function (dataItem) {
                    const value = dataItem[column.field];
                    return value === null || value === undefined || value === '' ? '-' : kendo.toString(value, 'n2');
                };
            }

            if (column.kind === 'multiline') {
                definition.template = function (dataItem) {
                    const value = dataItem[column.field];
                    return value || '-';
                };
            }

            return definition;
        });

        columns.push({
            title: 'Actions',
            width: 170,
            filterable: false,
            sortable: false,
            template: function (dataItem) {
                const id = dataItem[config.idField];
                const name = sanitizeForAttribute(dataItem[config.displayField] || config.moduleTitle);
                return `<div class="form-actions-inline">` +
                    `<button type="button" class="btn btn--table-secondary js-edit-record" data-id="${id}">Edit</button>` +
                    `<button type="button" class="btn btn--table-danger js-delete-record" data-id="${id}" data-name="${name}">Delete</button>` +
                    `</div>`;
            }
        });

        return columns;
    }

    function bindGridActions(config) {
        $(config.dom.grid).find('.js-edit-record').off('click').on('click', function () {
            const id = parseInt($(this).data('id'), 10);
            config.moduleRef.Details?.openEditForm?.(id);
        });

        $(config.dom.grid).find('.js-delete-record').off('click').on('click', function () {
            const id = parseInt($(this).data('id'), 10);
            const name = $(this).data('name');
            config.moduleRef.Summary?.deleteRecord?.(id, name);
        });
    }

    function mapGridOptions(options) {
        return {
            page: options?.page || 1,
            pageSize: options?.pageSize || 20,
            sort: options?.sort || [],
            filter: options?.filter || null
        };
    }

    function normalizeReadResponse(data, id, idField) {
        if (Array.isArray(data)) {
            return data.find(function (item) {
                return parseInt(item[idField], 10) === parseInt(id, 10);
            }) || null;
        }

        if (data && Array.isArray(data.items)) {
            return data.items.find(function (item) {
                return parseInt(item[idField], 10) === parseInt(id, 10);
            }) || null;
        }

        return data || null;
    }

    function renderField(field, data) {
        const value = data[field.name] ?? field.defaultValue ?? '';

        if (field.type === 'hidden') {
            return `<input type="hidden" id="${field.name}" name="${field.name}" value="${escapeHtml(value)}" />`;
        }

        if (field.type === 'checkbox') {
            return `<div class="form-group form-group--span-2">` +
                `<label><input type="checkbox" id="${field.name}" name="${field.name}" ${value ? 'checked' : ''} /> ${field.label}</label>` +
                `</div>`;
        }

        const groupClass = field.wide ? 'form-group form-group--span-2' : 'form-group';
        const required = field.required ? ' <span class="text-danger">*</span>' : '';
        const placeholder = field.placeholder ? ` placeholder="${escapeHtml(field.placeholder)}"` : '';
        const maxLength = field.maxLength ? ` maxlength="${field.maxLength}"` : '';
        const min = field.min !== undefined ? ` min="${field.min}"` : '';
        const step = field.step !== undefined ? ` step="${field.step}"` : '';

        if (field.type === 'textarea') {
            return `<div class="${groupClass}">` +
                `<label for="${field.name}">${field.label}${required}</label>` +
                `<textarea id="${field.name}" name="${field.name}" class="app-input app-input--textarea"${placeholder}${maxLength}>${escapeHtml(value)}</textarea>` +
                `</div>`;
        }

        return `<div class="${groupClass}">` +
            `<label for="${field.name}">${field.label}${required}</label>` +
            `<input type="${field.type}" id="${field.name}" name="${field.name}" class="app-input" value="${escapeHtml(value)}"${placeholder}${maxLength}${min}${step} />` +
            `</div>`;
    }

    function initializeInputs(fields) {
        fields.forEach(function (field) {
            const selector = `#${field.name}`;
            if (!document.querySelector(selector)) {
                return;
            }

            if (field.type === 'text' || field.type === 'email' || field.type === 'url' || field.type === 'hidden') {
                if ($.fn.kendoTextBox && field.type !== 'hidden') {
                    $(selector).kendoTextBox();
                }
                return;
            }

            if (field.type === 'number' && $.fn.kendoNumericTextBox) {
                $(selector).kendoNumericTextBox({
                    decimals: field.decimals ?? 0,
                    format: field.decimals ? `n${field.decimals}` : 'n0',
                    min: field.min,
                    step: field.step || 1
                });
                return;
            }

            if (field.type === 'date' && $.fn.kendoDatePicker) {
                $(selector).kendoDatePicker({
                    format: 'yyyy-MM-dd'
                });
                return;
            }

            if (field.type === 'checkbox' && $.fn.kendoCheckBox) {
                $(selector).kendoCheckBox();
            }
        });
    }

    function collectFormValues(fields) {
        const values = {};

        fields.forEach(function (field) {
            const selector = `#${field.name}`;
            if (field.type === 'checkbox') {
                values[field.name] = $(selector).is(':checked');
                return;
            }

            if (field.type === 'number') {
                const widget = $(selector).data('kendoNumericTextBox');
                const value = widget ? widget.value() : $(selector).val();
                values[field.name] = value === '' || value === null || value === undefined ? null : Number(value);
                return;
            }

            if (field.type === 'date') {
                const widget = $(selector).data('kendoDatePicker');
                const value = widget ? widget.value() : $(selector).val();
                values[field.name] = value instanceof Date ? value.toISOString() : value;
                return;
            }

            values[field.name] = $(selector).val();
        });

        return values;
    }

    function validateValues(fields, values) {
        const errors = [];

        fields.forEach(function (field) {
            const value = values[field.name];

            if (field.required) {
                const isEmpty = value === null || value === undefined || value === '';
                if (isEmpty) {
                    errors.push(`${field.label} is required.`);
                    return;
                }
            }

            if (field.type === 'number' && value !== null && value !== undefined && value !== '') {
                if (Number.isNaN(Number(value))) {
                    errors.push(`${field.label} must be numeric.`);
                    return;
                }

                if (field.min !== undefined && Number(value) < field.min) {
                    errors.push(`${field.label} must be greater than or equal to ${field.min}.`);
                    return;
                }
            }

            if (field.maxLength && value && String(value).length > field.maxLength) {
                errors.push(`${field.label} cannot exceed ${field.maxLength} characters.`);
            }
        });

        return errors;
    }

    function renderBadge(isPositive, positiveText, negativeText) {
        const badgeClass = isPositive ? 'badge badge--success' : 'badge badge--warning';
        const label = isPositive ? positiveText : negativeText;
        return `<span class="${badgeClass}">${label}</span>`;
    }

    function sanitizeForAttribute(value) {
        return String(value)
            .replace(/&/g, '&amp;')
            .replace(/"/g, '&quot;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;');
    }

    function escapeHtml(value) {
        return String(value ?? '')
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;');
    }
})();
