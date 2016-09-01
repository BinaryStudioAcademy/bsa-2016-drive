(function () {
    'use strict';

    angular
        .module('driveApp')
        .factory('TrashBinService', TrashBinService);

    TrashBinService.$inject = ['$http', 'BaseUrl'];

    function TrashBinService($http, baseUrl) {
        var service = {
            getTrashBinContent: getTrashBinContent,
            deleteFilePermanently: deleteFilePermanently,
            restoreFile: restoreFile
        };

        function getTrashBinContent(callBack) {
            $http.get(baseUrl + '/api/trashbin')
                .then(function (response) {
                    if (callBack) {
                        callBack(response.data);
                    }
                });
        }

        function deleteFilePermanently(id, callBack) {
            $http.delete(baseUrl + '/api/trashbin/' + id)
            .then(function (response) {
                if (callBack) {
                    callBack(response.data);
                }
            });
        }

        function restoreFile(id, callBack) {
            $http.put(baseUrl + '/api/trashbin/' + id)
            .then(function (response) {
                if (callBack) {
                    callBack(response.data);
                }
            });
        }

        return service;
    }
})();