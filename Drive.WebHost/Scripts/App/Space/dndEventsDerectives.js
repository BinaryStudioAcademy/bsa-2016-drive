(function () {
    "use strict";
    angular.module('driveApp')
    .directive('ngRightClick', function ($parse) {
        return function (scope, element, attrs) {
            var fn = $parse(attrs.ngRightClick);
            element.bind('contextmenu', function (event) {
                scope.$apply(function () {
                    event.preventDefault();
                    fn(scope, { $event: event });
                });
            });
        };
    });
})();

(function () {
    "use strict";
    angular.module('driveApp')
    .directive('ngDragStartEndListen', function () {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                element[0].addEventListener('dragstart', scope.handleDragStart, false);
                element[0].addEventListener('dragend', scope.handleDragEnd, false);
            }
        }
    });
})();