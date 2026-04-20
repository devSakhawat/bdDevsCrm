window.AppModal = (() => {
    const close = (targetId) => {
        const modal = document.getElementById(targetId);
        if (!modal) {
            return;
        }

        modal.hidden = true;
        modal.setAttribute("aria-hidden", "true");
    };

    const open = (targetId) => {
        const modal = document.getElementById(targetId);
        if (!modal) {
            return;
        }

        modal.hidden = false;
        modal.setAttribute("aria-hidden", "false");
    };

    const confirm = (message, detail, onConfirm) => {
        document.getElementById("modalConfirmMessage").textContent = message;
        document.getElementById("modalConfirmDetail").textContent = detail ?? "";

        const actionButton = document.getElementById("modalConfirmAction");
        actionButton.onclick = () => {
            onConfirm?.();
            close("modalConfirm");
        };

        open("modalConfirm");
    };

    const openContent = (title, html) => {
        document.getElementById("modalDynamicTitle").textContent = title;
        document.getElementById("modalDynamicBody").innerHTML = html;
        open("modalDynamic");
    };

    document.addEventListener("click", (event) => {
        const closeTarget = event.target.closest("[data-modal-close]");
        if (!closeTarget) {
            return;
        }

        close("modalConfirm");
        close("modalDynamic");
    });

    return { confirm, openContent, close, open };
})();
