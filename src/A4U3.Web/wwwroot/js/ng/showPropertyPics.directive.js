(function() {
    'use strict';

    angular
        .module('a4u2app')
        .directive('showPropertyPics', showPropertyPics);

    showPropertyPics.$inject = ['$window'];
    
    function showPropertyPics ($window) {
        // Usage:
        //     <div showPropertyPics property="property"> </div>
        // Creates:
        // 
        var directive = {
            link: link,
            restrict: 'A',
            scope: {
                property: '='
            },
            templateUrl: 'templates/showPropertyPictures.html'
        };
        return directive;

        function link(scope, element, attrs) {
        }
    }

})();

