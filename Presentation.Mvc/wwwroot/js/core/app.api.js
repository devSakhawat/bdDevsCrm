// Enhanced API Client with Authentication Support
window.ApiClient = (() => {
    let accessToken = null;  // In-memory token storage for security

    // Token management
    const setToken = (token) => {
        accessToken = token;
    };

    const getToken = () => {
        return accessToken;
    };

    const clearToken = () => {
        accessToken = null;
    };

    // Build full API URL
    const buildApiUrl = (endpoint) => {
        if (endpoint.startsWith('http://') || endpoint.startsWith('https://')) {
            return endpoint;
        }

        if (endpoint.startsWith('/bdDevs-crm/') && window.AppConfig?.apiRouteBaseUrl) {
            return `${window.AppConfig.apiRouteBaseUrl}${endpoint.replace('/bdDevs-crm', '')}`;
        }

        return `${window.AppConfig.apiBaseUrl}${endpoint}`;
    };

    // Main request function with auth support
    const request = async (url, options = {}) => {
        const config = {
            method: options.method || 'GET',
            credentials: options.credentials || 'include',
            headers: {
                ...window.AppConfig.headers,
                ...(accessToken ? { 'Authorization': `Bearer ${accessToken}` } : {}),
                ...options.headers
            }
        };

        if (options.body) {
            config.body = JSON.stringify(options.body);
        }

        try {
            const response = await fetch(buildApiUrl(url), config);

            if (response.status === 401) {
                clearToken();

                if (typeof options.onUnauthorized === 'function') {
                    options.onUnauthorized(response);
                }

                if (options.suppressUnauthorizedRedirect) {
                    return undefined;
                }

                window.AppToast?.error(window.AppConstants?.messages.errors.unauthorized || 'Session expired');
                setTimeout(() => {
                    window.location.href = '/Account/Login';
                }, 1500);
                return undefined;
            }

            const data = await response.json();

            if (!response.ok) {
                const errorMessage = data.message || data.title || `HTTP ${response.status}`;
                throw new Error(errorMessage);
            }

            return data;  // Returns ApiResponse<T> structure
        } catch (error) {
            console.error('API Error:', error);
            throw error;
        }
    };

    // Convenience methods
    const get = async (url, options = {}) => {
        return request(url, { ...options, method: 'GET' });
    };

    const post = async (url, body, options = {}) => {
        return request(url, { ...options, method: 'POST', body });
    };

    const put = async (url, body, options = {}) => {
        return request(url, { ...options, method: 'PUT', body });
    };

    const del = async (url, options = {}) => {
        return request(url, { ...options, method: 'DELETE' });
    };

    return {
        setToken,
        getToken,
        clearToken,
        request,
        get,
        post,
        put,
        delete: del
    };
})();

// Keep backward compatibility with existing AppApi
if (!window.AppApi) {
    window.AppApi = window.ApiClient;
}
