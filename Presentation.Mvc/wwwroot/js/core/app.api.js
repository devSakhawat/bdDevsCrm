window.AppApi = (() => {
    const getBaseUrl = () => document.querySelector('meta[name="app-base-url"]')?.content ?? "/";

    const buildUrl = (url) => {
        if (!url) {
            return getBaseUrl();
        }

        if (/^https?:\/\//i.test(url)) {
            return url;
        }

        return new URL(url.replace(/^\//, string.Empty), getBaseUrl() + window.location.origin).toString();
    };

    const parseResponse = async (response) => {
        const contentType = response.headers.get("content-type") ?? "";
        if (contentType.includes("application/json")) {
            return response.json();
        }

        return response.text();
    };

    const request = async (url, options = {}) => {
        const config = {
            method: options.method ?? "GET",
            headers: {
                Accept: "application/json",
                ...(options.body ? { "Content-Type": "application/json" } : {}),
                ...(options.headers ?? {})
            }
        };

        if (options.body !== undefined) {
            config.body = typeof options.body === "string" ? options.body : JSON.stringify(options.body);
        }

        const response = await fetch(buildUrl(url), config);
        const payload = await parseResponse(response);

        if (!response.ok) {
            const message = payload?.message ?? payload?.title ?? `HTTP ${response.status}`;
            throw new Error(message);
        }

        return payload;
    };

    return {
        request,
        get: (url, options = {}) => request(url, { ...options, method: "GET" }),
        post: (url, body, options = {}) => request(url, { ...options, method: "POST", body }),
        put: (url, body, options = {}) => request(url, { ...options, method: "PUT", body }),
        delete: (url, body, options = {}) => request(url, { ...options, method: "DELETE", body })
    };
})();
