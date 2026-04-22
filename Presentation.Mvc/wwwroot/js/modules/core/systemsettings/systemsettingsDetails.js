(function () {
    'use strict';

    window.SystemSettingsModule = window.SystemSettingsModule || {};

    function init() {
        render();
    }

    function render() {
        const html = window.SystemSettingsModule.config.sections.map(function (section) {
            const fieldsHtml = section.fields.map(renderField).join('');
            return `<section class="info-card"><div class="info-card__header"><h2 class="info-card__title">${section.title}</h2></div><div class="form-layout form-layout--grid">${fieldsHtml}</div></section>`;
        }).join('');
        $(window.SystemSettingsModule.config.dom.form).html(html);
        initializeWidgets();
    }

    async function loadByCompany(companyId) {
        if (!companyId || companyId <= 0) {
            return;
        }
        window.AppLoader?.show('Loading company settings...');
        try {
            const response = await window.ApiClient.get(window.SystemSettingsModule.config.apiEndpoints.byCompany(companyId));
            if (response?.success && response.data) {
                populate(response.data);
                $(window.SystemSettingsModule.config.dom.status).val(`Loaded settings for company #${companyId}`);
                return;
            }
            window.AppToast?.warning(response?.message || 'No settings found for the selected company');
        } catch (error) {
            window.AppToast?.error(error?.message || 'Failed to load company settings');
        } finally {
            window.AppLoader?.hide();
        }
    }

    function populate(data) {
        window.SystemSettingsModule.state.currentSettings = data;
        const companyDropDown = $(window.SystemSettingsModule.config.dom.companySelect).data('kendoDropDownList');
        if (companyDropDown && data.companyId) {
            companyDropDown.value(String(data.companyId));
        }
        window.SystemSettingsModule.config.sections.forEach(function (section) {
            section.fields.forEach(function (field) {
                const selector = `#${field.name}`;
                if (!document.querySelector(selector)) {
                    return;
                }
                const value = data[field.name];
                if (field.type === 'checkbox') {
                    $(selector).prop('checked', field.valueKind === 'bool' ? !!value : Number(value) === 1);
                    return;
                }
                const numeric = $(selector).data('kendoNumericTextBox');
                if (numeric) {
                    numeric.value(value ?? null);
                    return;
                }
                const datePicker = $(selector).data('kendoDatePicker');
                if (datePicker) {
                    datePicker.value(value ? new Date(value) : null);
                    return;
                }
                $(selector).val(value ?? '');
            });
        });
    }

    async function save() {
        const companyId = parseInt($(window.SystemSettingsModule.config.dom.companySelect).data('kendoDropDownList')?.value() || '0', 10);
        const payload = { companyId: companyId };
        window.SystemSettingsModule.config.sections.forEach(function (section) {
            section.fields.forEach(function (field) {
                if (field.type === 'hidden' || field.type === 'text') {
                    payload[field.name] = normalizeString($(`#${field.name}`).val());
                } else if (field.type === 'checkbox') {
                    payload[field.name] = field.valueKind === 'bool' ? $(`#${field.name}`).is(':checked') : ($(`#${field.name}`).is(':checked') ? 1 : 0);
                } else if (field.type === 'number') {
                    const widget = $(`#${field.name}`).data('kendoNumericTextBox');
                    const value = widget ? widget.value() : $(`#${field.name}`).val();
                    payload[field.name] = value === null || value === '' ? 0 : Number(value);
                } else if (field.type === 'date') {
                    const widget = $(`#${field.name}`).data('kendoDatePicker');
                    const value = widget ? widget.value() : $(`#${field.name}`).val();
                    payload[field.name] = value instanceof Date ? value.toISOString() : (value || null);
                }
            });
        });

        if (!payload.settingsId) {
            payload.settingsId = window.SystemSettingsModule.state.currentSettings?.settingsId || 0;
        }
        if (!payload.companyId) {
            payload.companyId = window.SystemSettingsModule.state.currentSettings?.companyId || 0;
        }

        window.AppLoader?.show('Saving settings...');
        try {
            const response = await window.ApiClient.put(window.SystemSettingsModule.config.apiEndpoints.update, payload);
            if (response?.success && response.data) {
                populate(response.data);
                $(window.SystemSettingsModule.config.dom.status).val('Settings saved successfully');
                window.AppToast?.success(response.message || 'System settings saved successfully');
                return;
            }
            window.AppToast?.error(response?.message || 'Failed to save system settings');
        } catch (error) {
            window.AppToast?.error(error?.message || 'Failed to save system settings');
        } finally {
            window.AppLoader?.hide();
        }
    }

    function renderField(field) {
        if (field.type === 'hidden') {
            return `<input type="hidden" id="${field.name}" name="${field.name}" />`;
        }
        if (field.type === 'checkbox') {
            return `<div class="form-group form-group--span-2"><label><input type="checkbox" id="${field.name}" name="${field.name}" /> ${field.label}</label></div>`;
        }
        const input = field.type === 'text'
            ? `<input type="text" id="${field.name}" name="${field.name}" class="app-input" />`
            : field.type === 'date'
                ? `<input id="${field.name}" name="${field.name}" />`
                : `<input id="${field.name}" name="${field.name}" />`;
        return `<div class="form-group"><label for="${field.name}">${field.label}</label>${input}</div>`;
    }

    function initializeWidgets() {
        window.SystemSettingsModule.config.sections.forEach(function (section) {
            section.fields.forEach(function (field) {
                const selector = `#${field.name}`;
                if (!document.querySelector(selector) || field.type === 'hidden') {
                    return;
                }
                if (field.type === 'text' && $.fn.kendoTextBox) {
                    $(selector).kendoTextBox();
                }
                if (field.type === 'number' && $.fn.kendoNumericTextBox) {
                    $(selector).kendoNumericTextBox({ min: field.min, decimals: field.decimals ?? 0, step: field.step || 1, format: field.decimals ? `n${field.decimals}` : 'n0' });
                }
                if (field.type === 'date' && $.fn.kendoDatePicker) {
                    $(selector).kendoDatePicker({ format: 'yyyy-MM-dd' });
                }
                if (field.type === 'checkbox' && $.fn.kendoCheckBox) {
                    $(selector).kendoCheckBox();
                }
            });
        });
    }

    function normalizeString(value) {
        return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim();
    }

    window.SystemSettingsModule.Details = { init: init, render: render, loadByCompany: loadByCompany, populate: populate, save: save };
})();
