(function () {
    'use strict';

    angular
        .module('a4u2app')
        .controller('propertyDetailsController', propertyDetailsController);

    propertyDetailsController.$inject = ['$routeParams', '$http' ];

    function propertyDetailsController($routeParams, $http ) {
        /* jshint validthis:true */

        _fancyBox();

        var vm = this;

        vm.propertyId = $routeParams.propertyId;
        vm.errorMessage = "";
        vm.isBusy = true;
        vm.property = {};

        //vm.frommap = $location.search().frommap // get query string frommap
        // $routeParams also contains the query string items
        vm.frommap = $routeParams.frommap // get query string frommap


        // same url for Post and Get
        var url = "/api/properties/" + vm.propertyId;

        // Get the property

        $http.get(url)
           .then(function (response) {
               // Success function

               var result = response.data;  // the json from server is parsed to an object (and array of objects in this case)

               //angular.copy(result, vm.stops);
               vm.property = result;
           },
                function (error) {
                    // Failure function
                    vm.errorMessage = "Failed to load property: " + error;

                })
            .finally(function () {
                vm.isBusy = false;

                if (vm.property.location) {
                    _showMap(vm.property.location.lat, vm.property.location.long, vm.property.address);
                }
            });
    }

    // _prefix is a shorthand for a private function
    function _showMap(lat, long, address) {
        // Show map

        $('#map').show();

        var myLatlng = new google.maps.LatLng(lat, long);

        var mapOptions = {
            center: myLatlng,
            zoom: 16,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };

        var map = new google.maps.Map(document.getElementById("map"), mapOptions);

        var marker = new google.maps.Marker({
            map: map,
            position: myLatlng,
            title: address
        });

        //google.maps.event.addDomListener(window, 'page:load', initialize);
    };

    function _fancyBox() {

        // Attach fancybox to our pictures
        $("ul.pictures a").fancybox({
            prevEffect: 'none',
            nextEffect: 'none'
        });
    }

})();
