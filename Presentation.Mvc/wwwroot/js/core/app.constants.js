// Application Constants
window.AppConstants = {
    // Messages
    messages: {
        success: {
            created: 'Record created successfully!',
            updated: 'Record updated successfully!',
            deleted: 'Record deleted successfully!',
            saved: 'Saved successfully!',
            loginSuccess: 'Login successful!'
        },
        errors: {
            network: 'Network error. Please check your connection.',
            unauthorized: 'Session expired. Please login again.',
            serverError: 'Server error. Please try again later.',
            validationFailed: 'Please fix the errors before submitting.',
            loginFailed: 'Login failed. Please check your credentials.',
            required: 'This field is required.',
            invalidEmail: 'Please enter a valid email address.',
            minLength: (length) => `Minimum ${length} characters required.`
        },
        warnings: {
            unsavedChanges: 'You have unsaved changes. Are you sure you want to leave?',
            deleteConfirm: 'Are you sure you want to delete this record?'
        }
    },

    // Cache keys
    cache: {
        countriesDdl: 'countries_ddl',
        userInfo: 'user_info',
        menuItems: 'menu_items'
    },

    // Grid defaults
    grid: {
        pageSize: 20,
        pageSizes: [10, 20, 50, 100]
    },

    // Form validation
    validation: {
        minPasswordLength: 6,
        maxFileSize: 5242880, // 5MB in bytes
        allowedImageTypes: ['image/jpeg', 'image/png', 'image/gif']
    }
};
