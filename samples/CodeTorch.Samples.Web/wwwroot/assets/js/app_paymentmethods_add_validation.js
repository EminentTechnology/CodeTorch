
$(document).ready(function () {
    //'use strict';

    console.log('about to setup form validation');
   
    $('#ctl01').validate({
        ignore: '.ignore',
        focusInvalid: false,
        rules: {
            'ctl00$MainContent$Add_your_payment_method$AccountName$ctrl': {
                required: true
            },
            'ctl00$MainContent$Add_your_payment_method$BillingPhoneNo$ctrl': {
                required: true
            },
            'ctl00$MainContent$Add_your_payment_method$BillingAddressLine1$ctrl': {
                required: true
            },
            'ctl00$MainContent$Add_your_payment_method$BillingCity$ctrl': {
                required: true
            },
            'ctl00$MainContent$Add_your_payment_method$BillingStateId$ctrl': {
                required: true
            }
        },

        // Errors
        //

        errorPlacement: function errorPlacement(error, element) {
            var $parent = $(element).parents('.form-group');

            // Do not duplicate errors
            if ($parent.find('.jquery-validation-error').length) { return; }

            $parent.append(
                error.addClass('jquery-validation-error small form-text invalid-feedback')
            );
        },
        highlight: function (element) {
            var $el = $(element);
            var $parent = $el.parents('.form-group');

            $el.addClass('is-invalid');

        },
        unhighlight: function (element) {
            $(element).parents('.form-group').find('.is-invalid').removeClass('is-invalid');
        }
    });

    console.log('form validation setup');
   

});



