(function () {
    'use strict';

    angular
        .module('driveApp')
        .factory('MenuService', MenuService);

    MenuService.$inject = ['$http', 'BaseUrl'];

    function MenuService($http, baseUrl) {
        var service = {
            getAllSpaces: getAllSpaces
        };

        function getAllSpaces(callBack) {
            $http.get(baseUrl + '/api/spaces')
                .success(function(response) {
                    if (callBack)
                        callBack(response);
                });
        }

        return service;
    }
})();