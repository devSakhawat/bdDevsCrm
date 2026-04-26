(function () {
    'use strict';

    window.QueryAnalyzerModule = window.QueryAnalyzerModule || {};
    let grid = null;

    function init() {
        if (!window.kendo || !$.fn.kendoGrid) {
            window.AppToast?.error('Kendo UI assets are required for Query Analyzer.');
            return;
        }

        grid = $(window.QueryAnalyzerModule.config.dom.grid).kendoGrid({
            dataSource: new kendo.data.DataSource({
                data: [],
                pageSize: 20,
                schema: {
                    model: {
                        id: 'reportHeaderId',
                        fields: {
                            reportHeaderId: { type: 'number' },
                            reportHeader: { type: 'string' },
                            reportTitle: { type: 'string' },
                            queryTypeName: { type: 'string' },
                            orderByColumn: { type: 'string' },
                            isActive: { type: 'number' }
                        }
                    }
                }
            }),
            height: 560,
            sortable: true,
            filterable: true,
            pageable: { refresh: true, pageSizes: [10, 20, 50, 100], buttonCount: 5 },
            resizable: true,
            reorderable: true,
            scrollable: true,
            noRecords: { template: '<div class="grid-empty-state">No report queries available.</div>' },
            columns: [
                { field: 'reportHeaderId', title: 'ID', width: 90, filterable: false },
                { field: 'reportHeader', title: 'Report Header', width: 220 },
                { field: 'reportTitle', title: 'Report Title', width: 240 },
                { field: 'queryTypeName', title: 'Query Type', width: 160 },
                { field: 'orderByColumn', title: 'Order By', width: 160 },
                {
                    field: 'isActive',
                    title: 'Status',
                    width: 110,
                    filterable: false,
                    template: function (item) {
                        return item.isActive ? '<span class="badge badge--success">Active</span>' : '<span class="badge badge--warning">Inactive</span>';
                    }
                },
                {
                    title: 'Actions',
                    width: 150,
                    filterable: false,
                    sortable: false,
                    template: function (item) {
                        return `<button type="button" class="btn btn--table-secondary js-view-query" data-id="${item.reportHeaderId}">View</button>`;
                    }
                }
            ],
            dataBound: function () {
                $(window.QueryAnalyzerModule.config.dom.grid).find('.js-view-query').off('click').on('click', function () {
                    const id = parseInt($(this).attr('data-id'), 10);
                    const record = window.QueryAnalyzerModule.state.data.find(function (item) { return item.reportHeaderId === id; });
                    if (record) {
                        window.QueryAnalyzerModule.Details?.open?.(record);
                    }
                });
            }
        }).data('kendoGrid');

        reload();
    }

    async function reload() {
        window.AppLoader?.show('Loading report queries...');
        try {
            const response = await window.ApiClient.get(window.QueryAnalyzerModule.config.apiEndpoints.reports);
            const items = response?.success && Array.isArray(response.data) ? response.data : [];
            window.QueryAnalyzerModule.state.data = items;
            grid.dataSource.data(items);
            if (!response?.success) {
                window.AppToast?.error(response?.message || 'Failed to load reports');
            }
        } catch (error) {
            window.AppToast?.error(error?.message || 'Failed to load reports');
        } finally {
            window.AppLoader?.hide();
        }
    }

    function applySearch(term) {
        if (!grid) {
            return;
        }
        const value = (term || '').trim().toLowerCase();
        if (!value) {
            grid.dataSource.data(window.QueryAnalyzerModule.state.data);
            return;
        }
        const filtered = window.QueryAnalyzerModule.state.data.filter(function (item) {
            return [item.reportHeader, item.reportTitle, item.queryTypeName, item.orderByColumn]
                .filter(Boolean)
                .some(function (field) { return String(field).toLowerCase().includes(value); });
        });
        grid.dataSource.data(filtered);
    }

    window.QueryAnalyzerModule.Summary = { init: init, reload: reload, applySearch: applySearch };
})();
