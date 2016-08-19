(function () {
    'use strict';

    angular
        .module('driveApp')
        .factory('RoleService', RoleService);

    RoleService.$inject = ['$http'];

    function RoleService($http) {
        var service = {
            getAllSpaces: getAllSpaces,
            getAllUsers: getAllUsers,
            getAllRoles: getAllRoles,
            createRole: createRole,
            getById: getById,
            saveRole: saveRole
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

        function getById(id, callback) {
            $http.get('/api/roles/' + id)
            .then(function (response) {
                if (callback) {
                    callback(response.data);
                }
            }, function () {
                console.log('Error while getting role by ID!');
            });
        }

        function createRole(data) {
            $http.post('/api/roles', data)
                .then(function (response) {
                    console.log('Success!');
                }, function (response) {
                    console.log('Error while pushing changes!');
                });
        }

        function saveRole(data) {
            $http.put('/api/roles', data)
            .then(function (response) {
                console.log('Success!');
            }, function (response) {
                console.log('Error while saving changes!');
            });
        }

        return service;
    }
})();