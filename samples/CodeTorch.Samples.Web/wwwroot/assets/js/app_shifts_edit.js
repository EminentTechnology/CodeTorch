$(function() {

    $('#ctl00_MainContent_ShiftDetails_StaffRate_ctrl').prop("disabled", true);
    $('#ctl00_MainContent_ShiftDetails_StaffRate_ctrl').prop("readonly", true);
    $('#ctl00_MainContent_ShiftDetails_StaffRate_ctrl').css("background-color", "#f1f1f2");

    $('#ctl00_MainContent_ShiftDetails_BillRate_ctrl').prop("disabled", true);
    $('#ctl00_MainContent_ShiftDetails_BillRate_ctrl').prop("readonly", true);
    $('#ctl00_MainContent_ShiftDetails_BillRate_ctrl').css("background-color", "#f1f1f2");

    $('#ctl00_MainContent_ShiftDetails_ShiftRate_ctrl').prop("disabled", true);
    $('#ctl00_MainContent_ShiftDetails_ShiftRate_ctrl').prop("readonly", true);
    $('#ctl00_MainContent_ShiftDetails_ShiftRate_ctrl').css("background-color", "#f1f1f2");

    $('#ctl00_MainContent_Enter_details_about_your_Job_StaffRate_ctrl').css("margin-bottom", 0);

    $("#ctl00_MainContent_ShiftDetails_BillableHours_ctrl_Input").change(function () {

        

        var rate = $("#ctl00_MainContent_ShiftDetails_BillRate_ctrl").val();
        var hrs_text = $("#ctl00_MainContent_ShiftDetails_BillableHours_ctrl_Input").val().replace('hrs', '').replace(':00', '.0').replace(':30', '.5');
        var total = (rate * hrs_text).toFixed(2);

        $("#ctl00_MainContent_ShiftDetails_ShiftRate_ctrl").val(total);
        

        console.log('working hours changed', rate, hrs_text, total);
    });

  
    
});
