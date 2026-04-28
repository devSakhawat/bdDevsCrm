(function () {
    'use strict';

    let selectedLead = null;

    async function convertSelectedLead() {
        if (!selectedLead) {
            window.AppToast?.warning('Select a lead first.');
            return;
        }

        const request = {
            leadId: selectedLead.leadId,
            preferredCountryId: selectedLead.interestedCountryId || null,
            preferredDegreeLevelId: selectedLead.interestedDegreeLevelId || null,
            desiredIntake: null,
            passportNumber: null,
            forceProceed: false,
            requestedBy: 1
        };

        try {
            const preflight = await window.ApiClient.post(window.LeadModule.config.apiEndpoints.convertPreflight, request);
            const result = preflight?.data || {};
            if ((preflight?.message || '').toUpperCase() === 'SOFT_WARNING' || (result.softWarnings || []).length) {
                const warningsText = (result.softWarnings || []).join('\n- ');
                const proceed = confirm(`Warnings:\n- ${warningsText}\n\nProceed with conversion?`);
                if (!proceed) {
                    return;
                }
                request.forceProceed = true;
            }

            const response = await window.ApiClient.post(window.LeadModule.config.apiEndpoints.convert, request);
            if ((response?.data?.studentId || 0) > 0) {
                window.AppToast?.success('Lead converted to student successfully.');
                window.LeadModule.Summary?.refreshGrid?.();
                return;
            }

            window.AppToast?.warning(response?.message || 'Conversion requires additional review.');
        } catch (error) {
            const message = error?.message || 'Lead conversion failed';
            window.AppToast?.error(message);
        }
    }

    function bindGridSelection() {
        const grid = $('#leadGrid').data('kendoGrid');
        if (!grid) {
            setTimeout(bindGridSelection, 500);
            return;
        }

        $('#leadGrid').off('click.leadRow').on('click.leadRow', 'tbody tr', function () {
            selectedLead = grid.dataItem(this);
        });

        $('#btnConvertLead').off('click').on('click', convertSelectedLead);
    }

    $(document).ready(bindGridSelection);
})();
