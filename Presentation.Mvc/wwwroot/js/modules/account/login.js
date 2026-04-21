// Login Page JavaScript
(function() {
    'use strict';

    const $form = $('#loginForm');
    const $loginBtn = $('#loginButton');
    const $loginIdField = $('#loginId');
    const $passwordField = $('#password');

    // Form validation
    function validateLoginForm() {
        let isValid = true;

        const loginId = $loginIdField.val().trim();
        const password = $passwordField.val().trim();

        // Clear previous errors
        clearAllErrors();

        // Validate Login ID
        if (!loginId) {
            showError('loginId', window.AppConstants.messages.errors.required);
            isValid = false;
        }

        // Validate Password
        if (!password) {
            showError('password', window.AppConstants.messages.errors.required);
            isValid = false;
        }

        return isValid;
    }

    // Show field error
    function showError(fieldId, message) {
        const $field = $(`#${fieldId}`);
        const $errorSpan = $(`#err_${fieldId}`);

        $field.closest('.form-group').addClass('has-error');
        $errorSpan.html(`<i class="fa fa-exclamation-circle"></i> ${message}`);
        $errorSpan.css('display', 'block');
    }

    // Clear field error
    function clearError(fieldId) {
        const $field = $(`#${fieldId}`);
        const $errorSpan = $(`#err_${fieldId}`);

        $field.closest('.form-group').removeClass('has-error');
        $errorSpan.css('display', 'none').html('');
    }

    // Clear all errors
    function clearAllErrors() {
        $('.form-group').removeClass('has-error');
        $('.field-error-msg').css('display', 'none').html('');
    }

    // Form submit handler
    $form.on('submit', async function(e) {
        e.preventDefault();

        // Validate form
        if (!validateLoginForm()) {
            window.AppToast?.warning(window.AppConstants.messages.errors.validationFailed);
            return;
        }

        const loginId = $loginIdField.val().trim();
        const password = $passwordField.val().trim();

        // Show loading state
        $loginBtn.prop('disabled', true).text('Logging in...');
        window.AppLoader?.show();

        try {
            // Call login API
            const user = await window.AuthManager.login(loginId, password);

            // Success
            window.AppToast?.success(window.AppConstants.messages.success.loginSuccess);

            // Initialize session manager after successful login
            if (window.SessionManager) {
                window.SessionManager.init();
                console.log('Session manager initialized after login');
            }

            // Redirect to dashboard after short delay
            setTimeout(() => {
                window.location.href = '/Home/Index';
            }, 500);

        } catch (error) {
            // Error handling
            const errorMessage = error.message || window.AppConstants.messages.errors.loginFailed;
            window.AppToast?.error(errorMessage);

            // Re-enable button
            $loginBtn.prop('disabled', false).text('Login');

            // Clear password field on error
            $passwordField.val('').focus();
        } finally {
            window.AppLoader?.hide();
        }
    });

    // Clear error on input
    $loginIdField.on('input', function() {
        if ($(this).closest('.form-group').hasClass('has-error')) {
            clearError('loginId');
        }
    });

    $passwordField.on('input', function() {
        if ($(this).closest('.form-group').hasClass('has-error')) {
            clearError('password');
        }
    });

    // Focus on first field on load
    $(document).ready(function() {
        $loginIdField.focus();
    });
})();
