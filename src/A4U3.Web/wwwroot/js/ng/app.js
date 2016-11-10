(function () {
    'use strict';

    angular.module('a4u2app', [
        // Angular modules 
        'ngRoute',

        // Custom modules 

        // 3rd Party Modules
        'xeditable'
    ])
    .config(function ($routeProvider) {

        $routeProvider.when("/", {
            controller: "propertiesListController",
            controllerAs: "vm",
            templateUrl: "/views/propertiesListView.html"
        });

        $routeProvider.when("/details/:propertyId", {
            controller: "propertyDetailsController",
            controllerAs: "vm",
            templateUrl: "/views/propertyDetailsView.html"
        });

        $routeProvider.when("/detailscms/:propertyId", {
            controller: "propertyDetailsControllerCms",
            controllerAs: "vm",
            templateUrl: "/views/propertyDetailsViewCms.html"
        });

        $routeProvider.otherwise({ redirectTo: "/" });
    });

})();