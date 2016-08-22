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
            pushChanges: pushChanges
        };

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

        return service;
    }
})();