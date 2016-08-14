(function () {
    'use strict';

    angular
        .module('driveApp')
        .factory('CreateService', CreateService);

    CreateService.$inject = ['$http'];

    function CreateService($http) {
        var service = {
            getSpace: getSpace,
            getAllUsers: getAllUsers,
            pushData: pushData
        };

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

        function pushData(data) {
            $http.put('/api/spaces', data)
                .then(function () {
                    console.log('Success!');
                }, function () {
                    console.log('Error while pushing data!');
                });
        }

        return service;
    }
})();