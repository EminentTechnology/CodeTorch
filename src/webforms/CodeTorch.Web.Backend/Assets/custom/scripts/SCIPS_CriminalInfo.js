$(document).ready(function () {
    initialize();

    $('.IsArrested').change(function () {
        if ($(this).val() != "Yes") {
            $(".IsArrestedReason").hide();
        } else {
            $(".IsArrestedReason").show();
        }

    });

    $('.IsChargesPending').change(function () {
        if ($(this).val() != "Yes") {
            $(".IsChargesPendingDetails").hide();
        } else {
            $(".IsChargesPendingDetails").show();
        }

    });

    $('.IsChargedSecurityViolation').change(function () {
        if ($(this).val() != "Yes") {
            $(".IsChargedSecurityViolationDetails").hide();
        } else {
            $(".IsChargedSecurityViolationDetails").show();
        }

    });

    $('.IsChargedMisdemeanor').change(function () {
        if ($(this).val() != "Yes") {
            $(".IsChargedMisdemeanorDetails").hide();
        } else {
            $(".IsChargedMisdemeanorDetails").show();
        }

    });

    $('.IsChargedDrugAlcohol').change(function () {
        if ($(this).val() != "Yes") {
            $(".IsChargedDrugAlcoholDetails").hide();
        } else {
            $(".IsChargedDrugAlcoholDetails").show();
        }
    });

        $('.IsAlcoholAdverseInvolvement').change(function () {
            if ($(this).val() != "Yes") {
                $(".IsAlcoholAdverseInvolvementDetails").hide();
            } else {
                $(".IsAlcoholAdverseInvolvementDetails").show();
            }
        });

        $('.IsIllegalDrugUsed_SF86').change(function () {
            if ($(this).val() != "Yes") {
                $(".IsIllegalDrugUsed_SF86Details").hide();
            } else {
                $(".IsIllegalDrugUsed_SF86Details").show();
            }
        });

        $('.IsIllegalDrugBought').change(function () {
            if ($(this).val() != "Yes") {
                $(".IsIllegalDrugBoughtDetails").hide();
            } else {
                $(".IsIllegalDrugBoughtDetails").show();
            }
        });

});

function initialize() {
    var isArrested = $('#ctl00_MainContent_FederalAccessRequirements_IsArrested_ctrl_Input').val();
    var isChargesPending = $('#ctl00_MainContent_FederalAccessRequirements_IsChargesPending_ctrl_Input').val();
    var isChargedSecurityViolation = $('#ctl00_MainContent_FederalAccessRequirements_IsChargedSecurityViolation_ctrl_Input').val();
    var isChargedMisdemeanor = $('#ctl00_MainContent_FederalAccessRequirements_IsChargedMisdemeanor_ctrl_Input').val();
     
    var isChargedDrugAlcohol = $('#ctl00_MainContent_FederalAccessRequirements_IsChargedDrugAlcohol_ctrl_Input').val();
    var isAlcoholAdverseInvolvement = $('#ctl00_MainContent_FederalAccessRequirements_IsAlcoholAdverseInvolvement_ctrl_Input').val();
    var isIllegalDrugUsed_SF86 = $('#ctl00_MainContent_FederalAccessRequirements_IsIllegalDrugUsed_SF86_ctrl_Input').val();
    var isIllegalDrugBought = $('#ctl00_MainContent_FederalAccessRequirements_IsIllegalDrugBought_ctrl_Input').val();


    if (isArrested != "Yes") {
        $(".IsArrestedReason").hide();
    } else {
        $(".IsArrestedReason").show();
    }

    if (isChargesPending != "Yes") {
        $(".IsChargesPendingDetails").hide();
    } else {
        $(".IsChargesPendingDetails").show();
    }

    if (isChargedSecurityViolation != "Yes") {
        $(".IsChargedSecurityViolationDetails").hide();
    } else {
        $(".IsChargedSecurityViolationDetails").show();
    }

    if (isChargedMisdemeanor != "Yes") {
        $(".IsChargedMisdemeanorDetails").hide();
    } else {
        $(".IsChargedMisdemeanorDetails").show();
    }

    if (isChargedDrugAlcohol != "Yes") {
        $(".IsChargedDrugAlcoholDetails").hide();
    } else {
        $(".IsChargedDrugAlcoholDetails").show();
    }

    if (isAlcoholAdverseInvolvement != "Yes") {
        $(".IsAlcoholAdverseInvolvementDetails").hide();
    } else {
        $(".IsAlcoholAdverseInvolvementDetails").show();
    }

    if (isIllegalDrugUsed_SF86 != "Yes") {
        $(".IsIllegalDrugUsed_SF86Details").hide();
    } else {
        $(".IsIllegalDrugUsed_SF86Details").show();
    }

    if (isIllegalDrugBought != "Yes") {
        $(".IsIllegalDrugBoughtDetails").hide();
    } else {
        $(".IsIllegalDrugBoughtDetails").show();
    }

}


function SetDate() {
    dateVar = new Date();
    var datepicker = $find("<%= RadDatePicker1.ClientID %>");
    datepicker.set_selectedDate(dateVar);
}