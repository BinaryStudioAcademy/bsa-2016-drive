(function () {
    'use strict';

    angular
        .module('driveApp')
        .factory('FolderService', FolderService);

    FolderService.$inject = ['$http'];

    function FolderService($http) {
        var service = {
            getFolders: getFolders,
            updateFolder: updateFolder,
            deleteFolder : deleteFolder
        };

        function getFolders(callback) {
            return $http.get('api/folders').then(function (response) {
                if (callback)
                    callback(response.data);
            });
        }

        function updateFolder(id, folder, callback) {
            $http.put('api/folders/'+ id)
                .then(function(response) {
                    if (callback)
                        callback(response);
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