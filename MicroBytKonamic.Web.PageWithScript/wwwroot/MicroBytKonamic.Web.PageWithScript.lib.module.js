const pageScriptInfoBySrc = new Map();

function registerPageScriptElement(src, params) {


    let pageScriptInfo = pageScriptInfoBySrc.get(src);

    if (pageScriptInfo) {
        pageScriptInfo.referenceCount++;
    } else {
        pageScriptInfo = {
            referenceCount: 1, module: null, params: params
        };
        pageScriptInfoBySrc.set(src, pageScriptInfo);
        initializePageScriptModule(src, pageScriptInfo);
    }
}

function unregisterPageScriptElement(src) {
    if (!src) {
        return;
    }

    const pageScriptInfo = pageScriptInfoBySrc.get(src);
    if (!pageScriptInfo) {
        return;
    }

    pageScriptInfo.referenceCount--;
}

async function initializePageScriptModule(src, pageScriptInfo) {
    if (src.startsWith("./")) {
        src = new URL(src.substr(2), document.baseURI).toString();
    }

    const module = await import(src);

    if (pageScriptInfo.referenceCount <= 0) {
        return;
    }

    pageScriptInfo.module = module;
    module.onLoad?.(pageScriptInfo.params);
    module.onUpdate?.();
}

function onEnhancedLoad() {
    for (const [src, { module, referenceCount }] of pageScriptInfoBySrc) {
        if (referenceCount <= 0) {
            module?.onDispose?.();
            pageScriptInfoBySrc.delete(src);
        }
    }

    for (const { module } of pageScriptInfoBySrc.values()) {
        module?.onUpdate?.();
    }
}

function getQueryParams(qs) {
    qs = qs.split('+').join(' ');

    var params = {},
        tokens,
        re = /[?&]?([^=]+)=([^&]*)/g;

    while (tokens = re.exec(qs)) {
        params[decodeURIComponent(tokens[1])] = decodeURIComponent(tokens[2]);
    }

    return params;
}

function getSrcSplit(src) {
    if (!src)
        return {
            src: src, params: {}
        };
    const url = (src.startsWith("./")) ? new URL(src.substr(2), document.baseURI) : new URL(src);
    const ret = { src: url.origin + url.pathname, params: getQueryParams(url.search) };

    return ret;
}

export function afterWebStarted(blazor) {
    customElements.define('page-script', class extends HTMLElement {
        static observedAttributes = ['src'];

        attributeChangedCallback(name, oldValue, newValue) {
            if (name !== 'src') {
                return;
            }

            if (!newValue) {
                throw new Error('Must provide a non-empty value for the "src" attribute.');
            }

            const unewValue = getSrcSplit(newValue);
            const uoldValue = getSrcSplit(oldValue);

            this.src = unewValue.src;
            unregisterPageScriptElement(uoldValue.src);
            registerPageScriptElement(unewValue.src, unewValue.params);
        }

        disconnectedCallback() {
            unregisterPageScriptElement(this.src);
        }
    });

    blazor.addEventListener('enhancedload', onEnhancedLoad);
}