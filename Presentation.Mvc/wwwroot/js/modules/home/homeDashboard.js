document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll("[data-demo-action]").forEach((button) => {
        button.addEventListener("click", () => {
            const action = button.getAttribute("data-demo-action");

            switch (action) {
                case "refresh-dashboard":
                    window.AppLoader?.show("Refreshing dashboard...");
                    window.setTimeout(() => {
                        window.AppLoader?.hide();
                        window.AppToast?.success("Dashboard refreshed.");
                    }, 700);
                    break;
                case "export-report":
                    window.AppToast?.info("Export workflow placeholder is ready.");
                    break;
                case "save-form":
                    window.AppToast?.success("Form action bar is wired and ready for API integration.");
                    break;
                default:
                    window.AppToast?.info("Demo interaction is ready for the selected action.");
                    break;
            }
        });
    });

    document.getElementById("formDashboardFilters")?.addEventListener("submit", (event) => {
        event.preventDefault();
        window.AppToast?.success("Filter bar submitted. Hook this form to a module endpoint next.");
    });

    document.getElementById("btnResetFilters")?.addEventListener("click", () => {
        document.getElementById("formDashboardFilters")?.reset();
        window.AppToast?.info("Filters reset.");
    });

    document.querySelectorAll("[data-row-action]").forEach((button) => {
        button.addEventListener("click", () => {
            const action = button.getAttribute("data-row-action");
            if (action === "delete") {
                window.AppModal?.confirm(
                    "Delete employee record?",
                    "This is a demo confirmation modal for destructive actions.",
                    () => window.AppToast?.warning("Delete action confirmed.")
                );
                return;
            }

            window.AppToast?.info(`${action} action is ready for module-specific logic.`);
        });
    });
});
