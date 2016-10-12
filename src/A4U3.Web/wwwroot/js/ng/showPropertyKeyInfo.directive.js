(function() {
    'use strict';

    angular
        .module('a4u2app')
        .directive('showPropertyKeyInfo', showPropertyKeyInfo);

    showPropertyKeyInfo.$inject = ['$window'];
    
    function showPropertyKeyInfo($window) {
        // Usage:
        //     <div showPropertyKeyInfo property="property"> </div>
        // Creates:
        // 
        var directive = {
            link: link,
            restrict: 'A',
            scope: {
                property: '='
            },
            templateUrl: 'templates/showPropertyKeyInfo.html'
        };
        return directive;

        function link(scope, element, attrs) {
        }
    }

})();

