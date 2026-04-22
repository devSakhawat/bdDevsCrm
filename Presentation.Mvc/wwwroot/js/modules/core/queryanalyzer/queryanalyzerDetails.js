(function () {
    'use strict';

    window.QueryAnalyzerModule = window.QueryAnalyzerModule || {};
    let detailWindow = null;

    function init() {
        detailWindow = $(window.QueryAnalyzerModule.config.dom.window).kendoWindow({
            width: '960px',
            title: 'Query Details',
            modal: true,
            visible: false,
            actions: ['Close']
        }).data('kendoWindow');
    }

    function open(record) {
        if (!detailWindow || !record) {
            return;
        }

        const html = `
            <article class="info-card">
                <div class="info-card__header">
                    <h2 class="info-card__title">${escapeHtml(record.reportTitle || 'Report')}</h2>
                    <p class="info-card__text">${escapeHtml(record.reportHeader || '')}</p>
                </div>
                <div class="card-grid card-grid--2">
                    <div class="stat-card"><span class="stat-card__label">Query Type</span><span class="stat-card__value">${escapeHtml(record.queryTypeName || '-')}</span></div>
                    <div class="stat-card"><span class="stat-card__label">Order By</span><span class="stat-card__value">${escapeHtml(record.orderByColumn || '-')}</span></div>
                </div>
                <div class="form-group form-group--span-2">
                    <label>Query Text</label>
                    <pre class="code-block">${escapeHtml(record.queryText || '')}</pre>
                </div>
            </article>`;

        $(window.QueryAnalyzerModule.config.dom.details).html(html);
        detailWindow.center().open();
    }

    function escapeHtml(value) {
        return String(value ?? '')
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;');
    }

    window.QueryAnalyzerModule.Details = { init: init, open: open };
})();
