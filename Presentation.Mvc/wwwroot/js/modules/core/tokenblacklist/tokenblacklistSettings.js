(function () {
    'use strict';

    window.TokenBlacklistModule = window.TokenBlacklistModule || {};

    window.TokenBlacklistModule.config = {
        moduleTitle: 'Token Blacklist',
        pluralTitle: 'Token Blacklists',
        idField: 'tokenId',
        displayField: 'token',
        dom: {
            grid: '#tokenBlacklistGrid',
            window: '#tokenBlacklistWindow',
            form: '#tokenBlacklistForm',
            addButton: '#btnAddTokenBlacklist',
            refreshButton: '#btnRefreshTokenBlacklist',
            saveButton: '#btnSaveTokenBlacklist',
            cancelButton: '#btnCancelTokenBlacklist'
        },
        apiEndpoints: {
            summary: `${window.AppConfig.apiBaseUrl}/core/systemadmin/token-blacklist-summary`,
            create: `${window.AppConfig.apiBaseUrl}/core/systemadmin/token-blacklist`,
            update: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/token-blacklist/${id}`; },
            delete: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/token-blacklist/${id}`; },
            read: function (id) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/token-blacklist/${id}`; },
            cleanup: `${window.AppConfig.apiBaseUrl}/core/systemadmin/token-blacklist/cleanup`,
            check: `${window.AppConfig.apiBaseUrl}/core/systemadmin/token-blacklist/check`
        },
        gridOptions: {
            pageSize: 20,
            sortable: true,
            filterable: true,
            height: 560,
            pageable: { refresh: true, pageSizes: [10, 20, 50, 100], buttonCount: 5 }
        },
        windowOptions: { width: '920px' },
        grid: {
            columns: [
                { field: 'tokenId', title: 'Token Id', width: 220 },
                { field: 'token', title: 'Token', width: 260, kind: 'multiline' },
                { field: 'expiryDate', title: 'Expiry Date', width: 170 },
                { field: 'createdAt', title: 'Created At', width: 170 }
            ]
        },
        form: {
            fields: [
                { name: 'tokenId', label: 'Token Id', type: 'hidden' },
                { name: 'token', label: 'Token', type: 'textarea', required: true, wide: true, maxLength: 8000, placeholder: 'Paste JWT token' },
                { name: 'tokenHash', label: 'Token Hash', type: 'textarea', wide: true, maxLength: 8000, placeholder: 'Leave blank to auto-generate server-side' },
                { name: 'expiryDate', label: 'Expiry Date', type: 'date', required: true },
                { name: 'createdAt', label: 'Created At', type: 'date' }
            ],
            buildPayload: function (values, state) {
                function normalizeString(value) { return value === null || value === undefined || String(value).trim() === '' ? null : String(value).trim(); }
                return {
                    tokenId: values.tokenId || state.currentRecord?.tokenId || null,
                    token: normalizeString(values.token),
                    tokenHash: normalizeString(values.tokenHash),
                    expiryDate: values.expiryDate || null,
                    createdAt: values.createdAt || state.currentRecord?.createdAt || new Date().toISOString()
                };
            }
        }
    };

    window.TokenBlacklistModule.config.moduleRef = window.TokenBlacklistModule;
    window.TokenBlacklistModule.config.form.fields.forEach(function (field) { field.moduleRef = window.TokenBlacklistModule; });

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () { window.location.href = '/Account/Login'; }, 1500);
            return;
        }

        window.TokenBlacklistModule.Summary?.init?.();
        window.TokenBlacklistModule.Details?.init?.();

        $(window.TokenBlacklistModule.config.dom.addButton).on('click', function () { window.TokenBlacklistModule.Details?.openAddForm?.(); });
        $(window.TokenBlacklistModule.config.dom.refreshButton).on('click', function () { window.TokenBlacklistModule.Summary?.refreshGrid?.(); });
        $('#btnCleanupExpired').on('click', window.TokenBlacklistModule.cleanupExpiredTokens);
        $('#btnCheckToken').on('click', window.TokenBlacklistModule.checkTokenStatus);
    });

    window.TokenBlacklistModule.cleanupExpiredTokens = async function () {
        window.AppLoader?.show('Cleaning expired tokens...');
        try {
            const response = await window.ApiClient.post(window.TokenBlacklistModule.config.apiEndpoints.cleanup, {});
            if (response?.success) {
                window.AppToast?.success(response.message || 'Expired tokens removed successfully');
                window.TokenBlacklistModule.Summary?.refreshGrid?.();
                return;
            }
            window.AppToast?.error(response?.message || 'Cleanup failed');
        } catch (error) {
            window.AppToast?.error(error?.message || 'Cleanup failed');
        } finally {
            window.AppLoader?.hide();
        }
    };

    window.TokenBlacklistModule.checkTokenStatus = async function () {
        const token = ($('#tokenCheckValue').val() || '').trim();
        if (!token) {
            window.AppToast?.warning('Please paste a token to check.');
            return;
        }

        window.AppLoader?.show('Checking token...');
        try {
            const response = await window.ApiClient.post(window.TokenBlacklistModule.config.apiEndpoints.check, token);
            const isBlacklisted = !!response?.data;
            $('#tokenCheckResult').text(isBlacklisted ? 'This token is already blacklisted.' : 'This token is not blacklisted.');
            window.AppToast?.info(response?.message || (isBlacklisted ? 'Token is blacklisted.' : 'Token is not blacklisted.'));
        } catch (error) {
            window.AppToast?.error(error?.message || 'Failed to check token');
        } finally {
            window.AppLoader?.hide();
        }
    };
})();
