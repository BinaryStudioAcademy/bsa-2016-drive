(function() {
    'use strict';

    angular
        .module('driveApp')
        .factory('FileService', FileService);

    FileService.$inject = ['$http'];

    function FileService($http) {

        var service = {
            createFile: createFile,
            updateFile: updateFile,
            deleteFile: deleteFile,
            getFile: getFile,
            getAllFiles: getAllFiles
        };

        function getAllFiles(callBack) {
            $http.get('api/files')
                .success(function(response) {
                    if (callBack) {
                        callBack(response);
                    }
                });
        }

        function getFile(id, callBack) {
            $http.get('api/files/' + id)
                .success(function(response) {
                    if (callBack) {
                        callBack(response);
                    }
                });
        }

        function createFile(file, callBack) {
            $http.post('api/files', file)
                .success(function(response) {
                    if (callBack) {
                        callBack(response);
                    }
                });
        }

        function updateFile(id, file, callBack) {
            $http.put('api/files/' + id, file)
                .success(function(response) {
                    if (callBack) {
                        callBack(response);
                    }
                });
        }

        function deleteFile(id, callBack) {
            $http.delete('api/files/' + id)
                .success(function(response) {
                    if (callBack) {
                        callBack(response);
                    }
                });
        }

        return service;
    }
})();