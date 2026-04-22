// Authentication & Session Management
window.AuthManager = (() => {
    let bootstrapPromise = null;

    const applyTokenResponse = (response) => {
        if (response?.success && response.data?.accessToken) {
            window.ApiClient.setToken(response.data.accessToken);
            return true;
        }

        return false;
    };

    const bootstrapAccessToken = async (forceRefresh = false) => {
        if (!forceRefresh && window.ApiClient.getToken()) {
            return true;
        }

        try {
            const response = await window.ApiClient.post(
                window.AppConfig.endpoints.refreshToken,
                null,
                { suppressUnauthorizedRedirect: true }
            );

            return applyTokenResponse(response);
        } catch (error) {
            console.warn('Token bootstrap skipped:', error);
            return false;
        }
    };

    const ensureBootstrapPromise = (forceRefresh = false) => {
        if (!bootstrapPromise || forceRefresh) {
            bootstrapPromise = bootstrapAccessToken(forceRefresh);
        }

        return bootstrapPromise;
    };

    // Login function
    const login = async (loginId, password) => {
        try {
            const response = await window.ApiClient.post(window.AppConfig.endpoints.login, {
                loginId,
                password
            });

            if (applyTokenResponse(response)) {
                bootstrapPromise = Promise.resolve(true);
                console.log('Login successful, token stored in memory');
                return response.data;
            } else {
                throw new Error(response.message || window.AppConstants.messages.errors.loginFailed);
            }
        } catch (error) {
            console.error('Login error:', error);
            throw error;
        }
    };

    // Logout function
    const logout = async () => {
        try {
            // Call logout endpoint to invalidate refresh token
            await window.ApiClient.post(window.AppConfig.endpoints.logout);
        } catch (error) {
            console.error('Logout error:', error);
        } finally {
            // Clear token and redirect
            window.ApiClient.clearToken();
            bootstrapPromise = Promise.resolve(false);
            window.location.href = '/Account/Login';
        }
    };

    // Refresh token function
    const refreshToken = async () => {
        try {
            const response = await window.ApiClient.post(
                window.AppConfig.endpoints.refreshToken,
                null,
                { suppressUnauthorizedRedirect: true }
            );

            if (applyTokenResponse(response)) {
                bootstrapPromise = Promise.resolve(true);
                console.log('Token refreshed successfully');
                return true;
            }
            bootstrapPromise = Promise.resolve(false);
            return false;
        } catch (error) {
            console.error('Token refresh failed:', error);
            bootstrapPromise = Promise.resolve(false);
            return false;
        }
    };

    // Check if user is authenticated
    const isAuthenticated = () => {
        return window.ApiClient.getToken() !== null;
    };

    // Get current token (for debugging/testing only)
    const getCurrentToken = () => {
        return window.ApiClient.getToken();
    };

    bootstrapPromise = ensureBootstrapPromise();

    return {
        login,
        logout,
        refreshToken,
        isAuthenticated,
        getCurrentToken,
        ready: (forceRefresh = false) => ensureBootstrapPromise(forceRefresh)
    };
})();
