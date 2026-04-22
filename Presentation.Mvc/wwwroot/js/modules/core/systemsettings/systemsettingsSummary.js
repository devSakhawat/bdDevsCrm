(function () {
    'use strict';

    window.SystemSettingsModule = window.SystemSettingsModule || {};
    let companyDropDown = null;

        async function init() {
            await reload();
        }

    async function reload() {
        window.AppLoader?.show('Loading system settings...');
        try {
            const [companiesResponse, listResponse, assemblyResponse] = await Promise.all([
                window.ApiClient.get(window.SystemSettingsModule.config.apiEndpoints.companies),
                window.ApiClient.get(window.SystemSettingsModule.config.apiEndpoints.list),
                window.ApiClient.get(window.SystemSettingsModule.config.apiEndpoints.assemblyInfo)
            ]);

            window.SystemSettingsModule.state.companies = companiesResponse?.success && Array.isArray(companiesResponse.data) ? companiesResponse.data : [];
            window.SystemSettingsModule.state.settingsList = listResponse?.success && Array.isArray(listResponse.data) ? listResponse.data : [];
            window.SystemSettingsModule.state.assemblyInfo = assemblyResponse?.success ? (assemblyResponse.data || null) : null;

            renderCompanyDropDown();
            renderAssemblyInfo();

            const preferredCompanyId = window.SystemSettingsModule.state.companies[0]?.companyId || window.SystemSettingsModule.state.settingsList[0]?.companyId || null;
            if (preferredCompanyId) {
                companyDropDown?.value(String(preferredCompanyId));
                await window.SystemSettingsModule.Details?.loadByCompany?.(preferredCompanyId);
            } else if (window.SystemSettingsModule.state.settingsList.length) {
                window.SystemSettingsModule.Details?.populate?.(window.SystemSettingsModule.state.settingsList[0]);
            }
        } catch (error) {
            window.AppToast?.error(error?.message || 'Failed to load system settings');
        } finally {
            window.AppLoader?.hide();
        }
    }

    function renderCompanyDropDown() {
        companyDropDown = $(window.SystemSettingsModule.config.dom.companySelect).kendoDropDownList({
            dataSource: window.SystemSettingsModule.state.companies,
            dataTextField: 'companyName',
            dataValueField: 'companyId',
            optionLabel: 'Select company...',
            filter: 'contains',
            change: async function () {
                const companyId = parseInt(this.value(), 10);
                if (companyId > 0) {
                    await window.SystemSettingsModule.Details?.loadByCompany?.(companyId);
                }
            }
        }).data('kendoDropDownList');
    }

    function renderAssemblyInfo() {
        const info = window.SystemSettingsModule.state.assemblyInfo || {};
        const cards = [
            { label: 'Assembly Title', value: info.assemblyTitle || '-' },
            { label: 'Version', value: info.assemblyVersion || '-' },
            { label: 'Company', value: info.assemblyCompany || '-' },
            { label: 'Product', value: info.assemblyProduct || '-' },
            { label: 'Powered By', value: info.poweredBy || '-' },
            { label: 'API Path', value: info.apiPath || '-' }
        ];
        $(window.SystemSettingsModule.config.dom.assemblyInfo).html(cards.map(function (card) {
            return `<div class="stat-card"><span class="stat-card__label">${card.label}</span><span class="stat-card__value">${escapeHtml(card.value)}</span></div>`;
        }).join(''));
    }

    function escapeHtml(value) {
        return String(value ?? '')
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;');
    }

    window.SystemSettingsModule.Summary = { init: init, reload: reload };
})();
