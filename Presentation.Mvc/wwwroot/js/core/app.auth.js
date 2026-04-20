// Authentication & Session Management
window.AuthManager = (() => {
    // Login function
    const login = async (loginId, password) => {
        try {
            const response = await window.ApiClient.post(window.AppConfig.endpoints.login, {
                loginId,
                password
            });

            if (response.success && response.data) {
                // Store access token in memory (XSS protection)
                window.ApiClient.setToken(response.data.accessToken);

                // RefreshToken is automatically stored in HTTP-only cookie by server
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
            // RefreshToken is sent automatically via HTTP-only cookie
            const response = await window.ApiClient.post(window.AppConfig.endpoints.refreshToken);

            if (response.success && response.data) {
                window.ApiClient.setToken(response.data.accessToken);
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

    return {
        login,
        logout,
        refreshToken,
        isAuthenticated,
        getCurrentToken
    };
})();
