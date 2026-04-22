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

    const bootstrapAccessToken = async () => {
        if (window.ApiClient.getToken()) {
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

    // Login function
    const login = async (loginId, password) => {
        try {
            const response = await window.ApiClient.post(window.AppConfig.endpoints.login, {
                loginId,
                password
            });

            if (applyTokenResponse(response)) {
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
                console.log('Token refreshed successfully');
                return true;
            }
            return false;
        } catch (error) {
            console.error('Token refresh failed:', error);
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

    bootstrapPromise = bootstrapAccessToken();

    return {
        login,
        logout,
        refreshToken,
        isAuthenticated,
        getCurrentToken,
        ready: () => bootstrapPromise
    };
})();
