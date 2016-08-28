﻿(function () {
    'use strict';

    angular
        .module('driveApp')
        .factory('RoleService', RoleService);

    RoleService.$inject = ['$http', 'BaseUrl'];

    function RoleService($http, baseUrl) {
        var service = {
            getAllSpaces: getAllSpaces,
            getAllUsers: getAllUsers,
            getAllRoles: getAllRoles,
            createRole: createRole,
            getById: getById,
            saveRole: saveRole
        };

        function getAllSpaces(callback) {
            $http.get(baseUrl + '/api/spaces')
                .then(function (response) {
                    if (callback) {
                        callback(response.data);
                    }
                }, function () {
                    console.log('Error while getting spaces!');
                });
        }

        function getAllUsers(callback) {
            $http.get(baseUrl + '/api/users')
            .then(function (response) {
                if (callback) {
                    callback(response.data);
                }
            }, function () {
                console.log('Error while getting users!');
            });
        }

        function getAllRoles(callback) {
            $http.get(baseUrl + '/api/roles')
            .then(function (response) {
                if (callback) {
                    callback(response.data);
                }
            }, function () {
                console.log('Error while getting roles!');
            });
        }

        function getById(id, callback) {
            $http.get(baseUrl + '/api/roles/' + id)
            .then(function (response) {
                if (callback) {
                    callback(response.data);
                }
            }, function () {
                console.log('Error while getting role by ID!');
            });
        }

        function createRole(data) {
            $http.post(baseUrl + '/api/roles', data)
                .then(function (response) {
                    console.log('Success!');
                }, function (response) {
                    console.log('Error while pushing changes!');
                });
        }

        function saveRole(data) {
            $http.put(baseUrl + '/api/roles', data)
            .then(function (response) {
                console.log('Success!');
            }, function (response) {
                console.log('Error while saving changes!');
            });
        }

        return service;
    }
})();