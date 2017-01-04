$(document).ready(function () {
    initialize();

    $('.MaritalStatusChanged').change(function () {
        if ($(this).val() != "Yes") {
            $(".CurrentMaritalStatus").hide();
            $(".CurrentMaritalStatusCitizenship").hide();
        } else {
            $(".CurrentMaritalStatus").show();
            $(".CurrentMaritalStatusCitizenship").show();
        }

    });

    $('.IsForeignProperty').change(function () {
        if ($(this).val() != "Yes") {
            $(".IsForeignPropertyDetails").hide();
        } else {
            $(".IsForeignPropertyDetails").show();
        }

    });

    $('.IsForeignNationalsRel').change(function () {
        if ($(this).val() != "Yes") {
            $(".ForeignNationalsDetails").hide();
        } else {
            $(".ForeignNationalsDetails").show();
        }

    });

    $('.IsMedicalProfessionalConsulted').change(function () {
        if ($(this).val() != "Yes") {
            $(".IsMedicalProfessionalConsultedDetails").hide();
        } else {
            $(".IsMedicalProfessionalConsultedDetails").show();
        }

    });

});

function initialize() {
    var maritalStatusChanged = $('#ctl00_MainContent_BasicInfo_IsMaritalStatusChanged_ctrl_Input').val();
    var isForeignProperty = $('#ctl00_MainContent_BasicInfo_IsForeignProperty_ctrl_Input').val();
    var isForeignNationalsRel = $('#ctl00_MainContent_BasicInfo_IsForeignNationalsRel_ctrl_Input').val();
    var isMedicalProfessionalConsulted = $('#ctl00_MainContent_BasicInfo_IsMedicalProfessionalConsulted_ctrl_Input').val();
     
    if (maritalStatusChanged != "Yes") {
        $(".CurrentMaritalStatus").hide();
        $(".CurrentMaritalStatusCitizenship").hide();
    } else {
        $(".CurrentMaritalStatus").show();
        $(".CurrentMaritalStatusCitizenship").show();
    }

    if (isForeignProperty != "Yes") {
        $(".IsForeignPropertyDetails").hide();
    } else {
        $(".IsForeignPropertyDetails").show();
    }

    if (isForeignNationalsRel != "Yes") {
        $(".ForeignNationalsDetails").hide();
    } else {
        $(".ForeignNationalsDetails").show();
    }

    if (isMedicalProfessionalConsulted != "Yes") {
        $(".IsMedicalProfessionalConsultedDetails").hide();
    } else {
        $(".IsMedicalProfessionalConsultedDetails").show();
    }
}

function SetDate() {
    dateVar = new Date();
    var datepicker = $find("<%= RadDatePicker1.ClientID %>");
    datepicker.set_selectedDate(dateVar);
}