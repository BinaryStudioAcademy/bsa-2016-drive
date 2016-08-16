(function() {
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
            deleteFolder: deleteFolder
        };

        function getAll(callBack) {
            $http.get('api/folders')
                .then(function(response) {
                        if (callBack) {
                            callBack(response.data);
                        }
                    },
                    function() {
                        console.log('Error while getting folder!');
                    });
        }

        function get(id, callBack) {
            $http.get('api/folders/' + id)
                .then(function(response) {
                        if (callBack) {
                            callBack(response.data);
                        }
                    },
                    function() {
                        console.log('Error while getting folder!');
                    });
        }

        function create(data, callBack) {
            $http.post('api/folders', data)
                .then(function(response) {
                        if (callBack) {
                            callBack(response.data);
                        }
                    },
                    function() {
                        console.log('Error while getting folder!');
                    });
        }

        function updateFolder(data, callback) {
            $http.put('api/folders/' + data.id, data)
                .then(function(response) {
                        if (callback) {
                            callback(response.data);
                        }
                    },
                    function() {
                        console.log('Error while getting folder!');
                    });
        }

        function deleteFolder(id, callback) {
            $http.delete('api/folders/' + id)
                .then(function(response) {
                    if (callback)
                        callback(response);
                });
        }

        return service;
    }
})();