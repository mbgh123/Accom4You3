

var a4u2 = (function (a4u2) {

    // Initialization. the mapLink element is outside the ng view so is available.
    $("a.maplink").fancybox({
        autoSize: true,
        closeClick: false,
        openEffect: 'none',
        closeEffect: 'none',
        beforeClose: function () { $('#mapDiv').html(''); },
    });


    a4u2.showMap = function (event) {

        $('#mapDiv').show();

        // For ng-click, we've passed in $event from the view
        var lng = event.target.attributes['data-a4u-long'].value;
        var lat = event.target.attributes['data-a4u-lat'].value;
        var address = event.target.attributes['data-a4u-address'].value;

        _getMap(lng, lat, address);
    }

    function _getMap(lng, lat, address) {

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
    }

    return a4u2;
}

)(a4u2 || {});



