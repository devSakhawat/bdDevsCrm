(function () {
    'use strict';

    const apiRoot = window.AppConfig?.apiRouteBaseUrl || '/bdDevs-crm';

    function getToken() {
        return localStorage.getItem('authToken') || '';
    }

    async function fetchJson(url, options) {
        const response = await fetch(url, {
            ...options,
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${getToken()}`,
                ...(options?.headers || {})
            }
        });
        if (!response.ok) throw new Error(`HTTP ${response.status}`);
        return response.json();
    }

    function setKpiCard(elId, value) {
        const el = document.getElementById(elId);
        if (el) el.textContent = value !== null && value !== undefined ? value.toLocaleString() : '—';
    }

    let kpiGrid = null;

    async function loadKpiData() {
        const year = parseInt(document.getElementById('kpiYear')?.value || new Date().getFullYear());
        const monthVal = document.getElementById('kpiMonth')?.value;

        try {
            const res = await fetchJson(`${apiRoot}/crm-branch-target-summary`, {
                method: 'POST',
                body: JSON.stringify({
                    page: 1,
                    pageSize: 200,
                    sortColumn: 'Year',
                    sortDirection: 'desc',
                    filterColumn: monthVal ? 'Month' : 'Year',
                    filterValue: monthVal ? String(monthVal) : String(year)
                })
            });

            if (res.success && res.data?.items) {
                const items = res.data.items.filter(x => x.year === year && (!monthVal || x.month === parseInt(monthVal)));

                const totals = items.reduce((acc, x) => {
                    acc.lead += x.leadTarget || 0;
                    acc.conversion += x.conversionTarget || 0;
                    acc.application += x.applicationTarget || 0;
                    acc.enrolment += x.enrolmentTarget || 0;
                    acc.revenue += x.revenueTarget || 0;
                    return acc;
                }, { lead: 0, conversion: 0, application: 0, enrolment: 0, revenue: 0 });

                setKpiCard('kpiLeadTarget', totals.lead);
                setKpiCard('kpiConversionTarget', totals.conversion);
                setKpiCard('kpiApplicationTarget', totals.application);
                setKpiCard('kpiEnrolmentTarget', totals.enrolment);
                setKpiCard('kpiRevenueTarget', totals.revenue.toFixed(2));

                if (kpiGrid) {
                    kpiGrid.dataSource.data(items);
                    kpiGrid.refresh();
                }
            }
        } catch (e) {
            console.warn('Could not load KPI data:', e);
        }
    }

    function initKpiBranchTargetGrid() {
        const gridEl = document.getElementById('kpiBranchTargetGrid');
        if (!gridEl) return;

        kpiGrid = $(gridEl).kendoGrid({
            dataSource: new kendo.data.DataSource({ data: [], pageSize: 20 }),
            sortable: true,
            pageable: { pageSizes: [10, 20, 50] },
            height: 400,
            columns: [
                { field: 'branchTargetId', title: '#', width: 60 },
                { field: 'branchId', title: 'Branch ID', width: 90 },
                { field: 'year', title: 'Year', width: 80 },
                { field: 'month', title: 'Month', width: 80 },
                { field: 'leadTarget', title: 'Lead Target', width: 110, format: '{0:n0}' },
                { field: 'conversionTarget', title: 'Conversion', width: 110, format: '{0:n0}' },
                { field: 'applicationTarget', title: 'Application', width: 110, format: '{0:n0}' },
                { field: 'enrolmentTarget', title: 'Enrolment', width: 110, format: '{0:n0}' },
                { field: 'revenueTarget', title: 'Revenue Target', width: 130, format: '{0:n2}' }
            ]
        }).data('kendoGrid');
    }

    document.addEventListener('DOMContentLoaded', function () {
        const yearInput = document.getElementById('kpiYear');
        if (yearInput) yearInput.value = new Date().getFullYear();

        initKpiBranchTargetGrid();
        loadKpiData();

        document.getElementById('btnLoadKpi')?.addEventListener('click', loadKpiData);
    });
})();
