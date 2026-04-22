/**
 * Advanced Session Management Module
 * Handles session timeout, idle detection, multi-tab sync, and auto token refresh
 */

window.SessionManager = (() => {
    'use strict';

    // Configuration
    const config = {
        // Session timeout in milliseconds (30 minutes default)
        sessionTimeout: 30 * 60 * 1000,

        // Idle timeout in milliseconds (15 minutes default)
        idleTimeout: 15 * 60 * 1000,

        // Warning time before session expires (2 minutes)
        warningTime: 2 * 60 * 1000,

        // Token refresh interval (refresh 5 minutes before expiry)
        tokenRefreshInterval: 25 * 60 * 1000,

        // Activity check interval (every 30 seconds)
        activityCheckInterval: 30 * 1000,

        // Multi-tab sync channel name
        syncChannelName: 'bdDevsCrm_session_sync',

        // LocalStorage key for last activity
        lastActivityKey: 'bdDevsCrm_last_activity',

        // LocalStorage key for session state
        sessionStateKey: 'bdDevsCrm_session_state'
    };

    // State
    let sessionTimer = null;
    let idleTimer = null;
    let warningTimer = null;
    let tokenRefreshTimer = null;
    let activityCheckTimer = null;
    let lastActivity = Date.now();
    let isWarningDisplayed = false;
    let broadcastChannel = null;

    /**
     * Initialize session manager
     */
    const init = () => {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            console.log('Session manager: User not authenticated, skipping initialization');
            return;
        }

        console.log('Initializing session manager...');

        // Setup activity listeners
        setupActivityListeners();

        // Setup multi-tab sync
        setupMultiTabSync();

        // Start session timers
        startSessionTimers();

        // Start token refresh timer
        startTokenRefreshTimer();

        // Start activity checker
        startActivityChecker();

        // Update last activity
        updateLastActivity();

        console.log('Session manager initialized');
    };

    /**
     * Setup activity listeners for user interactions
     */
    const setupActivityListeners = () => {
        const activityEvents = ['mousedown', 'keydown', 'scroll', 'touchstart', 'click'];

        activityEvents.forEach(event => {
            document.addEventListener(event, handleActivity, true);
        });
    };

    /**
     * Handle user activity
     */
    const handleActivity = () => {
        lastActivity = Date.now();
        updateLastActivity();

        // Reset idle timer
        resetIdleTimer();

        // Hide warning if displayed
        if (isWarningDisplayed) {
            hideSessionWarning();
        }

        // Broadcast activity to other tabs
        broadcastSessionActivity();
    };

    /**
     * Update last activity timestamp in localStorage
     */
    const updateLastActivity = () => {
        try {
            localStorage.setItem(config.lastActivityKey, lastActivity.toString());
        } catch (e) {
            console.warn('Failed to update last activity:', e);
        }
    };

    /**
     * Get last activity timestamp
     */
    const getLastActivity = () => {
        try {
            const stored = localStorage.getItem(config.lastActivityKey);
            return stored ? parseInt(stored) : Date.now();
        } catch (e) {
            return Date.now();
        }
    };

    /**
     * Setup multi-tab synchronization using BroadcastChannel
     */
    const setupMultiTabSync = () => {
        if (typeof BroadcastChannel === 'undefined') {
            console.warn('BroadcastChannel not supported, multi-tab sync disabled');
            return;
        }

        try {
            broadcastChannel = new BroadcastChannel(config.syncChannelName);

            broadcastChannel.onmessage = (event) => {
                const { type, data } = event.data;

                switch (type) {
                    case 'LOGOUT':
                        handleCrossTabLogout();
                        break;
                    case 'ACTIVITY':
                        handleCrossTabActivity(data);
                        break;
                    case 'TOKEN_REFRESH':
                        handleCrossTabTokenRefresh(data);
                        break;
                    default:
                        break;
                }
            };

            console.log('Multi-tab sync enabled');
        } catch (e) {
            console.warn('Failed to setup multi-tab sync:', e);
        }
    };

    /**
     * Broadcast session activity to other tabs
     */
    const broadcastSessionActivity = () => {
        if (broadcastChannel) {
            try {
                broadcastChannel.postMessage({
                    type: 'ACTIVITY',
                    data: { timestamp: Date.now() }
                });
            } catch (e) {
                console.warn('Failed to broadcast activity:', e);
            }
        }
    };

    /**
     * Broadcast logout to other tabs
     */
    const broadcastLogout = () => {
        if (broadcastChannel) {
            try {
                broadcastChannel.postMessage({
                    type: 'LOGOUT',
                    data: { timestamp: Date.now() }
                });
            } catch (e) {
                console.warn('Failed to broadcast logout:', e);
            }
        }
    };

    /**
     * Broadcast token refresh to other tabs
     */
    const broadcastTokenRefresh = (token) => {
        if (broadcastChannel) {
            try {
                broadcastChannel.postMessage({
                    type: 'TOKEN_REFRESH',
                    data: { token, timestamp: Date.now() }
                });
            } catch (e) {
                console.warn('Failed to broadcast token refresh:', e);
            }
        }
    };

    /**
     * Handle logout from another tab
     */
    const handleCrossTabLogout = () => {
        console.log('Logout detected from another tab');
        cleanup();
        window.ApiClient.clearToken();
        window.location.href = '/Account/Login';
    };

    /**
     * Handle activity from another tab
     */
    const handleCrossTabActivity = (data) => {
        lastActivity = data.timestamp;
        resetIdleTimer();

        if (isWarningDisplayed) {
            hideSessionWarning();
        }
    };

    /**
     * Handle token refresh from another tab
     */
    const handleCrossTabTokenRefresh = (data) => {
        if (data.token) {
            window.ApiClient.setToken(data.token);
            console.log('Token synchronized from another tab');
        }
    };

    /**
     * Start session timeout timer
     */
    const startSessionTimers = () => {
        // Clear existing timers
        clearSessionTimers();

        // Set warning timer (fires before session expires)
        warningTimer = setTimeout(() => {
            showSessionWarning();
        }, config.sessionTimeout - config.warningTime);

        // Set session timeout timer
        sessionTimer = setTimeout(() => {
            handleSessionTimeout();
        }, config.sessionTimeout);
    };

    /**
     * Reset idle timer
     */
    const resetIdleTimer = () => {
        if (idleTimer) {
            clearTimeout(idleTimer);
        }

        idleTimer = setTimeout(() => {
            handleIdleTimeout();
        }, config.idleTimeout);
    };

    /**
     * Clear all session timers
     */
    const clearSessionTimers = () => {
        if (sessionTimer) clearTimeout(sessionTimer);
        if (idleTimer) clearTimeout(idleTimer);
        if (warningTimer) clearTimeout(warningTimer);
        if (tokenRefreshTimer) clearTimeout(tokenRefreshTimer);
        if (activityCheckTimer) clearTimeout(activityCheckTimer);
    };

    /**
     * Handle session timeout
     */
    const handleSessionTimeout = () => {
        console.warn('Session timeout reached');
        window.AppToast?.warning('Session expired due to timeout');
        logout();
    };

    /**
     * Handle idle timeout
     */
    const handleIdleTimeout = () => {
        console.warn('Idle timeout reached');
        window.AppToast?.warning('Session expired due to inactivity');
        logout();
    };

    /**
     * Show session warning dialog
     */
    const showSessionWarning = () => {
        if (isWarningDisplayed) return;

        isWarningDisplayed = true;

        const warningHtml = `
            <div class="session-warning-overlay">
                <div class="session-warning-dialog">
                    <h3>⚠️ Session Expiring Soon</h3>
                    <p>Your session will expire in <strong>2 minutes</strong> due to inactivity.</p>
                    <p>Click anywhere or press any key to continue your session.</p>
                    <div class="session-warning-actions">
                        <button id="btnContinueSession" class="btn btn-primary">
                            Continue Session
                        </button>
                        <button id="btnLogoutNow" class="btn btn-secondary">
                            Logout Now
                        </button>
                    </div>
                </div>
            </div>
        `;

        const warningElement = document.createElement('div');
        warningElement.id = 'sessionWarning';
        warningElement.innerHTML = warningHtml;
        document.body.appendChild(warningElement);

        // Attach event handlers
        document.getElementById('btnContinueSession').addEventListener('click', () => {
            handleActivity();
            hideSessionWarning();
        });

        document.getElementById('btnLogoutNow').addEventListener('click', () => {
            logout();
        });

        console.log('Session warning displayed');
    };

    /**
     * Hide session warning dialog
     */
    const hideSessionWarning = () => {
        const warningElement = document.getElementById('sessionWarning');
        if (warningElement) {
            warningElement.remove();
            isWarningDisplayed = false;
            console.log('Session warning hidden');
        }
    };

    /**
     * Start automatic token refresh timer
     */
    const startTokenRefreshTimer = () => {
        if (tokenRefreshTimer) {
            clearTimeout(tokenRefreshTimer);
        }

        tokenRefreshTimer = setTimeout(async () => {
            await refreshTokenAutomatically();
        }, config.tokenRefreshInterval);
    };

    /**
     * Automatically refresh token before it expires
     */
    const refreshTokenAutomatically = async () => {
        console.log('Auto-refreshing token...');

        try {
            const success = await window.AuthManager.refreshToken();

            if (success) {
                const newToken = window.AuthManager.getCurrentToken();
                broadcastTokenRefresh(newToken);

                // Restart the refresh timer
                startTokenRefreshTimer();

                console.log('Token auto-refreshed successfully');
            } else {
                console.warn('Token auto-refresh failed, user will need to re-login');
            }
        } catch (error) {
            console.error('Error during auto token refresh:', error);
        }
    };

    /**
     * Start activity checker (monitors activity across tabs)
     */
    const startActivityChecker = () => {
        if (activityCheckTimer) {
            clearInterval(activityCheckTimer);
        }

        activityCheckTimer = setInterval(() => {
            const lastActivityTimestamp = getLastActivity();
            const timeSinceLastActivity = Date.now() - lastActivityTimestamp;

            // If no activity for idle timeout duration
            if (timeSinceLastActivity >= config.idleTimeout) {
                handleIdleTimeout();
            }
        }, config.activityCheckInterval);
    };

    /**
     * Logout with cleanup
     */
    const logout = async () => {
        // Broadcast logout to other tabs
        broadcastLogout();

        // Cleanup timers and listeners
        cleanup();

        // Call auth manager logout
        if (window.AuthManager) {
            await window.AuthManager.logout();
        }
    };

    /**
     * Cleanup session manager
     */
    const cleanup = () => {
        // Clear all timers
        clearSessionTimers();

        // Close broadcast channel
        if (broadcastChannel) {
            try {
                broadcastChannel.close();
            } catch (e) {
                console.warn('Error closing broadcast channel:', e);
            }
        }

        // Clear localStorage
        try {
            localStorage.removeItem(config.lastActivityKey);
            localStorage.removeItem(config.sessionStateKey);
        } catch (e) {
            console.warn('Error clearing localStorage:', e);
        }

        console.log('Session manager cleaned up');
    };

    /**
     * Extend session (manual refresh)
     */
    const extendSession = async () => {
        handleActivity();
        await refreshTokenAutomatically();
        window.AppToast?.success('Session extended successfully');
    };

    /**
     * Get session info for debugging/testing
     */
    const getSessionInfo = () => {
        return {
            lastActivity,
            isWarningDisplayed,
            isAuthenticated: window.AuthManager?.isAuthenticated() || false,
            timeSinceLastActivity: Date.now() - lastActivity,
            config: { ...config }
        };
    };

    // Public API
    return {
        init,
        cleanup,
        logout,
        extendSession,
        getSessionInfo,
        updateConfig: (newConfig) => {
            Object.assign(config, newConfig);
            console.log('Session config updated:', config);
        }
    };
})();

const initializeSessionManager = async () => {
    if (window.AuthManager?.ready) {
        await window.AuthManager.ready();
    }

    if (window.AuthManager && window.AuthManager.isAuthenticated()) {
        window.SessionManager.init();
    }
};

if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => {
        initializeSessionManager();
    });
} else {
    initializeSessionManager();
}
