(function () {
    'use strict';

    const apiRoot = window.AppConfig?.apiRouteBaseUrl || '/bdDevs-crm';
    const baseUrl = window.AppConfig?.apiBaseUrl || '';

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

    let currentReport = null;
    let resultGrid = null;

    async function loadReportList() {
        const container = document.getElementById('crmReportList');
        if (!container) return;

        try {
            const res = await fetchJson(`${baseUrl}/api/report-builders`);
            if (res.success && res.data) {
                container.innerHTML = '';
                res.data.filter(r => r.isActive).forEach(function (report) {
                    const item = document.createElement('div');
                    item.className = 'report-list-item';
                    item.style.cssText = 'padding:.65rem 1rem;cursor:pointer;border-bottom:1px solid #eee;';
                    item.textContent = report.reportTitle || report.reportHeader || `Report #${report.reportHeaderId}`;
                    item.dataset.id = report.reportHeaderId;
                    item.addEventListener('click', function () {
                        document.querySelectorAll('.report-list-item--active').forEach(el => el.classList.remove('report-list-item--active'));
                        item.classList.add('report-list-item--active');
                        item.style.background = '#f0f4ff';
                        currentReport = report;
                        showReportHeader(report);
                    });
                    container.appendChild(item);
                });
            }
        } catch (e) {
            console.warn('Could not load report list:', e);
            if (container) container.innerHTML = '<p style="padding:1rem;color:#999;">Unable to load reports.</p>';
        }
    }

    function showReportHeader(report) {
        const titleEl = document.getElementById('crmReportTitle');
        const headerEl = document.getElementById('crmReportHeader');
        const placeholderEl = document.getElementById('crmReportPlaceholder');
        if (titleEl) titleEl.textContent = report.reportTitle || report.reportHeader;
        if (headerEl) headerEl.style.display = '';
        if (placeholderEl) placeholderEl.style.display = 'none';

        if (resultGrid) {
            $('#crmReportResultGrid').data('kendoGrid')?.destroy?.();
            document.getElementById('crmReportResultGrid').innerHTML = '';
            resultGrid = null;
        }
    }

    async function runReport() {
        if (!currentReport) {
            window.AppToast?.warning('Please select a report first.');
            return;
        }

        try {
            const res = await fetchJson(`${baseUrl}/api/report-builder-execute`, {
                method: 'POST',
                body: JSON.stringify({ reportHeaderId: currentReport.reportHeaderId })
            });

            if (res.success && Array.isArray(res.data) && res.data.length > 0) {
                renderDynamicGrid(res.data);
            } else {
                document.getElementById('crmReportResultGrid').innerHTML =
                    '<p style="padding:1rem;color:#999;">No data returned for this report.</p>';
            }
        } catch (e) {
            console.warn('Could not run report:', e);
            window.AppToast?.error('Failed to run report. Please try again.');
        }
    }

    function renderDynamicGrid(data) {
        const gridEl = document.getElementById('crmReportResultGrid');
        if (!gridEl) return;

        const columns = Object.keys(data[0]).map(function (key) {
            return { field: key, title: key.replace(/([A-Z])/g, ' $1').trim(), width: 150 };
        });

        resultGrid = $(gridEl).kendoGrid({
            dataSource: new kendo.data.DataSource({
                data: data,
                pageSize: 20
            }),
            columns: columns,
            sortable: true,
            filterable: true,
            pageable: { pageSizes: [10, 20, 50, 100] },
            height: 450
        }).data('kendoGrid');
    }

    function exportCsv() {
        if (!resultGrid) {
            window.AppToast?.warning('Run a report first before exporting.');
            return;
        }
        resultGrid.saveAsExcel?.() || window.AppToast?.info('Export not supported for this grid.');
    }

    document.addEventListener('DOMContentLoaded', function () {
        loadReportList();

        document.getElementById('btnRunReport')?.addEventListener('click', runReport);
        document.getElementById('btnExportCsv')?.addEventListener('click', exportCsv);
        document.getElementById('btnRefreshCrmReport')?.addEventListener('click', loadReportList);
    });
})();
