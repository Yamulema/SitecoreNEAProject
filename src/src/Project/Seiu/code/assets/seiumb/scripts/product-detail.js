function executelink(contextitemid, calllinkid, actiontarget, postparameterid) {
    $("#contextitemid").val(contextitemid);
    $("#calllinkid").val(calllinkid);
    $("#postparameterid").val(postparameterid);
    $("#executelinkForm").prop("target", actiontarget);
    $("#executelinkForm").submit();

}

function downloadpdf(materialId, mdsid) {
    $("#materialId").val(materialId);
    $("#mdsid").val(mdsid);
    $("#downloadPdfForm").submit();

}

function gettruecarurl() {
    $("#truecarForm").submit();
}

function closeLoginModal() {
    var $modal = $('#loginModal');
    $modal.foundation('close');
}
function setactionlogin(logintype, storeid, redirecturlid, hotdeal, ctaText) {
    $("#loginAjaxProcess").val(logintype);
    $("#storeid").val(storeid);
    $("#redirecturlid").val(redirecturlid);
    $("#hotdeal").val(hotdeal);
    $("#ctatext").val(ctaText);
}
function operationprocedure(programCode, mdsid) {

    var url = '/api/ProductApi/ExecuteStoredProcedureOrder';

    $.ajax({
        type: 'POST',
        url: url,
        data: { programCode: programCode, mdsid: mdsid },
        dataType: 'json'
    });
}

function gaEventBuilder(imageUrl) {
    var urlArr = (location.pathname).split('/');
    var channelName = urlArr[1];
    var productName = (document.title).split('|', 1);
    var imageFilename = imageUrl.substr(imageUrl.lastIndexOf('/') + 1, imageUrl.length);
    var label = channelName + " > " + productName + " > " + imageFilename;
    ga_event(channelName, 'click', label);
}

function setvariables(url, kind, title, par1, par2, materialid, productname, buttontype, contextitemid, calllinkid, actiontarget, postparameterid) {
    $("#actionurl").val(url);
    $("#actionkind").val(kind);
    $("#actiontitle").val(title);
    $("#actionprocedurepar1").val(par1);
    $("#actionprocedurepar2").val(par2);
    $("#materialid").val(materialid);
    $("#productname").val(productname);
    $("#buttontype").val(buttontype);
    $("#contextitemidPar").val(contextitemid);
    $("#calllinkidPar").val(calllinkid);
    $("#actiontargetPar").val(actiontarget);
    $("#postparameteridPar").val(postparameterid);
}

function operationloginajax(username, password) {
    var result = false;
    //var url = 'http://integ-seiumb.neamb.com/api/LoginForm/LoginFormAjax';
    var url = '/api/LoginForm/LoginFormAjax';
    var contextitemid = $("#itemcontextid").val();
    var loginAjaxProcess = $("#loginAjaxProcess").val();
    var previousPage = $('#PreviousPage').val();
    var storeid = $("#storeid").val();
    var redirecturlid = $("#redirecturlid").val();
    var hotdeal = $("#hotdeal").val();
    var ctatext = $("#ctatext").val();

    $.ajax({
        type: 'POST',
        url: url,
        async: false,
        data: {
            Username: username, Password: password, ContextItemId: contextitemid,
            LoginAjaxProcess: loginAjaxProcess, StoreId: storeid,
            RedirectUrlId: redirecturlid,
            HotDeal: hotdeal,
            CtaText: ctatext
        },
        dataType: 'json',
        error: function (data) {
            $("#loginAjaxProcess").val('');
            $("#storeid").val('');
            $("#redirecturlid").val('');
            $("#hotdeal").val('');
        },
        success: function (data) {

            var urldepured = "";
            if (previousPage != undefined && !previousPage.length == 0) {
                urldepured = previousPage;
            }
            else {
                urldepured = window.location.protocol + '//' + window.location.host + window.location.pathname;
            }

            if (data.result == "ok" && $("#logiccta").val() == "1") {

                var actionurl = $("#actionurl").val();
                var actionkind = $("#actionkind").val();
                var actiontitle = $("#actiontitle").val();
                var actionprocedurepar1 = $("#actionprocedurepar1").val();
                var actionprocedurepar2 = $("#actionprocedurepar2").val();
                var materialid = $("#materialid").val();
                var productname = $("#productname").val();
                var buttontype = $("#buttontype").val();
                var contextitemidPar = $("#contextitemidPar").val();
                var calllinkidPar = $("#calllinkidPar").val();
                var actiontargetPar = $("#actiontargetPar").val();
                var postparameteridPar = $("#postparameteridPar").val();
                var primaryEventText = "product cta";
                var secondaryEventText = "secondary cta";
                if (buttontype == "1") {
                    dataLayerPush({ 'event': primaryEventText, 'productName': productname, 'ctaText': actiontitle, 'clickHref': actionurl }, this);
                } else if (buttontype == "2") {
                    dataLayerPush({ 'event': secondaryEventText, 'productName': productname, 'ctaText': actiontitle, 'clickHref': actionurl }, this);
                }
                if (actionkind == "1") {
                    executelink(contextitemidPar, calllinkidPar, actiontargetPar, postparameteridPar);
                    //$("#submitLogin").attr("href", actionurl);
                    operationprocedure(actionprocedurepar1, actionprocedurepar2);
                    result = true;
                }
                else if (actionkind == "2") {
                    downloadpdf(materialid, actionprocedurepar2);
                    result = false;
                }
                else if (actionkind == "3") {
                    operationprocedure(actionprocedurepar1, actionprocedurepar2);
                    $('#partnerformdesktop').submit();
                    result = false;
                }
                else if (actionkind == "4") {
                    cnsinvitation('partnerformdesktop');
                    operationprocedure(actionprocedurepar1, actionprocedurepar2);
                    $('#partnerformdesktop').submit();
                    resetinvitation('partnerformdesktop');
                    result = false;
                }
                else if (actionkind == "5") {
                    operationprocedure(actionprocedurepar1, actionprocedurepar2);
                    gettruecarurl();
                    result = false;
                }
                if (data.type == "RakutenStore" && data.store != undefined && data.store != '') {
                    $("#loginAjaxProcess").val('');
                    $("#storeid").val('');
                    $("#redirecturlid").val('');
                    $("#hotdeal").val('');

                    if (data.gtm != undefined && data.gtm != '') {
                        eval(data.gtm);
                    }
                    operationprocedure(actionprocedurepar1, actionprocedurepar2);
                    executestoreredirectionseiumb(data.store);
                }
                if (data.url != undefined && data.url != '') {
                    setTimeout(function () {
                        window.location.href = data.url
                    }, 3000);
                } else {
                    setTimeout(function () {
                        window.location.href = urldepured
                    }, 3000);
                }

            }
            else if (data.result == "error") {
                $('#login-modal-errors').removeClass("hide");
                $('#login-modal-errors').html("<p>" + data.errorMessage + "</p>");
            }
            else if (data.result == "duplicated") {
                window.location.href = data.url;
            }
            else if (data.result == "errorRakutenMember" && data.type == "RakutenStore") {
                var actionprocedurepar1AuxMem = $("#actionprocedurepar1").val();
                var actionprocedurepar2AuxMem = $("#actionprocedurepar2").val();
                operationprocedure(actionprocedurepar1AuxMem, actionprocedurepar2AuxMem);

                if (data.url != undefined && data.url != '') {
                    window.location.href = data.url + '?result=1&store=' + data.store;
                } else {
                    window.location.href = urldepured + '?result=1&store=' + data.store;
                }
            }
            else if (data.result == "errorRakutenMember" && data.type == "RakutenNoVendor") {
                var actionprocedurepar1AuxNoVen = $("#actionprocedurepar1").val();
                var actionprocedurepar2AuxNoVen = $("#actionprocedurepar2").val();
                operationprocedure(actionprocedurepar1AuxNoVen, actionprocedurepar2AuxNoVen);

                if (data.url != undefined && data.url != '') {
                    window.location.href = data.url + '?result=3';
                } else {
                    window.location.href = urldepured + '?result=3';
                }
            }
            else if (data.result == "errorRakutenEligible") {
                var actionprocedurepar1AuxEligible = $("#actionprocedurepar1").val();
                var actionprocedurepar2AuxEligible = $("#actionprocedurepar2").val();
                operationprocedure(actionprocedurepar1AuxEligible, actionprocedurepar2AuxEligible);

                if (data.url != undefined && data.url != '') {
                    window.location.href = data.url + '?result=2';
                } else {
                    window.location.href = urldepured + '?result=2';
                }
            }
            else {
                if (data.type == "RakutenStore" && data.store != undefined && data.store != '') {
                    if (data.gtm != undefined && data.gtm != '') {
                       eval(data.gtm);
                    }
                    var actionprocedurepar1Aux = $("#actionprocedurepar1").val();
                    var actionprocedurepar2Aux = $("#actionprocedurepar2").val();
                    operationprocedure(actionprocedurepar1Aux, actionprocedurepar2Aux);
                    executestoreredirectionseiumb(data.store);
                }

                if (data.url != undefined && data.url != '') {
                    setTimeout(function () {
                        window.location.href = data.url
                    }, 3000);
                } else {
                    setTimeout(function () {
                        window.location.href = urldepured
                    }, 3000);
                }
            }
        }
    });
    return result;
}

function trackingGoalProduct(productId, goalId) {

    var url = '/api/ProductRoute/ExecuteGoalTracking';

    $.ajax({
        type: 'POST',
        url: url,
        data: {
            productId: productId,
            goalId: goalId
        },
        dataType: 'json'
    });
}
