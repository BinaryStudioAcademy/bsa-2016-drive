(function () {
    'use strict';

    angular
        .module('driveApp')
        .factory('MenuService', MenuService);

    MenuService.$inject = ['$http'];

    function MenuService($http) {
        var service = {
            getAllSpaces: getAllSpaces
        };

        function getAllSpaces(callBack) {
            $http.get('api/spaces/').success(function (response) {
                if (callBack)
                    callBack(response);
            });
        }

        return service;
    }
})();