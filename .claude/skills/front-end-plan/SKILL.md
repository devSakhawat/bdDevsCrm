---
name: bddevs-crm-frontend-crud
description: >
  Use this skill when generating any frontend CRUD code for the bdDevsCrm project.
  Covers Razor View, Kendo Grid, Form (Single/Multi-column), JavaScript (Fetch API),
  and CSS following the HRIS Enterprise Design System.
  Triggers: "create frontend for [Entity]", "generate UI for [Entity]",
  "add Kendo grid for [Entity]", "create form for [Entity]".
project: bdDevsCrm
framework: ASP.NET Core MVC + Razor + Kendo UI 2024 Q4 + jQuery (DOM only) + Fetch API
---

# bdDevsCrm Frontend CRUD Skill

## 1. Design System Foundation

### 1.1 Layout Dimensions (FIXED — never change)
```
┌──────────────────────────────────────────┐
│  Header: 60px (fixed top, #1E5FA8)       │
├──────────┬───────────────────────────────┤
│ Sidebar  │  Main Content Area            │
│ 240px    │  margin-left: 240px           │
│ (64px    │  margin-top: 60px             │
│collapsed)│  padding: 24px                │
│ #1E293B  │  background: #F1F5F9          │
├──────────┴───────────────────────────────┤
│  Footer: 48px (border-top, #FFFFFF)      │
└──────────────────────────────────────────┘
```

### 1.2 Color Palette (Use ONLY these — no custom colors)
```scss
// Primary
$color-primary:    #1E5FA8;   // Header, Primary Button, Grid Header
$color-primary-dk: #1E3A5F;   // Hover state
$color-accent:     #2563EB;   // Active border, focus ring, selected row border
$color-primary-lt: #DBEAFE;   // Alert info bg, selected row bg

// Neutral
$color-dark:       #1E293B;   // Page title, sidebar bg, headings
$color-mid:        #475569;   // Body text, table cells
$color-light:      #94A3B8;   // Placeholder, helper, disabled, breadcrumb
$color-border:     #CBD5E1;   // All borders, dividers
$color-bg:         #F1F5F9;   // Page background, odd row, disabled bg

// Semantic
$color-success:    #16A34A;   // Save, approve, success toast
$color-danger:     #DC2626;   // Delete, error, required asterisk
$color-warning:    #D97706;   // Warning toast, pending badge
$color-info:       #0284C7;   // Info toast, help
```

### 1.3 Typography Scale (Use ONLY these sizes)
| Element | Size | Weight | Color |
|---------|------|--------|-------|
| Page Title (H1) | 22px | 700 | #1E293B |
| Section Title (H2) | 18px | 600 | #1E5FA8 |
| Card/Group Title (H3) | 15px | 600 | #1E293B |
| Form Label | 13px | 500 | #1E293B |
| Body Text | 13px | 400 | #475569 |
| Helper/Caption | 12px | 400 | #94A3B8 |
| Error Message | 11px | 400 italic | #DC2626 |

**Font stack:** `'Segoe UI', system-ui, -apple-system, sans-serif`

---

## 2. File Structure for Each Entity

```
Presentation.Mvc/
└── Views/
    └── SystemAdmin/
        └── EntityName/
            ├── Index.cshtml        ← Grid page
            ├── Create.cshtml       ← Create form (or Modal-based)
            └── Edit.cshtml         ← Edit form (optional, or same modal)

wwwroot/
└── js/
    └── modules/
        └── SystemAdmin/
            └── EntityName/
                ├── entityName.grid.js    ← Kendo Grid config
                └── entityName.form.js    ← Form logic
```

---

## 3. Razor Grid Page Template (Index.cshtml)

```razor
@{
    ViewData["Title"] = "Entity Name Management";
    Layout = "~/Views/Shared/_Layout.cshtml";
}

<!-- Page Header Zone -->
<div class="page-header">
    <div class="page-header__title-row">
        <div>
            <h1 class="page-title">Entity Name Management</h1>
            <nav class="breadcrumb">
                <span>Home</span>
                <span class="breadcrumb__sep">/</span>
                <span>System Admin</span>
                <span class="breadcrumb__sep">/</span>
                <span class="breadcrumb__current">Entity Name</span>
            </nav>
        </div>
        <div class="page-header__actions">
            <button class="k-button btn-primary" id="btnAddNew" onclick="entityNameGrid.openCreateModal()">
                <i class="fa fa-plus"></i> Add New
            </button>
            <button class="k-button btn-secondary" id="btnExport" onclick="entityNameGrid.exportExcel()">
                <i class="fa fa-file-excel"></i> Export Excel
            </button>
        </div>
    </div>
    <hr class="page-header__divider" />
</div>

<!-- Grid Card -->
<div class="card">
    <div id="entityNameGrid"></div>
</div>

<!-- Modal for Create/Edit -->
<div id="entityNameModal" class="k-window" style="display:none;">
    <div class="modal-header">
        <h3 class="modal-title" id="modalTitle">Add Entity Name</h3>
        <button class="modal-close" onclick="entityNameGrid.closeModal()">
            <i class="fa fa-times"></i>
        </button>
    </div>
    <div class="modal-body" id="modalBody">
        <!-- Form injected here -->
    </div>
</div>

@section Scripts {
    <script src="~/js/modules/SystemAdmin/EntityName/entityName.grid.js"></script>
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            entityNameGrid.init();
        });
    </script>
}
```

---

## 4. Kendo Grid JavaScript Template

**File:** `wwwroot/js/modules/SystemAdmin/EntityName/entityName.grid.js`

```javascript
'use strict';

const entityNameGrid = (function () {

    // ─── Constants ────────────────────────────────────────────────────────────
    const API_BASE = '/bdDevs-crm';
    const ENDPOINTS = {
        summary:  `${API_BASE}/entity-name-summary`,
        create:   `${API_BASE}/entity-name`,
        update:   `${API_BASE}/entity-name`,
        delete:   (key) => `${API_BASE}/entity-name/${key}`,
        single:   (id)  => `${API_BASE}/entity-name/${id}`,
    };

    let _grid = null;

    // ─── Grid Initialization ──────────────────────────────────────────────────
    function init() {
        _grid = $('#entityNameGrid').kendoGrid({
            dataSource: _buildDataSource(),
            columns:    _buildColumns(),
            pageable: {
                pageSize:    20,
                pageSizes:   [10, 20, 50, 100],
                buttonCount: 5,
                messages: {
                    display: 'Showing {0}-{1} of {2} records'
                }
            },
            sortable:   true,
            filterable: { mode: 'row' },
            resizable:  true,
            reorderable: true,
            scrollable:  { virtual: false },
            noRecords: {
                template: '<div class="grid-empty-state">' +
                          '<i class="fa fa-inbox"></i>' +
                          '<p>No records found.</p>' +
                          '<small>Try adjusting your filters.</small>' +
                          '</div>'
            },
            height: 'calc(100vh - 220px)'
        }).data('kendoGrid');
    }

    // ─── DataSource ───────────────────────────────────────────────────────────
    function _buildDataSource() {
        return new kendo.data.DataSource({
            serverPaging:   true,
            serverSorting:  true,
            serverFiltering: true,
            transport: {
                read: function (options) {
                    _showLoading();
                    fetch(ENDPOINTS.summary, {
                        method: 'POST',
                        headers: _getHeaders(),
                        body: JSON.stringify(_mapGridOptions(options.data))
                    })
                    .then(r => r.json())
                    .then(res => {
                        _hideLoading();
                        if (res.success) {
                            options.success({
                                data:  res.data.items,
                                total: res.data.totalCount
                            });
                        } else {
                            options.error(res);
                            app.toast.error(res.message || 'Failed to load data.');
                        }
                    })
                    .catch(err => {
                        _hideLoading();
                        options.error(err);
                        app.toast.error('Network error. Please try again.');
                    });
                }
            },
            schema: {
                data:  'data',
                total: 'total',
                model: {
                    id: 'entityNameId',
                    fields: {
                        entityNameId: { type: 'number' },
                        name:         { type: 'string' },
                        isActive:     { type: 'boolean' }
                        // add other fields matching your DTO
                    }
                }
            },
            pageSize: 20
        });
    }

    // ─── Grid Columns ─────────────────────────────────────────────────────────
    function _buildColumns() {
        return [
            {
                field: 'entityNameId',
                title: '#',
                width: 60,
                filterable: false
            },
            {
                field: 'name',
                title: 'Name',
                width: 200
            },
            {
                field: 'isActive',
                title: 'Status',
                width: 100,
                template: function (d) {
                    return d.isActive
                        ? '<span class="badge badge--success">Active</span>'
                        : '<span class="badge badge--danger">Inactive</span>';
                },
                filterable: false
            },
            // Action Column — ALWAYS last
            {
                title: 'Actions',
                width: 160,
                filterable: false,
                sortable:   false,
                template:   _actionColumnTemplate
            }
        ];
    }

    function _actionColumnTemplate(dataItem) {
        return `
            <div class="action-group">
                <button class="k-button btn-sm btn-secondary"
                        title="Edit"
                        onclick="entityNameGrid.openEditModal(${dataItem.entityNameId})">
                    <i class="fa fa-pencil"></i>
                </button>
                <button class="k-button btn-sm btn-outline"
                        title="View"
                        onclick="entityNameGrid.viewDetail(${dataItem.entityNameId})">
                    <i class="fa fa-eye"></i>
                </button>
                <button class="k-button btn-sm btn-danger"
                        title="Delete"
                        onclick="entityNameGrid.confirmDelete(${dataItem.entityNameId}, '${dataItem.name}')">
                    <i class="fa fa-trash"></i>
                </button>
            </div>`;
    }

    // ─── CRUD Operations ──────────────────────────────────────────────────────
    async function openCreateModal() {
        $('#modalTitle').text('Add Entity Name');
        $('#modalBody').html(_buildFormHtml(null));
        _initFormWidgets();
        $('#entityNameModal').show();
    }

    async function openEditModal(id) {
        try {
            _showLoading();
            const res = await fetch(ENDPOINTS.single(id), { headers: _getHeaders() });
            const data = await res.json();
            _hideLoading();

            if (data.success) {
                $('#modalTitle').text('Edit Entity Name');
                $('#modalBody').html(_buildFormHtml(data.data));
                _initFormWidgets(data.data);
                $('#entityNameModal').show();
            } else {
                app.toast.error(data.message || 'Failed to load record.');
            }
        } catch (err) {
            _hideLoading();
            app.toast.error('Network error.');
        }
    }

    async function saveForm() {
        if (!_validateForm()) return;

        const formData = _collectFormData();
        const isEdit   = formData.entityNameId > 0;
        const url      = isEdit ? `${ENDPOINTS.update}/${formData.entityNameId}` : ENDPOINTS.create;
        const method   = isEdit ? 'PUT' : 'POST';

        try {
            _setBtnLoading('#btnSave', true);
            const res  = await fetch(url, {
                method,
                headers: _getHeaders(),
                body: JSON.stringify(formData)
            });
            const data = await res.json();
            _setBtnLoading('#btnSave', false);

            if (data.success) {
                app.toast.success(isEdit ? 'Record updated successfully.' : 'Record created successfully.');
                closeModal();
                _grid.dataSource.read();
            } else {
                app.toast.error(data.message || 'Operation failed.');
            }
        } catch (err) {
            _setBtnLoading('#btnSave', false);
            app.toast.error('Network error.');
        }
    }

    function confirmDelete(id, name) {
        if (!confirm(`Delete "${name}"? This action cannot be undone.`)) return;
        _deleteRecord(id);
    }

    async function _deleteRecord(id) {
        try {
            const res  = await fetch(ENDPOINTS.delete(id), {
                method: 'DELETE',
                headers: _getHeaders()
            });
            const data = await res.json();

            if (data.success) {
                app.toast.success('Record deleted successfully.');
                _grid.dataSource.read();
            } else {
                app.toast.error(data.message || 'Delete failed.');
            }
        } catch (err) {
            app.toast.error('Network error.');
        }
    }

    // ─── Form Builder ─────────────────────────────────────────────────────────
    function _buildFormHtml(data) {
        const d = data || {};
        return `
        <form id="entityNameForm" class="form-layout form-layout--single" novalidate>
            <input type="hidden" id="entityNameId" name="entityNameId" value="${d.entityNameId || 0}" />

            <div class="form-group" id="fg_Name">
                <label for="Name">Name <span class="required-star">*</span></label>
                <input type="text"
                       id="Name"
                       name="Name"
                       class="k-input"
                       value="${d.name || ''}"
                       data-val="true"
                       data-required-msg="Name is required."
                       placeholder="Enter name" />
                <span class="field-error-msg" id="err_Name"></span>
            </div>

            <!-- Add more fields here following the same pattern -->

            <div class="form-group" id="fg_IsActive">
                <label>Status</label>
                <div class="checkbox-group">
                    <input type="checkbox"
                           id="IsActive"
                           name="IsActive"
                           ${d.isActive !== false ? 'checked' : ''} />
                    <label for="IsActive">Active</label>
                </div>
            </div>
        </form>

        <!-- Form Action Bar -->
        <div class="form-action-bar">
            <button class="k-button btn-outline" onclick="entityNameGrid.closeModal()">
                Cancel
            </button>
            <button class="k-button btn-primary" id="btnSave" onclick="entityNameGrid.saveForm()">
                <i class="fa fa-save"></i> Save
            </button>
        </div>`;
    }

    // ─── Form Validation ──────────────────────────────────────────────────────
    function _validateForm() {
        let isValid = true;

        // Name validation
        const name = document.getElementById('Name')?.value?.trim();
        if (!name) {
            _showFieldError('Name', 'Name is required.');
            isValid = false;
        } else {
            _clearFieldError('Name');
        }

        // Add more validations...

        if (!isValid) {
            document.querySelector('.field-error')?.scrollIntoView({ behavior: 'smooth', block: 'center' });
        }

        return isValid;
    }

    function _showFieldError(fieldId, message) {
        const group = document.getElementById(`fg_${fieldId}`);
        const input = document.getElementById(fieldId);
        const err   = document.getElementById(`err_${fieldId}`);
        if (group) group.classList.add('field-error');
        if (input) input.style.borderColor = '#DC2626';
        if (err)   err.textContent = message;
    }

    function _clearFieldError(fieldId) {
        const group = document.getElementById(`fg_${fieldId}`);
        const input = document.getElementById(fieldId);
        const err   = document.getElementById(`err_${fieldId}`);
        if (group) group.classList.remove('field-error');
        if (input) input.style.borderColor = '';
        if (err)   err.textContent = '';
    }

    // ─── Helpers ──────────────────────────────────────────────────────────────
    function _collectFormData() {
        return {
            entityNameId: parseInt(document.getElementById('entityNameId')?.value) || 0,
            name:         document.getElementById('Name')?.value?.trim(),
            isActive:     document.getElementById('IsActive')?.checked ?? true
        };
    }

    function _initFormWidgets(data) {
        // Initialize Kendo widgets here
        // e.g., $('#StatusId').kendoDropDownList({ ... });
    }

    function _mapGridOptions(kendoOptions) {
        return {
            page:     kendoOptions.page     || 1,
            pageSize: kendoOptions.pageSize || 20,
            sortField: kendoOptions.sort?.[0]?.field || '',
            sortDir:   kendoOptions.sort?.[0]?.dir   || 'asc',
            filter:    kendoOptions.filter  || null
        };
    }

    function _getHeaders() {
        const token = localStorage.getItem('authToken');
        return {
            'Content-Type': 'application/json',
            'Authorization': token ? `Bearer ${token}` : ''
        };
    }

    function closeModal() {
        $('#entityNameModal').hide();
        $('#entityNameForm')[0]?.reset();
    }

    function _showLoading() {
        kendo.ui.progress($('#entityNameGrid'), true);
    }

    function _hideLoading() {
        kendo.ui.progress($('#entityNameGrid'), false);
    }

    function _setBtnLoading(selector, loading) {
        const btn = document.querySelector(selector);
        if (!btn) return;
        btn.disabled = loading;
        btn.innerHTML = loading
            ? '<i class="fa fa-spinner fa-spin"></i> Processing...'
            : '<i class="fa fa-save"></i> Save';
    }

    function exportExcel() {
        _grid.saveAsExcel();
    }

    function viewDetail(id) {
        // implement detail view if needed
    }

    // ─── Public API ───────────────────────────────────────────────────────────
    return {
        init,
        openCreateModal,
        openEditModal,
        saveForm,
        confirmDelete,
        closeModal,
        exportExcel,
        viewDetail
    };

})();
```

---

## 5. Form Types Reference

### Type 1 — Single Column (4–6 fields, max-width 720px)
```html
<form class="form-layout form-layout--single">
    <!-- Used for: simple modals, leave requests, basic setup forms -->
</form>
```

### Type 2 — Multi-Column Grid (6–20+ fields)
```html
<form class="form-layout form-layout--grid">
    <div class="form-row">
        <div class="form-col"><!-- First Name --></div>
        <div class="form-col"><!-- Last Name --></div>
    </div>
    <!-- Used for: employee profile, complex setup, payroll -->
</form>
```

### Type 3 — Inline Filter (above grid)
```html
<form class="form-layout form-layout--inline">
    <!-- Used for: search bar, filter panel above grid -->
</form>
```

---

## 6. Form Field HTML Patterns

### Text Input
```html
<div class="form-group" id="fg_FieldName">
    <label for="FieldName">Field Label <span class="required-star">*</span></label>
    <input type="text"
           id="FieldName"
           name="FieldName"
           class="k-input"
           data-val="true"
           data-required-msg="Field Label is required."
           placeholder="Enter value" />
    <span class="field-error-msg" id="err_FieldName"></span>
</div>
```

### Dropdown (Kendo DropDownList)
```html
<div class="form-group" id="fg_StatusId">
    <label for="StatusId">Status <span class="required-star">*</span></label>
    <input id="StatusId" name="StatusId" />
    <span class="field-error-msg" id="err_StatusId"></span>
</div>
<!-- JS init: -->
<script>
$('#StatusId').kendoDropDownList({
    dataSource: { /* fetch from API */ },
    dataTextField:  'text',
    dataValueField: 'value',
    optionLabel: '-- Select Status --',
    filter: 'contains'
});
</script>
```

### Date Picker
```html
<div class="form-group" id="fg_FromDate">
    <label for="FromDate">From Date</label>
    <input id="FromDate" name="FromDate" />
    <span class="field-error-msg" id="err_FromDate"></span>
</div>
<!-- JS init: -->
<script>
$('#FromDate').kendoDatePicker({
    format: 'dd MMM yyyy',
    max: new Date()
});
</script>
```

### Checkbox
```html
<div class="form-group">
    <label>Status</label>
    <div class="checkbox-group">
        <input type="checkbox" id="IsActive" name="IsActive" checked />
        <label for="IsActive">Active</label>
    </div>
</div>
```

---

## 7. CSS Patterns (BEM-inspired)

```css
/* Form Layout */
.form-layout { }
.form-layout--single { max-width: 720px; }
.form-layout--grid   { display: grid; grid-template-columns: 1fr 1fr; gap: 16px; }
.form-layout--inline { display: flex; flex-wrap: wrap; gap: 16px; align-items: flex-end; }

/* Form Group */
.form-group        { margin-bottom: 16px; }
.form-group label  { display: block; font-size: 13px; font-weight: 500; color: #1E293B; margin-bottom: 4px; }
.required-star     { color: #DC2626; }
.field-error-msg   { color: #DC2626; font-size: 11px; font-style: italic; margin-top: 4px; display: block; }

/* Field Error State */
.field-error .k-input  { border-color: #DC2626 !important; background: #FFF5F5 !important; }

/* Buttons */
.btn-primary   { background: #1E5FA8; color: #fff; border: none; height: 36px; padding: 0 16px; border-radius: 4px; font-size: 13px; font-weight: 500; }
.btn-primary:hover   { background: #1E3A5F; }
.btn-secondary { background: #fff; color: #1E5FA8; border: 1px solid #1E5FA8; height: 36px; padding: 0 16px; border-radius: 4px; }
.btn-outline   { background: transparent; color: #475569; border: 1px solid #CBD5E1; height: 36px; padding: 0 16px; border-radius: 4px; }
.btn-danger    { background: #DC2626; color: #fff; border: none; height: 36px; padding: 0 16px; border-radius: 4px; }
.btn-sm        { height: 28px; padding: 0 10px; font-size: 12px; }

/* Card */
.card { background: #fff; border: 1px solid #CBD5E1; border-radius: 8px; box-shadow: 0 1px 3px rgba(0,0,0,0.08); padding: 24px; margin-bottom: 16px; }

/* Grid Empty State */
.grid-empty-state { text-align: center; padding: 48px; color: #94A3B8; }
.grid-empty-state i { font-size: 48px; color: #CBD5E1; display: block; margin-bottom: 16px; }

/* Action Group */
.action-group { display: flex; gap: 4px; }

/* Badge */
.badge { display: inline-block; padding: 2px 8px; border-radius: 4px; font-size: 11px; font-weight: 600; text-transform: uppercase; }
.badge--success { background: #DCFCE7; color: #16A34A; }
.badge--danger  { background: #FEE2E2; color: #DC2626; }
.badge--warning { background: #FEF3C7; color: #D97706; }

/* Page Header */
.page-header        { margin-bottom: 24px; }
.page-header__title-row { display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 12px; }
.page-header__actions   { display: flex; gap: 8px; }
.page-header__divider   { border: none; border-top: 1px solid #CBD5E1; margin: 0; }
.page-title    { font-size: 22px; font-weight: 700; color: #1E293B; margin: 0 0 4px; }
.breadcrumb    { font-size: 12px; color: #94A3B8; }
.breadcrumb__sep     { margin: 0 6px; }
.breadcrumb__current { color: #475569; }

/* Form Action Bar */
.form-action-bar { display: flex; justify-content: flex-end; gap: 8px; padding: 16px 0 0; border-top: 1px solid #CBD5E1; margin-top: 24px; }

/* Modal */
.modal-header { display: flex; justify-content: space-between; align-items: center; padding: 20px 24px; background: #F8FAFC; border-bottom: 1px solid #CBD5E1; }
.modal-title  { font-size: 16px; font-weight: 700; color: #1E293B; margin: 0; }
.modal-close  { background: none; border: none; color: #94A3B8; cursor: pointer; font-size: 20px; }
.modal-body   { padding: 24px; overflow-y: auto; max-height: 70vh; }

/* Kendo Grid Overrides */
.k-grid                { border-radius: 6px; border-color: #CBD5E1; }
.k-grid-header th      { background: #1E5FA8 !important; color: #fff; font-weight: 600; font-size: 13px; }
.k-grid tr:hover       { background: #EFF6FF; }
.k-grid tr:nth-child(odd) { background: #F8FAFC; }
.k-grid tr.k-state-selected { background: #DBEAFE; border-left: 3px solid #2563EB; }
.k-pager-wrap          { border-top: 1px solid #CBD5E1; background: #fff; }
```

---

## 8. Kendo Grid Standard Configuration

**Mandatory features for every grid:**
```javascript
{
    serverPaging:   true,   // ALWAYS — never client-side for production data
    serverSorting:  true,
    serverFiltering: true,
    pageable: {
        pageSize:  20,
        pageSizes: [10, 20, 50, 100]
    },
    sortable:    true,
    filterable:  { mode: 'row' },  // filter row below header
    resizable:   true,
    reorderable: true,
    scrollable:  { virtual: false }
}
```

**Grid Column Rules:**
- Action column: ALWAYS last, width 120–160px
- ID column: width 60px, filterable: false
- Boolean columns: render as badge (never raw true/false)
- Date columns: format `dd MMM yyyy`
- Currency: format with thousand separator + 2 decimals
- Long text: set maxWidth + overflow ellipsis

---

## 9. API Call Rules (CRITICAL)

### ✅ ALWAYS use Fetch API
```javascript
// ✅ CORRECT
const response = await fetch(url, {
    method: 'POST',
    headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${localStorage.getItem('authToken')}`
    },
    body: JSON.stringify(data)
});
const result = await response.json();
```

### ❌ NEVER use jQuery.ajax()
```javascript
// ❌ WRONG — NEVER use this
$.ajax({ url, method, data, success: callback });
$.get(url, callback);
$.post(url, data, callback);
```

### jQuery Usage Rule
```javascript
// ✅ jQuery ONLY for DOM manipulation
const $btn = $('#btnSave');
$btn.prop('disabled', true);
$('#myModal').show();
$('.form-group').removeClass('field-error');

// ❌ NEVER for HTTP calls
```

---

## 10. API Response Handling Pattern

```javascript
// Standard ApiResponse<T> structure from backend
{
    statusCode: 200,
    success: true,
    message: "Operation completed successfully",
    data: { /* payload */ },
    pagination: {
        currentPage: 1,
        pageSize: 20,
        totalCount: 150,
        totalPages: 8
    },
    error: null
}

// Always check res.success before using res.data
async function loadData() {
    const res = await fetch(url, { headers: _getHeaders() }).then(r => r.json());
    if (!res.success) {
        app.toast.error(res.message || 'Operation failed.');
        return;
    }
    // use res.data
}
```

---

## 11. Toast Notification Standard

```javascript
// Use project-wide app.toast module
app.toast.success('Record saved successfully.');   // 3s, top-right, green
app.toast.error('Operation failed.');              // 5s, manual close, red
app.toast.warning('Please review your input.');   // 4s, orange
app.toast.info('Loading data...');                // 3s, blue
```

---

## 12. Delete Confirmation Pattern

```javascript
// Simple confirm for standard deletes
function confirmDelete(id, name) {
    if (!confirm(`Delete "${name}"?\n\nThis action cannot be undone.`)) return;
    _deleteRecord(id);
}

// Kendo Window for important deletes (use when data has cascading effects)
function showDeleteModal(id, name) {
    $('#deleteConfirmMsg').text(`Are you sure you want to delete "${name}"?`);
    $('#btnConfirmDelete').data('id', id);
    $('#deleteModal').show();
}
```

---

## 13. Naming Conventions

### JavaScript
```javascript
// Module pattern — IIFE with public API
const entityNameGrid = (function () {
    // private
    function _privateMethod() {}

    // public
    return { init, openCreateModal, saveForm };
})();

// camelCase for functions and variables
function openEditModal(id) {}
const entityNameId = 123;
const API_BASE_URL = '/bdDevs-crm';  // UPPER_SNAKE for constants
```

### CSS Classes (BEM-inspired)
```css
.entity-form { }                /* block */
.entity-form__header { }        /* block__element */
.entity-form--readonly { }      /* block--modifier */
.mt-16 { margin-top: 16px; }   /* utility */
```

### HTML IDs (used in JS)
```html
id="entityNameGrid"     <!-- Grid container -->
id="entityNameModal"    <!-- Modal container -->
id="entityNameForm"     <!-- Form element -->
id="btnSave"            <!-- Save button -->
id="fg_FieldName"       <!-- Form group wrapper -->
id="err_FieldName"      <!-- Error message span -->
```

---

## 14. Anti-Patterns to Avoid

❌ **NEVER do these:**
- `$.ajax()`, `$.get()`, `$.post()` for HTTP calls
- Inline styles in HTML (`style="color:red"`) — use CSS classes
- Client-side paging for large datasets
- Random colors not from the palette
- Random font sizes not from the type scale
- `alert()` or `confirm()` for success/error — use `app.toast`
- Business logic in the view file
- Direct DOM manipulation when Kendo widget exists

✅ **ALWAYS do these:**
- `fetch()` for all HTTP calls
- Server-side paging in Kendo Grid (`serverPaging: true`)
- Colors ONLY from the defined palette
- Font sizes ONLY from the type scale
- BEM-style CSS classes
- Form validation on `blur` AND `submit`
- Scroll to first error on form submit
- Loading overlay during async operations
- `app.toast` for all notifications
