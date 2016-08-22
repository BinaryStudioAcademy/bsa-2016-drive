(function () {
    'use strict';

    angular
        .module('driveApp')
        .factory('SettingsService', SettingsService);

    SettingsService.$inject = ['$http'];

    function SettingsService($http) {
        var service = {
            getSpace: getSpace,
            getAllUsers: getAllUsers,
            pushChanges: pushChanges,
            getAllRoles: getAllRoles
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

        function pushChanges(data, callback) {
            $http.put('/api/spaces/' + data.id, data)
                .then(function (response) {
                    if (callback)
                        callback();
                }, function (response) {
                    console.log('Error while pushing changes!');
                });
        }

        return service;
    }
})();