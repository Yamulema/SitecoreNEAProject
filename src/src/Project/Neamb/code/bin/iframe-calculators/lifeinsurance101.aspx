<!--
To change this template, choose Tools | Templates
and open the template in the editor.
-->
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html>

<head>
	<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
	<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">

	<meta name="viewport" content="width=device-width, initial-scale=1.0">

	<title>NEA MB: Insurance Needs Estimator</title>

	<script src="//ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
	<script src="//ajax.googleapis.com/ajax/libs/jqueryui/1.9.2/jquery-ui.min.js"></script>
	<script src="https://cdnjs.cloudflare.com/ajax/libs/lodash.js/1.3.1/lodash.legacy.min.js"></script>
	<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa"
	 crossorigin="anonymous"></script>
	<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u"
	 crossorigin="anonymous">


	<style type="text/css">
		body {
			background-color: rgb(244, 241, 232);
		}

		body.wide {
			background-color: #fff;
		}

		.column {
			clear: none;
			display: inline;
			float: left;
			margin-right: 10px;
		}

		a,
		a:link,
		a:active,
		a:visited {
			color: #003366;
		}

		a:hover {
			color: #8B8178;
		}

		td {
			font-family: Tahoma, Verdana;
			font-size: 12px;
			padding: 5px 0;
		}

		input {
			font-family: Tahoma, Verdana;
			font-size: 12px;
		}

		option {
			font-family: Tahoma, Verdana;
			font-size: 12px;
		}

		select {
			font-family: Tahoma, Verdana;
			font-size: 12px;
		}

		#tabs_insurance_calculator.article_box {
			margin: 15px 0 0 0px;
			font-family: Tahoma, Verdana;
		}

		#tabs_insurance_calculator.article_box p {
			font-size: 12px;
			margin: 0px 0 0 0;
			text-align: left;
		}

		#tabs_insurance_calculator.article_box p em {
			font-style: italic;
		}

		#tabs_insurance_calculator.article_box ol {
			font-size: 12px;
		}

		#tabs_insurance_calculator.article_box ul {
			font-size: 12px;
		}

		#tabs_insurance_calculator.article_box h2 {
			font-family: Verdana, Arial, Helvetica, sans-serif;
			font-size: 15px;
			font-weight: bold;
		}

		#tabs_insurance_calculator.article_box h3 {
			font-size: 10px;
			font-style: italic;
			margin: 10px 0;
		}

		#calculator_intro.article_box {
			margin: -2px 0 0 0px;
			font-family: Tahoma, Verdana;
			padding-left: 12px;
		}

		#calculator_intro.article_box p {
			font-size: 12px;
			margin: 0px 0 0 0;
			text-align: left;
		}

		#calculator_intro.article_box p em {
			font-style: italic;
		}

		#calculator_intro.article_box ol {
			font-size: 12px;
		}

		#calculator_intro.article_box ul {
			font-size: 12px;
		}

		#calculator_intro.article_box h2 {
			font-family: Verdana, Arial, Helvetica, sans-serif;
			font-size: 15px;
			font-weight: bold;
		}

		#calculator_intro.article_box h3 {
			font-size: 10px;
			font-style: italic;
			margin: 10px 0;
		}

		#tabs_insurance_calculator .article_top {
			height: 30px;
			width: 100%;
		}

		.wide #tabs_insurance_calculator .article_top {
			left: 48%;
			position: relative;
			width: 400px;
		}



		#tabs_insurance_calculator .article_top a {
			font-size: 12px;
			color: #686a6d;
		}

		#tabs_insurance_calculator .article_mid {}

		#tabs_insurance_calculator .article_mid h2 {
			color: #003366;
		}

		#tabs_insurance_calculator .article_mid a {
			color: #003366;
		}

		#tabs_insurance_calculator .article_mid {
			/*width:325px;*/
			margin: 0;
			clear: left;
			padding-left: 12px;
			padding-right: 20px;
		}

		#tabs_insurance_calculator .article_bottom {}

		@media screen and (-webkit-min-device-pixel-ratio:0) {
			#tabs_insurance_calculator .article_bottom {
				margin-top: -5px;
			}
		}

		#tabs_insurance_calculator .ui-tabs .ui-tabs-nav {
			margin: 0;
			padding: 2px 4px 0;
		}

		#tabs_insurance_calculator .ui-tabs .ui-tabs-hide {
			display: none !important;
		}

		#tabs_insurance_calculator .ui-tabs .ui-tabs-panel {
			background: none repeat scroll 0 0 transparent;
			border-width: 0;
			display: block;
			padding: 1em 1.4em;
		}

		#tabs_insurance_calculator .ui-tabs .ui-tabs-nav li {
			border-bottom: 0 none !important;
			float: left;
			list-style: none outside none;
			margin: 0 0.2em 1px 0;
			padding: 0;
			position: relative;
			top: -8px;
			width: auto;
			height: 22px;
			text-align: center;
			line-height: 21px;
		}

		#tabs_insurance_calculator .ui-tabs .ui-tabs-nav li.ui-tabs-selected a,
		#tabs_insurance_calculator .ui-tabs .ui-tabs-nav li.ui-state-disabled a,
		#tabs_insurance_calculator .ui-tabs .ui-tabs-nav li.ui-state-processing a {
			cursor: text;
		}

		#tabs_insurance_calculator .ui-tabs .ui-tabs-nav li a,
		#tabs_insurance_calculator .ui-tabs.ui-tabs-collapsible .ui-tabs-nav li.ui-tabs-selected a {
			cursor: pointer;
		}

		#tabs_insurance_calculator .ui-tabs .ui-tabs-nav li a {
			/*float:left;
padding:0.5em 1em;*/
			text-decoration: none;
		}

		#tabs_insurance_calculator .ui-state-default a,
		#tabs_insurance_calculator .ui-state-default a:link,
		.ui-state-default a:visited {
			color: #fff;
			font-weight: normal;
			outline: medium none;
			text-decoration: none;
			padding: 5px;
			cursor: default !important;
			text-shadow: 1px 1px 2px #292929;
			filter: dropshadow(color=#292929, offx=1, offy=1);
		}

		#tabs_insurance_calculator .ui-state-active a,
		#tabs_insurance_calculator .ui-state-active a:link,
		.ui-state-active a:visited {
			color: #fff;
			font-weight: bold;
			outline: medium none;
			text-decoration: none;
			cursor: default !important;
		}

		#tabs_insurance_calculator .ui-state-default,
		#tabs_insurance_calculator .ui-widget-content .ui-state-default {
			background: #999;
			font-weight: bold;
			outline: medium none;
			width: auto;
		}

		#tabs_insurance_calculator .ui-state-active,
		#tabs_insurance_calculator .ui-widget-content .ui-state-active {
			background: #666;
			font-weight: bold;
			outline: medium none;
		}

		#tabs_insurance_calculator .img-calc-wide,
		#calculator_intro .img-calc-wide {
			display: none;
		}

		.wide #tabs_insurance_calculator .img-calc-wide {
			display: inline;
			float: left;
			width: 420px;
		}

		.wide #calculator_intro .img-calc-wide {
			display: inline;
			float: left;
			width: 420px;
		}

		#tabs_insurance_calculator .ui-tabs-nav {
			list-style: none outside none;
			margin: 0;
			padding: 0;
		}

		#tabs_insurance_calculator .ui-tabs-nav li {
			background: none repeat scroll 0 0 #999;
			float: left;
			font-weight: bold;
			margin: 0 1px 0 0;
			outline: medium none;
			padding: 5px;
		}

		#tabs_insurance_calculator .ui-tabs-nav li.ui-state-active {
			background: none repeat scroll 0 0 #666;
		}

		#tabs_insurance_calculator .right {
			float: right;
		}

		#tabs_insurance_calculator .left {
			float: left;
		}

		#tabs_insurance_calculator .next-tab,
		#tabs_insurance_calculator .prev-tab,
		#calculator_intro .next,
		#tabs_insurance_calculator .prev {
			font-size: 12px;
		}

		.wide #tabs_insurance_calculator .column.half {
			position: relative;
			top: -55px;
			width: 48%;
		}

		.wide #calculator_intro .column.half {
			position: relative;
			top: 0px;
			width: 48%;
		}

		.wide #tabs_insurance_calculator .column.half.content,
		.wide #calculator_intro .column.half.content {
			top: 0px;
		}

		#btSubmit,
		#btSubmit2 {
			color: #FFFFFF;
			cursor: pointer;
			font-weight: normal;
			outline: medium none;
			padding: 5px;
			text-decoration: none;
			text-shadow: 1px 1px 2px #292929;
			background: none repeat scroll 0 0 #999999;
			/* float: left;*/
			font-weight: bold;
			margin: 0 1px 0 0;
			outline: medium none;
			padding: 5px;
			height: 30px;
			width: 70px;
			border: 1px solid #d2d2d2;
			padding: 4px 5px 7px;
			width: auto;
		}

		#btSubmit:hover,
		#btSubmit2:hover {
			color: #FFFFFF;
			font-weight: bold;
			outline: medium none;
			text-decoration: none;
			background: none repeat scroll 0 0 #666666;
			padding: 4px 5px 7px;
		}

		@media only screen and (max-width: 991px) {
			.wide #tabs_insurance_calculator .article_top {
				left: 0;
			}
			.wide #tabs_insurance_calculator .column.half.content,
			.wide #calculator_intro .column.half.content {
				width: 100%;
			}
			.wide #tabs_insurance_calculator .column.half{
				top:0;
				width: 100%;
			}
			.wide #tabs_insurance_calculator .img-calc-wide{
				float:none;
				margin: 30px 0 10px 0;
    			display: block;
			}
		}
	</style>
	<script type="text/javascript">
		$(document).ready(function () {
			if (window.location.hash) {
				// Fragment exists
				$('#calculator_intro').hide();
				$('#tabs_insurance_calculator').show();
			} else {
				// Fragment doesn't exist
			}
			var allowTabChange = false;//false 
			var $tabs = $('#tabs_insurance_calculator').tabs();

			$tabs.tabs({
				select: function (event, ui) { return allowTabChange; },
				show: function (event, ui) { allowTabChange = false; }
			});

			$("#tabs_insurance_calculator .ui-tabs-panel .column.content").each(function (i) {
				var totalSize = $("#tabs_insurance_calculator .ui-tabs-panel").size() - 1;
				if (i != 0 && i != totalSize) {
					prev = i;
					$(this).append("<a href='#' class='prev-tab left btn' style='margin: 10px 0; background: #337ab7; color:white;' rel='" + prev + "'>&#171; Prev Step</a>");
				}
				if (i != totalSize) {
					next = i + 2;
					if (next == totalSize + 1) {
						//$(this).append("<a href='#' class='next-tab right'  rel='" + next + "'>Next Step &#187;</a><div class='clear'></div>");
					}
					else {
						$(this).append("<a href='#' class='next-tab right btn' style='margin: 10px 0; background: #337ab7; color:white;' rel='" + next + "'>Next Step &#187;</a><div class='clear'></div>");
					}
				}
			});
			$('.next-tab').click(function () { offsetTab(1); return false; });
			$('.prev-tab').click(function () { offsetTab(-1); return false; });

			// Helper functions
			function offsetTab(offset) {
				var tab_num = $tabs.tabs('option', 'selected');
				var nextTab = tab_num + offset;
				console.log('offset tab');

				if (check_table()) {
					if (tab_num == 4 && nextTab == 5) {
						allowTabChange = true;
						$tabs.tabs('select', nextTab);
						//alert('ajax or something to change to results tab')
						//return false;
					}
					else {
						allowTabChange = true;
						$tabs.tabs('select', nextTab);
					}
				}
			}
			//    $('.next-tab, .prev-tab').click(function() {
			//        $tabs.tabs('select', $(this).attr("rel"));
			//        return false;
			//    });
			$('#btSubmit').click(function () {
				console.log('submit 1');
				offsetTab(1);
			});

			$('#btSubmit2').click(function () {
				console.log('submit 2');
				offsetTab(-4);
			});

			$('#start-over').click(function () {
				console.log('star over');
				offsetTab(-5);
				$('#calculator_intro').show();
				$('#tabs_insurance_calculator').hide();
			});


			$('#calculator_intro .lets-begin.next').click(function () {
				$('#calculator_intro').hide();
				$('#tabs_insurance_calculator').show();
			});
			$('#tabs_insurance_calculator .lets-begin.prev').click(function () {
				$('#calculator_intro').show();
				$('#tabs_insurance_calculator').hide();
			})
		});

		function check_table() {
			rdFields[0] = "clcrdCh13";
			rdFields[1] = "clcrdCh23";
			rdFields[2] = "clcrdCh33";
			ovFields[0] = "clcChild1Amount";
			ovFields[1] = "clcChild2Amount";
			ovFields[2] = "clcChild3Amount";
			requiredFields[0] = "clcAnnualFamilyIncome"
			requiredFields[1] = "clcAnnualIncome";
			requiredFields[2] = "clcYears";
			requiredFields[3] = "clcFinalExpenses";
			requiredFields[4] = "clcMajorDebt";
			requiredFields[5] = "clcSavings";
			requiredFields[6] = "clcRetirementSavings";
			requiredFields[7] = "clcCurrentLifeInsurance";
			requiredFields[8] = "clcSocialSecurity";
			requiredFields[9] = "clcSpouseIncome";
			requiredFields[10] = "xxx1";
			requiredFields[11] = "xxx2";
			requiredFields[12] = "xxx3";
			fieldNames[0] = "Question 1";
			fieldNames[1] = "Question 2";
			fieldNames[2] = "Question 3";
			fieldNames[3] = "Question 4";
			fieldNames[4] = "Question 5";
			fieldNames[5] = "Question 7";
			fieldNames[6] = "Question 8";
			fieldNames[7] = "Question 9";
			fieldNames[8] = "Question 10";
			fieldNames[9] = "Question 11";
			fieldNames[10] = "Child age 1";
			fieldNames[11] = "Child age 2";
			fieldNames[12] = "Child age 3";
			flag = checkRequiredFields();
			if (flag) {
				return true;
			}
			else {
				return false;
			}

		}
		//original validation script
		function Trim(inputString) {
			var position = 0;
			while (inputString.charAt(position) == " ") {
				position++;
			}
			if (position == inputString.length) {
				return ("");
			}
			//Now position points to first char
			var endpos = inputString.length - 1
			while ((inputString.charAt(endpos) == " ") && (endpos >= 0)) {
				endpos--;
			}
			//alert(position + " " + endpos);
			return (inputString.substring(position, endpos + 1));
		}
		function isNumeric(strNumber) {
			var bNumber = false;
			for (i = 0; i < strNumber.length; i++) {
				bNumber = false;
				if (strNumber.charAt(i) == '0') bNumber = true;
				if (strNumber.charAt(i) == '1') bNumber = true;
				if (strNumber.charAt(i) == '2') bNumber = true;
				if (strNumber.charAt(i) == '3') bNumber = true;
				if (strNumber.charAt(i) == '4') bNumber = true;
				if (strNumber.charAt(i) == '5') bNumber = true;
				if (strNumber.charAt(i) == '6') bNumber = true;
				if (strNumber.charAt(i) == '7') bNumber = true;
				if (strNumber.charAt(i) == '8') bNumber = true;
				if (strNumber.charAt(i) == '9') bNumber = true;
				if (strNumber.charAt(i) == '.') bNumber = true;
				if (bNumber == false) return false;
			}
			// now check ti make sure there is only 1 .
			if (strNumber.length > 0) {
				if (strNumber.indexOf('.') != -1) {
					var temp = strNumber.substring(strNumber.indexOf('.') + 1, strNumber.length);
					if (temp.indexOf('.') != -1) {
						return false;
					}
				}
			}
			return true;
		}
		var requiredFields = new Array();
		var fieldNames = new Array();
		var rdFields = new Array();
		var ovFields = new Array();
		var flag;

		function chkForm() {
			var retVal;
			//  document.lpForm.btSubmit.disabled = true;
			rdFields[0] = "clcrdCh13";
			rdFields[1] = "clcrdCh23";
			rdFields[2] = "clcrdCh33";
			ovFields[0] = "clcChild1Amount";
			ovFields[1] = "clcChild2Amount";
			ovFields[2] = "clcChild3Amount";
			requiredFields[0] = "clcAnnualFamilyIncome"
			requiredFields[1] = "clcAnnualIncome";
			requiredFields[2] = "clcYears";
			requiredFields[3] = "clcFinalExpenses";
			requiredFields[4] = "clcMajorDebt";
			requiredFields[5] = "clcSavings";
			requiredFields[6] = "clcRetirementSavings";
			requiredFields[7] = "clcCurrentLifeInsurance";
			requiredFields[8] = "clcSocialSecurity";
			requiredFields[9] = "clcSpouseIncome";
			requiredFields[10] = "xxx1";
			requiredFields[11] = "xxx2";
			requiredFields[12] = "xxx3";
			fieldNames[0] = "Question 1";
			fieldNames[1] = "Question 2";
			fieldNames[2] = "Question 3";
			fieldNames[3] = "Question 4";
			fieldNames[4] = "Question 5";
			fieldNames[5] = "Question 7";
			fieldNames[6] = "Question 8";
			fieldNames[7] = "Question 9";
			fieldNames[8] = "Question 10";
			fieldNames[9] = "Question 11";
			fieldNames[10] = "Child age 1";
			fieldNames[11] = "Child age 2";
			fieldNames[12] = "Child age 3";
			flag = checkRequiredFields();
			if (flag) {
				//document.lpForm.submit();
				geResults($("#lpForm").serializeArray());
				$("#tabs-6").show().siblings().hide();
				offsetTab(1);
				$tabs.tabs('select', 0);

			}
			else {
				// document.lpForm.btSubmit.disabled = false;
				return false;
			}
		}

		function geResults(data) {
			console.log(data);

			var doubleAnnualFamilyIncome = Number(data[_.findIndex(data, { 'name': 'clcAnnualFamilyIncome' })].value) || 0,
				doubleAnnualIncome = Number(data[_.findIndex(data, { 'name': 'clcAnnualIncome' })].value) || 0,
				intYears = Number(data[_.findIndex(data, { 'name': 'clcYears' })].value) || 0,
				doubleFinalExpenses = Number(data[_.findIndex(data, { 'name': 'clcFinalExpenses' })].value) || 0,
				doubleMajorDebt = Number(data[_.findIndex(data, { 'name': 'clcMajorDebt' })].value) || 0,
				doubleSavings = Number(data[_.findIndex(data, { 'name': 'clcSavings' })].value) || 0,
				doubleRetirementSavings = Number(data[_.findIndex(data, { 'name': 'clcRetirementSavings' })].value) || 0,
				doubleCurrentLifeInsurance = Number(data[_.findIndex(data, { 'name': 'clcCurrentLifeInsurance' })].value) || 0,
				doubleSocialSecurity = Number(data[_.findIndex(data, { 'name': 'clcSocialSecurity' })].value) || 0,
				doubleSpouseIncome = Number(data[_.findIndex(data, { 'name': 'clcSpouseIncome' })].value) || 0,
				dCollegeExp = 0,
				dTotalCollegeExp = 0,
				strChild = "",
				iNumChildren = 0,
				dWithDrawalPercent = 0;
			iSSIAddYrs = [0, 0, 0],
				dTotalAssets = 0,
				dSocSecBaseBen = 0,
				dNewYearValue = 0,
				dIncomeNeeds = 0,
				dSocSecAddition = 0,
				dTotalAssets = 0,
				dExpenses = 0,
				dInsNeeds = 0,
				dAnnualWithdrawal = 0,
				iAnnualIncomeNeed = 0,
				iAnnualWithdrawal = 0,
				iIncomeNeeds = 0,
				iInsNeeds = 0,
				iChildAge = 0,
				iCurrChild = 0,
				clcrdCh = 0,
				iExpenses = 0;

			for (j = 1; j <= 3; j++) {
				iChildAge = Number(data[_.findIndex(data, { 'name': 'xxx' + j })].value) || null;

				if (_.findIndex(data, { 'name': 'clcrdCh' + j }) != - 1) {
					clcrdCh = Number(data[_.findIndex(data, { 'name': 'clcrdCh' + j })].value);
				}

				if (iChildAge != "" && iChildAge != null && clcrdCh != 0) {
					if (clcrdCh == "1") {
						dCollegeExp = 62000.0;
					} else if (clcrdCh == "2") {
						dCollegeExp = 142000.0;
					} else {
						dCollegeExp = Number(data[_.findIndex(data, { 'name': 'clcChild' + j + 'Amount' })].value)
					}

					for (var i = 1; i < 18 - iChildAge; i++) {
						dCollegeExp += dCollegeExp * 0.04;
					}

					dTotalCollegeExp += dCollegeExp;

					if (iChildAge < 18) {
						iSSIAddYrs[iNumChildren] = (18 - iChildAge - 1);
						iNumChildren++;
					}
				}//if

			}//for
			var bFullPass = false;
			while (!bFullPass) {
				bFullPass = true;
				for (var i = 0; i < iNumChildren - 1; i++) {
					if (iSSIAddYrs[i] > iSSIAddYrs[(i + 1)]) {
						var iTemp = iSSIAddYrs[(i + 1)];
						iSSIAddYrs[(i + 1)] = iSSIAddYrs[i];
						iSSIAddYrs[i] = iTemp;
						bFullPass = false;
					}
				}
			}
			//line 100
			dTotalAssets = doubleSavings + doubleRetirementSavings + doubleCurrentLifeInsurance;
			//line 102
			dWithDrawalPercent = WithDrawalPercent(intYears);
			//line 166
			dAnnualWithdrawal = dTotalAssets * dWithDrawalPercent;
			dSocSecBaseBen = doubleSocialSecurity * 12;
			dNewYearValue = doubleAnnualIncome - (dSocSecBaseBen + doubleSpouseIncome + dAnnualWithdrawal);

			dSocSecAddition = dSocSecBaseBen / (iNumChildren + 1); //mising
			//line 175
			for (var i = 0; i < intYears; i++) {
				if (i > 0) {
					dNewYearValue += dNewYearValue * 0.04;
				}
				if ((iCurrChild < iNumChildren) &&
					(i == iSSIAddYrs[iCurrChild])) {
					iCurrChild++;
					dNewYearValue += dSocSecAddition;
				}
				dIncomeNeeds += dNewYearValue;
			}
			dExpenses = dTotalCollegeExp + doubleMajorDebt + doubleFinalExpenses;
			dInsNeeds = dIncomeNeeds + dExpenses;

			iAnnualWithdrawal = doubleAnnualIncome - doubleSpouseIncome;
			iAnnualWithdrawal = dAnnualWithdrawal + dSocSecBaseBen;
			iIncomeNeeds = dIncomeNeeds;
			iExpenses = dExpenses;
			iInsNeeds = dInsNeeds;
			console.log(iInsNeeds);

			$("#results-text").text('$' + (iInsNeeds).toFixed(0).replace(/\d(?=(\d{3})+\.)/g, '$&,'));

			//NumberFormat nf = NumberFormat.getInstance();

		}//getResults


		function WithDrawalPercent(intYears) {
			var dWithDrawalPercent = 0;
			switch (parseInt(intYears)) {
				case 1:
					dWithDrawalPercent = 1.0;
					break;
				case 2:
					dWithDrawalPercent = 0.5;
					break;
				case 3:
					dWithDrawalPercent = 0.37;
					break;
				case 4:
					dWithDrawalPercent = 0.28;
					break;
				case 5:
					dWithDrawalPercent = 0.25;
					break;
				case 6:
					dWithDrawalPercent = 0.22;
					break;
				case 7:
					dWithDrawalPercent = 0.2;
					break;
				case 8:
					dWithDrawalPercent = 0.15;
					break;
				case 9:
					dWithDrawalPercent = 0.14;
					break;
				case 10:
					dWithDrawalPercent = 0.13;
					break;
				case 11:
					dWithDrawalPercent = 0.12;
					break;
				case 12:
					dWithDrawalPercent = 0.115;
					break;
				case 13:
					dWithDrawalPercent = 0.11;
					break;
				case 14:
					dWithDrawalPercent = 0.105;
					break;
				case 15:
					dWithDrawalPercent = 0.1;
					break;
				case 16:
					dWithDrawalPercent = 0.0966;
					break;
				case 17:
					dWithDrawalPercent = 0.0933;
					break;
				case 18:
					dWithDrawalPercent = 0.09;
					break;
				case 19:
					dWithDrawalPercent = 0.088;
					break;
				case 20:
					dWithDrawalPercent = 0.086;
			}
			return dWithDrawalPercent;
		}//dWithDrawalPercent



		function checkRequiredFields() {
			var fieldCheck = true;
			var fieldsNeeded = "\A valid number must be entered in the following question(s):\n\n\t";
			var temp;
			for (var fieldNum = 0; fieldNum < requiredFields.length; fieldNum++) {
				temp = document.lpForm.elements[requiredFields[fieldNum]].value;
				/*    while (temp.indexOf('$') != -1) {
				temp = temp.substring(0,temp.indexOf('$')) + temp.substring(temp.indexOf('$')+1,temp.length);
			}*/
				while (temp.indexOf(',') != -1) {
					temp = temp.substring(0, temp.indexOf(',')) + temp.substring(temp.indexOf(',') + 1, temp.length);
				}
				if (Trim(temp) == "") {
					temp = 0;
				}
				if (!isNumeric(temp)) {
					fieldsNeeded += fieldNames[fieldNum] + "\n\t";
					fieldCheck = false;
				}
				if (fieldCheck) {
					document.lpForm.elements[requiredFields[fieldNum]].value = temp;
				}
			}
			// check ather amounts
			for (var fieldNum = 0; fieldNum < rdFields.length; fieldNum++) {
				var chkrdFields = document.getElementById(rdFields[fieldNum]);
				if (chkrdFields.checked) {
					temp = document.lpForm.elements[ovFields[fieldNum]].value;
					/*
	while (temp.indexOf('$') != -1) {
					temp = temp.substring(0,temp.indexOf('$')) + temp.substring(temp.indexOf('$')+1,temp.length);
				}*/
					while (temp.indexOf(',') != -1) {
						temp = temp.substring(0, temp.indexOf(',')) + temp.substring(temp.indexOf(',') + 1, temp.length);
					}
					if (Trim(temp) == "") {
						temp = 0;
					}
					if (!isNumeric(temp)) {
						fieldsNeeded += "Other value for child " + (fieldNum + 1) + "\n\t";
						fieldCheck = false;
					}
					if (fieldCheck) {
						document.lpForm.elements[ovFields[fieldNum]].value = temp;
					}
				}
			}
			if (!fieldCheck) {
				alert(fieldsNeeded);
				return false;
			}
			return true;
		}
		function toggleChildFund(child, tog) {
			if (child == 1) {
				if (tog == 1) {
					document.lpForm.clcChild1Amount.disabled = false;
				}
				else {
					document.lpForm.clcChild1Amount.disabled = true;
				}
			}
			else if (child == 2) {
				if (tog == 1) {
					document.lpForm.clcChild2Amount.disabled = false;
				}
				else {
					document.lpForm.clcChild2Amount.disabled = true;
				}
			}
			else if (child == 3) {
				if (tog == 1) {
					document.lpForm.clcChild3Amount.disabled = false;
				}
				else {
					document.lpForm.clcChild3Amount.disabled = true;
				}
			}
		}
		function prefillAI() {
			var temp = document.lpForm.clcAnnualFamilyIncome.value;
			/*
	while (temp.indexOf('$') != -1) {
			temp = temp.substring(0,temp.indexOf('$')) + temp.substring(temp.indexOf('$')+1,temp.length);
		}
			 */
			while (temp.indexOf(',') != -1) {
				temp = temp.substring(0, temp.indexOf(',')) + temp.substring(temp.indexOf(',') + 1, temp.length);
			}
			if (Trim(temp) == "") {
				temp = 0;
			}
			if (isNumeric(temp)) {
				document.lpForm.clcAnnualIncome.value = Math.round(temp * .7);
			}
		}






	</script>
</head>

<body style="" class="wide">
	<div class="container">

		<div id="calculator_intro" class="article_box">
			<div class="column half hidden-xs hidden-sm">
				<h2 class="img-calc-wide">Personal Coverage Review</h2>
				<img border="0" src="/assets/neamb/images/gtl_calc_wide_img1.jpg" alt="image" class="img-calc-wide">
			</div>
			<div class="column half last content">


				<h2>Life Insurance 101</h2>
				<p>A step-by-step guide to determining how much coverage you need.</p>

				<p>At NEA Member Benefits, we can help you find the right amount of coverage to protect your family, pay final expenses,
					send your kids to college, leave a little something for the grandkids or do anything else that matters to you.</p>


				<a class="lets-begin next right btn" style="margin: 10px 0; background: #337ab7; color:white;" href="#">Let&#8217;s begin
					»</a>
			</div>
			<div style="clear:both;"></div>
		</div>
		<div id="tabs_insurance_calculator" style="display:none;" class="article_box">
			<div style="" class="article_top">
				<ul class="">
					<li>
						<a href="#tabs-1">Step 1</a>
					</li>
					<li>
						<a href="#tabs-2">Step 2</a>
					</li>
					<li>
						<a href="#tabs-3">Step 3</a>
					</li>
					<li>
						<a href="#tabs-4">Step 4</a>
					</li>
					<li>
						<a href="#tabs-5">Step 5</a>
					</li>
					<li>
						<a href="#tabs-6">Results</a>
					</li>
				</ul>
			</div>
			<div style="" class="article_mid">
				<div style="" class="">
					<!-- aaaaaaaaaaaaaaaaaaaaaaaaaaa -->
					<form id="lpForm" name="lpForm" method="post" action="#tabs-6">
						<div id="tabs-1">
							<div class="column half hidden-xs hidden-sm">

								<h2 class="img-calc-wide">Personal Coverage Review</h2>

								<img class="img-calc-wide" alt="image" src="/assets/neamb/images/gtl_calc_wide_img3.jpg" border="0" />
							</div>
							<div class="column half last content">

								<div class="row">
									<div class="col-md-12">
										<h2>Let’s start by figuring out how much your family will need for day-to-day expenses.</h2>
										<br />
										<div class="form-group">
											<p class="pad1"><b>1.</b> Annual family income you have now. Include your wages and wages from spouse, interest
												and dividends. </p>
											$ <span class="erroritem">
												<input class="form-control" onblur="prefillAI();" name="clcAnnualFamilyIncome" type="text" /> </span>
										</div>

										<div class="form-group">
											<p><b>2.</b> Annual income your family would still need if you died today. Typically 70% of the number in question
												1.</p>
											$ <span class="erroritem">
												<input class="form-control" id="clcAnnualIncome" name="clcAnnualIncome" type="text" /> </span>
										</div>

										<div class="form-group">
											<p><b>3.</b> Number of years your family will need to have this level of income. </p>
											<span class="erroritem ">
												<select class="form-control" name="clcYears">
													<option value="1">1</option>
													<option value="2">2</option>
													<option value="3">3</option>
													<option value="4">4</option>
													<option value="5">5</option>
													<option value="6">6</option>
													<option value="7">7</option>
													<option value="8">8</option>
													<option value="9">9</option>
													<option selected="true" value="10">10</option>
													<option value="11">11</option>
													<option value="12">12</option>
													<option value="13">13</option>
													<option value="14">14</option>
													<option value="15">15</option>
													<option value="16">16</option>
													<option value="17">17</option>
													<option value="18">18</option>
													<option value="19">19</option>
													<option value="20">20</option>
												</select> </span>
										</div>
									</div>
									<!--col-->
								</div>
								<!--row-->
								<br/>
								<a class="lets-begin prev left btn" style="margin: 10px 0; background: #337ab7; color:white;" href="#">&#171; Prev
									Step</a>
							</div>
							<div style="clear:both;"></div>
						</div><!-- tab 1-->

						<div id="tabs-2">
							<div class="column half hidden-xs hidden-sm">
								<h2 class="img-calc-wide">Personal Coverage Review</h2>
								<img class="img-calc-wide" alt="image" src="/assets/neamb/images/gtl_calc_wide_img4.jpg" border="0" />
							</div>
							<div class="column half last content">
								<h2>You don’t want to burden your family with unpaid expenses. I’ll help you find out how much coverage you need
									to take care of the most important ones.</h2>
								<div class="row">
									<div class="col-md-12">
										<div class="form-group">
											<p><b>4.</b> Final expenses. Include burial expenses (typically $15,000) plus probate fees. </p>
											$ <span class="erroritem">
												<input name="clcFinalExpenses" type="text" class="form-control" />
											</span>
										</div>

										<div class="form-group">
											<p><b>5.</b> Home mortgage balance and other major debt. Include only if you want your mortgage and other debts
												paid off when you die. </p>
											$ <span class="erroritem">
												<input name="clcMajorDebt" type="text" class="form-control" />
											</span>
										</div>
									</div><!-- col -->
								</div><!-- row -->
								<br/>
							</div>
							<div style="clear:both;"></div>
						</div><!-- trabs2 -->

						<div id="tabs-3">
							<div class="column half hidden-xs hidden-sm">
								<h2 class="img-calc-wide">Personal Coverage Review</h2>
								<img class="img-calc-wide" alt="image" src="/assets/neamb/images/gtl_calc_wide_img5.jpg" border="0" />
							</div>
							<div class="column half last content">
								<div class="row">
									<div class="col-md-12">
										<br>
										<p><b>6.</b> College costs. Current 4-year college cost (tuition, fees, room and
											board) estimates are $62,000 for a public college (in-state) and $142,000 for a private college. Inflation of
											4% annually until the child is 18 will be taken into account. Student loans and other financial aid can considerably
											lower out-of-pocket college expenses, if your child plans to apply for them, enter a lower amount under “Other.”<br/><br/></p>
										<div class="form-group">
											<label>Child Age: </label>
											<input name="xxx1" size="2" type="text" class="form-control" /> 
											<input onclick="toggleChildFund(1, 2);" name="clcrdCh1" value="1" type="radio" /> $62,000 <br />
											<input onclick="toggleChildFund(1, 2);" name="clcrdCh1" value="2" type="radio" /> $142,000 <br />
											<input id="clcrdCh13" onclick="toggleChildFund(1, 1);" name="clcrdCh1" value="3" type="radio" /> Other: $
											<input disabled="true" name="clcChild1Amount" size="5" type="text" class="form-control" /> 
										</div>
										<div class="form-group m-t-md">
												<label>Child Age: </label>
											<input name="xxx2" size="2" type="text" class="form-control" /> 
											<input onclick="toggleChildFund(2, 2);" name="clcrdCh2" value="1" type="radio" /> $62,000 <br />
											<input onclick="toggleChildFund(2, 2);" name="clcrdCh2" value="2" type="radio" /> $142,000 <br />
											<input id="clcrdCh23" onclick="toggleChildFund(2, 1);" name="clcrdCh2" value="3" type="radio" /> Other: $
											<input disabled="true" name="clcChild2Amount" size="5" type="text" class="form-control" />
										</div>
										<div class="form-group m-t-md">
											<label>Child Age: </label>
											<input name="xxx3" size="2" type="text" class="form-control" />
											<input onclick="toggleChildFund(3, 2);" name="clcrdCh3" value="1" type="radio" /> $62,000 <br />
											<input onclick="toggleChildFund(3, 2);" name="clcrdCh3" value="2" type="radio" /> $142,000 <br />
											<input id="clcrdCh33" onclick="toggleChildFund(3, 1);" name="clcrdCh3" value="3" type="radio" /> Other: $
											<input disabled="true" name="clcChild3Amount" size="5" type="text" class="form-control" /> 
										</div>
									</div><!--col-->
								</div><!--row-->
								<br/>
							</div>
							<div style="clear:both;"></div>
						</div><!--tabs 3 -->


						<div id="tabs-4">
							<div class="column half hidden-xs hidden-sm">
								<h2 class="img-calc-wide">Personal Coverage Review</h2>
								<img class="img-calc-wide" alt="image" src="/assets/neamb/images/gtl_calc_wide_img6.jpg" border="0" />
							</div>
							<div class="column half last content">
								<h2>Assets are good. The more your family has, the less additional coverage they’ll need.</h2>
								<br />
								<div class="row">
									<div class="col-md-12">
										<div class="form-group">
											<p class="pad1"><b>7.</b> Savings and investments. Bank accounts, CDs, mutual funds, stocks, bonds, financial gifts and inheritance. </p>
											$ <span class="erroritem">
												<input class="form-control" name="clcSavings" type="text"/> 
											</span>
										</div>
										<div class="form-group">
											<p><b>8.</b> Retirement savings. IRAs, 401(k)s, 403(b)s, any cash value of pension and profit
												sharing plans.</p>
												$ <span class="erroritem">
													<input name="clcRetirementSavings" type="text" class="form-control" /> </span>
										</div>
									</div><!--col-->
								</div><!--row-->
								<br/>
							</div>
							<div style="clear:both;"></div>
						</div><!--tab 4 -->


						<div id="tabs-5">
							<div class="column half hidden-xs hidden-sm">
								<h2 class="img-calc-wide">Personal Coverage Review</h2>
								<img class="img-calc-wide" alt="image" src="/assets/neamb/images/gtl_calc_wide_img7.jpg" border="0" />
							</div>
							<div class="column half last content">
								<br />
								<div class="row">
									<div class="col-md-12">
										<br>
										<div class="form-group">
											<p><b>9.</b> Amount of life insurance you have now.</p>
											$ <span class="erroritem">
												<input class="form-control" name="clcCurrentLifeInsurance" type="text" /> </span>
										</div>
										<div class="form-group">
											<p><b>10.</b> Amount of monthly Social Security benefits your survivors may receive upon your
												death.</p>
												$ <span class="erroritem"><input name="clcSocialSecurity" type="text" class="form-control"/> </span>
										</div>
										<div class="form-group">
											<p><b>11.</b> Amount of annual additional income spouse will supply by returning to workforce.</p>
											$ <span class="erroritem"><input name="clcSpouseIncome" type="text"  class="form-control"/> </span></span>
										</div>
									</div><!--col--->
								</div><!--row-->
								
								

								<br />
								<center>
									<!--
								<input type="image" src="/assets/neamb/images/buttons/calculateBtn.gif" id="btSubmit" name="btSubmit" value="Calculate" onClick="chkForm();return false;" onmouseover="this.src='/assets/neamb/images/buttons/calculateRolloverBtn.gif'" onmouseout="this.src='/assets/neamb/images/buttons/calculateBtn.gif'"/>
								<input type="image" src="/assets/neamb/images/buttons/clearBtn.gif" id="btSubmit2" name="Submit2" value="Clear" onClick="document.lpForm.reset();return false;" onmouseover="this.src='/assets/neamb/images/buttons/clearRolloverBtn.gif'" onmouseout="this.src='/assets/neamb/images/buttons/clearBtn.gif'" />
								-->
									<input class="btn" type="button" id="btSubmit" name="btSubmit" value="Calculate" onClick="chkForm(); return false;" style="margin: 10px 0; background: #337ab7; color:white;"
									/>
									<input class="btn" type="button" id="btSubmit2" name="Submit2" value="Clear" onClick="document.lpForm.reset(); return false;"
									 style="margin: 10px 0; background: #337ab7; color:white;" />
								</center>
								<br/>
							</div>

							<div style="clear:both;"></div>
						</div>

						<div id="tabs-6">
							<div class="column half ">
								<div class="">


									<!-- Comment -->



									<!-- con_action -->

									<h2 class="img-calc-wide">Your Result</h2>
									<br/>
									<div class="col-md-12">
										<div class="col-md-12" style="background-color:#E3E4E4; padding: 40px 20px;">
												<p style="text-align: center; font-size: 20px;">You need this extra coverage to help protect your family:<br><br></p>
												<p style="text-align: center; font-size: 20px;" id="results-text">$ 0</p>
										</div>
									</div>

								</div>
							</div>
							<div class="column half last content">

								<h2>You have two great options:</h2>

								<h3>Get started with a lower rate.</h3>

								<p>NEA Group Term Life Insurance coverage, <strong>issued by The Prudential Insurance Company of America,</strong>
									may be the most affordable way for most people to purchase high coverage amounts right away. Begin with $25,000
									to $500,000 in coverage, then increase or decrease it at any time.</p>

								<h3>Or lock in your rate for up to 20 years.</h3>

								<p>NEA Level Premium Group Term Life Insurance is a smart way to plan ahead, with rates guaranteed not to go up throughout
									your term. You can choose terms of 10, 15 or 20 years (depending on your age), with coverage amounts up to $1,000,000.</p>
								<br/>
								<a id="start-over" class="prev left btn" style="margin: 10px 0; background: #337ab7; color:white;" href="#" onclick="document.lpForm.reset();return false;">« Start over</a>
							</div>
							<div style="clear:both;"></div>
							<br/>
							<span style="font-size:10px">NEA Group Term Life coverage and NEA Level Premium Group Term Life coverage are issued by The
								Prudential Insurance Company of America, Newark, NJ. The Booklet-Certificate contains all details, including any
								policy exclusions, limitations, and restrictions, which may apply. Contract Series: 83500. CA COA# 1179. NAIC# 68241.</span>
							<br/>
							<span style="font-size:10px;margin-top:5px;" class="left">1041794-00001-00</span>
						</div>

					</form>
					<!-- ----------------------------------------- -->
				</div>
			</div>
			<div style="clear: both;" class="article_bottom"></div>
		</div>
	</div><!-- container -->

</body>

</html>