(function () {
    'use strict';

    let selectedStudentId = 0;
    let documentTypes = [];

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

    function renderDocuments(documents, checklists) {
        const checklistMap = (checklists || []).reduce((acc, item) => {
            acc[item.documentTypeId] = item;
            return acc;
        }, {});
        const options = documentTypes.map(item => `<option value="${item.documentTypeId}">${item.name}</option>`).join('');
        const docsHtml = (documents || []).length
            ? documents.map(item => `
                <div class="document-item">
                    <div>
                        <strong>${item.originalFileName}</strong><br />
                        <small>Status: ${item.status} | Type: ${item.documentTypeId}</small>
                    </div>
                    <div class="page-header__actions">
                        <button type="button" class="btn btn--secondary js-verify-doc" data-id="${item.studentDocumentId}">Verify</button>
                        <button type="button" class="btn btn--ghost js-reject-doc" data-id="${item.studentDocumentId}">Reject</button>
                    </div>
                </div>`).join('')
            : '<div class="empty-state">No uploaded documents found.</div>';
        const checklistHtml = (checklists || []).length
            ? checklists.map(item => `<li>Type ${item.documentTypeId}: ${item.isMandatory ? 'Mandatory' : 'Optional'} / ${item.isVerified ? 'Verified' : item.isSubmitted ? 'Submitted' : 'Pending'}</li>`).join('')
            : '<li>No generated checklist found.</li>';

        $('#studentDocumentsPanel').html(`
            <div class="card">
                <div class="card__header">
                    <h3 class="card__title">Upload Document</h3>
                    <p class="card__subtitle">Drag and drop or browse a file to upload.</p>
                </div>
                <div id="studentDocumentDropZone" class="file-upload-area">
                    <select id="studentDocumentTypeId" class="app-input">${options}</select>
                    <input type="file" id="studentDocumentFile" class="app-input mt-16" />
                    <button type="button" id="btnUploadStudentDocument" class="btn btn--primary mt-16">Upload Document</button>
                </div>
                <div class="mt-16"><strong>Checklist</strong><ul>${checklistHtml}</ul></div>
                <div class="document-list mt-16">${docsHtml}</div>
            </div>
        `);

        bindDocumentActions();
    }

    function renderApplications(applications) {
        const html = (applications || []).length
            ? applications.map(item => `
                <div class="timeline-item">
                    <div class="timeline-item__date">${item.internalRefNo || '-'} / Status ${item.status}</div>
                    <div class="timeline-item__body">
                        <strong>Program:</strong> ${item.programId} | <strong>Country:</strong> ${item.countryId}<br />
                        <span>Applied: ${item.appliedDate ? kendo.toString(new Date(item.appliedDate), 'dd MMM yyyy') : '-'}</span>
                    </div>
                </div>`).join('')
            : '<div class="empty-state">No applications found for this student.</div>';
        $('#studentApplicationsPanel').html(html);
    }

    async function changeDocumentStatus(studentDocumentId, newStatus) {
        const payload = {
            studentDocumentId,
            newStatus,
            changedBy: 1,
            notes: newStatus === 3 ? 'Verified from student workspace' : 'Rejected from student workspace',
            rejectionReason: newStatus === 4 ? prompt('Enter rejection reason') : null
        };
        if (newStatus === 4 && !payload.rejectionReason) {
            return;
        }
        await window.ApiClient.post(window.StudentModule.config.apiEndpoints.changeDocumentStatus, payload);
        await loadWorkspace(selectedStudentId);
    }

    function bindDocumentActions() {
        $('#btnUploadStudentDocument').off('click').on('click', async function () {
            const fileInput = document.getElementById('studentDocumentFile');
            if (!selectedStudentId || !fileInput?.files?.length) {
                window.AppToast?.warning('Select a student and choose a file first.');
                return;
            }

            const formData = new FormData();
            formData.append('studentId', String(selectedStudentId));
            formData.append('documentTypeId', String(document.getElementById('studentDocumentTypeId').value || 0));
            formData.append('branchId', '1');
            formData.append('requestedBy', '1');
            formData.append('file', fileInput.files[0]);

            const token = localStorage.getItem('authToken');
            const response = await fetch(window.StudentModule.config.apiEndpoints.uploadDocument, {
                method: 'POST',
                headers: token ? { Authorization: `Bearer ${token}` } : {},
                body: formData
            });
            const result = await response.json();
            if (result.success) {
                window.AppToast?.success('Document uploaded successfully.');
                await loadWorkspace(selectedStudentId);
                return;
            }
            window.AppToast?.error(result.message || 'Document upload failed.');
        });

        $('.js-verify-doc').off('click').on('click', function () {
            changeDocumentStatus(parseInt($(this).data('id'), 10), 3);
        });

        $('.js-reject-doc').off('click').on('click', function () {
            changeDocumentStatus(parseInt($(this).data('id'), 10), 4);
        });
    }

    async function ensureDocumentTypesLoaded() {
        if (documentTypes.length) {
            return;
        }
        try {
            const response = await window.ApiClient.get(window.StudentModule.config.apiEndpoints.documentTypesDdl);
            documentTypes = response?.data || [];
        } catch {
            documentTypes = [];
        }
    }

    async function loadWorkspace(studentId) {
        selectedStudentId = studentId;
        try {
            await ensureDocumentTypesLoaded();
            const [studentResponse, academicResponse, historyResponse, readinessResponse, documentResponse, checklistResponse, applicationResponse] = await Promise.all([
                window.ApiClient.get(window.StudentModule.config.apiEndpoints.read(studentId)),
                window.ApiClient.get(window.StudentModule.config.apiEndpoints.academicByStudent(studentId)),
                window.ApiClient.get(window.StudentModule.config.apiEndpoints.statusHistoryByStudent(studentId)),
                window.ApiClient.get(window.StudentModule.config.apiEndpoints.applicationReady(studentId)),
                window.ApiClient.get(window.StudentModule.config.apiEndpoints.documentsByStudent(studentId)),
                window.ApiClient.get(window.StudentModule.config.apiEndpoints.checklistsByStudent(studentId)),
                window.ApiClient.get(window.StudentModule.config.apiEndpoints.applicationsByStudent(studentId))
            ]);

            renderProfile(studentResponse?.data || {}, readinessResponse?.data || {});
            renderAcademic(academicResponse?.data || []);
            renderHistory(historyResponse?.data || []);
            renderDocuments(documentResponse?.data || [], checklistResponse?.data || []);
            renderApplications(applicationResponse?.data || []);
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
