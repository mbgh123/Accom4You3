$(document).ready(function () {

    $("ul.pictures a").fancybox({
        prevEffect: 'none',
        nextEffect: 'none'
    });


    $("a.maplink").on("click", null, function () {
        $('#mapDiv').show();

        var lng = $(this).attr('data-a4u-long');
        var lat = $(this).attr('data-a4u-lat');
        var address = $(this).attr('data-a4u-address');

        GetMap(lng, lat, address);

        //return false;
    });

    $("a.maplink").fancybox({
        autoSize: true,
        closeClick: false,
        openEffect: 'none',
        closeEffect: 'none',
        beforeClose: function () { $('#mapDiv').html(''); },
    });

});

function GetMap(lng, lat, address) {

    var myLatlng = new google.maps.LatLng(lat, lng);

    var mapOptions = {
        center: myLatlng,
        zoom: 16,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    map = new google.maps.Map(document.getElementById("mapDiv"), mapOptions);

    var marker = new google.maps.Marker({
        map: map,
        position: myLatlng,
        title: address
    });


    //geocoder = new google.maps.Geocoder();

    //geocoder.geocode({ 'address': postcode }, function (results, status) {
    //    if (status == google.maps.GeocoderStatus.OK) {
    //        map.setCenter(results[0].geometry.location);
    //        var marker = new google.maps.Marker({
    //            map: map,
    //            position: results[0].geometry.location
    //        });
    //    } else {
    //        alert("Geocode was not successful for the following reason: " + status);
    //    }
    //});
}