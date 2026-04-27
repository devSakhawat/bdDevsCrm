(function () {
    'use strict';

    function renderProfile(student, readiness) {
        $('#studentProfilePanel').html(`
            <div class="profile-grid">
                <div><strong>Name:</strong> ${student.studentName || '-'}</div>
                <div><strong>Code:</strong> ${student.studentCode || '-'}</div>
                <div><strong>Email:</strong> ${student.email || '-'}</div>
                <div><strong>Phone:</strong> ${student.phone || '-'}</div>
                <div><strong>Branch:</strong> ${student.branchId || '-'}</div>
                <div><strong>Processing Officer:</strong> ${student.processingOfficerId || '-'}</div>
                <div><strong>Preferred Country:</strong> ${student.preferredCountryId || '-'}</div>
                <div><strong>Preferred Degree Level:</strong> ${student.preferredDegreeLevelId || '-'}</div>
                <div><strong>Desired Intake:</strong> ${student.desiredIntake || '-'}</div>
                <div><strong>IELTS:</strong> ${student.ieltsScore || '-'}</div>
                <div><strong>Passport:</strong> ${student.passportNumber || '-'}</div>
                <div><strong>Ready:</strong> ${readiness?.isReady ? 'Yes' : 'No'}</div>
            </div>
            <div class="mt-16"><strong>Missing Requirements:</strong> ${(readiness?.missingRequirements || []).join(', ') || 'None'}</div>
        `);
    }

    function renderAcademic(items) {
        const profile = Array.isArray(items) ? items[0] : null;
        if (!profile) {
            $('#studentAcademicPanel').html('<div class="empty-state">No academic profile found for this student.</div>');
            return;
        }

        $('#studentAcademicPanel').html(`
            <div class="profile-grid">
                <div><strong>SSC:</strong> ${profile.sscResult || '-'} (${profile.sscYear || '-'})</div>
                <div><strong>HSC:</strong> ${profile.hscResult || '-'} (${profile.hscYear || '-'})</div>
                <div><strong>Bachelor:</strong> ${profile.bachelorResult || '-'} (${profile.bachelorYear || '-'})</div>
                <div><strong>Master:</strong> ${profile.masterResult || '-'} (${profile.masterYear || '-'})</div>
                <div><strong>PhD:</strong> ${profile.phdResult || '-'} (${profile.phdYear || '-'})</div>
                <div><strong>English Proficiency:</strong> ${profile.currentEnglishProficiency || '-'} ${profile.currentEnglishScore || ''}</div>
                <div class="form-group--span-2"><strong>Institutes:</strong> SSC ${profile.sscInstitute || '-'}, HSC ${profile.hscInstitute || '-'}, Bachelor ${profile.bachelorInstitute || '-'}, Master ${profile.masterInstitute || '-'}, PhD ${profile.phdInstitute || '-'}</div>
            </div>
        `);
    }

    function renderHistory(items) {
        const html = (items || []).length
            ? items.map(item => `
                <div class="timeline-item">
                    <div class="timeline-item__date">${kendo.toString(new Date(item.changedDate), 'dd MMM yyyy HH:mm')}</div>
                    <div class="timeline-item__body">
                        <strong>${item.oldStatus || '-'} → ${item.newStatus}</strong><br />
                        <span>${item.notes || '-'}</span>
                    </div>
                </div>`).join('')
            : '<div class="empty-state">No student status history found.</div>';
        $('#studentStatusHistoryPanel').html(html);
    }

    async function loadWorkspace(studentId) {
        try {
            const [studentResponse, academicResponse, historyResponse, readinessResponse] = await Promise.all([
                window.ApiClient.get(window.StudentModule.config.apiEndpoints.read(studentId)),
                window.ApiClient.get(window.StudentModule.config.apiEndpoints.academicByStudent(studentId)),
                window.ApiClient.get(window.StudentModule.config.apiEndpoints.statusHistoryByStudent(studentId)),
                window.ApiClient.get(window.StudentModule.config.apiEndpoints.applicationReady(studentId))
            ]);

            renderProfile(studentResponse?.data || {}, readinessResponse?.data || {});
            renderAcademic(academicResponse?.data || []);
            renderHistory(historyResponse?.data || []);
        } catch (error) {
            window.AppToast?.error(error?.message || 'Failed to load student workspace');
        }
    }

    function bindGridSelection() {
        const grid = $('#studentGrid').data('kendoGrid');
        if (!grid) {
            setTimeout(bindGridSelection, 600);
            return;
        }

        $('#studentWorkspaceTabStrip').kendoTabStrip({ animation: { open: { effects: 'fadeIn' } } });

        $('#studentGrid').off('click.studentRow').on('click.studentRow', 'tbody tr', function () {
            const dataItem = grid.dataItem(this);
            if (!dataItem) return;
            loadWorkspace(dataItem.studentId);
        });
    }

    $(document).ready(bindGridSelection);
})();
