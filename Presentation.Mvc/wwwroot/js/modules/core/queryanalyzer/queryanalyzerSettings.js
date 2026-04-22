(function () {
    'use strict';

    window.QueryAnalyzerModule = window.QueryAnalyzerModule || {};
    window.QueryAnalyzerModule.state = { data: [] };
    window.QueryAnalyzerModule.config = {
        dom: {
            grid: '#queryAnalyzerGrid',
            refreshButton: '#btnRefreshQueryAnalyzer',
            searchInput: '#queryAnalyzerSearch',
            window: '#queryAnalyzerWindow',
            details: '#queryAnalyzerDetails'
        },
        apiEndpoints: {
            reports: `${window.AppConfig.apiBaseUrl}/core/systemadmin/customized-report`
        }
    };

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () { window.location.href = '/Account/Login'; }, 1500);
            return;
        }

        window.QueryAnalyzerModule.Details?.init?.();
        window.QueryAnalyzerModule.Summary?.init?.();
        $(window.QueryAnalyzerModule.config.dom.refreshButton).on('click', function () { window.QueryAnalyzerModule.Summary?.reload?.(); });
        $(window.QueryAnalyzerModule.config.dom.searchInput).on('input', function () { window.QueryAnalyzerModule.Summary?.applySearch?.($(this).val()); });
    });
})();
