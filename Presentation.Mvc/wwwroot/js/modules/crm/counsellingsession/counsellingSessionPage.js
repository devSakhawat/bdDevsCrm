(function () {
    'use strict';

    function initShortlistGrid() {
        $('#sessionProgramShortlistGrid').kendoGrid({
            dataSource: { data: [] },
            sortable: true,
            filterable: true,
            pageable: false,
            height: 340,
            columns: [
                { field: 'sessionProgramShortlistId', title: '#', width: 50 },
                { field: 'universityId', title: 'University', width: 100 },
                { field: 'programId', title: 'Program', width: 90 },
                { field: 'intakeId', title: 'Intake', width: 80 },
                { field: 'priority', title: 'Priority', width: 80 },
                { field: 'eligibilityStatus', title: 'Eligibility', width: 90 },
                { field: 'isRecommended', title: 'Recommended', width: 100, template: d => d.isRecommended ? 'Yes' : 'No' }
            ]
        });
    }

    async function loadShortlists(sessionId) {
        try {
            const response = await window.ApiClient.get(window.CounsellingSessionModule.config.apiEndpoints.shortlistBySession(sessionId));
            const grid = $('#sessionProgramShortlistGrid').data('kendoGrid');
            grid.dataSource.data(response?.data || []);
        } catch (error) {
            window.AppToast?.error(error?.message || 'Failed to load program shortlists');
        }
    }

    async function loadTimeline(leadId) {
        try {
            const response = await window.ApiClient.get(window.CounsellingSessionModule.config.apiEndpoints.byLead(leadId));
            const items = response?.data || [];
            const html = items.length
                ? items.map(item => `
                    <div class="timeline-item">
                        <div class="timeline-item__date">${kendo.toString(new Date(item.sessionDate), 'dd MMM yyyy')}</div>
                        <div class="timeline-item__body">
                            <strong>Outcome:</strong> ${item.outcome}<br />
                            <strong>Target Intake:</strong> ${item.targetIntake || '-'}<br />
                            <span>${item.outcomeNotes || item.nextSteps || '-'}</span>
                        </div>
                    </div>`).join('')
                : '<div class="empty-state">No session history found for this lead.</div>';
            $('#counsellingSessionTimeline').html(html);
        } catch (error) {
            $('#counsellingSessionTimeline').html('<div class="empty-state">Failed to load session timeline.</div>');
        }
    }

    function bindGridSelection() {
        const grid = $('#counsellingSessionGrid').data('kendoGrid');
        if (!grid) {
            setTimeout(bindGridSelection, 600);
            return;
        }

        initShortlistGrid();
        $('#counsellingSessionGrid').off('click.counsellingRow').on('click.counsellingRow', 'tbody tr', function () {
            const dataItem = grid.dataItem(this);
            if (!dataItem) return;
            loadShortlists(dataItem.counsellingSessionId);
            loadTimeline(dataItem.leadId);
        });
    }

    $(document).ready(bindGridSelection);
})();
