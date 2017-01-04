var currentIndex;
var prevIndex = -1;
$(document).ready(function () {
    //enable fancybox for pickers
    $(".fancybox").fancybox();

    //menu dropdowns
    $('ul.menu').each(function () {
        // add the menu toggle
        $(this).prepend('<li class="menu-toggle"><a href="#"><span class="icon" data-icon="Y"></span> Menu</a></li>');

        // find menu items with children.
        $(this).find('li').has('ul').addClass('has-menu')
        .find('a:first').append('<span class="arrow">&nbsp;</span>');
    });

    $('ul.menu li').hover(function () {
        $(this).find('ul:first').stop(true, true).fadeIn('fast');
        $(this).addClass('hover');
    },
    function () {
        $(this).find('ul').stop(true, true).fadeOut('slow');
        $(this).removeClass('hover');
    });

    //enable responsive menu
    $('#nav').meanmenu(
        {
            meanRevealColour: "#fff",
            meanScreenWidth: "991",
            meanRemoveAttrs: true
        });

    /*--------------------Custome Script----------------------------*/
    $('#ctl00_MainContent_Sponsored_employees_ctl09').css('overflow-x', 'inherit');
    /*-------------------------------Styling Status Columns--------------------------------*/
    $('#ctl00_MainContent_postDepartureSection_ctl08_ctl00 > tbody tr').each(function ()
    {
        $(this).find('td:last-child').addClass('status');
        var status = $(this).find('td.status').html();

        var font_color = $(this).find('td.status').parent().css('background-color');
        $(this).find('td.status').parent().find('td:first-child').css('color', font_color);

        if (status == "Deleted") {
            $(this).find('td.status').wrapInner('<span class="red"></span>');
        }
        else if (status == "Added") {
            $(this).find('td.status').wrapInner('<span class="green"></span>');
        }
        else if (status == "Modified") {
            $(this).find('td.status').addClass('blue');
            $(this).find('td.status').wrapInner('<span class="blue"></span>');
            $('#ctl00_MainContent_postDepartureSection_ctl08').css('overflow-x', 'inherit');
        }
        else if (status == "Unchanged") {
            $(this).find('td.status').wrapInner('<span class="white"></span>');
        }
    })

    $('#ctl00_MainContent_postItinerarySection_ctl08_ctl00 > tbody tr').each(function ()
    {
        $(this).find('td:last-child').addClass('status');
        var status = $(this).find('td.status').html();

        var font_color = $(this).find('td.status').parent().css('background-color');
        $(this).find('td.status').parent().find('td:first-child').css('color', font_color);

        if (status == "Deleted")
        {
            $(this).find('td.status').wrapInner('<span class="red"></span>');
        }
        else if (status == "Added")
        {
            $(this).find('td.status').wrapInner('<span class="green"></span>');
        }
        else if (status == "Modified")
        {
            $(this).find('td.status').addClass('blue');
            $(this).find('td.status').wrapInner('<span class="blue"></span>');
            $('#ctl00_MainContent_postItinerarySection_ctl08').css('overflow-x', 'inherit');
        }
        else if (status == "Unchanged")
        {
            $(this).find('td.status').wrapInner('<span class="white"></span>');
        }
    })

    $('#ctl00_MainContent_postAccommodationSection_ctl08_ctl00 > tbody tr').each(function ()
    {
        $(this).find('td:last-child').addClass('status');
        var status = $(this).find('td.status').html();

        var font_color = $(this).find('td.status').parent().css('background-color');
        $(this).find('td.status').parent().find('td:first-child').css('color', font_color);

        if (status == "Deleted")
        {
            $(this).find('td.status').wrapInner('<span class="red"></span>');
        }
        else if (status == "Added")
        {
            $(this).find('td.status').wrapInner('<span class="green"></span>');
        }
        else if (status == "Modified")
        {
            $(this).find('td.status').addClass('blue');
            $(this).find('td.status').wrapInner('<span class="blue"></span>');
            $('#ctl00_MainContent_postAccommodationSection_ctl08').css('overflow-x', 'inherit');
        }
        else if (status == "Unchanged")
        {
            $(this).find('td.status').wrapInner('<span class="white"></span>');
        }
    })

    $('#ctl00_MainContent_postReturnSection_ctl08_ctl00 > tbody tr').each(function ()
    {
        $(this).find('td:last-child').addClass('status');
        var status = $(this).find('td.status').html();

        var font_color = $(this).find('td.status').parent().css('background-color');
        $(this).find('td.status').parent().find('td:first-child').css('color', font_color);

        if (status == "Deleted")
        {
            $(this).find('td.status').wrapInner('<span class="red"></span>');
        }
        else if (status == "Added")
        {
            $(this).find('td.status').wrapInner('<span class="green"></span>');
        }
        else if (status == "Modified")
        {
            $(this).find('td.status').addClass('blue');
            $(this).find('td.status').wrapInner('<span class="blue"></span>');
            $('#ctl00_MainContent_postReturnSection_ctl08').css('overflow-x', 'inherit');
        }
        else if (status == "Unchanged")
        {
            $(this).find('td.status').wrapInner('<span class="white"></span>');
        }
    })
    /*----------------------------------------------------------------------------------------*/

    /*----------------------------Hover Effect-------------------------------*/
    $('#ctl00_MainContent_postDepartureSection_ctl08_ctl00 tr td.blue').hover(
                function () {
                    var id = parseInt($(this).parent().find('td:first-child').html());

                    currentIndex = $(this).parent().parent().children().index($(this).parent());

                    if (prevIndex != -1) {
                        $('.rgMasterTable tbody tr.rgRow:nth-child(' + (parseInt(prevIndex) + 1) + ') td.status #tooltip').detach();
                        $('.rgMasterTable tbody tr.rgAltRow:nth-child(' + (parseInt(prevIndex) + 1) + ') td.status #tooltip').detach();
                    }

                    prevIndex = currentIndex;

                    $('.rgMasterTable tbody tr.rgRow:nth-child(' + (parseInt(currentIndex) + 1) + ')').find('td.status').append('<div id="tooltip"></div>');
                    $('.rgMasterTable tbody tr.rgAltRow:nth-child(' + (parseInt(currentIndex) + 1) + ')').find('td.status').append('<div id="tooltip"></div>');

                    showTooltip($(this).parent().height());

                    $.ajax({
                        url: "../../services/ComparisonService/" + id + "/1",
                        method: 'POST',
                        success: tooltipData,
                        error: errorMsg
                    });
                },
                function () {
                    hideTooltip();
                }
        );

    $('#ctl00_MainContent_postItinerarySection_ctl08_ctl00 tr td.blue').hover(
                function () {
                    var id = parseInt($(this).parent().find('td:first-child').html());
                    
                    currentIndex = $(this).parent().parent().children().index($(this).parent());

                    if (prevIndex != -1) {
                        $('.rgMasterTable tbody tr.rgRow:nth-child(' + (parseInt(prevIndex) + 1) + ') td.status #tooltip').detach();
                        $('.rgMasterTable tbody tr.rgAltRow:nth-child(' + (parseInt(prevIndex) + 1) + ') td.status #tooltip').detach();
                    }

                    prevIndex = currentIndex;

                    $('.rgMasterTable tbody tr.rgRow:nth-child(' + (parseInt(currentIndex) + 1) + ')').find('td.status').append('<div id="tooltip"></div>');
                    $('.rgMasterTable tbody tr.rgAltRow:nth-child(' + (parseInt(currentIndex) + 1) + ')').find('td.status').append('<div id="tooltip"></div>');

                    showTooltip($(this).parent().height());

                    $.ajax({
                        url: "../../services/ComparisonService/" + id + "/2",
                        method: 'POST',
                        success: tooltipData,
                        error: errorMsg
                    });
                },
                function () {
                   hideTooltip();
                }
        );

    $('#ctl00_MainContent_postAccommodationSection_ctl08_ctl00 tr td.blue').hover(
                function () {
                    var id = parseInt($(this).parent().find('td:first-child').html());
                    
                    currentIndex = $(this).parent().parent().children().index($(this).parent());

                    if (prevIndex != -1) {
                        $('.rgMasterTable tbody tr.rgRow:nth-child(' + (parseInt(prevIndex) + 1) + ') td.status #tooltip').detach();
                        $('.rgMasterTable tbody tr.rgAltRow:nth-child(' + (parseInt(prevIndex) + 1) + ') td.status #tooltip').detach();
                    }

                    prevIndex = currentIndex;

                    $('.rgMasterTable tbody tr.rgRow:nth-child(' + (parseInt(currentIndex) + 1) + ')').find('td.status').append('<div id="tooltip"></div>');
                    $('.rgMasterTable tbody tr.rgAltRow:nth-child(' + (parseInt(currentIndex) + 1) + ')').find('td.status').append('<div id="tooltip"></div>');

                    showTooltip($(this).parent().height());

                    $.ajax({
                        url: "../../services/ComparisonService/" + id + "/3",
                        method: 'POST',
                        success: tooltipData,
                        error: errorMsg
                    });
                },
                function () {
                    hideTooltip();
                }
        );

    $('#ctl00_MainContent_postReturnSection_ctl08_ctl00 tr td.blue').hover(
                function () {
                    var id = parseInt($(this).parent().find('td:first-child').html());
                    
                    currentIndex = $(this).parent().parent().children().index($(this).parent());

                    if (prevIndex != -1) {
                        $('.rgMasterTable tbody tr.rgRow:nth-child(' + (parseInt(prevIndex) + 1) + ') td.status #tooltip').detach();
                        $('.rgMasterTable tbody tr.rgAltRow:nth-child(' + (parseInt(prevIndex) + 1) + ') td.status #tooltip').detach();
                    }

                    prevIndex = currentIndex;

                    $('.rgMasterTable tbody tr.rgRow:nth-child(' + (parseInt(currentIndex) + 1) + ')').find('td.status').append('<div id="tooltip"></div>');
                    $('.rgMasterTable tbody tr.rgAltRow:nth-child(' + (parseInt(currentIndex) + 1) + ')').find('td.status').append('<div id="tooltip"></div>');

                    showTooltip($(this).parent().height());

                   $.ajax({
                       url: "../../services/ComparisonService/" + id + "/4",
                        method: 'POST',
                        success: tooltipData,
                        error: errorMsg
                    });
                },
                function () {
                    hideTooltip();
                }
        );
    /*-----------------------------------------------------------------------*/
});

function showTooltip(height)
{
    $('#tooltip').css('opacity', '1').css('top', (height + 6) + 'px').css('right', '0px').css('pointer-events', 'auto');
}

function hideTooltip()
{
    $('#tooltip').css('opacity', '0').css('top', '156px').css('pointer-events', 'none');;
}

function tooltipData(data)
{
    var sectionNumber = data[0].SectionNumber;

    if (sectionNumber == 1)
    {
        var departureDate = ((data[0].DepartureDate).split('T'))[0];

        $('#tooltip').html("<div id='detailsForm'>" +
                                "<label for='field1'><span>Departure Date:</span><input type='text' id='departure_date' name='field1'/></label>" +
                                "<label for='field2'><span>Departure City:</span><input type='text' id='departure_city' name='field2'/></label>" +
                                "<label for='field3'><span>Arrival City:</span><input type='text' id='arrival_city' name='field3'/></label>" +
                                "<label for='field4'><span>Transport:</span><input type='text' id='transport' name='field4'/></label>" +
                                "<div id='travel_details'></div>" +
                            "</div>")

        $('#departure_date').val(departureDate);
        $('#departure_city').val(data[0].DepartureCity);
        $('#arrival_city').val(data[0].ArrivalCity);
        $('#transport').val(data[0].TransportationModeDisplay);
        $('#travel_details').html(data[0].TransportationModeDetails);
    }
    else if (sectionNumber == 2)
    {        
        var arrivalDate = ((data[0].ArrivalDate).split('T'))[0];
        var departureDate = ((data[0].DepartureDate).split('T'))[0];

        $('#tooltip').html("<div id='detailsForm'>"+
                                "<label for='field1'><span>Country:</span><input type='text' id='country' name='field1'/></label>" +
                                "<label for='field2'><span>Arrival Date:</span><input type='text' id='arrival_date' name='field2'/></label>" +
                                "<label for='field3'><span>Departure Date:</span><input type='text' id='departure_date' name='field3'/></label>" +
                                "<label for='field4'><span>Visited City:</span><input type='text' id='visited_city' name='field4'/></label>" +
                            "</div>")

        $('#country').val(data[0].Country);
        $('#arrival_date').val(arrivalDate);
        $('#departure_date').val(departureDate);
        $('#visited_city').val(data[0].VisitedCity);
    }
    else if (sectionNumber == 3)
    {
        console.log(data);
        var arrivalDate = "";
        var departureDate = "";
        if (data[0].ArrivalDate != null) {
             arrivalDate = ((data[0].ArrivalDate).split('T'))[0];
        }

        if (data[0].DepartureDate != null) {
            departureDate = ((data[0].DepartureDate).split('T'))[0];
        }

        $('#tooltip').html("<div id='detailsForm'>" +
                                "<label for='field1'><span>Arrival Date:</span><input type='text' id='arrival_date' name='field1'/></label>" +
                                "<label for='field2'><span>Departure Date:</span><input type='text' id='departure_date' name='field2'/></label>" +
                                "<label for='field3'><span>Accommodation Type:</span><input type='text' id='accommodation_type' name='field3'/></label>" +
                                "<label for='field4'><span>Name of Accommodation:</span><input type='text' id='accommodation_name' name='field4'/></label>" +
                                "<label for='field5'><span>Visited City:</span><input type='text' id='visited_city' name='field5'/></label>" +
                                "<label for='field6'><span>Country:</span><input type='text' id='country' name='field6'/></label>" +
                            "</div>")

        $('#arrival_date').val(arrivalDate);
        $('#departure_date').val(departureDate);
        $('#visited_city').val(data[0].City);
        $('#accommodation_type').val(data[0].AccommodationType);
        $('#accommodation_name').val(data[0].AccommodationName);
        $('#country').val(data[0].Country);
    }
    else if (sectionNumber = 4)
    {
        var returnDate = ((data[0].ReturnDate).split(' '))[0];

        $('#tooltip').html("<div id='detailsForm'>" +
                                "<label for='field1'><span>Return Date:</span><input type='text' id='return_date' name='field1'/></label>" +
                                "<label for='field2'><span>Departure City:</span><input type='text' id='departure_city' name='field2'/></label>" +
                                "<label for='field3'><span>Arrival City:</span><input type='text' id='arrival_city' name='field3'/></label>" +
                                "<label for='field4'><span>Transport:</span><input type='text' id='transport' name='field4'/></label>" +
                                "<div id='travel_details'></div>" +
                            "</div>")

        $('#return_date').val(returnDate);
        $('#departure_city').val(data[0].DepartureCity);
        $('#arrival_city').val(data[0].ArrivalCity);
        $('#transport').val(data[0].TransportationModeDisplay);
        $('#travel_details').html(data[0].TransportationModeDetails);
    }
}

function errorMsg(err)
{
    console.log(err);
}
/*-------------------------------------------------------------------------*/