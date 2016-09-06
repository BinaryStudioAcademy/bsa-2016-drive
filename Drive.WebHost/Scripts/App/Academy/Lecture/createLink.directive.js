(function () {
    "use strict";

    angular.module('driveApp.academyPro')
        .directive('createLinks', CreateLinks);

    CreateLinks.$inject = ['BaseUrl'];

    function CreateLinks(baseUrl) {
        var directive = {
            restrict: 'E',
            templateUrl: baseUrl + '/Scripts/App/Academy/Lecture/createLink.directive.html',
            replace: true,
            scope: {
                linksModel: '=',
                linksName: '@',
                imageClass: '@'
            },
            link: link
        }

        return directive;

        function link(scope, element, attrs) {
            var unwatch = scope.$watch('linksModel', function (newVal, oldVal) {
                if (newVal) {
                    init();
                    unwatch();
                }
            });

            function init() {
                scope.internalLinks = scope.linksModel;
            }

            scope.linkModel = {};
            scope.saveLink = function() {
                scope.internalLinks.push(scope.linkModel);
                scope.linkModel = {};
            };
        }
    }
})();