(function () {
    'use strict';
    angular.module('driveApp').directive('fileDropzone', ['$parse', "FileService", function ($parse, fileService) {
        return {
            restrict: 'A',
            scope: {
                dropFile: '=',
                maxSize: "=",
                valid: "="
            },
            link: function (scope, element, attrs) {
                var processDragOverOrEnter, queue;
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
                    var files = event.dataTransfer.files;
                    for (var i = 0; i < files.length; i++) {
                        (function (file) {
                            var reader = new FileReader();
                            reader.onload = function (dropEvent) {
                                scope.$apply(function () {
                                    file.filename = fileService.getFileName(file.name);
                                    file.extension = fileService.getFileExtension(file.name);
                                    file.isValid = fileService.checkFileSize(file.size, scope.maxSize);
                                    
                                });
                            }
                            reader.readAsArrayBuffer(file);
                        })(files[i]);
                    }
                });
            }
        };
    }])
    .directive("fileread", ["$parse", "FileService", function ($parse, fileService) {
        return {
            restrict: "A",
            scope: {
                inputFile: "=",
                maxSize: "=",
                valid: "="
            },
            link: function (scope, element, attributes) {
                element.bind("change", function (changeEvent) {
                    var files = event.target.files;
                    for (var i = 0; i < files.length; i++) {
                        (function (file) {
                            var reader = new FileReader();
                            reader.onload = function (loadEvent) {
                                scope.$apply(function () {
                                    file.filename = fileService.getFileName(file.name);
                                    file.extension = fileService.getFileExtension(file.name);
                                    file.isValid = fileService.checkFileSize(file.size, scope.maxSize);
                                    scope.inputFile.push(file);
                                });
                            }
                            reader.readAsArrayBuffer(file);
                        })(files[i]);
                    }
                });
            }
        };
    }])
    .directive('repeatDone', ['$parse', 'FileService', function ($parse, fileService) {
        return {
            scope: {
                model: "=",
                valid: "="
            },
            link: function (scope, element, attrs) {
                scope.valid = fileService.checkFilesValidationProperty(scope.model);
            }
        }
    }])
}).call(this);