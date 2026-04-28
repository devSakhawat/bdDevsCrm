(function () {
    'use strict';

    const apiRoot = (function () {
        const base = window.AppConfig?.apiRouteBaseUrl || window.AppConfig?.apiBaseUrl || '';
        return base.replace(/\/$/, '');
    })();

    const ENDPOINTS = {
        dashboard: `${apiRoot}/crm-commission-dashboard`,
        agingReport: `${apiRoot}/crm-commission-aging-report`,
        agentSummary: `${apiRoot}/crm-commission-agent-summary`
    };

    function getAuthHeaders() {
        const token = window.AuthManager?.getToken?.() || localStorage.getItem('authToken') || '';
        return {
            'Content-Type': 'application/json',
            'Authorization': token ? `Bearer ${token}` : ''
        };
    }

    function statusLabel(status) {
        const map = { 1: 'Pending', 2: 'Due', 3: 'Invoiced', 4: 'Paid', 5: 'Disputed', 6: 'Written Off' };
        return map[status] || 'Unknown';
    }

    function formatCurrency(value) {
        return Number(value || 0).toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    }

    async function loadDashboard() {
        try {
            const res = await fetch(ENDPOINTS.dashboard, { headers: getAuthHeaders() });
            const json = await res.json();
            if (!json.success) { window.AppToast?.error(json.message || 'Failed to load dashboard.'); return; }
            renderKpiCards(json.data);
        } catch (e) {
            window.AppToast?.error('Network error loading dashboard.');
        }
    }

    function renderKpiCards(data) {
        const container = document.getElementById('dashboardKpiCards');
        if (!container) return;
        const kpis = [
            { label: 'Pending', count: data.pendingCount, color: '#D97706' },
            { label: 'Due', count: data.dueCount, color: '#DC2626' },
            { label: 'Invoiced', count: data.invoicedCount, color: '#0284C7' },
            { label: 'Paid', count: data.paidCount, color: '#16A34A' },
            { label: 'Disputed', count: data.disputedCount, color: '#7C3AED' },
            { label: 'Written Off', count: data.writtenOffCount, color: '#475569' }
        ];
        const amountCards = [
            { label: 'Total Net', amount: data.totalNetAmount, currency: 'Foreign' },
            { label: 'Total Net (BDT)', amount: data.totalNetAmountBdt, currency: 'BDT' },
            { label: 'Total Paid', amount: data.totalPaidAmount, currency: 'Foreign' },
            { label: 'Outstanding', amount: data.totalOutstandingAmount, currency: 'Foreign' }
        ];

        container.innerHTML = kpis.map(k => `
            <div class="kpi-card" style="border-top:3px solid ${k.color}">
                <span class="kpi-card__label">${k.label}</span>
                <span class="kpi-card__value" style="color:${k.color}">${k.count}</span>
            </div>
        `).join('') + amountCards.map(a => `
            <div class="kpi-card kpi-card--amount">
                <span class="kpi-card__label">${a.label}</span>
                <span class="kpi-card__value">${formatCurrency(a.amount)}</span>
                <span class="kpi-card__sub">${a.currency}</span>
            </div>
        `).join('');
    }

    function initAgingGrid() {
        if (!$('#agingReportGrid').length) return;
        $('#agingReportGrid').kendoGrid({
            dataSource: {
                transport: {
                    read: function (options) {
                        fetch(ENDPOINTS.agingReport, { headers: getAuthHeaders() })
                            .then(r => r.json())
                            .then(json => {
                                if (json.success) { options.success(json.data || []); }
                                else { options.error(json); window.AppToast?.error(json.message || 'Failed to load aging report.'); }
                            })
                            .catch(err => { options.error(err); window.AppToast?.error('Network error.'); });
                    }
                },
                schema: {
                    model: {
                        fields: {
                            commissionId: { type: 'number' },
                            studentNameSnapshot: { type: 'string' },
                            universityNameSnapshot: { type: 'string' },
                            invoiceNo: { type: 'string' },
                            dueDate: { type: 'date' },
                            agingDays: { type: 'number' },
                            netAmount: { type: 'number' },
                            paidAmount: { type: 'number' },
                            outstandingAmount: { type: 'number' },
                            status: { type: 'number' }
                        }
                    }
                }
            },
            columns: [
                { field: 'commissionId', title: '#', width: 60 },
                { field: 'invoiceNo', title: 'Invoice No', width: 150 },
                { field: 'studentNameSnapshot', title: 'Student', width: 160 },
                { field: 'universityNameSnapshot', title: 'University', width: 160 },
                { field: 'dueDate', title: 'Due Date', width: 110, format: '{0:dd MMM yyyy}' },
                { field: 'agingDays', title: 'Aging (Days)', width: 110 },
                { field: 'netAmount', title: 'Net Amt', width: 110, format: '{0:n2}' },
                { field: 'paidAmount', title: 'Paid Amt', width: 110, format: '{0:n2}' },
                { field: 'outstandingAmount', title: 'Outstanding', width: 120, format: '{0:n2}' },
                {
                    field: 'status', title: 'Status', width: 110,
                    template: function (d) { return statusLabel(d.status); }
                }
            ],
            sortable: true,
            filterable: { mode: 'row' },
            pageable: { pageSize: 20, pageSizes: [10, 20, 50], buttonCount: 5 },
            resizable: true,
            height: 400
        });
    }

    function initAgentSummaryGrid() {
        if (!$('#agentSummaryGrid').length) return;
        $('#agentSummaryGrid').kendoGrid({
            dataSource: {
                transport: {
                    read: function (options) {
                        fetch(ENDPOINTS.agentSummary, { headers: getAuthHeaders() })
                            .then(r => r.json())
                            .then(json => {
                                if (json.success) { options.success(json.data || []); }
                                else { options.error(json); window.AppToast?.error(json.message || 'Failed to load agent summary.'); }
                            })
                            .catch(err => { options.error(err); window.AppToast?.error('Network error.'); });
                    }
                },
                schema: {
                    model: {
                        fields: {
                            agentId: { type: 'number' },
                            agentName: { type: 'string' },
                            totalCommissions: { type: 'number' },
                            enrolledStudentCount: { type: 'number' },
                            pendingCommissions: { type: 'number' },
                            grossAmount: { type: 'number' },
                            netAmount: { type: 'number' },
                            paidAmount: { type: 'number' },
                            outstandingAmount: { type: 'number' }
                        }
                    }
                }
            },
            columns: [
                { field: 'agentId', title: '#', width: 60 },
                { field: 'agentName', title: 'Agent', width: 180 },
                { field: 'totalCommissions', title: 'Total', width: 80 },
                { field: 'enrolledStudentCount', title: 'Enrolled', width: 90 },
                { field: 'pendingCommissions', title: 'Pending', width: 90 },
                { field: 'grossAmount', title: 'Gross', width: 110, format: '{0:n2}' },
                { field: 'netAmount', title: 'Net', width: 110, format: '{0:n2}' },
                { field: 'paidAmount', title: 'Paid', width: 110, format: '{0:n2}' },
                { field: 'outstandingAmount', title: 'Outstanding', width: 120, format: '{0:n2}' }
            ],
            sortable: true,
            filterable: { mode: 'row' },
            pageable: { pageSize: 20, pageSizes: [10, 20, 50], buttonCount: 5 },
            resizable: true,
            height: 400
        });
    }

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () { window.location.href = '/Account/Login'; }, 1500);
            return;
        }

        loadDashboard();
        initAgingGrid();
        initAgentSummaryGrid();

        $('#btnRefreshDashboard').on('click', function () {
            loadDashboard();
            $('#agingReportGrid').data('kendoGrid')?.dataSource.read();
            $('#agentSummaryGrid').data('kendoGrid')?.dataSource.read();
        });
    });
})();
