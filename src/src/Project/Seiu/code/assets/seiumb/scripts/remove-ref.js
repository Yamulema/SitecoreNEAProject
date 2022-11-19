function removeParams(sParam) {
    var url = window.location.href.split('?')[0] + '?';
    var result = false;
    var sPageURL = decodeURIComponent(window.location.search.substring(1)),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');
        if (sParameterName[0] !== sParam) {
            url = url + sParameterName[0] + '=' + sParameterName[1] + '&';
        }
        else {
            result = true;
        }
    }
    if (result) {
        window.location = url.substring(0, url.length - 1);
    }
}

document.addEventListener("DOMContentLoaded", function () {
    removeParams('ref');
});