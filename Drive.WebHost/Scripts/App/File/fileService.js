/// <reference path="fileService.js" />
(function() {
    'use strict';

    angular
        .module('driveApp')
        .factory('FileService', FileService);

    FileService.$inject = ['$http', 'BaseUrl'];

    function FileService($http, baseUrl) {

        var service = {
            createFile: createFile,
            updateFile: updateFile,
            deleteFile: deleteFile,
            getFile: getFile,
            getAllFiles: getAllFiles,
            getFilesApp: getFilesApp,
            getAllByParentId: getAllByParentId,
            orderByColumn: orderByColumn,
            openFile: openFile
        };

        function getAllFiles(callBack) {
            $http.get(baseUrl + '/api/files')
                .success(function(response) {
                    if (callBack) {
                        callBack(response);
                    }
                });
        }

        function getAllByParentId(spaceId, parentId, callBack) {
            $http.get(baseUrl + '/api/files/parent?spaceId=' + spaceId + '&parentId=' + parentId)
                .then(function (response) {
                    if (callBack) {
                        callBack(response.data);
                    }
                },
                    function () {
                        console.log('Error while getting files!');
                    });
        }

        function getFilesApp(fileType, callBack) {
            $http.get(baseUrl + '/api/files/apps/' + fileType)
                .then(function (response) {
                    if (callBack) {
                        callBack(response.data);
                    }
                })
        }

        function getFile(id, callBack) {
            $http.get(baseUrl + '/api/files/' + id)
                .then(function(response) {
                        if (callBack) {
                            callBack(response.data);
                        }
                    },
                    function() {
                        console.log('Error while getting file!');
                    });
        }

        function createFile(file, callBack) {
            $http.post(baseUrl + '/api/files', file)
                .then(function(response) {
                        if (callBack) {
                            callBack(response.data);
                        }
                    },
                    function() {
                        console.log('Error while getting file!');
                    });
        }

        function updateFile(id, file, callBack) {
            $http.put(baseUrl + '/api/files/' + id, file)
                .then(function(response) {
                        if (callBack) {
                            callBack(response.data);
                        }
                    },
                    function() {
                        console.log('Error while getting file!');
                    });
        }

        function deleteFile(id, callBack) {
            $http.delete(baseUrl + '/api/files/' + id)
                .then(function(response) {
                        if (callBack) {
                            callBack(response.data);
                        }
                    },
                    function() {
                        console.log('Error while getting file!');
                    });
        }

        function orderByColumn(columnClicked, columnCurrent) {
            var pos = columnCurrent.indexOf(columnClicked);
            if (pos == 0) { return '-' + columnClicked; }
            return columnClicked;
        }

        function openFile(url) {
            window.open(url, '_blank');
        }

        return service;
    }
})();