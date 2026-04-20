window.AppForm = (() => {
    const clearErrors = (form) => {
        form.querySelectorAll(".field-error-msg").forEach((message) => message.remove());
        form.querySelectorAll(".field-error").forEach((item) => item.classList.remove("field-error"));
    };

    const validate = (form) => {
        clearErrors(form);
        const fields = form.querySelectorAll("[required]");
        let firstInvalid = null;

        fields.forEach((field) => {
            if (field.value.trim()) {
                return;
            }

            field.classList.add("field-error");
            const feedback = document.createElement("div");
            feedback.className = "field-error-msg";
            feedback.textContent = "This field is required.";
            field.closest(".form-group")?.appendChild(feedback);
            firstInvalid ??= field;
        });

        firstInvalid?.focus();
        return !firstInvalid;
    };

    const init = ({ formId, saveUrl, onSuccess, onError }) => {
        const form = document.getElementById(formId);
        if (!form) {
            return;
        }

        form.addEventListener("submit", async (event) => {
            event.preventDefault();

            if (!validate(form)) {
                return;
            }

            const formData = Object.fromEntries(new FormData(form).entries());

            try {
                window.AppLoader?.show("Saving...");
                const response = await window.AppApi.post(saveUrl, formData);
                window.AppToast?.success(response?.message ?? "Saved successfully.");
                onSuccess?.(response);
            } catch (error) {
                window.AppToast?.error(error.message);
                onError?.(error);
            } finally {
                window.AppLoader?.hide();
            }
        });
    };

    return { init, validate };
})();
