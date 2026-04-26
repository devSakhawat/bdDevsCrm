window.AppCore = (() => {
    const STORAGE_KEY = "bddevscrm.sidebar.collapsed";

    const syncSidebarState = () => {
        const collapsed = window.localStorage.getItem(STORAGE_KEY) === "true";
        document.body.classList.toggle("sidebar-collapsed", collapsed && window.innerWidth >= 768);
    };

    const toggleSidebar = () => {
        if (window.innerWidth < 768) {
            document.body.classList.toggle("sidebar-open");
            return;
        }

        const collapsed = !document.body.classList.contains("sidebar-collapsed");
        document.body.classList.toggle("sidebar-collapsed", collapsed);
        window.localStorage.setItem(STORAGE_KEY, String(collapsed));
    };

    const bindSidebarGroups = () => {
        document.querySelectorAll("[data-nav-toggle]").forEach((button) => {
            button.addEventListener("click", () => {
                const panel = button.nextElementSibling;
                const expanded = button.getAttribute("aria-expanded") === "true";
                button.setAttribute("aria-expanded", String(!expanded));
                panel?.classList.toggle("is-open", !expanded);
            });
        });
    };

    const bindGlobalActions = () => {
        document.getElementById("sidebarToggle")?.addEventListener("click", toggleSidebar);
        document.getElementById("globalSearch")?.addEventListener("keydown", (event) => {
            if (event.key !== "Enter") {
                return;
            }

            event.preventDefault();
            window.AppToast?.info(`Search ready: ${event.target.value || "type a keyword"}`);
        });
    };

    const init = () => {
        syncSidebarState();
        bindSidebarGroups();
        bindGlobalActions();
    };

    document.addEventListener("DOMContentLoaded", init);
    window.addEventListener("resize", syncSidebarState);

    return { init };
})();
