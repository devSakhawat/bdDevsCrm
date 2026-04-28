(function () {
    'use strict';

    const scholarshipStatusMap = {
        1: 'Draft',
        2: 'Confirmed',
        3: 'Expired',
        4: 'Cancelled'
    };

    const visaStatusMap = {
        1: 'Draft',
        2: 'Prepared',
        3: 'Submitted',
        4: 'Biometric',
        5: 'Interview',
        6: 'Decision Pending',
        7: 'Approved',
        8: 'Refused',
        9: 'Expired'
    };

    const paymentStatusMap = {
        1: 'Draft',
        2: 'Submitted',
        3: 'Verified',
        4: 'Receipted',
        5: 'Refund Pending',
        6: 'Refunded'
    };

    let currentApplication = null;
    let refundMap = {};

    function formatDate(value, format = 'dd MMM yyyy') {
        return value ? kendo.toString(new Date(value), format) : '-';
    }

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

    function renderBasic(application) {
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
                <div><strong>Applied:</strong> ${formatDate(application.appliedDate)}</div>
                <div><strong>Offer:</strong> ${formatDate(application.offerReceivedDate)}</div>
                <div><strong>Enrollment:</strong> ${formatDate(application.enrollmentDate)}</div>
                <div><strong>Processing Officer:</strong> ${application.processingOfficerId || '-'}</div>
            </div>
        `);
    }

    function renderConditions(conditions) {
        $('#applicationConditionsPanel').html((conditions || []).length
            ? conditions.map(item => `<div class="document-item"><div><strong>${item.conditionText}</strong><br /><small>Status ${item.status} / Due ${formatDate(item.dueDate)}</small></div></div>`).join('')
            : '<div class="empty-state">No conditions found.</div>');
    }

    function renderDocuments(documents) {
        $('#applicationDocumentsPanel').html((documents || []).length
            ? documents.map(item => `<div class="document-item"><div><strong>Document ${item.documentId}</strong><br /><small>${item.isRequired ? 'Required' : 'Optional'}</small></div></div>`).join('')
            : '<div class="empty-state">No linked documents found.</div>');
    }

    function renderScholarships(items, impact) {
        const listHtml = (items || []).length
            ? items.map(item => `
                <div class="document-item">
                    <div>
                        <strong>${item.scholarshipName}</strong><br />
                        <small>${item.scholarshipType} / ${item.grantedAmount} ${item.currency} / ${scholarshipStatusMap[item.status] || item.status}</small>
                    </div>
                    <button type="button" class="btn btn--ghost js-delete-scholarship" data-id="${item.scholarshipApplicationId}">Delete</button>
                </div>`).join('')
            : '<div class="empty-state">No scholarships recorded.</div>';

        $('#applicationScholarshipPanel').html(`
            <div class="card">
                <div class="card__header">
                    <h3 class="card__title">Scholarship Entry</h3>
                    <p class="card__subtitle">Track scholarship awards and their impact on commission.</p>
                </div>
                <div class="form-layout form-layout--grid">
                    <div class="form-group"><label for="scholarshipName">Scholarship Name</label><input id="scholarshipName" class="app-input" type="text" /></div>
                    <div class="form-group"><label for="scholarshipType">Scholarship Type</label><input id="scholarshipType" class="app-input" type="text" /></div>
                    <div class="form-group"><label for="scholarshipAmount">Granted Amount</label><input id="scholarshipAmount" class="app-input" type="number" step="0.01" /></div>
                    <div class="form-group"><label for="scholarshipCurrency">Currency</label><input id="scholarshipCurrency" class="app-input" type="text" value="BDT" /></div>
                    <div class="form-group"><label for="scholarshipPercentage">Scholarship %</label><input id="scholarshipPercentage" class="app-input" type="number" step="0.01" /></div>
                    <div class="form-group"><label for="scholarshipStatus">Status</label><input id="scholarshipStatus" class="app-input" type="number" value="2" min="1" max="4" /></div>
                    <div class="form-group"><label for="scholarshipConfirmedDate">Confirmed Date</label><input id="scholarshipConfirmedDate" class="app-input" type="date" /></div>
                    <div class="form-group"><label for="scholarshipExpiryDate">Expiry Date</label><input id="scholarshipExpiryDate" class="app-input" type="date" /></div>
                    <div class="form-group form-group--span-2"><label for="scholarshipNotes">Notes</label><textarea id="scholarshipNotes" class="app-input"></textarea></div>
                </div>
                <div class="form-action-bar">
                    <button type="button" id="btnCreateScholarship" class="btn btn--primary">Save Scholarship</button>
                </div>
                <div class="mt-16">
                    <strong>Commission Impact</strong>
                    <div class="profile-grid mt-16">
                        <div><strong>Tuition:</strong> ${impact?.tuitionAmount ?? 0}</div>
                        <div><strong>Scholarship:</strong> ${impact?.scholarshipAmount ?? 0}</div>
                        <div><strong>Commissionable:</strong> ${impact?.commissionableAmount ?? 0}</div>
                        <div><strong>Estimated Commission:</strong> ${impact?.estimatedCommissionAmount ?? 0}</div>
                    </div>
                </div>
                <div class="document-list mt-16">${listHtml}</div>
            </div>
        `);

        $('#btnCreateScholarship').off('click').on('click', createScholarship);
        $('.js-delete-scholarship').off('click').on('click', function () {
            deleteScholarship(parseInt($(this).data('id'), 10));
        });
    }

    function renderVisa(visas, histories, checklist) {
        const visaListHtml = (visas || []).length
            ? visas.map(item => `
                <div class="document-item">
                    <div>
                        <strong>${item.applicationRefNo || `Visa-${item.visaApplicationId}`}</strong><br />
                        <small>${item.embassyName || '-'} / ${visaStatusMap[item.status] || item.status} / Decision ${formatDate(item.decisionDate)}</small>
                    </div>
                    <div class="page-header__actions">
                        <button type="button" class="btn btn--secondary js-visa-approve" data-id="${item.visaApplicationId}">Approve</button>
                        <button type="button" class="btn btn--ghost js-visa-refuse" data-id="${item.visaApplicationId}">Refuse</button>
                    </div>
                </div>`).join('')
            : '<div class="empty-state">No visa applications recorded.</div>';

        const historyHtml = (histories || []).length
            ? histories.map(item => `<div class="timeline-item"><div class="timeline-item__date">${formatDate(item.changedDate, 'dd MMM yyyy HH:mm')}</div><div class="timeline-item__body"><strong>${item.oldStatus} → ${item.newStatus}</strong><br /><span>${item.notes || '-'}</span></div></div>`).join('')
            : '<div class="empty-state">No visa timeline entries found.</div>';

        const checklistHtml = (checklist || []).length
            ? checklist.map(item => `<li>Document ${item.documentTypeId}: ${item.isVerified ? 'Verified' : item.isSubmitted ? 'Submitted' : 'Pending'}${item.isMandatory ? ' (Mandatory)' : ''}</li>`).join('')
            : '<li>No student document checklist found.</li>';

        $('#applicationVisaPanel').html(`
            <div class="card">
                <div class="card__header">
                    <h3 class="card__title">Visa Application</h3>
                    <p class="card__subtitle">Create visa records, track timeline, and verify visa document readiness.</p>
                </div>
                <div class="form-layout form-layout--grid">
                    <div class="form-group"><label for="visaCountryId">Visa Country Id</label><input id="visaCountryId" class="app-input" type="number" value="${currentApplication.countryId || 0}" /></div>
                    <div class="form-group"><label for="visaEmbassyName">Embassy Name</label><input id="visaEmbassyName" class="app-input" type="text" /></div>
                    <div class="form-group"><label for="visaApplicationRefNo">Application Ref No</label><input id="visaApplicationRefNo" class="app-input" type="text" /></div>
                    <div class="form-group"><label for="visaStatus">Status</label><input id="visaStatus" class="app-input" type="number" value="3" min="1" max="9" /></div>
                    <div class="form-group"><label for="visaSubmittedDate">Submitted Date</label><input id="visaSubmittedDate" class="app-input" type="date" /></div>
                    <div class="form-group"><label for="visaBiometricDate">Biometric Date</label><input id="visaBiometricDate" class="app-input" type="date" /></div>
                    <div class="form-group"><label for="visaInterviewDate">Interview Date</label><input id="visaInterviewDate" class="app-input" type="date" /></div>
                    <div class="form-group"><label for="visaDecisionDate">Decision Date</label><input id="visaDecisionDate" class="app-input" type="date" /></div>
                    <div class="form-group"><label for="visaExpiryDate">Visa Expiry Date</label><input id="visaExpiryDate" class="app-input" type="date" /></div>
                    <div class="form-group form-group--span-2"><label for="visaNotes">Notes</label><textarea id="visaNotes" class="app-input"></textarea></div>
                </div>
                <div class="form-action-bar">
                    <button type="button" id="btnCreateVisa" class="btn btn--primary">Save Visa Application</button>
                </div>
                <div class="mt-16"><strong>Visa Document Checklist</strong><ul>${checklistHtml}</ul></div>
                <div class="document-list mt-16">${visaListHtml}</div>
                <div class="timeline-panel mt-16">${historyHtml}</div>
            </div>
        `);

        $('#btnCreateVisa').off('click').on('click', createVisa);
        $('.js-visa-approve').off('click').on('click', function () {
            changeVisaStatus(parseInt($(this).data('id'), 10), 7);
        });
        $('.js-visa-refuse').off('click').on('click', function () {
            changeVisaStatus(parseInt($(this).data('id'), 10), 8);
        });
    }

    function renderPayments(payments) {
        const listHtml = (payments || []).length
            ? payments.map(item => {
                const refunds = refundMap[item.studentPaymentId] || [];
                const refundsHtml = refunds.length
                    ? refunds.map(refund => `<li>${refund.refundAmount} on ${formatDate(refund.refundDate)} / Status ${refund.status}</li>`).join('')
                    : '<li>No refunds.</li>';
                return `
                    <div class="document-item">
                        <div>
                            <strong>${item.receiptNo || 'Pending Receipt'}</strong><br />
                            <small>${item.amount} ${item.currency} / ${paymentStatusMap[item.status] || item.status} / ${formatDate(item.paymentDate)}</small>
                            <ul class="mt-16">${refundsHtml}</ul>
                        </div>
                        <div class="page-header__actions">
                            <button type="button" class="btn btn--secondary js-payment-verify" data-id="${item.studentPaymentId}">Verify</button>
                            <button type="button" class="btn btn--secondary js-payment-receipt" data-id="${item.studentPaymentId}">Receipt</button>
                            <button type="button" class="btn btn--ghost js-payment-refund" data-id="${item.studentPaymentId}" data-amount="${item.amount}">Refund</button>
                        </div>
                    </div>`;
            }).join('')
            : '<div class="empty-state">No payments recorded.</div>';

        $('#applicationPaymentsPanel').html(`
            <div class="card">
                <div class="card__header">
                    <h3 class="card__title">Student Payments</h3>
                    <p class="card__subtitle">Capture payment entry, student payment history, refunds, and receipt generation.</p>
                </div>
                <div class="form-layout form-layout--grid">
                    <div class="form-group"><label for="paymentType">Payment Type</label><input id="paymentType" class="app-input" type="number" value="1" min="1" /></div>
                    <div class="form-group"><label for="paymentAmount">Amount</label><input id="paymentAmount" class="app-input" type="number" step="0.01" /></div>
                    <div class="form-group"><label for="paymentCurrency">Currency</label><input id="paymentCurrency" class="app-input" type="text" value="BDT" /></div>
                    <div class="form-group"><label for="paymentExchangeRate">Exchange Rate</label><input id="paymentExchangeRate" class="app-input" type="number" step="0.000001" value="1" /></div>
                    <div class="form-group"><label for="paymentDate">Payment Date</label><input id="paymentDate" class="app-input" type="date" /></div>
                    <div class="form-group"><label for="paymentMethod">Payment Method</label><input id="paymentMethod" class="app-input" type="text" /></div>
                    <div class="form-group"><label for="paymentBankName">Bank Name</label><input id="paymentBankName" class="app-input" type="text" /></div>
                    <div class="form-group"><label for="paymentTransactionRef">Transaction Ref</label><input id="paymentTransactionRef" class="app-input" type="text" /></div>
                    <div class="form-group"><label for="paymentStatus">Status</label><input id="paymentStatus" class="app-input" type="number" value="1" min="1" max="6" /></div>
                    <div class="form-group form-group--span-2"><label for="paymentNotes">Notes</label><textarea id="paymentNotes" class="app-input"></textarea></div>
                </div>
                <div class="form-action-bar">
                    <button type="button" id="btnCreatePayment" class="btn btn--primary">Save Payment</button>
                </div>
                <div id="applicationPaymentReceiptPanel" class="timeline-panel mt-16 empty-state">Select "Receipt" from a payment to preview receipt details.</div>
                <div class="document-list mt-16">${listHtml}</div>
            </div>
        `);

        $('#btnCreatePayment').off('click').on('click', createPayment);
        $('.js-payment-verify').off('click').on('click', function () {
            changePaymentStatus(parseInt($(this).data('id'), 10), 3);
        });
        $('.js-payment-receipt').off('click').on('click', function () {
            viewReceipt(parseInt($(this).data('id'), 10));
        });
        $('.js-payment-refund').off('click').on('click', function () {
            createRefund(parseInt($(this).data('id'), 10), Number($(this).data('amount') || 0));
        });
    }

    function renderTimeline(application, histories) {
        $('#applicationTimelinePanel').html(`
            <div class="timeline-item">
                <div class="timeline-item__date">${formatDate(application.createdDate, 'dd MMM yyyy HH:mm')}</div>
                <div class="timeline-item__body">Application created</div>
            </div>
            ${application.offerReceivedDate ? `<div class="timeline-item"><div class="timeline-item__date">${formatDate(application.offerReceivedDate, 'dd MMM yyyy HH:mm')}</div><div class="timeline-item__body">Offer received</div></div>` : ''}
            ${application.enrollmentDate ? `<div class="timeline-item"><div class="timeline-item__date">${formatDate(application.enrollmentDate, 'dd MMM yyyy HH:mm')}</div><div class="timeline-item__body">Student enrolled</div></div>` : ''}
            ${(histories || []).map(item => `<div class="timeline-item"><div class="timeline-item__date">${formatDate(item.changedDate, 'dd MMM yyyy HH:mm')}</div><div class="timeline-item__body">Visa ${item.oldStatus} → ${item.newStatus}</div></div>`).join('')}
        `);
    }

    async function createScholarship() {
        const payload = {
            applicationId: currentApplication.applicationId,
            scholarshipName: $('#scholarshipName').val()?.trim() || '',
            scholarshipType: $('#scholarshipType').val()?.trim() || '',
            grantedAmount: Number($('#scholarshipAmount').val() || 0),
            currency: $('#scholarshipCurrency').val()?.trim() || 'BDT',
            scholarshipPercentage: $('#scholarshipPercentage').val() ? Number($('#scholarshipPercentage').val()) : null,
            confirmedDate: $('#scholarshipConfirmedDate').val() || null,
            expiryDate: $('#scholarshipExpiryDate').val() || null,
            status: Number($('#scholarshipStatus').val() || 2),
            notes: $('#scholarshipNotes').val()?.trim() || null,
            createdDate: new Date().toISOString(),
            createdBy: 1,
            updatedDate: null,
            updatedBy: null
        };
        await window.ApiClient.post(window.ApplicationModule.config.apiEndpoints.createScholarship, payload);
        window.AppToast?.success('Scholarship saved successfully.');
        await loadWorkspace(currentApplication.applicationId);
    }

    async function deleteScholarship(id) {
        if (!confirm('Delete this scholarship entry?')) {
            return;
        }
        await window.ApiClient.delete(window.ApplicationModule.config.apiEndpoints.deleteScholarship(id));
        window.AppToast?.success('Scholarship deleted successfully.');
        await loadWorkspace(currentApplication.applicationId);
    }

    async function createVisa() {
        const payload = {
            applicationId: currentApplication.applicationId,
            studentId: currentApplication.studentId,
            branchId: currentApplication.branchId,
            visaCountryId: Number($('#visaCountryId').val() || currentApplication.countryId || 0),
            embassyName: $('#visaEmbassyName').val()?.trim() || null,
            applicationRefNo: $('#visaApplicationRefNo').val()?.trim() || null,
            status: Number($('#visaStatus').val() || 3),
            submittedDate: $('#visaSubmittedDate').val() || null,
            biometricDate: $('#visaBiometricDate').val() || null,
            interviewDate: $('#visaInterviewDate').val() || null,
            decisionDate: $('#visaDecisionDate').val() || null,
            expiryDate: $('#visaExpiryDate').val() || null,
            refusalReason: null,
            notes: $('#visaNotes').val()?.trim() || null,
            isDeleted: false,
            createdDate: new Date().toISOString(),
            createdBy: 1,
            updatedDate: null,
            updatedBy: null
        };
        await window.ApiClient.post(window.ApplicationModule.config.apiEndpoints.createVisa, payload);
        window.AppToast?.success('Visa application saved successfully.');
        await loadWorkspace(currentApplication.applicationId);
    }

    async function changeVisaStatus(visaApplicationId, newStatus) {
        const payload = {
            visaApplicationId,
            newStatus,
            changedBy: 1,
            notes: newStatus === 7 ? 'Approved from application workspace' : 'Refused from application workspace',
            refusalReason: newStatus === 8 ? prompt('Enter refusal reason') : null
        };
        if (newStatus === 8 && !payload.refusalReason) {
            return;
        }
        await window.ApiClient.post(window.ApplicationModule.config.apiEndpoints.changeVisaStatus, payload);
        window.AppToast?.success('Visa status updated successfully.');
        await loadWorkspace(currentApplication.applicationId);
    }

    async function createPayment() {
        const payload = {
            studentId: currentApplication.studentId,
            applicationId: currentApplication.applicationId,
            branchId: currentApplication.branchId,
            paymentType: Number($('#paymentType').val() || 1),
            amount: Number($('#paymentAmount').val() || 0),
            currency: $('#paymentCurrency').val()?.trim() || 'BDT',
            exchangeRate: Number($('#paymentExchangeRate').val() || 1),
            paymentDate: $('#paymentDate').val() || new Date().toISOString(),
            paymentMethod: $('#paymentMethod').val()?.trim() || null,
            bankName: $('#paymentBankName').val()?.trim() || null,
            transactionRef: $('#paymentTransactionRef').val()?.trim() || null,
            status: Number($('#paymentStatus').val() || 1),
            receivedBy: 1,
            verifiedBy: null,
            notes: $('#paymentNotes').val()?.trim() || null,
            isDeleted: false,
            createdDate: new Date().toISOString(),
            createdBy: 1,
            updatedDate: null,
            updatedBy: null
        };
        await window.ApiClient.post(window.ApplicationModule.config.apiEndpoints.createPayment, payload);
        window.AppToast?.success('Payment saved successfully.');
        await loadWorkspace(currentApplication.applicationId);
    }

    async function changePaymentStatus(studentPaymentId, newStatus) {
        const payload = {
            studentPaymentId,
            newStatus,
            changedBy: 1,
            notes: 'Updated from application workspace'
        };
        await window.ApiClient.post(window.ApplicationModule.config.apiEndpoints.changePaymentStatus, payload);
        window.AppToast?.success('Payment status updated successfully.');
        await loadWorkspace(currentApplication.applicationId);
    }

    async function viewReceipt(studentPaymentId) {
        const response = await window.ApiClient.get(window.ApplicationModule.config.apiEndpoints.paymentReceipt(studentPaymentId));
        const receipt = response?.data || {};
        $('#applicationPaymentReceiptPanel').html(`
            <div class="review-section">
                <h3>Receipt Preview</h3>
                <div class="review-item"><span class="review-label">Receipt No</span><span class="review-value">${receipt.receiptNo || '-'}</span></div>
                <div class="review-item"><span class="review-label">Amount</span><span class="review-value">${receipt.amount || 0} ${receipt.currency || ''}</span></div>
                <div class="review-item"><span class="review-label">BDT</span><span class="review-value">${receipt.amountBdt || 0}</span></div>
                <div class="review-item"><span class="review-label">Payment Date</span><span class="review-value">${formatDate(receipt.paymentDate)}</span></div>
                <div class="review-item"><span class="review-label">Method</span><span class="review-value">${receipt.paymentMethod || '-'}</span></div>
                <div class="review-item"><span class="review-label">Reference</span><span class="review-value">${receipt.transactionRef || '-'}</span></div>
            </div>
        `);
    }

    async function createRefund(studentPaymentId, maxAmount) {
        const refundAmount = Number(prompt(`Enter refund amount (max ${maxAmount})`) || 0);
        if (!refundAmount) {
            return;
        }
        const reason = prompt('Enter refund reason') || null;
        const payload = {
            paymentId: studentPaymentId,
            refundAmount,
            refundDate: new Date().toISOString(),
            refundMethod: 'Manual',
            approvedBy: 1,
            reason,
            status: 1,
            processedDate: null,
            createdDate: new Date().toISOString(),
            createdBy: 1,
            updatedDate: null,
            updatedBy: null
        };
        await window.ApiClient.post(window.ApplicationModule.config.apiEndpoints.createRefund, payload);
        window.AppToast?.success('Refund request saved successfully.');
        await loadWorkspace(currentApplication.applicationId);
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
            const applicationResponse = await window.ApiClient.get(window.ApplicationModule.config.apiEndpoints.read(applicationId));
            currentApplication = applicationResponse?.data || {};
            if (!currentApplication.applicationId) {
                return;
            }

            const [conditionsResponse, documentsResponse, scholarshipsResponse, impactResponse, visasResponse, paymentsResponse, checklistResponse] = await Promise.all([
                window.ApiClient.get(window.ApplicationModule.config.apiEndpoints.conditionsByApplication(applicationId)),
                window.ApiClient.get(window.ApplicationModule.config.apiEndpoints.documentsByApplication(applicationId)),
                window.ApiClient.get(window.ApplicationModule.config.apiEndpoints.scholarshipsByApplication(applicationId)),
                window.ApiClient.get(window.ApplicationModule.config.apiEndpoints.scholarshipImpact(applicationId)),
                window.ApiClient.get(window.ApplicationModule.config.apiEndpoints.visasByApplication(applicationId)),
                window.ApiClient.get(window.ApplicationModule.config.apiEndpoints.paymentsByApplication(applicationId)),
                window.ApiClient.get(window.ApplicationModule.config.apiEndpoints.checklistsByStudent(currentApplication.studentId))
            ]);

            const visas = visasResponse?.data || [];
            const payments = paymentsResponse?.data || [];
            const visaHistories = visas.length
                ? (await window.ApiClient.get(window.ApplicationModule.config.apiEndpoints.visaStatusHistoryByVisaApplication(visas[0].visaApplicationId)))?.data || []
                : [];

            const refundEntries = await Promise.all((payments || []).map(async function (item) {
                const response = await window.ApiClient.get(window.ApplicationModule.config.apiEndpoints.refundsByPayment(item.studentPaymentId));
                return [item.studentPaymentId, response?.data || []];
            }));
            refundMap = Object.fromEntries(refundEntries);

            renderBasic(currentApplication);
            renderConditions(conditionsResponse?.data || []);
            renderDocuments(documentsResponse?.data || []);
            renderScholarships(scholarshipsResponse?.data || [], impactResponse?.data || {});
            renderVisa(visas, visaHistories, checklistResponse?.data || []);
            renderPayments(payments);
            renderTimeline(currentApplication, visaHistories);
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
            if (currentApplication?.applicationId) {
                loadWorkspace(currentApplication.applicationId);
            }
        });
    }

    $(document).ready(bindGridSelection);
})();
