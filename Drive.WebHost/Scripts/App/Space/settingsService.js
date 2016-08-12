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
            pushChanges: pushChanges
        };

        function getSpace(id, callback) {
            $http.get('/api/spaces/' + id)
                .then(function (response) {
                    if (callback) {
                        callback(response.data);
                    }
                }, function () {
                    alert('Error while getting space!');
                });
        }

        function getAllUsers(callback) {
            $http.get('/api/users')
            .then(function (response) {
                if (callback) {
                    callback(response.data);
                }
            }, function () {
                alert('Error while getting all users!');
            });
        }

        function pushChanges(data) {
            $http.put('/api/spaces', data)
                .then(function (response) {
                    alert('Success!');
                }, function (response) {
                    alert('Error while pushing changes!');
                });
        }

        return service;
    }
})();