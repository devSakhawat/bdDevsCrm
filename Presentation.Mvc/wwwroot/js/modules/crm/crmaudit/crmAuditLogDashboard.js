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

    function initAuditLogGrid() {
        const gridEl = document.getElementById('crmAuditLogGrid');
        if (!gridEl) return;

        const dataSource = new kendo.data.DataSource({
            transport: {
                read: function (options) {
                    const gridOptions = {
                        page: options.data.page || 1,
                        pageSize: options.data.pageSize || 20,
                        sortColumn: options.data.sort?.[0]?.field || 'Timestamp',
                        sortDirection: options.data.sort?.[0]?.dir || 'desc',
                        filterColumn: '',
                        filterValue: ''
                    };
                    fetchJson(`${apiRoot}/audit-log-summary`, {
                        method: 'POST',
                        body: JSON.stringify(gridOptions)
                    })
                        .then(function (res) {
                            if (res.success) {
                                options.success({ data: res.data?.items || [], total: res.data?.totalCount || 0 });
                            } else {
                                options.error(res);
                            }
                        })
                        .catch(function (err) { options.error(err); });
                }
            },
            schema: {
                data: 'data',
                total: 'total',
                model: {
                    fields: {
                        auditId: { type: 'number' },
                        username: { type: 'string' },
                        action: { type: 'string' },
                        entityType: { type: 'string' },
                        entityId: { type: 'string' },
                        endpoint: { type: 'string' },
                        module: { type: 'string' },
                        ipAddress: { type: 'string' },
                        success: { type: 'boolean' },
                        statusCode: { type: 'number' },
                        timestamp: { type: 'date' },
                        durationMs: { type: 'number' }
                    }
                }
            },
            serverPaging: true,
            serverSorting: true,
            pageSize: 20
        });

        $(gridEl).kendoGrid({
            dataSource: dataSource,
            sortable: true,
            filterable: false,
            pageable: {
                refresh: true,
                pageSizes: [10, 20, 50, 100],
                buttonCount: 5
            },
            height: 580,
            columns: [
                { field: 'auditId', title: '#', width: 65 },
                { field: 'username', title: 'User', width: 130 },
                { field: 'action', title: 'Action', width: 100 },
                { field: 'entityType', title: 'Entity', width: 140 },
                { field: 'entityId', title: 'Entity ID', width: 90 },
                { field: 'endpoint', title: 'Endpoint', width: 220 },
                { field: 'module', title: 'Module', width: 100 },
                { field: 'ipAddress', title: 'IP Address', width: 120 },
                {
                    field: 'success', title: 'Result', width: 80,
                    template: function (row) {
                        return row.success
                            ? '<span class="badge badge--success">OK</span>'
                            : '<span class="badge badge--danger">Fail</span>';
                    }
                },
                { field: 'statusCode', title: 'Status', width: 80 },
                { field: 'durationMs', title: 'ms', width: 70 },
                { field: 'timestamp', title: 'Time', width: 150, format: '{0:yyyy-MM-dd HH:mm:ss}' }
            ]
        });
    }

    function bindRefreshButton() {
        document.getElementById('btnRefreshAuditLog')?.addEventListener('click', function () {
            const grid = $('#crmAuditLogGrid').data('kendoGrid');
            grid?.dataSource?.read();
            grid?.refresh();
        });
    }

    document.addEventListener('DOMContentLoaded', function () {
        initAuditLogGrid();
        bindRefreshButton();
    });
})();
