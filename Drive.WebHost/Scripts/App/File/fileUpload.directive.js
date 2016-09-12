(function () {
    'use strict';
    angular.module('driveApp').directive('fileDropzone', ['$parse', function ($parse) {
        return {
            restrict: 'A',
            scope: {
                dropFile: '=',
            },
            link: function (scope, element, attrs) {
                var processDragOverOrEnter;
                processDragOverOrEnter = function (event) {
                    if (event != null) {
                        event.preventDefault();
                    }
                    event.dataTransfer.effectAllowed = 'copy';
                    return false;
                };
                element.bind('dragover', processDragOverOrEnter);
                element.bind('dragenter', processDragOverOrEnter);
                return element.bind('drop', function (event) {
                    if (event != null) {
                        event.preventDefault();
                    }
                    if (scope.dropFile != null) {
                        scope.$apply(function () {
                            scope.dropFile = null;
                        });
                    }
                    var reader = new FileReader();
                    reader.onload = function (evt) {
                        scope.$apply(function () {
                            scope.dropFile = event.dataTransfer.files[0];
                        })
                    };
                    reader.readAsArrayBuffer(event.dataTransfer.files[0]);
                    return false;
                });
            }
        };
    }])
    .directive("fileread", ["$parse", function ($parse) {
        return {
            restrict: "A",
            scope: {
                inputFile: "="
            },
            link: function (scope, element, attributes) {
                element.bind("change", function (changeEvent) {

                    if (scope.inputFile != null) {
                        scope.$apply(function () {
                            scope.inputFile = null;
                        });
                    }
                    var reader = new FileReader();

                    reader.onload = function (loadEvent) {
                        scope.$apply(function () {
                            scope.inputFile = changeEvent.target.files[0];
                        });
                    }
                    reader.readAsArrayBuffer(changeEvent.target.files[0]);
                });
            }
        };
    }])
}).call(this);