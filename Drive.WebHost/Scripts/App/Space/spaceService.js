(function () {
    'use strict';

    angular
        .module('driveApp')
        .factory('SpaceService', SpaceService);

    SpaceService.$inject = ['$http'];

    function SpaceService($http) {
        var service = {
            getSpace: getSpace,
            getAllSpaces: getAllSpaces
        };

        function getSpace(id, callback) {
            $http.get('/api/spaces/' + id)
                .then(function (response) {
                    if (callback) {
                        callback(response.data);
                    }
                }, function () {
                });
        }

        function getAllSpaces(callback) {
            $http.get('/api/spaces')
            .then(function (response) {
                if (callback) {
                    callback(response.data);
                }
            }, function () {
                console.log('Error while getting all spaces!');
            });
        }
        return service;
    }

    var app = angular.module('driveApp');

    app.filter('typeOfFile', function () {
        return function (input, uppercase) {
            switch (input) {
                case 0:
                    return 'None';
                case 1:
                    return 'Document';
                case 2:
                    return 'Archive';
                case 3:
                    return 'Presentation';
                case 4:
                    return 'WebPage';
                default:
                    return '';
            }
        }
    });
})();