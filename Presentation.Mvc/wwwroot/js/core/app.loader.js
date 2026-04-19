window.AppLoader = (() => {
    const overlay = () => document.getElementById("loading-overlay");
    const text = () => document.querySelector(".loading-overlay__text");

    const show = (message = "Loading...") => {
        const element = overlay();
        const messageText = text();
        if (!element) {
            return;
        }

        if (messageText) {
            messageText.textContent = message;
        }

        element.classList.remove("is-hidden");
        element.setAttribute("aria-hidden", "false");
    };

    const hide = () => {
        const element = overlay();
        if (!element) {
            return;
        }

        element.classList.add("is-hidden");
        element.setAttribute("aria-hidden", "true");
    };

    return { show, hide };
})();
