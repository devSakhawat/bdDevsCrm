(function () {
    'use strict';

    async function loadHistory(followUpId) {
        try {
            const response = await window.ApiClient.get(window.FollowUpModule.config.apiEndpoints.historyByFollowUp(followUpId));
            const items = response?.data || [];
            const html = items.length
                ? items.map(item => `
                    <div class="timeline-item">
                        <div class="timeline-item__date">${kendo.toString(new Date(item.changedDate), 'dd MMM yyyy HH:mm')}</div>
                        <div class="timeline-item__body">
                            <strong>${item.oldStatus} → ${item.newStatus}</strong><br />
                            <span>${item.remarks || '-'}</span>
                        </div>
                    </div>`).join('')
                : '<div class="empty-state">No follow-up history found.</div>';
            $('#followUpHistoryList').html(html);
        } catch (error) {
            $('#followUpHistoryList').html('<div class="empty-state">Failed to load follow-up history.</div>');
        }
    }

    function bindGridSelection() {
        const grid = $('#followUpGrid').data('kendoGrid');
        if (!grid) {
            setTimeout(bindGridSelection, 600);
            return;
        }

        $('#followUpGrid').off('click.followupRow').on('click.followupRow', 'tbody tr', function () {
            const dataItem = grid.dataItem(this);
            if (!dataItem) return;
            loadHistory(dataItem.followUpId);
        });
    }

    $(document).ready(bindGridSelection);
})();
