(function () {
    'use strict';

    angular
        .module('driveApp')
        .factory('RoleService', RoleService);

    RoleService.$inject = ['$http', 'BaseUrl', 'toastr'];

    function RoleService($http, baseUrl, toastr) {
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

        function createRole(data, callback) {
            $http.post(baseUrl + '/api/roles', data)
                .then(function (response) {
                    if (response.data == -1) {
                        toastr.warning(
                            'A role with same name already exists!', 'Admin panel',
                  {
                      closeButton: true, timeOut: 5000
                  });
                    }
                    else {
                        if (callback) {
                            callback(response.data);
                        }
                    }
                }, function (response) {
                    console.log('Error while pushing changes!');
                });
        }

        function saveRole(data) {
            $http.put(baseUrl + '/api/roles', data)
            .then(function (response) {
                if (response.data == false) {
                    toastr.warning(
                          'A role with same name already exists!', 'Admin panel',
                 {
                     closeButton: true, timeOut: 5000
                 });
                }
            }, function (response) {
                console.log('Error while saving changes!');
            });
        }

        return service;
    }
})();