(function () {
    'use strict';

    angular
        .module('driveApp')
        .factory('FileService', FileService);

    FileService.$inject = ['$http'];

    function FileService($http) {

        var service = {
            createFile: createFile,
            updateFile: updateFile,
            removeFile: removeFile
        };


        function createFile(file, callBack) {
            $http.post('/ApiUrl', file)
            .then(function (response) {
                if (callBack) {
                    callBack();
                }
            });
        };

        function updateFile(file, callBack) {
            $http.put('ApiUrl/' + file.id, callBack)
            .then(function (response) {
                if (callBack) {
                    callBack();
                }
            })
        }

        function removeFile(id, callBack) {
            $http.delete('/url/' + id)
            .then(function (response) {
                if (callBack) {
                    callBack();
                }
            });
        }

        return service;
    }
})();