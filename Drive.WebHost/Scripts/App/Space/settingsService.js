(function () {
    'use strict';

    angular
        .module('driveApp')
        .factory('SettingsService', SettingsService);

    SettingsService.$inject = ['$http', 'BaseUrl'];

    function SettingsService($http, baseUrl) {
        var service = {
            getSpace: getSpace,
            getAllUsers: getAllUsers,
            pushChanges: pushChanges,
            getAllRoles: getAllRoles,
            deleteSpace: deleteSpace,
            deleteSpaceWithStaff: deleteSpaceWithStaff
        };

        function getAllRoles(callback) {
            $http.get(baseUrl + '/api/roles')
            .then(function (response) {
                if (callback) {
                    callback(response.data);
                }
            }, function () {
                console.log('Error while getting roles!');
            })
        }

        function getSpace(id, callback) {
            $http.get(baseUrl + '/api/spaces/' + id + '/settings')
                .then(function (response) {
                    if (callback) {
                        callback(response.data);
                    }
                }, function () {
                    console.log('Error while getting space!');
                });
        }

        function getAllUsers(callback) {
            $http.get(baseUrl + '/api/users')
            .then(function (response) {
                if (callback) {
                    callback(response.data);
                }
            }, function () {
                console.log('Error while getting all users!');
            });
        }

        function pushChanges(data, callback) {
            $http.put(baseUrl + '/api/spaces/' + data.id, data)
                .then(function (response) {
                    if (callback)
                        callback();
                }, function (response) {
                    console.log('Error while pushing changes!');
                });
        }

        function deleteSpace(id, callback) {
            $http.delete(baseUrl + '/api/spaces/' + id)
                .then(function (response) {
                    if (callback)
                        callback(response);
                });
        }

        function deleteSpaceWithStaff(id, callback) {
            $http.delete(baseUrl + '/api/spaces/' + id + '/staff')
                .then(function (response) {
                    if (callback)
                        callback(response);
                });
        }

        return service;
    }
})();