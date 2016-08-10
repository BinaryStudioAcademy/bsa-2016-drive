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

        function create(callBack) {
            $http.post('api/folders', {isDeleted: false, name: 'from js', description: ''}).success(function (response) {
                if (callBack)
                    callBack(response);
            });
        }

        return service;
    };
})();