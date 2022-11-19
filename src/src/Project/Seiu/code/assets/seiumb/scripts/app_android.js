$(document).foundation();

$(document).tooltip();

//Disable enter submit on duplicate registration form
$('#selectOneRegistration').on('keyup keypress', function (e) {
    var keyCode = e.keyCode || e.which;
    if (keyCode === 13) {
        e.preventDefault();
        checkPassword();
        return false;
    }
});

//Select all server-error
$(".server-error").each(function (index) {
    if ($(this).text().trim().length !== 0) {
        $(this).css("display", "block");
    }
});

//Tooltips section

$.widget("ui.tooltip", $.ui.tooltip, {
    options: {
        content: function () {
            return $(this).attr('data-tooltip');
        }
    }
});

$("form#registration-form :input").each(function () {
    var content_dialog = $('#' + this.id + 'Dialog');
    if (!content_dialog.length == 0) {
        var dialog = content_dialog.html();
        $('#' + this.id).attr('data-tooltip', dialog);
        $('#' + this.id).tooltip({
            items: "[data-tooltip]",
            tooltipClass: "custom-tooltip-styling",
            position: { my: "left+15 center", at: "right center" },
        });
        $('#' + this.id).tooltip().off("mouseover mouseout");
    }


});

function cancelDialog(element) {
    $("form#registration-form :input").each(function () {
        $('#' + this.id).removeAttr("data-tooltip");
    });
}



/*
	function cancelDialog(element){  
    $('#'+ element ).removeAttr( "data-tooltip" )
}*/

//End tooltips section



var errRegistration = $('#errRegistration');
if (!errRegistration.length == 0) {
    deleteCookie("SEIUMBUsername");
}

var loginError = $('#login-error');
if (!loginError.length == 0) {
    var loginModal = $('#loginModal');
    if (!loginModal.length == 0) {
        loginModal.foundation('open');
    }
}

var welcome_message = $('#welcome_message');
if (!welcome_message.length == 0) {
    var membership = welcome_message.find("span")[0].innerText;
    if (welcome_message[0].innerText.length > 55) {
        var length_of_memebership = membership.length;
        var difference = welcome_message[0].innerText.length - 55;
        if (length_of_memebership > difference) {
            var temp_message = membership.substring(0, length_of_memebership - difference);

            welcome_message.find("span")[0].innerText = temp_message + "... | ";
        } else {
            welcome_message.find("span")[0].innerText = "... | ";
        }

    }
}

var button = $('#searchBtn');
var elem = $('#menuSearch');
var fieldSelector = $('#searchtext');

if (!elem.length == 0) {
    var toggler = new Foundation.Toggler(elem);
    var hide = false;

    elem.on('on.zf.toggler', function (e) {
        hide = false;
        console.log('toogling search');
        fieldSelector.focus();
    });

    elem.on('off.zf.toggler', function (e) {
        hide = true;
        console.log('toogling search');
        fieldSelector.focus();
    });

    $(document).on('click', function (e) {
        if ($(e.target)[0].id != 'searchBtn'
            && $(e.target)[0].id != 'menuSearch'
            && hide
            && $(e.target.parentElement)[0].id != 'menuSearch'
            && $(e.target)[0].type != 'text') {
            toggler.toggle();
            hide = true;
        }

    });

    $(document).on('keydown', function (e) {
        if (e.keyCode === 27) {
            if (hide) {
                toggler.toggle();
                hide = true;
            }
        }
    });
}

function saveCookie() {
    checkbox = $('#ckb_remember');
    checkboxmobile = $('#ckb_mobile_remember');
    if (!checkbox.length == 0) {
        if (checkbox.is(':checked')) {
            var username = $('#login-username');
            if (!username.length == 0 && username.val() != "") {
                setCookie("SEIUMBUsername", username.val(), document.domain, 7);
            }
        } else {
            deleteCookie("SEIUMBUsername");
        }
    }

    if (!checkboxmobile.length == 0) {
        if (checkboxmobile.is(':checked')) {
            var usernamemobile = $('#mobile-login-username');
            if (!usernamemobile.length == 0 && usernamemobile.val() != "") {
                setCookie("SEIUMBUsername", usernamemobile.val(), document.domain, 7);
            }
        } else {
            deleteCookie("SEIUMBUsername");
        }
    }
}

function checkCookie() {
    var username = getCookie("SEIUMBUsername");
    var loginUsername = $('#login-username');
    var loginPassword = $('#login-password');
    var mobileLogin = $('#mobile-login-username');

    if (username != "") {
        if (!loginUsername.length == 0) {
            loginUsername.val(username);
        }
        if (!mobileLogin.length == 0) {
            mobileLogin.val(username);
        }
    }
}

checkCookie();


if ($(window).width() < 510) {
    var navbar = $('#navbar');
    var accordion = new Foundation.AccordionMenu(navbar.children());
}

$(window).resize(function () {
    if ($(window).width() < 510) {
        var navbar = $('#navbar');
        var accordion = new Foundation.AccordionMenu(navbar.children());
    } else {
        var menu = $('#menu');
        var navbar = $('#navbar');
        var accordion = new Foundation.AccordionMenu(navbar.children());
        accordion.destroy();
    }
});

var loginModal = $('#loginModal');
loginModal.on(
    'open.zf.reveal', function () {
        if (typeof url_img_campaign !== 'undefined') {
            $('#imgLoginHeader').attr('src', url_img_campaign);
        }
    }
);

function hideLoginError() {
    var loginError = $('#login-error');
    if (!loginError.length == 0) {
        loginError.hide();
    }
}

var displayModal = $('#display-login-modal');
if (!displayModal.length == 0) {
    var loginModal = $('#loginModal');
    var loginpassword = $('#login-password');
    if (!loginModal.length == 0) {
        loginModal.foundation('open');
        loginpassword.val('');
    }
}

var userAlreadyRegisteredError = $('#user-already-registered');
if (!userAlreadyRegisteredError.length == 0) {
    var loginModal = $('#loginModal');
    if (!loginModal.length == 0) {
        loginModal.foundation('open');
    }
    var errorToDisplay = $('#already-registered-error');
    errorToDisplay.css("display", "block");
}

function onCloseLogin() {
    var loginError = $('#login-error');
    var loginForm = $('#login-form');
    var loginUsername = $('#login-username');
    var email = loginUsername.val();
    if (!loginError.length == 0) {
        loginError.css("display", "none");
    }
    if (!loginForm.length == 0) {
        var abideValidator = new Foundation.Abide(loginForm);
        abideValidator.resetForm();
        loginUsername.val(email);
    }
    var errorToDisplay = $('#already-registered-error');
    errorToDisplay.css("display", "none");
}

var formReg = $('#formBig');
var form = new Foundation.Abide(formReg);

function showPasswordMessage() {
    var passwordMessage = $('#messagePassword');
    passwordMessage.show();
}

function showMissMatchFieldMessage(field1, field2) {
    var missMatch = $('#missMatch-' + field2);
    var element1 = $('#' + field1);
    var element2 = $('#' + field2);
    if (element1.val().length > 0 && element2.val().length > 0 && element1.val() != element2.val()) {
        missMatch.css('display', 'block');
    } else {
        missMatch.css('display', 'none');
    }

}

function submitForm(el, form) {
    var element = $('#' + el.id);
    var valid = false;

    if (form != undefined) {
        var valid_form = $('#' + form);
        valid_form.on("formvalid.zf.abide", function (ev, frm) {
            valid = true;
        });
        valid_form.foundation('validateForm');
        if (valid) {
            element.attr({
                'disabled': true
            });
            valid_form.submit();

        }
    } else {
        element.click();
    }
}

function hidePasswordMessage() {
    var passwordMessage = $('#messagePassword');
    passwordMessage.hide();
}

function dateValidator(el, required, parent) {
    var emptyStringRegEx = /^$/;
    var validRegExp = /^[A-Za-z ]+$/;
    var result = true;

    var element = $('#' + el[0].id);
    var validMessage = $('#valid-' + el.attr('id'));
    var requiredMessage = $('#required-' + el.attr('id'));
    if (el[0].validity.valueMissing) {
        validMessage.css('display', 'block');
        requiredMessage.css('display', 'none');
        result = false;
    }

    var dts = el[0].value.split('/')
        , dateTest = new Date(dts[2], dts[0] - 1, dts[1]);

    var dateToday = new Date();
    var yrRangeMax = dateToday.getFullYear() - 90;
    var yrRangeMinimun = dateToday.getFullYear() - 16;

    if (yrRangeMax - dateTest.getFullYear() > 0 || yrRangeMinimun - dateTest.getFullYear() < 0) {
        validMessage.css('display', 'block');
        requiredMessage.css('display', 'none');
        result = false;
    }

    if (!(!isNaN(dateTest) &&
        dateTest.getFullYear() === parseInt(dts[2], 10) &&
        dateTest.getMonth() === (parseInt(dts[0], 10) - 1) &&
        dateTest.getDate() === parseInt(dts[1], 10))) {
        validMessage.css('display', 'block');
        requiredMessage.css('display', 'none');
        result = false;
    }

    if (result) {
        requiredMessage.css('display', 'none');
        validMessage.css('display', 'none');
    }

    return result;
};

function hideServerError(field) {
    var serverErrorMessage = $('#server-error-' + field.id);
    if (!serverErrorMessage.length == 0) {
        serverErrorMessage.css('display', 'none');
    }
}

function hideAllServerErrors() {
    $(".server-error").each(function (index) {
        $(this).css("display", "none");
    });
}

function validateEqualTo(field, equal_field, form) {
    var password_confirm = $('#' + equal_field.id);
    var password_current = $('#' + field.id);

    var form = $('#' + form.id);
    var serverErrorMessage = $('#server-error-' + field.id);
    if (!serverErrorMessage.length == 0) {
        serverErrorMessage.css('display', 'none');
    }
    if (!password_confirm.length == 0 && password_confirm.val() != "") {
        form.foundation("validateInput", password_confirm);
    }

    return true;
}

function myEmailValidator(el, required, parent) {
    var emptyStringRegEx = /^$/;
    var validRegExp = /^[A-Za-z ]+$/;
    var result = true;

    var re = /[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,3}$/;

    var element = $('#' + el[0].id);
    var validMessage = $('#valid-' + el.attr('id'));
    var requiredMessage = $('#required-' + el.attr('id'));
    var serverErrorMessage = $('#server-error-' + el.attr('id'));
    if (!serverErrorMessage.length == 0) {
        serverErrorMessage.css('display', 'none');
    }
    if (el[0].validity.valueMissing) {
        validMessage.css('display', 'block');
        requiredMessage.css('display', 'none');
        result = false;
    }
    else if (!re.test(el[0].value)) {
        requiredMessage.css('display', 'block');
        validMessage.css('display', 'none');
        result = false;
    }

    if (result) {
        requiredMessage.css('display', 'none');
        validMessage.css('display', 'none');
    } else {
        if (!serverErrorMessage.length == 0) {
            serverErrorMessage.css('display', 'none');
        }
    }

    return result;
};

function myValidator(el, required, parent) {
    var emptyStringRegEx = /^$/;
    var validRegExp = /^[A-Za-z ]+$/;
    var result = true;

    var re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

    var element = $('#' + el[0].id);
    var validMessage = $('#valid-' + el.attr('id'));
    var requiredMessage = $('#required-' + el.attr('id'));
    var serverErrorMessage = $('#server-error-' + el.attr('id'));
    if (!serverErrorMessage.length == 0) {
        serverErrorMessage.css('display', 'none');
    }
    if (el[0].validity.valueMissing) {
        validMessage.css('display', 'block');
        requiredMessage.css('display', 'none');
        result = false;
    }
    else if (!re.test(el[0].value)) {
        requiredMessage.css('display', 'block');
        validMessage.css('display', 'none');
        result = false;
    }

    if (result) {
        requiredMessage.css('display', 'none');
        validMessage.css('display', 'none');
    } else {
        if (!serverErrorMessage.length == 0) {
            serverErrorMessage.css('display', 'none');
        }
    }

    return result;
};

function showValidMessage(el, valid) {
    var element = $('#' + el[0].id);
    var validMessage = $('#valid-' + el.attr('id'));
    var invalidMessage = $('#invalid-' + el.attr('id'));
    if (el.attr('class') == 'is-invalid-input') {
        valid = false;
    }
    if (valid) {
        validMessage.css('color', 'green');
        validMessage.css('display', 'block');
        invalidMessage.css('display', 'none');
    } else {
        invalidMessage.css('color', 'red');
        invalidMessage.css('display', 'block');
        validMessage.css('display', 'none');
    }
}

function dataEqual(el, field, form) {
    var field = $('#' + field);
    var element = $('#' + el.id);
    if (element.val().length > 0 && field.val().length > 0) {
        $("#" + form).foundation("validateInput", field);
        passwordValidator(field, false, false);
    }
}

function onKeyPressPasswordValidator(el, form) {
    var element = $('#' + el.id);
    $("#" + form).foundation("validateInput", element);
    passwordValidator(element, false, false);
}

function passwordValidator(el, required, parent) {
    var emptyStringRegEx = /^$/;
    var validRegExp = /^([^a-zA-Z]*([a-zA-Z]+)[^a-zA-Z]*)+$/;
    var numbers = /\d+/g;
    var result = true;
    var valid = true;
    if (!validRegExp.test(el[0].value)) {
        valid = false;
    }
    if (!numbers.test(el[0].value)) {
        valid = false;
    }
    if (el[0].value.indexOf(' ') >= 0) {
        valid = false;
    }
    if (el[0].value.length < 8) {
        valid = false;
    }

    if (el[0].validity.valueMissing) {
        result = false;
        showValidMessage(el, false);
    }
    else if (!valid) {
        result = false;
        showValidMessage(el, false);
    }
    else {
        showValidMessage(el, true);
    }

    return result;
};


form.options.validators.myValidator = myValidator;
form.options.validators.passwordValidator = passwordValidator;
form.options.validators.dateValidator = dateValidator;
form.options.validators.myEmailValidator = myEmailValidator;

var carousel = $('#orbit-carousel');
if (!carousel.length === 0) {
    var found_carousel = new Foundation.Orbit(carousel);
}

var actualUrl = $('#actual-url');


if (!actualUrl.length == 0) {
    actualUrl.val(document.referrer);
}

var button = $('#searchBtn');
var elem = $('#menuSearch');

function openSearch() {
    var toggler = new Foundation.Toggler(elem);
    toggler.toggle();
}

var cmsMenuToogler = $('#cmsMenuToogler');
var smartEditPanel = $('#smart-edit-panel');


if (!cmsMenuToogler.length === 0 && !smartEditPanel.length === 0) {
    var toggler = new Foundation.Toggler(smartEditPanel);
    toggler.options.animate = true;
    toggler.animationOut = "fade-out";
    toggler.toggle();
}


function openCMSMenu() {
    var toggler = new Foundation.Toggler(smartEditPanel);

    toggler.options.animate = true;
    toggler.animationIn = "hinge-in-from-top";
    toggler.animationOut = "hinge-out-from-top";
    toggler.toggle();
}
$('#cmsMenuToogler').click(function () {
    $(this).toggleClass("open");
    if ($(this).hasClass("open")) {
        $(this).text('CMS Tool | Close');
    } else {
        $(this).text('CMS Tool | Open');
    }
});
if (!button.length === 0 && !elem.length === 0) {
    var toggler = new Foundation.Toggler(elem);

    var hide = false;

    elem.on('on.zf.toggler', function (e) {
        hide = false;
    });

    elem.on('off.zf.toggler', function (e) {
        hide = true;
    });

    $(document).on('click', function (e) {
        if ($(e.target)[0].id != 'searchBtn'
            && $(e.target)[0].id != 'menuSearch'
            && hide
            && $(e.target.parentElement)[0].id != 'menuSearch'
            && $(e.target)[0].type != 'text') {
            toggler.toggle();
            hide = true;
        }

    });

    $(document).on('keydown', function (e) {
        if (e.keyCode === 27) {
            if (hide) {
                toggler.toggle();
                hide = true;
            }
        }
    });
}

var text_max = 1000;
$('#textarea_feedback').html(text_max + $('#textarea_feedback').text());

$('#message').keyup(function () {
    var text_length = $('#message').val().length;
    var text_remaining = text_max - text_length;

    $('#textarea_feedback').html(text_remaining);
});

//Validations


function validateZipCode(object, event) {
    var charCode = (event.which) ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57) && (charCode < 96 || charCode > 105)) {
        event.preventDefault();
        event.stopPropagation();
        return false;
    }
    if (object.value.length == 5 && charCode != 8) {
        event.preventDefault();
        event.stopPropagation();
        return false;
    }
    return true;
}

function validateDate(object, event) {
    if (event.keyCode > 31 && (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
        event.preventDefault();
    }
    if (object.value.length == 2 && event.keyCode != 8) object.value = object.value + "/";
    if (object.value.length == 5 && event.keyCode != 8) object.value = object.value + "/";
    if (object.value.length == 10 && event.keyCode != 8) event.preventDefault();
}

function validateTelephone(object, event) {
    if (event.keyCode > 31 && (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
        event.preventDefault();
    }
    if (object.value.length == 1 && event.keyCode != 8) object.value = "(" + object.value;
    if (object.value.length == 4 && event.keyCode != 8) object.value = object.value + ")";
    if (object.value.length == 8 && event.keyCode != 8) object.value = object.value + "-";
    if (object.value.length == 13 && event.keyCode != 8) event.preventDefault();
}

var datePreviousValue = "";
function validateDateInput(field) {

    var element = $('#' + field.id);
    var characters_alloweds = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
    var last_character = element.val().substring(element.val().length - 1, element.val().length);
    var before_last_character = element.val().substring(element.val().length - 2, element.val().length - 1);
    var last_character_old = datePreviousValue.substring(datePreviousValue.length - 1, datePreviousValue.length);

    if (characters_alloweds.indexOf(last_character) == -1) {
        element.val(element.val().substring(0, element.val().length - 1));
    }

    var text = element.val().replace(new RegExp('/', 'g'), '');
    text = text.replace(/\D/g, '');


    var diff = element.val().length - text.length;

    var pos = element.val().slice(0, field.selectionStart).length;

    if (diff == 0 || diff == 1) {
        if (pos == 3) {
            pos = pos + 1;
        }
        if (pos == 6) {
            pos = pos + 1;
        }

    }

    if (text.length > 8) {
        text = text.substring(0, text.length - 1);
    }

    if (text.length < 3 && last_character_old != "/") {

    } else if (text.length < 5 && last_character_old != "/") {
        text = text.substring(0, 2) + '/' + text.substring(2, text.length);

    } else if (text.length < 10 && last_character_old != "/") {
        text = text.substring(0, 2) + '/' + text.substring(2, 4) + '/' + text.substring(4, text.length);

    }

    element.val(text);
    datePreviousValue = field.value;

    field.setSelectionRange(pos, pos);


}

function validateDateInput2(field) {
    var element = $('#' + field.id);
    var characters_alloweds = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
    var last_character = element.val().substring(element.val().length - 1, element.val().length);
    var before_last_character = element.val().substring(element.val().length - 2, element.val().length - 1);
    var last_character_old = datePreviousValue.substring(datePreviousValue.length - 1, datePreviousValue.length);

    if (characters_alloweds.indexOf(last_character) == -1) {
        element.val(element.val().substring(0, element.val().length - 1));
    }
    if (element.val().length == 2 && last_character != "/" ||
        element.val().length == 3 && before_last_character != "/") {
        if (element.val().length == 2 && last_character != "/") {
            if (last_character_old != "/") {
                element.val(element.val() + "/");
            }
        } else {
            element.val(element.val().substring(0, 2) + "/" + element.val().substring(2, 3));
        }
    }
    if (element.val().length == 5 && last_character != "/" ||
        element.val().length == 6 && before_last_character != "/") {
        if (element.val().length == 5 && last_character != "/") {
            if (last_character_old != "/") {
                element.val(element.val() + "/");
            }
        } else {
            element.val(element.val().substring(0, 5) + "/" + element.val().substring(5, 6));
        }
    }
    if (element.val().length > 10) element.val(element.val().substring(0, element.val().length - 1));

    datePreviousValue = field.value;
}

var phoneNumberPreviousValue = "";
function validatePhoneInput2(field, old) {
    var element = $('#' + field.id);
    var characters_alloweds = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
    var last_character = element.val().substring(element.val().length - 1, element.val().length);
    var before_last_character = element.val().substring(element.val().length - 2, element.val().length - 1);
    var last_character_old = phoneNumberPreviousValue.substring(phoneNumberPreviousValue.length - 1, phoneNumberPreviousValue.length);


    if (characters_alloweds.indexOf(last_character) == -1) {
        element.val(element.val().substring(0, element.val().length - 1));
    }
    if (element.val().length == 1) {
        element.val("(" + element.val());
    }
    if (element.val().length == 4 && last_character != ")" ||
        element.val().length == 5 && before_last_character != ")") {
        if (element.val().length == 4 && last_character != ")") {
            if (last_character_old != ")") {
                element.val(element.val() + ")");
            }
        } else {
            element.val(element.val().substring(0, 4) + ")" + element.val().substring(4, 5));
        }

    }
    if (element.val().length == 8 && last_character != "-" ||
        element.val().length == 9 && before_last_character != "-") {
        if (element.val().length == 8 && last_character != "-") {
            if (last_character_old != "-") {
                element.val(element.val() + "-");
            }
        } else {
            element.val(element.val().substring(0, 8) + "-" + element.val().substring(8, 9));
        }
    }
    if (element.val().length > 13) {
        element.val(element.val().substring(0, element.val().length - 1));
    }
    phoneNumberPreviousValue = field.value;
}

function validatePhoneInput(field, old) {
    var element = $('#' + field.id);
    var characters_alloweds = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
    var last_character = element.val().substring(element.val().length - 1, element.val().length);
    var before_last_character = element.val().substring(element.val().length - 2, element.val().length - 1);
    var last_character_old = phoneNumberPreviousValue.substring(phoneNumberPreviousValue.length - 1, phoneNumberPreviousValue.length);

    if (characters_alloweds.indexOf(last_character) == -1) {
        element.val(element.val().substring(0, element.val().length - 1));
    }

    var text = element.val().replace('(', '');
    text = text.replace(')', '');
    text = text.replace('-', '');
    text = text.replace(/\D/g, '');

    var diff = element.val().length - text.length;

    var pos = element.val().slice(0, field.selectionStart).length;

    if (diff == 0 || diff == 1) {
        pos = pos + 1;
    } else if (diff == 2) {
        if (pos == 9) {
            pos = pos + 1;
        }
    } else if (diff == 3) {
        //pos = pos +2;
    }

    if (text.length > 10) {
        text = text.substring(0, text.length - 1);
    }

    if (element.val().length < 1 && last_character_old == "(") {
        text = "";
    } else if (text.length < 4 && last_character_old != ")") {
        text = '(' + text;
    } else if (text.length < 7 && last_character_old != "-") {
        text = '(' + text.substring(0, 3) + ')' + text.substring(3, text.length);
    } else if (text.length < 11) {
        text = '(' + text.substring(0, 3) + ')' + text.substring(3, 6) + '-' + text.substring(6, text.length);
    } else {
    }

    element.val(text);
    phoneNumberPreviousValue = field.value;

    field.setSelectionRange(pos, pos);

}

function validate(field) {
    var element = $('#' + field.id);
    var form = element.closest("form");
    form.foundation("validateInput", element);
}


function validateZipCodeInput(field) {
    var element = $('#' + field.id);
    var reg = new RegExp('^[0-9]');
    if (!reg.test(element.val()) || element.val().length > 5) {
        element.val(element.val().substring(0, element.val().length - 1));
    }
}

$("#contact_us").submit(function (event) {
    var $phoneNumber = $('#phoneNumber');
    $phoneNumber.val($phoneNumber.val().replace('-', ''));
    $phoneNumber.val($phoneNumber.val().replace('(', ''));
    $phoneNumber.val($phoneNumber.val().replace(')', ''));
    return;
});

$("#registration-form-mobile").submit(function (event) {
    var $phoneNumber = $('#phoneNumberSmall');
    $phoneNumber.val($phoneNumber.val().replace('-', ''));
    $phoneNumber.val($phoneNumber.val().replace('(', ''));
    $phoneNumber.val($phoneNumber.val().replace(')', ''));
    return;
});

$("#registration-form").submit(function (event) {
	/*var $phoneNumber = $( '#phoneNumber' );
		$phoneNumber.val( $phoneNumber.val().replace('-', '') );
		$phoneNumber.val( $phoneNumber.val().replace('(', '') );
	$phoneNumber.val( $phoneNumber.val().replace(')', '') );*/
    return;
});

function formatDate(parent, source, target) {
    var dateVal = $("#" + parent + " " + "#" + source).val();
    if (dateVal != null && dateVal != undefined && dateVal != '') {
        var year = dateVal.substring(0, 4);
        var month = dateVal.substring(5, 7);
        var day = dateVal.substring(8, dateVal.length);
        var formated_dob = month + '/' + day + '/' + year;
        $("#" + parent + " " + "#" + target).val(formated_dob);
    }

}

//Jonathan's Code
function deleteCookie(name) {
    var cookie = getCookie(name);
    if (cookie != "") {
        setCookie(name, "", document.domain, 1);
    }
};
function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
    }
    return "";
}
function setCookie(cname, cvalue, domain, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    var domain = ";Domain=" + domain;
    document.cookie = cname + "=" + cvalue + "; " + expires + ";" + domain + ";Path=/";
}

function formatDate2(date/*date variable*/) {
    var dayAux = date.getDate();
    var day = dayAux.toString().length > 1 ? dayAux : '0' + dayAux;
    var monthAux = date.getMonth() + 1;
    var month = monthAux.toString().length > 1 ? monthAux : '0' + monthAux;
    var year = date.getFullYear();
    var formatedDate;
    if (day != "" && day != undefined && month != "" && month != undefined && year != "" && year != undefined) {
        formatedDate = year + "-" + month + "-" + day;
    }
    return formatedDate;
}

function formatDate3(datetoFormat) {
    var valueformated;
    if (datetoFormat != null && datetoFormat != "" && datetoFormat != undefined) {
        var val = datetoFormat;
        valueformated = val.replace(/(\d{2})(\d{2})(\d{4})/, '$1/$2/$3');
    }
    return valueformated != null ? valueformated : datetoFormat;

}


function formatPhoneNumber(phoneToFormat) {
    var valueformated;
    if (phoneToFormat != null && phoneToFormat != "" && phoneToFormat != undefined) {
        var val = phoneToFormat;
        valueformated = val.replace(/(\d{3})(\d{3})(\d{4})/, '($1)$2-$3');
    }
    return valueformated != null ? valueformated : phoneToFormat;

}

var myAccount_PhoneNumber = $("#my-account-form #phoneNumber").val();
$("#my-account-form #phoneNumber").val(formatPhoneNumber(myAccount_PhoneNumber));

var myAccount_DateOfBirth = $("#my-account-form #dateOfBirth").val();
$("#my-account-form #dateOfBirth").val(formatDate3(myAccount_DateOfBirth));


var registration_PhoneNumber = $("#registration-form #phoneNumber").val();
$("#registration-form #phoneNumber").val(formatPhoneNumber(registration_PhoneNumber));

var contactus_PhoneNumber = $("#contact-us-form #phoneNumber").val();
$("#contact-us-form #phoneNumber").val(formatPhoneNumber(contactus_PhoneNumber));

//Bolivar's Code
function cnsinvitation(form) {
    var x = document.getElementById(form);
    x.action = x.action.concat("https%3A%2F%2Fseiumb.affinityperks.com%2Finvitefriend%2Finvite")
}
function resetinvitation(form) {
    var x = document.getElementById(form);
    x.action = x.action.replace("https%3A%2F%2Fseiumb.affinityperks.com%2Finvitefriend%2Finvite", '');
}

//=============== GA TRACK EVENT UNIVERSAL =================
function ga_event(category, action, label) {
    var rex = /(<([^>]+)>)/ig;
    label = label.replace(rex, ""); // REMOVE HTML TAGS FROM ALL LABELS
    if (action == "impression")
        ga('send', 'event', category, action, label, { 'nonInteraction': 1 });
    else
        ga('send', 'event', category, action, label);
}
//========================================================


function doTheMaths() {
    var orbit_container = $(".orbit-container");
    var orbit_slide = $(".orbit-slide");
    var orbit_img_bg = $(".orbit-slide #bg");
    var orbit_img = $(".orbit-img");
    var carousel = $("#carousel");
    var home_shedron = $("#home-shedron");
    var proper_height = carousel.height();
    var screen_width = window.screen.width;

    //Android Portrait
    if(screen_width == 360 && window.matchMedia("(orientation: portrait)").matches){
        proper_height = 227;
    }
    //Android Landscape
    else if(screen_width == 640 && window.matchMedia("(orientation: landscape)").matches){
        proper_height = 254;
    }
    //IPhone Portrait
    else if(screen_width == 375 && window.matchMedia("(orientation: portrait)").matches){
        proper_height = 267;
    } 
    //IPhone Landscape   
    else if (screen_width == 650 && window.matchMedia("(orientation: landscape)").matches) {
        proper_height = 285;
    }    
    orbit_container.height(proper_height);
    orbit_slide.height(proper_height);
    orbit_img_bg.height(proper_height);
    orbit_img.height(proper_height);
}


function setCarsHeight(){
    var card_rows = $( "#cards .row");
    var cards = $( "#cards .panel");
    var max_height = 410;
    if(!cards.length == 0){
        $.each( cards, function( i, val ) {
            if($(val).height() > max_height){
               // max_height = parseInt($(val).height());
            }
        }); 

        $.each( cards, function( i, val ) {            
            $(val).height(max_height);
        }); 
    }
}

function changeTabsResponsive(){
    var element = $('#account-tabs');  
    if(!element.length == 0){
        
        if(Foundation.MediaQuery.current=='small'){
            console.log('is mobile');
            element.removeClass( "menu expanded" ).addClass( "vertical" );
        }else{
            element.removeClass( "vertical" ).addClass( "menu expanded" );
        }
    }  
}

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
        } else {
            result = true;
        }
    }
    if (result) {
        window.location = url.substring(0, url.length - 1);
    }
}


$(function () {
    doTheMaths();
    //setCarsHeight();
    changeTabsResponsive()

    $(window).resize(function () {
        doTheMaths();
        //setCarsHeight();
        changeTabsResponsive()
    });


    /* need to add all of this beacuse the Foundation Deep Link doesn´t work */
    var element = $('#account-tabs');

    if(!element.length == 0){
        var options = {deep_linking: true, deep_link_smudge: true, update_history: true};
        var elem = new Foundation.Tabs(element, options);

        element.on('change.zf.tabs', function(event, tab) {
            var indexOfId = tab[0].children[0].id.indexOf("-label");
            var panelId = tab[0].children[0].id.substring(0, indexOfId);
            window.location.hash = panelId;
            $('html,body').scrollTop(0);
        });

        var link_tab = window.location.hash.substr(1);
        if(link_tab){
            console.log(link_tab);
            var selectedTab = $('#'+link_tab);        
            $('[data-tabs]').eq(0).foundation('selectTab', selectedTab, true);
            $('.tabs-title.is-active').removeClass('is-active').find('a').attr('aria-selected',false);
            $("#account-tabs").find('a[href="#'+link_tab+'"]').attr('aria-selected',true).parent().addClass('is-active');
        }
    }

    
});
