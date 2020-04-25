$(function() {

    $('#MainContent_Add_your_shifts_EndTime_ctrl').prop("disabled", true);
    $('#MainContent_Add_your_shifts_EndTime_ctrl').prop("readonly", true);
    $('#MainContent_Add_your_shifts_EndTime_ctrl').css("background-color", "#f1f1f2");

    $("#ctl00_MainContent_Add_your_shifts_StartTme_ctrl_Input").change(calculateEndTime);
    $("#ctl00_MainContent_Add_your_shifts_BillableHours_ctrl_Input").change(calculateEndTime);
    $("#ctl00_MainContent_Add_your_shifts_UnbillableHours_ctrl_Input").change(calculateEndTime);

  
    
});

function calculateEndTime()
{
    var startDate = $("#ctl00_MainContent_Add_your_shifts_StartDate_ctrl_dateInput").val();
    var startTime = $("#ctl00_MainContent_Add_your_shifts_StartTme_ctrl_Input").val();
    var workingHrs = $("#ctl00_MainContent_Add_your_shifts_BillableHours_ctrl_Input").val();
    var breakHrs = $("#ctl00_MainContent_Add_your_shifts_UnbillableHours_ctrl_Input").val();

    var start = moment(startTime, moment.CUSTOM_FORMAT, "HH:mm");

  
}

Number.isInteger = Number.isInteger || function (value) {
    return typeof value === "number" &&
        isFinite(value) &&
        Math.floor(value) === value;
};

Number.isNaN = Number.isNaN || function (value) {
    return value !== value;
}