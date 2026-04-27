(function () {
    'use strict';

    function renderBoard(items) {
        const statuses = [
            { key: 1, title: 'Draft' },
            { key: 2, title: 'Submitted' },
            { key: 3, title: 'Under Review' },
            { key: 4, title: 'Conditional' },
            { key: 5, title: 'Unconditional' },
            { key: 6, title: 'Deposit Pending' },
            { key: 7, title: 'Deposit Paid' },
            { key: 8, title: 'Visa' },
            { key: 9, title: 'Enrolled' },
            { key: 10, title: 'Withdrawn' },
            { key: 11, title: 'Rejected' }
        ];

        const html = statuses.map(status => {
            const cards = (items || []).filter(item => item.status === status.key).map(item => `
                <div class="kanban-card">
                    <strong>${item.internalRefNo || `APP-${item.applicationId}`}</strong><br />
                    <small>Student ${item.studentId} / Program ${item.programId}</small>
                </div>`).join('') || '<div class="empty-state">No items</div>';
            return `<div class="kanban-column"><h4>${status.title}</h4>${cards}</div>`;
        }).join('');
        $('#applicationKanbanBoard').html(html);
    }

    function renderWorkspace(application, conditions, documents) {
        $('#applicationBasicPanel').html(`
            <div class="profile-grid">
                <div><strong>Ref No:</strong> ${application.internalRefNo || '-'}</div>
                <div><strong>Student:</strong> ${application.studentId || '-'}</div>
                <div><strong>Branch:</strong> ${application.branchId || '-'}</div>
                <div><strong>Country:</strong> ${application.countryId || '-'}</div>
                <div><strong>University:</strong> ${application.universityId || '-'}</div>
                <div><strong>Program:</strong> ${application.programId || '-'}</div>
                <div><strong>Intake:</strong> ${application.intakeId || '-'}</div>
                <div><strong>Status:</strong> ${application.status || '-'}</div>
                <div><strong>Priority:</strong> ${application.priority || '-'}</div>
                <div><strong>Applied:</strong> ${application.appliedDate ? kendo.toString(new Date(application.appliedDate), 'dd MMM yyyy') : '-'}</div>
                <div><strong>Offer:</strong> ${application.offerReceivedDate ? kendo.toString(new Date(application.offerReceivedDate), 'dd MMM yyyy') : '-'}</div>
                <div><strong>Enrollment:</strong> ${application.enrollmentDate ? kendo.toString(new Date(application.enrollmentDate), 'dd MMM yyyy') : '-'}</div>
            </div>
        `);

        $('#applicationConditionsPanel').html((conditions || []).length
            ? conditions.map(item => `<div class="document-item"><div><strong>${item.conditionText}</strong><br /><small>Status ${item.status} / Due ${item.dueDate ? kendo.toString(new Date(item.dueDate), 'dd MMM yyyy') : '-'}</small></div></div>`).join('')
            : '<div class="empty-state">No conditions found.</div>');

        $('#applicationDocumentsPanel').html((documents || []).length
            ? documents.map(item => `<div class="document-item"><div><strong>Document ${item.documentId}</strong><br /><small>${item.isRequired ? 'Required' : 'Optional'}</small></div></div>`).join('')
            : '<div class="empty-state">No linked documents found.</div>');

        $('#applicationTimelinePanel').html(`
            <div class="timeline-item">
                <div class="timeline-item__date">${application.createdDate ? kendo.toString(new Date(application.createdDate), 'dd MMM yyyy HH:mm') : '-'}</div>
                <div class="timeline-item__body">Application created</div>
            </div>
            ${application.offerReceivedDate ? `<div class="timeline-item"><div class="timeline-item__date">${kendo.toString(new Date(application.offerReceivedDate), 'dd MMM yyyy HH:mm')}</div><div class="timeline-item__body">Offer received</div></div>` : ''}
            ${application.enrollmentDate ? `<div class="timeline-item"><div class="timeline-item__date">${kendo.toString(new Date(application.enrollmentDate), 'dd MMM yyyy HH:mm')}</div><div class="timeline-item__body">Student enrolled</div></div>` : ''}
        `);
    }

    async function loadBoard() {
        try {
            const response = await window.ApiClient.get(window.ApplicationModule.config.apiEndpoints.board);
            renderBoard(response?.data || []);
        } catch {
            $('#applicationKanbanBoard').html('<div class="empty-state">Failed to load application board.</div>');
        }
    }

    async function loadWorkspace(applicationId) {
        try {
            const [applicationResponse, conditionsResponse, documentsResponse] = await Promise.all([
                window.ApiClient.get(window.ApplicationModule.config.apiEndpoints.read(applicationId)),
                window.ApiClient.get(window.ApplicationModule.config.apiEndpoints.conditionsByApplication(applicationId)),
                window.ApiClient.get(window.ApplicationModule.config.apiEndpoints.documentsByApplication(applicationId))
            ]);
            renderWorkspace(applicationResponse?.data || {}, conditionsResponse?.data || [], documentsResponse?.data || []);
        } catch (error) {
            window.AppToast?.error(error?.message || 'Failed to load application workspace');
        }
    }

    function bindGridSelection() {
        const grid = $('#applicationGrid').data('kendoGrid');
        if (!grid) {
            setTimeout(bindGridSelection, 600);
            return;
        }

        $('#applicationWorkspaceTabStrip').kendoTabStrip({ animation: { open: { effects: 'fadeIn' } } });
        loadBoard();

        $('#applicationGrid').off('click.applicationRow').on('click.applicationRow', 'tbody tr', function () {
            const dataItem = grid.dataItem(this);
            if (!dataItem) return;
            loadWorkspace(dataItem.applicationId);
        });

        $('#btnRefreshApplication').off('click.board').on('click.board', function () {
            loadBoard();
        });
    }

    $(document).ready(bindGridSelection);
})();
