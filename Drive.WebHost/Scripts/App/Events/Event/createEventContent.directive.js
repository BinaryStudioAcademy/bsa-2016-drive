(function () {
    "use strict";

    angular.module('driveApp.events')
        .directive('createEventContent', CreateEventContent);

    CreateEventContent.$inject = ['BaseUrl'];

    function CreateEventContent(baseUrl) {
        var directive = {
            restrict: 'E',
            templateUrl: baseUrl + '/Scripts/App/Events/Event/createEventContent.directive.html',
            replace: true,
            scope: {
                contentsModel: '=',
                contentList: '=',
                afterSaveMethod: '='
            },
            link: content
        }

        return directive;

        function content(scope, element, attrs) {
            scope.$watch('contentsModel', function (newVal, oldVal) {
                if (newVal) {
                    init();
                }
            });

            function init() {
                scope.content = scope.contentsModel;
                scope.urlIsValid = true;
                if (scope.contentsModel !== undefined) {
                    scope.contentModel = scope.contentsModel;
                }
            }

            
            scope.saveContent = function () {
                scope.content = scope.contentModel;
                if (scope.content.order === undefined) {
                    scope.contentList.push(scope.content);
                }
                else {
                    scope.contentList[scope.content.order] = scope.content;
                }

                scope.afterSaveMethod();
                scope.contentModel = { contentType: scope.contentsModel.contentType, isCollapsed: true };
            };

            scope.isValidEventUrl= function() {
                if (scope.contentModel.contentType === 1) {
                    scope.urlIsValid = true;
                    return;
                }

                var reg = new RegExp("^https?://");

                if (!reg.test(scope.contentModel.content)) {
                    scope.contentModel.content = "http://" + scope.contentModel.content;
                }

                var expression = "^(?:(?:ht|f)tps?://)?(?:[\\-\\w]+@)?(?:[\\-0-9a-z]*[0-9a-z]\\.)+[a-z]{2,6}(?::\\d{1,5})?(?:[?/\\\\#][?!^$.(){}:|=[\\]+\\-/\\\\*;&~#@,%\\wА-Яа-я]*)?$";
                reg = new RegExp(expression);
                scope.urlIsValid = reg.test(scope.contentModel.content);
            }
        }
    }
})();