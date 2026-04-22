window.AppSidebar = (() => {
    const selectors = {
        search: '[data-sidebar-search]',
        content: '[data-sidebar-content]',
        loading: '[data-sidebar-state="loading"]',
        empty: '[data-sidebar-state="empty"]',
        emptyMessage: '[data-sidebar-empty-message]',
        error: '[data-sidebar-state="error"]',
        errorMessage: '[data-sidebar-error-message]',
        retry: '[data-sidebar-retry]',
        login: '[data-sidebar-login]',
        menu: '[data-sidebar-menu]'
    };

    let elements = null;
    let menuTree = [];
    const apiPrefixes = window.AppConfig?.routes?.apiPrefixes || ['/bdDevs-crm', '/api'];

    const escapeHtml = (value) => String(value ?? '')
        .replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;')
        .replace(/"/g, '&quot;')
        .replace(/'/g, '&#39;');

    const renderIcon = (value) => escapeHtml(String(value || '?').trim().charAt(0).toUpperCase() || '?');

    const normalizePath = (menu) => {
        const fallbackDashboardPath = /^dashboard$/i.test(menu.menuName || '')
            ? (window.AppConfig?.routes?.dashboard || '/Home/Index')
            : '';
        let path = String(menu.menuPath || fallbackDashboardPath || '').trim();

        if (!path || /^javascript:/i.test(path)) {
            return '';
        }

        if (/^https?:\/\//i.test(path)) {
            return path;
        }

        path = path.replace(/\\/g, '/');

        while (path.startsWith('.')) {
            path = path.substring(1);
        }

        if (!path.startsWith('/')) {
            path = `/${path}`;
        }

        path = path.replace(/\/{2,}/g, '/');

        if (apiPrefixes.some((prefix) => path.toLowerCase() === prefix.toLowerCase() || path.toLowerCase().startsWith(`${prefix.toLowerCase()}/`))) {
            return '';
        }

        return path;
    };

    const normalizeMenus = (menus) => menus
        .filter((menu) => Number(menu?.isActive ?? 1) !== 0)
        .map((menu, index) => ({
            id: String(menu.menuId || `generated-${index + 1}`),
            parentId: menu.parentMenu ? String(menu.parentMenu) : '',
            menuName: String(menu.menuName || '').trim(),
            moduleName: String(menu.moduleName || 'General').trim() || 'General',
            menuPath: normalizePath(menu),
            sortOrder: Number(menu.sortOrder ?? index),
            searchText: [
                menu.menuName,
                menu.moduleName,
                menu.parentMenuName,
                menu.menuPath
            ].filter(Boolean).join(' ').toLowerCase()
        }))
        .filter((menu) => menu.menuName);

    const sortNodes = (nodes) => {
        nodes.sort((left, right) => {
            const orderDelta = left.sortOrder - right.sortOrder;
            return orderDelta !== 0
                ? orderDelta
                : left.menuName.localeCompare(right.menuName);
        });

        nodes.forEach((node) => sortNodes(node.children));
        return nodes;
    };

    const buildTree = (menus) => {
        const nodesById = new Map();
        const modules = new Map();

        menus.forEach((menu) => {
            nodesById.set(menu.id, { ...menu, children: [] });
        });

        nodesById.forEach((node) => {
            if (node.parentId && nodesById.has(node.parentId)) {
                nodesById.get(node.parentId).children.push(node);
                return;
            }

            if (!modules.has(node.moduleName)) {
                modules.set(node.moduleName, []);
            }

            modules.get(node.moduleName).push(node);
        });

        return Array.from(modules.entries())
            .map(([moduleName, items]) => ({
                moduleName,
                items: sortNodes(items).filter((node) => node.menuPath || node.children.length > 0)
            }))
            .filter((module) => module.items.length > 0)
            .sort((left, right) => left.moduleName.localeCompare(right.moduleName));
    };

    const filterNodes = (nodes, query) => nodes.reduce((result, node) => {
        const children = filterNodes(node.children, query);
        const isMatch = !query || node.searchText.includes(query);

        if (!isMatch && children.length === 0) {
            return result;
        }

        result.push({
            ...node,
            children
        });

        return result;
    }, []);

    const filterTree = (tree, query) => tree.reduce((result, section) => {
        const items = filterNodes(section.items, query);

        if (items.length > 0) {
            result.push({
                moduleName: section.moduleName,
                items
            });
        }

        return result;
    }, []);

    const renderNode = (node, depth = 0) => {
        const hasChildren = node.children.length > 0;
        const text = escapeHtml(node.menuName);
        const searchText = escapeHtml(node.searchText);

        if (!hasChildren && node.menuPath) {
            const linkClass = depth === 0 ? 'nav-link' : 'nav-sublink';
            const linkContent = depth === 0
                ? `<span class="nav-group__lead"><span class="nav-link__icon">${renderIcon(node.menuName)}</span><span class="nav-link__text">${text}</span></span>`
                : text;
            return `<a class="${linkClass}" href="${escapeHtml(node.menuPath)}" data-sidebar-link data-search-text="${searchText}">${linkContent}</a>`;
        }

        if (!hasChildren) {
            return '';
        }

        return `
            <button class="nav-group" type="button" data-nav-toggle aria-expanded="false" data-search-text="${searchText}">
                <span class="nav-group__lead">
                    <span class="nav-link__icon">${renderIcon(node.menuName)}</span>
                    <span class="nav-link__text">${text}</span>
                </span>
                <span class="nav-group__chevron" aria-hidden="true">⌄</span>
            </button>
            <div class="nav-group__items">
                ${node.children.map((child) => renderNode(child, depth + 1)).join('')}
            </div>
        `;
    };

    const normalizeCurrentPath = (value) => {
        if (!value) {
            return '/';
        }

        const url = new URL(value, window.location.origin);
        const normalized = url.pathname.replace(/\/+$/, '');
        return normalized || '/';
    };

    const isPathMatch = (currentPath, candidatePath) => {
        if (candidatePath === '/') {
            return currentPath === '/';
        }

        return currentPath === candidatePath || currentPath.startsWith(`${candidatePath}/`);
    };

    const showState = (state, message) => {
        const isContent = state === 'content';
        elements.loading.classList.toggle('is-hidden', state !== 'loading');
        elements.empty.classList.toggle('is-hidden', state !== 'empty');
        elements.error.classList.toggle('is-hidden', state !== 'error');
        elements.content.classList.toggle('is-hidden', !isContent);
        elements.search.disabled = !isContent;

        if (state === 'empty' && message) {
            elements.emptyMessage.textContent = message;
        }

        if (state === 'error' && message) {
            elements.errorMessage.textContent = message;
        }
    };

    const updateActiveState = () => {
        const currentPath = normalizeCurrentPath(window.location.pathname);
        const links = Array.from(elements.content.querySelectorAll('[data-sidebar-link]'));

        links.forEach((link) => link.classList.remove('is-active'));
        elements.content.querySelectorAll('[data-nav-toggle]').forEach((toggle) => {
            toggle.classList.remove('is-active');
            toggle.setAttribute('aria-expanded', 'false');
        });
        elements.content.querySelectorAll('.nav-group__items').forEach((panel) => {
            panel.classList.remove('is-open');
        });

        const bestMatch = links
            .map((link) => ({
                link,
                path: normalizeCurrentPath(link.getAttribute('href'))
            }))
            .filter(({ path }) => isPathMatch(currentPath, path))
            .sort((left, right) => right.path.length - left.path.length)[0];

        if (!bestMatch) {
            return;
        }

        bestMatch.link.classList.add('is-active');

        let panel = bestMatch.link.closest('.nav-group__items');
        while (panel) {
            panel.classList.add('is-open');

            const toggle = panel.previousElementSibling;
            if (toggle?.matches('[data-nav-toggle]')) {
                toggle.classList.add('is-active');
                toggle.setAttribute('aria-expanded', 'true');
            }

            panel = toggle?.closest('.nav-group__items');
        }
    };

    const renderTree = (tree, emptyMessage) => {
        if (!elements) {
            return;
        }

        if (!tree.length) {
            showState('empty', emptyMessage);
            return;
        }

        elements.content.innerHTML = tree.map((section) => `
            <div class="app-sidebar__section">
                <p class="app-sidebar__caption">${escapeHtml(section.moduleName)}</p>
                ${section.items.map((item) => renderNode(item)).join('')}
            </div>
        `).join('');

        showState('content');
        updateActiveState();
    };

    const applyFilter = () => {
        const query = elements.search.value.trim().toLowerCase();
        renderTree(
            query ? filterTree(menuTree, query) : menuTree,
            query ? 'No menu matches your search.' : 'No menu available.'
        );
    };

    const ensureAuthenticated = async () => {
        if (window.AuthManager?.ready) {
            await window.AuthManager.ready();
        }

        if (window.AuthManager?.isAuthenticated()) {
            return true;
        }

        return window.AuthManager?.refreshToken
            ? window.AuthManager.refreshToken()
            : false;
    };

    const loadMenus = async () => {
        if (!elements) {
            return;
        }

        showState('loading');
        elements.login.classList.add('is-hidden');

        try {
            const isAuthenticated = await ensureAuthenticated();

            if (!isAuthenticated) {
                showState('error', 'Session expired. Please login again.');
                elements.login.classList.remove('is-hidden');
                return;
            }

            const response = await window.ApiClient.get(
                window.AppConfig.endpoints.menusByUserPermission,
                { suppressUnauthorizedRedirect: true }
            );

            if (!response) {
                showState('error', 'Session expired. Please login again.');
                elements.login.classList.remove('is-hidden');
                return;
            }

            const normalizedMenus = normalizeMenus(Array.isArray(response?.data) ? response.data : []);
            menuTree = buildTree(normalizedMenus);
            applyFilter();
        } catch (error) {
            console.error('Sidebar loading error:', error);
            showState('error', error.message || 'Unable to load menu.');
        }
    };

    const bindEvents = () => {
        elements.retry.addEventListener('click', () => {
            loadMenus();
        });

        elements.search.addEventListener('input', () => {
            applyFilter();
        });

        elements.content.addEventListener('click', (event) => {
            const toggle = event.target.closest('[data-nav-toggle]');

            if (!toggle) {
                return;
            }

            const panel = toggle.nextElementSibling;
            const isExpanded = toggle.getAttribute('aria-expanded') === 'true';

            toggle.setAttribute('aria-expanded', String(!isExpanded));
            panel?.classList.toggle('is-open', !isExpanded);
        });
    };

    const init = () => {
        const menu = document.querySelector(selectors.menu);

        if (!menu) {
            return;
        }

        elements = {
            search: document.querySelector(selectors.search),
            content: document.querySelector(selectors.content),
            loading: document.querySelector(selectors.loading),
            empty: document.querySelector(selectors.empty),
            emptyMessage: document.querySelector(selectors.emptyMessage),
            error: document.querySelector(selectors.error),
            errorMessage: document.querySelector(selectors.errorMessage),
            retry: document.querySelector(selectors.retry),
            login: document.querySelector(selectors.login)
        };

        bindEvents();
        loadMenus();
    };

    document.addEventListener('DOMContentLoaded', init);

    return {
        init,
        reload: loadMenus
    };
})();
