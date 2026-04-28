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
        if (el) el.textContent = value !== null && value !== undefined ? value : '—';
    }

    async function loadSecurityKpis() {
        const today = new Date().toISOString().split('T')[0];
        try {
            const res = await fetchJson(`${apiRoot}/audit-log-summary`, {
                method: 'POST',
                body: JSON.stringify({
                    page: 1,
                    pageSize: 1000,
                    sortColumn: 'Timestamp',
                    sortDirection: 'desc',
                    filterColumn: '',
                    filterValue: ''
                })
            });

            if (res.success && res.data?.items) {
                const items = res.data.items;
                const todayItems = items.filter(x => x.timestamp && x.timestamp.startsWith(today));
                const failed = todayItems.filter(x => !x.success);
                const uniqueUsers = new Set(todayItems.map(x => x.username)).size;
                const crmItems = todayItems.filter(x => x.module && x.module.toLowerCase().includes('crm'));

                setKpiCard('kpiLoginsToday', todayItems.length);
                setKpiCard('kpiFailedLogins', failed.length);
                setKpiCard('kpiUniqueUsers', uniqueUsers);
                setKpiCard('kpiCrmActions', crmItems.length);
            }
        } catch (e) {
            console.warn('Could not load security KPIs:', e);
        }
    }

    function initSecurityEventsGrid() {
        const gridEl = document.getElementById('crmSecurityEventsGrid');
        if (!gridEl) return;

        const dataSource = new kendo.data.DataSource({
            transport: {
                read: function (options) {
                    fetchJson(`${apiRoot}/audit-log-summary`, {
                        method: 'POST',
                        body: JSON.stringify({
                            page: options.data.page || 1,
                            pageSize: 20,
                            sortColumn: 'Timestamp',
                            sortDirection: 'desc',
                            filterColumn: '',
                            filterValue: ''
                        })
                    })
                        .then(function (res) {
                            if (res.success) {
                                const failed = (res.data?.items || []).filter(x => !x.success);
                                options.success({ data: failed, total: failed.length });
                            } else {
                                options.error(res);
                            }
                        })
                        .catch(function (err) { options.error(err); });
                }
            },
            schema: { data: 'data', total: 'total' },
            serverPaging: false,
            pageSize: 20
        });

        $(gridEl).kendoGrid({
            dataSource: dataSource,
            sortable: true,
            pageable: { refresh: true, pageSizes: [10, 20, 50] },
            height: 400,
            columns: [
                { field: 'username', title: 'User', width: 130 },
                { field: 'action', title: 'Action', width: 100 },
                { field: 'endpoint', title: 'Endpoint', width: 220 },
                { field: 'ipAddress', title: 'IP Address', width: 120 },
                { field: 'statusCode', title: 'Status', width: 80 },
                { field: 'errorMessage', title: 'Error', width: 250 },
                { field: 'timestamp', title: 'Time', width: 150, format: '{0:yyyy-MM-dd HH:mm:ss}' }
            ]
        });
    }

    function bindRefreshButton() {
        document.getElementById('btnRefreshSecurity')?.addEventListener('click', function () {
            loadSecurityKpis();
            const grid = $('#crmSecurityEventsGrid').data('kendoGrid');
            grid?.dataSource?.read();
            grid?.refresh();
        });
    }

    document.addEventListener('DOMContentLoaded', function () {
        loadSecurityKpis();
        initSecurityEventsGrid();
        bindRefreshButton();
    });
})();
