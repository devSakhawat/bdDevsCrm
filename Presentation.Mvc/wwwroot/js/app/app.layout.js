/* ================================================================
   bdDevCRM — app.layout.js
   Sidebar toggle · Nav groups · User menu · Active nav highlight
================================================================ */

'use strict';

const app = window.app || {};

// ── Layout Module ─────────────────────────────────────────────────
app.layout = (function () {

    const STORAGE_SIDEBAR = 'bdcrm_sidebar_collapsed';

    // ── Init ──────────────────────────────────────────────────────
    function init() {
        _restoreSidebarState();
        _bindSidebarToggle();
        _bindNavGroups();
        _bindUserMenu();
        _highlightActiveNav();
        _autoOpenActiveGroup();
        _bindSidebarSearch();
        _bindOverlay();
    }

    // ── Sidebar Toggle ────────────────────────────────────────────
    function _restoreSidebarState() {
        const isCollapsed = localStorage.getItem(STORAGE_SIDEBAR) === 'true';
        if (isCollapsed) document.body.classList.add('sidebar-collapsed');
    }

    function _bindSidebarToggle() {
        const isMobile = () => window.innerWidth < 768;

        document.getElementById('sidebarToggle')?.addEventListener('click', function () {
            if (isMobile()) {
                // Mobile: overlay slide-in
                document.body.classList.toggle('mobile-sidebar-open');
                document.getElementById('sidebarOverlay')?.classList.toggle('is-visible');
            } else {
                // Desktop: collapse/expand
                document.body.classList.toggle('sidebar-collapsed');
                const collapsed = document.body.classList.contains('sidebar-collapsed');
                localStorage.setItem(STORAGE_SIDEBAR, collapsed);
            }
        });
    }

    function _bindOverlay() {
        document.getElementById('sidebarOverlay')?.addEventListener('click', function () {
            document.body.classList.remove('mobile-sidebar-open');
            this.classList.remove('is-visible');
        });
    }

    // ── Nav Groups (accordion) ────────────────────────────────────
    function _bindNavGroups() {
        document.querySelectorAll('.nav-group__header').forEach(function (header) {
            header.addEventListener('click', function () {
                const groupId  = this.dataset.group;
                const children = document.getElementById('group-' + groupId);
                const isOpen   = this.classList.contains('is-open');

                // Close all
                document.querySelectorAll('.nav-group__header').forEach(h => h.classList.remove('is-open'));
                document.querySelectorAll('.nav-group__children').forEach(c => c.classList.remove('is-open'));

                // Toggle clicked
                if (!isOpen && children) {
                    this.classList.add('is-open');
                    children.classList.add('is-open');
                }
            });
        });
    }

    function _autoOpenActiveGroup() {
        const activeChild = document.querySelector('.nav-child.is-active');
        if (!activeChild) return;

        const children = activeChild.closest('.nav-group__children');
        if (!children) return;

        const groupId = children.id.replace('group-', '');
        const header  = document.querySelector(`.nav-group__header[data-group="${groupId}"]`);

        if (header)   header.classList.add('is-open');
        if (children) children.classList.add('is-open');
    }

    // ── Active Nav Highlight ──────────────────────────────────────
    function _highlightActiveNav() {
        const currentPath = window.location.pathname.toLowerCase();

        document.querySelectorAll('.nav-child').forEach(function (link) {
            const href = link.getAttribute('href');
            if (href && currentPath.startsWith(href.toLowerCase())) {
                link.classList.add('is-active');
            }
        });

        document.querySelectorAll('.nav-item--single .nav-link').forEach(function (link) {
            const href = link.getAttribute('href');
            if (href && (href === '/' ? currentPath === '/' : currentPath.startsWith(href.toLowerCase()))) {
                link.closest('.nav-item--single')?.classList.add('is-active');
            }
        });
    }

    // ── User Menu ─────────────────────────────────────────────────
    function _bindUserMenu() {
        const trigger  = document.getElementById('userMenuTrigger');
        const menu     = document.getElementById('userMenu');

        trigger?.addEventListener('click', function (e) {
            e.stopPropagation();
            menu?.classList.toggle('is-open');
        });

        document.addEventListener('click', function () {
            menu?.classList.remove('is-open');
        });
    }

    // ── Sidebar Search (filter nav items) ─────────────────────────
    function _bindSidebarSearch() {
        const input = document.getElementById('sidebarSearchInput');
        if (!input) return;

        input.addEventListener('input', function () {
            const q = this.value.trim().toLowerCase();

            if (!q) {
                // Restore all
                document.querySelectorAll('.nav-child, .nav-item--single, .nav-group').forEach(el => {
                    el.style.display = '';
                });
                return;
            }

            // Hide all groups first
            document.querySelectorAll('.nav-group').forEach(g => g.style.display = 'none');
            document.querySelectorAll('.nav-item--single').forEach(g => g.style.display = 'none');

            // Show matching children
            document.querySelectorAll('.nav-child').forEach(function (link) {
                const text = link.textContent.toLowerCase();
                if (text.includes(q)) {
                    link.style.display = '';
                    const group    = link.closest('.nav-group');
                    const children = link.closest('.nav-group__children');
                    if (group)    { group.style.display = ''; }
                    if (children) {
                        children.classList.add('is-open');
                        children.closest('.nav-group')
                            ?.querySelector('.nav-group__header')
                            ?.classList.add('is-open');
                    }
                } else {
                    link.style.display = 'none';
                }
            });

            // Show matching single items
            document.querySelectorAll('.nav-item--single').forEach(function (item) {
                const text = item.textContent.toLowerCase();
                item.style.display = text.includes(q) ? '' : 'none';
            });
        });
    }

    return { init };

})();

// ── Auto-init on DOM ready ────────────────────────────────────────
document.addEventListener('DOMContentLoaded', function () {
    app.layout.init();
});

window.app = app;
