# Frontend Design - bdDevsCrm

## 📖 Overview

এই ডকুমেন্টে **bdDevsCrm** প্রজেক্টের frontend architecture, UI/UX design system, এবং layout structure বিস্তারিত বর্ণনা করা হয়েছে।

---

## 🎨 Technology Stack

### Frontend Framework
- **ASP.NET Core MVC** (.NET 8/10)
- **Razor Pages** for server-side rendering
- **jQuery** for DOM manipulation
- **Kendo UI 2024 Q4** for enterprise UI components

### API Communication
- **JavaScript Fetch API** (NOT jQuery Ajax)
- **Promise-based** async/await pattern
- **Generic API client** for all HTTP requests

### CSS Framework
- **Bootstrap 5** (optional, for grid system)
- **Custom CSS** for specific designs
- **Kendo UI Themes** for consistent styling

---

## 📐 Layout Structure

### Overall Page Layout

```
┌─────────────────────────────────────────────────────┐
│              Header (70px - Fixed)                  │
├──────────┬──────────────────────────────────────────┤
│          │                                          │
│ Sidebar  │        Main Content Area                 │
│ (260px)  │        (Adaptive Width)                  │
│          │                                          │
│ Toggle   │                                          │
│ Button   │                                          │
│          │                                          │
│          │                                          │
├──────────┴──────────────────────────────────────────┤
│              Footer (20px - Fixed)                  │
└─────────────────────────────────────────────────────┘
```

### Layout Dimensions

| Component | Size | Behavior |
|-----------|------|----------|
| **Header** | 70px height | Fixed, sticky at top |
| **Footer** | 20px height | Fixed at bottom |
| **Sidebar** | 260px width | Collapsible to ~50px (icon only) |
| **Main Content** | Remaining space | Adaptive, responsive |

---

## 🏗️ Header Design

### Header Structure

```html
<header class="main-header" style="height: 70px;">
    <div class="header-container">
        <!-- Logo Section -->
        <div class="logo-section">
            <img src="/images/logo.png" alt="bdDevsCRM" />
            <span class="app-title">bdDevsCRM</span>
        </div>

        <!-- Navigation Items -->
        <nav class="header-nav">
            <ul class="nav-items">
                <li><a href="/dashboard">Dashboard</a></li>
                <li><a href="/reports">Reports</a></li>
            </ul>
        </nav>

        <!-- User Profile Section -->
        <div class="user-section">
            <div class="notifications">
                <i class="k-icon k-i-bell"></i>
                <span class="badge">3</span>
            </div>
            <div class="user-profile dropdown">
                <img src="/images/user-avatar.jpg" class="avatar" />
                <span class="user-name">John Doe</span>
                <div class="dropdown-menu">
                    <a href="/profile">Profile</a>
                    <a href="/settings">Settings</a>
                    <a href="/logout">Logout</a>
                </div>
            </div>
        </div>
    </div>
</header>
```

### Header CSS

```css
.main-header {
    height: 70px;
    background: #2c3e50;
    color: white;
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    z-index: 1000;
    box-shadow: 0 2px 4px rgba(0,0,0,0.1);
}

.header-container {
    display: flex;
    justify-content: space-between;
    align-items: center;
    height: 100%;
    padding: 0 20px;
}

.logo-section {
    display: flex;
    align-items: center;
    gap: 10px;
}

.user-section {
    display: flex;
    align-items: center;
    gap: 20px;
}
```

---

## 📂 Sidebar Design

### Tree Structured Menu (Multi-level: 1 → n)

```html
<aside class="sidebar" id="sidebar">
    <!-- Toggle Button -->
    <button class="sidebar-toggle" onclick="toggleSidebar()">
        <i class="k-icon k-i-menu"></i>
    </button>

    <!-- Menu Tree -->
    <div class="menu-tree">
        <ul class="menu-level-1">
            <!-- Dashboard -->
            <li class="menu-item">
                <a href="/dashboard">
                    <i class="k-icon k-i-home"></i>
                    <span class="menu-text">Dashboard</span>
                </a>
            </li>

            <!-- User Management (with submenu) -->
            <li class="menu-item has-submenu">
                <a href="#" class="menu-toggle">
                    <i class="k-icon k-i-user"></i>
                    <span class="menu-text">User Management</span>
                    <i class="k-icon k-i-arrow-chevron-down expand-icon"></i>
                </a>
                <ul class="submenu menu-level-2">
                    <li><a href="/users">Users</a></li>
                    <li><a href="/roles">Roles</a></li>
                    <li><a href="/permissions">Permissions</a></li>
                </ul>
            </li>

            <!-- Settings (nested submenu) -->
            <li class="menu-item has-submenu">
                <a href="#" class="menu-toggle">
                    <i class="k-icon k-i-gear"></i>
                    <span class="menu-text">Settings</span>
                    <i class="k-icon k-i-arrow-chevron-down expand-icon"></i>
                </a>
                <ul class="submenu menu-level-2">
                    <li class="has-submenu">
                        <a href="#" class="menu-toggle">
                            <span>System Settings</span>
                            <i class="k-icon k-i-arrow-chevron-right"></i>
                        </a>
                        <ul class="submenu menu-level-3">
                            <li><a href="/settings/general">General</a></li>
                            <li><a href="/settings/email">Email</a></li>
                        </ul>
                    </li>
                    <li class="has-submenu">
                        <a href="#" class="menu-toggle">
                            <span>Security</span>
                            <i class="k-icon k-i-arrow-chevron-right"></i>
                        </a>
                        <ul class="submenu menu-level-3">
                            <li><a href="/security/password-policy">Password Policy</a></li>
                            <li><a href="/security/2fa">Two-Factor Auth</a></li>
                        </ul>
                    </li>
                </ul>
            </li>
        </ul>
    </div>
</aside>
```

### Sidebar States

#### Expanded State (260px)

```css
.sidebar {
    width: 260px;
    background: #34495e;
    height: calc(100vh - 70px); /* Full height minus header */
    position: fixed;
    left: 0;
    top: 70px;
    overflow-y: auto;
    transition: width 0.3s ease;
}

.sidebar .menu-text {
    display: inline-block;
}

.sidebar .submenu {
    display: none;
}

.sidebar .menu-item.active > .submenu {
    display: block;
}
```

#### Collapsed State (~50px)

```css
.sidebar.collapsed {
    width: 50px;
}

.sidebar.collapsed .menu-text {
    display: none;
}

.sidebar.collapsed .expand-icon {
    display: none;
}

.sidebar.collapsed .submenu {
    display: none;
}

/* Hover করলে temporary expanded menu দেখাবে */
.sidebar.collapsed .menu-item:hover .submenu {
    display: block;
    position: absolute;
    left: 50px;
    background: #34495e;
    min-width: 200px;
    box-shadow: 2px 0 5px rgba(0,0,0,0.2);
}
```

### Sidebar Toggle JavaScript

```javascript
function toggleSidebar() {
    const sidebar = document.getElementById('sidebar');
    const mainContent = document.getElementById('mainContent');

    sidebar.classList.toggle('collapsed');

    if (sidebar.classList.contains('collapsed')) {
        mainContent.style.marginLeft = '50px';
    } else {
        mainContent.style.marginLeft = '260px';
    }
}

// Submenu toggle
document.querySelectorAll('.menu-toggle').forEach(item => {
    item.addEventListener('click', function(e) {
        e.preventDefault();
        const parent = this.parentElement;
        parent.classList.toggle('active');
    });
});
```

---

## 🧩 Main Content Area

### Responsive Layout

```html
<main id="mainContent" class="main-content">
    <div class="content-wrapper">
        <!-- Breadcrumb -->
        <nav class="breadcrumb">
            <a href="/dashboard">Home</a> /
            <a href="/users">Users</a> /
            <span>User List</span>
        </nav>

        <!-- Page Title -->
        <div class="page-header">
            <h1>User Management</h1>
            <div class="page-actions">
                <button class="k-button k-button-primary">
                    <i class="k-icon k-i-plus"></i> Add User
                </button>
            </div>
        </div>

        <!-- Main Content -->
        <div class="page-content">
            @RenderBody()
        </div>
    </div>
</main>
```

### Main Content CSS

```css
.main-content {
    margin-left: 260px; /* Sidebar width */
    margin-top: 70px;   /* Header height */
    margin-bottom: 20px; /* Footer height */
    padding: 20px;
    transition: margin-left 0.3s ease;
    min-height: calc(100vh - 90px);
}

/* Sidebar collapsed হলে */
.sidebar.collapsed ~ .main-content {
    margin-left: 50px;
}

.content-wrapper {
    max-width: 100%;
    margin: 0 auto;
}

.page-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 20px;
}
```

---

## 🧾 Form Design Patterns

### 1️⃣ Inline Form (Grid-based)

**কখন ব্যবহার**: ছোট form (2-5 fields)

```html
<!-- Kendo Grid with inline editing -->
<div id="userGrid"></div>

<script>
$("#userGrid").kendoGrid({
    dataSource: {
        transport: {
            read: async (e) => {
                const data = await callApi('/api/users');
                e.success(data);
            },
            create: async (e) => {
                const result = await callApi('/api/users', 'POST', e.data);
                e.success(result);
            },
            update: async (e) => {
                const result = await callApi(`/api/users/${e.data.userId}`, 'PUT', e.data);
                e.success(result);
            },
            destroy: async (e) => {
                await callApi(`/api/users/${e.data.userId}`, 'DELETE');
                e.success();
            }
        },
        schema: {
            model: {
                id: "userId",
                fields: {
                    userId: { editable: false, nullable: true },
                    loginId: { validation: { required: true } },
                    email: { validation: { required: true, email: true } },
                    status: { type: "boolean" }
                }
            }
        },
        pageSize: 20
    },
    toolbar: ["create"],
    editable: "inline",
    columns: [
        { field: "loginId", title: "Login ID" },
        { field: "email", title: "Email" },
        { field: "status", title: "Active", template: "#= status ? 'Yes' : 'No' #" },
        { command: ["edit", "destroy"], title: "Actions", width: 200 }
    ],
    pageable: true,
    sortable: true,
    filterable: true
});
</script>
```

### 2️⃣ Modal Form (Popup)

**কখন ব্যবহার**: Medium size form (5-15 fields)

```html
<!-- Modal trigger button -->
<button onclick="openUserModal()" class="k-button k-button-primary">
    Add User
</button>

<!-- Kendo Window (Modal) -->
<div id="userModal"></div>

<script>
function openUserModal(userId = null) {
    const modal = $("#userModal").kendoWindow({
        title: userId ? "Edit User" : "Add User",
        modal: true,
        width: 600,
        height: 500,
        content: {
            url: userId ? `/users/edit/${userId}` : '/users/create'
        },
        close: function() {
            this.destroy();
        }
    }).data("kendoWindow");

    modal.center().open();
}

// Form submission
async function saveUser(formData) {
    try {
        const method = formData.userId ? 'PUT' : 'POST';
        const url = formData.userId
            ? `/api/users/${formData.userId}`
            : '/api/users';

        const response = await callApi(url, method, formData);

        if (response.success) {
            showNotification('User saved successfully', 'success');
            $("#userModal").data("kendoWindow").close();
            $("#userGrid").data("kendoGrid").dataSource.read();
        }
    } catch (error) {
        showNotification('Failed to save user: ' + error.message, 'error');
    }
}
</script>
```

### 3️⃣ Complex Tabbed Form (Multi-section)

**কখন ব্যবহার**: বড় form (15+ fields, multiple sections)

```html
<!-- Employee Form with Tabs -->
<div id="employeeForm">
    <ul>
        <li class="k-active">Personal Information</li>
        <li>Educational Information</li>
        <li>Language Skills</li>
        <li>Financial Information</li>
        <li>Other Details</li>
    </ul>

    <!-- Tab 1: Personal Information -->
    <div>
        <div class="form-section">
            <div class="form-row">
                <div class="form-group">
                    <label>Full Name <span class="required">*</span></label>
                    <input type="text" name="fullName" class="k-textbox" required />
                </div>
                <div class="form-group">
                    <label>Date of Birth</label>
                    <input type="date" name="dateOfBirth" class="k-textbox" />
                </div>
            </div>
            <div class="form-row">
                <div class="form-group">
                    <label>Gender</label>
                    <select name="gender" class="k-dropdown">
                        <option value="M">Male</option>
                        <option value="F">Female</option>
                        <option value="O">Other</option>
                    </select>
                </div>
                <div class="form-group">
                    <label>Phone Number</label>
                    <input type="tel" name="phone" class="k-textbox" />
                </div>
            </div>
        </div>
    </div>

    <!-- Tab 2: Educational Information -->
    <div>
        <div class="form-section" data-tab-index="1">
            <div id="educationGrid"></div>
            <button onclick="addEducation()" class="k-button">Add Education</button>
        </div>
    </div>

    <!-- Tab 3: Language Skills -->
    <div>
        <div class="form-section" data-tab-index="2">
            <!-- Language skills form -->
        </div>
    </div>

    <!-- Tab 4: Financial Information -->
    <div>
        <div class="form-section" data-tab-index="3">
            <!-- Financial info form -->
        </div>
    </div>

    <!-- Tab 5: Other Details -->
    <div>
        <div class="form-section" data-tab-index="4">
            <!-- Other details form -->
        </div>
    </div>
</div>

<script>
// Initialize TabStrip
$("#employeeForm").kendoTabStrip({
    animation: {
        open: { effects: "fadeIn", duration: 300 }
    },
    activate: function(e) {
        const tabIndex = $(e.item).index();
        loadTabData(tabIndex);
    }
});

// Lazy loading tab data
const loadedTabs = new Set();

async function loadTabData(tabIndex) {
    if (loadedTabs.has(tabIndex)) {
        return; // Already loaded
    }

    const employeeId = getEmployeeId();

    try {
        const data = await callApi(`/api/employees/${employeeId}/tab/${tabIndex}`);
        populateTabForm(tabIndex, data);
        loadedTabs.add(tabIndex);
    } catch (error) {
        console.error('Error loading tab data:', error);
    }
}
</script>
```

---

## 💾 Form Data Handling

### Temporary In-Memory Store

```javascript
// Global form data store
const formDataStore = new Map();

// Save form data to memory
function saveFormData(entityId, tabIndex, formData) {
    const key = `${entityId}_tab_${tabIndex}`;
    formDataStore.set(key, {
        data: formData,
        timestamp: new Date(),
        isDirty: true
    });
}

// Retrieve form data from memory
function getFormData(entityId, tabIndex) {
    const key = `${entityId}_tab_${tabIndex}`;
    return formDataStore.get(key);
}

// Auto-save interval (every 30 seconds)
setInterval(() => {
    formDataStore.forEach((value, key) => {
        if (value.isDirty) {
            autoSaveToServer(key, value.data)
                .then(() => {
                    value.isDirty = false;
                    console.log(`Auto-saved: ${key}`);
                })
                .catch(err => console.error('Auto-save failed:', err));
        }
    });
}, 30000);

// Submit all tabs data
async function submitCompleteForm(entityId) {
    const allTabsData = {};

    // Collect data from all tabs
    for (let i = 0; i < 5; i++) {
        const tabData = getFormData(entityId, i);
        if (tabData) {
            allTabsData[`tab${i}`] = tabData.data;
        }
    }

    try {
        const response = await callApi(`/api/employees/${entityId}`, 'PUT', allTabsData);

        if (response.success) {
            showNotification('Employee saved successfully', 'success');
            formDataStore.clear();
        }
    } catch (error) {
        showNotification('Failed to save: ' + error.message, 'error');
    }
}
```

---

## ✅ Validation Strategy

### Real-time Validation

```javascript
// Initialize Kendo Validator
const validator = $("#employeeForm").kendoValidator({
    rules: {
        required: function(input) {
            if (input.is("[required]")) {
                return $.trim(input.val()) !== "";
            }
            return true;
        },
        email: function(input) {
            if (input.is("[type=email]")) {
                return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(input.val());
            }
            return true;
        },
        minlength: function(input) {
            const minLength = input.data("min-length");
            if (minLength) {
                return input.val().length >= minLength;
            }
            return true;
        }
    },
    messages: {
        required: "This field is required",
        email: "Please enter a valid email address",
        minlength: "Minimum {0} characters required"
    }
}).data("kendoValidator");

// Validate on blur
$('input, select, textarea').on('blur', function() {
    validator.validateInput($(this));
});

// Validate before submit
function validateForm() {
    if (validator.validate()) {
        return true;
    } else {
        showNotification('Please fix validation errors', 'warning');
        return false;
    }
}
```

### Visual Feedback

```css
/* Invalid field styling */
.k-textbox.k-invalid,
.k-dropdown.k-invalid {
    border-color: #dc3545;
    background-color: #fff5f5;
}

/* Validation error message */
.k-invalid-msg {
    color: #dc3545;
    font-size: 12px;
    margin-top: 4px;
    display: block;
}

/* Required field indicator */
.required {
    color: #dc3545;
    font-weight: bold;
}

/* Valid field styling */
.k-textbox.k-valid {
    border-color: #28a745;
}
```

---

## 📊 Grid Design System

### Standard Grid Configuration

```javascript
// Reusable grid configuration
function createStandardGrid(elementId, options) {
    $(`#${elementId}`).kendoGrid({
        dataSource: {
            transport: {
                read: async (e) => {
                    const data = await callApi(options.apiUrl);
                    e.success(data.data || data);
                }
            },
            pageSize: options.pageSize || 20,
            schema: {
                model: {
                    id: options.idField,
                    fields: options.fields
                },
                data: function(response) {
                    return response.data || response;
                },
                total: function(response) {
                    return response.total || response.length;
                }
            }
        },
        columns: options.columns,
        height: options.height || 600,
        pageable: {
            refresh: true,
            pageSizes: [10, 20, 50, 100],
            buttonCount: 5
        },
        sortable: true,
        filterable: {
            mode: "menu"
        },
        resizable: true,
        reorderable: true,
        columnMenu: true,
        scrollable: true
    });
}

// Usage example
createStandardGrid('userGrid', {
    apiUrl: '/api/users',
    idField: 'userId',
    fields: {
        userId: { type: 'number' },
        loginId: { type: 'string' },
        email: { type: 'string' },
        isActive: { type: 'boolean' }
    },
    columns: [
        { field: 'userId', title: 'ID', width: 80 },
        { field: 'loginId', title: 'Login ID', width: 150 },
        { field: 'email', title: 'Email', width: 200 },
        { field: 'isActive', title: 'Active', width: 100, template: '#= isActive ? "Yes" : "No" #' }
    ],
    height: 600
});
```

### Grid Width & Height Behavior

```css
/* Grid container */
.k-grid {
    max-width: 100%;
    overflow-x: auto;
}

/* কম column - auto-fit */
.k-grid.auto-fit {
    width: fit-content;
    max-width: 100%;
}

/* বেশি column - full width with scroll */
.k-grid.full-width {
    width: 100%;
}

/* Grid content height */
.k-grid-content {
    max-height: 600px;
    overflow-y: auto;
}

/* Responsive grid */
@media (max-width: 768px) {
    .k-grid {
        font-size: 12px;
    }

    .k-grid-content {
        max-height: 400px;
    }
}
```

---

## 🔌 API Client (Generic Fetch)

### Generic API Caller

```javascript
// Generic API client using Fetch API
async function callApi(url, method = 'GET', data = null, options = {}) {
    const token = localStorage.getItem('authToken');

    const config = {
        method: method,
        headers: {
            'Content-Type': 'application/json',
            'Authorization': token ? `Bearer ${token}` : '',
            ...options.headers
        }
    };

    if (data && (method === 'POST' || method === 'PUT' || method === 'PATCH')) {
        config.body = JSON.stringify(data);
    }

    try {
        const response = await fetch(url, config);

        // Check if response is ok
        if (!response.ok) {
            const error = await response.json();
            throw new Error(error.message || `HTTP ${response.status}: ${response.statusText}`);
        }

        // Parse JSON response
        const result = await response.json();

        // Log for debugging
        console.log(`API Call: ${method} ${url}`, {
            correlationId: result.correlationId,
            success: result.success,
            data: result.data
        });

        return result;
    } catch (error) {
        console.error('API Error:', error);
        throw error;
    }
}

// Helper functions
async function get(url) {
    return await callApi(url, 'GET');
}

async function post(url, data) {
    return await callApi(url, 'POST', data);
}

async function put(url, data) {
    return await callApi(url, 'PUT', data);
}

async function del(url) {
    return await callApi(url, 'DELETE');
}
```

### Usage Examples

```javascript
// Get user
const user = await get('/api/users/123');

// Create user
const newUser = await post('/api/users', {
    loginId: 'john.doe',
    email: 'john@example.com'
});

// Update user
const updatedUser = await put('/api/users/123', {
    email: 'newemail@example.com'
});

// Delete user
await del('/api/users/123');
```

---

## 🎨 UI/UX Best Practices

### Color Scheme

```css
:root {
    /* Primary Colors */
    --primary-color: #3498db;
    --primary-dark: #2980b9;
    --primary-light: #5dade2;

    /* Secondary Colors */
    --secondary-color: #2c3e50;
    --secondary-dark: #1a252f;
    --secondary-light: #34495e;

    /* Status Colors */
    --success-color: #28a745;
    --warning-color: #ffc107;
    --danger-color: #dc3545;
    --info-color: #17a2b8;

    /* Neutral Colors */
    --background-color: #ecf0f1;
    --text-color: #2c3e50;
    --border-color: #bdc3c7;
}
```

### Typography

```css
body {
    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    font-size: 14px;
    line-height: 1.6;
    color: var(--text-color);
}

h1 { font-size: 28px; font-weight: 600; }
h2 { font-size: 24px; font-weight: 600; }
h3 { font-size: 20px; font-weight: 500; }
h4 { font-size: 18px; font-weight: 500; }
h5 { font-size: 16px; font-weight: 500; }
```

### Spacing System

```css
.p-1 { padding: 4px; }
.p-2 { padding: 8px; }
.p-3 { padding: 12px; }
.p-4 { padding: 16px; }
.p-5 { padding: 20px; }

.m-1 { margin: 4px; }
.m-2 { margin: 8px; }
.m-3 { margin: 12px; }
.m-4 { margin: 16px; }
.m-5 { margin: 20px; }
```

---

## 📱 Responsive Design

### Breakpoints

```css
/* Mobile First Approach */

/* Small devices (phones, 576px and up) */
@media (min-width: 576px) { }

/* Medium devices (tablets, 768px and up) */
@media (min-width: 768px) { }

/* Large devices (desktops, 992px and up) */
@media (min-width: 992px) { }

/* Extra large devices (large desktops, 1200px and up) */
@media (min-width: 1200px) { }
```

### Mobile Adaptations

```css
@media (max-width: 768px) {
    /* Sidebar always collapsed on mobile */
    .sidebar {
        width: 50px;
    }

    .main-content {
        margin-left: 50px;
    }

    /* Stack form fields vertically */
    .form-row {
        flex-direction: column;
    }

    /* Full width buttons */
    .k-button {
        width: 100%;
        margin-bottom: 8px;
    }
}
```

---

## 🚀 Performance Optimization

### Lazy Loading

```javascript
// Lazy load images
document.addEventListener('DOMContentLoaded', function() {
    const lazyImages = document.querySelectorAll('img[data-src]');

    const imageObserver = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                const img = entry.target;
                img.src = img.dataset.src;
                img.removeAttribute('data-src');
                imageObserver.unobserve(img);
            }
        });
    });

    lazyImages.forEach(img => imageObserver.observe(img));
});
```

### Debouncing

```javascript
// Debounce function for search
function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

// Usage
const searchInput = document.getElementById('search');
const debouncedSearch = debounce(async (value) => {
    const results = await callApi(`/api/search?q=${value}`);
    displaySearchResults(results);
}, 300);

searchInput.addEventListener('input', (e) => {
    debouncedSearch(e.target.value);
});
```

---

## 📚 References

- Kendo UI Documentation: https://docs.telerik.com/kendo-ui/
- Bootstrap 5: https://getbootstrap.com/
- MDN Web Docs: https://developer.mozilla.org/

---

**Last Updated**: 2026-04-14
**Version**: 2.0.0
**Status**: ✅ Complete UI/UX Design System

---

## 🧭 Recommended UI Design Context for HRIS + BonusPayment + Multi-Module System

এই section-এ HRIS + BonusPayment + 30+ module ভিত্তিক enterprise application-এর জন্য recommended UI design context define করা হলো। এটি existing frontend design guideline-এর extension হিসেবে কাজ করবে এবং নতুন design decision নেওয়ার সময় reference হিসেবে ব্যবহার করা যাবে।

### 1. Overall Design Direction

এই project-এর জন্য best-fit UI direction হবে:

- **Hybrid Enterprise Kendo-First Design System**
- **Corporate + Modern SaaS style**
- **Data-heavy friendly interface**
- **Consistency-first approach**
- **Light theme first**

এই system-এর মূল লক্ষ্য হবে:

- module-to-module consistency বজায় রাখা
- Kendo UI component ভিত্তিক reusable design system তৈরি করা
- HR, finance, approval, reporting এবং admin workflow-কে একই visual language-এর মধ্যে আনা
- high-volume data screen-এ readability ও productivity নিশ্চিত করা

### 2. UI Identity

এই application-এর design identity হবে:

> **Structured, calm, reusable enterprise UI for HR and financial operations**

UI-এর overall feel হবে:

- clean
- structured
- professional
- easy to scan
- function-first, decoration-light

### 3. Core Design Philosophy

এই UI system-এর core principle হবে ৫টি:

1. **Consistency First** – সব module-এ same interaction pattern থাকবে
2. **Kendo-First** – custom control-এর পরিবর্তে Kendo component preferred হবে
3. **Form Clarity** – form-heavy workflow-এ readability এবং error handling clear হতে হবে
4. **Grid Efficiency** – list, search, filter, sort এবং action execution fast হতে হবে
5. **Low Cognitive Load** – user যেন কম effort-এ দ্রুত কাজ শেষ করতে পারে

### 4. Layout Context

এই project-এর main layout structure হবে fixed enterprise shell based:

- **Header**: fixed top bar, 70px
- **Sidebar**: left navigation, 260px, collapsible to approximately 50px
- **Main Content**: responsive work area
- **Footer**: fixed bottom, 20px

#### Header

Header-এ থাকবে:

- application logo / name
- global search
- notification
- user profile dropdown

Header সব page-এ fixed থাকবে এবং top-level navigation identity maintain করবে।

#### Sidebar

Sidebar-এর behavior হবে:

- default expanded
- icon + text based menu
- collapsible
- multi-level navigation support
- active state clearly highlighted
- mobile-এ collapsed-first behavior

30+ module থাকার কারণে sidebar navigation structure অত্যন্ত গুরুত্বপূর্ণ। User যেন সবসময় বুঝতে পারে সে কোন module, submenu বা page-এ আছে।

#### Main Content

Main content area-এ প্রতি page-এ সাধারণত থাকবে:

- page title
- breadcrumb
- page action bar
- primary content area

Primary content area-এর মধ্যে form, grid, card, tabs, summary block বা modal-triggered action থাকতে পারে।

#### Footer

Footer minimal হবে এবং only informational content বহন করবে:

- version
- copyright
- optional environment label

### 5. Theme Context

Recommended theme strategy:

- **Light Theme Only (Phase 1)**
- Dark mode optional future enhancement

Reason:

- enterprise HR user base সাধারণত light UI-তে বেশি comfortable
- Kendo customization effort কম হবে
- consistency maintain করা সহজ হবে
- readability বেশি stable থাকবে

### 6. Color System Context

HRIS এবং BonusPayment module-এর মতো business-critical system-এর জন্য color system হবে calm, professional এবং readable।

#### Recommended Color Role

- **Primary**: Blue / Indigo tone
- **Secondary**: Slate / Deep gray
- **Background**: very light gray / off-white
- **Success**: green
- **Warning**: orange
- **Danger**: red
- **Info**: cyan / light blue

#### Color Usage Rule

- primary color only high-priority action এবং active state-এ ব্যবহার হবে
- এক screen-এ অনেক accent color ব্যবহার করা যাবে না
- module-wise আলাদা visual identity তৈরি করা যাবে না
- semantic color সব module-এ একই অর্থ বহন করবে

### 7. Typography Context

Typography হবে readability-first, enterprise-friendly এবং compact।

#### Recommended Font Stack

- `Inter`
- `Segoe UI`
- `Roboto`

#### Hierarchy

- **H1** → Page title
- **H2** → Section title
- **H3** → Card / form block title
- **Body text** → readable, compact, neutral

Typography decorative হবে না। Visual emphasis hierarchy-এর মাধ্যমে আসবে, style effect-এর মাধ্যমে না।

### 8. Spacing System Context

Spacing inconsistency enterprise UI-কে দ্রুত messy করে ফেলে। তাই fixed spacing system follow করতে হবে।

#### Recommended Base Unit

- **8px spacing system**

#### Common Values

- `8px`
- `16px`
- `24px`
- `32px`

#### Usage Rule

- form field gap fixed হবে
- card padding fixed হবে
- page section spacing consistent হবে
- header / sidebar / content alignment একই scale follow করবে

### 9. Component Strategy

এই project-এ component strategy হবে:

- **Kendo UI component first**
- custom styling allowed
- custom replacement discouraged

Rule:

> **Kendo কে override করা যাবে, replace করা যাবে না — unless business need absolutely requires it**

### 10. Button System

Button hierarchy হবে action priority অনুযায়ী।

#### Button Types

- **Primary** → Save / Submit / Confirm
- **Secondary** → Cancel / Back / Close
- **Outline** → less important action
- **Danger** → Delete / Remove / Reset
- **Text/Icon Button** → utility action / grid row action

#### Button Rules

- এক page-এ একটাই dominant primary action থাকবে
- destructive action primary button style নেবে না
- icon-only button only তখনই ব্যবহার হবে যখন meaning universally clear
- button label action-oriented হবে

### 11. Form Design Context

এই application form-heavy হওয়ায় form design হবে design system-এর central part।

#### Standard Form Strategy

এই project-এ ৩ ধরনের form pattern officially supported হবে:

##### 1. Inline Form

ব্যবহার হবে:

- search area
- filter area
- small update
- grid inline edit

##### 2. Modal Form

ব্যবহার হবে:

- medium size create/edit screen
- 5–15 fields
- quick maintenance workflow

##### 3. Complex Tabbed Form

ব্যবহার হবে:

- employee profile
- bonus configuration
- payroll / policy setup
- 15+ fields
- multiple business section

#### Default Recommendation

- small/medium module → modal form
- large business module → tabbed form
- filter/search panel → inline form

### 12. Form Field Context

Most cases-এ form field হিসেবে Kendo UI component ব্যবহার করা হবে।

#### Recommended Mapping

- Text input → **Kendo TextBox**
- Numeric input → **Kendo NumericTextBox**
- Date input → **Kendo DatePicker**
- Standard dropdown → **Kendo DropDownList**
- Searchable dropdown → **Kendo ComboBox**
- Multi selection → **Kendo MultiSelect**
- Checkbox / toggle → Kendo-compatible styled control
- Long text → textarea with consistent Kendo-aligned styling

#### Form Field Rules

- label top aligned হবে
- required field visibly marked থাকবে
- help text থাকলে consistent placement follow করবে
- validation message field-এর নিচে দেখাবে
- error state visually obvious হবে
- read-only এবং disabled state clearly distinguishable হবে

### 13. Validation UX Context

Validation behavior সব module-এ consistent হতে হবে।

#### Validation Rule

- invalid field → red border
- error message → field-এর নিচে
- required indicator → clear visual marker
- validation trigger → blur + submit
- summary message → optional notification

Validation message user-friendly হবে; technical language ব্যবহার করা যাবে না।

### 14. Dropdown / ComboBox Context

HRIS এবং admin workflow-এ dropdown-heavy interaction থাকবে। তাই standard behavior define করা জরুরি।

#### DropDownList ব্যবহার হবে যখন:

- option count কম
- fixed selection list
- search দরকার নেই

#### ComboBox ব্যবহার হবে যখন:

- option count বেশি
- user search করে খুঁজবে
- branch, employee, department, designation, account head-এর মতো large dataset থাকবে

#### Rule

- large dataset-এ plain dropdown ব্যবহার করা যাবে না
- searchable input performance-friendly হতে হবে
- remote data binding হলে server-side filtering preferred

### 15. Kendo Grid Context

এই system-এ grid হলো প্রধান operational component।

#### Grid Role

Grid ব্যবহার হবে:

- master list
- transactional list
- approval list
- bonus/payment records
- employee/search result
- audit/report preview

#### Grid Standard

Major grid-এ থাকবে:

- server-side paging
- sorting
- filtering
- action column
- column resize
- optional column reorder
- export where needed

#### Grid Placement

- normal module → card/container based grid
- data-heavy module → near full-width grid area

#### Grid Action Column

Standard action pattern:

- View
- Edit
- Delete
- More

#### Grid Rule

- overcrowded column avoid করতে হবে
- action icon predictable হতে হবে
- filters standardized হতে হবে
- important column left side-এ রাখতে হবে

### 16. Modal / Popup Context

Modal-এর জন্য **Kendo Window** standard হবে।

#### Modal Use Case

- quick create/edit
- short data entry
- confirmation dialog
- approval / note capture

#### Modal Rule

- large complex form modal-এ না
- nested modal avoid করতে হবে
- footer action predictable হবে
- modal title action-specific হবে

### 17. Card and Section Context

Card UI ব্যবহার হবে grouped content, summary block এবং structured information display-এর জন্য।

#### Card Use Case

- dashboard summary
- section wrapper
- form block
- informational panel

#### Card Style

- minimal shadow
- light border
- clean header
- consistent padding

### 18. Navigation UX Context

Navigation structure enterprise usage-friendly হতে হবে।

#### Standard Navigation Elements

- sidebar = main navigation
- breadcrumb = path indicator
- page title = current screen identity
- active menu state = current module indicator

30+ module environment-এ navigation clarity productivity-এর জন্য critical।

### 19. Page Action Area Context

প্রতি operational page-এ action area predictable থাকবে।

#### Common Actions

- Add New
- Save
- Export
- Refresh
- Filter
- Approve / Reject

#### Rule

- action placement consistent হবে
- primary action visually strongest হবে
- secondary action grouped থাকবে

### 20. State Design Context

সব major component-এর জন্য state pattern define করতে হবে।

#### Mandatory UI States

- **Loading**
- **Empty**
- **Error**
- **Success**

#### Example

- grid loading spinner
- empty list message
- form save success notification
- data load error block

### 21. Notification and Feedback Context

User action-এর পর system feedback clear হতে হবে।

#### Feedback Type

- success notification
- warning notification
- error notification
- confirmation dialog

#### Rule

- feedback immediate হবে
- tone polite হবে
- ambiguous message avoid করতে হবে

### 22. Performance-Oriented UI Context

UI শুধু সুন্দর হলেই হবে না; fast ও scalable হতে হবে।

#### Recommended Performance Strategy

- lazy loading in tabbed form
- server-side data operation in grid
- searchable remote combo for large datasets
- unnecessary DOM re-render কমানো
- fetch-based async communication

#### Important Rule

- `jQuery.ajax()` ব্যবহার করা যাবে না
- API call standard Fetch-based client-এর মাধ্যমে হবে

### 23. Design Decision Summary

এই project-এর জন্য final recommended UI direction হলো:

> **A Kendo-first, corporate, clean, light-theme, data-heavy enterprise UI system যেখানে sidebar-header-main-content fixed structure থাকবে, forms 3 pattern follow করবে, Kendo Grid হবে central component, আর সব module একই spacing/color/validation/button rules follow করবে।**

এই context follow করলে project-wide consistency, maintainability এবং developer alignment অনেক বেশি শক্তিশালী হবে।
