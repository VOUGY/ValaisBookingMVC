/*
    After every arrival input change, update value and min for departure input (arrival + 1 day)
    use in:
     - Hotel/Index
     - 
*/
$('#Arrival').change(function () {
    var arrival = new Date($('#Arrival').val());
    var departure = new Date();

    departure.setDate(arrival.getDate() + 1);

    var day = ("0" + departure.getDate()).slice(-2);
    var month = ("0" + (departure.getMonth() + 1)).slice(-2);

    var date = departure.getFullYear() + "-" + month + "-" + day;

    $('#Departure').val(date);
    $('#Departure').attr({
        "min": date    
    });
});

/*
Check the checkbox when div with room class is clicked
and ass the css class active
*/
$('.room').click(function () {
    if ($(this).find('input:checkbox').is(":checked")) {

        $(this).find('input:checkbox').attr("checked", false);
        $(this).removeClass('active');
    }
    else {
        $(this).find('input:checkbox').prop("checked", true);
        $(this).addClass('active');
    }
});

/*
Active button when user as clicker the checkbox I have validate
*/
$('#agre').change(function () {
    if ($('#agre:checked').length > 0) {
        $('#submitButton').prop('disabled', false);
    } else {
        $('#submitButton').prop('disabled', true);
    }
});

/*
Make modal reservation visible
*/
$('#reservation').modal({
    backdrop: 'static',
    keyboard: false
});