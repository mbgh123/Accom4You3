(function() {
    'use strict';

    angular
        .module('a4u2app')
        .directive('showPropertyFeatures', showPropertyFeatures);

    showPropertyFeatures.$inject = ['$window'];
    
    function showPropertyFeatures($window) {
        // Usage:
        //     <div showPropertyFeatures property="property"> </div>
        // Creates:
        // 
        var directive = {
            link: link,
            restrict: 'A',
            scope: {
                property: '='
            },
            templateUrl: 'templates/showPropertyFeatures.html'
        };
        return directive;

        function link(scope, element, attrs) {
        }
    }

})();

