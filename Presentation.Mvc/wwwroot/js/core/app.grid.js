window.AppGrid = (() => {
    const init = (config) => {
        if (window.kendo && window.jQuery && config?.selector) {
            window.jQuery(config.selector).kendoGrid(config.options ?? {});
            return window.jQuery(config.selector).data("kendoGrid");
        }

        return null;
    };

    const refresh = (selector) => {
        if (window.kendo && window.jQuery && selector) {
            const grid = window.jQuery(selector).data("kendoGrid");
            grid?.dataSource?.read?.();
        }
    };

    return { init, refresh };
})();
