var stripe = Stripe('pk_test_zvFeHXBWwKyy8Kq43itkGOwC');

var elements = stripe.elements();

var card = elements.create('card', {
    iconStyle: 'solid',
    style: {
        base: {
            iconColor: '#c4f0ff',
            color: '#000',

        },
        invalid: {
            color: '#fa755a',
            iconColor: '#fa755a'
        },
    },
});

$(function () {
    //'use strict';

    $("#MainContent_Details_AccountName_ctrl").attr("placeholder", "Enter your name as shown on your card - eg John Doe");

    $("#MainContent_Buttons_Save").remove();
    $(".payment-fieldset").append("<div id='card_FormGroup' class='form-group row'><label class='col-form-label col-sm-2 text-sm-right required required-label'>Credit Card:</label><div id='card-element' class='form-control col-sm-9' style='margin-left:.75rem'  ></div><div id='card-errors' role='alert' class='text-center' style='color:red;width:100%;padding-top:12px'></div></div>");
    $("<input type='button' name='ctl00$MainContent$Buttons$Save' value='Add Card' onclick='stripecheck();' id='MainContent_Buttons_Save' class='btn btn-primary mr-1'>").insertBefore("#MainContent_Buttons_Cancel");

    


    card.mount('#card-element');


   

});





async function stripecheck() {

    console.log('in stripe check');

    

    var isValid = Page_ClientValidate("");
    console.log('stripecheck.validation results', isValid, Page_Validators.length);



    if (isValid) {
        var data = {
            name: $("#MainContent_Add_your_payment_method_AccountName_ctrl").val(),
            address_line1: $("#MainContent_Add_your_payment_method_BillingAddressLine1_ctrl").val(),
            address_line2: $("#MainContent_Add_your_payment_method_BillingAddressLine2_ctrl").val(),
            address_city: $("#MainContent_Add_your_payment_method_BillingCity_ctrl").val(),
            address_state: $("#ctl00_MainContent_Add_your_payment_method_BillingStateId_ctrl_Input").val(),
            address_country: 'US',
            currency: 'usd'
        };

        var result = await stripe.createToken(card, data);

        console.log('in stripe check - response from stripe', result);


        var myJSON = JSON.stringify(result);
        console.log(myJSON);

        if (result.error) {
            // Inform the user if there was an error
            var errorElement = document.getElementById('card-errors');
            errorElement.textContent = result.error.message;
            console.log('in stripe check - got error - returning false');


        } else {
            // Send the token to your server
            stripeTokenHandler(result.token);
            console.log('in stripe check - completed tokenhandler - we are good to go - submit up the chain', result);

            __doPostBack("ctl00$MainContent$Buttons$Save", "");
        }
    }



}

function stripeTokenHandler(token) {
    // Insert the token ID into the form so it gets submitted to the server
    var form = document.getElementById('ctl01');

    addHiddenField(form, 'stripeToken', token.id);
    addHiddenField(form, 'stripeCardId', token.card.id);
    addHiddenField(form, 'address_zip', token.card.address_zip);
    addHiddenField(form, 'address_zip_check', token.card.address_zip_check);
    addHiddenField(form, 'brand', token.card.brand);
    addHiddenField(form, 'cvc_check', token.card.cvc_check);
    addHiddenField(form, 'exp_month', token.card.exp_month);
    addHiddenField(form, 'exp_year', token.card.exp_year);
    addHiddenField(form, 'last4', token.card.last4);
    addHiddenField(form, 'client_ip', token.client_ip);
}

function addHiddenField(form, fieldName, fieldValue) {
    var hiddenInput = document.createElement('input');
    hiddenInput.setAttribute('type', 'hidden');
    hiddenInput.setAttribute('name', fieldName);
    hiddenInput.setAttribute('value', fieldValue);

    form.appendChild(hiddenInput);
}

