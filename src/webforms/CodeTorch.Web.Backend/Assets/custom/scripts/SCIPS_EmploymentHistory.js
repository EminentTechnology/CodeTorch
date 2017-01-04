$(document).ready(function () {
    initialize();

    $('.IsEmploymentScenarios').change(function () {
        if ($(this).val() != "Yes") {
            $(".IsEmploymentScenariosType").hide();
            $(".IsEmploymentScenariosComment").hide();
        } else {
            $(".IsEmploymentScenariosType").show();
            $(".IsEmploymentScenariosComment").show();
        }

    });

    $('.IsClassifiedInfoInvolvement').change(function () {
        if ($(this).val() != "Yes") {
            $(".IsClassifiedInfoInvolvementReason").hide();
        } else {
            $(".IsClassifiedInfoInvolvementReason").show();
        }

    });

    $('.IsClearanceSuspended').change(function () {
        if ($(this).val() != "Yes") {
            $(".IsClearanceSuspendedReason").hide();
        } else {
            $(".IsClearanceSuspendedReason").show();
        }
    });

    $('.IsCurrentPreviousAccess').change(function () {
        if ($(this).val() != "Yes") {
            $(".AccessHeldDateFrom").hide();
            $(".AccessHeldDateTo").hide(); 
            $(".GrantingAgency").hide();
            $(".InvestigationDate").hide();
        } else {
            $(".AccessHeldDateFrom").show();
            $(".AccessHeldDateTo").show();
            $(".GrantingAgency").show();
            $(".AccessHeldDateTo").show();
            $(".InvestigationDate").show();
        }

    });


    $('.CounterIntelligencePoly').change(function () {
        if ($(this).val() != "Yes") {
            $(".CounterIntelligencePolyDate").hide();
        } else {
            $(".CounterIntelligencePolyDate").show();
        }
    });

    $('.FullscopeLifestylePoly').change(function () {
        if ($(this).val() != "Yes") {
            $(".FullscopeLifestylePolyDate").hide();
        } else {
            $(".FullscopeLifestylePolyDate").show();
        }
    });

    $('.IsSCI_SAPAccessHeld').change(function () {
        if ($(this).val() != "Yes") {
            $(".IsSCI_SAPAccessHeldDate").hide();
        } else {
            $(".IsSCI_SAPAccessHeldDate").show();
        }
    });

    $('.IsUSGovtOverthrow').change(function () {
            if ($(this).val() != "Yes") {
                $(".IsUSGovtOverthrowDetails").hide();
            } else {
                $(".IsUSGovtOverthrowDetails").show();
            }
        });

    $('.IsDelinquentOver180Days').change(function () {
            if ($(this).val() != "Yes") {
                $(".IsDelinquentOver180DaysDetails").hide();
            } else {
                $(".IsDelinquentOver180DaysDetails").show();
            }
        });

    $('.IsBillReferred_SF86').change(function () {
            if ($(this).val() != "Yes") {
                $(".IsBillReferred_SF86Details").hide();
            } else {
                $(".IsBillReferred_SF86Details").show();
            }
        });

});

function initialize() {
    var isEmploymentScenarios = $('#ctl00_MainContent_EmploymentHistory_IsEmploymentScenarios_ctrl_Input').val();
    var isClassifiedInfoInvolvement = $('#ctl00_MainContent_EmploymentHistory_IsClassifiedInfoInvolvement_ctrl_Input').val();
    var isClearanceSuspended = $('#ctl00_MainContent_EmploymentHistory_IsClearanceSuspended_ctrl_Input').val();
    var isCurrentPreviousAccess = $('#ctl00_MainContent_EmploymentHistory_IsCurrentPreviousAccess_ctrl_Input').val();
     
    var counterIntelligencePoly = $('#ctl00_MainContent_EmploymentHistory_CounterIntelligencePoly_ctrl_Input').val();

    var fullscopeLifestylePoly = $('#ctl00_MainContent_EmploymentHistory_FullscopeLifestylePoly_ctrl_Input').val();
    var isSCI_SAPAccessHeld = $('#ctl00_MainContent_EmploymentHistory_IsSCI_SAPAccessHeld_ctrl_Input').val();
    var isUSGovtOverthrow = $('#ctl00_MainContent_EmploymentHistory_IsUSGovtOverthrow_ctrl_Input').val();
   

    //if (isEmploymentScenarios != "Yes") {
    //    $(".IsEmploymentScenariosType").hide();
    //} else {
    //    $(".IsEmploymentScenariosType").show();
    //}

    if (isEmploymentScenarios == "No") {
        $(".IsEmploymentScenariosType").hide(); 
        $(".IsEmploymentScenariosComment").hide();
    } else {
        $(".IsEmploymentScenariosType").show();
        $(".IsEmploymentScenariosComment").show();
    }

    if (isClassifiedInfoInvolvement != "Yes") {
        $(".IsClassifiedInfoInvolvementReason").hide();
    } else {
        $(".IsClassifiedInfoInvolvementReason").show();
    }

    if (isClearanceSuspended != "Yes") {
        $(".IsClearanceSuspendedReason").hide();
    } else {
        $(".IsClearanceSuspendedReason").show();
    }

    if (isCurrentPreviousAccess != "Yes") {
        $(".AccessHeldDateFrom").hide();
        $(".AccessHeldDateTo").hide();
        $(".GrantingAgency").hide();
        $(".InvestigationDate").hide();
    } else {
        $(".AccessHeldDateFrom").show();
        $(".AccessHeldDateTo").show();
        $(".GrantingAgency").show();
        $(".AccessHeldDateTo").show();
        $(".InvestigationDate").show();
    }

    if (counterIntelligencePoly != "Yes") {
        $(".CounterIntelligencePolyDate").hide();
    } else {
        $(".CounterIntelligencePolyDate").show();
    }

    if (fullscopeLifestylePoly != "Yes") {
        $(".FullscopeLifestylePolyDate").hide();
    } else {
        $(".FullscopeLifestylePolyDate").show();
    }

    if (isSCI_SAPAccessHeld != "Yes") {
        $(".IsSCI_SAPAccessHeldDate").hide();
    } else {
        $(".IsSCI_SAPAccessHeldDate").show();
    }

    if (isUSGovtOverthrow != "Yes") {
        $(".IsUSGovtOverthrowDetails").hide();
    } else {
        $(".IsUSGovtOverthrowDetails").show();
    }
}
