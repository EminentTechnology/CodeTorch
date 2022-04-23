$(function() {

    var url = $('.payment-footer-add').attr('href');
    var jobId = getUrlParameter('JobId');
    console.log('jobid', jobId);
    url = url.replace('{querystring:JobId}', jobId);
    $('.payment-footer-add').attr('href', url);

    $('.payment-method-select > a').addClass("btn btn-xs btn-primary");

  
    function getUrlParameter(name) {
        name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
        var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
        var results = regex.exec(location.search);
        return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
    };
});
