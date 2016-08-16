(function () {
    'use strict';

    angular
        .module('driveApp')
        .factory('AdminPanelService', AdminPanelService);

    AdminPanelService.$inject = ['$http'];

    function AdminPanelService($http) {
        var service = {
            getAllSpaces: getAllSpaces,
            getAllUsers: getAllUsers,
            getAllRoles: getAllRoles
        };

        function getAllSpaces(callback) {
            $http.get('/api/spaces')
                .then(function (response) {
                    if (callback) {
                        callback(response.data);
                    }
                }, function () {
                    console.log('Error while getting spaces!');
                });
        }

        function getAllUsers(callback) {
            $http.get('/api/users')
            .then(function (response) {
                if (callback) {
                    callback(response.data);
                }
            }, function () {
                console.log('Error while getting users!');
            });
        }

        function getAllRoles(callback) {
            $http.get('/api/roles')
            .then(function (response) {
                if (callback) {
                    callback(response.data);
                }
            }, function () {
                console.log('Error while getting roles!');
            });
        }

        return service;
    }
})();