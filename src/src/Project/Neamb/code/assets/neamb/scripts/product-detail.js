function operationprocedureactioncta(programCode) {

    var url = '/api/ProductRoute/ExecuteStoredActionCta';

    $.ajax({
        type: 'POST',
        url: url,
        data: { programCode: programCode },
        dataType: 'json'
    });
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

function gaEventBuilder(imageUrl) {
	var urlArr = (location.pathname).split('/');
	var channelName = urlArr[1];
	var productName = (document.title).split('|', 1);
	var imageFilename = imageUrl.substr(imageUrl.lastIndexOf('/') + 1, imageUrl.length);
	var label = channelName + " > " + productName + " > " + imageFilename;
	ga_event(channelName, 'click', label);
}
