(function () {
    'use strict';

    angular.module('a4u2app', [
        // Angular modules 
        'ngRoute'

        // Custom modules 

        // 3rd Party Modules
        
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

        $routeProvider.when("/editor/:propertyId", {
            controller: "propertyEditorController",
            controllerAs: "vm",
            templateUrl: "/views/propertyEditorView.html"
        });

        $routeProvider.otherwise({ redirectTo: "/" });
    });

})();