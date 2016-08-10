(function () {
    'use strict';

    angular
        .module('driveApp')
        .factory('FoldersService', FoldersService);

    FoldersService.$inject = ['$http'];

    function FoldersService($http) {

        var service = {
            getAll: getAll,
            get: get,
            create: create
        };

        

        function getAll(callBack) {
            $http.get('api/folders').success(function (response) {
                if (callBack)
                    callBack(response);
            });
        }

        function get(id, callBack) {
            $http.get('api/folders/' + 1).success(function (response) {
                if (callBack)
                    callBack(response);
            });
        }

        function create(data, callBack) {
            $http.post('api/folders', data).success(function (response) {
                if (callBack)
                    callBack(response);
            });
        }

        return service;
    };
})();