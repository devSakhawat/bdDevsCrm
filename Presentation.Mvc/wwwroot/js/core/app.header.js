window.AppHeader = (() => {
    const selectors = {
        root: '[data-app-header]',
        quickLinks: '[data-header-quick-links]',
        quickLinksEmpty: '[data-header-quick-links-empty]',
        time: '[data-header-time]',
        date: '[data-header-date]',
        avatarImage: '[data-header-avatar-image]',
        avatarInitials: '[data-header-avatar-initials]',
        displayName: '[data-header-display-name]',
        displayMeta: '[data-header-display-meta]',
        company: '[data-header-company]',
        designation: '[data-header-designation]',
        orgInfo: '[data-header-org-info]',
        badge: '[data-header-badge]',
        panel: '[data-header-panel]',
        panelTitle: '[data-header-panel-title]',
        panelCount: '[data-header-panel-count]',
        panelList: '[data-header-panel-list]',
        panelEmpty: '[data-header-panel-empty]'
    };

    let elements = null;
    let clockTimer = null;
    let currentClockValue = null;

    const escapeHtml = (value) => String(value ?? '')
        .replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;')
        .replace(/"/g, '&quot;')
        .replace(/'/g, '&#39;');

    const isSafeUrl = (value) => {
        const normalized = String(value ?? '').trim();
        return /^https?:\/\//i.test(normalized) || normalized.startsWith('/');
    };

    const formatTime = (value) => value.toLocaleTimeString([], {
        hour: '2-digit',
        minute: '2-digit',
        second: '2-digit'
    });

    const formatDate = (value) => value.toLocaleDateString([], {
        weekday: 'short',
        year: 'numeric',
        month: 'short',
        day: 'numeric'
    });

    const setClock = (value) => {
        if (!elements) {
            return;
        }

        elements.time.textContent = formatTime(value);
        elements.date.textContent = formatDate(value);
    };

    const startClock = (serverTimeUtc) => {
        if (clockTimer) {
            window.clearInterval(clockTimer);
        }

        currentClockValue = serverTimeUtc ? new Date(serverTimeUtc) : new Date();
        setClock(currentClockValue);

        clockTimer = window.setInterval(() => {
            currentClockValue = new Date(currentClockValue.getTime() + 1000);
            setClock(currentClockValue);
        }, 1000);
    };

    const renderQuickLinks = (links) => {
        const items = Array.isArray(links) ? links.filter((link) => isSafeUrl(link?.url)) : [];

        if (!items.length) {
            elements.quickLinks.innerHTML = '';
            elements.quickLinksEmpty.classList.remove('is-hidden');
            return;
        }

        elements.quickLinksEmpty.classList.add('is-hidden');
        elements.quickLinks.innerHTML = items.map((link) => `
            <a class="header-quick-links__item" href="${escapeHtml(link.url)}" title="${escapeHtml(link.description || link.title)}">
                <span class="header-quick-links__item-title">${escapeHtml(link.title)}</span>
            </a>
        `).join('');
    };

    const renderUser = (user) => {
        const displayName = user?.displayName || 'User';
        const metaParts = [user?.designation, user?.companyName].filter(Boolean);
        const orgParts = [user?.departmentName, user?.branchName].filter(Boolean);

        elements.displayName.textContent = displayName;
        elements.displayMeta.textContent = metaParts.join(' • ') || 'Workspace access';
        elements.company.textContent = user?.companyName || 'Company not available';
        elements.designation.textContent = user?.designation || 'Designation not available';
        elements.orgInfo.textContent = orgParts.join(' • ') || 'Department / branch not available';
        elements.avatarInitials.textContent = user?.initials || 'U';

        if (user?.avatarUrl && isSafeUrl(user.avatarUrl)) {
            elements.avatarImage.src = user.avatarUrl;
            elements.avatarImage.alt = `${displayName} profile picture`;
            elements.avatarImage.classList.remove('is-hidden');
            elements.avatarInitials.classList.add('is-hidden');
        } else {
            elements.avatarImage.classList.add('is-hidden');
            elements.avatarImage.removeAttribute('src');
            elements.avatarInitials.classList.remove('is-hidden');
        }
    };

    const renderBadge = (key, count) => {
        const badge = elements.root.querySelector(`${selectors.badge}[data-key="${key}"]`);
        if (!badge) {
            return;
        }

        if (count > 0) {
            badge.textContent = String(count);
            badge.classList.remove('is-hidden');
        } else {
            badge.textContent = '0';
            badge.classList.add('is-hidden');
        }
    };

    const renderPanelItems = (panelElement, widget) => {
        const list = panelElement.querySelector(selectors.panelList);
        const empty = panelElement.querySelector(selectors.panelEmpty);
        const items = Array.isArray(widget?.items) ? widget.items : [];

        panelElement.querySelector(selectors.panelTitle).textContent = widget?.title || 'Panel';
        panelElement.querySelector(selectors.panelCount).textContent = String(widget?.count || 0);

        if (!items.length) {
            list.innerHTML = '';
            empty.textContent = widget?.emptyMessage || 'No items available.';
            empty.classList.remove('is-hidden');
            return;
        }

        empty.classList.add('is-hidden');
        list.innerHTML = items.map((item) => {
            const content = `
                <span class="header-panel__item-main">
                    ${item?.avatarUrl && isSafeUrl(item.avatarUrl)
                        ? `<img class="header-panel__avatar" src="${escapeHtml(item.avatarUrl)}" alt="${escapeHtml(item.title || 'Item')}" />`
                        : `<span class="header-panel__avatar header-panel__avatar--fallback">${escapeHtml(String(item?.title || '?').trim().charAt(0).toUpperCase())}</span>`}
                    <span class="header-panel__copy">
                        <strong>${escapeHtml(item?.title || 'Item')}</strong>
                        <small>${escapeHtml(item?.subtitle || '')}</small>
                    </span>
                </span>
                ${item?.badgeText ? `<span class="header-panel__badge">${escapeHtml(item.badgeText)}</span>` : ''}
            `;

            if (item?.url && isSafeUrl(item.url)) {
                return `<a class="header-panel__item" href="${escapeHtml(item.url)}">${content}</a>`;
            }

            return `<div class="header-panel__item">${content}</div>`;
        }).join('');
    };

    const renderPanels = (summary) => {
        const widgetMap = {
            notifications: summary?.notifications,
            approvals: summary?.pendingApprovals,
            messages: summary?.messages,
            birthdays: summary?.birthdays
        };

        Object.entries(widgetMap).forEach(([key, widget]) => {
            renderBadge(key, widget?.count || 0);

            const panel = elements.root.querySelector(`${selectors.panel}[data-key="${key}"]`);
            if (panel) {
                renderPanelItems(panel, widget);
            }
        });
    };

    const closePanels = () => {
        elements.root.querySelectorAll(selectors.panel).forEach((panel) => panel.classList.remove('is-open'));
        elements.root.querySelectorAll('[data-header-toggle]').forEach((button) => button.setAttribute('aria-expanded', 'false'));
    };

    const bindInteractions = () => {
        elements.root.querySelectorAll('[data-header-toggle]').forEach((button) => {
            button.addEventListener('click', (event) => {
                event.preventDefault();
                const key = button.getAttribute('data-header-toggle');
                const panel = elements.root.querySelector(`${selectors.panel}[data-key="${key}"]`);
                if (!panel) {
                    return;
                }

                const willOpen = !panel.classList.contains('is-open');
                closePanels();

                if (willOpen) {
                    panel.classList.add('is-open');
                    button.setAttribute('aria-expanded', 'true');
                }
            });
        });

        document.addEventListener('click', (event) => {
            if (!elements.root.contains(event.target)) {
                closePanels();
            }
        });

        document.addEventListener('keydown', (event) => {
            if (event.key === 'Escape') {
                closePanels();
            }
        });

        document.getElementById('btnLogout')?.addEventListener('click', () => {
            if (window.confirm('Are you sure you want to logout?')) {
                window.AuthManager?.logout?.();
            }
        });
    };

    const ensureAuthenticated = async () => {
        if (window.AuthManager?.ready) {
            await window.AuthManager.ready();
        }

        if (window.AuthManager?.isAuthenticated?.()) {
            return true;
        }

        return window.AuthManager?.refreshToken
            ? window.AuthManager.refreshToken()
            : false;
    };

    const loadSummary = async () => {
        try {
            const isAuthenticated = await ensureAuthenticated();
            if (!isAuthenticated) {
                startClock();
                return;
            }

            const response = await window.ApiClient.get(window.AppConfig.endpoints.headerSummary, {
                suppressUnauthorizedRedirect: true
            });

            const summary = response?.data;
            if (!summary) {
                startClock();
                return;
            }

            renderUser(summary.user);
            renderQuickLinks(summary.quickLinks);
            renderPanels(summary);
            startClock(summary.serverTimeUtc);
        } catch (error) {
            console.error('Header summary load failed:', error);
            startClock();
        }
    };

    const init = () => {
        elements = {
            root: document.querySelector(selectors.root),
            quickLinks: document.querySelector(selectors.quickLinks),
            quickLinksEmpty: document.querySelector(selectors.quickLinksEmpty),
            time: document.querySelector(selectors.time),
            date: document.querySelector(selectors.date),
            avatarImage: document.querySelector(selectors.avatarImage),
            avatarInitials: document.querySelector(selectors.avatarInitials),
            displayName: document.querySelector(selectors.displayName),
            displayMeta: document.querySelector(selectors.displayMeta),
            company: document.querySelector(selectors.company),
            designation: document.querySelector(selectors.designation),
            orgInfo: document.querySelector(selectors.orgInfo)
        };

        if (!elements.root) {
            return;
        }

        bindInteractions();
        loadSummary();
    };

    document.addEventListener('DOMContentLoaded', init);

    return { init };
})();
