(function () {
    'use strict';

    angular
        .module('driveApp')
        .factory('FoldersService', FoldersService);

    FoldersService.$inject = ['$http'];

    function FoldersService($http) {

        var service = {
            getFolders: getFolders
        };

        function getFolders(callBack) {
            $http.get('api/folders').success(function (response) {
                console.log(response);
                if (callBack)
                    callBack(response);
            });
        }

        return service;
    };
})();