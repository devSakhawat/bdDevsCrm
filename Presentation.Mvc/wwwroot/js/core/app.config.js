// API Configuration
window.AppConfig = {
    apiBaseUrl: 'https://localhost:7001/api',  // Presentation.Api URL
    timeout: 30000,  // 30 seconds
    headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json'
    },
    auth: {
        tokenKey: 'accessToken',        // In-memory storage key
        refreshTokenKey: 'refreshToken'  // HTTP-only cookie
    },
    endpoints: {
        // Authentication
        login: '/authentication/login',
        refreshToken: '/authentication/refresh-token',
        logout: '/authentication/logout',

        // Country (first module)
        countries: '/core/systemadmin/countries',
        countrySummary: '/core/systemadmin/country-summary',
        createCountry: '/core/systemadmin/create-country',
        updateCountry: (id) => `/core/systemadmin/update-country/${id}`,
        deleteCountry: (id) => `/core/systemadmin/delete-country/${id}`
    }
};
