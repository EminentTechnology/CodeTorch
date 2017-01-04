$(document).ready(function () {


    init()
    AddressPlaceholder();
 


    $(".alternativeid").focus();
    startValidation();
    


});

function init() {

   
    $(".alternativeid").attr("AUTOCOMPLETE", "off");
   
}

//function AddressPlaceholder() {

//    if (navigator.appVersion.indexOf("MSIE 8.") == -1) {

//        $('.bvnno').attr('placeholder', 'Your Bank Verification Number');
//        $('.mobileNumber').attr('placeholder', 'Your Mobile Number');
//        $('.pinAndToken').attr('placeholder', 'Your Pin and Token ');





//    var input = document.createElement("input");
//    if (('placeholder' in input) == false) {
//        $('[placeholder]').focus(function () {
//            var i = $(this);
//            if (i.val() == i.attr('placeholder')) {
//                i.val('').removeClass('placeholder');
//                if (i.hasClass('password')) {
//                    i.removeClass('password');
//                    this.type = 'password';
//                }
//            }
//        }).blur(function () {
//            var i = $(this);
//            if (i.val() == '' || i.val() == i.attr('placeholder')) {
//                if (this.type == 'password') {
//                    i.addClass('password');

//                   //this.type = 'text';
//                    i.attr('type','text')
//                    i.addClass('placeholder').val(i.attr('placeholder'));
//                }
//            }
//            else {
//                if (this.type == 'password') {
//                    i.addClass('password');
//                    this.type = 'password';
//                }
//            }
//        }).blur().parents('form').submit(function () {
//            $(this).find('[placeholder]').each(function () {
//                var i = $(this);
//                if (i.val() == i.attr('placeholder'))
//                    i.val('');
//            })
//        });
//    }

//  }
//}



function startValidation() {


    $('#form1').bootstrapValidator({
        message: 'This value is not valid',
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        
        //fields: {
        //    bvnno: {
        //        message: 'Your bank verification number is required',
        //        validators: {
        //            notEmpty: {
        //                message: 'Your bank verification number is required and can\'t be empty'
        //            },
                   
        //        }
        //     },

        //     mobileNumber: {
        //         message: 'Your mobile number is required',
        //         validators: {
        //             notEmpty: {
        //                 message: 'Your mobile number is required and can\'t be empty'
        //             },
        //             regexp: {
        //                 regexp: /^(\+\d{1,3}[- ]?)?\d{11,14}$/,
        //                 message: 'Enter a valid mobile number'
        //             },
                      
        //         }

        //     },

        //       pinAndToken: {
        //         message: 'Your pin and Token is required',
        //         validators: {
        //             notEmpty: {
        //                 message: 'Your pin and token is required and can\'t be empty'
        //             },
                    
        //         }
        //     },
                 
        //}
    });




    $('.alternativeid').keydown(function (event) {
        var keyCode = event.keyCode;
        var backspace = 8;
        var tab = 9;
        var deleteKey = 46;
        var ctrlKey = 17, vKey = 86, cKey = 67;


        if ((ctrlKey && (keyCode == vKey || keyCode == cKey)) || (keyCode >= 48 && keyCode <= 57) || (keyCode >= 96 && keyCode <= 105) || keyCode == backspace || keyCode == deleteKey || keyCode == tab) {

            return true;
        }
        else {

            event.preventDefault();
        }
    });

}
