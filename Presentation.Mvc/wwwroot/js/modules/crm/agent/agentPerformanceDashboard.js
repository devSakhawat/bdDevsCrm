(function () {
    'use strict';

    const apiRoot = (function () {
        const base = window.AppConfig?.apiRouteBaseUrl || window.AppConfig?.apiBaseUrl || '';
        return base.replace(/\/$/, '');
    })();

    function getAuthHeaders() {
        const token = window.AuthManager?.getToken?.() || localStorage.getItem('authToken') || '';
        return {
            'Content-Type': 'application/json',
            'Authorization': token ? `Bearer ${token}` : ''
        };
    }

    function formatCurrency(value) {
        return Number(value || 0).toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    }

    function showAgentDetail(agentId, agentName) {
        fetch(`${apiRoot}/crm-agent-performance/${agentId}`, { headers: getAuthHeaders() })
            .then(r => r.json())
            .then(json => {
                if (!json.success) { window.AppToast?.error(json.message || 'Failed to load agent performance.'); return; }
                const d = json.data;
                const panel = document.getElementById('agentDetailPanel');
                const title = document.getElementById('agentDetailTitle');
                const content = document.getElementById('agentDetailContent');
                if (!panel || !title || !content) return;
                title.textContent = `${agentName} — Performance Details`;
                content.innerHTML = [
                    { label: 'Assigned Leads', value: d.assignedLeadCount, color: '#0284C7' },
                    { label: 'Total Commissions', value: d.totalCommissions, color: '#1E5FA8' },
                    { label: 'Enrolled Students', value: d.enrolledStudentCount, color: '#16A34A' },
                    { label: 'Pending Commissions', value: d.pendingCommissions, color: '#D97706' },
                    { label: 'Total Net Amount', value: formatCurrency(d.totalNetAmount), color: '#475569' },
                    { label: 'Total Paid Amount', value: formatCurrency(d.totalPaidAmount), color: '#16A34A' },
                    { label: 'Outstanding Amount', value: formatCurrency(d.outstandingAmount), color: '#DC2626' }
                ].map(k => `
                    <div class="kpi-card" style="border-top:3px solid ${k.color}">
                        <span class="kpi-card__label">${k.label}</span>
                        <span class="kpi-card__value" style="color:${k.color}">${k.value}</span>
                    </div>
                `).join('');
                panel.style.display = 'block';
                panel.scrollIntoView({ behavior: 'smooth', block: 'start' });
            })
            .catch(() => { window.AppToast?.error('Network error loading agent performance.'); });
    }

    function initGrid() {
        if (!$('#agentPerformanceGrid').length) return;
        $('#agentPerformanceGrid').kendoGrid({
            dataSource: {
                transport: {
                    read: function (options) {
                        fetch(`${apiRoot}/crm-agents`, { headers: getAuthHeaders() })
                            .then(r => r.json())
                            .then(json => {
                                if (json.success) { options.success(json.data || []); }
                                else { options.error(json); window.AppToast?.error(json.message || 'Failed to load agents.'); }
                            })
                            .catch(err => { options.error(err); window.AppToast?.error('Network error.'); });
                    }
                },
                schema: {
                    model: {
                        fields: {
                            agentId: { type: 'number' },
                            agentName: { type: 'string' },
                            agentCode: { type: 'string' },
                            commissionRate: { type: 'number' },
                            isActive: { type: 'boolean' }
                        }
                    }
                }
            },
            columns: [
                { field: 'agentId', title: '#', width: 60 },
                { field: 'agentName', title: 'Agent Name', width: 200 },
                { field: 'agentCode', title: 'Code', width: 120 },
                { field: 'commissionRate', title: 'Commission Rate (%)', width: 150, format: '{0:n4}' },
                {
                    field: 'isActive', title: 'Status', width: 90,
                    template: function (d) {
                        return d.isActive
                            ? '<span style="color:#16A34A;font-weight:600">Active</span>'
                            : '<span style="color:#DC2626;font-weight:600">Inactive</span>';
                    }
                },
                {
                    title: 'Actions', width: 160, filterable: false, sortable: false,
                    template: function (d) {
                        return `<button class="btn btn--secondary btn--sm" onclick="window.AgentPerformanceDashboard.viewPerformance(${d.agentId}, '${(d.agentName || '').replace(/'/g, '\\\'') }')">View Performance</button>`;
                    }
                }
            ],
            sortable: true,
            filterable: { mode: 'row' },
            pageable: { pageSize: 20, pageSizes: [10, 20, 50, 100], buttonCount: 5 },
            resizable: true,
            height: 500
        });
    }

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () { window.location.href = '/Account/Login'; }, 1500);
            return;
        }

        initGrid();

        $('#btnRefreshPerformance').on('click', function () {
            $('#agentPerformanceGrid').data('kendoGrid')?.dataSource.read();
            $('#agentDetailPanel').hide();
        });
    });

    window.AgentPerformanceDashboard = {
        viewPerformance: showAgentDetail
    };
})();
