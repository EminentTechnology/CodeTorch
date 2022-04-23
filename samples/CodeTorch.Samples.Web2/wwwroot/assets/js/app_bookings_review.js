$(function() {

    $(".review-applicant").append("&nbsp;&nbsp;&nbsp;<a href='Edit?BookingId=" + getUrlParameter("BookingId") + "' class='btn btn-default btn-xs icon-btn md-btn-flat article-tooltip text-muted' title='Edit'><i class='ion ion-md-create'></i></a>");

    $(".review-shifts").append("&nbsp;&nbsp;&nbsp;<a href='Shifts?BookingId=" + getUrlParameter("BookingId") + "' class='btn btn-default btn-xs icon-btn md-btn-flat article-tooltip text-muted' title='Edit'><i class='ion ion-md-create'></i></a>");
    
    $(".review-startson").append("&nbsp;&nbsp;&nbsp;<a href='Shifts?BookingId=" + getUrlParameter("BookingId") + "' class='btn btn-default btn-xs icon-btn md-btn-flat article-tooltip text-muted' title='Edit'><i class='ion ion-md-create'></i></a>");
    $(".review-endson").append("&nbsp;&nbsp;&nbsp;<a href='Shifts?BookingId=" + getUrlParameter("BookingId") + "' class='btn btn-default btn-xs icon-btn md-btn-flat article-tooltip text-muted' title='Edit'><i class='ion ion-md-create'></i></a>");
    $(".review-hourlyrate").append("&nbsp;&nbsp;&nbsp;<a href='Edit?BookingId=" + getUrlParameter("BookingId") + "' class='btn btn-default btn-xs icon-btn md-btn-flat article-tooltip text-muted' title='Edit'><i class='ion ion-md-create'></i></a>");
    $(".review-billrate").append("&nbsp;&nbsp;&nbsp;<a href='Edit?BookingId=" + getUrlParameter("BookingId") + "' class='btn btn-default btn-xs icon-btn md-btn-flat article-tooltip text-muted' title='Edit'><i class='ion ion-md-create'></i></a>");

    
  
    function getUrlParameter(name) {
        name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
        var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
        var results = regex.exec(location.search);
        return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
    };
});
