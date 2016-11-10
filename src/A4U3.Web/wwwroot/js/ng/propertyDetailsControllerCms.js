(function () {
    'use strict';

    angular
        .module('a4u2app')
        .controller('propertyDetailsControllerCms', propertyDetailsControllerCms);

    propertyDetailsControllerCms.$inject = ['$routeParams', '$scope', '$http', '$location', '$filter' ];

    function propertyDetailsControllerCms($routeParams, $scope, $http, $location, $filter) {
        /* jshint validthis:true */

        _fancyBox();


        var vm = this;

        vm.propertyId = $routeParams.propertyId;
        vm.errorMessage = "";
        vm.isBusy = true;
        vm.property = {};
        vm.frommap = $location.search().frommap // get query string frommap


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

        //$scope.furnishings = [
        //    { value: 0, text: 'Unfurnished' },
        //    { value: 1, text: 'Furnished' },
        //    { value: 2, text: 'Part Furnished' }
        //];

        $scope.furnishings = [
            { value: 'Unfurnished', text: 'Unfurnished' },
            { value: 'Furnished', text: 'Furnished' },
            { value: 'Part Furnished', text: 'Part Furnished' }
        ];

        $scope.showFurnishings = function () {

            var selected = $filter('filter')($scope.furnishings, { value: vm.property.furnishing }, true);

            return (vm.property.furnishing && selected.length) ? selected[0].text : 'Not set';
        };

        $scope.updateProperty = function () {
            return $http.put('/api/properties/' + vm.property.propertyId, vm.property);
        };


        /* Feature logic
         * =============
         */
        $scope.saveFeature = function (data) {

            var feature = {
                featureId: 0,      /* the DB will assign the id*/
                propertyId: vm.property.propertyId,
                description: data.description
            };

            return $http.post('/api/features', feature);
        };

        $scope.removeFeature = function (index) {
            var featureId = vm.property.features[index].featureId;

            vm.property.features.splice(index, 1);

            // If the user added then immediately deleted (before saving) then the feature isnn't in the DB.
            if (featureId) {
                $http.delete('api/features/' + featureId);
            }
        };

        $scope.addFeature = function () {
            $scope.inserted = {
                featureId: 0,       /* The final value will be set by the DB store operation*/
                description: '',
                propertyId: vm.property.propertyId
            };
            vm.property.features.push($scope.inserted);
        };
    }


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
