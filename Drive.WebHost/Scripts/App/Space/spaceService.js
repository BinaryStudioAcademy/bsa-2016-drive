﻿(function () {
    'use strict';

    angular
        .module('driveApp')
        .factory('SpaceService', SpaceService);

    SpaceService.$inject = ['$http'];

    function SpaceService($http) {
        var service = {
            getSpace: getSpace,
            getAllSpaces: getAllSpaces,
            getAllUsers: getAllUsers,
            pushData: pushData,
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
                },
                function errorCallback(response) {
                    console.log('Error getSpace spaceService!' + response.status);
                    if (response.status == 404 && callback) {
                        console.log(response.status + ' ' + response.data);
                        callback(response.data)
                    }
                });
        }

        function getSpaceTotal(id, callback) {
            $http.get('/api/spaces/' + id + '/sptotal')
               .then(function (response) {
                   if (callback) {
                       callback(response.data);
                   }
               },
               function errorCallback(response) {
                   console.log('Error getSpaceTotal spaceService!' + response.status);
                   if (response.status == 404 && callback) {
                       console.log(response.status + ' ' + response.data);
                       callback(response.data)
                   }
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
            }, function errorCallback(response) {
                console.log('Error in searchFoldersAndFiles Method! Code:' + response.status);
                if (response.status == 404 && callback) {
                    callback(response.data)
                }
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
            }, function errorCallback(response) {
                console.log('Error in getNumberOfResultSearchFoldersAndFiles Method! Code:' + response.status);
                if (response.status == 404 && callback) {
                    callback(response.data)
                }
            });
        }

        return service;
    }
})();