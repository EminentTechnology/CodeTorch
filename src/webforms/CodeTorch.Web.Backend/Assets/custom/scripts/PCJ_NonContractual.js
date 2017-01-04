$(document).ready(function () {
    //function OverrideOnClientSelectedIndexChanged(sender, args) {
    //    var item = args.get_item();
    //    var value = item.get_value();
    //    var id = sender.get_id();
    //    if (id == $(".GovtSponCon").attr("id")) {
    //        if (value != "1") {
    //            $(".GovtSponConDate").hide();
    //        }
    //    }
    //}

    initialize();

    $('.GovtSponCon').change(function () {
        if ($(this).val() != "Yes") {
            $(".GovtSponConDate").hide();
          //  $("#ctl00_MainContent_Clearance_Justification_GovtSponsorConcurrenceDate_ctrl_dateInput").val('');
        } else {
            $(".GovtSponConDate").show();
           // $("#ctl00_MainContent_Clearance_Justification_GovtSponsorConcurrenceDate_ctrl_dateInput").val('');
        }

    });

    $('.PrimeSponCon').change(function () {
        if ($(this).val() != "Yes") {
            $(".PrimeSponConDate").hide();
         //   $("#ctl00_MainContent_Clearance_Justification_PrimeSponsorConcurrenceDate_ctrl_dateInput").val('');
        } else {
            $(".PrimeSponConDate").show();
           // $("#ctl00_MainContent_Clearance_Justification_PrimeSponsorConcurrenceDate_ctrl_dateInput").val('');
        }

    });
   
    $('.PMSponCon').change(function () {
        if ($(this).val() != "Yes") {
            $(".PMSponConDate").hide();
          //  $("#ctl00_MainContent_Clearance_Justification_PMConcurrenceDate_ctrl_dateInput").val('');
        } else {
            $(".PMSponConDate").show();
           // $("#ctl00_MainContent_Clearance_Justification_PMConcurrenceDate_ctrl_dateInput").val('');
        }

    });

});

function initialize() {
    var gotSponCon = $('#ctl00_MainContent_Edit_Clearance_Justification_GovtSponsorConcurrence_ctrl_Input').val(); // On EditNonContractual Page
    var nonContractualGovtSponCon = $('#ctl00_MainContent_Clearance_Justification_GovtSponsorConcurrence_ctrl_Input').val(); //On NonContractual Page
    console.log(gotSponCon);
    

    if (gotSponCon != "Yes" && nonContractualGovtSponCon != "Yes") {
            $(".GovtSponConDate").hide();
        } else {
            $(".GovtSponConDate").show();
        }

      //  var primeSponCon = $('.PrimeSponCon').val();
        var primeSponCon = $('#ctl00_MainContent_Edit_Clearance_Justification_PrimeSponsorConcurrence_ctrl_Input').val();
        var nonContractualPrimeSponCon = $('#ctl00_MainContent_Clearance_Justification_PrimeSponsorConcurrence_ctrl_Input').val();
        
        console.log(primeSponCon);
        if (primeSponCon != "Yes" && nonContractualPrimeSponCon != "Yes") {
            $(".PrimeSponConDate").hide();
        } else {
            $(".PrimeSponConDate").show();
        }

        //var pmSponCon = $('.PMSponCon').val();
        var pmSponCon = $('#ctl00_MainContent_Edit_Clearance_Justification_PMConcurrence_ctrl_Input').val();
        var nonContractualPmSponCon = $('#ctl00_MainContent_Clearance_Justification_PMConcurrence_ctrl_Input').val();
        
        console.log(pmSponCon);
        if (pmSponCon != "Yes" && nonContractualPmSponCon != "Yes") {
            $(".PMSponConDate").hide();
        } else {
            $(".PMSponConDate").show();
        }
}

function SetDate() {
    dateVar = new Date();
    var datepicker = $find("<%= RadDatePicker1.ClientID %>");
    datepicker.set_selectedDate(dateVar);
}