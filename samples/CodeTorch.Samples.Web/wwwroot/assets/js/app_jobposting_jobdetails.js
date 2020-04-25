$(function() {

    $('#ctl00_MainContent_Enter_details_about_your_Job_BillRate_ctrl').prop("disabled", true);
    $('#ctl00_MainContent_Enter_details_about_your_Job_BillRate_ctrl').prop("readonly", true);
    $('#ctl00_MainContent_Enter_details_about_your_Job_BillRate_ctrl').css("background-color", "#f1f1f2");

    $('#ctl00_MainContent_Enter_details_about_your_Job_StaffRate_ctrl').css("margin-bottom", 0);
    $("#ctl00_MainContent_Enter_details_about_your_Job_StaffRate_ctrl").change(function () {

        var rate = $("#ctl00_MainContent_Enter_details_about_your_Job_StaffRate_ctrl").val();
        var multiplier = 1.25;
        var total = (rate * multiplier).toFixed(2);

        $("#ctl00_MainContent_Enter_details_about_your_Job_BillRate_ctrl").val(total);
    });

  
    
});
