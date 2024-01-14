export function onLoad() {
    console.log('Loaded');
}

export function onUpdate() {
    console.log('Updated');
    //const p = getQueryParams(location.search);
    //const p = location.search;
    RunPlayer(p);
}

export function onDispose() {
    console.log('Disposed');
}

function RunPlayer(o) {
    alert(p)
}