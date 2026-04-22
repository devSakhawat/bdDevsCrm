(() => {
    const apiBaseUrl = 'https://localhost:7001/api';
    const apiOrigin = new URL(apiBaseUrl, window.location.origin).origin;
    const apiRouteBaseUrl = `${apiOrigin}/bdDevs-crm`;

    window.AppConfig = {
        apiBaseUrl,
        apiRouteBaseUrl,
        timeout: 30000,
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        },
        auth: {
            tokenKey: 'accessToken',
            refreshTokenKey: 'refreshToken'
        },
        routes: {
            dashboard: '/Home/Index',
            apiPrefixes: ['/bdDevs-crm', '/api']
        },
        endpoints: {
            // Shared/auth endpoints use the bdDevs-crm base route.
            login: `${apiRouteBaseUrl}/login`,
            refreshToken: `${apiRouteBaseUrl}/refresh-token`,
            logout: `${apiRouteBaseUrl}/logout`,
            userInfo: `${apiRouteBaseUrl}/user-info`,
            menusByUserPermission: `${apiRouteBaseUrl}/menus-user-permission`,

            // Existing feature endpoints still use the legacy relative pattern.
            countries: '/core/systemadmin/countries',
            countrySummary: '/core/systemadmin/country-summary',
            createCountry: '/core/systemadmin/create-country',
            updateCountry: (id) => `/core/systemadmin/update-country/${id}`,
            deleteCountry: (id) => `/core/systemadmin/delete-country/${id}`
        }
    };
})();
