window.AppToast = (() => {
    const container = () => document.getElementById("toast-container");

    const show = (message, type = "info", timeout = 3000) => {
        const host = container();
        if (!host) {
            return;
        }

        const toast = document.createElement("div");
        toast.className = `app-toast app-toast--${type}`;
        toast.textContent = message;
        host.appendChild(toast);
        window.setTimeout(() => toast.remove(), timeout);
    };

    return {
        show,
        success: (message) => show(message, "success"),
        error: (message) => show(message, "error", 5000),
        warning: (message) => show(message, "warning", 4000),
        info: (message) => show(message, "info")
    };
})();
