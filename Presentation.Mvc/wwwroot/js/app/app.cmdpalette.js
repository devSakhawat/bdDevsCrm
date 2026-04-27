/* ================================================================
   bdDevCRM — app.cmdpalette.js
   Command Palette  |  Ctrl+K trigger  |  Keyboard navigation
================================================================ */

'use strict';

const app = window.app || {};

app.cmdPalette = (function () {

    // ── Static command registry ───────────────────────────────────
    const COMMANDS = [
        // Dashboard
        { label: 'Dashboard',             path: '/',                              icon: 'fa-house',                section: 'Navigation' },
        // CRM
        { label: 'Leads',                 path: '/Lead',                          icon: 'fa-user-plus',            section: 'CRM' },
        { label: 'Contacts',              path: '/Contact',                       icon: 'fa-address-book',         section: 'CRM' },
        { label: 'Accounts',              path: '/Account',                       icon: 'fa-building',             section: 'CRM' },
        { label: 'Opportunities',         path: '/Opportunity',                   icon: 'fa-funnel',               section: 'CRM' },
        { label: 'Tasks',                 path: '/Task',                          icon: 'fa-list-check',           section: 'CRM' },
        // HR
        { label: 'Employees',             path: '/Employee',                      icon: 'fa-id-card',              section: 'Human Resources' },
        { label: 'Departments',           path: '/Department',                    icon: 'fa-sitemap',              section: 'Human Resources' },
        { label: 'Designations',          path: '/Designation',                   icon: 'fa-briefcase',            section: 'Human Resources' },
        { label: 'Leave Management',      path: '/Leave',                         icon: 'fa-calendar-days',        section: 'Human Resources' },
        { label: 'Attendance',            path: '/Attendance',                    icon: 'fa-clock',                section: 'Human Resources' },
        // Payroll
        { label: 'Salary Setup',          path: '/SalarySetup',                   icon: 'fa-sliders',              section: 'Payroll' },
        { label: 'Process Payroll',       path: '/PayrollProcess',                icon: 'fa-gears',                section: 'Payroll' },
        { label: 'Pay Slips',             path: '/Payslip',                       icon: 'fa-file-invoice-dollar',  section: 'Payroll' },
        { label: 'Tax Reports',           path: '/TaxReport',                     icon: 'fa-receipt',              section: 'Payroll' },
        // System Admin
        { label: 'User Management',       path: '/SystemAdmin/Users',             icon: 'fa-user-gear',            section: 'System Admin' },
        { label: 'Roles & Permissions',   path: '/SystemAdmin/Roles',             icon: 'fa-key',                  section: 'System Admin' },
        { label: 'Menu Builder',          path: '/SystemAdmin/Menu',              icon: 'fa-bars-staggered',       section: 'System Admin' },
        { label: 'Company Setup',         path: '/SystemAdmin/Company',           icon: 'fa-building-flag',        section: 'System Admin' },
    ];

    let _selectedIndex = -1;
    let _filtered      = [];

    // ── Open / Close ──────────────────────────────────────────────
    function open() {
        document.getElementById('cmdBackdrop')?.classList.add('is-open');
        document.getElementById('cmdPalette')?.classList.add('is-open');
        const input = document.getElementById('cmdInput');
        if (input) {
            input.value = '';
            input.focus();
            _render('');
        }
        _selectedIndex = -1;
    }

    function close() {
        document.getElementById('cmdBackdrop')?.classList.remove('is-open');
        document.getElementById('cmdPalette')?.classList.remove('is-open');
    }

    // ── Render Results ────────────────────────────────────────────
    function _render(query) {
        const q = query.toLowerCase().trim();
        _filtered = q
            ? COMMANDS.filter(c => c.label.toLowerCase().includes(q) || c.section.toLowerCase().includes(q))
            : COMMANDS;

        const container = document.getElementById('cmdResults');
        if (!container) return;

        if (_filtered.length === 0) {
            container.innerHTML = '<div class="cmd-palette__empty">No results found for "<strong>' + query + '</strong>"</div>';
            return;
        }

        // Group by section
        const sections = {};
        _filtered.forEach((cmd, idx) => {
            if (!sections[cmd.section]) sections[cmd.section] = [];
            sections[cmd.section].push({ ...cmd, _idx: idx });
        });

        let html = '';
        Object.keys(sections).forEach(section => {
            html += `<div class="cmd-palette__section-label">${section}</div>`;
            sections[section].forEach(cmd => {
                html += `
                <div class="cmd-result-item" data-idx="${cmd._idx}" data-path="${cmd.path}">
                    <i class="fa ${cmd.icon}"></i>
                    <span class="cmd-result-item__label">${cmd.label}</span>
                    <span class="cmd-result-item__path">${cmd.path}</span>
                </div>`;
            });
        });

        container.innerHTML = html;

        // Bind click
        container.querySelectorAll('.cmd-result-item').forEach(item => {
            item.addEventListener('click', function () {
                window.location.href = this.dataset.path;
                close();
            });
        });
    }

    function _updateSelection() {
        const items = document.querySelectorAll('.cmd-result-item');
        items.forEach((item, i) => {
            item.classList.toggle('is-selected', i === _selectedIndex);
        });
        if (_selectedIndex >= 0 && items[_selectedIndex]) {
            items[_selectedIndex].scrollIntoView({ block: 'nearest' });
        }
    }

    // ── Keyboard Navigation ───────────────────────────────────────
    function _handleKeydown(e) {
        const items  = document.querySelectorAll('.cmd-result-item');
        const isOpen = document.getElementById('cmdPalette')?.classList.contains('is-open');

        if (!isOpen) return;

        if (e.key === 'ArrowDown') {
            e.preventDefault();
            _selectedIndex = Math.min(_selectedIndex + 1, items.length - 1);
            _updateSelection();
        } else if (e.key === 'ArrowUp') {
            e.preventDefault();
            _selectedIndex = Math.max(_selectedIndex - 1, 0);
            _updateSelection();
        } else if (e.key === 'Enter') {
            e.preventDefault();
            if (_selectedIndex >= 0 && items[_selectedIndex]) {
                window.location.href = items[_selectedIndex].dataset.path;
                close();
            }
        } else if (e.key === 'Escape') {
            close();
        }
    }

    // ── Init ──────────────────────────────────────────────────────
    function init() {
        // Ctrl+K global
        document.addEventListener('keydown', function (e) {
            if ((e.ctrlKey || e.metaKey) && e.key === 'k') {
                e.preventDefault();
                const isOpen = document.getElementById('cmdPalette')?.classList.contains('is-open');
                isOpen ? close() : open();
            }
            _handleKeydown(e);
        });

        // Input search
        document.getElementById('cmdInput')?.addEventListener('input', function () {
            _selectedIndex = -1;
            _render(this.value);
        });

        // Global search bar
        document.getElementById('globalSearchInput')?.addEventListener('focus', function () {
            open();
        });

        // Backdrop click
        document.getElementById('cmdBackdrop')?.addEventListener('click', close);
    }

    return { init, open, close };

})();

document.addEventListener('DOMContentLoaded', function () {
    app.cmdPalette.init();
});

window.app = app;
