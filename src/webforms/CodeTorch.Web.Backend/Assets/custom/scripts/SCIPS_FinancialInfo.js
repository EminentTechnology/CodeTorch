$(document).ready(function () {
    initialize();

    $('.IsBankruptcyFormFiled').change(function () {
        if ($(this).val() != "Yes") {
            $(".IsBankruptcyFormFiledDetails").hide();
        } else {
            $(".IsBankruptcyFormFiledDetails").show();
        }

    });

    $('.IsDebtsDelinquent').change(function () {
        if ($(this).val() != "Yes") {
            $(".IsDebtsDelinquentDetails").hide();
        } else {
            $(".IsDebtsDelinquentDetails").show();
        }

    });

    $('.IsWageGarnished').change(function () {
        if ($(this).val() != "Yes") {
            $(".IsWageGarnishedDetails").hide();
        } else {
            $(".IsWageGarnishedDetails").show();
        }

    });

    $('.IsPropertyRepossessed').change(function () {
        if ($(this).val() != "Yes") {
            $(".IsPropertyRepossessedDetails").hide();
        } else {
            $(".IsPropertyRepossessedDetails").show();
        }

    });

    $('.IsLienPlacedAgainstProperty').change(function () {
        if ($(this).val() != "Yes") {
            $(".IsLienPlacedAgainstPropertyDetails").hide();
        } else {
            $(".IsLienPlacedAgainstPropertyDetails").show();
        }
    });

    $('.IsJudgementsAgainstYou').change(function () {
            if ($(this).val() != "Yes") {
                $(".IsJudgementsAgainstYouDetails").hide();
            } else {
                $(".IsJudgementsAgainstYouDetails").show();
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
    var isBankruptcyFormFiled = $('#ctl00_MainContent_FinancialInformation_IsBankruptcyFormFiled_ctrl_Input').val();
    var isDebtsDelinquent = $('#ctl00_MainContent_FinancialInformation_IsDebtsDelinquent_ctrl_Input').val();
    var isWageGarnished = $('#ctl00_MainContent_FinancialInformation_IsWageGarnished_ctrl_Input').val();
    var isPropertyRepossessed = $('#ctl00_MainContent_FinancialInformation_IsPropertyRepossessed_ctrl_Input').val();
     
    var isLienPlacedAgainstProperty = $('#ctl00_MainContent_FinancialInformation_IsLienPlacedAgainstProperty_ctrl_Input').val();
    var isJudgementsAgainstYou = $('#ctl00_MainContent_FinancialInformation_IsJudgementsAgainstYou_ctrl_Input').val();
    var isDelinquentOver180Days = $('#ctl00_MainContent_FinancialInformation_IsDelinquentOver180Days_ctrl_Input').val();
    var isBillReferred_SF86 = $('#ctl00_MainContent_FinancialInformation_IsBillReferred_SF86_ctrl_Input').val();


    if (isBankruptcyFormFiled != "Yes") {
        $(".IsBankruptcyFormFiledDetails").hide();
    } else {
        $(".IsBankruptcyFormFiledDetails").show();
    }

    if (isDebtsDelinquent != "Yes") {
        $(".IsDebtsDelinquentDetails").hide();
    } else {
        $(".IsDebtsDelinquentDetails").show();
    }

    if (isWageGarnished != "Yes") {
        $(".IsWageGarnishedDetails").hide();
    } else {
        $(".IsWageGarnishedDetails").show();
    }

    if (isPropertyRepossessed != "Yes") {
        $(".IsPropertyRepossessedDetails").hide();
    } else {
        $(".IsPropertyRepossessedDetails").show();
    }

    if (isLienPlacedAgainstProperty != "Yes") {
        $(".IsLienPlacedAgainstPropertyDetails").hide();
    } else {
        $(".IsLienPlacedAgainstPropertyDetails").show();
    }

    if (isJudgementsAgainstYou != "Yes") {
        $(".IsJudgementsAgainstYouDetails").hide();
    } else {
        $(".IsJudgementsAgainstYouDetails").show();
    }

    if (isDelinquentOver180Days != "Yes") {
        $(".IsDelinquentOver180DaysDetails").hide();
    } else {
        $(".IsDelinquentOver180DaysDetails").show();
    }

    if (isBillReferred_SF86 != "Yes") {
        $(".IsBillReferred_SF86Details").hide();
    } else {
        $(".IsBillReferred_SF86Details").show();
    }

}
