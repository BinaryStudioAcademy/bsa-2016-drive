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
<<<<<<< HEAD
            getAllRoles: getAllRoles,
            deleteSpace: deleteSpace
=======
            deleteSpace: deleteSpace,
            deleteSpaceWithStaff: deleteSpaceWithStaff
>>>>>>> refs/remotes/origin/develop
        };

        function getAllRoles(callback) {
            $http.get('/api/roles')
            .then(function (response) {
                if (callback) {
                    callback(response.data);
                }
            }, function () {
                console.log('Error while getting roles!');
            })
        }


        function getSpace(id, callback) {
            $http.get(baseUrl + '/api/spaces/' + id)
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