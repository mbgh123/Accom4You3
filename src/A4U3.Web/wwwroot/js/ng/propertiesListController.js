(function () {
    'use strict';

    angular
        .module('a4u2app')
        .controller('propertiesListController', propertiesListController);

    propertiesListController.$inject = ['$http', '$scope'];

    function propertiesListController($http, $scope) {
        /* jshint validthis:true */

        _fancyBox();

        // Make function availble to ng-click
        $scope.showMap = function (event) {
            // Prevent ng view refreshing
            event.preventDefault();  

            // Could put the code here, but want to demonstrate a modular approach
            a4u2.showMap(event);
        }

        // filtering functions for ng-repeat
        $scope.greaterThanFilter = function (rate) {
            return function (item) {
                if (rate == null) {
                    return true;        // rate not set
                }
                else {
                    return item.ratePCM < rate;
                }
            }
        }

        $scope.bedroomFilter = function (bedrooms) {
            return function (item) {
                if (!bedrooms) {
                    return true;        // bedrooms not set
                }
                else {
                    if (bedrooms == 1) {
                        return item.bedrooms == bedrooms;
                    } else {
                        return item.bedrooms >= bedrooms;
                    }
                }
            }
        }

        // VM
        var vm = this;
        vm.properties = [];
        vm.errorMessage = null;
        vm.isBusy = true;

        $http.get("/api/properties")
            .then(function (response) {
                    // Success function
                    var result = response.data;  // the json from server is parsed to an object (and array of objects in this case)

                    angular.copy(result, vm.properties);

            },
                function (error) {
                    // Failure function
                    vm.errorMessage = "Failed to load data: " + error;

                })
            .finally(function () {
                vm.isBusy = false;
            });

    }

    function _fancyBox() {
        
        // Attach fancybox to our pictures
        $("ul.pictures a").fancybox({
            prevEffect: 'none',
            nextEffect: 'none'
        });
    }

})();
