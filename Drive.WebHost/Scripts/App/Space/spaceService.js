(function () {
    'use strict';

    angular
        .module('driveApp')
        .factory('SpaceService', SpaceService);

    SpaceService.$inject = ['$http'];

    function SpaceService($http) {
        var service = {
            getSpace: getSpace,
            getAllSpaces: getAllSpaces,

            getSpaceCreate: getSpaceCreate,
            getAllUsers: getAllUsers,
            pushData: pushData,
            pushUsers: pushUsers,

            searchFoldersAndFiles,
            getNumberOfResultSearchFoldersAndFiles,
            getSpaceTotal
        };

        function getSpace(id, currentPage, pageSize, callback) {
            $http.get('/api/spaces/' + id, {
                params: {
                    page: currentPage,
                    count: pageSize
                }
            })
                .then(function (response) {
                    if (callback) {
                        callback(response.data);
                    }
                }, function () {
                    console.log('Error getSpace spaceService!');
                });
        }

        function getSpaceTotal(id, callback) {
            $http.get('/api/spaces/' + id + '/sptotal')
               .then(function (response) {
                   if (callback) {
                       callback(response.data);
                   }
               }, function () {
                   console.log('Error getSpaceTotal spaceService!');
               });

        }

        function getAllSpaces(callback) {
            $http.get('/api/spaces')
            .then(function (response) {
                if (callback) {
                    callback(response.data);
                }
            }, function () {
                console.log('Error while getting all spaces!');
            });
        }

        function getSpaceCreate(id, callback) {
            $http.get('/api/spaces/' + id)
                .then(function (response) {
                    if (callback) {
                        callback(response.data);
                    }
                }, function () {
                    console.log('Error while getting space!');
                });
        }

        function getAllUsers(callback) {
            $http.get('/api/users')
            .then(function (response) {
                if (callback) {
                    callback(response.data);
                }
            }, function () {
                console.log('Error while getting all users!');
            });
        }

        function pushData(data) {
            $http.post('/api/spaces', data)
                .then(function () {
                    console.log('Success!');
                }, function () {
                    console.log('Error while pushing data!');
                });
        }

        function pushUsers(data, callback) {
            $http.put('/api/spaces/' + data.id, data)
                .then(function () {
                    if (callback)
                        callback();
                }, function () {
                    console.log('Error while pushing new users!');
                });
        }

        function searchFoldersAndFiles(spaceId, folderId, text, currentPage, pageSize, callback) {
            $http.get('/api/spaces/' + spaceId + '/search', {
                params: {
                    folderId: folderId,
                    text: text,
                    page: currentPage,
                    count: pageSize
                }
            })
            .then(function (response) {
                if (callback) {
                    callback(response.data);
                }
            }, function () {
                console.log('Error in searchFoldersAndFiles Method!');
            });
        }

        function getNumberOfResultSearchFoldersAndFiles(spaceId, folderId, text, callback) {
            $http.get('/api/spaces/' + spaceId + '/total', {
                params: {
                    folderId: folderId,
                    text: text
                }
            })
            .then(function (response) {
                if (callback) {
                    callback(response.data);
                }
            }, function () {
                console.log('Error in getNumberOfResultSearchFoldersAndFiles Method!');
            });
        }

        return service;
    }

})();