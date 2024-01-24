var _unityInstance = null;

export function onLoad(params) {
    runPlayer(params);
}

export function onUpdate() {
}

export function onDispose() {
    console.log('Disposed');
    if (_unityInstance != null)
        _unityInstance.Quit();
}

function runPlayer(params) {
    //alert(params.p)
    var container = document.querySelector("#unity-container");
    var canvas = document.querySelector("#unity-canvas");
    var loadingBar = document.querySelector("#unity-loading-bar");
    var progressBarFull = document.querySelector("#unity-progress-bar-full");
    var fullscreenButton = document.querySelector("#unity-fullscreen-button");
    var rungameButton = document.querySelector("#unity-rungame-button");
    var warningBanner = document.querySelector("#unity-warning");
    var warningBanner = document.querySelector("#unity-warning");

    // Shows a temporary message banner/ribbon for a few seconds, or
    // a permanent error message on top of the canvas if type=='error'.
    // If type=='warning', a yellow highlight color is used.
    // Modify or remove this function to customize the visually presented
    // way that non-critical warnings and error messages are presented to the
    // user.
    function unityShowBanner(msg, type) {
        function updateBannerVisibility() {
            warningBanner.style.display = warningBanner.children.length ? 'block' : 'none';
        }
        var div = document.createElement('div');
        div.innerHTML = msg;
        warningBanner.appendChild(div);
        if (type == 'error') div.style = 'background: red; padding: 10px;';
        else {
            if (type == 'warning') div.style = 'background: yellow; padding: 10px;';
            setTimeout(function () {
                warningBanner.removeChild(div);
                updateBannerVisibility();
            }, 5000);
        }
        updateBannerVisibility();
    }

    var buildUrl = params.buildUrl;
    var loaderUrl = buildUrl + params.loaderFilename;
    var config = {
        dataUrl: buildUrl + params.dataFilename,
        frameworkUrl: buildUrl + params.frameworkFilename,
        streamingAssetsUrl: "StreamingAssets",
        companyName: "MicroBytKonamic",
        productName: params.productName,
        productVersion: params.productVersion,
        showBanner: unityShowBanner,
    };

    if (params.workerFilename)
        config.workerUrl = buildUrl + params.workerFilename;
    if (params.codeFilename)
        config.codeUrl = buildUrl + params.codeFilename;
    if (params.memoryFilename)
        config.memoryUrl = buildUrl + params.memoryFilename;
    if (params.symbolsFilename)
        config.symbolsUrl = buildUrl + params.symbolsFilename;

    // By default, Unity keeps WebGL canvas render target size matched with
    // the DOM size of the canvas element (scaled by window.devicePixelRatio)
    // Set this to false if you want to decouple this synchronization from
    // happening inside the engine, and you would instead like to size up
    // the canvas DOM size and WebGL render target sizes yourself.
    // config.matchWebGLToCanvasSize = false;

    if (/iPhone|iPad|iPod|Android/i.test(navigator.userAgent)) {
        // Mobile device style: fill the whole browser client area with the game canvas:

        var meta = document.createElement('meta');
        meta.name = 'viewport';
        meta.content = 'width=device-width, height=device-height, initial-scale=1.0, user-scalable=no, shrink-to-fit=yes';
        document.getElementsByTagName('head')[0].appendChild(meta);
        container.className = "unity-mobile";
        canvas.className = "unity-mobile";

        // To lower canvas resolution on mobile devices to gain some
        // performance, uncomment the following line:
        // config.devicePixelRatio = 1;


    } else {
        // Desktop style: Render the game canvas in a window that can be maximized to fullscreen:

        if (params.width)
            canvas.style.width = params.width + "px";
        else
            canvas.style.width = "1024px";
        if (params.height)
            canvas.style.height = params.height + "px";
        else
            canvas.style.height = "768px";
    }

    if (params.showRunGameButton) {
        rungameButton.style.display = "block";
        rungameButton.onclick = () => {
            rungameButton.style.display = "none";
            //if (canvas.className == "unity-mobile")
            prepareScreenMobile(true, "landscape");
            runGame();
        };
    }
    else runGame();

    function prepareScreenMobile(fullScreen, orientation) {

    }

    function runGame() {
        if (params.backGroundFilename)
            canvas.style.background = "url('" + buildUrl + params.backGroundFilename.replace(/'/g, '%27') + "') center / cover";
        loadingBar.style.display = "block";

        var script = document.createElement("script");

        script.src = loaderUrl;
        script.onload = () => {
            if (params.showRunGameButton && canvas.className == "unity-mobile") {
                if ("wakeLock" in navigator)
                    navigator.wakeLock.request("screen");
                //unityInstance.SetFullscreen(1);
                screen.orientation.lock("landscape");
            }
            createUnityInstance(canvas, config, (progress) => {
                progressBarFull.style.width = 100 * progress + "%";
            }).then((unityInstance) => {
                _unityInstance = unityInstance;
                if (params.showRunGameButton && canvas.className == "unity-mobile") {
                    unityInstance.SetFullscreen(1);
                }
                loadingBar.style.display = "none";
                fullscreenButton.onclick = () => {
                    unityInstance.SetFullscreen(1);
                };
            }).catch((message) => {
                alert(message);
            });
        };

        document.body.appendChild(script);
    }
}