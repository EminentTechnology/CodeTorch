$(function() {

    $(".review-location").append("&nbsp;&nbsp;&nbsp;<a href='Locations?JobId=" + getUrlParameter("JobId") + "' class='btn btn-default btn-xs icon-btn md-btn-flat article-tooltip text-muted' title='Edit'><i class='ion ion-md-create'></i></a>");

    $(".review-jobtype").append("&nbsp;&nbsp;&nbsp;<a href='JobDetails?JobId=" + getUrlParameter("JobId") + "' class='btn btn-default btn-xs icon-btn md-btn-flat article-tooltip text-muted' title='Edit'><i class='ion ion-md-create'></i></a>");
    $(".review-hourlyrate").append("&nbsp;&nbsp;&nbsp;<a href='JobDetails?JobId=" + getUrlParameter("JobId") + "' class='btn btn-default btn-xs icon-btn md-btn-flat article-tooltip text-muted' title='Edit'><i class='ion ion-md-create'></i></a>");
    $(".review-additionalinfo").append("&nbsp;&nbsp;&nbsp;<a href='JobDetails?JobId=" + getUrlParameter("JobId") + "' class='btn btn-default btn-xs icon-btn md-btn-flat article-tooltip text-muted' title='Edit'><i class='ion ion-md-create'></i></a>");

    $(".review-startson").append("&nbsp;&nbsp;&nbsp;<a href='ReviewShifts?JobId=" + getUrlParameter("JobId") + "' class='btn btn-default btn-xs icon-btn md-btn-flat article-tooltip text-muted' title='Edit'><i class='ion ion-md-create'></i></a>");
    $(".review-endson").append("&nbsp;&nbsp;&nbsp;<a href='ReviewShifts?JobId=" + getUrlParameter("JobId") + "' class='btn btn-default btn-xs icon-btn md-btn-flat article-tooltip text-muted' title='Edit'><i class='ion ion-md-create'></i></a>");
    $(".review-shiftshours").append("&nbsp;&nbsp;&nbsp;<a href='ReviewShifts?JobId=" + getUrlParameter("JobId") + "' class='btn btn-default btn-xs icon-btn md-btn-flat article-tooltip text-muted' title='Edit'><i class='ion ion-md-create'></i></a>");

    $(".review-paymentmethod").append("&nbsp;&nbsp;&nbsp;<a href='PaymentMethods?JobId=" + getUrlParameter("JobId") + "' class='btn btn-default btn-xs icon-btn md-btn-flat article-tooltip text-muted' title='Edit'><i class='ion ion-md-create'></i></a>");
  
    function getUrlParameter(name) {
        name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
        var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
        var results = regex.exec(location.search);
        return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
    };
});
