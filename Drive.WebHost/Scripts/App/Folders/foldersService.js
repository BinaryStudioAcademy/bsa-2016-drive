(function () {
    'use strict';

    angular
        .module('driveApp')
        .factory('FoldersService', FoldersService);

    FoldersService.$inject = ['$http'];

    function FoldersService($http) {

        var service = {
            getAll: getAll,
            get: get
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

        return service;
    };
})();