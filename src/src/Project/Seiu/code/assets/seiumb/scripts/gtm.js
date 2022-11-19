function dataLayerPush(data, callerObject) {
    if (data != null) {
        if (data.clickHref == null || data.clickHref === "") {
            if (callerObject != null && callerObject.href != null && callerObject.href !== "") {
                data.clickHref = callerObject.href;
            }
        }
        var dataLayer = window.dataLayer = window.dataLayer || [];
        dataLayer.push(data);
    }
}