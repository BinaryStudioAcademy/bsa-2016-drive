(function () {
    'use strict';

    angular
        .module('driveApp')
        .factory('FolderService', FolderService);

    FolderService.$inject = ['$http'];

    function FolderService($http) {
        var service = {
            getAll: getAll,
            get: get,
            create: create,
            updateFolder: updateFolder,
            deleteFolder : deleteFolder
        };

        function getAll(callBack) {
            $http.get('api/folders').success(function (response) {
                if (callBack)
                    callBack(response);
            });
        }

        function get(id, callBack) {
            $http.get('api/folders/' + id).success(function (response) {
                if (callBack)
                    callBack(response);
            });
        }

        function create(data, callBack) {
            $http.post('api/folders', data).success(function (response) {
                if (callBack)
                    callBack(response);
            });
        }

        function updateFolder(data, callback) {
            $http.put('api/folders/' + data.id, data).success(function (response) {
                    if (callback) {
                        callback(response);
                    }
                });
        }

        function deleteFolder(id, callback) {
            $http.delete('api/folders/'+ id)
                .then(function (response) {
                    if (callback)
                        callback(response);
                });
        }
        return service;
    }
})();